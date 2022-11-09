using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	public class Tile_大森林 : Tile_大森林系
	{
		public Tile_大森林(DDPicture[,] pictbl, int 密集_l, int 密集_t, int 単独_l, int 単独_t, DDPicture groundPicture, DDPicture 単独セルPicture)
			: base(pictbl, 密集_l, 密集_t, 単独_l, 単独_t, groundPicture, 単独セルPicture)
		{ }

		protected override bool IsFriend(MapCell cell)
		{
			return cell.Tile is Tile_大森林 || cell.IsDefault;
		}

		public override Tile.Kind_e GetKind()
		{
			return Kind_e.WALL;
		}
	}
}
