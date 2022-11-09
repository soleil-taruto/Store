using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games;
using Charlotte.Games.Tiles;
using System.IO;

namespace Charlotte.Games
{
	public static class GameCommon
	{
		// ==================
		// ==== Map 関連 ====
		// ==================

		public const string MAP_FILE_PREFIX = @"res\World\Map\";
		public const string MAP_FILE_SUFFIX = ".txt";

		/// <summary>
		/// マップ名からマップファイル名を得る。
		/// </summary>
		/// <param name="mapName">マップ名</param>
		/// <returns>マップファイル名</returns>
		public static string GetMapFile(string mapName)
		{
			return MAP_FILE_PREFIX + mapName.Replace('/', '\\') + MAP_FILE_SUFFIX;
		}

		/// <summary>
		/// マップファイル名からマップ名を得る。
		/// 失敗すると、デフォルトのマップ名を返す。
		/// </summary>
		/// <param name="mapFile">マップファイル名</param>
		/// <param name="defval">デフォルトのマップ名</param>
		/// <returns>マップ名</returns>
		public static string GetMapName(string mapFile, string defval)
		{
			if (!SCommon.StartsWithIgnoreCase(mapFile, MAP_FILE_PREFIX))
				return defval;

			mapFile = mapFile.Substring(MAP_FILE_PREFIX.Length);

			if (!SCommon.EndsWithIgnoreCase(mapFile, MAP_FILE_SUFFIX))
				return defval;

			mapFile = mapFile.Substring(0, mapFile.Length - MAP_FILE_SUFFIX.Length);

			if (mapFile == "")
				return defval;

			return mapFile; // as mapName
		}

		/// <summary>
		/// マップの(ドット単位の)座標からマップセルの座標を得る。
		/// </summary>
		/// <param name="pt">マップの(ドット単位の)座標</param>
		/// <returns>マップセルの座標</returns>
		public static I2Point ToTablePoint(D2Point pt)
		{
			return ToTablePoint(pt.X, pt.Y);
		}

		/// <summary>
		/// マップの(ドット単位の)座標からマップセルの座標を得る。
		/// </summary>
		/// <param name="x">マップの(ドット単位の)X_座標</param>
		/// <param name="y">マップの(ドット単位の)Y_座標</param>
		/// <returns>マップセルの座標</returns>
		public static I2Point ToTablePoint(double x, double y)
		{
			return new I2Point(
				(int)(x / GameConsts.TILE_W),
				(int)(y / GameConsts.TILE_H)
				);
		}

		private static MapCell _defaultMapCell = null;

		/// <summary>
		/// デフォルトのマップセル
		/// マップ外を埋め尽くすマップセル
		/// デフォルトのマップセルは複数設置し得るため
		/// -- cell の判定には cell == DefaultMapCell ではなく cell.IsDefault を使用すること。
		/// </summary>
		public static MapCell DefaultMapCell
		{
			get
			{
				if (_defaultMapCell == null)
				{
					_defaultMapCell = new MapCell()
					{
						TileName = GameConsts.TILE_NONE,
						Tile = new Tile_None(),
						EnemyName = GameConsts.ENEMY_NONE,
					};
				}
				return _defaultMapCell;
			}
		}

		private static MapCell _defaultMapCell_ladder = null;

		/// <summary>
		/// デフォルトのマップセル(梯子)
		/// </summary>
		public static MapCell DefaultMapCell_Ladder
		{
			get
			{
				if (_defaultMapCell_ladder == null)
				{
					_defaultMapCell_ladder = new MapCell()
					{
						TileName = GameConsts.TILE_NONE,
						Tile = new Tile_Ladder(),
						EnemyName = GameConsts.ENEMY_NONE,
					};
				}
				return _defaultMapCell_ladder;
			}
		}

		// ===========================
		// ==== Map 関連 (ここまで) ====
		// ===========================

		public static bool 壁処理(ref double x, ref double y, D2Point[] crashPts)
		{
			D2Point pt = new D2Point(x, y);

			bool ret = 壁処理(ref pt, crashPts);

			x = pt.X;
			y = pt.Y;

			return ret;
		}

		/// <summary>
		/// 物体の壁へのめり込みを解消する。
		/// </summary>
		/// <param name="pt">物体の位置</param>
		/// <param name="crashPts">当たり判定(キャラクタ位置からの相対座標)</param>
		/// <returns>めり込んでいたか</returns>
		public static bool 壁処理(ref D2Point pt, D2Point[] crashPts)
		{
			bool ret = false;

			foreach (D2Point crashPt in crashPts)
			{
				I2Point cellPos = GameCommon.ToTablePoint(pt + crashPt);

				if (Game.I.Map.GetCell(cellPos).Tile.IsWall())
				{
					if (Math.Abs(crashPt.X) < Math.Abs(crashPt.Y)) // ? Y方向に遠い -> Y位置を調整
					{
						if (crashPt.Y < 0.0) // ? 中心より上 -> 下へ動かす。
						{
							pt.Y = (cellPos.Y + 1) * GameConsts.TILE_H - crashPt.Y;
						}
						else // ? 中心より下 -> 上へ動かす。
						{
							pt.Y = cellPos.Y * GameConsts.TILE_H - crashPt.Y;
						}
					}
					else // ? X方向に遠い -> X位置を調整
					{
						if (crashPt.X < 0.0) // ? 中心より左 -> 右へ動かす。
						{
							pt.X = (cellPos.X + 1) * GameConsts.TILE_W - crashPt.X;
						}
						else // ? 中心より右 -> 左へ動かす。
						{
							pt.X = cellPos.X * GameConsts.TILE_W - crashPt.X;
						}
					}
					ret = true;
				}
			}
			return ret;
		}

		public static int ShotChargePCTToLevel(int pct)
		{
			if (pct < 33)
				return 1;

			if (pct < 66)
				return 2;

			if (pct < 100)
				return 3;

			return 4;
		}
	}
}
