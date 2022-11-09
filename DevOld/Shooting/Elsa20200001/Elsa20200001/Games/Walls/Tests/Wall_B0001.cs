using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls.Tests
{
	public class Wall_B0001 : Wall
	{
		public override IEnumerable<bool> E_Draw()
		{
			return WallCommon.E_フェードイン(this.E_Draw2());
		}

		private IEnumerable<bool> E_Draw2()
		{
			for (int slide = 0; ; slide += 19, slide %= 180)
			{
				for (int dx = -slide; dx < GameConsts.FIELD_W; dx += 180)
				{
					for (int dy = -15; dy < GameConsts.FIELD_H; dy += 180) // フィールド高 510, 180 * 3 == 540 で 30 はみ出るので 15 上にズラす。
					{
						DDDraw.DrawSimple(Ground.I.Picture.Wall0001, dx, dy);
					}
				}
				yield return true;
			}
		}
	}
}
