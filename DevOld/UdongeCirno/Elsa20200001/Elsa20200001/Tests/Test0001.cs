using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		/// <summary>
		/// 選択肢表示テスト
		/// </summary>
		public void Test01()
		{
			for (; ; )
			{
				DDCurtain.DrawCurtain();

				// メッセージ枠
				{
					const int h = 136;

					DDDraw.SetAlpha(0.9);
					DDDraw.DrawRect(Ground.I.Picture.MessageFrame29_Message, 0, DDConsts.Screen_H - h, DDConsts.Screen_W, h);
					DDDraw.Reset();
				}

				// 選択項目 x3
				{
					const int x = 290;
					const int y = 70;
					const int yStep = 100;

					DDDraw.DrawSimple(Ground.I.Picture.MessageFrame29_Button2, x, y + yStep * 0);
					DDDraw.DrawSimple(Ground.I.Picture.MessageFrame29_Button2, x, y + yStep * 1);
					DDDraw.DrawSimple(Ground.I.Picture.MessageFrame29_Button2, x, y + yStep * 2);

					string[] items = new string[]
					{
						"ここに最初の選択肢の文字列を表示します。",
						"ここに真ん中の選択肢の文字列を表示します。",
						"ここに最後の選択肢の文字列を表示します。",
					};

					const int item_x = 80;
					const int item_y = 28;

					DDFontUtils.DrawString(x + item_x, y + yStep * 0 + item_y, items[0], DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90));
					DDFontUtils.DrawString(x + item_x, y + yStep * 1 + item_y, items[1], DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90));
					DDFontUtils.DrawString(x + item_x, y + yStep * 2 + item_y, items[2], DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90));
				}

				DDEngine.EachFrame();
			}
		}

		/// <summary>
		/// 選択肢表示テスト_2
		/// </summary>
		public void Test02()
		{
			for (; ; )
			{
				DDCurtain.DrawCurtain();

				// メッセージ枠
				{
					const int h = 136;

					DDDraw.SetAlpha(0.9);
					DDDraw.DrawRect(Ground.I.Picture.MessageFrame29_Message, 0, DDConsts.Screen_H - h, DDConsts.Screen_W, h);
					DDDraw.Reset();
				}

				// 選択項目 x2 / 枠 x3
				{
					const int x = 290;
					const int y = 70;
					const int yStep = 100;

					DDDraw.DrawSimple(Ground.I.Picture.MessageFrame29_Button2, x, y + yStep * 0);
					DDDraw.DrawSimple(Ground.I.Picture.MessageFrame29_Button2, x, y + yStep * 1);
					DDDraw.DrawSimple(Ground.I.Picture.MessageFrame29_Button, x, y + yStep * 2);

					string[] items = new string[]
					{
						"ここに最初の選択肢の文字列を表示します。",
						"ここに最後の選択肢の文字列を表示します。",
					};

					const int item_x = 80;
					const int item_y = 28;

					DDFontUtils.DrawString(x + item_x, y + yStep * 0 + item_y, items[0], DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90));
					DDFontUtils.DrawString(x + item_x, y + yStep * 1 + item_y, items[1], DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90));
				}

				DDEngine.EachFrame();
			}
		}
	}
}
