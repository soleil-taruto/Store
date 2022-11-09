/*
	音楽・効果音
*/

/*
	音楽
	Play()関数に渡す。
*/
function <Sound_t> @@_Load(<string> url)
{
	return LoadSound(url);
}

/@(ASTR)

/// SE_t
{
	<Sound_t[]> Handles // ハンドルのリスト(5つ)
	<int> Index // 次に再生するハンドルの位置
}

@(ASTR)/

/*
	効果音
	SE()関数に渡す。
*/
function <SE_t> @@_LoadSE(<string> url)
{
	var<SE_t> ret =
	{
		// ハンドルのリスト(5つ)
		Handles:
		[
			@@_Load(url), // 1
			@@_Load(url), // 2
			@@_Load(url), // 3
			@@_Load(url), // 4
			@@_Load(url), // 5
		],

		// 次に再生するハンドルの位置
		Index: 0,
	};

	return ret;
}

// ここから各種音楽・効果音

// プリフィクス
// M_ ... 音楽,BGM
// S_ ... 効果音(SE)

//var<Sound_t> M_無音 = @@_Load(RESOURCE_General__muon_mp3); // デカいのでロードしない。

//var<SE_t> S_無音 = @@_LoadSE(RESOURCE_General__muon_mp3); // デカいのでロードしない。

// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

var<Sound_t> M_Title  = @@_Load(RESOURCE_DovaSyndrome__Hanabi_mp3);
var<Sound_t> M_Field  = @@_Load(RESOURCE_DovaSyndrome__Midnight_Street_mp3);
var<Sound_t> M_Boss   = @@_Load(RESOURCE_DovaSyndrome__Battle_Fang_mp3);
var<Sound_t> M_Ending = @@_Load(RESOURCE_DovaSyndrome__Kindly_mp3);

var<SE_t> S_Jump    = @@_LoadSE(RESOURCE_小森平__jump12_mp3);
var<SE_t> S_Crashed = @@_LoadSE(RESOURCE_小森平__question_mp3);
var<SE_t> S_Dead    = @@_LoadSE(RESOURCE_小森平__explosion05_mp3);
var<SE_t> S_Clear   = @@_LoadSE(RESOURCE_小森平__warp1_mp3);
var<SE_t> S_Shoot   = @@_LoadSE(RESOURCE_出処不明__PlayerShoot_mp3);
var<SE_t> S_Damaged = @@_LoadSE(RESOURCE_小森平__damage5_mp3);
var<SE_t> S_EnemyDamaged = @@_LoadSE(RESOURCE_出処不明__EnemyDamaged_mp3);
var<SE_t> S_EnemyDead    = @@_LoadSE(RESOURCE_小森平__explosion01_mp3);
var<SE_t> S_BossDead     = @@_LoadSE(RESOURCE_小森平__game_explosion2_mp3);
