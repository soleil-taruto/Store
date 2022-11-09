/*
	固有エフェクト
*/

/*
	爆発

	x: 中心-X
	y: 中心-Y
*/
function* <generatorForTask> Effect_Explode(<double> x, <double> y)
{
	for (var<Picture_t> image of P_Explode)
	for (var<int> c = 0; c < 4; c++)
	{
		Draw(image, x, y - 50, 1.0, 0.0, 1.0);

		yield 1;
	}
}

/*
	自機消滅

	x: 中心-X
	y: 中心-Y
*/
function* <generatorForTask> Effect_PlayerDead(<double> x, <double> y)
{
	var<int> FRM_NUM = 60;

	for (var<int> c = 0; c < FRM_NUM; c++)
	{
		var<double> rate = c / FRM_NUM;

		Draw(P_Player, x, y,
			1.0 - rate,
			0.0 + rate * 6.0,
			1.0 + rate * 4.0
			);

		yield 1;
	}
}
