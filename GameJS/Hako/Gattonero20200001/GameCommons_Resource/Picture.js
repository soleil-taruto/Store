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

var<Picture_t> P_GameStartButton = @@_Load(RESOURCE_Picture__GameStartButton_png);
var<Picture_t> P_Star_S = @@_Load(RESOURCE_Picture__光る星20_png);
var<Picture_t> P_Player = @@_Load(RESOURCE_Picture__Player_png);
var<Picture_t> P_Wall = @@_Load(RESOURCE_Picture__Wall_png);
var<Picture_t> P_Goal = @@_Load(RESOURCE_Picture__Goal_png);
var<Picture_t> P_背景 = @@_Load(RESOURCE_Picture__Background_png);
var<Picture_t> P_Enemy_B = @@_Load(RESOURCE_Picture__敵青_png);
var<Picture_t> P_Enemy_R = @@_Load(RESOURCE_Picture__敵赤_png);
var<Picture_t> P_Enemy_G = @@_Load(RESOURCE_Picture__敵緑_png);

var<Picture_t> P_TitleLogo    = @@_Load(RESOURCE_Picture__TitleLogo_png);
var<Picture_t> P_EndingString = @@_Load(RESOURCE_Picture__EndingString_png);
