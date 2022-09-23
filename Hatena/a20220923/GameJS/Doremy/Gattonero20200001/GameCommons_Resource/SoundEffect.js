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
var<SE_t> S_Clear   = @@_Load(RESOURCE_小森平__warp1_mp3);
var<SE_t> S_Shoot   = @@_Load(RESOURCE_出処不明__PlayerShoot_mp3);
var<SE_t> S_Damaged = @@_Load(RESOURCE_小森平__damage5_mp3);
var<SE_t> S_EnemyDamaged = @@_Load(RESOURCE_出処不明__EnemyDamaged_mp3);
var<SE_t> S_EnemyDead    = @@_Load(RESOURCE_小森平__explosion01_mp3);
var<SE_t> S_BossDead     = @@_Load(RESOURCE_小森平__game_explosion2_mp3);

// ----

var<SE_t[]> S_テスト用 =
[
	S_Jump,
	S_Damaged,
	S_Dead,
];

var<SE_t> S_SaveDataRemoved = S_Dead;

// ----
