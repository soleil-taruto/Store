/*
	ìG - Houdai
*/

var<int> EnemyKind_Houdai = @(AUTO);

function <Enemy_t> CreateEnemy_Houdai(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_Houdai,
		X: x,
		Y: y,
		HP: 10,
		AttackPoint: 2,
		HitDie: false,
		Crash: null,

		// Ç±Ç±Ç©ÇÁå≈óL

		<int> GroundDir: -1, // ê⁄ínñ , Ç«ÇÃï˚å¸Ç…ê⁄ínñ Ç™Ç†ÇÈÇ© (2468-ï˚éÆ)
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	{
		var<I2Point_t> pt = ToTablePoint_XY(enemy.X, enemy.Y);
		var<int> dir;

		if (IsWall_XY(pt.X - 1, pt.Y))
		{
			dir = 4;
		}
		else if (IsWall_XY(pt.X + 1, pt.Y))
		{
			dir = 6;
		}
		else if (IsWall_XY(pt.X, pt.Y - 1))
		{
			dir = 8;
		}
		else if (IsWall_XY(pt.X, pt.Y + 1))
		{
			dir = 2;
		}
		else
		{
			error();
		}

		enemy.GroundDir = dir;
	}

	AddTask(GameTasks, @@_AttackTask(enemy));

	for (; ; )
	{
		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, 50.0, 50.0));

		var<double> rot;

		switch (enemy.GroundDir)
		{
		case 2: rot = (Math.PI / 2.0) * 0.0; break;
		case 4: rot = (Math.PI / 2.0) * 1.0; break;
		case 8: rot = (Math.PI / 2.0) * 2.0; break;
		case 6: rot = (Math.PI / 2.0) * 3.0; break;

		default:
			error(); // never
		}

		Draw(P_Enemy_Houdai, enemy.X - Camera.X, enemy.Y - Camera.Y, 1.0, rot, 1.0);

		yield 1;
	}
}

function* <generatorForTask> @@_AttackTask(<Enemy_t> enemy)
{
	var<Func boolean> isDead = () => enemy.HP == -1;

	for (; ; )
	{
		for (var<int> waitFrm of [ 120, 30, 30 ])
		{
			yield* Wait(waitFrm);

			if (isDead())
			{
				return;
			}

			@@_Shoot(enemy);
		}
	}
}

function <void> @@_Shoot(<Enemy_t> enemy)
{
	var<double> rot;

	switch (enemy.GroundDir)
	{
	case 2: rot = (Math.PI / 2.0) * 3.0; break;
	case 4: rot = (Math.PI / 2.0) * 0.0; break;
	case 8: rot = (Math.PI / 2.0) * 1.0; break;
	case 6: rot = (Math.PI / 2.0) * 2.0; break;

	default:
		error(); // never
	}

	var<double> SPEED = 7.0;

	GetEnemies().push(CreateEnemy_Tama_AngleSpeed(enemy.X, enemy.Y, rot - Math.PI / 4.0 , SPEED));
	GetEnemies().push(CreateEnemy_Tama_AngleSpeed(enemy.X, enemy.Y, rot                 , SPEED));
	GetEnemies().push(CreateEnemy_Tama_AngleSpeed(enemy.X, enemy.Y, rot + Math.PI / 4.0 , SPEED));
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy, <boolean> destroyed)
{
	EnemyCommon_Dead(enemy, destroyed);
}
