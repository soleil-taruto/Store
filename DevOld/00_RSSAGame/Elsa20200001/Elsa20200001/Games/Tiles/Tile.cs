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
		public enum Kind_e
		{
			SPACE, // 空間
			LADDER, // 梯子
			CLOUD, // 上に乗れる雲
			WALL, // 壁
		}

		public static string[] Kind_e_Names = new string[]
		{
			"SPACE",
			"LADDER",
			"CLOUD",
			"WALL",
		};

		/// <summary>
		/// このタイルの種類を取得する。
		/// </summary>
		/// <returns>このタイルの種類</returns>
		public abstract Kind_e GetKind();

		public bool IsWall()
		{
			return this.GetKind() == Kind_e.WALL;
		}

		public bool IsGround()
		{
			Kind_e kind = this.GetKind();

			return
				kind == Kind_e.LADDER ||
				kind == Kind_e.CLOUD ||
				kind == Kind_e.WALL;
		}

		/// <summary>
		/// このタイルを描画する。
		/// タイルが画面内に入り込んだときのみ実行される。
		/// 座標はタイルの中心座標 且つ 画面の座標(画面左上を0,0とする)
		/// -- マップセルを埋めるには LTWH (x - Consts.TILE_W / 2, y - Consts.TILE_H / 2, Consts.TILE_W, Consts.TILE_H) の領域に描画すれば良い。
		/// </summary>
		/// <param name="draw_x">タイルの中心座標(X-座標,ピクセル単位)</param>
		/// <param name="draw_y">タイルの中心座標(Y-座標,ピクセル単位)</param>
		/// <param name="map_x">マップ上の座標(X-座標,テーブル座標)</param>
		/// <param name="map_y">マップ上の座標(Y-座標,テーブル座標)</param>
		public abstract void Draw(double draw_x, double draw_y, int map_x, int map_y);
	}
}
