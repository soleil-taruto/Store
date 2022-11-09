using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;
using Charlotte.Games.Tiles.Tests;

namespace Charlotte.Games.Enemies.Tests.神奈子s
{
	public class Enemy_B神奈子9902 : Enemy
	{
		public Enemy_B神奈子9902()
			: base(0, 0, 0, 0, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			return this.E_開放();
		}

		private IEnumerable<bool> E_開放()
		{
			// ---- game_制御 ----

			DDMusicUtils.Fadeout();

			// ----

			Game.I.UserInputDisabled = true;

			for (int x = 0; x < Game.I.Map.W; x++)
			{
				this.開放(x, 0);
				this.開放(x, Game.I.Map.H - 1);

				yield return true;
			}
			for (int y = 0; y < Game.I.Map.H; y++)
			{
				this.開放(0, y);
				this.開放(Game.I.Map.W - 1, 0);

				yield return true;
			}
			Game.I.UserInputDisabled = false;
		}

		private void 開放(int x, int y)
		{
			MapCell cell = Game.I.Map.GetCell(x, y);

			if (cell.Tile is Tile_Bボス部屋Shutter)
				cell.Tile = TileCatalog.Create("芝"); // restore

			DDGround.EL.Add(SCommon.Supplier(Effects.B閉鎖と開放(x * GameConsts.TILE_W + GameConsts.TILE_W / 2, y * GameConsts.TILE_H + GameConsts.TILE_H / 2)));
		}
	}
}
