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

function <void> AddEffect_TouchGround(<double> x, <double> y, <int> direction) // direction: { 2, 4, 6, 8 }
{
	var<double> angleBgn;
	var<double> angleEnd;

	if (direction == 2) // 下
	{
		angleBgn = (Math.PI / 4) * 1;
		angleEnd = (Math.PI / 4) * 3;
	}
	else if(direction == 4) // 左
	{
		angleBgn = (Math.PI / 4) * 3;
		angleEnd = (Math.PI / 4) * 5;
	}
	else if(direction == 6) // 右
	{
		angleBgn = (Math.PI / 4) * 7;
		angleEnd = (Math.PI / 4) * 9;
	}
	else if(direction == 8) // 上
	{
		angleBgn = (Math.PI / 4) * 5;
		angleEnd = (Math.PI / 4) * 7;
	}
	else
	{
		error();
	}

	var<int> c_max = 2 + GetRand(2);

	for (var<int> c = 0; c < c_max; c++)
	{
		var<double> angle = angleBgn + GetRand1() * (angleEnd - angleBgn);

		AddEffect(@@_Effect_Star(P_Star_S, x, y, angle, 13.0 + 7.0 * GetRand1(), 0.7));
	}
}

function* <generatorForTask> @@_Effect_Star(<Picture_t> picture, <double> x, <double> y, <double> angle, <double> speed, <double> speedRate)
{
	var<double> rot = GetRand1() * Math.PI * 2.0;
	var<double> rotAdd = GetRand2() * 0.5;
	var<double> rotAddRate = 0.9;

	for (var<Scene_t> scene of CreateScene(20))
	{
		var<D2Point_t> pt = AngleToPoint(angle, speed);

		x += pt.X;
		y += pt.Y;

		speed *= speedRate;

		Draw(picture, x, y, 0.666 * scene.RemRate, rot, 1.0);

		rot += rotAdd;
		rotAdd *= rotAddRate;

		yield 1;
	}
}

/*
	パネル融合エフェクト

	(x, y): 融合後の描画位置
	exponent: 融合後の数字インデックス
	direction: 融合前のパネルがある方向(2468)
*/
function <void> AddEffect_Fusion(<double> x, <double> y, <int> exponent, <int> direction)
{
	var<int> dx = 0;
	var<int> dy = 0;

	if (direction == 2)
	{
		dy++;
	}
	else if (direction == 4)
	{
		dx--;
	}
	else if (direction == 6)
	{
		dx++;
	}
	else if (direction == 8)
	{
		dy--;
	}
	else
	{
		error();
	}

	AddEffect(function* ()
	{
		for (var<Scene_t> scene of CreateScene(20))
		{
			Draw(
				P_数字パネル[exponent - 1],
				x + dx * Cell_W * scene.RemRate,
				y + dy * Cell_H * scene.RemRate,
				0.5,
				Math.PI * scene.Rate,
				1.0
				);

			yield 1;
		}
	}());

	AddEffect(function* ()
	{
		for (var<Scene_t> scene of CreateScene(30))
		{
			Draw(
				P_数字パネル[exponent],
				x,
				y,
				0.6 * scene.RemRate,
				0.0,
				1.0 + 1.0 * scene.Rate
				);

			yield 1;
		}
	}());
}

/*
	パネル生成エフェクト

	(x, y): 描画位置
	exponent: 生成するパネルの数字インデックス
*/
function <void> AddEffect_BornPanel(<double> x, <double> y, <int> exponent)
{
	AddEffect(function* ()
	{
		var<double> rot = GetRand2() * 7.0;

		// オートモード時は早めに終わる。
		// -- オートモードの時はすぐに落下するため、エフェクトが残っていると違和感があるため。

		for (var<Scene_t> scene of CreateScene(AutoMode ? 10 : 20))
		{
			Draw(
				P_数字パネル[exponent],
				x,
				y,
				scene.Rate,
				rot,
				1.0 + 0.5 * scene.RemRate
				);

			rot *= 0.7;

			yield 1;
		}
	}());
}
