/*
	シナリオ - エンデイング
*/

function* <generatorForTask> Scenario_Ending()
{
	Play(M_Ending);

	yield* Wait(60);

	AddTask(GameTasks, function* <generatorForTask> ()
	{
		var<double> a_dest = 1.0;
		var<double> a = 0.0;

		for (; ; )
		{
			a = Approach(a, a_dest, 0.99);

			Draw(P_EndingString, Screen_W / 2.0, Screen_H / 2.0, a, 0.0, 1.0);

			yield 1;
		}
	}());

	yield* Wait(120);

	for (; ; )
	{
		if (GetInput_A() == 1)
		{
			break;
		}
		yield 1;
	}
}
