/*
	敵 - E0007
*/

var<int> EnemyKind_E0007 = @(AUTO);

function <Enemy_t> CreateEnemy_E0007(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: EnemyKind_E0007,
		X: x,
		Y: y,
		HP: hp,
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
	var<double> angle = GetAngle(PlayerX - enemy.X, PlayerY - enemy.Y);
	var<double> speed = 10.0;

	for (; ; )
	{
		{
			var<double> ANGLE_ADD = 0.02;
			var<double> a = GetAngle(PlayerX - enemy.X, PlayerY - enemy.Y);

			while (a < angle) a += Math.PI * 2;
			while (a > angle) a -= Math.PI * 2;

			if (a + Math.PI < angle)
			{
				angle += ANGLE_ADD;
			}
			else
			{
				angle -= ANGLE_ADD;
			}
		}

		speed -= 0.07;

		var<D2Point_t> speedXY = AngleToPoint(angle, Math.abs(speed));

		enemy.X += speedXY.X;
		enemy.Y += speedXY.Y;

		// ? 画面外に出た -> 終了(死亡させる)
		if (IsOutOfScreen(CreateD2Point(enemy.X, enemy.Y), 50.0))
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
	EnemyCommon_AddScore(700);
	EnemyCommon_Dead(enemy, destroyed);
}
