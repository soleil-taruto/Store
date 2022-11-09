/*
	定数
*/

/*
	フィールド領域
*/
var<int> FIELD_L = 0;
var<int> FIELD_T = 100;
var<int> FIELD_W = Screen_W - 0;
var<int> FIELD_H = Screen_H - 200;
var<int> FIELD_R = FIELD_L + FIELD_W;
var<int> FIELD_B = FIELD_T + FIELD_H;

/*
	マップ・サイズ (テーブルサイズ)
*/
var<int> MAP_X_SIZE = 20;
var<int> MAP_Y_SIZE = 15;

/*
	マップセル・サイズ (ドット単位・画面上のサイズ)
*/
var<int> MAP_CELL_W = 40;
var<int> MAP_CELL_H = 40;
