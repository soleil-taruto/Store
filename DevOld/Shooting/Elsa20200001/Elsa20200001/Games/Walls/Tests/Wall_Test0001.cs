using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls.Tests
{
	public class Wall_Test0001 : Wall
	{
		public override IEnumerable<bool> E_Draw()
		{
			double a = 0.0;

			for (int slide = 0; ; slide += 19, slide %= 180)
			{
				DDDraw.SetAlpha(a);

				for (int dx = -slide; dx < GameConsts.FIELD_W; dx += 180)
				{
					for (int dy = -15; dy < GameConsts.FIELD_H; dy += 180) // フィールド高 510, 180 * 3 == 540 で 30 はみ出るので 15 上にズラす。
					{
						DDDraw.DrawSimple(Ground.I.Picture.Wall0001, dx, dy);
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
