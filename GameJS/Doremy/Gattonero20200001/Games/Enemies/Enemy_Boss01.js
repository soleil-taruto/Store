/*
	“G - Boss01
*/

var<int> EnemyKind_Boss01 = @(AUTO);

function <Enemy_t> CreateEnemy_Boss01(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_Boss01,
		X: x,
		Y: y,
		HP: 100,
		AttackPoint: 5,
		HitDie: false,
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
	var<D2Point_t> center = CreateD2Point(enemy.X, enemy.Y);

	AddTask(GameTasks, @@_AttackTask(enemy));

	for (; ; )
	{
		var<D2Point_t> pt = AngleToPoint(ProcFrame / 108.0, 100.0);

		enemy.X = center.X + pt.X;
		enemy.Y = center.Y + pt.Y;

		enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, 100.0);

		Draw(P_Enemy_Boss0001, enemy.X - Camera.X, enemy.Y - Camera.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function* <generatorForTask> @@_AttackTask(<Enemy_t> enemy)
{
	var<Func boolean> isDead = () => enemy.HP == -1;

	for (; ; )
	{
		yield* Wait(120);

		if (isDead())
		{
			return;
		}

		@@_Shoot(enemy);
	}
}

function <void> @@_Shoot(<Enemy_t> enemy)
{
	for (var<int> c = 0; c < 5; c++)
	{
		var<D2Point_t> speed = AngleToPoint(GetRand3(Math.PI, Math.PI * 2.0), 7.0);

		GetEnemies().push(CreateEnemy_FireBall(enemy.X, enemy.Y, speed.X, speed.Y));
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy, <boolean> destroyed)
{
//	EnemyCommon_Dead(enemy);
	AddEffect(Effect_Explode_L(enemy.X, enemy.Y));
	SE(S_BossDead);
}
