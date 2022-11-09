/*
	敵 - BDummy

	★サンプルとしてキープ
*/

var<int> EnemyKind_BDummy = @(AUTO);

function <Enemy_t> CreateEnemy_BDummy(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: EnemyKind_BDummy,
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
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	for (; ; )
	{
		enemy.Y += 2.0;

		if (FIELD_B < enemy.Y)
		{
			break;
		}

		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, 50.0, 50.0));

		Draw(P_Dummy, enemy.X, enemy.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_Dead(enemy);
}
