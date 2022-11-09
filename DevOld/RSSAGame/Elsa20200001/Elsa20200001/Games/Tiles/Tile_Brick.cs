using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	public abstract class Tile_Brick : Tile
	{
		protected DDPicture Picture_L;
		protected DDPicture Picture_R;
		protected DDPicture Picture_Alone;

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDDraw.DrawCenter(this.GetPicture(map_x, map_y), draw_x, draw_y);
		}

		private DDPicture GetPicture(int map_x, int map_y)
		{
			if ((map_x + map_y) % 2 == 0)
			{
				if (Game.I.Map.GetCell(map_x + 1, map_y).Tile is Tile_Brick)
				{
					return this.Picture_L;
				}
			}
			else
			{
				if (Game.I.Map.GetCell(map_x - 1, map_y).Tile is Tile_Brick)
				{
					return this.Picture_R;
				}
			}
			return this.Picture_Alone;
		}
	}
}
