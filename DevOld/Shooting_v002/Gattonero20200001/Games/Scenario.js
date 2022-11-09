/*
	シナリオ
*/

function* <generatorForTask> ScenarioTask()
{
	if (DEBUG)
	{
		// -- choose one --

//		yield* @@_Test01();
		yield* @@_Main();

		// --
	}
	else
	{
		yield* @@_Main();
	}
}

function* <generatorForTask> @@_Test01()
{
	/*
	for (; ; )
	{
		if (GetRand1() < 0.2)
		{
			GetEnemies().push(CreateEnemy_BDummy(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
		}

		yield* Repeat(1, 20); // 20フレーム待つ。-- ウェイトはこの様に記述する。
	}
	*/

	for (; ; )
	{
		if (GetRand1() < 0.02)
//		if (GetRand1() < 0.09) // 無理ゲー
		{
			switch(GetRand(8))
			{
			case 0:
				GetEnemies().push(CreateEnemy_E0001(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 1:
				GetEnemies().push(CreateEnemy_E0002(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 2:
				GetEnemies().push(CreateEnemy_E0003(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 3:
				GetEnemies().push(CreateEnemy_E0004(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 4:
				GetEnemies().push(CreateEnemy_E0005(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 5:
				GetEnemies().push(CreateEnemy_E0006(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 6:
				GetEnemies().push(CreateEnemy_E0007(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 7:
				GetEnemies().push(CreateEnemy_E0008(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;
			}
		}

		yield 1;
	}
}

function* <generatorForTask> @@_Main()
{
	yield* Wait(30);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0001(600, FIELD_T - 25, 5));

		yield* Wait(20);
	}

	yield* Wait(30);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0002(GetRand3(50, 650), FIELD_T - 25, 5));

		yield* Wait(20);
	}

	yield* Wait(90);

	GetEnemies().push(EnemyCommon_ToItemer(
		CreateEnemy_E0003(FIELD_L + FIELD_W / 2, FIELD_T - 25, 25),
		EnemyItemType_e_POWER_UP
		));

	yield* Wait(180);

	GetEnemies().push(EnemyCommon_ToItemer(
		CreateEnemy_E0005(FIELD_L + FIELD_W / 3, FIELD_T - 25, 5),
		EnemyItemType_e_ZANKI_UP
		));

	yield* Wait(120);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0001(100, FIELD_T - 25, 5));

		yield* Wait(20);
	}

	yield* Wait(30);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0002(GetRand3(50, 650), FIELD_T - 25, 5));

		yield* Wait(20);
	}

	yield* Wait(90);

	GetEnemies().push(EnemyCommon_ToItemer(
		CreateEnemy_E0004(FIELD_L + (FIELD_W / 3) * 1, FIELD_T - 25, 10),
		EnemyItemType_e_POWER_UP
		));

	yield* Wait(90);

	GetEnemies().push(EnemyCommon_ToItemer(
		CreateEnemy_E0004(FIELD_L + (FIELD_W / 3) * 2, FIELD_T - 25, 10),
		EnemyItemType_e_ZANKI_UP
		));

	yield* Wait(120);

	for (var<int> c = 0; c < 20; c++)
	{
		GetEnemies().push(CreateEnemy_E0005(GetRand3(50, 650), FIELD_T - 25, 5));

		yield* Wait(20);
	}

	yield* Wait(60);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(EnemyCommon_ToShooter(
			CreateEnemy_E0001(600, FIELD_T - 25, 5)
			));

		yield* Wait(20);
	}

	yield* Wait(30);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(EnemyCommon_ToShooter(
			CreateEnemy_E0002(GetRand3(50, 650), FIELD_T - 25, 5)
			));

		yield* Wait(20);
	}

	yield* Wait(90);

	for (var<int> c = 0; c < 10; c++)
	{
		GetEnemies().push(CreateEnemy_E0006(GetRand3(FIELD_L, FIELD_R), FIELD_T - 25, 5));

		yield* Wait(30);
	}

	yield* Wait(90);

	for (var<int> c = 0; c < 20; c++)
	{
		GetEnemies().push(CreateEnemy_E0005(GetRand3(300, 400), FIELD_T - 25, 5));

		yield* Wait(20);
	}

	yield* Wait(60);

	for (var<int> c = 0; c < 20; c++)
	{
		GetEnemies().push(EnemyCommon_ToShooter(
			CreateEnemy_E0005(GetRand3(100, 600), FIELD_T - 25, 5)
			));

		yield* Wait(20);
	}

	yield* Wait(60);

	// ★★★ウェーブの追加はここへ

	// 以下、ランダムを永久に

	for (var<int> frame = 0; ; frame++)
	{
		if (GetRand1() < Math.min(0.02 + frame / 360000.0, 0.09)) // 1分で0.01上がる, 0.02～0.09
		{
			switch(GetRand(8))
			{
			case 0:
				GetEnemies().push(CreateEnemy_E0001(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 1:
				GetEnemies().push(CreateEnemy_E0002(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 2:
				GetEnemies().push(CreateEnemy_E0003(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 3:
				GetEnemies().push(CreateEnemy_E0004(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 4:
				GetEnemies().push(CreateEnemy_E0005(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 5:
				GetEnemies().push(CreateEnemy_E0006(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 6:
				GetEnemies().push(CreateEnemy_E0007(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;

			case 7:
				GetEnemies().push(CreateEnemy_E0008(GetRand3(FIELD_L, FIELD_R), 0.0, 10));
				break;
			}

			if (900 <= frame)
			{
				var<Enemy_t> enemy = GetEnemies()[GetEnemies().length - 1];

				if (GetRand1() < 0.3)
				{
					EnemyCommon_ToShooter(enemy);
				}
				if (GetRand1() < 0.01)
				{
					var<Enemy_Item_Kind_e> itemKind;

					switch (GetRand(2))
					{
					case 0: itemKind = EnemyItemType_e_POWER_UP; break;
					case 1: itemKind = EnemyItemType_e_ZANKI_UP; break;

					default:
						error(); // never
					}

					EnemyCommon_ToItemer(enemy, itemKind);
				}
			}
		}

		yield 1;
	}
}
