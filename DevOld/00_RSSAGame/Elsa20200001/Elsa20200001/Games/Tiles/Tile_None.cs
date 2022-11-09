using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// 何もない空間
	/// </summary>
	public class Tile_None : Tile
	{
		public override Tile.Kind_e GetKind()
		{
			return Kind_e.SPACE;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			// noop
		}
	}
}
