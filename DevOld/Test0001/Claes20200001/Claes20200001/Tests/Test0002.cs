using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Utilities;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			foreach (string file in Directory.GetFiles(@"C:\temp"))
			{
				Canvas canvas = Canvas.LoadFromFile(file);

				const int PIC_W = 1000;
				const int PIC_H = 1872;

				canvas = canvas.Expand(25, 50);
				canvas = canvas.Expand(PIC_W, PIC_H);

				// W : 1000 == 20 x 50 == 10 x 100
				// H : 1872 == 26 x 72 == 13 x 144

				const int TILE_W = 100;
				const int TILE_H = 144;

				Canvas[] drChrImgs = "MASKED!".Select(chr =>
				{
					Canvas drChrImg = new Canvas(TILE_W, TILE_H);

					const int ZURE = 10;

					drChrImg.Fill(new I4Color(0, 0, 0, 0));
					drChrImg.DrawString(
						"" + chr,
						400,
						"Impact",
						FontStyle.Bold,
						new I3Color(
							0,
							0,
							0
							),
						new I4Rect(
							0,
							0,
							TILE_W - ZURE,
							TILE_H - ZURE
							),
						8
						);
					drChrImg.DrawString(
						"" + chr,
						400,
						"Impact",
						FontStyle.Bold,
						new I3Color(
							255,
							255,
							255
							),
						new I4Rect(
							ZURE,
							ZURE,
							TILE_W - ZURE,
							TILE_H - ZURE
							),
						8
						);
					drChrImg.ForEachDot(dot =>
					{
						dot.A = (int)(dot.A * 0.3);
						return dot;
					});

					return drChrImg;
				})
				.ToArray();

				{
					int drChrIdx = 0;

					for (int drT = 0; drT < PIC_H; drT += TILE_H)
					{
						for (int drL = 0; drL < PIC_W; drL += TILE_W)
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
}
