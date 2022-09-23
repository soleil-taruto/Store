/*
	ƒQ[ƒ€EƒƒCƒ“
*/

function* <generatorForTask> GameMain()
{
	FreezeInput();
	ClearAllActor();

	for (; ; )
	{
		if (GetInput_A() == 1)
		{
			break;
		}

		SetColor("#000000");
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		ExecuteAllActor();

		yield 1;
	}
	FreezeInput();
}
