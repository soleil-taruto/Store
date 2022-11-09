/*
	シナリオ - エンデイング
*/

function* <generatorForTask> Scenario_Ending()
{
	Play(M_Ending);

	yield* Wait(180);

	AddEffect(function* <generatorForTask> ()
	{
		for (; ; )
		{
			SetColor("#00000080");
			PrintRect(0, 200, Screen_W, 200);
			SetColor("#ffffff");
			SetPrint(90, 340, 0);
			SetFSize(110);
			PrintLine("E N D I N G");

			yield 1;
		}
	}());

	for (; ; )
	{
		if (GetInput_A() == 1)
		{
			break;
		}
		yield 1;
	}
}
