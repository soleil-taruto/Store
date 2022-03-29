/*
	固有エフェクト
*/

/*
	爆発

	x: 中心-X
	x: 中心-Y
*/
function* Effect_Explode(x, y)
{
	for (var image of P_Explode)
	for (var c = 0; c < 4; c++)
	{
		Draw(image, x, y - 50, 1.0, 0.0, 1.0);

		yield 1;
	}
}

/*
	自機消滅

	x: 中心-X
	x: 中心-Y
*/
function* Effect_PlayerDead(x, y)
{
	var FRM_NUM = 60;

	for (var c = 0; c < FRM_NUM; c++)
	{
		var rate = c / FRM_NUM;

		Draw(P_Player, x, y,
			1.0 - rate,
			0.0 + rate * 6.0,
			1.0 + rate * 4.0
			);

		yield 1;
	}
}
