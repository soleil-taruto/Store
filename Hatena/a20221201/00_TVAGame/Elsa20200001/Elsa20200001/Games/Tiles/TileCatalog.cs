using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

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
			new TileInfo("芝", () => new Tile_Space(Ground.I.Picture2.Tile_A2[0, 0])),
			new TileInfo("水", () => new Tile_River(Ground.I.Picture2.Tile_A1[0, 0])),
			new TileInfo("箱", () => new Tile_Wall(Ground.I.Picture2.Tile_B[8, 2])),
			new TileInfo("水辺", () => new Tile_水辺(Ground.I.Picture2.MiniTile_A1, 16, 0, 3, 60)),
			new TileInfo("森林", () => new Tile_森林(Ground.I.Picture2.Tile_B, 6, 6, 6, 4, Ground.I.Picture2.Tile_A2[0, 0], Ground.I.Picture.Tile_Dummy)), // 使用しない。-- 木の高さがキャラの身長とほぼ同じなので
			new TileInfo("大森林", () => new Tile_大森林(Ground.I.Picture2.Tile_B, 6, 6, 6, 4, Ground.I.Picture2.Tile_A2[0, 0], Ground.I.Picture2.Tile_B[4, 6])),

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
