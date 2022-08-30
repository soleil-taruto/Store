/*
	効果音読み込み
*/

/@(ASTR)

/// SE_t
{
	<Sound_t[]> Handles // ハンドルのリスト
	<int> Index // 次に再生するハンドルの位置
}

@(ASTR)/

/*
	効果音
	SE()関数に渡す。
*/
function <SE_t> LoadSE(<string> url)
{
	var<SE_t> ret =
	{
		Handles:
		[
			LoadSound(url), // 1
			LoadSound(url), // 2
			LoadSound(url), // 3
			LoadSound(url), // 4
			LoadSound(url), // 5
		],

		Index: 0,
	};

	return ret;
}
