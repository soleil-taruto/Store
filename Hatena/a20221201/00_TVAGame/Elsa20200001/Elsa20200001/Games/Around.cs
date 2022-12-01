using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Games
{
	public class Around
	{
		/// <summary>
		/// 周辺テーブルの幅・高さ
		/// </summary>
		public int Size;

		/// <summary>
		/// 周辺テーブル
		/// 周辺のマップセルを集めたテーブル
		/// </summary>
		public MapCell[,] Table;

		/// <summary>
		/// 周辺テーブルの中心座標(ドット単位)
		/// </summary>
		public I2Point CenterPoint;

		/// <summary>
		/// 周辺テーブルの中心から指定座標までの相対座標(ドット単位)
		/// </summary>
		public I2Point RelativePoint;

		/// <summary>
		/// 周辺テーブルを作成する。
		/// </summary>
		/// <param name="x">指定座標(X-座標_ドット単位)</param>
		/// <param name="y">指定座標(Y-座標_ドット単位)</param>
		/// <param name="size">周辺テーブルの幅・高さ</param>
		public Around(int x, int y, int size)
		{
			I2Point pt = new I2Point(x, y);

			this.Size = size;
			this.Table = new MapCell[size, size];

			// 周辺テーブルの左上へ移動
			x -= (size - 1) * GameConsts.TILE_W / 2;
			y -= (size - 1) * GameConsts.TILE_H / 2;

			const int TMP_SPAN = 1000;

			// マップ座標(ドット単位) -> マップテーブル座標
			x += GameConsts.TILE_W * TMP_SPAN;
			y += GameConsts.TILE_W * TMP_SPAN;
			x /= GameConsts.TILE_W;
			y /= GameConsts.TILE_H;
			x -= TMP_SPAN;
			y -= TMP_SPAN;

			for (int xc = 0; xc < size; xc++)
				for (int yc = 0; yc < size; yc++)
					this.Table[xc, yc] = Game.I.Map.GetCell(x + xc, y + yc);

			// マップテーブル座標 -> マップ座標(ドット単位)
			x += TMP_SPAN;
			y += TMP_SPAN;
			x *= GameConsts.TILE_W;
			y *= GameConsts.TILE_H;
			x -= GameConsts.TILE_W * TMP_SPAN;
			y -= GameConsts.TILE_W * TMP_SPAN;

			// 周辺テーブルの中心へ移動
			x += size * GameConsts.TILE_W / 2;
			y += size * GameConsts.TILE_H / 2;

			this.CenterPoint = new I2Point(x, y);
			this.RelativePoint = new I2Point(pt.X - x, pt.Y - y);

			if (!SCommon.IsRange(this.RelativePoint.X, -16, 15)) ProcMain.WriteLog("RP_X: " + this.RelativePoint.X); // test
			if (!SCommon.IsRange(this.RelativePoint.Y, -16, 15)) ProcMain.WriteLog("RP_Y: " + this.RelativePoint.Y); // test
		}

		public void XTurn()
		{
			this.RelativePoint.X *= -1;

			for (int x = 0; x < this.Size / 2; x++)
				for (int y = 0; y < this.Size; y++)
					this.TableSwap(x, y, this.Size - 1 - x, y);
		}

		public void YTurn()
		{
			this.RelativePoint.Y *= -1;

			for (int x = 0; x < this.Size; x++)
				for (int y = 0; y < this.Size / 2; y++)
					this.TableSwap(x, y, x, this.Size - 1 - y);
		}

		private void TableSwap(int x1, int y1, int x2, int y2)
		{
			MapCell tmp = this.Table[x1, y1];
			this.Table[x1, y1] = this.Table[x2, y2];
			this.Table[x2, y2] = tmp;
		}
	}
}
