/*
	定数
*/

/*
	マップサイズ (縦・横マップセル数)
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

var<int> PLAYER_DAMAGE_FRAME_MAX = 20;
var<int> PLAYER_INVINCIBLE_FRAME_MAX = 60;

/*
	ジャンプ回数の上限
	1 == 通常
	2 == 2-段ジャンプまで可能
	3 == 3-段ジャンプまで可能
	...
*/
var<int> PLAYER_JUMP_MAX = 2;

/*
	プレイヤーキャラクタの重力加速度
*/
var<double> PLAYER_GRAVITY = 0.5;

/*
	プレイヤーキャラクタの落下最高速度
*/
var<double> PLAYER_FALL_SPEED_MAX = 10.0;

/*
	プレイヤーキャラクタの(横移動)速度
*/
var<double> PLAYER_SPEED = 4.0;

/*
	プレイヤーキャラクタの低速移動時の(横移動)速度
*/
var<double> PLAYER_SLOW_SPEED = 2.0;

/*
	プレイヤーキャラクタのジャンプによる上昇速度
*/
var<double> PLAYER_JUMP_SPEED = -11.0;

// 滞空中に壁に突進しても、脳天判定・接地判定に引っ掛からないように側面判定を先に行う。
// -- ( 脳天判定Pt_X < 側面判定Pt_X && 接地判定Pt_X < 側面判定Pt_X ) を維持すること。
// 上昇が速すぎると、脳天判定より先に側面判定に引っ掛かってしまう可能性がある。
// -- ( -(PLAYER_JUMP_SPEED) < 脳天判定Pt_Y - 側面判定Pt_YT ) を維持すること。
// 下降が速すぎると、接地判定より先に側面判定に引っ掛かってしまう可能性がある。
// -- ( PLAYER_FALL_SPEED_MAX < 接地判定Pt_Y - 側面判定Pt_YB ) を維持すること。

var<double> PLAYER_側面判定Pt_X = 11.0;
var<double> PLAYER_側面判定Pt_YT = 4.0;
var<double> PLAYER_側面判定Pt_YB = 19.0;
var<double> PLAYER_脳天判定Pt_X = 10.0;
var<double> PLAYER_脳天判定Pt_Y = 16.0;
var<double> PLAYER_接地判定Pt_X = 10.0;
var<double> PLAYER_接地判定Pt_Y = 30.0;

// ----
// プレイヤー情報ここまで
// ----

/// GameEndReason_e
//
var<int> GameEndReason_e_STAGE_CLEAR = @(AUTO);
var<int> GameEndReason_e_RETURN_MENU = @(AUTO);
