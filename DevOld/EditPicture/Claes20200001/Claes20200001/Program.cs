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

			//Main4(new ArgsReader(new string[] { @"C:\temp", "/E", "2" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/T", "0", "0" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/TC", "255", "255", "255" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/BC", "255", "255", "255" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/ACM", "0", "0", "0" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/ACM", "0", "0", "10" }));
			//Main4(new ArgsReader(new string[] { @"C:\temp", "/S", "280", "400" }));
			Main4(new ArgsReader(new string[] { @"C:\temp", "/S4", "280", "400", "1", "1" }));
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

				MessageBox.Show("" + ex, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
				else if (ar.ArgIs("/ACM"))
				{
					int x = int.Parse(ar.NextArg());
					int y = int.Parse(ar.NextArg());
					int marginForPut = int.Parse(ar.NextArg());

					if (
						x < 0 || canvas.W <= x ||
						y < 0 || canvas.H <= y ||
						marginForPut < 0 || SCommon.IMAX < marginForPut
						)
						throw new Exception("不正なパラメータ");

					canvas = AutoCutMargin(canvas, new I2Point(x, y), marginForPut);
				}
				else if (ar.ArgIs("/S"))
				{
					int w = int.Parse(ar.NextArg());
					int h = int.Parse(ar.NextArg());

					if (
						w < 1 || SCommon.IMAX < w ||
						h < 1 || SCommon.IMAX < h
						)
						throw new Exception("不正なパラメータ");

					canvas = canvas.Expand(w, h);
				}
				else if (ar.ArgIs("/S4"))
				{
					int w = int.Parse(ar.NextArg());
					int h = int.Parse(ar.NextArg());
					int xSampling = int.Parse(ar.NextArg());
					int ySampling = int.Parse(ar.NextArg());

					if (
						w < 1 || SCommon.IMAX < w ||
						h < 1 || SCommon.IMAX < h ||
						xSampling < 1 || SCommon.IMAX < xSampling ||
						ySampling < 1 || SCommon.IMAX < ySampling
						)
						throw new Exception("不正なパラメータ");

					canvas = canvas.Expand(w, h, xSampling, ySampling);
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

		private Canvas AutoCutMargin(Canvas canvas, I2Point marginColorPt, int marginForPut)
		{
			I4Color marginColor = canvas[marginColorPt.X, marginColorPt.Y];

			canvas = canvas.GetSubImage(GetRectWithoutMargin(canvas, marginColor));
			canvas = canvas.PutMargin(marginForPut, marginColor);

			return canvas;
		}

		private I4Rect GetRectWithoutMargin(Canvas canvas, I4Color marginColor)
		{
			int l = 0;
			int t = 0;
			int r = canvas.W;
			int b = canvas.H;

			ProcMain.WriteLog(string.Format("LTRB-1: {0}, {1}, {2}, {3}", l, t, r, b));

			for (; ; l++)
			{
				for (int y = 0; y < canvas.H; y++)
					if (!Common.IsSame(canvas[l, y], marginColor))
						goto endLoop_L;
			}
		endLoop_L:

			for (; ; t++)
			{
				for (int x = 0; x < canvas.W; x++)
					if (!Common.IsSame(canvas[x, t], marginColor))
						goto endLoop_T;
			}
		endLoop_T:

			for (; ; r--)
			{
				for (int y = 0; y < canvas.H; y++)
					if (!Common.IsSame(canvas[r - 1, y], marginColor))
						goto endLoop_R;
			}
		endLoop_R:

			for (; ; b--)
			{
				for (int x = 0; x < canvas.W; x++)
					if (!Common.IsSame(canvas[x, b - 1], marginColor))
						goto endLoop_B;
			}
		endLoop_B:

			ProcMain.WriteLog(string.Format("LTRB-2: {0}, {1}, {2}, {3}", l, t, r, b));

			if (r <= l) throw new Exception("マージン以外の部分がありません。(LR)");
			if (b <= t) throw new Exception("マージン以外の部分がありません。(TB)");

			return I4Rect.LTRB(l, t, r, b);
		}
	}
}
