/*
	入り口
*/

function* <generatorForTask> EntranceMain()
{
	var<int> button_w = GetPicture_W(P_GameStartButton);
	var<int> button_h = GetPicture_H(P_GameStartButton);
	var<int> button_l = ToFix((Screen_W - button_w) / 2.0);
	var<int> button_t = ToFix((Screen_H - button_h) / 2.0);

	SetCurtain();
	FreezeInput();

	for (; ; )
	{
		// マウスクリック
		if (
			GetMouseDown() == -1 &&
			button_l < GetMouseX() && GetMouseX() < button_l + button_w &&
			button_t < GetMouseY() && GetMouseY() < button_t + button_h
			)
		{
			break;
		}

		// キー・ボタン押下
		if (
			GetInput_2() == 1 ||
			GetInput_4() == 1 ||
			GetInput_6() == 1 ||
			GetInput_8() == 1 ||
			GetInput_A() == 1 ||
			GetInput_B() == 1 ||
			GetInput_Pause() == 1
			)
		{
			break;
		}

		SetColor("#000000");
		PrintRect(0.0, 0.0, Screen_W, Screen_H);
		SetColor("#ffffff");
		Draw(P_GameStartButton, Screen_W / 2.0, Screen_H / 2.0, 1.0, 0.0, 1.0);

		yield 1;
	}

	FreezeInput();

	yield* TitleMain();
}
