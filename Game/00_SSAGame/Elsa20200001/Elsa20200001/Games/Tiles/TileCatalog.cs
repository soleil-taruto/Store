using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles.Tests;

namespace Charlotte.Games.Tiles
{
	/// <summary>
	/// タイルのカタログ
	/// </summary>
	public static class TileCatalog
	{
		private class TileInfo
		{
			public string Name; // タイルの名前 -- マップ上の配置とか識別に使用する。変更してはならない。
			public string GroupName; // 表示グループ名
			public string MemberName; // 表示名
			public Func<Tile> Creator;

			private const string DEFAULT_GROUP_NAME = "Default";

			/// <summary>
			/// タイルのカタログ要素を生成する。
			/// タイルの名前情報_書式：
			/// -- 名前
			/// -- 表示グループ名/名前
			/// -- 名前:表示名
			/// -- 名前:表示グループ名/表示名
			/// 省略時：
			/// -- 表示グループ名 -- DEFAULT_GROUP_NAME を使用する。
			/// -- 表示名 -- 名前を使用する。
			/// </summary>
			/// <param name="name">タイルの名前情報</param>
			/// <param name="creator">タイル生成ルーチン</param>
			public TileInfo(string name, Func<Tile> creator)
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

		private static TileInfo[] Tiles = new TileInfo[]
		{
			new TileInfo(GameConsts.TILE_NONE, () => new Tile_None()),
			new TileInfo("テスト用/ブロック01", () => new Tile_B0001()),
			new TileInfo("テスト用/ブロック02", () => new Tile_B0002()),
			new TileInfo("テスト用/ブロック03", () => new Tile_B0003()),
			new TileInfo("テスト用/ブロック04", () => new Tile_B0004()),

			// 新しいタイルをここへ追加..
		};

		public static void INIT()
		{
			SCommon.ForEachPair(Tiles, (a, b) =>
			{
				if (a.Name == b.Name)
					throw new DDError("タイルの名前の重複：" + a.Name);
			});
		}

		private static string[] _names = null;

		public static string[] GetNames()
		{
			if (_names == null)
				_names = Tiles.Select(tile => tile.Name).ToArray();

			return _names;
		}

		private static string[] _groupNames = null;

		public static string[] GetGroupNames()
		{
			if (_groupNames == null)
				_groupNames = Tiles.Select(tile => tile.GroupName).ToArray();

			return _groupNames;
		}

		private static string[] _memberNames = null;

		public static string[] GetMemberNames()
		{
			if (_memberNames == null)
				_memberNames = Tiles.Select(tile => tile.MemberName).ToArray();

			return _memberNames;
		}

		public static Tile Create(string name)
		{
			return SCommon.FirstOrDie(Tiles, tile => tile.Name == name, () => new DDError(name)).Creator();
		}
	}
}
