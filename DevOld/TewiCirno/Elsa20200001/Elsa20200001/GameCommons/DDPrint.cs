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

		public static void SetBorder(I3Color color, int width = 1)
		{
			Extra.BorderColor = color;
			Extra.BorderWidth = width;
		}

		// < Extra

		private static int P_BaseX;
		private static int P_BaseY;
		private static int P_YStep;
		private static int P_X;
		private static int P_Y;
		private static int P_FontSize = -1; // -1 == デフォルトのフォントを使用する。

		/// <summary>
		/// 開発デバッグ用文字列描画準備
		/// XY-位置は最初の文字の左上座標
		/// </summary>
		/// <param name="x">X-位置</param>
		/// <param name="y">Y-位置</param>
		/// <param name="yStep">Y-ステップ</param>
		public static void SetDebug(int x = 0, int y = 0, int yStep = 16)
		{
			P_BaseX = x;
			P_BaseY = y;
			P_YStep = yStep;
			P_X = 0;
			P_Y = 0;
			P_FontSize = -1;
		}

		/// <summary>
		/// ゲーム画面用文字列描画準備
		/// XY-位置は最初の文字の左上座標
		/// </summary>
		/// <param name="x">X-位置</param>
		/// <param name="y">Y-位置</param>
		/// <param name="yStep">Y-ステップ</param>
		/// <param name="fontSize">フォントサイズ</param>
		public static void SetPrint(int x, int y, int yStep, int fontSize = 24)
		{
			if (fontSize == -1)
				throw new DDError("デフォルトのフォントを使用するには SetDebug() を呼び出して下さい。");

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
				return DDFontUtils.GetFont("木漏れ日ゴシック", P_FontSize);
			}
		}

		private static void Print_Main2(string line, int x, int y, I3Color color)
		{
			if (P_FontSize == -1)
			{
				DX.DrawString(x, y, line, DDUtils.GetColor(color));
			}
			else
			{
				DDFontUtils.DrawString(x, y, line, Font, false, color);
			}
		}

		private static void Print_Main(string line, int x, int y)
		{
			if (Extra.BorderWidth != 0)
			{
				int BORDER_WIDTH = Extra.BorderWidth;
				int BORDER_STEP = Extra.BorderWidth;

				for (int xc = -BORDER_WIDTH; xc <= BORDER_WIDTH; xc += BORDER_STEP)
					for (int yc = -BORDER_WIDTH; yc <= BORDER_WIDTH; yc += BORDER_STEP)
						Print_Main2(line, x + xc, y + yc, Extra.BorderColor);
			}
			Print_Main2(line, x, y, Extra.Color);
		}

		public static void Print(string line)
		{
			if (line == null)
				throw new DDError();

			int x = P_BaseX + P_X;
			int y = P_BaseY + P_Y;

			if (Extra.TL == null)
			{
				Print_Main(line, x, y);
			}
			else
			{
				ExtraInfo storedExtra = Extra;

				Extra.TL.Add(() =>
				{
					ExtraInfo currExtra = Extra;

					Extra = storedExtra;
					Print_Main(line, x, y);
					Extra = currExtra;

					return false;
				});
			}

			int w;

			if (P_FontSize == -1)
				w = DX.GetDrawStringWidth(line, SCommon.ENCODING_SJIS.GetByteCount(line));
			else
				w = DX.GetDrawStringWidthToHandle(line, SCommon.ENCODING_SJIS.GetByteCount(line), Font.GetHandle(), 0);

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
