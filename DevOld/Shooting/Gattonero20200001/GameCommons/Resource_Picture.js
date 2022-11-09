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

var<Image> P_Enemy_0001 = @@_Load(Resources.Picture__Enemy0001_png);
var<Image> P_Player     = @@_Load(Resources.Picture__Player_png);
var<Image> P_Shot_0001  = @@_Load(Resources.Picture__Shot0001_png);
var<Image> P_Tama_0001  = @@_Load(Resources.Picture__Tama0001_png);
var<Image> P_Wall_0002  = @@_Load(Resources.Picture__Wall0002_png);
var<Image[]> P_Explode =
[
	@@_Load(Resources.ぴぽや倉庫__Explode__0001_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0002_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0003_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0004_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0005_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0006_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0007_png),
	@@_Load(Resources.ぴぽや倉庫__Explode__0008_png),
];
