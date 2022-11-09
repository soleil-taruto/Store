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

	var<map> m = {};

	m.Handle = new Audio(url);
	m.TryLoadCount = 0;

	if (DEBUG)
	{
		m.Handle.load();
		Loading--;
	}
	else
	{
		@@_Standby(m);
	}
	return m.Handle;
}

function <void> @@_Standby(<map> m)
{
	setTimeout(
		function()
		{
			@@_TryLoad(m);
		},
		100
		);
}

var<boolean> @@_Loading = false;

function <void> @@_TryLoad(<map> m)
{
	if (@@_Loading)
	{
		@@_Standby(m);
		return;
	}
	@@_Loading = true;

	m.Loaded = function()
	{
		m.Handle.removeEventListener("canplaythrough", m.Loaded);
		m.Handle.removeEventListener("error", m.Errored);

		m.Loaded = null;
		m.Errored = null;

		LOGPOS();
		Loading--;
		@@_Loading = false;
	};

	m.Errored = function()
	{
		m.Handle.removeEventListener("canplaythrough", m.Loaded);
		m.Handle.removeEventListener("error", m.Errored);

		m.Loaded = null;
		m.Errored = null;

		if (m.TryLoadCount < 10) // rough limit
		{
			LOGPOS();
			@@_Standby(m);
			@@_Loading = false;
		}
		else
		{
			LOGPOS();
			error();
		}
	};

	m.Handle.addEventListener("canplaythrough", m.Loaded);
	m.Handle.addEventListener("error", m.Errored);
	m.Handle.load();
	m.TryLoadCount++;
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

var<Audio> M_Field = @@_Load(Resources.HMIX__n62_mp3);

var<SE_t> S_EnemyDamaged = @@_LoadSE(Resources.出処不明__EnemyDamaged_mp3);
var<SE_t> S_PlayerShoot = @@_LoadSE(Resources.出処不明__PlayerShoot_mp3);
var<SE_t> S_EnemyDead = @@_LoadSE(Resources.小森平__explosion01_mp3);
var<SE_t> S_PowerUp = @@_LoadSE(Resources.小森平__powerup03_mp3);
