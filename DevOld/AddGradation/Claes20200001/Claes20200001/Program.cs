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
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { "/W", "100", "0", "0", "0", @"C:\temp\input.png" }));
			//Main4(new ArgsReader(new string[] { "/B", "0", "0", "0", "70", @"C:\temp" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			Common.Pause();
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

		private void Main5(ArgsReader ar)
		{
			I3Color color;

			if (ar.ArgIs("/W"))
			{
				color = new I3Color(255, 255, 255);
			}
			else if (ar.ArgIs("/B"))
			{
				color = new I3Color(0, 0, 0);
			}
			else
			{
				throw new Exception("Bad command-option (not /W or /B)");
			}

			int aLT = int.Parse(ar.NextArg());
			int aRT = int.Parse(ar.NextArg());
			int aLB = int.Parse(ar.NextArg());
			int aRB = int.Parse(ar.NextArg());

			if (aLT < 0 || 255 < aLT) throw new Exception("Bad command-option (left top alpha)");
			if (aRT < 0 || 255 < aRT) throw new Exception("Bad command-option (right top alpha)");
			if (aLB < 0 || 255 < aLB) throw new Exception("Bad command-option (left bottom alpha)");
			if (aRB < 0 || 255 < aRB) throw new Exception("Bad command-option (right bottom alpha)");

			string path = SCommon.MakeFullPath(ar.NextArg());

			ar.End();

			string[] files;

			if (File.Exists(path))
			{
				files = new string[] { path };
			}
			else if (Directory.Exists(path))
			{
				files = Directory.GetFiles(path)
					.Where(file => Common.ExtIn(file, ".bmp", ".gif", ".jpg", ".jpeg", ".png"))
					.ToArray();
			}
			else
			{
				throw new Exception("入力ファイル・フォルダがありません。");
			}

			foreach (string file in files)
			{
				string outFile = Path.Combine(Common.GetOutputDir(), Path.GetFileNameWithoutExtension(file) + ".png");

				Console.WriteLine("< " + file);
				Console.WriteLine("> " + outFile);

				Canvas canvas = Canvas.LoadFromFile(file);
				Canvas mask = new Canvas(canvas.W, canvas.H);
				mask.Gradation(
					dot => true,
					color.WithAlpha(aLT),
					color.WithAlpha(aRT),
					color.WithAlpha(aLB),
					color.WithAlpha(aRB)
					);
				Canvas back = canvas.GetClone();
				canvas.DrawImage(mask, 0, 0, true);
				canvas.DrawImage(back, 0, 0, (dDot, sDot) =>
				{
					dDot.A = sDot.A;
					return dDot;
				});
				canvas.Save(outFile);

				Console.WriteLine("done");
			}
		}
	}
}
