/*
	敵 - AutoStageClear
*/

var<int> EnemyKind_AutoStageClear = @(AUTO);

function <Enemy_t> CreateEnemy_AutoStageClear(<double> x, <double> y)
{
	var ret =
	{
		Kind: EnemyKind_AutoStageClear,
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
	AddTask(GameTasks, @@_BackgroundTask());

	for (; ; )
	{
		enemy.Crash = null; // 当たり判定無し。

		// 描画無し。

		yield 1;
	}
}

function* <generatorForTask> @@_BackgroundTask()
{
	yield* Wait(30); // ステージ開始直後から判定始めると何かあるかもしれないので、少し待ってから開始する。

	while (GetEnemies().some(v => IsEnemyBoss(v)))
	{
		yield 1;
	}

	yield* Wait(30); // 余韻

	Fadeout_F(100);

	yield* Wait(150); // 余韻

	GameRequestStageClear = true;
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	// noop
}

function <void> @@_Dead(<Enemy_t> enemy, <Shot_t> shot)
{
	// noop
}
