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
using Charlotte.SubCommons;

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
				Main4();
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			//Common.Pause();
		}

		private void Main4()
		{
			// -- choose one --

			Main5();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}

		private void Main5()
		{
			string resInputFiles = @"

C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206181.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206182.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206184.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206185.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206186.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206187.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206188.jpg
C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ大)\yande.re_206189.jpg

C:\home\旧画像\壁紙\ゆるゆり大(ファイルサイズ小)\1554822098.jpg

C:\home\旧画像\壁紙\三人組yandere\yande.re_268445.jpg
C:\home\旧画像\壁紙\三人組yandere\yande.re_268446.jpg
C:\home\旧画像\壁紙\三人組yandere\yande.re_329013.jpg

C:\home\旧画像\壁紙\三人組yandere\yande.re_268450.jpg
C:\home\旧画像\壁紙\三人組yandere\yande.re_268451.jpg
C:\home\旧画像\壁紙\三人組yandere\yande.re_329015.jpg

C:\home\旧画像\壁紙\三人組yandere\yande.re_268452.jpg
C:\home\旧画像\壁紙\三人組yandere\yande.re_268453.jpg
C:\home\旧画像\壁紙\三人組yandere\yande.re_329014.jpg

C:\home\旧画像\壁紙\三人組大\yande.re_268447.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_268448.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_268449.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_268454.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_268455.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_329009.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_329010.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_329011.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_329012.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_329019.jpg
C:\home\旧画像\壁紙\三人組大\yande.re_329020.jpg

C:\home\旧画像\壁紙\ふたごちゃん大\yande.re_48162.jpg
C:\home\旧画像\壁紙\ふたごちゃん大\yande.re_48163.jpg
C:\home\旧画像\壁紙\ふたごちゃん大\yande.re_48164.jpg
C:\home\旧画像\壁紙\ふたごちゃん大\yande.re_48165.jpg
C:\home\旧画像\壁紙\ふたごちゃん大\yande.re_48167.jpg
C:\home\旧画像\壁紙\ふたごちゃん大\yande.re_48169.jpg

C:\home\旧画像\壁紙\ふたごちゃん中\uki127.jpg
C:\home\旧画像\壁紙\ふたごちゃん中\uki128.jpg
C:\home\旧画像\壁紙\ふたごちゃん中\uki129.jpg
C:\home\旧画像\壁紙\ふたごちゃん中\uki131.jpg
C:\home\旧画像\壁紙\ふたごちゃん中\uki132.jpg

";

			string[] inputFiles = SCommon.TextToLines(resInputFiles)
				.Select(v => v.Trim())
				.Where(v => v != "")
				.ToArray();

			string outputDir = Common.GetOutputDir();
			string outputDir_w1920 = Path.Combine(outputDir, "1920x1080");
			string outputDir_w1366 = Path.Combine(outputDir, "1366x768");

			SCommon.CreateDir(outputDir_w1920);
			SCommon.CreateDir(outputDir_w1366);

			for (int index = 0; index < inputFiles.Length; index++)
			{
				string inputFile = inputFiles[index];
				string outputLocalFile = "studio_sdt_" + (index + 1).ToString("D4") + ".png";

				MakeWallPaper(inputFile, Path.Combine(outputDir_w1920, outputLocalFile), 1920, 1080);
				MakeWallPaper(inputFile, Path.Combine(outputDir_w1366, outputLocalFile), 1366, 768);
			}
		}

		/// <summary>
		/// 壁紙ファイルを作成する。
		/// </summary>
		/// <param name="inputFile">入力ファイル</param>
		/// <param name="outputFile">出力ファイル</param>
		/// <param name="w">作成する壁紙の幅</param>
		/// <param name="h">作成する壁紙の高さ</param>
		private void MakeWallPaper(string inputFile, string outputFile, int w, int h)
		{
			Console.WriteLine("< " + inputFile);
			Console.WriteLine("> " + outputFile);
			Console.WriteLine("壁紙の幅：" + w);
			Console.WriteLine("壁紙の高さ：" + h);

			Canvas canvas = Canvas.LoadFromFile(inputFile);
			canvas.Filter(dot => { dot.A = 255; return dot; }); // 念のため透過しないようにする。

			I2Size interior;
			I2Size exterior;
			GetNewSize(canvas.W, canvas.H, w, h, out interior, out exterior);

			Canvas surface = canvas.Expand(interior.W, interior.H);

			canvas = canvas.Expand(exterior.W, exterior.H);
			canvas = canvas.SubRect(new I4Rect((canvas.W - w) / 2, (canvas.H - h) / 2, w, h));
			canvas.Filter(dot => { dot.R /= 2; dot.G /= 2; dot.B /= 2; return dot; }); // 暗くする。

			canvas.DrawImage(surface, (w - surface.W) / 2, (h - surface.H) / 2, false);

			canvas.Save(outputFile);

			Console.WriteLine("壁紙を作成しました。");
		}

		private void GetNewSize(int sw, int sh, int dw, int dh, out I2Size interior, out I2Size exterior)
		{
			Console.WriteLine("元の幅：" + sw);
			Console.WriteLine("元の高さ：" + sh);
			Console.WriteLine("目的の幅：" + dw);
			Console.WriteLine("目的の高さ：" + dh);

			int w = SCommon.ToInt((double)(sw * dh) / sh); // 目的の高さに合わせたときの幅
			int h = SCommon.ToInt((double)(sh * dw) / sw); // 目的の幅に合わせたときの高さ

			if (Math.Abs(w - dw) < 3) // ? 同じサイズ || ほとんど同じサイズ
			{
				Console.WriteLine("同じまたはほとんど同じサイズ");

				interior.W = dw;
				interior.H = dh;
				exterior.W = dw;
				exterior.H = dh;
			}
			else if (w < dw)
			{
				Console.WriteLine("目的のサイズより縦長");

				interior.W = w;
				interior.H = dh;
				exterior.W = dw;
				exterior.H = h;
			}
			else
			{
				Console.WriteLine("目的のサイズより横長");

				interior.W = dw;
				interior.H = h;
				exterior.W = w;
				exterior.H = dh;
			}
			Console.WriteLine("内側の幅：" + interior.W);
			Console.WriteLine("内側の高さ：" + interior.H);
			Console.WriteLine("外側の幅：" + exterior.W);
			Console.WriteLine("外側の高さ：" + exterior.H);
		}
	}
}
