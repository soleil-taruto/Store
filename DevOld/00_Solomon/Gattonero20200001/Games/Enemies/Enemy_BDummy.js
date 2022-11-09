/*
	敵 - BDummy

	★サンプルとしてキープ
*/

function <Enemy_t> CreateEnemy_BDummy(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: Enemy_Kind_e_BDummy,
		X: x,
		Y: y,
		HP: hp,
		Crash: null,

		// ここから固有

		<double> Dummy_01: 1.0,
		<double> Dummy_02: 2.0,
		<double> Dummy_03: 3.0,
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	for (; ; )
	{
		enemy.Y += 2.0;

		if (Screen_H < enemy.Y)
		{
			break;
		}

		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, 50.0, 50.0));

		Draw(P_Dummy, enemy.X, enemy.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_Dead(enemy);
}
