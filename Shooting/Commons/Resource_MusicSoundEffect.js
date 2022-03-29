/*
	音楽・効果音
*/

/*
	音楽
	Play()関数に渡す。
*/
function @@_Load(url)
{
	LOGPOS;
	Loading++;

	var audio = new Audio(url);

	var loaded = function()
	{
		audio.removeEventListener("canplaythrough", loaded);
		audio.removeEventListener("error", errorLoad);

		LOGPOS;
		Loading--;
	};

	var errorLoad = function()
	{
		error;
	};

	audio.addEventListener("canplaythrough", loaded);
	audio.addEventListener("error", errorLoad);
	audio.load();

	return audio;
}

/*
	効果音
	SE()関数に渡す。
*/
function @@_LoadSE(url)
{
	var ret =
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

var M_Field = @@_Load(Resources.HMIX__n62_mp3);

var S_Explode = @@_LoadSE(Resources.小森平__explosion01_mp3);
