/*
	固有エフェクト
*/

/*
	ダミーエフェクト

	追加方法：
		AddEffect(Effect_Dummy(x, y));

	★サンプルとしてキープ
*/
function* <generatorForTask> Effect_Dummy(<double> x, <double> y)
{
	for (var<Scene_t> scene of CreateScene(30))
	{
		Draw(P_Dummy, x - Camera.X, y - Camera.Y, 0.5 - 0.5 * scene.Rate, scene.Rate * Math.PI, 1.0);

		yield 1;
	}
}

function <void> AddEffect_Explode(<double> x, <double> y) // 汎用爆発
{
	for (var<int> c = 0; c < 30; c++)
	{
		AddEffect(function* <generatorForTask> ()
		{
			var<D2Point_t> pt = CreateD2Point(x, y);
			var<D2Point_t> speed = AngleToPoint(GetRand1() * Math.PI * 2, 17.0);
			var<double> rot = GetRand1() * Math.PI * 2;
			var<double> rotAdd = GetRand2() * 0.1;

			for (var<Scene_t> scene of CreateScene(40))
			{
				Draw(
					P_ExplodePiece,
					pt.X - Camera.X,
					pt.Y - Camera.Y,
					scene.RemRate,
					rot,
					1.0
					);

				pt.X += speed.X;
				pt.Y += speed.Y;

				speed.X *= 0.8;
				speed.Y *= 0.8;

				rot += rotAdd;

				rotAdd *= 0.8;

				yield 1;
			}
		}());
	}
}

var<boolean> Effect_BattleResult_DeadFlag = false;

function* <generatorForTask> Effect_BattleResult(<string> strResult, <I3Color_t> backColor)
{
	for (var<Scene_t> scene of CreateScene(30))
	{
		@@_DrawBar(scene.RemRate, backColor);

		yield 1;
	}
	while (!Effect_BattleResult_DeadFlag)
	{
		@@_DrawBar(0.0, backColor);

		SetColor("#ffffff");
		SetFSize(160);
		SetPrint(ToFix((Screen_W - GetPrintLineWidth(strResult)) / 2), Screen_H / 2 + 60, 0);
		PrintLine(strResult);

		yield 1;
	}
	for (var<Scene_t> scene of CreateScene(30))
	{
		@@_DrawBar(scene.Rate, backColor);

		yield 1;
	}
}

function <void> @@_DrawBar(<double> zureRate, <I3Color_t> backColor)
{
	for (var<int> index = 0; index < 10; index++)
	{
		@@_DrawBar2(zureRate, backColor, index);
	}
}

function <void> @@_DrawBar2(<double> zureRate, <I3Color_t> backColor, <int> barIndex)
{
	var<double> wave = Math.sin(
		barIndex * 0.11 * ProcFrame * AToBRate(0.01, 1.0, zureRate) +
		barIndex * 0.31
		);

	var<double> h = 300.0 * AToBRate(0.2, 1.0, 1.0 - zureRate);
	var<double> y = Screen_H / 2 + 600.0 * wave * AToBRate(0.05, 1.0, zureRate);

	SetColor(I4ColorToString(I3ColorToI4Color(backColor, 32)));
	PrintRect(0, y - h / 2, Screen_W, h);
}
