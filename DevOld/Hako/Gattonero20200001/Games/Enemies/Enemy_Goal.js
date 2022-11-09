/*
	ìG - ÉSÅ[Éã
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
		Crash: null,

		// Ç±Ç±Ç©ÇÁå≈óL
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	for (var<int> frame = 0; ; frame++)
	{
		enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, 20.0);

		Draw(P_Goal, enemy.X, enemy.Y, 1.0, 0.1 * frame, 1.0);

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
