/*
	自弾 - BDummy

	★サンプルとしてキープ
*/

var<int> ShotKind_BDummy = @(AUTO);

function <Shot_t> CreateShot_BDummy(<doule> x, <double> y, <double> xAdd, <double> yAdd)
{
	var ret =
	{
		Kind: ShotKind_BDummy,
		X: x,
		Y: y,
		AttackPoint: 1,
		Crash: null,

		// ここから固有

		<double> XAdd: xAdd,
		<double> YAdd: yAdd,
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Shot_t> shot)
{
	for (; ; )
	{
		shot.X += shot.XAdd;
		shot.Y += shot.YAdd;

		if (IsOut(
			CreateD2Point(shot.X, shot.Y),
			CreateD4Rect(FIELD_L, FIELD_T, FIELD_W, FIELD_H),
			0.0
			))
		{
			break;
		}

		shot.Crash = CreateCrash_Circle(shot.X, shot.Y, 25.0);

		Draw(P_Dummy, shot.X, shot.Y, 1.0, ProcFrame / 20.0, 1.0);

		yield 1;
	}
}

function <void> @@_Dead(<Shot_t> shot)
{
	ShotCommon_Dead(shot);
}
