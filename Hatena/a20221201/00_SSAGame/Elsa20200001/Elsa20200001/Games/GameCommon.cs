using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;
using Charlotte.Games.Tiles;

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

			string mapName = mapFile.Replace('\\', '/');
			return mapName;
		}

		/// <summary>
		/// 同じマップ名であるか判定する。
		/// </summary>
		/// <param name="mapName1">マップ名1</param>
		/// <param name="mapName2">マップ名2</param>
		/// <returns>同じマップ名か</returns>
		public static bool IsSameMapName(string mapName1, string mapName2)
		{
			mapName1 = mapName1.Replace('\\', '/');
			mapName2 = mapName2.Replace('\\', '/');

			return mapName1 == mapName2;
		}

		/// <summary>
		/// マップ上の座標(ドット単位)からマップセルの座標(テーブル・インデックス)を取得する。
		/// </summary>
		/// <param name="pt">マップ上の座標(ドット単位)</param>
		/// <returns>マップセルの座標(テーブル・インデックス)</returns>
		public static I2Point ToTablePoint(D2Point pt)
		{
			return ToTablePoint(pt.X, pt.Y);
		}

		/// <summary>
		/// マップ上の座標(ドット単位)からマップセルの座標(テーブル・インデックス)を取得する。
		/// </summary>
		/// <param name="x">マップ上の X-座標(ドット単位)</param>
		/// <param name="y">マップ上の Y-座標(ドット単位)</param>
		/// <returns>マップセルの座標(テーブル・インデックス)</returns>
		public static I2Point ToTablePoint(double x, double y)
		{
			return new I2Point(
				(int)Math.Floor(x / GameConsts.TILE_W),
				(int)Math.Floor(y / GameConsts.TILE_H)
				);
		}

		/// <summary>
		/// マップセルの座標(テーブル・インデックス)からマップ上の座標(ドット単位)を取得する。
		/// 戻り値は、マップセルの中心座標である。
		/// </summary>
		/// <param name="pt">マップセルの座標(テーブル・インデックス)</param>
		/// <returns>マップ上の座標(ドット単位)</returns>
		public static D2Point ToFieldPoint(I2Point pt)
		{
			return ToFieldPoint(pt.X, pt.Y);
		}

		/// <summary>
		/// マップセルの座標(テーブル・インデックス)からマップ上の座標(ドット単位)を取得する。
		/// 戻り値は、マップセルの中心座標である。
		/// </summary>
		/// <param name="x">マップセルの X-座標(テーブル・インデックス)</param>
		/// <param name="y">マップセルの Y-座標(テーブル・インデックス)</param>
		/// <returns>マップ上の座標(ドット単位)</returns>
		public static D2Point ToFieldPoint(int x, int y)
		{
			return new D2Point(
				(double)(x * GameConsts.TILE_W + GameConsts.TILE_W / 2.0),
				(double)(y * GameConsts.TILE_H + GameConsts.TILE_H / 2.0)
				);
		}

		/// <summary>
		/// マップ上の X-座標(ドット単位)からマップセルの中心 X-座標(ドット単位)を取得する。
		/// </summary>
		/// <param name="x">マップ上の X-座標(ドット単位)</param>
		/// <returns>マップセルの中心 X-座標(ドット単位)</returns>
		public static double ToTileCenterX(double x)
		{
			return ToTileCenter(new D2Point(x, 0.0)).X;
		}

		/// <summary>
		/// マップ上の Y-座標(ドット単位)からマップセルの中心 Y-座標(ドット単位)を取得する。
		/// </summary>
		/// <param name="y">マップ上の Y-座標(ドット単位)</param>
		/// <returns>マップセルの中心 Y-座標(ドット単位)</returns>
		public static double ToTileCenterY(double y)
		{
			return ToTileCenter(new D2Point(0.0, y)).Y;
		}

		/// <summary>
		/// マップ上の座標(ドット単位)からマップセルの中心座標(ドット単位)を取得する。
		/// </summary>
		/// <param name="pt">マップ上の座標(ドット単位)</param>
		/// <returns>マップセルの中心座標(ドット単位)</returns>
		public static D2Point ToTileCenter(D2Point pt)
		{
			return ToFieldPoint(ToTablePoint(pt));
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

		// ===========================
		// ==== Map 関連 (ここまで) ====
		// ===========================

		public static void SaveGame()
		{
			GameStatus gameStatus = Game.I.Status.GetClone();

			// ★★★★★ *****PSH (<-このパターンで検索できるようにしておく)
			// プレイヤー・ステータス反映(セーブ時)
			// その他の反映箇所：
			// -- マップ入場時
			// -- マップ退場時
			{
				// すべきこと：
				// -- 各方面に展開されているゲーム状態を gameStatus に反映・格納する。

				// 例：
				//gameStatus.StartChara = Game.I.Player.Chara;
				//gameStatus.StartHP = Game.I.Player.HP;
				//gameStatus.StartFacingLeft = Game.I.Player.FacingLeft;
				// --

				gameStatus.StartChara = Game.I.Player.Chara;
				gameStatus.StartHP = Game.I.Player.HP;
				gameStatus.StartFacingLeft = Game.I.Player.FacingLeft;
			}

			SaveGame(gameStatus);
		}

		public static void SaveGame(GameStatus gameStatus)
		{
			SaveGame_幕間();

			DDEngine.FreezeInput();

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			SimpleMenu simpleMenu = new SimpleMenu()
			{
				BorderColor = new I3Color(0, 128, 0),
				WallDrawer = () =>
				{
					DDDraw.SetBright(new I3Color(128, 64, 0));
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();
				},
			};

			int selectIndex = 0;

			for (; ; )
			{
				// セーブしたら戻ってくるので、毎回更新する。
				string[] items = Ground.I.SaveDataSlots.Select(v => v.GameStatus == null ?
					"----" :
					"[" + v.TimeStamp + "]　" + v.Description).Concat(new string[] { "戻る" }).ToArray();

				selectIndex = simpleMenu.Perform(selectIndex, 18, 18, 32, 24, "セーブ", items);

				if (selectIndex < GameConsts.SAVE_DATA_SLOT_NUM)
				{
					if (new Confirm()
					{
						BorderColor =
							Ground.I.SaveDataSlots[selectIndex].GameStatus != null ?
							new I3Color(200, 0, 0) :
							new I3Color(100, 100, 0)
					}
					.Perform(
						Ground.I.SaveDataSlots[selectIndex].GameStatus != null ?
						"スロット " + (selectIndex + 1) + " のデータを上書きします。" :
						"スロット " + (selectIndex + 1) + " にセーブします。", "はい", "いいえ") == 0)
					{
						Ground.P_SaveDataSlot saveDataSlot = Ground.I.SaveDataSlots[selectIndex];

						saveDataSlot.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd (ddd) HH:mm:ss");
						saveDataSlot.Description = "＠～＠～＠";
						saveDataSlot.MapName = GameCommon.GetMapName(Game.I.Map.MapFile, "Tests/t0001");
						saveDataSlot.GameStatus = gameStatus;
					}
				}
				else // [戻る]
				{
					break;
				}
				//DDEngine.EachFrame(); // 不要
			}

			SaveGame_幕間();

			DDEngine.FreezeInput();
		}

		private static void SaveGame_幕間()
		{
			const int METER_W = DDConsts.Screen_W - 100;
			const int METER_H = 10;
			const int METER_L = (DDConsts.Screen_W - METER_W) / 2;
			const int METER_T = (DDConsts.Screen_H - METER_H) / 2;

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				DDDraw.SetBright(new I3Color(64, 32, 0));
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
				DDDraw.Reset();

				DDDraw.SetBright(new I3Color(0, 0, 0));
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, METER_L, METER_T, METER_W, METER_H);
				DDDraw.Reset();

				DDDraw.SetBright(new I3Color(255, 255, 255));
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, METER_L, METER_T, Math.Max(METER_W * scene.Rate, 1), METER_H);
				DDDraw.Reset();

				DDEngine.EachFrame();
			}
		}
	}
}
