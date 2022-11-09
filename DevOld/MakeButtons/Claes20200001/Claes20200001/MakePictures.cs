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
	public static class MakePictures
	{
		/// <summary>
		/// 例のボタンを作成する。
		/// </summary>
		/// <param name="scale">縮小スケール, 1～</param>
		/// <param name="w">描画時の幅, 2～, 偶数</param>
		/// <param name="h">描画時の高さ, 2～, 偶数</param>
		/// <param name="frameColor">枠の明るい方の色</param>
		/// <param name="text">テキストまたは(出力名:テキスト)</param>
		/// <param name="text_x">テキストの描画時の左側位置, 0～</param>
		public static void MakeButton(int scale, int w, int h, I4Color frameColor, int fontSize, string text, int text_x)
		{
			int FRAME = 60;

			I4Color BACK_COLOR = new I4Color(255, 255, 255, 0);
			I4Color TEXT_COLOR = new I4Color(255, 255, 255, 255);

			Canvas canvas = new Canvas(w, h);

			canvas.Fill(BACK_COLOR);
			canvas.FillCircle(frameColor, new I2Point(0 + h / 2, h / 2), h / 2);
			canvas.FillCircle(frameColor, new I2Point(w - h / 2, h / 2), h / 2);
			canvas.FillRect(frameColor, new I4Rect(h / 2, FRAME * 0, w - h, h - FRAME * 0));
			canvas.FillRect(BACK_COLOR, new I4Rect(h / 2, FRAME * 1, w - h, h - FRAME * 2));

			{
				Func<I4Color, I4Color> filter = dot =>
				{
					dot.R /= 2;
					dot.G /= 2;
					dot.B /= 2;

					return dot;
				};

				canvas.FilterRect(filter, new I4Rect(1 * w / 2, 0 * h / 2, w / 2, h / 2));
				canvas.FilterRect(filter, new I4Rect(0 * w / 2, 1 * h / 2, w / 2, h / 2));
			}

			string name = text;

			{
				int p = text.IndexOf(':');

				if (p != -1)
				{
					name = text.Substring(0, p);
					text = text.Substring(p + 1);
				}
			}

			name = SCommon.ToFairLocalPath(name, -1);

			canvas = canvas.DrawString(text, fontSize, "メイリオ", FontStyle.Regular, TEXT_COLOR, h / 2 + text_x, FRAME + 10);
			canvas = canvas.Expand(w / scale, h / scale);
			canvas.Save(Path.Combine(SCommon.GetOutputDir(), name + ".png"));
		}
	}
}
