/*
	シナリオ
*/

function* <generatorForTask> ScenarioTask()
{
	// ★サンプル -- 要削除
	{
		for (; ; )
		{
			if (GetRand1() < 0.2)
			{
				GetEnemies().push(CreateEnemy_BDummy(GetRand3(FIELD_L, FIELD_R), FIELD_T, 10));
			}

			yield* Repeat(1, 20); // 20フレーム待つ。-- ウェイトはこの様に記述する。
		}
	}
}
