using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles.Tests
{
	public class Tile_B0004 : Tile
	{
		public override bool IsWall()
		{
			return true;
		}

		public override void Draw(double x, double y)
		{
			DDDraw.DrawCenter(Ground.I.Picture.Tile_B0004, x, y);
		}
	}
}
