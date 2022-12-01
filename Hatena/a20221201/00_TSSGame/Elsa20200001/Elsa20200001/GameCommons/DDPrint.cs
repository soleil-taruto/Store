using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDPrint
	{
		// Extra >

		private class ExtraInfo
		{
			public DDTaskList TL = null;
			public I3Color Color = new I3Color(255, 255, 255);
			public I3Color BorderColor = new I3Color(0, 0, 0); // BorderWidth == 0 のときは使用しない。
			public int BorderWidth = 0;
		}

		private static ExtraInfo Extra = new ExtraInfo();

		public static void Reset()
		{
			Extra = new ExtraInfo();
		}

		public static void SetTaskList(DDTaskList tl)
		{
			Extra.TL = tl;
		}

		public static void SetColor(I3Color color)
		{
			Extra.Color = color;
		}

		public static void SetBorder(I3Color color, int width = 2)
		{
			Extra.BorderColor = color;
			Extra.BorderWidth = width;
		}

		// < Extra

		private const int DEFAULT_FONT_SIZE = 32;

		private static int P_BaseX;
		private static int P_BaseY;
		private static int P_YStep;
		private static int P_X;
		private static int P_Y;
		private static int P_FontSize = DEFAULT_FONT_SIZE;

		public static void SetPrint(int x, int y, int yStep)
		{
			SetPrint(x, y, yStep, DEFAULT_FONT_SIZE);
		}

		public static void SetPrint(int x, int y, int yStep, int fontSize) // 難読化のため、デフォルト引数をオーバーロードの引数に指定する。
		{
			P_BaseX = x;
			P_BaseY = y;
			P_YStep = yStep;
			P_X = 0;
			P_Y = 0;
			P_FontSize = fontSize;
		}

		public static void PrintRet()
		{
			P_X = 0;
			P_Y += P_YStep;
		}

		private static DDFont Font
		{
			get
			{
				return DDFontUtils.GetFont("Kゴシック", P_FontSize);
			}
		}

		private static void Print_Main2(string line, int x, int y, I3Color color, double centeringRate)
		{
			// centeringRate:
			// 0.0 == 右寄せ (最初の文字の左側面が x になる)
			// 0.5 == 中央寄せ
			// 1.0 == 左寄せ (最後の文字の右側面が x になる)

			x -= SCommon.ToInt(DDFontUtils.GetDrawStringWidth(line, Font) * centeringRate);

			DDFontUtils.DrawString(x, y, line, Font, false, color);
		}

		private static void Print_Main(string line, int x, int y, double centeringRate)
		{
			if (Extra.BorderWidth != 0)
			{
				int BORDER_WIDTH = Extra.BorderWidth;
				int BORDER_STEP = Extra.BorderWidth;

				for (int xc = -BORDER_WIDTH; xc <= BORDER_WIDTH; xc += BORDER_STEP)
					for (int yc = -BORDER_WIDTH; yc <= BORDER_WIDTH; yc += BORDER_STEP)
						Print_Main2(line, x + xc, y + yc, Extra.BorderColor, centeringRate);
			}
			Print_Main2(line, x, y, Extra.Color, centeringRate);
		}

		public static void Print(string line, double centeringRate = 0.0)
		{
			if (line == null)
				throw new DDError();

			int x = P_BaseX + P_X;
			int y = P_BaseY + P_Y;

			if (Extra.TL == null)
			{
				Print_Main(line, x, y, centeringRate);
			}
			else
			{
				ExtraInfo storedExtra = Extra;

				Extra.TL.Add(() =>
				{
					ExtraInfo currExtra = Extra;

					Extra = storedExtra;
					Print_Main(line, x, y, centeringRate);
					Extra = currExtra;

					return false;
				});
			}

			//int w = DX.GetDrawStringWidth(line, SCommon.ENCODING_SJIS.GetByteCount(line));
			int w = DX.GetDrawStringWidthToHandle(line, SCommon.ENCODING_SJIS.GetByteCount(line), Font.GetHandle(), 0);

			if (w < 0 || SCommon.IMAX < w)
				throw new DDError();

			P_X += w;
		}

		public static void PrintLine(string line)
		{
			Print(line);
			PrintRet();
		}
	}
}
