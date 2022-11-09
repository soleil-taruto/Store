using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	public class Tile_Waterfall_Ground : Tile
	{
		/// <summary>
		/// Ground.I.Picture.Stage02_Chip_f0x の [0]～[3] を想定する。
		/// </summary>
		private DDPicture[] Pictures;

		/// <summary>
		/// 以下を想定する。
		/// -- Ground.I.Picture.Stage01_Chip_e03
		/// </summary>
		private DDPicture WallPicture;

		public Tile_Waterfall_Ground(DDPicture[] pictures, DDPicture wallPicture)
		{
			this.Pictures = pictures;
			this.WallPicture = wallPicture;
		}

		public override Kind_e GetKind()
		{
			return Kind_e.WALL;
		}

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			DDGround.EL.Add(() =>
			{
				DDDraw.DrawCenter(this.WallPicture, draw_x, draw_y);
				DDDraw.SetAlpha(0.5);
				DDDraw.DrawCenter(this.Pictures[DDEngine.ProcFrame / 4 % 4], draw_x, draw_y);
				DDDraw.Reset();
				return false;
			});
		}
	}
}
