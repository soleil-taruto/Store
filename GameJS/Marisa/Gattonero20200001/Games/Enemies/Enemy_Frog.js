/*
	“G - Frog
*/

var<int> EnemyKind_Frog = @(AUTO);

function <Enemy_t> CreateEnemy_Frog(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_Frog,
		X: x,
		Y: y,
		HP: 5,
		AttackPoint: 1,
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
	var<double> ATARI_X = 25.0;
	var<double> ATARI_Y = 25.0;

	for (; ; )
	{
		enemy.X += GetRand2() * 10.0 * Math.cos(ProcFrame / 500.0);
		enemy.Y += GetRand2() * 10.0 * Math.sin(ProcFrame / 500.0);

		if (IsPtWall_XY(enemy.X - ATARI_X, enemy.Y))
		{
			enemy.X = ToTileCenterX(enemy.X - ATARI_X) + TILE_W / 2.0 + ATARI_X;
		}
		if (IsPtWall_XY(enemy.X + ATARI_X, enemy.Y))
		{
			enemy.X = ToTileCenterX(enemy.X + ATARI_X) - TILE_W / 2.0 - ATARI_X;
		}
		if (IsPtWall_XY(enemy.X, enemy.Y - ATARI_Y))
		{
			enemy.Y = ToTileCenterY(enemy.Y - ATARI_Y) + TILE_H / 2.0 + ATARI_Y;
		}
		if (IsPtWall_XY(enemy.X, enemy.Y + ATARI_Y))
		{
			enemy.Y = ToTileCenterY(enemy.Y + ATARI_Y) - TILE_H / 2.0 - ATARI_Y;
		}

		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, 50.0, 50.0));

		Draw(P_Enemy_Frog, enemy.X - Camera.X, enemy.Y - Camera.Y, 1.0, 0.0, 1.0);

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
