using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.JSConfusers
{
	/// <summary>
	/// 難読化
	/// </summary>
	public class JSConfuser
	{
		private List<string> JSLines;

		/// <summary>
		/// 難読化を実行する。
		/// </summary>
		/// <param name="lines">ソースコード</param>
		/// <returns>難読化したソースコード</returns>
		public List<string> Confuse(List<string> lines)
		{
			this.JSLines = lines;

			this.Confuse_Main();

			lines = this.JSLines;
			this.JSLines = null;
			return lines;
		}

		/// <summary>
		/// 難読化メイン
		/// </summary>
		private void Confuse_Main()
		{
			this.RemoveComments_EscapeLiteralStrings();
			this.SolveLiteralStrings();
			this.RenameEx();
		}

		private void RemoveComments_EscapeLiteralStrings()
		{
			string text = SCommon.LinesToText(this.JSLines);
			StringBuilder dest = new StringBuilder();

			for (int index = 0; index < text.Length; )
			{
				// ? C系コメントの開始
				if (text[index] == '/' && text[index + 1] == '*')
				{
					index += 2;

					for (; ; )
					{
						// ? C系コメントの終了
						if (text[index] == '*' && text[index + 1] == '/')
							break;

						index++;
					}
					index += 2;
					continue;
				}
				// ? C++系コメントの開始
				if (text[index] == '/' && text[index + 1] == '/')
				{
					index += 2;

					for (; ; )
					{
						// ? C++系コメントの終了
						if (text[index] == '\n')
							break;

						index++;
					}
					//index++; // 改行は出力する。
					continue;
				}
				// ? 文字列の開始
				if (text[index] == '\'' || text[index] == '"')
				{
					char bracket = text[index];
					StringBuilder buff = new StringBuilder();

					index++;

					for (; ; )
					{
						char chr = text[index];

						// ? 文字列の終了
						if (chr == bracket)
							break;

						if (chr == '\\')
						{
							index++;
							chr = text[index];

							if (chr == 'b')
							{
								chr = '\b';
							}
							else if (chr == 't')
							{
								chr = '\t';
							}
							else if (chr == 'v')
							{
								chr = '\v';
							}
							else if (chr == 'r')
							{
								chr = '\r';
							}
							else if (chr == 'f')
							{
								chr = '\f';
							}
							else if (chr == '\'')
							{
								chr = '\'';
							}
							else if (chr == '"')
							{
								chr = '"';
							}
							else if (chr == '`')
							{
								chr = '`';
							}
							else if (chr == '\\')
							{
								chr = '\\';
							}
							else if (chr == '0')
							{
								chr = '\0';
							}
							else if (chr == 'x')
							{
								throw new Exception("非対応：2桁の16進数が表すLatin-1文字");
							}
							else if (chr == 'u')
							{
								throw new Exception("非対応：4桁の16進数が表すUnicode文字");
							}
							else
							{
								throw new Exception("不明な文字列エスケープ");
							}
						}
						buff.Append(chr);
						index++;
					}
					string str = buff.ToString();
					dest.Append('"');

					foreach (char chr in str)
					{
						dest.Append("\\u");
						dest.Append(((int)chr).ToString("x4"));
					}
					dest.Append('"');
					index++;
					continue;
				}
				dest.Append(text[index]);
				index++;
			}
			this.JSLines = SCommon.TextToLines(dest.ToString()).ToList();
		}

		private void SolveLiteralStrings()
		{
			// TODO
		}

		private void RenameEx()
		{
			string text = SCommon.LinesToText(this.JSLines);

			text += " "; // 番兵設置

			Dictionary<string, string> wordFilter = SCommon.CreateDictionary<string>();

			foreach (string word in JSResource.予約語リスト)
				wordFilter.Add(word, word);

			for (int index = 0; index < text.Length; )
			{
				// ? 文字列の開始
				if (text[index] == '"')
				{
					index++;

					for (; ; )
					{
						// ? 文字列の終了
						if (text[index] == '"')
							break;

						index++;
					}
					index++;
					continue;
				}
				// ? 単語の開始
				if (JSCommon.IsJSWordChar(text[index]))
				{
					int end = index + 1;

					for (; ; )
					{
						// ? 単語の終了
						if (!JSCommon.IsJSWordChar(text[end]))
							break;

						end++;
					}
					string word = text.Substring(index, end - index);

					// 数字で始まる場合は単語ではなく定数 -> 置き換えしない。
					if (SCommon.DECIMAL.Contains(word[0]))
					{
						index = end;
					}
					// ? 小文字で始まるメソッド名 -> 置き換えしない。
					else if (SCommon.alpha.Contains(word[0]) && 1 <= index && text[index - 1] == '.')
					{
						index = end;
					}
					else if (wordFilter.ContainsKey(word))
					{
						string destWord = wordFilter[word];

						if (word == destWord) // ? 予約語である。
						{
							// (予約語).(後続のワード).(後続のワード).(後続のワード) ... の「後続のワード」も置き換え禁止とする。
							// Math.PI など
							// 但し this は除外する。

							if (word != "this")
							{
								for (; ; )
								{
									// ? 連続する後続のワードの終了
									if (text[end] != '.' && !JSCommon.IsJSWordChar(text[end]))
										break;

									end++;
								}
							}
							index = end;
						}
						else // ? 予約語ではない。既知の置き換え
						{
							text = text.Substring(0, index) + destWord + text.Substring(end);
							index += destWord.Length;
						}
					}
					else // ? 未知の置き換え
					{
						string destWord = JSCommon.CreateNewIdent();

						wordFilter.Add(word, destWord);

						text = text.Substring(0, index) + destWord + text.Substring(end);
						index += destWord.Length;
					}
					continue;
				}
				index++;
			}
			this.JSLines = SCommon.TextToLines(text).ToList();
		}
	}
}
