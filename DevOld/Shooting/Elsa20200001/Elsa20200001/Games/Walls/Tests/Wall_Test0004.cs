using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls.Tests
{
	public class Wall_Test0004 : Wall
	{
		public override IEnumerable<bool> E_Draw()
		{
			double a = 0.0;

			for (int frame = 0; ; frame++)
			{
				DDDraw.SetAlpha(a);

				{
					int slide = (int)((frame * 3L) % 108L);

					for (int dx = -slide; dx < GameConsts.FIELD_W; dx += 108)
					{
						for (int dy = -15; dy < GameConsts.FIELD_H; dy += 108) // フィールド高 510, 108 * 5 == 540 で 30 はみ出るので 15 上にズラす。
						{
							DDDraw.DrawSimple(Ground.I.Picture.Wall0002, dx, dy);
						}
					}
				}

				{
					int slide = (int)((frame * 13L) % 90L);

					for (int dx = -slide; dx < GameConsts.FIELD_W; dx += 90)
					{
						for (int dy = -15; dy < GameConsts.FIELD_H; dy += 90) // フィールド高 510, 90 * 6 == 540 で 30 はみ出るので 15 上にズラす。
						{
							DDDraw.DrawSimple(Ground.I.Picture.Wall0003, dx, dy);
						}
					}
				}

				DDDraw.Reset();
				DDUtils.Approach(ref a, 1.0, 0.997);
				this.FilledFlag = 1.0 - SCommon.MICRO < a;
				yield return true;
			}
		}
	}
}
