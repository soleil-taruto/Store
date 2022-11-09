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

function <void> AddEffect_Fusion(<double> x, <double> y)
{
	for (var<int> c = 0; c < 10; c++)
	{
		AddEffect(@@_Effect_Star(P_Star_M, x, y, GetRand1() * Math.PI * 2.0, 15.0, 0.666));
	}
}

function <void> AddEffect_BornPanel(<double> x, <double> y)
{
	for (var<int> c = 0; c < 10; c++)
	{
		AddEffect(@@_Effect_Star(P_Star_L, x, y, GetRand1() * Math.PI * 2.0, 23.0, 0.6));
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
