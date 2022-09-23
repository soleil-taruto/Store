/*
	敵 - FireBall
*/

var<int> EnemyKind_FireBall = @(AUTO);

function <Enemy_t> CreateEnemy_FireBall(<double> x, <double> y, <double> xAdd, <souble> yAdd)
{
	var ret =
	{
		Kind: EnemyKind_FireBall,
		X: x,
		Y: y,
		HP: 1,
		AttackPoint: 1,
		HitDie: true,
		Crash: null,

		// ここから固有

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
	var<int> bouncedCount = 0;

gameLoop:
	for (; ; )
	{
		var<D2Point_t> accel = MakeXYSpeed(enemy.X, enemy.Y, PlayerX, PlayerY, 0.3);

		enemy.XAdd += accel.X;
		enemy.YAdd += accel.Y;

		// 加速度制限
		{
			var<double> ACCEL_MAX = 8.0;
			var<double> accel = GetDistance(enemy.XAdd, enemy.YAdd);

			if (ACCEL_MAX < accel)
			{
				var<double> m = ACCEL_MAX / accel;

				enemy.XAdd *= m;
				enemy.YAdd *= m;
			}
		}

		enemy.X += enemy.XAdd;
		enemy.Y += enemy.YAdd;

		var<double> R = 20.0;
		var<boolean> bounced = false;

		if (IsPtWallForAir_XY(enemy.X - R, enemy.Y))
		{
			enemy.XAdd = Math.abs(enemy.XAdd);
			bounced = true;
		}
		if (IsPtWallForAir_XY(enemy.X + R, enemy.Y))
		{
			enemy.XAdd = Math.abs(enemy.XAdd) * -1;
			bounced = true;
		}
		if (IsPtWallForAir_XY(enemy.X, enemy.Y - R))
		{
			enemy.YAdd = Math.abs(enemy.YAdd);
			bounced = true;
		}
		if (IsPtWallForAir_XY(enemy.X, enemy.Y + R))
		{
			enemy.YAdd = Math.abs(enemy.YAdd) * -1;
			bounced = true;
		}

		if (bounced)
		{
			bouncedCount++;

			if (5 <= bouncedCount) // バウンド回数上限
			{
//				KillEnemy(enemy);
				AddEffect(Effect_Explode_M(enemy.X, enemy.Y));

				break gameLoop;
			}
		}

		enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, R);

		SetColor("#ff4040");
		PrintCircle(enemy.X - Camera.X, enemy.Y - Camera.Y, R);

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
