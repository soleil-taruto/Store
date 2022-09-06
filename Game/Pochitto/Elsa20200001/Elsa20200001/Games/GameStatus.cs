using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games.Shots;
using Charlotte.Utilities;

namespace Charlotte.Games
{
	/// <summary>
	/// ゲームの状態を保持する。
	/// プレイヤーのレベルとか保有アイテムといった概念が入ってくることを想定して、独立したクラスとする。
	/// セーブ時、このインスタンスが保存される。
	/// </summary>
	public class GameStatus
	{
		// ****
		// テスト時など、特にフィールドを設定せずにインスタンスを生成する使い方を想定して、
		// 全てのパラメータはデフォルト値で初期化すること。
		// ****

		private const int MAX_HP = 10;

		/// <summary>
		/// プレイヤーの最大HP
		/// </summary>
		public int MaxHP = MAX_HP;

		/// <summary>
		/// (マップ入場時の)プレイヤーのHP
		/// </summary>
		public int StartHP = MAX_HP;

		/// <summary>
		/// スタート地点の Direction 値
		/// 5 == 中央(デフォルト) == ゲームスタート時
		/// 2 == 下から入場
		/// 4 == 左から入場
		/// 6 == 右から入場
		/// 8 == 上から入場
		/// 101 == ロード地点
		/// </summary>
		public int StartPointDirection = 5;

		/// <summary>
		/// (マップ入場時に)プレイヤーが左を向いているか
		/// </summary>
		public bool StartFacingLeft = false;

		/// <summary>
		/// 最後のマップを退場した方向
		/// 5 == 中央(デフォルト) == メニュー操作によりゲーム終了
		/// 2 == 下から退場
		/// 4 == 左から退場
		/// 6 == 右から退場
		/// 8 == 上から退場
		/// 901 == 死亡によりゲーム終了
		/// </summary>
		public int ExitDirection = 5;

		/// <summary>
		/// (マップ入場時に)プレイヤーが装備している武器
		/// </summary>
		public ShotCatalog.武器_e Start_武器 = ShotCatalog.武器_e.NORMAL;

		/// <summary>
		/// game_進行・インベントリ(enum)
		/// </summary>
		public enum Inventory_e
		{
			B神奈子を倒した, // テスト用

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

		#region Serialize / Deserialize

		public string Serialize()
		{
			List<string> dest = new List<string>();

			// ★★★ シリアライズ_ここから ★★★

			dest.Add("" + this.MaxHP);
			dest.Add("" + this.StartHP);
			dest.Add("" + this.StartPointDirection);
			dest.Add("" + (this.StartFacingLeft ? 1 : 0));
			dest.Add("" + this.ExitDirection);
			dest.Add("" + (int)this.Start_武器);
			dest.Add(this.InventoryFlags.Serialize());

			// ★★★ シリアライズ_ここまで ★★★

			return SCommon.Serializer.I.Join(dest.ToArray());
		}

		private void S_Deserialize(string value)
		{
			string[] lines = SCommon.Serializer.I.Split(value);
			int c = 0;

			// ★★★ デシリアライズ_ここから ★★★

			this.MaxHP = int.Parse(lines[c++]);
			this.StartHP = int.Parse(lines[c++]);
			this.StartPointDirection = int.Parse(lines[c++]);
			this.StartFacingLeft = int.Parse(lines[c++]) != 0;
			this.ExitDirection = int.Parse(lines[c++]);
			this.Start_武器 = (ShotCatalog.武器_e)int.Parse(lines[c++]);
			this.InventoryFlags.Deserialize(lines[c++]);

			// ★★★ デシリアライズ_ここまで ★★★
		}

		public static GameStatus Deserialize(string value)
		{
			GameStatus gameStatus = new GameStatus();
			gameStatus.S_Deserialize(value);
			return gameStatus;
		}

		#endregion

		public GameStatus GetClone()
		{
			return Deserialize(this.Serialize());
		}
	}
}
