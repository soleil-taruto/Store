/*
	シナリオ - ステージ 01 ボス
*/

function* <generatorForTask> Scenario_Stage01Boss()
{
	for (var<Enemy_t> enemy of GetEnemies())
	{
		KillEnemy(enemy);
	}

	Play(M_Stage01Boss);

	yield* Wait(30);

	GetEnemies().push(CreateEnemy_Boss01(Screen_W / 2.0, -100.0, 300));

	for (; ; ) // ボスが死ぬまで待つ。
	{
		if (!GetEnemies().some(v => IsEnemyBoss(v)))
		{
			break;
		}
		yield 1;
	}

	yield* Wait(90);
}
