using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games.Tiles;
using Charlotte.GameCommons;

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

		// ===========================
		// ==== Map 関連 (ここまで) ====
		// ===========================

		public static void SaveGame()
		{
			GameStatus gameStatus = Game.I.Status.GetClone();

			// ★★★★★
			// プレイヤー・ステータス反映(セーブ時)
			// その他の反映箇所：
			// -- マップ入場時
			// -- マップ退場時
			{
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

			DDSimpleMenu simpleMenu = new DDSimpleMenu()
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

				selectIndex = simpleMenu.Perform(18, 18, 32, 24, "セーブ", items, selectIndex);

				if (selectIndex < Consts.SAVE_DATA_SLOT_NUM)
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
