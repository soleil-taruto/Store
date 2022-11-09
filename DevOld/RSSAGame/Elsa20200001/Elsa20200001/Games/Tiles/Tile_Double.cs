using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// 標準
	/// 但し画像を2つ重ねて描画する。
	/// </summary>
	public class Tile_Double : Tile
	{
		private DDPicture Picture_01;
		private DDPicture Picture_02;
		private Kind_e Kind;

		public Tile_Double(DDPicture picture_01, DDPicture picture_02, Kind_e kind)
		{
			this.Picture_01 = picture_01;
			this.Picture_02 = picture_02;
			this.Kind = kind;
		}

		public override Kind_e GetKind()
		{
			return this.Kind;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDDraw.DrawCenter(this.Picture_01, draw_x, draw_y);
			DDDraw.DrawCenter(this.Picture_02, draw_x, draw_y);
		}
	}
}
