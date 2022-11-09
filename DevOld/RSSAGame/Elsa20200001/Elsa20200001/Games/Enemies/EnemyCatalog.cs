using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies.Tests;
using Charlotte.Games.Enemies.Tests.神奈子s;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 敵のカタログ
	/// </summary>
	public static class EnemyCatalog
	{
		private class EnemyInfo
		{
			public string Name; // 敵の名前 -- マップ上の配置とか識別に使用する。変更してはならない。
			public string GroupName; // 表示グループ名
			public string MemberName; // 表示名
			public Func<Enemy> Creator;

			/// <summary>
			/// 敵のカタログ要素を生成する。
			/// 敵の名前情報_書式：
			/// -- 名前
			/// -- 表示グループ名/名前
			/// -- 名前:表示名
			/// -- 名前:表示グループ名/表示名
			/// 省略時：
			/// -- 表示グループ名 -- DEFAULT_GROUP_NAME を使用する。
			/// -- 表示名 -- 名前を使用する。
			/// </summary>
			/// <param name="name">敵の名前情報</param>
			/// <param name="creator">敵生成ルーチン</param>
			public EnemyInfo(string name, Func<Enemy> creator)
			{
				{
					int p = name.IndexOf(':');

					if (p != -1)
					{
						this.Name = name.Substring(0, p);
						name = name.Substring(p + 1);
					}
					else
					{
						p = name.IndexOf('/');

						if (p != -1)
							this.Name = name.Substring(p + 1);
						else
							this.Name = name;
					}
				}

				{
					int p = name.IndexOf('/');

					if (p != -1)
					{
						this.GroupName = name.Substring(0, p);
						this.MemberName = name.Substring(p + 1);
					}
					else
					{
						this.GroupName = GameConsts.NAME_DEFAULT;
						this.MemberName = name;
					}
				}

				this.Creator = creator;
			}
		}

		// Creator 用
		// -- 初期値は適当な値
		private static double X = 300.0;
		private static double Y = 300.0;

		private static EnemyInfo[] Enemies = new EnemyInfo[]
		{
			new EnemyInfo(GameConsts.ENEMY_NONE, () => { throw new DDError("敵「無」を生成しようとしました。"); }),
			new EnemyInfo("スタート地点", () => new Enemy_スタート地点(X, Y, 5)),
			new EnemyInfo("上から入場", () => new Enemy_スタート地点(X, Y, 8)),
			new EnemyInfo("下から入場", () => new Enemy_スタート地点(X, Y, 2)),
			new EnemyInfo("左から入場", () => new Enemy_スタート地点(X, Y, 4)),
			new EnemyInfo("右から入場", () => new Enemy_スタート地点(X, Y, 6)),
			new EnemyInfo("敵01:テスト用/テスト用01", () => new Enemy_B0001(X, Y)),
			new EnemyInfo("敵02:テスト用/テスト用02", () => new Enemy_B0002(X, Y)),
			new EnemyInfo("敵03:テスト用/テスト用03", () => new Enemy_B0003(X, Y)),
			new EnemyInfo("テスト用/赤ノコノコ(左)", () => new Enemy_Bノコノコ(X, Y, true, true)),
			new EnemyInfo("テスト用/赤ノコノコ(右)", () => new Enemy_Bノコノコ(X, Y, true, false)),
			new EnemyInfo("テスト用/青ノコノコ(左)", () => new Enemy_Bノコノコ(X, Y, false, true)),
			new EnemyInfo("テスト用/青ノコノコ(右)", () => new Enemy_Bノコノコ(X, Y, false, false)),
			new EnemyInfo("テスト用/Helmet", () => new Enemy_BHelmet(X, Y)),
			new EnemyInfo("テスト用/Chaser", () => new Enemy_BChaser(X, Y)),
			new EnemyInfo("テスト用/即死トラップ-杭", () => new Enemy_B即死トラップ針(X, Y, Ground.I.Picture.Stage01_Chip_f01)),
			new EnemyInfo("テスト用/地蔵(背景)", () => new Enemy_B地蔵(X, Y)),
			new EnemyInfo("テスト用/Dog", () => new Enemy_BDog(X, Y)),
			new EnemyInfo("テスト用/Frog", () => new Enemy_BFrog(X, Y)),
			new EnemyInfo("テスト用/Bird", () => new Enemy_BBird(X, Y)),
			new EnemyInfo("テスト用/天井針", () => new Enemy_B天井針(X, Y)),
			new EnemyInfo("テスト用/即死トラップ-針(右)", () => new Enemy_B即死トラップ針(X, Y, Ground.I.Picture.Stage02_Chip_e01)),
			new EnemyInfo("テスト用/即死トラップ-針(下)", () => new Enemy_B即死トラップ針(X, Y, Ground.I.Picture.Stage02_Chip_e02)),
			new EnemyInfo("テスト用/即死トラップ-針(左)", () => new Enemy_B即死トラップ針(X, Y, Ground.I.Picture.Stage02_Chip_e03)),
			new EnemyInfo("テスト用/即死トラップ-針(上)", () => new Enemy_B即死トラップ針(X, Y, Ground.I.Picture.Stage02_Chip_e04)),
			new EnemyInfo("跳ねる陰陽玉:テスト用/アイテム-跳ねる陰陽玉", () => new Enemy_Item(X, Y, GameStatus.Inventory_e.取得済み_跳ねる陰陽玉)),
			new EnemyInfo("ハンマー陰陽玉:テスト用/アイテム-ハンマー陰陽玉", () => new Enemy_Item(X, Y, GameStatus.Inventory_e.取得済み_ハンマー陰陽玉)),
			new EnemyInfo("エアーシューター:テスト用/アイテム-エアーシューター", () => new Enemy_Item(X, Y, GameStatus.Inventory_e.取得済み_エアーシューター)),
			new EnemyInfo("マグネットエアー:テスト用/アイテム-マグネットエアー", () => new Enemy_Item(X, Y, GameStatus.Inventory_e.取得済み_マグネットエアー)),
			new EnemyInfo("テスト用/B神奈子", () => new Enemy_B神奈子(X, Y)),

			// 新しい敵をここへ追加..
		};

		private static string[] _names = null;

		public static string[] GetNames()
		{
			if (_names == null)
				_names = Enemies.Select(enemy => enemy.Name).ToArray();

			return _names;
		}

		private static string[] _groupNames = null;

		public static string[] GetGroupNames()
		{
			if (_groupNames == null)
				_groupNames = Enemies.Select(enemy => enemy.GroupName).ToArray();

			return _groupNames;
		}

		private static string[] _memberNames = null;

		public static string[] GetMemberNames()
		{
			if (_memberNames == null)
				_memberNames = Enemies.Select(enemy => enemy.MemberName).ToArray();

			return _memberNames;
		}

		public static Enemy Create(string name, double x, double y)
		{
			X = x;
			Y = y;

			return SCommon.FirstOrDie(Enemies, enemy => enemy.Name == name, () => new DDError(name)).Creator();
		}
	}
}
