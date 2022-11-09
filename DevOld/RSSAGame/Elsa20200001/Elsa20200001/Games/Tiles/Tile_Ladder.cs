using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// 梯子
	/// </summary>
	public class Tile_Ladder : Tile
	{
		public override Tile.Kind_e GetKind()
		{
			return Kind_e.LADDER;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDDraw.DrawCenter(Ground.I.Picture.Stage02_Chip_d01, draw_x, draw_y);
		}
	}
}
