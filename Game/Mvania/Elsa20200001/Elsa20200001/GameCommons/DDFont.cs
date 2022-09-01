using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public class DDFont
	{
		public string FontName;
		public int FontSize;
		public int FontThick;
		public bool AntiAliasing;
		public int EdgeSize;
		public bool ItalicFlag;

		private int Handle = -1; // -1 == Unloaded

		/*
			fontThick: 0 ～ 9 (デフォルト値：6)
		*/

		public DDFont(string fontName, int fontSize, int fontThick = 6, bool antiAliasing = true, int edgeSize = 0, bool italicFlag = false)
		{
			if (string.IsNullOrEmpty(fontName)) throw new DDError();
			if (fontSize < 1 || SCommon.IMAX < fontSize) throw new DDError();
			if (fontThick < 0 || 9 < fontThick) throw new DDError();
			// antiAliasing
			if (edgeSize < 0 || SCommon.IMAX < edgeSize) throw new DDError();
			// italicFlag

			this.FontName = fontName;
			this.FontSize = fontSize;
			this.FontThick = fontThick;
			this.AntiAliasing = antiAliasing;
			this.EdgeSize = edgeSize;
			this.ItalicFlag = italicFlag;

			DDFontUtils.Add(this);
		}

		public int GetHandle()
		{
			if (this.Handle == -1)
			{
				int fontType = DX.DX_FONTTYPE_NORMAL;

				if (this.AntiAliasing)
					fontType |= DX.DX_FONTTYPE_ANTIALIASING_8X8;

				if (this.EdgeSize != 0)
					fontType |= DX.DX_FONTTYPE_ANTIALIASING_8X8;

				this.Handle = DX.CreateFontToHandle(
					this.FontName,
					this.FontSize,
					this.FontThick,
					fontType,
					-1,
					this.EdgeSize
					);

				if (this.Handle == -1) // ? 失敗
					throw new DDError();
			}
			return this.Handle;
		}

		public void Unload()
		{
			if (this.Handle != -1)
			{
				if (DX.DeleteFontToHandle(this.Handle) != 0) // ? 失敗
					throw new DDError();

				this.Handle = -1;
			}
		}
	}
}
