/*
	“G - E0002
*/

var<int> EnemyKind_E0002 = @(AUTO);

function <Enemy_t> CreateEnemy_E0002(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: EnemyKind_E0002,
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
	while (enemy.Y < Screen_H && enemy.Y < PlayerY)
	{
		enemy.Y += 4.0;

		yield @@_DrawYield(enemy);
	}
	while (0.0 < enemy.Y)
	{
		{
			var<double> X_ADD = 4.0;

			if (Math.abs(enemy.X - PlayerX) < X_ADD)
			{
				break;
			}

			if (enemy.X < PlayerX)
			{
				enemy.X += X_ADD;
			}
			else
			{
				enemy.X -= X_ADD;
			}
		}

		enemy.Y -= 8.0;

		yield @@_DrawYield(enemy);
	}
	while (enemy.Y < Screen_H)
	{
		enemy.Y += 4.0;

		yield @@_DrawYield(enemy);
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
	EnemyCommon_AddScore(200);
	EnemyCommon_Dead(enemy, destroyed);
}
