/*
	音楽再生・停止
*/

var @@_State = 0; // 0 == 停止中, 1 == 再生中, 2 == フェードアウト中, 3 == 曲停止, 4 == 次の曲を再生
var @@_Music = null;
var @@_NextMusic = null;

// 再生
// music: 曲
function Play(music)
{
	if (!music)
	{
		error; // Bad music
	}

	if (@@_State == 1 && @@_Music == music) // ? 同じ曲を再生中 -> 何もしない。
	{
		return;
	}
	if (@@_State != 0) // ? 停止中ではない。-> フェードアウトしてから再生
	{
		if (@@_State == 1) // ? 再生中
		{
			Fadeout(60);
		}
		@@_NextMusic = music;
		return;
	}

	music.loop = true;
	music.currentTime = 0;
	music.volume = 1.0;
	music.play();

	@@_State = 1;
	@@_Music = music;
}

var @@_FadeoutFrame;
var @@_Volume;

function @(UNQN)_EACH()
{
	if (@@_State == 0) // ? 停止中
	{
		// nop
	}
	else if (@@_State == 1) // ? 再生中
	{
		// nop
	}
	else if (@@_State == 2) // ? フェードアウト中
	{
		@@_Volume -= 1.0 / @@_FadeoutFrame;

		if (@@_Volume < 0.0)
		{
			@@_Volume = 0.0;
			@@_State = 3;
		}
		@@_Music.volume = @@_Volume;
	}
	else if (@@_State == 3) // ? 曲停止
	{
		@@_State = 4;

		@@_Music.pause();
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
		error; // Bad @@_State
	}
}

// フェードアウト
// frame: 1～
function Fadeout(frame)
{
	if (frame < 1 || !Number.isInteger(frame))
	{
		error; // Bad frame
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
