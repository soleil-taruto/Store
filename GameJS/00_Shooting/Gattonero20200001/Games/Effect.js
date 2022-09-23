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
		Draw(P_Dummy, x, y, 0.5 - 0.5 * scene.Rate, scene.Rate * Math.PI, 1.0);

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

			for (var<Scene_t> scene of CreateScene(20))
			{
				Draw(
					P_ExplodePiece,
					pt.X,
					pt.Y,
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

function <void> AddEffect_ShotExplode(<double> x, <double> y)
{
	AddEffect(function* <generatorForTask> ()
	{
		var<double> rot = GetRand1() * Math.PI * 2;

		for (var<Scene_t> scene of CreateScene(10))
		{
			Draw(
				P_Shot0001,
				x,
				y,
				scene.RemRate,
				rot,
				1.0 + 4.0 * scene.Rate
				);

			yield 1;
		}
	}());
}

function <void> AddEffect_TamaExplode(<double> x, <double> y)
{
	AddEffect(function* <generatorForTask> ()
	{
		var<double> rot = GetRand1() * Math.PI * 2;

		for (var<Scene_t> scene of CreateScene(10))
		{
			Draw(
				P_Tama0001,
				x,
				y,
				scene.RemRate,
				rot,
				1.0 + 4.0 * scene.Rate
				);

			yield 1;
		}
	}());
}
