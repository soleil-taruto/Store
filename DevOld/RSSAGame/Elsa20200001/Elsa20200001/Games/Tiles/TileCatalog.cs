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
			public string Name; // 敵の名前 -- マップ上の配置とか識別に使用する。変更してはならない。
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
			new TileInfo("ブロック01", () => new Tile_B0001()),
			new TileInfo("ブロック02", () => new Tile_B0002()),
			new TileInfo("ブロック03", () => new Tile_B0003()),
			new TileInfo("ブロック04", () => new Tile_B0004()),
			new TileInfo("背景01", () => new Tile_Standard(Ground.I.Picture.Space_B0001, Tile.Kind_e.SPACE)),
			new TileInfo("梯子", () => new Tile_Ladder()),

			new TileInfo("Stage01_Chip_a01:地面", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_a01, Tile.Kind_e.WALL)),
			new TileInfo("Stage01_Chip_a02:地中", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_a02, Tile.Kind_e.WALL)),
			new TileInfo("Stage01_Chip_b01:地面(背景)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_b01, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_b02:地中(背景)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_b02, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c01:櫓/櫓01", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c01, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c02:櫓/櫓02", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c02, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c03:櫓/櫓03", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c03, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c04:櫓/櫓04", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c04, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage01_Chip_c05:櫓/櫓05", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c05, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c06:櫓/櫓06", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c06, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c07:櫓/櫓07", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c07, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage01_Chip_c08:櫓/櫓08", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c08, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage01_Chip_c09:櫓/櫓09", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c09, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage01_Chip_c10:櫓/櫓10", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c10, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage01_Chip_c11:櫓/櫓11", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c11, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage01_Chip_c12:櫓/櫓12", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c12, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_c13:櫓/櫓13", () => new Tile_Front(Ground.I.Picture.Stage01_Chip_c13, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_d01:ブロック壁/ブロック壁_背景(明)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_d01, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_d02:ブロック壁/ブロック壁_背景(グ)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_d02, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_d03:ブロック壁/ブロック壁_背景(暗)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_d03, Tile.Kind_e.SPACE)),
			new TileInfo("Stage01_Chip_e01:ブロック壁/ブロック壁(明)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_e01, Tile.Kind_e.WALL)),
			new TileInfo("Stage01_Chip_e02:ブロック壁/ブロック壁(グ)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_e02, Tile.Kind_e.WALL)),
			new TileInfo("Stage01_Chip_e03:ブロック壁/ブロック壁(暗)", () => new Tile_Standard(Ground.I.Picture.Stage01_Chip_e03, Tile.Kind_e.WALL)),

			//new TileInfo("Stage02_Bg_Chip_a01", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_a01, Tile.Kind_e.SPACE)), // レンガ_背景
			//new TileInfo("Stage02_Bg_Chip_a02", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_a02, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_a03", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_a03, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b01_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b01_01, Tile.Kind_e.SPACE)), // 滝_背面
			//new TileInfo("Stage02_Bg_Chip_b01_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b01_02, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b01_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b01_03, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b01_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b01_04, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b02_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b02_01, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b02_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b02_02, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b02_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b02_03, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b02_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b02_04, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b03_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b03_01, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b03_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b03_02, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b03_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b03_03, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b03_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b03_04, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b04_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b04_01, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b04_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b04_02, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b04_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b04_03, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Bg_Chip_b04_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_b04_04, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c01:背景パイプ/背景パイプC01", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c01, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c02:背景パイプ/背景パイプC02", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c02, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c03:背景パイプ/背景パイプC03", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c03, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c04:背景パイプ/背景パイプC04", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c04, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c05:背景パイプ/背景パイプC05", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c05, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c06:背景パイプ/背景パイプC06", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c06, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c07:背景パイプ/背景パイプC07", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c07, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c08:背景パイプ/背景パイプC08", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c08, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c09:背景パイプ/背景パイプC09", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c09, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c10:背景パイプ/背景パイプC10", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c10, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c11:背景パイプ/背景パイプC11", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c11, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c12:背景パイプ/背景パイプC12", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c12, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c13:背景パイプ/背景パイプC13", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c13, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Bg_Chip_c14:背景パイプ/背景パイプC14", () => new Tile_Standard(Ground.I.Picture.Stage02_Bg_Chip_c14, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_a01:機械壁/機械壁01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a01, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a02:機械壁/機械壁02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a02, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a03:機械壁/機械壁03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a03, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a04:機械壁/機械壁04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a04, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a05:機械壁/機械壁05", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a05, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a06:機械壁/機械壁06", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a06, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a07:機械壁/機械壁07", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a07, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a08:機械壁/機械壁08", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a08, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a09:機械壁/機械壁09", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a09, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a10:機械壁/機械壁10", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a10, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a11:機械壁/機械壁11", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a11, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a12:機械壁/機械壁12", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a12, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a13:機械壁/機械壁13", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a13, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a14:機械壁/機械壁14", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a14, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a15:機械壁/機械壁15", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a15, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a16:機械壁/機械壁16", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a16, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a17:機械壁/機械壁17", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a17, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a18:機械壁/機械壁18", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a18, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a19:機械壁/機械壁19", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a19, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a20:機械壁/機械壁20", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a20, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a21:機械壁/機械壁21", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a21, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a22:機械壁/機械壁22", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a22, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a23:機械壁/機械壁23", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a23, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a24:機械壁/機械壁24", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a24, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a25:機械壁/機械壁25", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a25, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a26:機械壁/機械壁26", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a26, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a27:機械壁/機械壁27", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a27, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_a28:機械壁/機械壁28", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_a28, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_b01:ネット/ネット01", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b01, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_b02:ネット/ネット02", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b02, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_b03:ネット/ネット03", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b03, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage02_Chip_b04:ネット/ネット04", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b04, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage02_Chip_b05:ネット/ネット05", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b05, Tile.Kind_e.CLOUD)),
			new TileInfo("Stage02_Chip_b06:ネット/ネット06", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b06, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_b07:ネット/ネット07", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b07, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_b08:ネット/ネット08", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b08, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_b09:ネット/ネット09", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_b09, Tile.Kind_e.SPACE)),
			//new TileInfo("Stage02_Chip_c01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_c01, Tile.Kind_e.WALL)), // レンガ
			//new TileInfo("Stage02_Chip_c02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_c02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_c03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_c03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_d01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_d01, Tile.Kind_e.LADDER)), // 梯子 -- 既に有る。
			//new TileInfo("Stage02_Chip_e01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_e01, Tile.Kind_e.WALL)), // 針
			//new TileInfo("Stage02_Chip_e02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_e02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_e03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_e03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_e04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_e04, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f01_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f01_01, Tile.Kind_e.WALL)), // 滝_前面
			//new TileInfo("Stage02_Chip_f01_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f01_02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f01_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f01_03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f01_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f01_04, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f02_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f02_01, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f02_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f02_02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f02_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f02_03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f02_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f02_04, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f03_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f03_01, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f03_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f03_02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f03_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f03_03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f03_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f03_04, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f04_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f04_01, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f04_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f04_02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f04_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f04_03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_f04_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_f04_04, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_g04_01", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_g04_01, Tile.Kind_e.WALL)), // 針アニメ
			//new TileInfo("Stage02_Chip_g04_02", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_g04_02, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_g04_03", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_g04_03, Tile.Kind_e.WALL)),
			//new TileInfo("Stage02_Chip_g04_04", () => new Tile_Standard(Ground.I.Picture.Stage02_Chip_g04_04, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h01:前面パイプ/前面パイプH01", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h01, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h02:前面パイプ/前面パイプH02", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h02, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h03:前面パイプ/前面パイプH03", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h03, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h04:前面パイプ/前面パイプH04", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h04, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h05:前面パイプ/前面パイプH05", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h05, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h06:前面パイプ/前面パイプH06", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h06, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h07:前面パイプ/前面パイプH07", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h07, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h08:前面パイプ/前面パイプH08", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h08, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h09:前面パイプ/前面パイプH09", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h09, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h10:前面パイプ/前面パイプH10", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h10, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h11:前面パイプ/前面パイプH11", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h11, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h12:前面パイプ/前面パイプH12", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h12, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h13:前面パイプ/前面パイプH13", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h13, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h14:前面パイプ/前面パイプH14", () => new Tile_Front(Ground.I.Picture.Stage02_Chip_h14, Tile.Kind_e.SPACE)),
			new TileInfo("Stage02_Chip_h01_Wall:壁パイプ/壁パイプH01", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h01, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h02_Wall:壁パイプ/壁パイプH02", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h02, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h03_Wall:壁パイプ/壁パイプH03", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h03, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h04_Wall:壁パイプ/壁パイプH04", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h04, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h05_Wall:壁パイプ/壁パイプH05", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h05, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h06_Wall:壁パイプ/壁パイプH06", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h06, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h07_Wall:壁パイプ/壁パイプH07", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h07, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h08_Wall:壁パイプ/壁パイプH08", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h08, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h09_Wall:壁パイプ/壁パイプH09", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h09, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h10_Wall:壁パイプ/壁パイプH10", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h10, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h11_Wall:壁パイプ/壁パイプH11", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h11, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h12_Wall:壁パイプ/壁パイプH12", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h12, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h13_Wall:壁パイプ/壁パイプH13", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h13, Tile.Kind_e.WALL)),
			new TileInfo("Stage02_Chip_h14_Wall:壁パイプ/壁パイプH14", () => new Tile_Double(Ground.I.Picture.Stage01_Chip_e03, Ground.I.Picture.Stage02_Chip_h14, Tile.Kind_e.WALL)),
			
			new TileInfo("レンガ/レンガ_背景", () => new Tile_Brick_Back()),
			new TileInfo("レンガ/レンガ_壁", () => new Tile_Brick_Front()),
			
			new TileInfo("滝/滝_背面_01", () => new Tile_Waterfall_Back(Ground.I.Picture.Stage02_Bg_Chip_b0x[0])),
			new TileInfo("滝/滝_背面_02", () => new Tile_Waterfall_Back(Ground.I.Picture.Stage02_Bg_Chip_b0x[1])),
			new TileInfo("滝/滝_背面_03", () => new Tile_Waterfall_Back(Ground.I.Picture.Stage02_Bg_Chip_b0x[2])),
			new TileInfo("滝/滝_背面_04", () => new Tile_Waterfall_Back(Ground.I.Picture.Stage02_Bg_Chip_b0x[3])),

			new TileInfo("滝/滝_前面_01", () => new Tile_Waterfall_Front(Ground.I.Picture.Stage02_Chip_f0x[0])),
			new TileInfo("滝/滝_前面_02", () => new Tile_Waterfall_Front(Ground.I.Picture.Stage02_Chip_f0x[1])),
			new TileInfo("滝/滝_前面_03", () => new Tile_Waterfall_Front(Ground.I.Picture.Stage02_Chip_f0x[2])),
			new TileInfo("滝/滝_前面_04", () => new Tile_Waterfall_Front(Ground.I.Picture.Stage02_Chip_f0x[3])),

			new TileInfo("滝/滝_前面-壁_01", () => new Tile_Waterfall_Ground(Ground.I.Picture.Stage02_Chip_f0x[0], Ground.I.Picture.Stage01_Chip_e03)),
			new TileInfo("滝/滝_前面-壁_02", () => new Tile_Waterfall_Ground(Ground.I.Picture.Stage02_Chip_f0x[1], Ground.I.Picture.Stage01_Chip_e03)),
			new TileInfo("滝/滝_前面-壁_03", () => new Tile_Waterfall_Ground(Ground.I.Picture.Stage02_Chip_f0x[2], Ground.I.Picture.Stage01_Chip_e03)),
			new TileInfo("滝/滝_前面-壁_04", () => new Tile_Waterfall_Ground(Ground.I.Picture.Stage02_Chip_f0x[3], Ground.I.Picture.Stage01_Chip_e03)),

			// 新しいタイルをここへ追加..
		};

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
				_memberNames = Tiles.Select(tile => tile.MemberName + " (" + Tile.Kind_e_Names[(int)tile.Creator().GetKind()][0] + ")").ToArray();

			return _memberNames;
		}

		public static Tile Create(string name)
		{
			return SCommon.FirstOrDie(Tiles, tile => tile.Name == name, () => new DDError(name)).Creator();
		}
	}
}
