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

var<Image> P_数字パネル =
[
	@@_Load(Resources.数字パネル__n1_png),
	@@_Load(Resources.数字パネル__n2_png),
	@@_Load(Resources.数字パネル__n4_png),
	@@_Load(Resources.数字パネル__n8_png),
	@@_Load(Resources.数字パネル__n16_png),
	@@_Load(Resources.数字パネル__n32_png),
	@@_Load(Resources.数字パネル__n64_png),
	@@_Load(Resources.数字パネル__n128_png),
	@@_Load(Resources.数字パネル__n256_png),
	@@_Load(Resources.数字パネル__n512_png),
	@@_Load(Resources.数字パネル__n1024_png),
	@@_Load(Resources.数字パネル__n2048_png),
	@@_Load(Resources.数字パネル__n4096_png),
	@@_Load(Resources.数字パネル__n8192_png),
	@@_Load(Resources.数字パネル__n16384_png),
	@@_Load(Resources.数字パネル__n32768_png),
	@@_Load(Resources.数字パネル__n65536_png),
	@@_Load(Resources.数字パネル__n131072_png),
	@@_Load(Resources.数字パネル__n262144_png),
	@@_Load(Resources.数字パネル__n524288_png),
	@@_Load(Resources.数字パネル__n1048576_png),
	@@_Load(Resources.数字パネル__n2097152_png),
	@@_Load(Resources.数字パネル__n4194304_png),
	@@_Load(Resources.数字パネル__n8388608_png),
	@@_Load(Resources.数字パネル__n16777216_png),
	@@_Load(Resources.数字パネル__n33554432_png),
	@@_Load(Resources.数字パネル__n67108864_png),
	@@_Load(Resources.数字パネル__n134217728_png),
	@@_Load(Resources.数字パネル__n268435456_png),
	@@_Load(Resources.数字パネル__n536870912_png),
	@@_Load(Resources.数字パネル__n1073741824_png),
	@@_Load(Resources.数字パネル__n2147483648_png),
	@@_Load(Resources.数字パネル__n4294967296_png),
	@@_Load(Resources.数字パネルex__n2p33_png),
	@@_Load(Resources.数字パネルex__n2p34_png),
	@@_Load(Resources.数字パネルex__n2p35_png),
	@@_Load(Resources.数字パネルex__n2p36_png),
	@@_Load(Resources.数字パネルex__n2p37_png),
	@@_Load(Resources.数字パネルex__n2p38_png),
	@@_Load(Resources.数字パネルex__n2p39_png),
	@@_Load(Resources.数字パネルex__n2p40_png),
	@@_Load(Resources.数字パネルex__n2p41_png),
	@@_Load(Resources.数字パネルex__n2p42_png),
	@@_Load(Resources.数字パネルex__n2p43_png),
	@@_Load(Resources.数字パネルex__n2p44_png),
	@@_Load(Resources.数字パネルex__n2p45_png),
	@@_Load(Resources.数字パネルex__n2p46_png),
	@@_Load(Resources.数字パネルex__n2p47_png),
	@@_Load(Resources.数字パネルex__n2p48_png),
	@@_Load(Resources.数字パネルex__n2p49_png),
	@@_Load(Resources.数字パネルex__n2p50_png),
	@@_Load(Resources.数字パネルex__n2p51_png),
	@@_Load(Resources.数字パネルex__n2p52_png),
	@@_Load(Resources.数字パネルex__n2p53_png),
	@@_Load(Resources.数字パネルex__n2p54_png),
	@@_Load(Resources.数字パネルex__n2p55_png),
	@@_Load(Resources.数字パネルex__n2p56_png),
	@@_Load(Resources.数字パネルex__n2p57_png),
	@@_Load(Resources.数字パネルex__n2p58_png),
	@@_Load(Resources.数字パネルex__n2p59_png),
	@@_Load(Resources.数字パネルex__n2p60_png),
	@@_Load(Resources.数字パネルex__n2p61_png),
	@@_Load(Resources.数字パネルex__n2p62_png),
	@@_Load(Resources.数字パネルex__n2p63_png),
	@@_Load(Resources.数字パネルex__n2p64_png),
];

var<Image> P_Gravity = @@_Load(Resources.Picture__Gravity_png);

var<Image> P_Star_S = @@_Load(Resources.Picture__光る星20_png);
