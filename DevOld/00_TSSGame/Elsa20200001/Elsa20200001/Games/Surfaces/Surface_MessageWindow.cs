using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Surfaces
{
	public class Surface_MessageWindow : Surface
	{
		public static bool Hide = false; // Game から制御される。

		public Surface_MessageWindow(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 60000;
		}

		private double A = Ground.I.MessageWindow_A_Pct / 100.0;

		public override IEnumerable<bool> E_Draw()
		{
			const int h = 272;

			for (; ; )
			{
				DDUtils.Approach(ref this.A, Hide ? 0.0 : Ground.I.MessageWindow_A_Pct / 100.0, 0.9);

				DDDraw.SetAlpha(this.A);
				DDDraw.DrawRect(Ground.I.Picture.MessageFrame29_Message, 0, DDConsts.Screen_H - h, DDConsts.Screen_W, h);
				DDDraw.Reset();

				if (!Hide)
				{
					// サブタイトル文字列
					{
						int dispSubtitleLength = Math.Min(Game.I.DispSubtitleCharCount, Game.I.CurrPage.Subtitle.Length);
						string dispSubtitle = Game.I.CurrPage.Subtitle.Substring(0, dispSubtitleLength);

						DDFontUtils.DrawString(
							20,
							826,
							dispSubtitle,
							DDFontUtils.GetFont("Kゴシック", 32)
							);
					}

					// シナリオのテキスト文字列
					{
						int dispTextLength = Math.Min(Game.I.DispCharCount, Game.I.CurrPage.Text.Length);
						string dispText = Game.I.CurrPage.Text.Substring(0, dispTextLength);
						string[] dispLines = dispText.Split('\n');

						for (int index = 0; index < dispLines.Length; index++)
						{
							DDFontUtils.DrawString(
								20,
								900 + index * 60,
								dispLines[index],
								DDFontUtils.GetFont("Kゴシック", 32), false, new I3Color(110, 100, 90)
								);
						}
					}
				}

				yield return true;
			}
		}
	}
}
