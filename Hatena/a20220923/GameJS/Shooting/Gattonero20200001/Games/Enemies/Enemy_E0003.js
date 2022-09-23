/*
	“G - E0003
*/

var<int> EnemyKind_E0003 = @(AUTO);

function <Enemy_t> CreateEnemy_E0003(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: EnemyKind_E0003,
		X: x,
		Y: y,
		HP: hp,
		Crash: null,

		// ‚±‚±‚©‚çŒÅ—L
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	while (enemy.Y < Screen_H)
	{
		for (var<int> c = 0; c < 20; c++)
		{
			enemy.Y += 3.0;

			yield @@_DrawYield(enemy);
		}
		for (var<int> c = 0; c < 20; c++)
		{
			enemy.X += 5.0;

			yield @@_DrawYield(enemy);
		}
		for (var<int> c = 0; c < 20; c++)
		{
			enemy.Y += 3.0;

			yield @@_DrawYield(enemy);
		}
		for (var<int> c = 0; c < 20; c++)
		{
			enemy.X -= 5.0;

			yield @@_DrawYield(enemy);
		}
	}
}

function <int> @@_DrawYield(<Enemy_t> enemy)
{
	EnemyCommon_Draw(enemy);
	return 1;
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy, <boolean> destroyed)
{
	EnemyCommon_AddScore(300);
	EnemyCommon_Dead(enemy, destroyed);
}
