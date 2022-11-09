using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte
{
	public static class EditPictures
	{
		/// <summary>
		/// 画像の詳細が分からないようにマスク処理を施す。
		/// 出力先：C:\1, 2, 3, ...
		/// </summary>
		/// <param name="dir">入力フォルダ</param>
		public static void MaskPictures(string dir)
		{
			foreach (string file in Directory.GetFiles(dir))
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".bmp" ||
					ext == ".gif" ||
					ext == ".jpg" ||
					ext == ".jpeg" ||
					ext == ".png"
					)
				{
					MaskPictureFile(file);
				}
			}
		}

		private static void MaskPictureFile(string file)
		{
			Console.WriteLine("< " + file);
			Console.WriteLine("> AUTO");

			Canvas canvas = Canvas.LoadFromFile(file);

			int w = canvas.W;
			int h = canvas.H;

			int mosaicScale = Math.Max(20, Math.Min(w, h) / 30);

			int small_w = Math.Max(1, w / mosaicScale);
			int small_h = Math.Max(1, h / mosaicScale);

			canvas = canvas.Expand(small_w, small_h);
			canvas = canvas.Expand(w, h, 1);

			// ----

			const string DR_CHR_FONT_NAME = "Impact";
			const FontStyle DR_CHR_FONT_STYLE = FontStyle.Bold;
			const int DR_CHR_BLUR_LV = 4;

			int DR_CHR_SCALE = (int)(mosaicScale * 1.7);
			int DR_CHR_WH_MIN = 20;
			int DR_CHR_ZURE = 10;

			int DR_CHR_W = Math.Max(DR_CHR_WH_MIN, w / Math.Max(1, w / DR_CHR_SCALE));
			int DR_CHR_H = Math.Max(DR_CHR_WH_MIN, h / Math.Max(1, h / DR_CHR_SCALE));

			Canvas[] drChrImgs = "@MASKED".Select(chr =>
			{
				Canvas drChrImg = new Canvas(DR_CHR_W, DR_CHR_H);

				drChrImg.Fill(new I4Color(0, 0, 0, 0));
				drChrImg.DrawString(
					chr.ToString(),
					DR_CHR_SCALE * 3,
					DR_CHR_FONT_NAME,
					DR_CHR_FONT_STYLE,
					new I3Color(
						0,
						0,
						0
						),
					new I4Rect(
						0,
						0,
						DR_CHR_W - DR_CHR_ZURE,
						DR_CHR_H - DR_CHR_ZURE
						),
					DR_CHR_BLUR_LV
					);
				drChrImg.DrawString(
					chr.ToString(),
					DR_CHR_SCALE * 3,
					DR_CHR_FONT_NAME,
					DR_CHR_FONT_STYLE,
					new I3Color(
						255,
						255,
						255
						),
					new I4Rect(
						DR_CHR_ZURE,
						DR_CHR_ZURE,
						DR_CHR_W - DR_CHR_ZURE,
						DR_CHR_H - DR_CHR_ZURE
						),
					DR_CHR_BLUR_LV
					);
				drChrImg.FilterAllDot(dot =>
				{
					dot.A = (int)(dot.A * 0.1);
					return dot;
				});

				return drChrImg;
			})
			.ToArray();

			{
				int drChrIdx = 0;

				for (int drT = 0; drT + DR_CHR_H <= h; drT += DR_CHR_H)
				{
					for (int drL = 0; drL + DR_CHR_W <= w; drL += DR_CHR_W)
					{
						canvas.DrawImage(drChrImgs[drChrIdx], drL, drT, true);

						drChrIdx++;
						drChrIdx %= drChrImgs.Length;
					}
				}
			}

			canvas.Save(Path.Combine(SCommon.GetOutputDir(), Path.GetFileNameWithoutExtension(file) + ".png"));
		}
	}
}
