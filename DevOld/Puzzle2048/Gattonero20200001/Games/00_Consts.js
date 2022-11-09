/*
	定数
*/

// セル幅(px)
var<int> Cell_W = 60;

// セル高さ(px)
var<int> Cell_H = 60;

// フィールド横方向のセル数
var<int> Field_XNum = 7;

// フィールド縦方向のセル数
var<int> Field_YNum = 7;

// フィールド幅(px)
var<int> Field_W = Cell_W * Field_XNum;

// フィールド高さ(px)
var<int> Field_H = Cell_H * Field_YNum;

// フィールド左側(px)
var<int> Field_L = ToInt((Screen_W - Field_W) / 2);

// フィールド上側(px)
var<int> Field_T = ToInt((Screen_H - Field_H) / 2);
