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
	<Sound_t[]> Handles // ハンドルのリスト(3つ)
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

var<Sound_t> M_Field = @@_Load(RESOURCE_HMIX__n62_mp3);

var<SE_t> S_Explode = @@_LoadSE(RESOURCE_小森平__explosion01_mp3);
