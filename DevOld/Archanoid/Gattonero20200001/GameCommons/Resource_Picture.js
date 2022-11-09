/*
	画像
*/

function <Image> @@_Load(<string> url)
{
	LOGPOS();
	Loading++;

	var image = new Image();

	image.src = url;
	image.onload = function()
	{
		LOGPOS();
		Loading--;
	};

	image.onerror = function()
	{
		error();
	};

	return image;
}

// ここから各種画像

// プリフィクス
// P_ ... 画像

var<Image> P_Dummy = @@_Load(Resources.General__Dummy_png);
var<Image> P_WhiteBox = @@_Load(Resources.General__WhiteBox_png);
var<Image> P_WhiteCircle = @@_Load(Resources.General__WhiteCircle_png);

// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

var<Image> P_Ball = @@_Load(Resources.Picture__Ball_png);
var<Image> P_弾道 = @@_Load(Resources.Picture__弾道_png);

var<Image> P_Circle_Hard = @@_Load(Resources.Picture__円形ブロック_Hard_png); // 硬い
var<Image> P_Circle_Norm = @@_Load(Resources.Picture__円形ブロック_Norm_png); // 硬さ普通
var<Image> P_Circle_Soft = @@_Load(Resources.Picture__円形ブロック_Soft_png); // 柔らかい

var<Image> P_Square_Hard = @@_Load(Resources.Picture__正方形ブロック_Hard_png); // 硬い
var<Image> P_Square_Norm = @@_Load(Resources.Picture__正方形ブロック_Norm_png); // 硬さ普通
var<Image> P_Square_Soft = @@_Load(Resources.Picture__正方形ブロック_Soft_png); // 柔らかい

var<Image> P_Star_S = @@_Load(Resources.Picture__光る星20_png);
