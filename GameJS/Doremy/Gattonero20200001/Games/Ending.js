/*
	エンディング
*/

function* <generatorForTask> Ending()
{
	var<int> INP_ST_FRM = 60

	SetCurtain();
	FreezeInput();

	Play(M_Ending);

	for (var<int> frame = 0; ; frame++)
	{
		if (INP_ST_FRM < frame && GetInput_A() == 1)
		{
			break;
		}

		SetColor("#404080");
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		SetColor("#ffffff");
		SetPrint(10, 350, 200);
		SetFSize(130);
		PrintLine("エンディング");

		if (INP_ST_FRM < frame)
		{
			SetPrint(20, 450, 200);
			SetFSize(24);
			PrintLine("Ａボタンを押して下さい。");
		}

		yield 1;
	}

	SetCurtain_FD(30, -1.0);

	for (var<Scene_t> scene of CreateScene(40))
	{
		SetColor("#404080");
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		yield 1;
	}

	FreezeInput();
}
