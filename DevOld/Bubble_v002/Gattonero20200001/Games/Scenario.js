/*
	ƒVƒiƒŠƒI
*/

function* <generatorForTask> ScenarioTask()
{
	for (var<int> c = 0; ; c++)
	{
//		if (1 <= c && c % 10 == 0)
		if (1 <= c && c % 20 == 0)
//		if (1 <= c && c % 30 == 0)
		{
			EnemyColorLmt = Math.min(EnemyColorLmt + 1, P_Balls.length);
		}

		LOGPOS();

		{
			var<int> step = c + 1;
			var<int> y = -1;

			for (var<int> x = 0; ; x++)
			{
				var<double> px = 17 + 15 * (step % 2) + 30 * x;
				var<double> py = 15 + 25 * y;

				if (FIELD_R - 15 < px)
				{
					break;
				}
				GetEnemies().push(CreateEnemy_Ball(px, py, 1, GetRand(EnemyColorLmt)));
			}
		}

		LOGPOS();

		for (var<int> w = 0; w < 250; w++)
		{
			yield 1;
		}
	}
}
