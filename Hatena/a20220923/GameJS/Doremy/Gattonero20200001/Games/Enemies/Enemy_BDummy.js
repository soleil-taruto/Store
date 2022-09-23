/*
	“G - BDummy šƒTƒ“ƒvƒ‹
*/

var<int> EnemyKind_BDummy = @(AUTO);

function <Enemy_t> CreateEnemy_BDummy(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_BDummy,
		X: x,
		Y: y,
		HP: 1,
		AttackPoint: 1,
		HitDie: false,
		Crash: null,

		// ‚±‚±‚©‚çŒÅ—L

		<double> Dummy_01: 1.0,
		<double> Dummy_02: 2.0,
		<double> Dummy_03: 3.0,
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
		enemy.Y += 2.0;

		if (Map.H * TILE_H < enemy.Y)
		{
			break;
		}

//		enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, 25.0);
		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, 50.0, 50.0));

		Draw(P_Dummy, enemy.X - Camera.X, enemy.Y - Camera.Y, 1.0, 0.0, 1.0);

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
