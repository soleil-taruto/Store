/*
	“G - E0001
*/

var<int> EnemyKind_E0001 = @(AUTO);

function <Enemy_t> CreateEnemy_E0001(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: EnemyKind_E0001,
		X: x,
		Y: y,
		HP: hp,
		Crash: null,

		// ‚±‚±‚©‚çŒÅ—L

		<double> XAdd: 0.0,
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
		{
			var<double> X_ADD_ADD = 0.1;

			if (Screen_W / 2.0 < enemy.X)
			{
				enemy.XAdd -= X_ADD_ADD;
			}
			else
			{
				enemy.XAdd += X_ADD_ADD;
			}
		}

		enemy.X += enemy.XAdd;
		enemy.Y += 3.0;

		if (Screen_H + 50.0 < enemy.Y)
		{
			break;
		}

		EnemyCommon_Draw(enemy);

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy, <boolean> destroyed)
{
	EnemyCommon_AddScore(100);
	EnemyCommon_Dead(enemy, destroyed);
}
