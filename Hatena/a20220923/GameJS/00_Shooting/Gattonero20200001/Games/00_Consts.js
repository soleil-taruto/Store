/*
	定数
*/

// ----
// プレイヤー情報ここから
// ----

var<int> PLAYER_REBORN_FRAME_MAX = 30;
var<int> PLAYER_INVINCIBLE_FRAME_MAX = 60;

/*
	攻撃レベル・最大値
*/
var<int> PLAYER_ATTACK_LV_MAX = 3;

// ----
// プレイヤー情報ここまで
// ----

/// GameEndReason_e
//
var<int> GameEndReason_e_RESTART_GAME = @(AUTO);
var<int> GameEndReason_e_RETURN_MENU  = @(AUTO);
var<int> GameEndReason_e_GAME_CLEAR   = @(AUTO);
var<int> GameEndReason_e_GAME_OVER    = @(AUTO);
