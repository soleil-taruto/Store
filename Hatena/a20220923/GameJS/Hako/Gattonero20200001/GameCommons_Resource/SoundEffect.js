/*
	効果音
*/

function <SE_t> @@_Load(<string> url)
{
	return LoadSE(url);
}

// ここから各種音楽・効果音

// プリフィクス
// S_ ... 効果音

//var<SE_t> S_無音 = @@_Load(RESOURCE_General__muon_mp3); // デカいのでロードしない。

// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

var<SE_t> S_Jump    = @@_Load(RESOURCE_小森平__jump12_mp3);
var<SE_t> S_Crashed = @@_Load(RESOURCE_小森平__question_mp3);
var<SE_t> S_Dead    = @@_Load(RESOURCE_小森平__explosion05_mp3);
var<SE_t> S_Start   = @@_Load(RESOURCE_小森平__strange_wave_mp3);
var<SE_t> S_Goal    = @@_Load(RESOURCE_小森平__warp1_mp3);

// ----

var<SE_t[]> S_テスト用 =
[
	S_Jump,
	S_Crashed,
	S_Dead,
];

var<SE_t> S_SaveDataRemoved = S_Dead;

// ----
