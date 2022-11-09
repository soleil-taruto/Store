/*
	設定
*/

function* <generatorForTask> SettingMain()
{
	var<int> selectIndex = 0;

	FreezeInput();

gameLoop:
	for (; ; )
	{
		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(30, 50, 50);
		SetFSize(20);
		PrintLine("■設定");

		selectIndex = DrawSimpleMenu(
			selectIndex,
			30,
			100,
			70,
			[
				"ゲームパッドのＡボタンの割り当て",
				"ゲームパッドのＢボタンの割り当て",
				"戻る",
			]);

		if (DSM_Desided)
		switch (selectIndex)
		{
		case 0:
			yield* @@_PadSetting("Ａ", index => PadInputIndex_A = index);
			break;

		case 1:
			yield* @@_PadSetting("Ｂ", index => PadInputIndex_B = index);
			break;

		case 2:
			break gameLoop;
		}
		yield 1;
	}
	FreezeInput();
}

function* <generatorForTask> @@_PadSetting(<string> name, <Action int> a_setBtn)
{
	yield* @@_WaitToReleaseButton();
	FreezeInput();

	for (; ; )
	{
		if (GetMouseDown() == -1 || GetKeyInput(32) == 1)
		{
			break;
		}

		var<int> index = @@_GetPressButtonIndex();

		if (index != -1)
		{
			a_setBtn(index);
			break;
		}

		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(30, 50, 50);
		SetFSize(20);
		PrintLine("ゲームパッドの" + name + "ボタン設定");
		PrintLine("割り当てるボタンを押して下さい。");
		PrintLine("キャンセルするにはスペースキーまたは画面をクリックして下さい。");

		yield 1;
	}
	yield* @@_WaitToReleaseButton();
	FreezeInput();
}

function* <generatorForTask> @@_WaitToReleaseButton()
{
	while (@@_GetPressButtonIndex() != -1)
	{
		yield 1;
	}
}

function <int> @@_GetPressButtonIndex()
{
	for (var<int> index = 0; index < 100; index++)
	{
		if (1 <= GetPadInput(index))
		{
			return index;
		}
	}
	return -1;
}
