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
using Charlotte.JSChecks;

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

			Main4(new ArgsReader(new string[] { @"C:\Dev\GameJS\Doremy\out\Game_Debug.html" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			SCommon.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				//MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private class LineInfo
		{
			public string Line;
			public string[] Tokens;
			public bool Declare;

			public string Identifier
			{
				get
				{
					if (!this.Declare)
						throw null; // never

					return this.Tokens[1];
				}
			}

			public IEnumerable<string> CodeTokens
			{
				get
				{
					return this.Declare ?
						this.Tokens.Skip(2) :
						this.Tokens;
				}
			}
		}

		private void Main5(ArgsReader ar)
		{
			string htmlFile = ar.NextArg();
			string[] htmlLines = File.ReadAllLines(htmlFile, SCommon.ENCODING_SJIS);
			string[] scriptLines;

			Console.WriteLine("< " + htmlFile); // cout

			{
				int start = SCommon.IndexOf(htmlLines, line => line == "<script>");

				if (start == -1)
					throw new Exception("スクリプト開始タグが見つかりません。");

				start++;
				int end = SCommon.IndexOf(htmlLines, line => line == "</script>", start);

				if (end == -1)
					throw new Exception("スクリプト終了タグが見つかりません。");

				scriptLines = htmlLines.ToList().GetRange(start, end - start).ToArray();
			}

			LineInfo[] lineInfos = scriptLines.Select(line =>
			{
				LineInfo ret = new LineInfo()
				{
					Line = line,
					Tokens = JSLineToTokens(line),
				};

				ret.Declare =
					2 <= ret.Tokens.Length &&
					(
						line.StartsWith("let ") ||
						line.StartsWith("function ") ||
						line.StartsWith("function* ")
					);

				return ret;
			})
			.ToArray();

			List<string> unreferencedIdentifiers = new List<string>();
			List<string> undeclaredIdentifiers = new List<string>();

			foreach (LineInfo declareLine in lineInfos.Where(v => v.Declare))
			{
				string identifier = declareLine.Identifier;

				{
					foreach (LineInfo line in lineInfos)
						foreach (string token in line.CodeTokens)
							if (token == identifier)
								goto found;

					unreferencedIdentifiers.Add(identifier);

				found:
					;
				}
			}

			foreach (LineInfo line in lineInfos)
			{
				foreach (string token in line.CodeTokens)
				{
					char firstChr = token[0];

					// ? グローバル変数と見なす開始文字
					if (
						('A' <= firstChr && firstChr <= 'Z') ||
						firstChr == '_'
						)
					{
						foreach (LineInfo declareLine in lineInfos.Where(v => v.Declare))
							if (declareLine.Identifier == token)
								goto found;

						undeclaredIdentifiers.Add(token);

					found:
						;
					}
				}
			}

			File.WriteAllLines(
				Path.Combine(SCommon.GetOutputDir(), "unreferencedIdentifiers.txt"),
				unreferencedIdentifiers.DistinctOrderBy(SCommon.Comp),
				SCommon.ENCODING_SJIS
				);

			File.WriteAllLines(
				Path.Combine(SCommon.GetOutputDir(), "undeclaredIdentifiers.txt"),
				undeclaredIdentifiers.DistinctOrderBy(SCommon.Comp),
				SCommon.ENCODING_SJIS
				);

			Console.WriteLine("DONE"); // cout
		}

		private static string[] JSLineToTokens(string line)
		{
			line = JSCommon.RemoveLiteralString(line);

			return SCommon.Tokenize(
				new string(line.Select(chr => JSCommon.IsJSWordChar(chr) ? chr : ' ').ToArray()),
				" ",
				false,
				true
				);
		}
	}
}
