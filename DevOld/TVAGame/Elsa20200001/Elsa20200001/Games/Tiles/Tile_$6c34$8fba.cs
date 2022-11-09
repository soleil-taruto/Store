using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	public class Tile_水辺 : Tile_水辺系
	{
		public Tile_水辺(DDPicture[,] pictbl, int pictbl_l, int pictbl_t, int animeKomaNum, int animeKomaSpan)
			: base(pictbl, pictbl_l, pictbl_t, animeKomaNum, animeKomaSpan)
		{ }

		protected override bool IsFriend(MapCell cell)
		{
			return cell.Tile.GetKind() == Kind_e.RIVER || cell.IsDefault;
		}

		public override Tile.Kind_e GetKind()
		{
			return Kind_e.RIVER;
		}
	}
}
