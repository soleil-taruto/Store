/*
	定数
*/

/*
	マップサイズ (縦・横マップセル数)

	以下のとおりになるようにせよ！
	-- Screen_W == MAP_W_MIN * TILE_W
	-- Screen_H == MAP_H_MIN * TILE_H
*/
var<int> MAP_W_MIN = 25;
var<int> MAP_H_MIN = 19;

/*
	マップセル・サイズ (ドット単位・画面上のサイズ)
*/
var<int> TILE_W = 32;
var<int> TILE_H = 32;

// ----
// プレイヤー情報ここから
// ----

var<int> PLAYER_HP_MAX = 10;

var<int> PLAYER_DAMAGE_FRAME_MAX = 5;
var<int> PLAYER_INVINCIBLE_FRAME_MAX = 60;

/*
	プレイヤーキャラクタの速度
*/
var<double> PLAYER_SPEED = 4.0;

/*
	プレイヤーキャラクタの低速移動時の速度
*/
var<double> PLAYER_SLOW_SPEED = 2.0;

// ----
// プレイヤー情報ここまで
// ----

/// GameEndReason_e
//
var<int> GameEndReason_e_STAGE_CLEAR = @(AUTO);
var<int> GameEndReason_e_RETURN_MENU = @(AUTO);
