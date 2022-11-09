using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls
{
	public class Wall_ロックマン風 : Wall
	{
		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				int x = -DDGround.ICamera.X;
				int y = -DDGround.ICamera.Y;

				x %= GameConsts.TILE_W;
				y %= GameConsts.TILE_H;

				for (int cx = x; cx < DDConsts.Screen_W; cx += GameConsts.TILE_W)
					for (int cy = y; cy < DDConsts.Screen_H; cy += GameConsts.TILE_H)
						DDDraw.DrawSimple(Ground.I.Picture.Stage01_Chip_d03, cx, cy);

				yield return true;
			}
		}
	}
}
