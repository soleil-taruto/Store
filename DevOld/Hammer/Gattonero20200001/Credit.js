/*
	クレジット
*/

function* <generatorForTask> CreditMain()
{
	var<string[]> credits = [ @(CRDT) ];

	if (credits.length == 0)
	{
		credits = [ "none", "" ];
	}

	FreezeInput();

	for (; ; )
	{
		if (GetMouseDown() == -1 || GetInput_A() == 1 || GetInput_B() == 1)
		{
			break;
		}

		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(30, 50, 24);
		SetFSize(20);
		PrintLine("■素材 (文字コード順・敬称略)");
		PrintLine("");

		for (var<int> index = 0; index < credits.length; index += 2)
		{
			PrintLine(credits[index] + "　" + credits[index + 1]);
		}

		SetPrint(Screen_W - 540, Screen_H - 20, 0);
		SetFSize(20);
		PrintLine("Ａボタンまたは画面をクリックするとタイトルに戻ります");

		yield 1;
	}
	FreezeInput();
}
