using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// 壁
	/// </summary>
	public class Tile_Wall : Tile
	{
		private DDPicture Picture;

		public Tile_Wall(DDPicture picture)
		{
			this.Picture = picture;
		}

		public override Tile.Kind_e GetKind()
		{
			return Kind_e.WALL;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDDraw.DrawCenter(this.Picture, draw_x, draw_y);
		}
	}
}
