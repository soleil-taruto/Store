/*
	固有エフェクト
*/

/*
	エフェクト・テスト (★サンプルとしてキープ)

	実行例：
		AddCommonEffect(Effect_Test(400, 300));

	x: 中心-X
	y: 中心-Y
*/
function* Effect_Test(x, y)
{
	for (var scene of CreateScene(30))
	{
		Draw(P_Dummy, x, y, 1.0, scene.Rate * Math.PI * 2.0, 1.0);

		yield 1;
	}
}
