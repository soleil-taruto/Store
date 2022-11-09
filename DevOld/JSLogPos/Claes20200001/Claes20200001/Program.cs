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
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { @"C:\temp\Ricecake" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//Common.Pause();
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

				Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			string targDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(targDir))
				throw new Exception("no targDir: " + targDir);

			foreach (string file in Directory.GetFiles(targDir, "*", SearchOption.AllDirectories))
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".js" ||
					ext == ".jsp"
					)
					ProcJSPFile(file);
			}
		}

		private void ProcJSPFile(string file)
		{
			ProcMain.WriteLog("file: " + file);

			string text = File.ReadAllText(file, Encoding.UTF8);

			for (int funcPtn = 1; funcPtn <= 2; funcPtn++)
			{
				for (int index = 0; ; )
				{
					int p;
					string name;

					if (funcPtn == 1)
					{
						p = text.IndexOf("function", index);

						if (p == -1)
							break;

						p += 8;

						if (' ' < text[p] && text[p] != '(')
						{
							index = p;
							continue;
						}
						p = Common.FirstPosition(text, p, chr => ' ' < chr);

						if (text[p] == '(')
						{
							name = "NO-NAME";
						}
						else
						{
							int q = Common.FirstPosition(text, p, chr => chr <= ' ' || chr == '(');
							name = text.Substring(p, q - p);
							p = Common.FirstPosition(text, q, chr => chr == '(');
						}
						p = Common.FirstPosition(text, p, chr => chr == ')');
						p = Common.FirstPosition(text, p, chr => chr == '{');
						p++;
					}
					else // funcPtn == 2
					{
						p = text.IndexOf("=>", index);

						if (p == -1)
							break;

						p += 2;
						p = Common.FirstPosition(text, p, chr => ' ' < chr);

						if (text[p] != '{')
						{
							index = p;
							continue;
						}
						p++;

						name = "LAMBDA";
					}

					ProcMain.WriteLog("name: " + name);

					// Insert LOGPOS code
					{
						string textLeft = text.Substring(0, p);
						string textRight = text.Substring(p);

						int lineNumb = textLeft.Where(chr => chr == '\n').Count() + 1;

						textLeft += " if (typeof LOGPOS == 'function') { LOGPOS(\"" + Path.GetFileName(file) + " (" + lineNumb + ") " + name + "\"); }";
						p = textLeft.Length;
						text = textLeft + textRight;
					}

					index = p;
				}
			}
			File.WriteAllText(file, text, Encoding.UTF8);
		}
	}
}
