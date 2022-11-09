/*
	エンディング
*/

function* <generatorForTask> Ending()
{
	var<int> INP_ST_FRM = 120;

	Play(M_Ending);

	for (var<int> frame = 0; frame < 60; frame++)
	{
		yield 1;
	}

	SetCurtain();
	FreezeInput();

	var<double> a_dest = 1.0;
	var<double> a = 0.0;

	for (var<int> frame = 0; ; frame++)
	{
		if (INP_ST_FRM < frame)
		if (GetMouseDown() == -1 || GetInput_A() == 1 || GetInput_B() == 1)
		{
			break;
		}

		a = Approach(a, a_dest, 0.992);

		DrawEndingBackground();
		Draw(P_EndingString, Screen_W / 2, Screen_H / 2, a, 0.0, 1.0);

		if (INP_ST_FRM < frame)
		{
			SetColor("#ffffff");
			SetPrint(Screen_W - 580, Screen_H - 30, 0);
			SetFSize(20);
			PrintLine("Ｚ・Ｘキーまたは画面をクリックするとタイトルに戻ります");
		}

		yield 1;
	}

	SetCurtain_FD(30, -1.0);

	for (var<Scene_t> scene of CreateScene(40))
	{
		DrawEndingBackground();

		yield 1;
	}

	FreezeInput();
}
