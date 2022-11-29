using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using Charlotte.JSConfusers;
using Charlotte.Utilities;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4(ar);
			}
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			//Main4(new ArgsReader(new string[] { @"C:\Dev\GameJS\Hako\Gattonero20200001", @"C:\Dev\GameJS\Hako\res", @"C:\temp" }));
			//Main4(new ArgsReader(new string[] { "/R", @"C:\Dev\GameJS\Hako\Gattonero20200001", @"C:\Dev\GameJS\Hako\res", @"C:\temp" }));
			Main4(new ArgsReader(new string[] { @"C:\Dev\GameJS\BlackJack_v2\Gattonero20200001", @"C:\Dev\GameJS\BlackJack_v2\res", @"C:\temp" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//SCommon.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);

				MessageBox.Show("" + e, "JSJoin - 変換エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				//Console.WriteLine("変換エラー(エンターキーを押して下さい)");
				//Console.ReadLine();
			}
		}

		private class IdentifierInfo
		{
			public string Name;
			public int Index;
		}

		private string SourceDir;
		private string ResourceDir;
		private string OutputDir;

		private List<string> SourceFiles = new List<string>();
		private List<string> ResourceFiles = new List<string>();
		private List<string> JSLines = new List<string>();
		private List<string> JSFunctions;
		private List<string> JSVariables;
		private List<string> Tags = new List<string>();

		private void Main5(ArgsReader ar)
		{
			bool releaseMode = ar.ArgIs("/R");

			this.SourceDir = SCommon.MakeFullPath(ar.NextArg());
			this.ResourceDir = SCommon.MakeFullPath(ar.NextArg());
			this.OutputDir = SCommon.MakeFullPath(ar.NextArg());

			ar.End();

			if (!Directory.Exists(this.SourceDir))
				throw new Exception("no SourceDir");

			if (!Directory.Exists(this.ResourceDir))
				throw new Exception("no ResourceDir");

			if (!Directory.Exists(this.OutputDir))
				throw new Exception("no OutputDir");

			// 出力先をクリア
			SCommon.DeletePath(this.OutputDir);
			SCommon.CreateDir(this.OutputDir);

			foreach (string file in Directory.GetFiles(this.SourceDir, "*", SearchOption.AllDirectories))
				if (!file.Contains("\\_")) // ? アンダースコアで始まるローカル名を含まない。
					if (SCommon.EndsWithIgnoreCase(file, ".js")) // ? JSファイル
						this.SourceFiles.Add(file);

			foreach (string file in Directory.GetFiles(this.ResourceDir, "*", SearchOption.AllDirectories))
				if (!file.Contains("\\_")) // ? アンダースコアで始まるローカル名を含まない。
					this.ResourceFiles.Add(file);

			// ソース・ファイルは読み込む順序に副作用があるので注意すること。

			// ソース・ファイルのソート
			this.SourceFiles.Sort(Common.CompPath);
			// リソースのソート
			this.ResourceFiles.Sort(Common.CompPath);

			for (int index = 0; index < this.SourceFiles.Count; index++)
			{
				string file = this.SourceFiles[index];
				string text = File.ReadAllText(file, SCommon.ENCODING_SJIS);
				string uniqueString = string.Format("_P_{0:D4}_", index);

				text = text.Replace("@@", uniqueString);

				string[] lines = SCommon.TextToLines(text);

				lines = ReplaceSpecialCode_01(lines, file, index, releaseMode);

				this.JSLines.Add("// ここから " + file);
				this.JSLines.AddRange(lines);
				this.JSLines.Add("// ここまで " + file);
			}

			this.EraseRedundancyCode(this.JSLines);

			this.SyntaxCheck(this.JSLines);

			this.JSFunctions = this.CollectFunctions(this.JSLines).Select(v => v.Name).ToList();
			this.JSVariables = this.CollectVariables(this.JSLines).Select(v => v.Name).ToList();

			string[] identifiers = this.JSFunctions.Concat(this.JSVariables).ToArray();

			SCommon.ForEachPair(identifiers, (a, b) =>
			{
				if (a == b)
					throw new Exception("識別子の重複：" + a);
			});

			this.JSLines = ReplaceSpecialCode_02(this.JSLines.ToArray()).ToList();

			List<string> escapedJSLines = this.JSLines;
			this.JSLines = new List<string>();

			this.JSLines.Add("// RESOURCE_* ここから");
			this.JSLines.Add("//");

			string resDir = Path.Combine(this.OutputDir, Consts.OUTPUT_RES_DIR_NAME);

			if (releaseMode)
			{
				SCommon.DeletePath(resDir);
				SCommon.CreateDir(resDir);
			}
			HashSet<string> resNames = new HashSet<string>();
			UniqueIssuer resFileNameGen = new UniqueIssuer(() => SCommon.CRandom.GetInt(100000000).ToString("D8"));

			foreach (string file in this.ResourceFiles)
			{
				string name = file;
				name = SCommon.ChangeRoot(name, this.ResourceDir);

				// ex. @"Data\music-0001.mp3" -> "Data__music_0001_mp3"
				{
					StringBuilder buff = new StringBuilder();

					foreach (char chr in name)
					{
						if (chr == '\\')
							buff.Append("__");
						else if (!JSCommon.IsJSWordChar(chr))
							buff.Append('_');
						else
							buff.Append(chr);
					}
					name = buff.ToString();
				}

				if (resNames.Contains(name))
					throw new Exception("リソース名の重複：" + name);

				resNames.Add(name);

				string url;

				if (releaseMode) // ? リリース・モード -> リリースフォルダへ展開
				{
					//string resFileExt = ".bin";
					string resFileExt = Path.GetExtension(file);

					string resFile = Path.Combine(resDir, resFileNameGen.Next() + resFileExt);

					File.Copy(file, resFile);

					url = Consts.OUTPUT_RES_DIR_NAME + "/" + Path.GetFileName(resFile);
				}
				else // ? デバッグ・モード -> ローカルファイルへのリンク
				{
					url = "file:" + file.Replace('\\', '/');
				}

				this.JSLines.Add(string.Format("var RESOURCE_{0} = \"{1}\";", name, url));
			}
			this.JSLines.Add("//");
			this.JSLines.Add("// RESOURCE_* ここまで");
			this.JSLines.Add("");

			// DEBUG, RELEASE 定数
			{
				int releaseModeValue = releaseMode ? 1 : 0;

				this.JSLines.Add("var DEBUG = 0 == " + releaseModeValue + ";");
				this.JSLines.Add("var RELEASE = 1 == " + releaseModeValue + ";");
				this.JSLines.Add("");
			}

			this.JSLines.AddRange(escapedJSLines);
			escapedJSLines = null;

			if (releaseMode) // ? リリース・モード -> 難読化
			{
				this.JSLines = new JSConfuser().Confuse(this.JSLines);
			}

			ReplaceWord("var", "let");

			IEnumerable<string> htmlLines = this.CreateHtmlLines(releaseMode);

			File.WriteAllLines(
				Path.Combine(this.OutputDir, "index.html"),
				htmlLines,
				SCommon.ENCODING_SJIS
				);

			this.CollectTags();

			File.WriteAllLines(
				Path.Combine(this.SourceDir, "tags"),
				this.Tags,
				SCommon.ENCODING_SJIS
				);
		}

		private int AutoCounter = 1;

		/// <summary>
		/// 特別なコードをJSコードに変換する。#01
		/// </summary>
		/// <param name="lines">コード行リスト</param>
		/// <param name="sourceFilePath">このソースファイルのパス</param>
		/// <param name="sourceFileIndex">このソースファイルのインデックス(0～)</param>
		/// <param name="releaseMode">リリースモードか</param>
		/// <returns>処理後のコード行リスト</returns>
		private string[] ReplaceSpecialCode_01(string[] lines, string sourceFilePath, int sourceFileIndex, bool releaseMode)
		{
			List<string> extendLines = new List<string>();

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];
				int indentLen;

				for (indentLen = 0; indentLen < line.Length; indentLen++)
					if (' ' < line[indentLen])
						break;

				string indent = line.Substring(0, indentLen); // インデントを抽出
				line = line.Substring(indentLen); // インデントを除去

				string positionString =
					sourceFilePath.Replace('\\', '/') +
					" (" +
					(index + 1) +
					")";

				string sha512_64 = SCommon.Hex.ToString(SCommon.GetSHA512(Encoding.UTF8.GetBytes(positionString))).Substring(0, 16);

				if (releaseMode)
					positionString = sha512_64;
				else
					positionString += " " + sha512_64;

				if (line.StartsWith("LOGPOS();"))
				{
					string trailer = line.Substring(9);

					if (releaseMode)
					{
						string dummyFuncName = Common.CreateRandIdent();

						line = indent + dummyFuncName + "();" + trailer;

						extendLines.Add("function " + dummyFuncName + "()");
						extendLines.Add("{");
						extendLines.Add("\treturn;");
						extendLines.Add("}");
					}
					else
					{
						line = indent + "console.log(\"" + positionString + "\");" + trailer;
					}
				}
				else if (line.StartsWith("error();"))
				{
					string trailer = line.Substring(8);

					line = indent + "throw \"" + positionString + "\";" + trailer;
				}
				else
				{
					continue; // 通常行なので何もせず次行へ
				}

				lines[index] = line; // 行を更新
			}

			string appIdent =
				"APP_" +
				SCommon.Hex.ToString(SCommon.GetSHA512(Encoding.UTF8.GetBytes(this.SourceDir))).Substring(0, 32).ToUpper() +
				"_ID";

			string text = SCommon.LinesToText(lines.Concat(extendLines).ToArray());

			text = Common.Replace(text, "@(ASTR)", () => "*");
			text = Common.Replace(text, "@(AUTO)", () => "" + (AutoCounter++));
			text = Common.Replace(text, "@(UNQN)", () => Common.CreateRandIdent());
			text = Common.Replace(text, "@(UUID)", () => Guid.NewGuid().ToString("B"));
			text = Common.Replace(text, "@(APID)", () => appIdent);
			text = Common.Replace(text, "@(SRCN)", () => "" + (sourceFileIndex + 1));

			lines = SCommon.TextToLines(text);

			return lines;
		}

		/// <summary>
		/// 特別なコードをJSコードに変換する。#02
		/// </summary>
		/// <param name="lines">コード行リスト</param>
		/// <returns>処理後のコード行リスト</returns>
		private string[] ReplaceSpecialCode_02(string[] lines)
		{
			string text = SCommon.LinesToText(lines);

			text = Common.Replace(text, "@(INIT)", () => string.Join(", ", this.JSFunctions.Where(v => v.EndsWith("_INIT"))));
			text = Common.Replace(text, "@(EACH)", () => string.Join(", ", this.JSFunctions.Where(v => v.EndsWith("_EACH"))));
			text = Common.Replace(text, "@(CRDT)", () => string.Join(", ", this.GetCreditStrings()));

			lines = SCommon.TextToLines(text);

			return lines;
		}

		private void CollectTags()
		{
			foreach (string file in this.SourceFiles)
			{
				string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

				this.EraseRedundancyCode(lines);

				foreach (IdentifierInfo identifier in this.CollectDefines(lines))
				{
					this.Tags.Add(file + "(" + (identifier.Index + 1) + ") : " + identifier.Name + " // 定義");
				}
				foreach (IdentifierInfo identifier in this.CollectFunctions(lines))
				{
					this.Tags.Add(file + "(" + (identifier.Index + 1) + ") : " + identifier.Name + " // 関数");
				}
				foreach (IdentifierInfo identifier in this.CollectVariables(lines))
				{
					this.Tags.Add(file + "(" + (identifier.Index + 1) + ") : " + identifier.Name + " // 変数");
				}
			}
		}

		private IEnumerable<IdentifierInfo> CollectDefines(IList<string> lines)
		{
			for (int index = 0; index < lines.Count; index++)
			{
				string line = lines[index];

				if (line == "") // ? 空行
					continue;

				// デリミタ：空白系文字
				//
				string[] tokens = SCommon.Tokenize(line, "\t ", false, true);

				if (tokens.Length < 2)
					continue;

				if (tokens[0] != "///")
					continue;

				string name = tokens[1];

				yield return new IdentifierInfo()
				{
					Name = name,
					Index = index,
				};
			}
		}

		private IEnumerable<IdentifierInfo> CollectFunctions(IList<string> lines)
		{
			for (int index = 0; index < lines.Count; index++)
			{
				string line = lines[index];

				if (line == "") // ? 空行
					continue;

				if (line[0] <= ' ') // ? 空白系文字で始まる == インデント有り
					continue;

				// デリミタ：空白系文字, ジェネレータ関数('*'), 引数の開始('(')
				//
				string[] tokens = SCommon.Tokenize(line, "\t *(", false, true);

				if (tokens.Length < 2)
					continue;

				if (tokens[0] != "function")
					continue;

				string name = tokens[1];

				if (name.StartsWith("@@_")) // ? ファイルスコープ
					continue;

				yield return new IdentifierInfo()
				{
					Name = name,
					Index = index,
				};
			}
		}

		private IEnumerable<IdentifierInfo> CollectVariables(IList<string> lines)
		{
			for (int index = 0; index < lines.Count; index++)
			{
				string line = lines[index];

				if (line == "") // ? 空行
					continue;

				if (line[0] <= ' ') // ? 空白系文字で始まる == インデント有り
					continue;

				// デリミタ：空白系文字, 初期値の開始('='), 宣言の終了(';')
				//
				string[] tokens = SCommon.Tokenize(line, "\t =;", false, true);

				if (tokens.Length < 2)
					continue;

				if (tokens[0] != "var")
					continue;

				string name = tokens[1];

				if (name.StartsWith("@@_")) // ? ファイルスコープ
					continue;

				yield return new IdentifierInfo()
				{
					Name = name,
					Index = index,
				};
			}
		}

		private IEnumerable<string> CreateHtmlLines(bool releaseMode)
		{
			string srcHtmlFile = Path.Combine(this.SourceDir, "_index_" + (releaseMode ? "Release" : "Debug") + ".html.js");

			string[] srcHtmlLines = File.ReadAllLines(srcHtmlFile, SCommon.ENCODING_SJIS);

			foreach (string srcHtmlLine in srcHtmlLines)
			{
				if (srcHtmlLine == "@(SCRIPT)")
				{
					foreach (string line in this.JSLines)
						yield return line;
				}
				else
				{
					yield return srcHtmlLine;
				}
			}
		}

		/// <summary>
		/// 補助コードを除去する。
		/// 指定された行リスト自体を変更する。
		/// </summary>
		/// <param name="lines">行リスト</param>
		private void EraseRedundancyCode(IList<string> lines)
		{
			for (int index = 0; index < lines.Count; index++)
			{
				string line = lines[index];

				for (int p = 0; p + 1 < line.Length; p++)
				{
					// HACK: C系コメントは考慮していない。

					if (line[p] == '/' && line[p + 1] == '/') // C++系コメントのスキップ
					{
						break;
					}

					if (line[p] == '"' || line[p] == '\'') // リテラル文字列のスキップ
					{
						char bracket = line[p];

						for (; ; )
						{
							p++;

							if (line.Length <= p)
								throw new Exception("リテラル文字列が閉じていません。\n行：" + line);

							if (line[p] == bracket)
								break;

							if (line[p] == '\\')
							{
								p++;

								if (line[p] == 'x')
									p += 2;
								else if (line[p] == 'u')
									p += 4;
							}
						}
						continue;
					}

					if (
						line[p] == '<' &&
						line[p + 1] > ' ' &&
						line[p + 1] != '<' &&
						line[p + 1] != '='
						)
					{
						int q = line.IndexOf('>', p + 1);

						if (q == -1)
							throw new Exception("補助コードが閉じていません。\n行：" + line);

						{
							int r = line.IndexOf('<', p + 1);

							if (r != -1 && r < q)
								throw new Exception("補助コードは入れ子にできません。\n行：" + line);
						}

						q++;

						while (q < line.Length && line[q] == ' ')
							q++;

						while (0 < p && line[p - 1] == ' ')
							p--;

						bool needPadding = true;

						if (0 < p && line[p - 1] == '(') // 関数の第1引数を想定 -- 他にもあるかも...
							needPadding = false;

						// 補助コードを除去
						line = line.Substring(0, p) + (needPadding ? " " : "") + line.Substring(q);

						// ループでインクリメントしているので、ここでデクリメントする。
						p--;
					}
				}

				// キャスト廃止
				//line = line.Replace("(double)", "");
				//line = line.Replace("(int)", ""); // ToFix を使用しなければならない。

				lines[index] = line;
			}
		}

		private void ReplaceWord(string srcWord, string destWord)
		{
			for (int lineIndex = 0; lineIndex < this.JSLines.Count; lineIndex++)
			{
				string line = this.JSLines[lineIndex];

				for (int index = 0; ; )
				{
					int p = line.IndexOf(srcWord, index);

					if (p == -1)
						break;

					bool replaceable = true; // 置換するか

					if (0 < p && JSCommon.IsJSWordChar(line[p - 1])) // ? 直前が{行頭または非ワード文字}ではない。
					{
						replaceable = false;
					}
					else if (p + srcWord.Length < line.Length && JSCommon.IsJSWordChar(line[p + srcWord.Length])) // ? 直後が{行末または非ワード文字}ではない。
					{
						replaceable = false;
					}

					if (replaceable) // ? 置換する。
					{
						line = line.Substring(0, p) + destWord + line.Substring(p + srcWord.Length);
						index = p + destWord.Length;
					}
					else // ? 置換しない。
					{
						index = p + srcWord.Length;
					}
				}
				this.JSLines[lineIndex] = line;
			}
		}

		private string[] GetCreditStrings()
		{
			List<string[]> credits = new List<string[]>();

			foreach (string file in Directory.GetFiles(this.ResourceDir, "_source.txt", SearchOption.AllDirectories))
			{
				Console.WriteLine("< " + file);

				string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

				if (
					lines.Length < 2 ||
					string.IsNullOrEmpty(lines[0]) ||
					string.IsNullOrEmpty(lines[1])
					)
					throw new Exception("Bad '_source.txt'");

				credits.Add(new string[]
				{
					lines[0],
					lines[1],
				});
			}

			credits.DistinctOrderBy((a, b) => SCommon.Comp(a, b, SCommon.Comp));

			return SCommon.Concat(credits)
				.Select(line => "\"" + string.Join("", line.Select(chr => "\\u" + ((int)chr).ToString("x4"))) + "\"")
				.ToArray();
		}

		/// <summary>
		/// 簡単な文法チェック
		/// </summary>
		/// <param name="lines">連結されたソースコード</param>
		private void SyntaxCheck(List<string> lines)
		{
			foreach (string line in lines)
			{
				string trLine = line.Trim();

				// 以下みたいなのを予防する。
				// return
				//   ret;
				if (
					trLine == "return" ||
					trLine.StartsWith("return /*") ||
					trLine.StartsWith("return //")
					)
				{
					throw new Exception("変な return があります。return; または return ret; として下さい。");
				}
			}
		}
	}
}
