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

			private const string DEFAULT_GROUP_NAME = "Default";

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
						this.GroupName = DEFAULT_GROUP_NAME;
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
			new EnemyInfo("ロード地点", () => new Enemy_スタート地点(X, Y, 101)),
			new EnemyInfo("テスト用/セーブ地点", () => new Enemy_Bセーブ地点(X, Y)),
			new EnemyInfo("テスト用/アイテム_0001", () => new Enemy_Bアイテム(X, Y, Enemy_Bアイテム.効用_e.TEST_0001)),
			new EnemyInfo("テスト用/アイテム_0002", () => new Enemy_Bアイテム(X, Y, Enemy_Bアイテム.効用_e.TEST_0002)),
			new EnemyInfo("テスト用/アイテム_0003", () => new Enemy_Bアイテム(X, Y, Enemy_Bアイテム.効用_e.TEST_0003)),
			new EnemyInfo("テスト用/敵01", () => new Enemy_B0001(X, Y)),
			new EnemyInfo("テスト用/敵02", () => new Enemy_B0002(X, Y)),
			new EnemyInfo("テスト用/敵03", () => new Enemy_B0003(X, Y)),
			new EnemyInfo("テスト用/敵11", () => new Enemy_B1001(X, Y)),
			new EnemyInfo("テスト用/Trump", () => new Enemy_BTrump(X, Y)),
			new EnemyInfo("テスト用/神奈子", () => new Enemy_B神奈子(X, Y)),
			new EnemyInfo("テスト用/イベント0001", () => new Enemy_Bイベント0001(X, Y)),

			// 新しい敵をここへ追加..
		};

		public static void INIT()
		{
			SCommon.ForEachPair(Enemies, (a, b) =>
			{
				if (a.Name == b.Name)
					throw new DDError("敵の名前の重複：" + a.Name);
			});
		}

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
