/*
	敵 - E0008
*/

var<int> EnemyKind_E0008 = @(AUTO);

function <Enemy_t> CreateEnemy_E0008(<double> x, <double> y, <int> hp)
{
	var ret =
	{
		Kind: EnemyKind_E0008,
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
	var<int> boundCount = 0;

	for (; ; )
	{
		{
			var<double> ANGLE_ADD = 0.003;
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

		var<double> SPEED = 9.0;

		var<D2Point_t> speedXY = AngleToPoint(angle, SPEED);

		enemy.X += speedXY.X;
		enemy.Y += speedXY.Y;

		if (boundCount < 7)
		{
			var<boolean> boundFlag = false;

			if (enemy.X < FIELD_L)
			{
				speedXY.X = Math.abs(speedXY.X);
				boundFlag = true;
			}
			if (FIELD_R < enemy.X)
			{
				speedXY.X = Math.abs(speedXY.X) * -1;
				boundFlag = true;
			}
			if (enemy.Y < FIELD_T)
			{
				speedXY.Y = Math.abs(speedXY.Y);
				boundFlag = true;
			}
			if (FIELD_B < enemy.Y)
			{
				speedXY.Y = Math.abs(speedXY.Y) * -1;
				boundFlag = true;
			}

			if (boundFlag)
			{
				enemy.X = ToRange(enemy.X, FIELD_L, FIELD_R);
				enemy.Y = ToRange(enemy.Y, FIELD_T, FIELD_B);

				angle = GetAngle(speedXY.X, speedXY.Y);
				boundCount++;
			}
		}


		// ? 画面外に出た -> 終了(死亡させる)
		if (IsOut(
			CreateD2Point(enemy.X, enemy.Y),
			CreateD4Rect(FIELD_L, FIELD_T, FIELD_W, FIELD_H),
			50.0
			))
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

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_AddScore(800);
	EnemyCommon_Dead(enemy);
}
