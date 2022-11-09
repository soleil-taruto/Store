/*
	音楽・効果音
*/

/*
	音楽
	Play()関数に渡す。
*/
function <Audio> @@_Load(<string> url)
{
	LOGPOS();
	Loading++;

	var<Audio> audio = new Audio(url);

	var<Action> loaded = function()
	{
		audio.removeEventListener("canplaythrough", loaded);
		audio.removeEventListener("error", errorLoad);

		LOGPOS();
		Loading--;
	};

	var<Action> errorLoad = function()
	{
		error();
	};

	audio.addEventListener("canplaythrough", loaded);
	audio.addEventListener("error", errorLoad);
	audio.load();

	return audio;
}

/@(ASTR)

/// SE_t
{
	<Audio[]> Handles // ハンドルのリスト(5つ)
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

//var<Audio> M_無音 = @@_Load(Resources.General__muon_mp3); // デカいのでロードしない。

//var<SE_t> S_無音 = @@_LoadSE(Resources.General__muon_mp3); // デカいのでロードしない。

// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

var<Audio> M_Stage01     = @@_Load(Resources.HMIX__n138_mp3);
var<Audio> M_Stage02     = @@_Load(Resources.HMIX__n70_mp3);
var<Audio> M_Stage03     = @@_Load(Resources.HMIX__n13_mp3);
var<Audio> M_Ending      = @@_Load(Resources.HMIX__n118_mp3);
var<Audio> M_Stage01Boss = @@_Load(Resources.ユーフルカ__Battle_Vampire_loop_m4a);
var<Audio> M_Stage02Boss = @@_Load(Resources.ユーフルカ__Battle_Conflict_loop_m4a);
var<Audio> M_Stage03Boss = @@_Load(Resources.ユーフルカ__Battle_rapier_loop_m4a);

var<SE_t> S_EnemyDamaged = @@_LoadSE(Resources.出処不明__EnemyDamaged_mp3);
var<SE_t> S_PlayerShoot  = @@_LoadSE(Resources.出処不明__PlayerShoot_mp3);
var<SE_t> S_EnemyDead    = @@_LoadSE(Resources.小森平__explosion01_mp3);
var<SE_t> S_PowerUp      = @@_LoadSE(Resources.小森平__powerup03_mp3);
