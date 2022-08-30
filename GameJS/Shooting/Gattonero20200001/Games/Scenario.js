/*
	シナリオ
*/

function* <generatorForTask> CreateScenarioTask()
{
	if (DEBUG)
	{
		// -- choose one --

//		yield* @@_Test01();
//		yield* @@_Test02();
//		yield* @@_Test03();
//		yield* @@_Test04();
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
			GetEnemies().push(CreateEnemy_BDummy(GetRand3(0.0, Screen_W), 0.0, 10));
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
				GetEnemies().push(CreateEnemy_E0001(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 1:
				GetEnemies().push(CreateEnemy_E0002(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 2:
				GetEnemies().push(CreateEnemy_E0003(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 3:
				GetEnemies().push(CreateEnemy_E0004(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 4:
				GetEnemies().push(CreateEnemy_E0005(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 5:
				GetEnemies().push(CreateEnemy_E0006(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 6:
				GetEnemies().push(CreateEnemy_E0007(GetRand3(0.0, Screen_W), 0.0, 10));
				break;

			case 7:
				GetEnemies().push(CreateEnemy_E0008(GetRand3(0.0, Screen_W), 0.0, 10));
				break;
			}
		}

		yield 1;
	}
}

function* <generatorForTask> @@_Test02()
{
	var<Sound_t[]> musics =
	[
		M_Stage01,
		M_Stage01Boss,
		M_Stage02,
		M_Stage02Boss,
		M_Stage03,
		M_Stage03Boss,
		M_Ending,
	];

	for (; ; )
	{
		Play(musics[BackgroundPhase]);

		yield* Wait(60 * 30);

		BackgroundPhase++;
		BackgroundPhase %= 7;
	}
}

function* <generatorForTask> @@_Test03()
{
	BackgroundPhase = 1;

	Play(M_Stage01Boss);

	GetEnemies().push(CreateEnemy_Boss01(Screen_W / 2.0, -100.0, 300));

	for (; ; )
	{
		yield 1;
	}
}

function* <generatorForTask> @@_Test04()
{
	BackgroundPhase = 6;
	yield* Scenario_Ending();
}

function* <generatorForTask> @@_Main()
{
	BackgroundPhase = 0;
	yield* Scenario_Stage01();
	@@_PostStage();

	BackgroundPhase++;
	yield* Scenario_Stage01Boss();
	@@_PostStage();

	BackgroundPhase++;
	yield* Scenario_Stage02();
	@@_PostStage();

	BackgroundPhase++;
	yield* Scenario_Stage02Boss();
	@@_PostStage();

	BackgroundPhase++;
	yield* Scenario_Stage03();
	@@_PostStage();

	BackgroundPhase++;
	yield* Scenario_Stage03Boss();
	@@_PostStage();

	BackgroundPhase++;
	yield* Scenario_Ending();
}

function <void> @@_PostStage()
{
	SaveLocalStorage(); // ハイスコア更新
}
