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
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { @"C:\temp", "/E", "2" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/T", "0", "0" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/TC", "255", "255", "255" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/BC", "255", "255", "255" }));
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

				MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			string inputPath = SCommon.MakeFullPath(ar.NextArg());

			if (Directory.Exists(inputPath))
			{
				Main6(inputPath, ar);
			}
			else if (File.Exists(inputPath))
			{
				using (WorkingDir wd = new WorkingDir())
				{
					string dir = wd.MakePath();

					SCommon.CreateDir(dir);
					File.Copy(inputPath, Path.Combine(dir, Path.GetFileName(inputPath)));

					Main6(dir, ar);
				}
			}
			else
			{
				throw new Exception("Bad inputPath");
			}
			Console.WriteLine("done!");
		}

		private void Main6(string dir, ArgsReader ar)
		{
			string outputDir = SCommon.GetOutputDir();

			foreach (string file in Directory.GetFiles(dir))
			{
				if (Consts.IMAGE_EXTS.Any(imageExt => SCommon.EqualsIgnoreCase(imageExt, Path.GetExtension(file))))
				{
					string outputLocalName = Path.GetFileNameWithoutExtension(file) + ".png";
					string outputFile = Path.Combine(outputDir, outputLocalName);

					Console.WriteLine("< " + file);
					Console.WriteLine("> " + outputFile);

					EditPicture(file, outputFile, ar.GetClone()); // 読み進められるよう、複製する。

					Console.WriteLine("done");
				}
			}
		}

		private void EditPicture(string inputFile, string outputFile, ArgsReader ar)
		{
			Canvas canvas = Canvas.LoadFromFile(inputFile);

			while (ar.HasArgs())
			{
				if (ar.ArgIs("/E"))
				{
					int magnification = int.Parse(ar.NextArg());

					if (magnification < 2 || SCommon.IMAX / Math.Max(canvas.W, canvas.H) < magnification)
						throw new Exception("Bad magnification: " + magnification);

					canvas = canvas.Expand(canvas.W * magnification, canvas.H * magnification, 1);
				}
				else if (ar.ArgIs("/T"))
				{
					int x = int.Parse(ar.NextArg());
					int y = int.Parse(ar.NextArg());

					ToTransparent(canvas, canvas[x, y].WithoutAlpha());
				}
				else if (ar.ArgIs("/TC"))
				{
					int r = int.Parse(ar.NextArg());
					int g = int.Parse(ar.NextArg());
					int b = int.Parse(ar.NextArg());

					if (
						r < 0 || 255 < r ||
						g < 0 || 255 < g ||
						b < 0 || 255 < b
						)
						throw new Exception("不正な色");

					ToTransparent(canvas, new I3Color(r, g, b));
				}
				else if (ar.ArgIs("/BC"))
				{
					int r = int.Parse(ar.NextArg());
					int g = int.Parse(ar.NextArg());
					int b = int.Parse(ar.NextArg());

					if (
						r < 0 || 255 < r ||
						g < 0 || 255 < g ||
						b < 0 || 255 < b
						)
						throw new Exception("不正な色");

					canvas = ToUntransparent(canvas, new I3Color(r, g, b));
				}
				else
				{
					throw new Exception("不明なオプション");
				}
			}
			canvas.Save(outputFile);
		}

		private void ToTransparent(Canvas canvas, I3Color transTargColor)
		{
			for (int x = 0; x < canvas.W; x++)
			{
				for (int y = 0; y < canvas.H; y++)
				{
					I3Color color = canvas[x, y].WithoutAlpha();

					if (Common.IsSame(color, transTargColor))
					{
						canvas[x, y] = color.WithAlpha(0); // 透明にする。
					}
				}
			}
		}

		private Canvas ToUntransparent(Canvas canvas, I3Color bgColor)
		{
			Canvas dest = new Canvas(canvas.W, canvas.H);

			dest.Fill(bgColor.WithAlpha());
			dest.DrawImage(canvas, 0, 0, true);

			return dest;
		}
	}
}
