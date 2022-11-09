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

// プレイヤー

var<Picture_t> P_PlayerAttack = @@_Load(RESOURCE_えむくろ__CirnoAttack_png);
var<Picture_t> P_PlayerStand  = @@_Load(RESOURCE_えむくろ__CirnoStand_png);
var<Picture_t> P_PlayerJump   = @@_Load(RESOURCE_えむくろ__CirnoJump_png);

var<Picture_t[]> P_PlayerRun =
[
	@@_Load(RESOURCE_えむくろ__CirnoRun_001_png),
	@@_Load(RESOURCE_えむくろ__CirnoRun_002_png),
	@@_Load(RESOURCE_えむくろ__CirnoRun_003_png),
	@@_Load(RESOURCE_えむくろ__CirnoRun_004_png),
];

// プレイヤー_Mirror

var<Picture_t> P_PlayerMirrorAttack = @@_Load(RESOURCE_えむくろ__Mirror__CirnoAttack_png);
var<Picture_t> P_PlayerMirrorStand  = @@_Load(RESOURCE_えむくろ__Mirror__CirnoStand_png);
var<Picture_t> P_PlayerMirrorJump   = @@_Load(RESOURCE_えむくろ__Mirror__CirnoJump_png);

var<Picture_t[]> P_PlayerMirrorRun =
[
	@@_Load(RESOURCE_えむくろ__Mirror__CirnoRun_001_png),
	@@_Load(RESOURCE_えむくろ__Mirror__CirnoRun_002_png),
	@@_Load(RESOURCE_えむくろ__Mirror__CirnoRun_003_png),
	@@_Load(RESOURCE_えむくろ__Mirror__CirnoRun_004_png),
];

// プレイヤー_End

var<Picture_t> P_TileNone = @@_Load(RESOURCE_Tile__Tile_None_png);

var<Picture_t[]> P_Tiles =
[
	@@_Load(RESOURCE_Tile__Tile_B0001_png),
	@@_Load(RESOURCE_Tile__Tile_B0002_png),
	@@_Load(RESOURCE_Tile__Tile_B0003_png),
	@@_Load(RESOURCE_Tile__Tile_B0004_png),
];

var<Picture_t> P_Wall0001 = @@_Load(RESOURCE_素材Good__Wall_0001_png);
var<Picture_t> P_Wall0002 = @@_Load(RESOURCE_素材Good__Wall_0002_png);
var<Picture_t> P_Wall0003 = @@_Load(RESOURCE_素材Good__Wall_0003_png);

var<Picture_t> P_Enemy_Frog = @@_Load(RESOURCE_Picture__Frog_png);
var<Picture_t> P_Enemy_Houdai = @@_Load(RESOURCE_Picture__Houdai_png);
var<Picture_t> P_Enemy_Tama0001 = @@_Load(RESOURCE_Picture__Tama0001_png);
var<Picture_t> P_Enemy_Boss0001 = @@_Load(RESOURCE_Picture__Boss0001_png);
