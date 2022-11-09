using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// 標準
	/// 但し前面に描画
	/// </summary>
	public class Tile_Front : Tile
	{
		private DDPicture Picture;
		private Kind_e Kind;

		public Tile_Front(DDPicture picture, Kind_e kind)
		{
			this.Picture = picture;
			this.Kind = kind;
		}

		public override Kind_e GetKind()
		{
			return this.Kind;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDGround.EL.Add(() =>
			{
				DDDraw.DrawCenter(this.Picture, draw_x, draw_y);
				return false;
			});
		}
	}
}
