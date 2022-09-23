/*
	定数
*/

/*
	フィールド領域
*/
var<int> FIELD_L = 50;
var<int> FIELD_T = 100;
var<int> FIELD_W = Screen_W - 100;
var<int> FIELD_H = Screen_H - 200;
var<int> FIELD_R = FIELD_L + FIELD_W;
var<int> FIELD_B = FIELD_T + FIELD_H;

/*
	マップ・サイズ (テーブルサイズ)
*/
var<int> MAP_W = 20;
var<int> MAP_H = 15;

/*
	マップセル・サイズ (ドット単位・画面上のサイズ)
*/
var<int> TILE_W = 40;
var<int> TILE_H = 40;

// ----
// プレイヤー情報ここから
// ----

/*
	プレイヤーキャラクタの重力加速度
*/
var<double> PLAYER_GRAVITY = 0.9;

/*
	プレイヤーキャラクタの落下最高速度
*/
var<double> PLAYER_FALL_SPEED_MAX = 8.0;

/*
	プレイヤーキャラクタの(横移動)速度
*/
var<double> PLAYER_SPEED = 6.0;

/*
	プレイヤーキャラクタの低速移動時の(横移動)速度
*/
var<double> PLAYER_SLOW_SPEED = 2.0;

/*
	プレイヤーキャラクタのジャンプによる上昇速度
*/
var<double> PLAYER_JUMP_SPEED = -8.0;

// 滞空中に壁に突進しても、脳天判定・接地判定に引っ掛からないように側面判定を先に行う。
// -- ( 脳天判定Pt_X < 側面判定Pt_X && 接地判定Pt_X < 側面判定Pt_X ) を維持すること。
// 上昇が速すぎると、脳天判定より先に側面判定に引っ掛かってしまう可能性がある。
// -- ( -(PLAYER_JUMP_SPEED) < 脳天判定Pt_Y - 側面判定Pt_YT ) を維持すること。
// 下降が速すぎると、接地判定より先に側面判定に引っ掛かってしまう可能性がある。
// -- ( PLAYER_FALL_SPEED_MAX < 接地判定Pt_Y - 側面判定Pt_YB ) を維持すること。

var<double> PLAYER_側面判定Pt_X = TILE_W / 2.0;
var<double> PLAYER_側面判定Pt_YT = TILE_H / 4.0;
var<double> PLAYER_側面判定Pt_YB = TILE_H / 4.0;
var<double> PLAYER_脳天判定Pt_X = TILE_W / 2.0 - 1.0;
var<double> PLAYER_脳天判定Pt_Y = TILE_H / 2.0;
var<double> PLAYER_接地判定Pt_X = TILE_W / 2.0 - 3.001;
var<double> PLAYER_接地判定Pt_Y = TILE_H / 2.0;

// ----
// プレイヤー情報ここまで
// ----

/// GameEndReason_e
//
var<int> GameEndReason_e_STAGE_CLEAR = @(AUTO);
var<int> GameEndReason_e_RETURN_MENU = @(AUTO);
