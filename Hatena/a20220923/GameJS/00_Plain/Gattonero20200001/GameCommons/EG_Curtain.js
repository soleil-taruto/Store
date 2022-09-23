/*
	カーテン

	ファイル名 Curtain.js -> EG_Curtain.js の理由：
	本ソースの *_EACH() を Effect.js の *_EACH より後に実行したい。-- エフェクトをカーテンの下へ
*/

var<double[]> @@_NextAfterWhiteLevels = [];
var<double> @@_WhiteLevel = 0.0;

function <void> SetCurtain()
{
	SetCurtain_FD(30, 0.0);
}

function <void> SetCurtain_FD(<int> frameMax, <double> destWhiteLevel) // frameMax: 0～, destWhiteLevel: -1.0 ～ 1.0
{
	@@_NextAfterWhiteLevels = [];

	if (frameMax == 0)
	{
		@@_WhiteLevel = destWhiteLevel;
	}
	else
	{
		for (var<Scene_t> scene of CreateScene(frameMax))
		{
			@@_NextAfterWhiteLevels.push(AToBRate(@@_WhiteLevel, destWhiteLevel, scene.Rate));
		}
	}
}

function <void> @(UNQN)_EACH()
{
	if (1 <= @@_NextAfterWhiteLevels.length)
	{
		@@_WhiteLevel = @@_NextAfterWhiteLevels.shift();
	}

	var<D4Color> color;

	if (@@_WhiteLevel < 0.0)
	{
		color = CreateD4Color(0.0, 0.0, 0.0, @@_WhiteLevel * -1.0);
	}
	else
	{
		color = CreateD4Color(1.0, 1.0, 1.0, @@_WhiteLevel);
	}

	SetColor(I4ColorToString(D4ColorToI4Color(color)));
	PrintRect(0, 0, Screen_W, Screen_H);
}

function <void> DrawCurtain(<double> whiteLevel) // whiteLevel: -1.0 ～ 1.0
{
	var<D4Color> color;

	if (whiteLevel < 0.0)
	{
		color = CreateD4Color(0.0, 0.0, 0.0, whiteLevel * -1.0);
	}
	else
	{
		color = CreateD4Color(1.0, 1.0, 1.0, whiteLevel);
	}

	SetColor(I4ColorToString(D4ColorToI4Color(color)));
	PrintRect(0, 0, Screen_W, Screen_H);
}
