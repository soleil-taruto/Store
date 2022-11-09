/*
	画像
*/

function <Picture_t> @@_Load(<string> url)
{
	return LoadPicture(url);
}

// ここから各種画像

// プリフィクス
// P_ ... 画像

var<Picture_t> P_Dummy = @@_Load(RESOURCE_General__Dummy_png);
var<Picture_t> P_WhiteBox = @@_Load(RESOURCE_General__WhiteBox_png);
var<Picture_t> P_WhiteCircle = @@_Load(RESOURCE_General__WhiteCircle_png);

// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

var<Picture_t> P_GameStartButton = @@_Load(RESOURCE_Picture__GameStartButton_png);
var<Picture_t> P_ExplodePiece = @@_Load(RESOURCE_Picture__光る星20_png);
var<Picture_t> P_EndingString = @@_Load(RESOURCE_Picture__EndingString_png);
var<Picture_t> P_Goal = @@_Load(RESOURCE_Picture__Goal_png);

// ==========
// プレイヤー
// ==========

/*
	[方向(テンキー_8方向)][コマ(0～3)]
*/
var<Picture_t[][]> P_Player =
[
	// 0
	null,

	// 1 (左下)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0005_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0105_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0205_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0305_png),
	],

	// 2 (下)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0004_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0104_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0204_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0304_png),
	],

	// 3 (右下)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0003_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0103_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0203_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0303_png),
	],

	// 4 (左)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0006_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0106_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0206_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0306_png),
	],

	// 5
	null,

	// 6 (右)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0002_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0102_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0202_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0302_png),
	],

	// 7 (左上)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0007_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0107_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0207_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0307_png),
	],

	// 8 (上)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0000_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0100_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0200_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0300_png),
	],

	// 9 (右上)
	[
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0001_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0101_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0201_png),
		@@_Load(RESOURCE_点睛集積__Marisa__Tile_0301_png),
	],
];

// ==============
// プレイヤー End
// ==============

// ======
// タイル
// ======

var<Picture_t> P_Tile_Grass = @@_Load(RESOURCE_ぴぽや倉庫__BaseChip__Tile_0000_png);

var<Picture_t> P_Tile_Tree_LT = @@_Load(RESOURCE_ぴぽや倉庫__BaseChip__Tile_0001_png);
var<Picture_t> P_Tile_Tree_RT = @@_Load(RESOURCE_ぴぽや倉庫__BaseChip__Tile_0101_png);
var<Picture_t> P_Tile_Tree_LB = @@_Load(RESOURCE_ぴぽや倉庫__BaseChip__Tile_0002_png);
var<Picture_t> P_Tile_Tree_RB = @@_Load(RESOURCE_ぴぽや倉庫__BaseChip__Tile_0102_png);
var<Picture_t> P_Tile_Tree_Error = @@_Load(RESOURCE_Picture__Tile_TreeError_png);

// ==========
// タイル End
// ==========

var<Picture_t> P_Enemy_Frog = @@_Load(RESOURCE_Picture__Frog_png);
var<Picture_t> P_Enemy_Houdai = @@_Load(RESOURCE_Picture__Houdai_png);
var<Picture_t> P_Enemy_Tama0001 = @@_Load(RESOURCE_Picture__Tama0001_png);
var<Picture_t> P_Enemy_Boss0001 = @@_Load(RESOURCE_Picture__Boss0001_png);
