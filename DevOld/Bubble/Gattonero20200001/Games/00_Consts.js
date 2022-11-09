/*
	定数
*/

/*
	フィールド領域
*/
var<int> FIELD_L = 0;
var<int> FIELD_T = 0;
var<int> FIELD_W = Screen_W;
var<int> FIELD_H = Screen_H - 30;
var<int> FIELD_R = FIELD_L + FIELD_W;
var<int> FIELD_B = FIELD_T + FIELD_H;

// 自弾(ボール)の速度
var<double> BALL_SPEED = 8.0;

/*
	ボールの色
	値：P_Balls-の添字と一致すること。
*/
/// BallColor_e
//
var<int> BallColor_e_GREEN   = 0;
var<int> BallColor_e_CYAN    = 1;
var<int> BallColor_e_BLUE    = 2;
var<int> BallColor_e_RED     = 3;
var<int> BallColor_e_MAGENTA = 4;
var<int> BallColor_e_ORANGE  = 5;
var<int> BallColor_e_YELLOW  = 6;

// チャージ・フレーム数
var<int> CHARGE_FRAME_MAX = 20;

// 次以降の自弾リスト・サイズ
var<int> SUBSEQUENT_BALL_MAX = 50;
