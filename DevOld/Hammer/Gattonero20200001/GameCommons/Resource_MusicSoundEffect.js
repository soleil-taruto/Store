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
