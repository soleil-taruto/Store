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
		public override bool IsWall()
		{
			return false;
		}

		public override void Draw(double x, double y)
		{
			// noop
		}
	}
}
