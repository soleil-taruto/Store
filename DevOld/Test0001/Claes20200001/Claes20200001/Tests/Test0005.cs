using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0005
	{
		private const string TARGET_DIR = @"C:\temp"; // ★★★ 注意：このフォルダ配下のファイルを変更する！

		public void Test01()
		{
			string[] pngFiles = Directory.GetFiles(TARGET_DIR, "*.png", SearchOption.AllDirectories);

			foreach (string pngFile in pngFiles)
			{
				Canvas canvas = Canvas.LoadFromFile(pngFile);

				int w = canvas.W;
				int h = canvas.H;

				if (w < 20 || h < 20)
				{
					canvas.Fill(MakeFillColor());
				}
				else
				{
					int halfW = w / 2;
					int halfH = h / 2;

					canvas.FillRect(MakeFillColor(), I4Rect.LTRB(0, 0, halfW, halfH)); // 左上
					canvas.FillRect(MakeFillColor(), I4Rect.LTRB(halfW, 0, w, halfH)); // 右上
					canvas.FillRect(MakeFillColor(), I4Rect.LTRB(halfW, halfH, w, h)); // 右下
					canvas.FillRect(MakeFillColor(), I4Rect.LTRB(0, halfH, halfW, h)); // 左下
				}
				canvas.Save(pngFile);
			}
		}

		private I4Color MakeFillColor()
		{
			return new I4Color(
				SCommon.CRandom.GetRange(0, 255),
				SCommon.CRandom.GetRange(0, 255),
				SCommon.CRandom.GetRange(0, 255),
				128 // 半透明
				);
		}
	}
}
