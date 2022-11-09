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

var<Picture_t> P_PlayerJump       = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_jump01_png);
var<Picture_t> P_PlayerJumpAttack = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_jump02attack_png);
var<Picture_t> P_PlayerJumpDamage = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_jump03damage_png);

var<Picture_t[]> P_PlayerRun =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_run01_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_run02_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_run03_png),
];

var<Picture_t[]> P_PlayerRunAttack =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_run04_attack_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_run05_attack_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_run06_attack_png),
];

var<Picture_t> P_PlayerSliding      = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_sliding01_png);
var<Picture_t> P_PlayerTelepo01     = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_telepo01_png);
var<Picture_t> P_PlayerTelepo02     = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_telepo02_png);
var<Picture_t> P_PlayerTelepo03     = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_telepo03_png);
var<Picture_t> P_PlayerWait         = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_wait01_png);
var<Picture_t> P_PlayerWaitMabataki = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_wait02_png);
var<Picture_t> P_PlayerWaitStart    = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_wait03start_png);
var<Picture_t> P_PlayerWaitAttack   = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_wait04attack_png);

var<Picture_t[]> P_PlayerClimb =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_climb01_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_climb02_png),
];

var<Picture_t> P_PlayerClimbAttack = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_climb03_attack_png);
var<Picture_t> P_PlayerClimbTop    = @@_Load(RESOURCE_ロックマン風__Doremy__chara_a01_climb04_png);

var<Picture_t> P_PlayerEffectShockA = @@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_shock01_png);
var<Picture_t> P_PlayerEffectShockB =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_shock02_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_shock03_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_shock04_png),
];

var<Picture_t> P_PlayerEffectSliding =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_sliding01_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_sliding02_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__effect_a01_sliding03_png),
];

// =================
// プレイヤー Mirror
// =================

var<Picture_t> P_PlayerMirrorJump       = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_jump01_png);
var<Picture_t> P_PlayerMirrorJumpAttack = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_jump02attack_png);
var<Picture_t> P_PlayerMirrorJumpDamage = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_jump03damage_png);

var<Picture_t[]> P_PlayerMirrorRun =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_run01_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_run02_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_run03_png),
];

var<Picture_t[]> P_PlayerMirrorRunAttack =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_run04_attack_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_run05_attack_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_run06_attack_png),
];

var<Picture_t> P_PlayerMirrorSliding      = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_sliding01_png);
var<Picture_t> P_PlayerMirrorTelepo01     = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_telepo01_png);
var<Picture_t> P_PlayerMirrorTelepo02     = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_telepo02_png);
var<Picture_t> P_PlayerMirrorTelepo03     = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_telepo03_png);
var<Picture_t> P_PlayerMirrorWait         = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_wait01_png);
var<Picture_t> P_PlayerMirrorWaitMabataki = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_wait02_png);
var<Picture_t> P_PlayerMirrorWaitStart    = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_wait03start_png);
var<Picture_t> P_PlayerMirrorWaitAttack   = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_wait04attack_png);

var<Picture_t[]> P_PlayerMirrorClimb =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_climb01_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_climb02_png),
];

var<Picture_t> P_PlayerMirrorClimbAttack = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_climb03_attack_png);
var<Picture_t> P_PlayerMirrorClimbTop    = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__chara_a01_climb04_png);

var<Picture_t> P_PlayerMirrorEffectShockA = @@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_shock01_png);
var<Picture_t> P_PlayerMirrorEffectShockB =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_shock02_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_shock03_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_shock04_png),
];

var<Picture_t> P_PlayerMirrorEffectSliding =
[
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_sliding01_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_sliding02_png),
	@@_Load(RESOURCE_ロックマン風__Doremy__Mirror__effect_a01_sliding03_png),
];

// ==============
// プレイヤー End
// ==============

// ======
// タイル
// ======

var<Picture_t[]> P_TileNone = @@_Load(RESOURCE_Tile__Tile_B0001_png);

var<Picture_t[]> P_Tile_B1 = @@_Load(RESOURCE_Tile__Tile_B0001_png);
var<Picture_t[]> P_Tile_B2 = @@_Load(RESOURCE_Tile__Tile_B0002_png);
var<Picture_t[]> P_Tile_B3 = @@_Load(RESOURCE_Tile__Tile_B0003_png);
var<Picture_t[]> P_Tile_B4 = @@_Load(RESOURCE_Tile__Tile_B0004_png);

/*
	梯子
*/
var<Picture_t> P_Tile_Ladder = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_d01_png);

/*
	煉瓦(小)
*/
var<Picture_t> P_Tile_Brick_S = @@_Load(RESOURCE_ロックマン風__Tile__stage01_chip_e01_png);

/*
	地面
*/
var<Picture_t> P_Tile_Ground1 = @@_Load(RESOURCE_ロックマン風__Tile__stage01_chip_a01_png);
var<Picture_t> P_Tile_Ground2 = @@_Load(RESOURCE_ロックマン風__Tile__stage01_chip_a02_png);

/*
	煉瓦(大)
*/
var<Picture_t> P_Tile_Brick_L1 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_c01_png);
var<Picture_t> P_Tile_Brick_L2 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_c02_png);
var<Picture_t> P_Tile_Brick_L3 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_c03_png);

/*
	フェンス
	番号はテンキー式の配置
*/
var<Picture_t> P_Tile_Fence1 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b01_png);
var<Picture_t> P_Tile_Fence2 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b08_png);
var<Picture_t> P_Tile_Fence3 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b07_png);
var<Picture_t> P_Tile_Fence4 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b02_png);
var<Picture_t> P_Tile_Fence5 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b09_png);
var<Picture_t> P_Tile_Fence6 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b06_png);
var<Picture_t> P_Tile_Fence7 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b03_png);
var<Picture_t> P_Tile_Fence8 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b04_png);
var<Picture_t> P_Tile_Fence9 = @@_Load(RESOURCE_ロックマン風__Tile__stage02_chip_b05_png);

// ==========
// タイル End
// ==========

var<Picture_t> P_Wall0001B = @@_Load(RESOURCE_素材Good__Wall_0001B_png);
var<Picture_t> P_Wall0001F = @@_Load(RESOURCE_素材Good__Wall_0001F_png);
var<Picture_t> P_Wall0002  = @@_Load(RESOURCE_素材Good__Wall_0002_png);
var<Picture_t> P_Wall0003  = @@_Load(RESOURCE_素材Good__Wall_0003_png);

var<Picture_t> P_Enemy_Frog = @@_Load(RESOURCE_Picture__Frog_png);
var<Picture_t> P_Enemy_Houdai = @@_Load(RESOURCE_Picture__Houdai_png);
var<Picture_t> P_Enemy_Tama0001 = @@_Load(RESOURCE_Picture__Tama0001_png);
var<Picture_t> P_Enemy_Boss0001 = @@_Load(RESOURCE_Picture__Boss0001_png);
