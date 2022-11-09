using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.JSSources
{
	public class JSSourceFile
	{
		public string OriginalFilePath;
		public string FilePath;

		public JSSourceFile(string filePath, WorkingDir wd)
		{
			this.OriginalFilePath = filePath;
			this.FilePath = wd.MakePath();

			File.Copy(this.OriginalFilePath, this.FilePath);
		}

		public void RemoveComments()
		{
			char[] text = File.ReadAllText(this.FilePath, JSConsts.SOURCE_FILE_ENCODING).ToArray();

			for (int index = 0; index < text.Length; )
			{
				char chr = text[index];

				if (chr == '"') // ? リテラル文字列(ダブルクォート)
				{
					index = this.SkipLiteralString(text, index, '"');
					continue;
				}
				if (chr == '\'') // ? リテラル文字列(シングルクォート)
				{
					index = this.SkipLiteralString(text, index, '\'');
					continue;
				}
				if (chr == '/')
				{
					index++;
					chr = text[index];

					if (chr == '*') // ? C系コメント開始
					{
						int p;

						for (p = index + 1; ; p++)
							if (text[p] == '*' && text[p + 1] == '/') // ? C系コメント終了
								break;

						this.EraseComment(text, index - 1, p + 2);
						index = p + 2;
						continue;
					}
					if (chr == '/') // ? C++系コメント開始
					{
						int p;

						for (p = index + 1; p < text.Length; p++)
							if (text[p] == '\n') // ? C++系コメント終了
								break;

						this.EraseComment(text, index - 1, p);
						index = p;
						continue;
					}
				}
				index++;
			}
			File.WriteAllText(this.FilePath, new string(text), JSConsts.SOURCE_FILE_ENCODING);
		}

		private int SkipLiteralString(char[] text, int index, char literalStringEndChr)
		{
			int p;

			for (p = index + 1; ; p++)
			{
				if (text[p] == literalStringEndChr)
					break;

				// ここでは \xff \uffff \u{ffff} を考慮しなくて良い。
				//
				if (text[p] == '\\') // ? エスケープ文字 -> スキップ
					p++;
			}
			return p + 1;
		}

		private void EraseComment(char[] text, int start, int end)
		{
			for (int index = start; index < end; index++)
				if (text[index] > ' ') // ? 空白系文字ではない。
					text[index] = ' ';
		}

		public void SolveLiteralStrings()
		{
			string text = File.ReadAllText(this.FilePath, JSConsts.SOURCE_FILE_ENCODING);
			StringBuilder dest = new StringBuilder();
			int literalStringEndChr = -1; // リテラル文字列「開始・終了」文字, -1 == リテラル文字列の外

			for (int index = 0; index < text.Length; index++)
			{
				char chr = text[index];

				if (literalStringEndChr == -1)
				{
					if (chr == '"') // ? リテラル文字列開始(ダブルクォート)
					{
						literalStringEndChr = '"';
						dest.Append('"');
					}
					else if (chr == '\'') // ? リテラル文字列開始(シングルクォート)
					{
						literalStringEndChr = '\'';
						dest.Append('"');
					}
					else
					{
						dest.Append(chr);
					}
				}
				else
				{
					if (chr == literalStringEndChr)
					{
						literalStringEndChr = -1;
						dest.Append('"');
					}
					else
					{
						if (chr == '\\')
						{
							index++;
							chr = text[index];

							if (chr == '0')
							{
								chr = '\0';
							}
							else if (chr == 'b')
							{
								chr = '\b';
							}
							else if (chr == 't')
							{
								chr = '\t';
							}
							else if (chr == 'n')
							{
								chr = '\n';
							}
							else if (chr == 'v')
							{
								chr = '\v';
							}
							else if (chr == 'f')
							{
								chr = '\f';
							}
							else if (chr == 'r')
							{
								chr = '\r';
							}
							else if (chr == '"')
							{
								// noop
							}
							else if (chr == '\'')
							{
								// noop
							}
							else if (chr == '\\')
							{
								// noop
							}
							else if (chr == '`')
							{
								// noop
							}
							else if (chr == 'x') // \xff
							{
								chr = (char)Convert.ToUInt16(new string(new char[]
								{
									text[index + 1],
									text[index + 2],
								}));

								index += 2;
							}
							else if (chr == 'u' && text[index + 1] == '{') // \u{ffff}
							{
								throw new Exception("対応していないエスケープシーケンス -- \\u{ffff}");
							}
							else if (chr == 'u') // \uffff
							{
								chr = (char)Convert.ToUInt16(new string(new char[]
								{
									text[index + 1],
									text[index + 2],
									text[index + 3],
									text[index + 4],
								}));

								index += 4;
							}
							else
							{
								throw new Exception("不明なエスケープシーケンス");
							}
						}
						dest.Append(string.Format("\\u{0:x4}", (int)chr));
					}
				}
			}
			File.WriteAllText(this.FilePath, dest.ToString(), JSConsts.SOURCE_FILE_ENCODING);
		}

		public List<JSContent> Contents = new List<JSContent>();

		public void CollectContents()
		{
			string[] lines = File.ReadAllLines(this.FilePath, JSConsts.SOURCE_FILE_ENCODING);

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];

				// クラス・関数・変数
				if (
					Regex.IsMatch(line, "^public\\s") ||
					Regex.IsMatch(line, "^private\\s")
					)
				{
					if (index + 1 < lines.Length && Regex.IsMatch(lines[index + 1], "^[\\{\\[](\\s.*)?$")) // ? 次行がブロック・マップ・配列の開始か
					{
						int p;

						for (p = index + 2; ; p++)
							if (Regex.IsMatch(lines[p], "^[\\}\\]](\\s.*)?$")) // ? ブロック・マップ・配列の終了か
								break;

						this.Contents.Add(new JSContent()
						{
							SourceFile = this,
							LineNumb = index + 1,
							Lines = Common.GetBetween(lines, index, p + 1).ToArray(),
						});

						index = p;
					}
					else
					{
						this.Contents.Add(new JSContent()
						{
							SourceFile = this,
							LineNumb = index + 1,
							Lines = new string[] { lines[index] },
						});
					}
				}
				else if (line.Trim() == "") // 空行
				{
					// noop
				}
				else // 空行以外
				{
					throw new Exception("作用の無い行を検出しました。行番号：" + (index + 1));
				}
			}
		}
	}
}
