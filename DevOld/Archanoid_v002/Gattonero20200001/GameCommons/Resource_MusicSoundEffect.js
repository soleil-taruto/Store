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
	<Audio[]> Handles // ハンドルのリスト(3つ)
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
		// ハンドルのリスト(3つ)
		Handles:
		[
			@@_Load(url),
			@@_Load(url),
			@@_Load(url),
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
