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

			for (var<Scene_t> scene of CreateScene(20))
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

function* <generatorForTask> Effect_Jump(<double> x, <double> y)
{
	for (var<Scene_t> scene of CreateScene(8))
	{
		SetColor(I4ColorToString(D4ColorToI4Color(CreateD4Color(0.5, 0.5, 1.0, 1.0 - scene.Rate * 0.25))));

		PrintCircle(x - Camera.X, y - Camera.Y, 5.0 + scene.Rate * 25.0);

		yield 1;
	}
}

function* <generatorForTask> Effect_Explode_S(<double> x, <double> y)
{
	for (var<Scene_t> scene of CreateScene(5))
	{
		SetColor(I4ColorToString(D4ColorToI4Color(CreateD4Color(0.0, 1.0, 1.0, 1.0 - scene.Rate * 0.5))));

		PrintCircle(x - Camera.X, y - Camera.Y, 3.0 + scene.Rate * 17.0);

		yield 1;
	}
}

function* <generatorForTask> Effect_Explode_M(<double> x, <double> y)
{
	for (var<Scene_t> scene of CreateScene(10))
	{
		SetColor(I4ColorToString(D4ColorToI4Color(CreateD4Color(1.0, 0.75, 0.0, 1.0 - scene.Rate * 0.5))));

		PrintCircle(x - Camera.X, y - Camera.Y, 5.0 + scene.Rate * 25.0);

		yield 1;
	}
}

function* <generatorForTask> Effect_Explode_L(<double> x, <double> y)
{
	for (var<Scene_t> scene of CreateScene(20))
	{
		SetColor(I4ColorToString(D4ColorToI4Color(CreateD4Color(1.0, 0.25, 1.0, 1.0 - scene.Rate * 0.25))));

		PrintCircle(x - Camera.X, y - Camera.Y, 5.0 + scene.Rate * 345.0);

		yield 1;
	}
}

function* <generatorForTask> Effect_ヒットバック(<double> x, <double> y, <boolean> facingLeft)
{
	for (var<Picture_t> picture of (facingLeft ? P_PlayerMirrorEffectShockB : P_PlayerEffectShockB))
	{
		for (var<int> c = 0; c < 6; c++)
		{
			Draw(picture, x - Camera.X, y - Camera.Y, 1.0, 0.0, 1.0);

			yield 1;
		}
	}
}

function* <generatorForTask> Effect_Sliding(<double> x, <double> y, <boolean> facingLeft)
{
	for (var<Picture_t> picture of (facingLeft ? P_PlayerMirrorEffectSliding : P_PlayerEffectSliding))
	{
		for (var<int> c = 0; c < 4; c++)
		{
			Draw(picture, x - Camera.X, y - Camera.Y, 1.0, 0.0, 1.0);

			yield 1;
		}
	}
}
