using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Charlotte.Commons;
using Charlotte.Utilities;

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
			this.ShuffleFunctions();
			this.FormatSource();
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
								index++;
								char chr_01 = text[index];
								index++;
								char chr_02 = text[index];

								chr = Common.HexCharsToUnicodeChar(chr_01, chr_02);
							}
							else if (chr == 'u')
							{
								index++;
								char chr_01 = text[index];
								index++;
								char chr_02 = text[index];
								index++;
								char chr_03 = text[index];
								index++;
								char chr_04 = text[index];

								chr = Common.HexCharsToUnicodeChar(chr_01, chr_02, chr_03, chr_04);
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

		/// <summary>
		/// リテラル文字列を判読しにくくする。
		/// </summary>
		private void SolveLiteralStrings()
		{
			List<string> extendLines = new List<string>();

			for (int index = 0; index < this.JSLines.Count; index++)
			{
				string line = this.JSLines[index];

				for (; ; )
				{
					int c = line.IndexOf('"');

					if (c == -1)
						break;

					int c2 = line.IndexOf('"', c + 1);

					if (c2 == -1)
						throw new Exception("文字列が閉じられていない。");

					c2++;
					string varName = Common.CreateRandIdent();
					string extendSource = @"

var $varName;

function $varName_01()
{
	if (!$varName)
	{
		$varName = $varName_GetString();
	}
	return $varName;
}

function $varName_GetString()
{
	var $varName_Str = """";
    
	for (var $varName_StrPart of $varName_E_GetStrParts())
	{
		$varName_Str += $varName_StrPart;
	}
	return $varName_Str;
}

function* $varName_E_GetStrParts()
{
	for (var $varName_Chr of $varName_E_GetChars())
	{
		if ($varName_Chr % $_65537_ != 0)
		{
			yield String.fromCodePoint($varName_Chr % $_65537_ - 1);
		}
	}
}

function* $varName_E_GetChars()
{

$callChrListFuncs

}

$chrListFuncs

";

					int[][] ranges = SLS_GetRanges(new int[] { c + 1, (c2 - c) - 2 });

					extendSource = extendSource.Replace(
						"$callChrListFuncs",
						string.Join("\r\n", SLS_GetCallCharListFuncs(varName, ranges.Length))
						);
					extendSource = extendSource.Replace(
						"$chrListFuncs",
						string.Join("\r\n", SLS_GetCharListFuncs(varName, line, ranges))
						);
					extendSource = Common.Replace(extendSource,
						"$_65537_",
						() => SLS_GetOperandOf65537()
						);
					extendSource = extendSource.Replace("$varName", varName);
					extendLines.AddRange(
						SCommon.TextToLines(extendSource).Where(v => v != "")
						);

					line = line.Substring(0, c) + varName + "_01()" + line.Substring(c2);
				}
				this.JSLines[index] = line;
			}
			this.JSLines.AddRange(extendLines);
		}

		private string SLS_GetOperandOf65537()
		{
			int a = SCommon.CRandom.GetRange(300, 399);
			int b = SCommon.CRandom.GetRange(300, 399);
			int c = a * b - 65537;

			return "(" + a + " * " + b + " - " + c + ")";
		}

		private int[][] SLS_GetRanges(int[] baseRange)
		{
			List<int[]> ranges = new List<int[]>();

			ranges.Add(baseRange);

			for (int c = SCommon.CRandom.GetRange(1, 3); 0 < c; c--)
			{
				int index = SCommon.CRandom.GetInt(ranges.Count);
				int subLength = SCommon.CRandom.GetInt(ranges[index][1] / 6 + 1) * 6; // 注意：6文字で実際の1文字を表す。

				ranges.Insert(index + 1, new int[] { ranges[index][0] + subLength, ranges[index][1] - subLength });
				ranges[index][1] = subLength;
			}
			return ranges.ToArray();
		}

		private static IEnumerable<string> SLS_GetCallCharListFuncs(string varName, int count)
		{
			for (int index = 0; index < count; index++)
			{
				yield return "\tyield* " + varName + "_E_GetChars_" + index + "_Z();";
			}
		}

		private static IEnumerable<string> SLS_GetCharListFuncs(string varName, string sourceLine, int[][] ranges)
		{
			for (int index = 0; index < ranges.Length; index++)
			{
				int[] range = ranges[index];

				yield return "function* " + varName + "_E_GetChars_" + index + "_Z()";
				yield return "{";

				foreach (var relay in SLS_E_GetCharList(sourceLine.Substring(range[0], range[1])))
					yield return relay;

				yield return "}";
			}
		}

		private static IEnumerable<string> SLS_E_GetCharList(string source)
		{
			// 注意：6文字で実際の1文字を表す。

			if (source.Length % 6 != 0)
				throw null;

			for (int c = SCommon.CRandom.GetRange(3, 7); 0 < c; c--)
				yield return SLS_MakeYR(-1);

			for (int index = 0; index * 6 < source.Length; index++)
			{
				if (SCommon.CRandom.GetInt(2) == 0) // ランダムにダミー値を差し込む
					yield return SLS_MakeYR(-1);

				yield return SLS_MakeYR(Convert.ToInt32(source.Substring(index * 6 + 2, 4), 16));
			}
		}

		private static string SLS_MakeYR(int chr)
		{
			// (0x0000 + 1) + 65537 *     0 == 1
			// (0xffff + 1) + 65537 *     0 == 65536
			// (0x0000 + 1) + 65537 *   153 == 10027162
			// (0xffff + 1) + 65537 *   153 == 10092697
			// (0x0000 + 1) + 65537 *  1524 == 99878389
			// (0xffff + 1) + 65537 *  1524 == 99943924
			// (0x0000 + 1) + 65537 * 15259 == 1000029084
			// (0xffff + 1) + 65537 * 15259 == 1000094619
			// (0x0000 + 1) + 65537 * 32766 == 2147385343
			// (0xffff + 1) + 65537 * 32766 == 2147450878 (0x7fff7ffe)

			int value = (chr + 1) + 65537 * SCommon.CRandom.GetRange(153, 1524); // 8桁
			//int value = (chr + 1) + 65537 * SCommon.CRandom.GetRange(15259, 32766); // 10桁
			//int value = (chr + 1) + 65537 * SCommon.CRandom.GetRange(0, 32766); // 1～10桁

			return "\tyield " + value + ";";
		}

		private void RenameEx()
		{
			ProcMain.WriteLog("RenameEx-ST");

			string text = SCommon.LinesToText(this.JSLines);

			text += " "; // 番兵設置

			StringSpliceSequencer sss = new StringSpliceSequencer(text);
			CrossDictionary knownWordPairs = new CrossDictionary();

			foreach (string word in JSResource.予約語リスト)
				knownWordPairs.Add(word, word);

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
					// ? 小文字で始まるメンバー名 -> 置き換えしない。
					else if (SCommon.alpha.Contains(word[0]) && 1 <= index && text[index - 1] == '.')
					{
						index = end;
					}
					else if (knownWordPairs.ContainsKey(word))
					{
						string destWord = knownWordPairs[word];

						if (word == destWord) // ? 予約語である。
						{
							// (予約語).(後続のワード).(後続のワード).(後続のワード) ... の「後続のワード」も置き換え禁止とする。
							// 但し this は除外する。

							// -- MEMO: 小文字で始まるメンバーを除外しているので、後続のワードの除外は不要と思ったが Math.PI, Math.E などがある。

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
							sss.Splice(index, end - index, destWord);
							index = end;
						}
					}
					else // ? 未知の置き換え
					{
						string destWord = JSCommon.CreateNewIdent(v => !knownWordPairs.ContainsValue(v));

						knownWordPairs.Add(word, destWord);

						sss.Splice(index, end - index, destWord);
						index = end;
					}
					continue;
				}
				index++;
			}
			text = sss.GetString();
			this.JSLines = SCommon.TextToLines(text).ToList();
			ProcMain.WriteLog("RenameEx-ED");
		}

		/// <summary>
		/// 関数と初期化子を持たない変数の並び方をめちゃくちゃにする。
		/// </summary>
		private void ShuffleFunctions()
		{
			List<string[]> functions = new List<string[]>();

			for (int index = 0; index + 1 < this.JSLines.Count; )
			{
				string line_01 = this.JSLines[index];
				string line_02 = this.JSLines[index + 1];

				if ((
					line_01.StartsWith("function ") ||
					line_01.StartsWith("function* ")
					) && (
					line_02.StartsWith("{")
					))
				{
					List<string> function = new List<string>();

					function.Add(line_01);
					function.Add(line_02);

					this.JSLines[index++] = "";
					this.JSLines[index++] = "";

					for (; ; )
					{
						string line = this.JSLines[index];

						function.Add(line);
						this.JSLines[index] = "";
						index++;

						if (line.StartsWith("}"))
							break;
					}
					functions.Add(function.ToArray());
					continue;
				}
				index++;
			}
			for (int index = 0; index < this.JSLines.Count; index++)
			{
				string line = this.JSLines[index];

				if (
					line.StartsWith("var ") &&
					!line.Contains('=') // ? 初期化子を持たない。
					)
				{
					functions.Add(new string[] { this.JSLines[index] });
					this.JSLines[index] = "";
				}
			}
			SCommon.CRandom.Shuffle(functions);

			this.JSLines = SCommon.Concat(functions).Concat(this.JSLines).ToList();
		}

		/// <summary>
		/// ソースコードの整形
		/// </summary>
		private void FormatSource()
		{
			// 行単位の整形
			//
			this.JSLines = this.JSLines.Select(line =>
			{
				if (line.Trim() == "")
				{
					line = "";
				}
				else
				{
					for (int c = 0; c < 20; c++)
					{
						line = line.Replace("\u0020\u0020", "\u0020"); // SP-SP -> SP
					}
					line = line.TrimEnd("\t ".ToArray());
				}
				return line;
			})
			.ToList();

			int dmyCmtNo = 0;

			// ダミーコメントの追加
			//
			for (int index = 0; index < this.JSLines.Count; index++)
			{
				string line = this.JSLines[index];

				if (
					line.StartsWith("var ") ||
					line.StartsWith("function ") ||
					line.StartsWith("function* ")
					)
				{
					this.JSLines.Insert(index++, "");
#if true
					this.JSLines.Insert(index++, "// " + (++dmyCmtNo));
#elif true
					this.JSLines.Insert(index++, "// " + SCommon.CRandom.GetInt(100).ToString("D2"));
#else
					this.JSLines.Insert(index++, "/*");
					this.JSLines.Insert(index++, "\tConfused by JSJoin");
					this.JSLines.Insert(index++, "*/");
#endif
				}
			}

			// 連続する空行を１つの空行にする。
			//
			for (int index = 1; index < this.JSLines.Count; index++)
			{
				string line_01 = this.JSLines[index - 1];
				string line_02 = this.JSLines[index];

				if (line_01 == "" && line_02 == "")
				{
					this.JSLines.RemoveAt(index);
					index--;
				}
			}

			// インデント有りの行に挟まれた空行を除去する。
			//
			for (int index = 2; index < this.JSLines.Count; index++)
			{
				string line_01 = this.JSLines[index - 2];
				string line_02 = this.JSLines[index - 1];
				string line_03 = this.JSLines[index];

				if (
					line_01.StartsWith("\t") &&
					line_02 == "" &&
					line_03.StartsWith("\t")
					)
				{
					this.JSLines.RemoveAt(index - 1);
					index--;
				}
			}

			// for ( ... などの前には空行を入れる。
			//
			for (int index = 1; index < this.JSLines.Count; index++)
			{
				string line_01 = this.JSLines[index - 1];
				string line_02 = this.JSLines[index];

				if (
					!Regex.IsMatch(line_01, "^[\t]*\\{") &&
					Regex.IsMatch(line_02, "^[\t]+[a-z]+ \\(") // ex. "\tfor (; ; )"
					)
				{
					this.JSLines.Insert(index, "");
					index++;
				}
			}

			// ]; }; の後には空行を入れる。
			//
			for (int index = 0; index < this.JSLines.Count; index++)
			{
				string trLine = this.JSLines[index].Trim();

				if (trLine == "];" || trLine == "};")
				{
					this.JSLines.Insert(index + 1, "");
					index++;
				}
			}

			// 連続する空行を除去する。
			//
			for (int index = 1; index < this.JSLines.Count; index++)
			{
				if (this.JSLines[index] == "" && this.JSLines[index - 1] == "")
				{
					this.JSLines.RemoveAt(index);
					index--;
				}
			}

			// 先頭と終端の空行を除去する。
			{
				while (1 <= this.JSLines.Count && this.JSLines[0] == "")
					this.JSLines.RemoveAt(0);

				while (1 <= this.JSLines.Count && this.JSLines[this.JSLines.Count - 1] == "")
					this.JSLines.RemoveAt(this.JSLines.Count - 1);
			}
		}
	}
}
