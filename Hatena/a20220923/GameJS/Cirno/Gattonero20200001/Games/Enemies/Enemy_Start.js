/*
	敵 - スタート地点
*/

var<int> EnemyKind_Start = @(AUTO);

function <Enemy_t> CreateEnemy_Start(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_Start,
		X: x,
		Y: y,
		HP: 0,
		AttackPoint: 0,
		HitDie: false,
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
	for (; ; )
	{
		enemy.Crash = null; // 当たり判定無し

		// 描画無し

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	// none
}

function <void> @@_Dead(<Enemy_t> enemy, <Shot_t> shot)
{
	// none
}
