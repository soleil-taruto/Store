using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles.Tests
{
	public class Tile_B0004 : Tile
	{
		public override Tile.Kind_e GetKind()
		{
			return Kind_e.WALL;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDDraw.DrawCenter(Ground.I.Picture.Tile_B0004, draw_x, draw_y);
		}
	}
}
