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

var<Image> P_Star_S = @@_Load(Resources.Picture__光る星20_png);
var<Image> P_Player = @@_Load(Resources.Picture__Player_png);
var<Image> P_Wall = @@_Load(Resources.Picture__Wall_png);
var<Image> P_Goal = @@_Load(Resources.Picture__Goal_png);
var<Image> P_背景 = @@_Load(Resources.Picture__Background_png);
var<Image> P_Enemy_B = @@_Load(Resources.Picture__敵青_png);
var<Image> P_Enemy_R = @@_Load(Resources.Picture__敵赤_png);
var<Image> P_Enemy_G = @@_Load(Resources.Picture__敵緑_png);
