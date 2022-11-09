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

var<Picture_t> P_Ball = @@_Load(RESOURCE_Picture__Ball_png);
var<Picture_t> P_弾道 = @@_Load(RESOURCE_Picture__弾道_png);

var<Picture_t> P_Circle_Hard = @@_Load(RESOURCE_Picture__円形ブロック_Hard_png); // 硬い
var<Picture_t> P_Circle_Norm = @@_Load(RESOURCE_Picture__円形ブロック_Norm_png); // 硬さ普通
var<Picture_t> P_Circle_Soft = @@_Load(RESOURCE_Picture__円形ブロック_Soft_png); // 柔らかい

var<Picture_t> P_Square_Hard = @@_Load(RESOURCE_Picture__正方形ブロック_Hard_png); // 硬い
var<Picture_t> P_Square_Norm = @@_Load(RESOURCE_Picture__正方形ブロック_Norm_png); // 硬さ普通
var<Picture_t> P_Square_Soft = @@_Load(RESOURCE_Picture__正方形ブロック_Soft_png); // 柔らかい

var<Picture_t> P_Star_S = @@_Load(RESOURCE_Picture__光る星20_png);
