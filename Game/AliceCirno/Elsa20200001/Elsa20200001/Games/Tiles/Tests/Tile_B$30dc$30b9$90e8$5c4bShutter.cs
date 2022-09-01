using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Tiles.Tests
{
	public class Tile_Bボス部屋Shutter : Tile
	{
		public override Kind_e GetKind()
		{
			return Kind_e.WALL;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDDraw.DrawBegin(Ground.I.Picture.Dummy, draw_x, draw_y);
			DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
			DDDraw.DrawEnd();

			DDPrint.SetBorder(new I3Color(0, 0, 0));
			DDPrint.SetDebug((int)draw_x, (int)draw_y);
			DDPrint.Print("扉");
			DDPrint.Reset();
		}
	}
}
