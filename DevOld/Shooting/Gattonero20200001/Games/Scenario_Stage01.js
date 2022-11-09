/*
	シナリオ - ステージ 01
*/

function* <generatorForTask> Scenario_Stage01()
{
	Play(M_Stage01);

	yield* Wait(60);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0001(600, -25, 5));

		yield* Wait(20);
	}

	yield* Wait(60);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0002(GetRand3(50, 750), -25, 5));

		yield* Wait(20);
	}

	yield* Wait(60);

	GetEnemies().push(EnemyCommon_ToItemer(
		CreateEnemy_E0006(GetRand3(150, 650), -25, 5),
		EnemyItemType_e_POWER_UP
		));

	yield* Wait(60);

	GetEnemies().push(EnemyCommon_ToItemer(
		CreateEnemy_E0007(GetRand3(150, 650), -25, 5),
		EnemyItemType_e_POWER_UP
		));

	yield* Wait(60);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(EnemyCommon_ToShooter(
			CreateEnemy_E0003(GetRand3(150, 650), -25, 5)
			));

		yield* Wait(20);
	}

	yield* Wait(180);
}
