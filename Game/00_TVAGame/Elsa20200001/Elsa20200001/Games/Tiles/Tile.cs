using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// タイル
	/// マップセルの視覚的・物理的な構成要素
	/// </summary>
	public abstract class Tile
	{
		/// <summary>
		/// タイルの種類
		/// </summary>
		public enum Kind_e
		{
			/// <summary>
			/// 壁
			/// プレイヤー侵入不可
			/// 自弾侵入不可
			/// </summary>
			WALL,

			/// <summary>
			/// 川などの水域
			/// プレイヤー侵入不可
			/// 自弾侵入可能
			/// </summary>
			RIVER,

			/// <summary>
			/// 地面・通路
			/// プレイヤー侵入可能
			/// 自弾侵入可能
			/// </summary>
			SPACE,
		}

		/// <summary>
		/// このタイルの種類を得る。
		/// </summary>
		/// <returns>このタイルの種類</returns>
		public abstract Kind_e GetKind();

		/// <summary>
		/// このタイルを描画する。
		/// タイルが画面内に入り込んだときのみ実行される。
		/// 座標はタイルの中心座標 且つ 画面の座標(画面左上を0,0とする)
		/// -- マップセルを埋めるには LTWH (x - Consts.TILE_W / 2, y - Consts.TILE_H / 2, Consts.TILE_W, Consts.TILE_H) の領域に描画すれば良い。
		/// </summary>
		/// <param name="draw_x">タイルの中心座標(X-軸)</param>
		/// <param name="draw_y">タイルの中心座標(Y-軸)</param>
		public abstract void Draw(double draw_x, double draw_y, int map_x, int map_y);
	}
}
