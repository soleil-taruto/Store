/*
	“G - Tama(“G’e)
*/

var<int> EnemyKind_Tama = @(AUTO);

function <Enemy_t> CreateEnemy_Tama_SnipPlayer(<double> x, <double> y, <double> speed)
{
	var<double> angle = GetAngle(PlayerX - x, PlayerY - y);

	return CreateEnemy_Tama_AngleSpeed(x, y, angle, speed);
}

function <Enemy_t> CreateEnemy_Tama_AngleSpeed(<double> x, <double> y, <double> angle, <double> speed)
{
	var<D2Point_t> pt = AngleToPoint(angle, speed);

	return CreateEnemy_Tama(x, y, pt.X, pt.Y);
}

function <Enemy_t> CreateEnemy_Tama(<double> x, <double> y, <double> xAdd, <souble> yAdd)
{
	var ret =
	{
		Kind: EnemyKind_Tama,
		X: x,
		Y: y,
		HP: 0,
		AttackPoint: 1,
		HitDie: true,
		Crash: null,

		// ‚±‚±‚©‚çŒÅ—L

		<double> XAdd: xAdd,
		<double> YAdd: yAdd,
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
		enemy.X += enemy.XAdd;
		enemy.Y += enemy.YAdd;

		if (IsPtWallForAir_XY(enemy.X, enemy.Y))
		{
			KillEnemy(enemy);
			break;
		}

		enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, 16.0);

		Draw(P_Enemy_Tama0001, enemy.X - Camera.X, enemy.Y - Camera.Y, 1.0, ProcFrame / 20.0, 1.0);

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy, <boolean> destroyed)
{
	EnemyCommon_Dead(enemy, destroyed);
}
