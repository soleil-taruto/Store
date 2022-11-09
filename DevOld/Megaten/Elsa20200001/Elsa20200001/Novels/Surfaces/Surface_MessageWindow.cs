using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_MessageWindow : Surface
	{
		public static bool Hide = false; // Novel から制御される。

		public Surface_MessageWindow(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 60000;
		}

		private double A = 1.0;

		public override IEnumerable<bool> E_Draw()
		{
			const int h = 136;

			for (; ; )
			{
				DDUtils.Approach(ref this.A, Hide ? 0.0 : 1.0, 0.9);

				DDDraw.SetAlpha(this.A);
				DDDraw.DrawRect(Ground.I.Picture.MessageFrame_Message, 0, DDConsts.Screen_H - h, DDConsts.Screen_W, h);
				DDDraw.Reset();

				if (!Hide)
				{
					// サブタイトル文字列
					{
						int dispSubtitleLength = Math.Min(Novel.I.DispSubtitleCharCount, Novel.I.CurrPage.Subtitle.Length);
						string dispSubtitle = Novel.I.CurrPage.Subtitle.Substring(0, dispSubtitleLength);

						DDFontUtils.DrawString(
							10,
							413,
							dispSubtitle,
							DDFontUtils.GetFont("Kゴシック", 16)
							);
					}

					// シナリオのテキスト文字列
					{
						int dispTextLength = Math.Min(Novel.I.DispCharCount, Novel.I.CurrPage.Text.Length);
						string dispText = Novel.I.CurrPage.Text.Substring(0, dispTextLength);
						string[] dispLines = dispText.Split('\n');

						for (int index = 0; index < dispLines.Length; index++)
						{
							DDFontUtils.DrawString(
								10,
								450 + index * 30,
								dispLines[index],
								DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90)
								);
						}
					}
				}

				yield return true;
			}
		}
	}
}
