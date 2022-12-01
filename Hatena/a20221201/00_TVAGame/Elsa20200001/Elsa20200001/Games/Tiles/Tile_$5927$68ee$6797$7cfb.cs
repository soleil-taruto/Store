using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tiles
{
	public abstract class Tile_大森林系 : Tile
	{
		private DrawTask _draw;

		/// <summary>
		/// 森林系のタイル(2x2倍)を生成する。
		/// </summary>
		/// <param name="pictbl">画像テーブル(画像はタイル(32x32)であること)</param>
		/// <param name="密集_l">「密集」画像の左上(X-座標)</param>
		/// <param name="密集_t">「密集」画像の左上(Y-座標)</param>
		/// <param name="単独_l">「単独」画像の左上(X-座標)</param>
		/// <param name="単独_t">「単独」画像の左上(Y-座標)</param>
		/// <param name="groundPicture">地面の画像</param>
		/// <param name="単独セルPicture">密集・単独を使用できないマップセル用の画像</param>
		public Tile_大森林系(DDPicture[,] pictbl, int 密集_l, int 密集_t, int 単独_l, int 単独_t, DDPicture groundPicture, DDPicture 単独セルPicture)
		{
			_draw = new DrawTask()
			{
				PictureTable = pictbl,
				単独_L = 単独_l,
				単独_T = 単独_t,
				密集_L = 密集_l,
				密集_T = 密集_t,
				GroundPicture = groundPicture,
				単独セルPicture = 単独セルPicture,
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
			public int 単独_L;
			public int 単独_T;
			public int 密集_L;
			public int 密集_T;
			public DDPicture GroundPicture;
			public DDPicture 単独セルPicture;
			public Predicate<MapCell> F_IsFriend;

			// <---- prm

			public D2Point DrawPoint;
			public I2Point MapTablePoint;

			// <---- Execute() prm

			public override IEnumerable<bool> E_Task()
			{
				for (; ; )
				{
					// 密集画像テーブルの位置
					bool 左列 = this.MapTablePoint.X / 2 % 2 == 0;
					bool 上段 = this.MapTablePoint.Y / 2 % 2 == 0;

					DDPicture picture;

					// 密集画像テーブルのどの位置に対応するか
					if (左列)
					{
						if (上段) // 左上
						{
							bool m左下 = this.IsFriend4x4(-1, 0);
							bool m右上 = this.IsFriend4x4(0, -1);

							if (m左下)
							{
								if (m右上)
									picture = this.PictureTable[this.密集_L + 0, this.密集_T + 0];
								else
									picture = this.PictureTable[this.単独_L + 1, this.単独_T + 0];
							}
							else
							{
								if (m右上)
									picture = this.PictureTable[this.単独_L + 0, this.単独_T + 1];
								else
									picture = null;
							}
						}
						else // 左下
						{
							bool m左上 = this.IsFriend4x4(-1, -1);
							bool m右下 = this.IsFriend4x4(0, 0);

							if (m左上)
							{
								if (m右下)
									picture = this.PictureTable[this.密集_L + 0, this.密集_T + 1];
								else
									picture = this.PictureTable[this.単独_L + 1, this.単独_T + 1];
							}
							else
							{
								if (m右下)
									picture = this.PictureTable[this.単独_L + 0, this.単独_T + 0];
								else
									picture = null;
							}
						}
					}
					else // 右列
					{
						if (上段) // 右上
						{
							bool m左上 = this.IsFriend4x4(-1, -1);
							bool m右下 = this.IsFriend4x4(0, 0);

							if (m左上)
							{
								if (m右下)
									picture = this.PictureTable[this.密集_L + 1, this.密集_T + 0];
								else
									picture = this.PictureTable[this.単独_L + 1, this.単独_T + 1];
							}
							else
							{
								if (m右下)
									picture = this.PictureTable[this.単独_L + 0, this.単独_T + 0];
								else
									picture = null;
							}
						}
						else // 右下
						{
							bool m左下 = this.IsFriend4x4(-1, 0);
							bool m右上 = this.IsFriend4x4(0, -1);

							if (m左下)
							{
								if (m右上)
									picture = this.PictureTable[this.密集_L + 1, this.密集_T + 1];
								else
									picture = this.PictureTable[this.単独_L + 1, this.単独_T + 0];
							}
							else
							{
								if (m右上)
									picture = this.PictureTable[this.単独_L + 0, this.単独_T + 1];
								else
									picture = null;
							}
						}
					}

					DDDraw.DrawCenter(this.GroundPicture, this.DrawPoint.X, this.DrawPoint.Y);

					if (picture == null)
					{
						DDDraw.DrawCenter(this.単独セルPicture, this.DrawPoint.X, this.DrawPoint.Y);
					}
					else
					{
						bool inner_左列 = this.MapTablePoint.X % 2 == 0;
						bool inner_上段 = this.MapTablePoint.Y % 2 == 0;

						if (!inner_左列 && !inner_上段) // 右下のみ描画する。-- マップは左上から描画するので、右下で描画しないと他のセルに上書きされる。
						{
							DDDraw.SetMosaic();
							DDDraw.DrawBegin(picture, this.DrawPoint.X - GameConsts.TILE_W / 2, this.DrawPoint.Y - GameConsts.TILE_H / 2);
							DDDraw.DrawZoom(2.0);
							DDDraw.DrawEnd();
							DDDraw.Reset();
						}
					}

					yield return true;
				}
			}

			private bool IsFriend4x4(int rel_l, int rel_t)
			{
				int l = (this.MapTablePoint.X / 2 + rel_l) * 2;
				int t = (this.MapTablePoint.Y / 2 + rel_t) * 2;

				for (int x = 0; x < 4; x++)
					for (int y = 0; y < 4; y++)
						if (!this.IsFriend(l + x, t + y))
							return false;

				return true;
			}

			private bool IsFriend(int x, int y)
			{
				return this.F_IsFriend(Game.I.Map.GetCell(x, y));
			}
		}
	}
}
