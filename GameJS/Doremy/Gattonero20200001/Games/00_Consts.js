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

var<int> PLAYER_DAMAGE_FRAME_MAX = 40;
var<int> PLAYER_INVINCIBLE_FRAME_MAX = 60;

/*
	ジャンプ回数の上限
	1 == 通常
	2 == 2-段ジャンプまで可能
	3 == 3-段ジャンプまで可能
	...
*/
var<int> PLAYER_JUMP_MAX = 1;

/*
	プレイヤーキャラクタの重力加速度
*/
var<double> PLAYER_GRAVITY = 0.6;

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
var<double> PLAYER_JUMP_SPEED = -13.0;

// 滞空中に壁に突進しても、脳天判定・接地判定に引っ掛からないように側面判定を先に行う。
// -- ( 脳天判定Pt_X < 側面判定Pt_X && 接地判定Pt_X < 側面判定Pt_X ) を維持すること。
// 上昇が速すぎると、脳天判定より先に側面判定に引っ掛かってしまう可能性がある。
// -- ( -(PLAYER_JUMP_SPEED) < 脳天判定Pt_Y - 側面判定Pt_YT ) を維持すること。
// 下降が速すぎると、接地判定より先に側面判定に引っ掛かってしまう可能性がある。
// -- ( PLAYER_FALL_SPEED_MAX < 接地判定Pt_Y - 側面判定Pt_YB ) を維持すること。

var<double> PLAYER_側面判定Pt_X = 16.0 / 2;
var<double> PLAYER_側面判定Pt_YT = 20.0 / 2;
var<double> PLAYER_側面判定Pt_YB = 20.0 / 2;
var<double> PLAYER_脳天判定Pt_X = 14.0 / 2;
var<double> PLAYER_脳天判定Pt_Y = 40.0 / 2;
var<double> PLAYER_接地判定Pt_X = 14.0 / 2;
var<double> PLAYER_接地判定Pt_Y = 48.0 / 2;

// キャラ画像的に崖っぷちギリギリまで行けるようにする。
//
// 崖っぷちギリギリまで行けるように、もう少し広げても良いかもしれない。-> 46.0 / 2
// そうすると１マスの穴に落ちられなくなるので妥協点としてこの幅とする。-> 30.0 / 2
//
var<double> PLAYER_接地判定Pt_X_接地時のみ = 30.0 / 2;

// 梯子の一番上のマップセルの上部の領域のみ接地判定アリとする。
// この領域の高さ。
//
// 梯子を登りきったとき梯子の上に立ってくれるように、
// 落下最高速度で落下中にすり抜けないように、
// 落下最高速度より大きいこと。
//
var<int> LADDER_TOP_GROUND_Y_SIZE = PLAYER_FALL_SPEED_MAX + 2;

var<int> PLAYER_SHOOTING_FRAME_MAX = 15;

// ----
// プレイヤー情報ここまで
// ----

/// GameEndReason_e
//
var<int> GameEndReason_e_STAGE_CLEAR = @(AUTO);
var<int> GameEndReason_e_RETURN_MENU = @(AUTO);
