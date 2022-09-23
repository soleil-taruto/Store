/*
	ゲーム・メイン
*/

function* <generatorForTask> GameMain()
{
	for (; ; )
	{
		if (GetInput_A() == 1)
		{
			break;
		}

		SetColor("#000000");
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		SetColor("#00ff00");
		SetPrint(20, 40, 0);
		SetFSize(20);
		PrintLine("Ａボタンを押すと終了...");

		yield 1;
	}
}
