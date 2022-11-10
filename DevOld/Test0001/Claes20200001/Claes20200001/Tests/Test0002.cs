using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			string RES_TARGET_EXTS = @"

.html
.c
.bat
.conf
.txt
.js
.cs

";

			string[] TARGET_EXTS = SCommon.TextToLines(RES_TARGET_EXTS).Select(v => v.Trim()).Where(v => v != "").ToArray();

			List<string> tokens = new List<string>();

			foreach (string file in Directory.GetFiles(@"C:\home\GitHub\Store\DevOld", "*", SearchOption.AllDirectories))
			{
				string ext = Path.GetExtension(file).ToLower();

				if (TARGET_EXTS.Contains(ext))
				{
					Console.WriteLine("< " + file); // cout

					Encoding encoding = ext == ".cs" ?
						Encoding.UTF8 :
						SCommon.ENCODING_SJIS;

					string text = File.ReadAllText(file, encoding);

					tokens.AddRange(SCommon.Tokenize(text, "\r\n\t 　", false, true));
				}
			}

			File.WriteAllLines(@"C:\temp\1.txt", tokens, Encoding.UTF8);
			File.WriteAllLines(@"C:\temp\2.txt", tokens.DistinctOrderBy(SCommon.Comp), Encoding.UTF8);
		}
	}
}
