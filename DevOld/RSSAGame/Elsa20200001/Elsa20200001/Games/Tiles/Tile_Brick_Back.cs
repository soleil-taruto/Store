using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Tiles
{
	public class Tile_Brick_Back : Tile_Brick
	{
		public Tile_Brick_Back()
		{
			this.Picture_L = Ground.I.Picture.Stage02_Bg_Chip_a01;
			this.Picture_R = Ground.I.Picture.Stage02_Bg_Chip_a02;
			this.Picture_Alone = Ground.I.Picture.Stage02_Bg_Chip_a03;
		}

		public override Kind_e GetKind()
		{
			return Kind_e.SPACE;
		}
	}
}
