/*
	敵 - スタート地点
*/

var<int> EnemyKind_Goal = @(AUTO);

function <Enemy_t> CreateEnemy_Goal(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_Goal,
		X: x,
		Y: y,
		HP: 0,
		AttackPoint: 0,
		HitDie: false,
		Crash: null,

		// ここから固有
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
		var<double> ZURE_X = 0.0;
		var<double> ZURE_Y = -10.0;

		var<double> x = enemy.X + ZURE_X;
		var<double> y = enemy.Y + ZURE_Y;

		if (GetDistanceLessThan(x - PlayerX, y - PlayerY, 35.0))
		{
			GameRequestStageClear = true;
		}

		Draw(P_Goal, x - Camera.X, y - Camera.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	// none
}

function <void> @@_Dead(<Enemy_t> enemy, <Shot_t> shot)
{
	// none
}
