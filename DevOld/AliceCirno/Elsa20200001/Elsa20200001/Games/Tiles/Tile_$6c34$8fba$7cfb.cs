using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Tiles
{
	public abstract class Tile_水辺系 : Tile
	{
		private DrawTask _draw;

		/// <summary>
		/// 水辺系のタイルを生成する。
		/// </summary>
		/// <param name="pictbl">画像テーブル(画像はミニタイル(16x16)であること)</param>
		/// <param name="pictbl_l">使用する画像の左上(X-座標)</param>
		/// <param name="pictbl_t">使用する画像の左上(Y-座標)</param>
		/// <param name="animeKomaNum">アニメーションのコマ数(1～)</param>
		/// <param name="animeKomaCycle">アニメーションの1コマの表示時間(0～,フレーム数)</param>
		public Tile_水辺系(DDPicture[,] pictbl, int pictbl_l, int pictbl_t, int animeKomaNum, int animeKomaCycle)
		{
			_draw = new DrawTask()
			{
				PictureTable = pictbl,
				PictureTable_L = pictbl_l,
				PictureTable_T = pictbl_t,
				AnimeKomaNum = animeKomaNum,
				AnimeKomaCycle = animeKomaCycle,
				F_IsFriend = this.IsFriend,
			};
		}

		protected abstract bool IsFriend(MapCell cell);

		public override void Draw(double draw_x, double draw_y, int map_x, int map_y)
		{
			_draw.DrawPoint = new D2Point(draw_x, draw_y);
			_draw.MapTablePoint = new I2Point(map_x, map_y);

			if (!_draw.Task())
				throw null; // never
		}

		private class DrawTask : DDTask
		{
			public DDPicture[,] PictureTable;
			public int PictureTable_L;
			public int PictureTable_T;
			public int AnimeKomaNum;
			public int AnimeKomaCycle;
			public Predicate<MapCell> F_IsFriend;

			// <---- prm

			public D2Point DrawPoint;
			public I2Point MapTablePoint;

			// <---- Execute() prm

			public override IEnumerable<bool> E_Task()
			{
				for (; ; )
					for (int c = 0; c < this.AnimeKomaNum; c++)
						foreach (DDScene scene in DDSceneUtils.Create(this.AnimeKomaCycle))
							yield return this.Draw_KBKFA(c, (c + 1) % this.AnimeKomaNum, scene.Rate);
			}

			private bool Draw_KBKFA(int komaBack, int komaFront, double komaFrontAlpha)
			{
				bool friend_4 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X - 1, this.MapTablePoint.Y));
				bool friend_6 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X + 1, this.MapTablePoint.Y));
				bool friend_8 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X, this.MapTablePoint.Y - 1));
				bool friend_2 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X, this.MapTablePoint.Y + 1));

				bool friend_1 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X - 1, this.MapTablePoint.Y + 1));
				bool friend_3 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X + 1, this.MapTablePoint.Y + 1));
				bool friend_7 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X - 1, this.MapTablePoint.Y - 1));
				bool friend_9 = this.F_IsFriend(Game.I.Map.GetCell(this.MapTablePoint.X + 1, this.MapTablePoint.Y - 1));

				I2Point lt;
				I2Point rt;
				I2Point lb;
				I2Point rb;

				if (friend_4 && friend_8)
					lt = friend_7 ? new I2Point(2, 4) : new I2Point(2, 0);
				else if (friend_4)
					lt = new I2Point(2, 2);
				else if (friend_8)
					lt = new I2Point(0, 4);
				else
					lt = new I2Point(0, 2);

				if (friend_6 && friend_8)
					rt = friend_9 ? new I2Point(1, 4) : new I2Point(3, 0);
				else if (friend_6)
					rt = new I2Point(1, 2);
				else if (friend_8)
					rt = new I2Point(3, 4);
				else
					rt = new I2Point(3, 2);

				if (friend_4 && friend_2)
					lb = friend_1 ? new I2Point(2, 3) : new I2Point(2, 1);
				else if (friend_4)
					lb = new I2Point(2, 5);
				else if (friend_2)
					lb = new I2Point(0, 3);
				else
					lb = new I2Point(0, 5);

				if (friend_6 && friend_2)
					rb = friend_3 ? new I2Point(1, 3) : new I2Point(3, 1);
				else if (friend_6)
					rb = new I2Point(1, 5);
				else if (friend_2)
					rb = new I2Point(3, 3);
				else
					rb = new I2Point(3, 5);

				foreach (var info in new[]
				{
					new { Pt = lt, Dir = new { X = -1, Y = -1 } },
					new { Pt = rt, Dir = new { X =  1, Y = -1 } },
					new { Pt = lb, Dir = new { X = -1, Y =  1 } },
					new { Pt = rb, Dir = new { X =  1, Y =  1 } },
				})
				{
					DDDraw.DrawCenter(
						this.PictureTable[this.PictureTable_L + info.Pt.X + komaBack * 4, this.PictureTable_T + info.Pt.Y],
						this.DrawPoint.X + info.Dir.X * GameConsts.MINI_TILE_W / 2,
						this.DrawPoint.Y + info.Dir.Y * GameConsts.MINI_TILE_H / 2
						);
					DDDraw.SetAlpha(komaFrontAlpha);
					DDDraw.DrawCenter(
						this.PictureTable[this.PictureTable_L + info.Pt.X + komaFront * 4, this.PictureTable_T + info.Pt.Y],
						this.DrawPoint.X + info.Dir.X * GameConsts.MINI_TILE_W / 2,
						this.DrawPoint.Y + info.Dir.Y * GameConsts.MINI_TILE_H / 2
						);
					DDDraw.Reset();
				}
				return true;
			}
		}
	}
}
