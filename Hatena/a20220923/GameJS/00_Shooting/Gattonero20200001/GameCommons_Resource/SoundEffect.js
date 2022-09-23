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

var<SE_t> S_EnemyDamaged = @@_Load(RESOURCE_出処不明__EnemyDamaged_mp3);
var<SE_t> S_PlayerShoot  = @@_Load(RESOURCE_出処不明__PlayerShoot_mp3);
var<SE_t> S_EnemyDead    = @@_Load(RESOURCE_小森平__explosion01_mp3);
var<SE_t> S_PowerUp      = @@_Load(RESOURCE_小森平__powerup03_mp3);

// ----

var<SE_t[]> S_テスト用 =
[
	S_EnemyDamaged,
	S_EnemyDead,
	S_PowerUp,
];

var<SE_t> S_SaveDataRemoved = S_EnemyDead;

// ----
