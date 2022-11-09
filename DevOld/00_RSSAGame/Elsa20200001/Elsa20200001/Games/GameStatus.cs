using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utilities;
using Charlotte.Games.Shots;

namespace Charlotte.Games
{
	/// <summary>
	/// ゲームの状態を保持する。
	/// プレイヤーのレベルとか保有アイテムといった概念が入ってくることを想定して、独立したクラスとする。
	/// パスワードは、このインスタンスから生成される。
	/// </summary>
	public class GameStatus
	{
		// (テスト時など)特にフィールドを設定せずにインスタンスを生成する使い方を想定して、
		// 全てのパラメータはデフォルト値で初期化すること。

		/// <summary>
		/// プレイヤーのHP
		/// </summary>
		public int StartHP = GameConsts.PLAYER_HP_MAX;

		/// <summary>
		/// 現在のワイリーステージのインデックス
		/// -- 0 == 最初のワイリーステージ || 通常ステージ
		/// -- 1～ == 2つ目以降のワイリーステージ
		/// </summary>
		public int WilyStageIndex = 0;

		/// <summary>
		/// スタート地点の Direction 値
		/// 5 == 中央(デフォルト) == ゲームスタート時
		/// 2 == 下から入場
		/// 4 == 左から入場
		/// 6 == 右から入場
		/// 8 == 上から入場
		/// </summary>
		public int StartPointDirection = 5;

		/// <summary>
		/// プレイヤーが左を向いているか
		/// </summary>
		public bool StartFacingLeft = false;

		/// <summary>
		/// スタート時点のプレイヤーの状態
		/// </summary>
		public class StartPlayerStatusInfo
		{
			public double X; // マップ上の位置(X-軸,ドット単位)
			public double Y; // マップ上の位置(Y-軸,ドット単位)
			public bool Ladder; // 梯子を登っているか
			public bool FacingLeft; // 左を向いているか
		}

		/// <summary>
		/// スタート時点のプレイヤーの状態
		/// null == 無効
		/// null ではない場合、ゲーム開始時(Game.Perform 開始時)にこの状態をプレイヤーに反映すること。
		/// </summary>
		public StartPlayerStatusInfo StartPlayerStatus = null;

		/// <summary>
		/// 最後のマップを退場した方向
		/// 5 == 中央(デフォルト) == ゲーム終了時
		/// 2 == 下から退場
		/// 4 == 左から退場
		/// 6 == 右から退場
		/// 8 == 上から退場
		/// </summary>
		public int ExitDirection = 5;

		/// <summary>
		/// 装備している武器
		/// </summary>
		public ShotCatalog.武器_e Equipment = ShotCatalog.武器_e.Normal;

		/// <summary>
		/// game_進行・インベントリ(enum)
		/// </summary>
		public enum Inventory_e
		{
			B神奈子を倒した, // テスト用

			取得済み_跳ねる陰陽玉,
			取得済み_ハンマー陰陽玉,
			取得済み_エアーシューター,
			取得済み_マグネットエアー,

			// 新しい項目をここへ追加...
		}

		public class S_InventoryFlags
		{
			private BitList Flags = new BitList();

			public bool this[Inventory_e inventory]
			{
				get
				{
					return this.Flags[(long)inventory];
				}

				set
				{
					this.Flags[(long)inventory] = value;
				}
			}

			public string Serialize()
			{
				return new string(this.Flags.Iterate().Select(flag => flag ? '1' : '0').ToArray());
			}

			public void Deserialize(string value)
			{
				this.Flags = new BitList(value.Select(chr => chr == '1'));
			}
		}

		/// <summary>
		/// game_進行・インベントリ
		/// </summary>
		public S_InventoryFlags InventoryFlags = new S_InventoryFlags();

		// <---- prm

		// パスワードに変換する：
		// -- GameStatus.PasswordConv.GetPassword(this.Serialize())
		// パスワードから変換する：
		// -- GameStatus.Deserialize(GameStatus.PasswordConv.GetValue(password))
		// ---- 不正な password -> GameStatus.Undeserializable を投げる。

		// ====
		// Serialize / Deserialize ここから
		// ====

		public UInt16 Serialize()
		{
			return 0; // TODO
		}

		public static GameStatus Deserialize(UInt16 value)
		{
			GameStatus gameStatus = new GameStatus();
			gameStatus.S_Deserialize(value);
			return gameStatus;
		}

		public class Undeserializable : Exception
		{ }

		private void S_Deserialize(UInt16 value) // throws Undeserializable
		{
			// TODO
		}

		// ====
		// Serialize / Deserialize ここまで
		// ====

		public static S_PassswordConv PasswordConv = new S_PassswordConv();

		public class S_PassswordConv
		{
			public bool[,] GetPassword(UInt16 value)
			{
				return new bool[GameConsts.PASSWORD_WH, GameConsts.PASSWORD_WH]; // TODO
			}

			public UInt16 GetValue(bool[,] password)
			{
				return 0; // TODO
			}
		}
	}
}
