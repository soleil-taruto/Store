/*
	音楽再生・停止
*/

var<int> @@_State = 0; // 0 == 停止中, 1 == 再生中, 2 == フェードアウト中, 3 == 曲停止, 4 == 次の曲を再生
var<Sound_t> @@_Music = null;
var<Sound_t> @@_NextMusic = null;

// 再生
// music: 曲
function <void> Play(<Sound_t> music)
{
	if (!music)
	{
		error(); // Bad music
	}

	if (@@_State == 1 && @@_Music == music) // ? 同じ曲を再生中 -> 何もしない。
	{
		return;
	}
	if (@@_State != 0) // ? 停止中ではない。-> フェードアウトしてから再生
	{
		if (@@_State == 1) // ? 再生中
		{
			Fadeout();
		}
		@@_NextMusic = music;
		return;
	}

	music.Handle.loop = true;
	music.Handle.currentTime = 0;
	music.Handle.volume = 1.0;
	music.Handle.play();

	@@_State = 1;
	@@_Music = music;
}

var<int> @@_FadeoutFrame;
var<double> @@_Volume;

function <void> @(UNQN)_EACH()
{
	if (@@_State == 0) // ? 停止中
	{
		// noop
	}
	else if (@@_State == 1) // ? 再生中
	{
		// noop
	}
	else if (@@_State == 2) // ? フェードアウト中
	{
		@@_Volume -= 1.0 / @@_FadeoutFrame;

		if (@@_Volume < 0.0)
		{
			@@_Volume = 0.0;
			@@_State = 3;
		}
		@@_Music.Handle.volume = @@_Volume;
	}
	else if (@@_State == 3) // ? 曲停止
	{
		@@_State = 4;

		@@_Music.Handle.pause();
		@@_Music = null;
	}
	else if (@@_State == 4) // ? 次の曲を再生
	{
		@@_State = 0;

		if (@@_NextMusic) // ? 次の曲_有り
		{
			var music = @@_NextMusic;

			@@_NextMusic = null;

			Play(music);
		}
	}
	else
	{
		error(); // Bad @@_State
	}
}

// フェードアウト
function <void> Fadeout()
{
	Fadeout_F(30);
}

// フェードアウト
// frame: 1～
function <void> Fadeout_F(<int> frame)
{
	if (frame < 1 || !Number.isInteger(frame))
	{
		error(); // Bad frame
	}

	if (@@_State == 0) // ? 停止中 -> nop
	{
		return;
	}
	if (2 <= @@_State) // ? フェードアウト中
	{
		@@_NextMusic = null; // フェードアウト中にフェードアウトしようとした。-> 次の曲をキャンセル
		return;
	}

	@@_State = 2;
	@@_FadeoutFrame = frame;
	@@_Volume = 1.0;
}
