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

var<Image> P_軌道 = @@_Load(Resources.Picture__軌道_png);

var<Image[]> P_Balls =
[
	@@_Load(Resources.Picture__00ff00_png),
	@@_Load(Resources.Picture__00ffff_png),
	@@_Load(Resources.Picture__0080ff_png),
	@@_Load(Resources.Picture__ff0000_png),
	@@_Load(Resources.Picture__ff00ff_png),
	@@_Load(Resources.Picture__ff8000_png),
	@@_Load(Resources.Picture__ffff00_png),
	@@_Load(Resources.Picture__ffffff_png),
];

var<Image> P_Star_S = @@_Load(Resources.Picture__光る星20_png);
