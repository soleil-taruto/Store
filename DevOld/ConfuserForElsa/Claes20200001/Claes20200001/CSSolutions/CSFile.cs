using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.CSSolutions
{
	public class CSFile
	{
		private string _file;

		public CSFile(string file)
		{
			_file = file;
		}

		public string GetFile()
		{
			return _file;
		}

		public long GetFileSize()
		{
			return new FileInfo(_file).Length;
		}

		/// <summary>
		/// クラス名または構造体名を取得する。
		/// １ファイル１クラス(構造体)を想定する。
		/// ジェネリック型の場合、型名が付いたままであることに注意すること。
		/// </summary>
		/// <returns>クラス名または構造体名</returns>
		public string GetClassOrStructName()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			string[] START_PTNS = new string[]
			{
				// フォーム系
				"\tpublic partial class ",
				"\tpartial class ",

				// その他 Elsa-系の開発で使っているクラスと構造体
				"\tpublic abstract class ",
				"\tpublic static class ",
				"\tpublic struct ",
				"\tpublic class ",
			};

			foreach (string line in lines)
			{
				foreach (string startPtn in START_PTNS)
				{
					if (line.StartsWith(startPtn))
					{
						string name = line.Substring(startPtn.Length);

						// 名前の後に空白・コメント・継承元がある場合は除去
						name = SCommon.Tokenize(name, "\t /:", false, false)[0];

						if (name == "")
							throw new Exception("Bad name");

						return name;
					}
				}
			}
			throw new Exception("クラス名または構造体名を見つけられませんでした。");
		}

		public void SolveNamespace()
		{
			// クラス配置の平滑化
			// ROOT_NAMESPACE 配下のクラスを ROOT_NAMESPACE 直下に置く。
			// クラス名の重複は想定しない。

			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			for (int index = 0; index < lines.Length; index++)
			{
				if (lines[index].StartsWith("using " + CSConsts.MY_PROJECT_ROOT_NAMESPACE + "."))
					lines[index] = "";
				else if (lines[index].StartsWith("namespace " + CSConsts.MY_PROJECT_ROOT_NAMESPACE + "."))
					lines[index] = "namespace " + CSConsts.MY_PROJECT_ROOT_NAMESPACE;
			}
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		public void RemoveComments()
		{
			char[] chrs = File.ReadAllText(_file, Encoding.UTF8).ToArray();

			for (int index = 0; index < chrs.Length; index++)
			{
				// ? プリプロセッサ
				if (chrs[index] == '#')
				{
					// 事故防止のためこの行のこの文字以降については危ない文字を消しておく。
					// コメントを削除する必要はあるので、index は進めない。

					for (int c = index + 1; chrs[c] != '\n'; c++)
					{
						if (
							chrs[c] == '\'' ||
							chrs[c] == '"'
							)
							chrs[c] = '_';
					}
				}
				// ? リテラル文字
				else if (chrs[index] == '\'')
				{
					index++;

					if (chrs[index] == '\\')
					{
						index++;

						if (chrs[index] == 'u')
							index += 4;
					}
					index++;

					if (chrs[index] != '\'')
						throw null; // 想定外
				}
				// ? (改行可能な)リテラル文字列
				else if (
					chrs[index + 0] == '@' &&
					chrs[index + 1] == '"'
					)
				{
					index = RC_Skip(chrs, index + 2, '"', '"', '"', '"', '"');
				}
				// ? リテラル文字列
				else if (chrs[index] == '"')
				{
					index = RC_Skip(chrs, index + 1, '\\', '\\', '\\', '"', '"');
				}
				// ? C系コメント
				else if (
					chrs[index + 0] == '/' &&
					chrs[index + 1] == '*'
					)
				{
					chrs[index + 0] = ' '; // '/' 除去
					chrs[index + 1] = ' '; // '*' 除去

					index = RC_Mask(chrs, index + 2, '*', '/');

					chrs[index + 0] = ' '; // '*' 除去
					chrs[index + 1] = ' '; // '/' 除去

					index++;
				}
				// ? C++系コメント
				else if (
					chrs[index + 0] == '/' &&
					chrs[index + 1] == '/'
					)
				{
					if (CSCommon.IsMatch(chrs, index, CSSpecialStrings.KEEP_COMMENT_START_PATTERN.ToArray()))
					{
						// C++系コメントの後に改行が必ずあると想定する。

						index = RC_Skip_EC2(chrs, index + 2, '\r', '\n') + 1;
					}
					else
					{
						chrs[index + 0] = ' '; // '/' 除去
						chrs[index + 1] = ' '; // '/' 除去

						// C++系コメントの後に改行が必ずあると想定する。

						index = RC_Mask(chrs, index + 2, '\r', '\n') + 1;
					}
				}
			}
			File.WriteAllText(_file, new string(chrs), Encoding.UTF8);

			// コメント除去後に生じうる行末の空白を除去する。
			{
				string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

				lines = lines.Select(v => v.TrimEnd()).ToArray();

				File.WriteAllLines(_file, lines, Encoding.UTF8);
			}
		}

		private static int RC_Skip(char[] chrs, int index, char ignChr_a1, char ignChr_a2, char ignChr_b1, char ignChr_b2, char endChr)
		{
			for (; ; index++)
			{
				while (
					chrs[index + 0] == ignChr_a1 &&
					chrs[index + 1] == ignChr_a2
					||
					chrs[index + 0] == ignChr_b1 &&
					chrs[index + 1] == ignChr_b2
					)
					index += 2;

				if (chrs[index] == endChr)
					break;
			}
			return index; // endChr の位置
		}

		private static int RC_Mask(char[] chrs, int index, char endChr_1, char endChr_2)
		{
			while (
				chrs[index + 0] != endChr_1 ||
				chrs[index + 1] != endChr_2
				)
				chrs[index++] = ' ';

			return index; // endChr_1 の位置
		}

		private static int RC_Skip_EC2(char[] chrs, int index, char endChr_1, char endChr_2)
		{
			while (
				chrs[index + 0] != endChr_1 ||
				chrs[index + 1] != endChr_2
				)
				index++;

			return index; // endChr_1 の位置
		}

		public void RemovePreprocessorDirectives()
		{
			// #if の入れ子に対応していない。
			// #if, #elif の条件式は true, !true, false, !false, DEBUG, !DEBUG のみ想定する。
			// -- DEBUG は false として扱う。

			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];

				if (
					line == "#if true" ||
					line == "#if !false" ||
					line == "#if !DEBUG"
					)
				{
					lines[index] = "";
					index = RPD_RemoveIfTrue(lines, index + 1);
				}
				else if (
					line == "#if !true" ||
					line == "#if false" ||
					line == "#if DEBUG"
					)
				{
					lines[index] = "";
					index = RPD_RemoveIfFalse(lines, index + 1);
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					throw new Exception("Bad #...");
				}
				else if (line.TrimStart().StartsWith("#")) // ? #region-系
				{
					lines[index] = "";
				}
			}
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		private int RPD_RemoveIfFalse(string[] lines, int index)
		{
			for (; ; index++)
			{
				string line = lines[index];

				if (
					line == "#else" ||
					line == "#elif true" ||
					line == "#elif !false" ||
					line == "#elif !DEBUG"
					)
				{
					lines[index] = "";
					index = RPD_RemoveIfTrue(lines, index + 1);
					break;
				}
				else if (
					line == "#elif !true" ||
					line == "#elif false" ||
					line == "#elif DEBUG"
					)
				{
					// このまま続ける。
				}
				else if (line == "#endif")
				{
					lines[index] = "";
					break;
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					throw new Exception("Bad #...");
				}
				lines[index] = "";
			}
			return index;
		}

		private int RPD_RemoveIfTrue(string[] lines, int index)
		{
			for (; ; index++)
			{
				string line = lines[index];

				if (
					line == "#else" ||
					line == "#elif true" ||
					line == "#elif !false" ||
					line == "#elif !true" ||
					line == "#elif false" ||
					line == "#elif DEBUG" ||
					line == "#elif !DEBUG"
					)
				{
					lines[index] = "";
					break;
				}
				else if (line == "#endif")
				{
					lines[index] = "";
					goto endFunc;
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					throw new Exception("Bad #...");
				}
				else if (line.TrimStart().StartsWith("#")) // ? #region-系
				{
					lines[index] = "";
				}
			}
			for (; ; index++)
			{
				string line = lines[index];

				if (line == "#endif")
				{
					lines[index] = "";
					break;
				}
				else if (line.StartsWith("#")) // ? #if-系
				{
					// noop -- 無視する。
				}
				lines[index] = "";
			}
		endFunc:
			return index;
		}

		public void SolveAccessModifiers()
		{
			string text = File.ReadAllText(_file, Encoding.UTF8);

			text = SAM_Replace(text, "{ get; private set; }", ";");
			text = SAM_Replace(text, "{ private get; set; }", ";");
			text = SAM_Replace(text, "{ set; private get; }", ";");
			text = SAM_Replace(text, "{ private set; get; }", ";");

			// const -> static
			{
#if true // string 以外 (int や double など) は const のままにする。
				text = SAM_Replace(text, "private const string", "public static string");
				text = SAM_Replace(text, "protected const string", "public static string");
				text = SAM_Replace(text, "public const string", "public static string");
				text = SAM_Replace(text, "const string", "string");

				text = SAM_Replace(text, "private const", "public const"); // HACK: 継承クラスに同名のメンバが居たらマズくない？
				text = SAM_Replace(text, "protected const", "public const");
				//text = SAM_Replace(text, "public const", "public const"); // 同じ
#else // old
				text = SAM_Replace(text, "private const", "public static");
				text = SAM_Replace(text, "protected const", "public static");
				text = SAM_Replace(text, "public const", "public static");
				text = SAM_Replace(text, "const", "");
#endif
			}

			text = SAM_Replace(text, "readonly", "");
			//text = SAM_Replace(text, "private", "public"); // 継承クラスに同名のメンバが居るとマズい。
			//text = SAM_Replace(text, "protected", "public"); // protected override に対応していない。
			text = SAM_Replace(text, "public static class", "public class");
			text = SAM_Replace(text, "static class Program", "public class Program"); // Program.cs 専用

			// public const -> public static
			{
				text = SAM_PublicConstVarToPublicStaticVar(text);
				text = SAM_Replace(text, "const", ""); // SAM_PublicConstVarToPublicStaticVar による変換により、必要になった。
			}

			File.WriteAllText(_file, text, Encoding.UTF8);
		}

		private string SAM_Replace(string text, string targPtn, string destPtn)
		{
			for (int index = 0; ; )
			{
				int next = text.IndexOf(targPtn, index);

				if (next == -1)
					break;

				if (
					SAM_IsSpaceOrPunct(text[next - 1]) &&
					SAM_IsSpaceOrPunct(text[next + targPtn.Length])
					)
				{
					text = text.Substring(0, next) + destPtn + text.Substring(next + targPtn.Length);
					index = next + destPtn.Length;
				}
				else
				{
					index = next + targPtn.Length;
				}
			}
			return text;
		}

		private static bool SAM_IsSpaceOrPunct(char chr)
		{
			return !CSCommon.IsCSWordChar(chr);
		}

		private static string SAM_PublicConstVarToPublicStaticVar(string text)
		{
			const string START_PTN = "\tpublic const ";

			int index = 0;

			for (; ; )
			{
				int start = text.IndexOf(START_PTN, index);

				if (start == -1)
					break;

				int end = text.IndexOf('=', start + START_PTN.Length);

				if (end == -1)
					break;

				int indentLen = SAM_PCVTPSV_GetIndentLen(text, start + 1);
				string indent = new string(Enumerable.Range(0, indentLen).Select(dummy => '\t').ToArray());

				string midText = text.Substring(start, end - start);
				string midTextNew;

				//Console.WriteLine("* " + midText); // test

				string varType;
				string varName;
				string varName_INITED = SAM_CreateVarName();
				string varName_Value = SAM_CreateVarName();
				string varName_GetValue = SAM_CreateVarName();
				string varName_GetValue_Ret = SAM_CreateVarName();

				{
					string[] tokens = SCommon.Tokenize(midText, " ", false, true);

					if (tokens.Length != 4)
						throw null; // 想定外

					// [0] public
					// [1] const
					varType = tokens[2]; // 型名
					varName = tokens[3]; // フィールド名
				}

				midTextNew =
					"\tpublic static " +
					varType + " " +
					varName + " { get { if (!" +
					varName_INITED + ") { " +
					varName_INITED + " = true; " +
					varName_Value + " = " +
					varName_GetValue + "(); } return " +
					varName_Value + "; }}" +
					CSConsts.CRLF + indent + "public static " +
					varType + " " +
					varName_GetValue + "() { " +
					varType + " " +
					varName_GetValue_Ret + " ";

				text = text.Substring(0, start) + midTextNew + text.Substring(end);

				index = end;
				index++; // Skip '='
				index -= midText.Length;
				index += midTextNew.Length;

				// ----

				// = から ; までの間に、余計な ; は無いと思ったけど
				// 少なくとも public char SemiColon = ';'; がある。
				// ; の直後は改行のはずなので、それも検索パターンに入れて回避する。
				//
				start = text.IndexOf(";" + CSConsts.CRLF, index);

				if (start == -1)
					throw null; // 想定外

				// 初期化値の置き換え
				{
					midText = text.Substring(index, start - index);
					midTextNew = SAM_PCVTPSV_初期化値の置き換え(midText);

					if (midTextNew != null)
					{
						text = text.Substring(0, index) + midTextNew + text.Substring(start);

						start -= midText.Length;
						start += midTextNew.Length;
					}
				}

				end = start + 1;

				midText = text.Substring(start, end - start);
				midTextNew =
					"; return " +
					varName_GetValue_Ret + "; }" +
					CSConsts.CRLF + indent + "public static bool " +
					varName_INITED + ";" +
					CSConsts.CRLF + indent + "public static " +
					varType + " " +
					varName_Value + ";";

				text = text.Substring(0, start) + midTextNew + text.Substring(end);

				index = end;
				index -= midText.Length;
				index += midTextNew.Length;
			}
			return text;
		}

		private static int SAM_PCVTPSV_GetIndentLen(string text, int end)
		{
			int start = end;

			while (0 < start && text[start - 1] == '\t')
				start--;

			return end - start;
		}

		private static string SAM_PCVTPSV_初期化値の置き換え(string initValue) // ret: null == 置き換えナシ
		{
			initValue = initValue.Trim();

			if (Regex.IsMatch(initValue, "^[-]?[0-9]{0,10}$"))
			{
				long tmp = long.Parse(initValue);

				if ((long)int.MinValue <= tmp && tmp <= (long)int.MaxValue)
				{
					// リテラル文字列の難読化を適用させるために、改行が要る。
					// 直前が = なので、最初に空白を入れる。
					//
					return " int.Parse(" + CSConsts.CRLF + "\"" + initValue + "\")";
				}
			}
			// else-にしては駄目
			if (Regex.IsMatch(initValue, "^[-]?[0-9]{0,19}[Ll]?$"))
			{
				string sVal = Regex.Match(initValue, "[-]?[0-9]+").Value;
				long tmp;

				if (long.TryParse(sVal, out tmp))
				{
					if (sVal != "" + tmp) // パースできたなら一致するはず
						throw null; // 想定外

					// リテラル文字列の難読化を適用させるために、改行が要る。
					// 直前が = なので、最初に空白を入れる。
					//
					return " long.Parse(" + CSConsts.CRLF + "\"" + sVal + "\")";
				}
			}
			return null;
		}

		private static string SAM_CreateVarName()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"SAM_a_" +
				SCommon.CRandom.GetULong().ToString("D20") + "_" +
				SCommon.CRandom.GetULong().ToString("D20") +
				"_z";
		}

		public void FormatCloseOrEmptyClass()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			{
				int bracketPairPos = SCommon.IndexOf(lines, v => v == "\t{ }");

				if (bracketPairPos != -1) // ? 空のクラス(閉じているクラス)
				{
					lines = lines
						.Take(bracketPairPos)
						.Concat(new string[] { "\t{", "\t}" })
						.Concat(lines.Skip(bracketPairPos + 1))
						.ToArray();
				}
			}

			int classInsideTopPos = SCommon.IndexOf(lines, v => v.StartsWith("\t\t"));

			if (classInsideTopPos == -1) // ? 空のクラス(中身無し)
			{
				int openBracketPos = SCommon.IndexOf(lines, v => v.StartsWith("\t{"));

				if (openBracketPos == -1)
					throw new Exception("クラスの先頭を見つけられませんでした。");

				lines = lines
					.Take(openBracketPos + 1)
					.Concat(new string[] { "\t\t// 00_CSFile_01_dummyClassInsideTopLine" })
					.Concat(lines.Skip(openBracketPos + 1))
					.ToArray();
			}
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		public void SolveLiteralStrings(string beforeWarpableMemberLine)
		{
			this.SolveLiteralStrings_01();
			this.SolveLiteralStrings_02(beforeWarpableMemberLine);
		}

		private void SolveLiteralStrings_01()
		{
			char[] chrs = File.ReadAllText(_file, Encoding.UTF8).ToArray();
			StringBuilder dest = new StringBuilder();

			for (int index = 0; index < chrs.Length; index++)
			{
				// ? リテラル文字
				if (chrs[index] == '\'')
				{
					char chr;

					index++;

					if (chrs[index] == '\\')
					{
						index++;

						if (chrs[index] == '\\')
						{
							chr = '\\';
						}
						else if (chrs[index] == 't')
						{
							chr = '\t';
						}
						else if (chrs[index] == 'r')
						{
							chr = '\r';
						}
						else if (chrs[index] == 'n')
						{
							chr = '\n';
						}
						else if (
							chrs[index] == 'u' &&
							CSCommon.IsHexadecimal(chrs[index + 1]) &&
							CSCommon.IsHexadecimal(chrs[index + 2]) &&
							CSCommon.IsHexadecimal(chrs[index + 3]) &&
							CSCommon.IsHexadecimal(chrs[index + 4])
							)
						{
							chr = (char)Convert.ToUInt16(new string(new char[]
							{
								chrs[index + 1],
								chrs[index + 2],
								chrs[index + 3],
								chrs[index + 4],
							}),
							16
							);

							index += 4;
						}
						else
						{
							throw null; // 想定外
						}
					}
					else
					{
						chr = chrs[index];
					}
					index++;

					if (chrs[index] != '\'')
						throw null; // 想定外

					dest.Append("((char)" + ((int)chr) + ")");
					//dest.Append("((char)0x" + ((int)chr).ToString("x4") + ")"); // old
				}
				// ? (改行可能な)リテラル文字列
				else if (
					chrs[index + 0] == '@' &&
					chrs[index + 1] == '"'
					)
				{
					dest.Append('"');
					index += 2;

					for (; ; index++)
					{
						char chr = chrs[index];

						if (chr == '"')
						{
							if (chrs[index + 1] != '"')
								break;

							index++;
						}
						dest.Append("\\u");
						dest.Append(((ushort)chr).ToString("x4"));
					}
					dest.Append('"');
				}
				else if (chrs[index] == '"') // ? リテラル文字列
				{
					dest.Append('"');
					index++;

					for (; ; index++)
					{
						char chr = chrs[index];

						if (chr == '"')
							break;

						if (chr == '\\')
						{
							chr = chrs[++index];

							switch (chr)
							{
								case 't': chr = '\t'; break;
								case 'r': chr = '\r'; break;
								case 'n': chr = '\n'; break;
								case 'u':
									{
										char c1 = chrs[++index];
										char c2 = chrs[++index];
										char c3 = chrs[++index];
										char c4 = chrs[++index];

										chr = (char)Convert.ToUInt32(new string(new char[] { c1, c2, c3, c4 }), 16);
									}
									break;

								case '\\': break;
								case '"': break;

								default:
									throw null; // 想定外
							}
						}
						dest.Append("\\u");
						dest.Append(((ushort)chr).ToString("x4"));
					}
					dest.Append('"');
				}
				else
				{
					dest.Append(chrs[index]);
				}
			}
			File.WriteAllText(_file, dest.ToString(), Encoding.UTF8);
		}

		private void SolveLiteralStrings_02(string beforeWarpableMemberLine)
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);
			List<string> varLines = new List<string>();

			varLines.Add("\t\t// 00_CSFile_02_dummyClassInsideTopLine");

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];
				string trLine = line.Trim();

				// 以下のリテラル文字列は変数化できない。
				// -- switch の case
				// -- デフォルト引数
				// -- [DllImport(... とか

				if (trLine.StartsWith("case ")) // ? switch の case
				{
					// noop
				}
				else if (Regex.IsMatch(line, "(private|protected|public).*[(].*[=]")) // ? デフォルト引数
				{
					// noop
				}
				else if (trLine.StartsWith("[")) // ? [DllImport(... とか
				{
					// noop
				}
				else
				{
					for (; ; )
					{
						int c = line.IndexOf('"');

						if (c == -1)
							break;

						int c2 = line.IndexOf('"', c + 1);

						if (c2 == -1)
							throw null; // never

						c2++;
						string varName = SLS2_CreateVarName();

						AddWarpableMemberMark(varLines, beforeWarpableMemberLine, varName + "_String");
						varLines.Add("\t\tpublic static string " +
							varName + "_String;"
							);

						AddWarpableMemberMark(varLines, beforeWarpableMemberLine, varName + "_01");
						varLines.Add("\t\tpublic static string " +
							varName + "_01() { if (" +
							varName + "_String == null) { " +
							varName + "_String = " +
							varName + "_GetString(); } return " +
							varName + "_String; }"
							);

						int[][] ranges = SLS2_GetStringRanges(c + 1, (c2 - c) - 2);

						for (int rangeIndex = 0; rangeIndex < ranges.Length; rangeIndex++)
						{
							AddWarpableMemberMark(varLines, beforeWarpableMemberLine, varName + "_S_GetString_" + rangeIndex + "_Z");
							varLines.Add("\t\tpublic static string " +
								varName + "_S_GetString_" +
								rangeIndex + "_Z() { return new string(" +
								varName + "_E_GetString_" +
								rangeIndex + "_Z().Where(" +
								varName + "_Var => " +
								varName + "_Var % 65537 != 0).Select(" +
								varName + "_Var2 => (char)(" +
								varName + "_Var2 % 65537 - 1)).ToArray()); }"
								);

							AddWarpableMemberMark(varLines, beforeWarpableMemberLine, varName + "_E_GetString_" + rangeIndex + "_Z");
							varLines.Add("\t\tpublic static int[] " +
								varName + "_E_GetString_" +
								rangeIndex + "_Z() { return new int[] { " +
								string.Join(", ", SLS2_ToYR(line.Substring(ranges[rangeIndex][0], ranges[rangeIndex][1]))) + " }; }"
								);
						}

						SLS2_RangeTreeInfo rangeTree = new SLS2_RangeTreeInfo(Enumerable.Range(0, ranges.Length).ToArray());
						SLS2_MakeRangeTree(rangeTree);

						AddWarpableMemberMark(varLines, beforeWarpableMemberLine, varName + "_GetString");
						varLines.Add("\t\tpublic static string " +
							varName + "_GetString() { return " +
							rangeTree.Ident + "(); }"
							);

						Queue<SLS2_RangeTreeInfo> qRangeTree = new Queue<SLS2_RangeTreeInfo>();
						qRangeTree.Enqueue(rangeTree);

						while (1 <= qRangeTree.Count)
						{
							rangeTree = qRangeTree.Dequeue();

							if (rangeTree.Children != null)
							{
								if (rangeTree.RangeIndexes != null)
									throw null; // never

								AddWarpableMemberMark(varLines, beforeWarpableMemberLine, rangeTree.Ident);
								varLines.Add("\t\tpublic static string " +
									rangeTree.Ident + "() { return " +
									string.Join(" + ", rangeTree.Children.Select(v => v.Ident + "()")) + "; }"
									);

								foreach (var v in rangeTree.Children)
									qRangeTree.Enqueue(v);
							}
							else
							{
								if (rangeTree.RangeIndexes == null)
									throw null; // never

								AddWarpableMemberMark(varLines, beforeWarpableMemberLine, rangeTree.Ident);
								varLines.Add("\t\tpublic static string " +
									rangeTree.Ident + "() { return " +
									string.Join(" + ", rangeTree.RangeIndexes.Select(v => varName + "_S_GetString_" + v + "_Z()")) + "; }"
									);
							}
						}
						line = line.Substring(0, c) + varName + "_01()" + line.Substring(c2);
					}
					lines[index] = line;
				}
			}
			File.WriteAllLines(_file, this.SLS2_クラスの先頭に挿入(lines, varLines), Encoding.UTF8);

			// IEnumerable<T>, Where(), Select() を使用するための、追加 using
			{
				this.SLS2_AddUsingLineIfNotExist("using System.Collections.Generic;");
				this.SLS2_AddUsingLineIfNotExist("using System.Linq;");
			}
		}

		private static string SLS2_CreateVarName()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"SLS2_a_" +
				SCommon.CRandom.GetULong().ToString("D20") + "_" +
				SCommon.CRandom.GetULong().ToString("D20") +
				"_z";
		}

		private static int[][] SLS2_GetStringRanges(int start, int length)
		{
			List<int[]> ranges = new List<int[]>();

			ranges.Add(new int[] { start, length });

			for (; ; )
			{
				int index = SLS2_GSR_GetMaxRange(ranges);

				// 注意：実際の1文字を6文字で表現
				//
				if (ranges[index][1] < 600) // 100文字くらい -- rough limit
					break;

				SLS2_GSR_Divide(ranges, index);
			}
			for (int c = SCommon.CRandom.GetRange(1, 3); 0 < c; c--) // rough limit
			{
				SLS2_GSR_Divide(ranges, SCommon.CRandom.GetInt(ranges.Count));
			}
			return ranges.ToArray();
		}

		private static int SLS2_GSR_GetMaxRange(List<int[]> ranges)
		{
			int ret = 0;

			for (int index = 1; index < ranges.Count; index++)
				if (ranges[ret][1] < ranges[index][1])
					ret = index;

			return ret;
		}

		private static void SLS2_GSR_Divide(List<int[]> ranges, int index)
		{
			// 注意：実際の1文字を6文字で表現
			//
			int newLength = SCommon.CRandom.GetInt(ranges[index][1] / 6 + 1) * 6;

			ranges.Insert(index + 1, new int[] { ranges[index][0] + newLength, ranges[index][1] - newLength });
			ranges[index][1] = newLength;
		}

		private static IEnumerable<string> SLS2_ToYR(string code)
		{
			string[] trueYRChrs = SLS2_ToYR_NoDummy(code).ToArray();

			List<bool> truePosList =
				Enumerable.Repeat(true, trueYRChrs.Length).Concat(
				Enumerable.Repeat(false, trueYRChrs.Length / 2)
				).ToList();

			while (truePosList.Count < 7)
				truePosList.Add(false);

			SCommon.CRandom.Shuffle(truePosList);

			{
				int index = 0;

				foreach (bool truePos in truePosList)
					yield return truePos ? trueYRChrs[index++] : SLS2_MakeYR(-1);

				if (index != trueYRChrs.Length) // 2bs
					throw null;
			}
		}

		private static IEnumerable<string> SLS2_ToYR_NoDummy(string code)
		{
			if (code.Length % 6 != 0)
				throw null;

			for (int index = 0; index < code.Length; index += 6)
			{
				if (
					code[index + 0] != '\\' ||
					code[index + 1] != 'u' ||
					!CSCommon.IsHexadecimal(code[index + 2]) ||
					!CSCommon.IsHexadecimal(code[index + 3]) ||
					!CSCommon.IsHexadecimal(code[index + 4]) ||
					!CSCommon.IsHexadecimal(code[index + 5])
					)
					throw null;

				yield return SLS2_MakeYR((int)Convert.ToUInt16(code.Substring(index + 2, 4), 16));
			}
		}

		private static string SLS2_MakeYR(int chr)
		{
			// (0x0000 + 1) + 65537 * 0 == 1
			// (0xffff + 1) + 65537 * 0 == 65536
			// (0x0000 + 1) + 65537 * 15259 == 1000029084
			// (0xffff + 1) + 65537 * 15259 == 1000094619
			// (0x0000 + 1) + 65537 * 32766 == 2147385343
			// (0xffff + 1) + 65537 * 32766 == 2147450878 (0x7fff7ffe)

			int value = (chr + 1) + 65537 * SCommon.CRandom.GetRange(15259, 32766); // 10桁
			//int value = (chr + 1) + 65537 * SCommon.CRandom.GetRange(0, 32766); // 1～10桁

			//string valueName = CSCommon.CreateNewIdent();

			//return "yield return new[] { new { " + valueName + " = " + value + " } }[0]." + valueName + ";";
			//return "yield return " + value + ";";
			return "" + value;
		}

		private class SLS2_RangeTreeInfo
		{
			public string Ident = CSCommon.CreateNewIdent();
			public SLS2_RangeTreeInfo[] Children = null;
			public int[] RangeIndexes;

			public SLS2_RangeTreeInfo(int[] rangeIndexes)
			{
				this.RangeIndexes = rangeIndexes;
			}
		}

		private void SLS2_MakeRangeTree(SLS2_RangeTreeInfo rangeTree)
		{
			Queue<SLS2_RangeTreeInfo> q = new Queue<SLS2_RangeTreeInfo>();

			q.Enqueue(rangeTree);

			while (1 <= q.Count)
			{
				rangeTree = q.Dequeue();

				if ((
					2 <= rangeTree.RangeIndexes.Length // ? 分割可能な最小の個数
					) && (
					5 <= rangeTree.RangeIndexes.Length || // ? 分割しなければならない最小の個数 -- rough limit
					SCommon.CRandom.GetInt(3) == 0 // 33.333 %
					))
				{
					// n個を { 1個,(n-1)個 } ～ { (n-1)個,1個 } に分割する。

					int subLength = SCommon.CRandom.GetRange(1, rangeTree.RangeIndexes.Length - 1);

					SLS2_RangeTreeInfo a = new SLS2_RangeTreeInfo(
						rangeTree.RangeIndexes.Take(subLength).ToArray()
						);
					SLS2_RangeTreeInfo b = new SLS2_RangeTreeInfo(
						rangeTree.RangeIndexes.Skip(subLength).ToArray()
						);

					// ConfuserForElsa-のデバッグ用にメソッド名を変更
					{
						a.Ident += "_MRT_Divided";
						b.Ident += "_MRT_Divided";
					}

					rangeTree.Children = new SLS2_RangeTreeInfo[] { a, b };
					rangeTree.RangeIndexes = null;

					q.Enqueue(a);
					q.Enqueue(b);
				}
			}
		}

		private IEnumerable<string> SLS2_クラスの先頭に挿入(string[] lines, List<string> varLines)
		{
			if (varLines.Count == 0)
				return lines;

			int index = SCommon.IndexOf(lines, v => v.StartsWith("\t\t"));

			if (index == -1)
				throw new Exception("クラスの先頭を見つけられませんでした。");

			return lines.Take(index).Concat(varLines).Concat(lines.Skip(index));
		}

		private void SLS2_AddUsingLineIfNotExist(string targLine)
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			if (!lines.Any(v => v == targLine)) // ? targLine という行は存在しない。
			{
				File.WriteAllLines(_file, new string[] { targLine }.Concat(lines), Encoding.UTF8); // targLine を先頭行に追加
			}
		}

		private void AddWarpableMemberMark(List<string> dest, string beforeWarpableMemberLine, string identifier)
		{
			dest.Add(beforeWarpableMemberLine);
			dest.Add(identifier);
		}

		private class WLS_MemberInfo
		{
			public int LineIndex;
			public CSFile DestCSFile;
		}

		public void WarpLiteralStrings(string beforeWarpableMemberLine, CSFile[] otherCSFiles, string dummyMyProjectRootNamespace)
		{
			if (otherCSFiles.Length == 0)
				throw new Exception("no otherCSFiles");

			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);
			List<WLS_MemberInfo> members = new List<WLS_MemberInfo>();

			for (int index = 0; index < lines.Length; index++)
			{
				if (lines[index] == beforeWarpableMemberLine)
				{
					CSFile otherCSFile_01 = SCommon.CRandom.ChooseOne(otherCSFiles);
					CSFile otherCSFile_02 = SCommon.CRandom.ChooseOne(otherCSFiles);
					CSFile otherCSFile;

					// ファイルサイズが小さい方を選択
					if (otherCSFile_01.GetFileSize() < otherCSFile_02.GetFileSize())
						otherCSFile = otherCSFile_01;
					else
						otherCSFile = otherCSFile_02;

					lines[index++] = "// 00_warped_01"; // beforeWarpableMemberLine だった行

					string identifier = lines[index];
					string identifierNew = dummyMyProjectRootNamespace + "." + otherCSFile.GetClassOrStructName() + "." + identifier;
					// 同じ名前のクラスとメンバーがあった場合、クラスが優先されるように、名前空間付きで呼び出す必要がある。

					lines[index++] = "// 00_warped_02"; // identifier だった行
					//lines[index++] = "// 00_warped_03"; // 後で移動することになるメンバー行

					// for で index++ していることに注意

					members.Add(new WLS_MemberInfo()
					{
						LineIndex = index,
						DestCSFile = otherCSFile,
					});

					for (int ndx = 0; ndx < lines.Length; ndx++)
					{
						// メンバー行は自分自身の宣言部分を含むため、置き換えを行わない。

						if (ndx != index) // ? メンバー行ではない。
						{
							lines[ndx] = lines[ndx].Replace(identifier, identifierNew);
						}
					}
				}
			}
			foreach (WLS_MemberInfo member in members)
			{
				member.DestCSFile.WLS_クラスの先頭に挿入(lines[member.LineIndex]);

				lines[member.LineIndex] = "// 00_warped_03:" + lines[member.LineIndex]; // メンバーだった行
			}
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		private void WLS_クラスの先頭に挿入(string line)
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);
			int index = SCommon.IndexOf(lines, v => v.StartsWith("\t\t"));

			if (index == -1)
				throw new Exception("クラスの先頭を見つけられませんでした。" + _file);

			string[] insertingLines = new string[]
			{
				"\t\t// 00_CSFile_03_dummyClassInsideTopLine",
				line,
			};

			lines = lines.Take(index).Concat(insertingLines).Concat(lines.Skip(index)).ToArray();
			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		public void AddDummyMember()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);
			bool structFlag = ADM_IsStruct(lines);
			string dmText = structFlag ? CSResources.STRUCT_DUMMY_MEMBER : CSResources.CLASS_DUMMY_MEMBER;
			string[] dmLines = SCommon.TextToLines(dmText).Where(v => v != "").ToArray();
			int end = ADM_GetClassEnd(lines);

			if (end == -1)
				throw null; // 想定外

			// ダミーメンバーの生成と挿入
			{
				List<string> dest = new List<string>();

				// ダミーメンバー_01
				{
					int dmCount = (lines.Length / dmLines.Length) / 13 + 1;

					for (int index = 0; index < dmCount; index++)
					{
						string ident = ADM_CreateIdent();

						foreach (string f_line in dmLines)
						{
							string line = f_line;
							line = line.Replace("SSS_", ident + "_");
							dest.Add(line);
						}
					}
				}

				lines = lines.Take(end).Concat(dest).Concat(lines.Skip(end)).ToArray();
			}

			File.WriteAllLines(_file, lines, Encoding.UTF8);
		}

		private static bool ADM_IsStruct(string[] lines)
		{
			return lines.Any(v => v.StartsWith("\tpublic struct "));
		}

		private static int ADM_GetClassEnd(string[] lines)
		{
			return SMO_GetClassEnd(lines);
		}

		private static string ADM_CreateIdent()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"ADM_a_" +
				SCommon.CRandom.GetULong().ToString("D20") + "_" +
				SCommon.CRandom.GetULong().ToString("D20") +
				"_z";
		}

		/// <summary>
		/// 識別子のリネーム
		/// -- 置き換え禁止クラス名は置き換えフィルタによって置き換えられないことを想定する。
		/// </summary>
		/// <param name="filter">置き換えフィルタ</param>
		/// <param name="f_isUnrenameableClassName">置き換え禁止クラス名判定</param>
		public void RenameEx(Func<string, string> filter, Predicate<string> f_isUnrenameableClassName)
		{
			string text = File.ReadAllText(_file, Encoding.UTF8);
			string text_bk = text;
			bool insideOfLiteralChar = false;
			bool insideOfLiteralString = false;
			Dictionary<string, string> escapedLines = SCommon.CreateDictionary<string>();

			text = RX_EscapeNoRenameLines(text, escapedLines);

			// C#の書式上「C#の単語」で終わることは無いはずだが、一応想定する。
			//
			text += " "; // 番兵設置

			StringSpliceSequencer sss = new StringSpliceSequencer(text);

			for (int index = 0; index < text.Length; index++)
			{
				if (text[index] == '\\') // ? エスケープ文字 -> スキップする。
				{
					if (text[index + 1] == 'u') // ? 文字コード(4桁) -- これしかないはず
					{
						index += 5; // for で index++ していることに注意
						continue;
					}
					throw new Exception("不正なエスケープ文字");
				}
				insideOfLiteralChar ^= text[index] == '\'';
				insideOfLiteralString ^= text[index] == '"';

				if (
					!insideOfLiteralChar &&
					!insideOfLiteralString &&
					CSCommon.IsCSWordChar(text[index])
					)
				{
					int end = index + 1;

					while (CSCommon.IsCSWordChar(text[end]))
						end++;

					string name = text.Substring(index, end - index);
					string nameNew = filter(name);

					if (name == nameNew) // ? 置き換え禁止ワード
					{
						index = end;

						// ? 置き換え禁止クラス名
						// -> (置き換え禁止ワード).(後続のワード).(後続のワード).(後続のワード) ... の「後続のワード」も置き換え禁止とする。
						if (f_isUnrenameableClassName(name))
						{
							while (text[index] == '.' && CSCommon.IsCSWordChar(text[index + 1]))
							{
								end = index + 2;

								while (CSCommon.IsCSWordChar(text[end]))
									end++;

								index = end;
							}
						}
					}
					else
					{
						sss.Splice(index, end - index, nameNew);
						index = end;
					}
				}
			}
			text = sss.GetString();
			sss = null;

			text = text.Substring(0, text.Length - 1); // 番兵除去

			text = RX_UnescapeNoRenameLines(text, escapedLines);
			text = RX_Mix(text, text_bk);

			File.WriteAllText(_file, text, Encoding.UTF8);
		}

		private static string RX_EscapeNoRenameLines(string text, Dictionary<string, string> escapedLines)
		{
			string[] lines = SCommon.TextToLines(text);

			lines = lines.Select(line =>
			{
				if (line.EndsWith(CSSpecialStrings.NO_RENAME_LINE_SUFFIX))
				{
					string ident = "9_" + CSCommon.CreateNewIdent(); // 数字で始まるトークンは置き換えない。
					escapedLines.Add(ident, line);
					return ident;
				}
				return line;
			})
			.ToArray();

			return SCommon.LinesToText(lines);
		}

		private static string RX_UnescapeNoRenameLines(string text, Dictionary<string, string> escapedLines)
		{
			string[] lines = SCommon.TextToLines(text);

			lines = lines.Select(line =>
			{
				if (escapedLines.ContainsKey(line))
					return escapedLines[line];

				return line;
			})
			.ToArray();

			return SCommon.LinesToText(lines);
		}

		private static string RX_Mix(string text, string text_bk)
		{
			string[] lines = SCommon.TextToLines(text);
			string[] lines_bk = SCommon.TextToLines(text_bk);

			if (lines.Length != lines_bk.Length) // 同じはず
				throw null; // 想定外

			List<string> dest = new List<string>();

			for (int index = 0; index < lines.Length; index++)
			{
				dest.Add(lines[index]);
				dest.Add("// " + lines_bk[index]);
			}
			return SCommon.LinesToText(dest.ToArray());
		}

		private class SMO_RangeInfo
		{
			public int Start;
			public int End;
			public bool HoldingOrder;
		}

		public void ShuffleMemberOrder()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			int start;
			int end = SMO_GetClassEnd(lines);

			if (end == -1) // ? クラス閉じが見つからない -> 想定外
				throw null;

			List<SMO_RangeInfo> ranges = new List<SMO_RangeInfo>();
			bool dllImportFlag = false;

			for (int index = 0; index < end; index++)
			{
				string line = lines[index];
				bool foundMemberFlag = false;

				if (line.StartsWith("\t\t[")) // ? [DllImport(... とか
				{
					if (!dllImportFlag)
					{
						foundMemberFlag = true;
						dllImportFlag = true;
					}
				}
				else if (
					line.StartsWith("\t\tprivate ") ||
					line.StartsWith("\t\tprotected ") ||
					line.StartsWith("\t\tpublic ") ||
					line.StartsWith("\t\tstatic ") // static void Main() とか
					)
				{
					if (!dllImportFlag)
						foundMemberFlag = true;
					else
						dllImportFlag = false;
				}

				if (foundMemberFlag)
				{
					// クラスのメンバーを追加
					ranges.Add(new SMO_RangeInfo()
					{
						Start = index,
						End = -1, // ダミー
					});
				}
			}
			if (ranges.Count <= 1) // ? メンバーが１つ以下 -> シャッフル不要
			{
				Console.WriteLine("シャッフル・キャンセル -- メンバーが１つ以下");
				return;
			}
			start = ranges[0].Start;

			for (int index = 1; index < ranges.Count; index++)
				ranges[index - 1].End = ranges[index].Start;

			ranges[ranges.Count - 1].End = end;

			foreach (SMO_RangeInfo range in ranges)
				range.HoldingOrder = Regex.IsMatch(lines[range.Start], "^[^(]*[=]"); // ? 初期化有りフィールド

			// シャッフル
			{
				List<SMO_RangeInfo> hoRanges = new List<SMO_RangeInfo>();
				List<SMO_RangeInfo> unhoRanges = new List<SMO_RangeInfo>();

				foreach (SMO_RangeInfo range in ranges)
				{
					if (range.HoldingOrder)
						hoRanges.Add(range);
					else
						unhoRanges.Add(range);
				}
				ranges = hoRanges;
				hoRanges = null;

				foreach (SMO_RangeInfo range in unhoRanges)
					ranges.Insert(SCommon.CRandom.GetInt(ranges.Count + 1), range);
			}

			// ファイルの先頭
			ranges.Insert(0, new SMO_RangeInfo()
			{
				Start = 0,
				End = start,
			});

			// ファイルの終端
			ranges.Add(new SMO_RangeInfo()
			{
				Start = end,
				End = lines.Length,
			});

			File.WriteAllLines(_file, SMO_GetOrderLines(lines, ranges), Encoding.UTF8);
		}

		private static int SMO_GetClassEnd(string[] lines)
		{
			for (int index = lines.Length - 1; 0 <= index; index--)
				if (lines[index].StartsWith("\t}")) // ? クラス閉じ
					return index;

			return -1; // not found
		}

		private static IEnumerable<string> SMO_GetOrderLines(string[] lines, IEnumerable<SMO_RangeInfo> ranges)
		{
			foreach (SMO_RangeInfo range in ranges)
			{
				yield return "// HO=" + range.HoldingOrder;

				for (int index = range.Start; index < range.End; index++)
					yield return lines[index];
			}
		}

		/// <summary>
		/// ビルドに不要な情報を除去する。
		/// -- 万が一出力実行ファイルに情報が含まれていると困るので、念のため実行する。
		/// ---- 恐らく実行しなくても良い。
		/// </summary>
		public void RemoveUnnecessaryInformations()
		{
			string[] lines = File.ReadAllLines(_file, Encoding.UTF8);

			lines = lines
				.Where(line => line != "") // ? 空行ではない。
				.Where(line => !line.StartsWith("//")) // ? 行頭から始まるC++系コメント行ではない。
				.Select(line => RUI_RemoveCPPComment(line))
				.ToArray();

			File.WriteAllLines(_file, lines, Encoding.UTF8);

			this.RUI_FormatSource();
		}

		/// <summary>
		/// キープされたC++コメントがこの時点まで残っているはず。
		/// これを除去する。
		/// </summary>
		/// <param name="line">行(入力)</param>
		/// <returns>行(出力)</returns>
		private static string RUI_RemoveCPPComment(string line)
		{
			// プリプロセッサの除去、Cコメントの除去、文字と文字列のエスケープ etc. は完了しているので、単純な検索で良いはず。

			int index = line.IndexOf("//");

			if (index != -1)
			{
				line = line.Substring(0, index);
				line = line.TrimEnd();
			}
			return line;
		}

		/// <summary>
		/// ソースコードの整形
		/// -- 尚更実行しなくても良い処理
		/// </summary>
		private void RUI_FormatSource()
		{
			{
				string text = File.ReadAllText(_file, Encoding.UTF8);

				text = text.Replace(" {", CSConsts.CRLF + "\t\t{");
				text = text.Replace(" }", CSConsts.CRLF + "\t\t}");
				text = text.Replace("{ ", "{" + CSConsts.CRLF);
				text = text.Replace("} ", "}" + CSConsts.CRLF);
				text = text.Replace("; yield return ", ";" + CSConsts.CRLF + "yield return ");

				// "\t\tconst int" を ("const" -> "") の置き換えを実施して "\t\t int" になった場合など
				//
				text = text.Replace("\t ", "\t");

				File.WriteAllText(_file, text, Encoding.UTF8);
			}

			{
				IEnumerable<string> lines = File.ReadAllLines(_file, Encoding.UTF8);

				lines = lines.Select(line =>
				{
					if (
						!line.StartsWith("using ") &&
						!line.StartsWith("namespace ") &&
						line != "{" &&
						line != "}" &&
						line != "" && // ? 空行ではない。
						line[0] != '\t' // ? インデント無し
						)
						line = "\t\t\t" + line;

					return line;
				});

				lines = lines.Where(line => line != ""); // 空行を除去

				lines = RUI_FS_P1(lines);
				lines = RUI_FS_P2(lines);
				lines = RUI_FS_P3(lines);
				lines = RUI_FS_P4(lines);
				lines = RUI_FS_P5(lines);

				File.WriteAllLines(_file, lines, Encoding.UTF8);
			}
		}

		private static IEnumerable<string> RUI_FS_P1(IEnumerable<string> lines)
		{
			string lastLine = null;
			string trLastLine = null;

			foreach (string line in lines)
			{
				bool _2行目以降 = lastLine != null;
				string trLine = line.Trim();

				if (
					line.StartsWith("namespace ") ||
					_2行目以降 &&
					trLastLine != "{" &&
					(
						trLine.StartsWith("public ") ||
						trLine.StartsWith("protected ") ||
						trLine.StartsWith("private ")
					))
					yield return "";

				yield return line;

				lastLine = line;
				trLastLine = trLine;
			}
		}

		private static IEnumerable<string> RUI_FS_P2(IEnumerable<string> lines)
		{
			string lastLine = null;
			string trLastLine = null;

			foreach (string line in lines)
			{
				bool _2行目以降 = lastLine != null;
				string trLine = line.Trim();

				if (
					_2行目以降 &&
					CSCommon.GetIndentDepth(lastLine) > CSCommon.GetIndentDepth(line) &&
					trLastLine != "" &&
					trLine != "" &&
					trLine != "else" &&
					!trLine.StartsWith("else ") &&
					CSCommon.IsCSWordChar(trLastLine[0]) &&
					CSCommon.IsCSWordChar(trLine[0])
					)
					yield return "";

				yield return line;

				lastLine = line;
				trLastLine = trLine;
			}
		}

		private static IEnumerable<string> RUI_FS_P3(IEnumerable<string> lines)
		{
			string lastLine = null;
			string trLastLine = null;

			foreach (string line in lines)
			{
				bool _2行目以降 = lastLine != null;
				string trLine = line.Trim();

				if (
					_2行目以降 &&
					CSCommon.GetIndentDepth(lastLine) == CSCommon.GetIndentDepth(line) &&
					trLastLine != "" &&
					trLine != "" &&
					CSCommon.IsCSWordChar(trLastLine[0]) &&
					CSCommon.IsCSWordChar(trLine[0]) &&
					Regex.IsMatch(trLine, "^[a-z]+ \\(") && // ? if ( ... , for ( ... など
					!trLine.StartsWith("using ")
					)
					yield return "";

				yield return line;

				lastLine = line;
				trLastLine = trLine;
			}
		}

		private static IEnumerable<string> RUI_FS_P4(IEnumerable<string> lines)
		{
			string lastLine = null;
			string trLastLine = null;

			foreach (string line in lines)
			{
				bool _2行目以降 = lastLine != null;
				string trLine = line.Trim();

				if (
					_2行目以降 &&
					Regex.IsMatch(trLastLine, "^[\\)\\}]+;$") && // ? ); , }; , }); など
					trLine != ""
					)
					yield return "";

				yield return line;

				lastLine = line;
				trLastLine = trLine;
			}
		}

		private static string RUI_FS_CommentMessage = null;

		private static IEnumerable<string> RUI_FS_P5(IEnumerable<string> lines)
		{
			if (RUI_FS_CommentMessage == null)
				RUI_FS_CommentMessage = "Confused by ConfuserForElsa " + Guid.NewGuid().ToString("B");

			foreach (string line in lines)
			{
				if (line.StartsWith("\tpublic ")) // ? クラス || 構造体
				{
					yield return "\t/// <summary>";
					yield return "\t/// " + RUI_FS_CommentMessage;
					yield return "\t/// </summary>";
				}
				else if (line.StartsWith("\t\tpublic ")) // ? メソッド || フィールド || プロパティ || サブクラス
				{
					yield return "\t\t/// <summary>";
					yield return "\t\t/// " + RUI_FS_CommentMessage;
					yield return "\t\t/// </summary>";
				}
				yield return line;
			}
		}
	}
}
