using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Tiles
{
	public class Tile_Brick_Front : Tile_Brick
	{
		public Tile_Brick_Front()
		{
			this.Picture_L = Ground.I.Picture.Stage02_Chip_c01;
			this.Picture_R = Ground.I.Picture.Stage02_Chip_c02;
			this.Picture_Alone = Ground.I.Picture.Stage02_Chip_c03;
		}

		public override Kind_e GetKind()
		{
			return Kind_e.WALL;
		}
	}
}
