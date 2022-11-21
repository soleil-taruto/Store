using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		private const string ROOT_DIR = @"C:\temp"; // dummy -- 処理的に危なくないけど念のためtempに振っておく
		//private const string ROOT_DIR = @"C:\Factory";

		private static string[] TARG_WORDS = new string[] { "if", "for", "while", "switch", "foreach" };

		public void Test01()
		{
			List<string> dest = new List<string>();

			string[] files = Directory.GetFiles(ROOT_DIR, "*", SearchOption.AllDirectories);

			foreach (string file in files)
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".c" ||
					ext == ".h"
					)
				{
					string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

					foreach (string line in lines)
					{
						foreach (string word in TARG_WORDS)
						{
							int p = 0;

							for (; ; p += word.Length + 1)
							{
								p = line.IndexOf(word + "(", p);

								if (p == -1)
									break;

								string ptn = line.Substring(0, p);
								string fmt = ptn;

								fmt = fmt.Replace('\t', ' ');
								fmt = fmt.Replace("  ", " ");
								fmt = fmt.Replace("  ", " ");
								fmt = fmt.Replace("  ", " ");
								fmt = fmt.Replace("  ", " ");

								if (fmt == " ")
									continue;

								if (fmt == "// ")
									continue;

								if (word == "if" && fmt == " else ")
									continue;

								if (word == "while" && fmt == " } ")
									continue;

								Console.WriteLine(file);
								Console.WriteLine(line);

								dest.Add(file);
								dest.Add(line);
							}
						}
					}
				}
			}
			File.WriteAllLines(SCommon.NextOutputPath() + ".txt", dest, SCommon.ENCODING_SJIS);
		}

		public void Test02()
		{
			List<string> dest = new List<string>();

			string[] files = Directory.GetFiles(ROOT_DIR, "*", SearchOption.AllDirectories);

			foreach (string file in files)
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".c" ||
					ext == ".h"
					)
				{
					string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

					foreach (string line in lines)
					{
						if (line.Contains(" \t"))
						{
							Console.WriteLine(file);
							Console.WriteLine(line);

							dest.Add(file);
							dest.Add(line);
						}
					}
				}
			}
			File.WriteAllLines(SCommon.NextOutputPath() + ".txt", dest, SCommon.ENCODING_SJIS);
		}

		public void Test03()
		{
			List<string> dest = new List<string>();

			string[] files = Directory.GetFiles(ROOT_DIR, "*", SearchOption.AllDirectories);

			foreach (string file in files)
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".c" ||
					ext == ".h"
					)
				{
					string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

					foreach (string line in lines)
					{
						if (line != "" && (line[line.Length - 1] <= ' ')) // ? 空白系文字で終わっている。
						{
							Console.WriteLine(file);
							Console.WriteLine(line);

							dest.Add(file);
							dest.Add(line);
						}
					}
				}
			}
			File.WriteAllLines(SCommon.NextOutputPath() + ".txt", dest, SCommon.ENCODING_SJIS);
		}
	}
}
