/*
	クレジット
*/

function* <generatorForTask> CreditMain()
{
	ClearMouseDown();

	for (; ; )
	{
		if (GetMouseDown() == -1)
		{
			break;
		}

		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(50, 80, 80);
		SetFSize(24);

		PrintLine("■素材提供(敬称略)");

		var<string[]> credits = [ @(CRDT) ];

		if (credits.length == 0)
		{
			credits = [ "none", "" ];
		}

		for (var<int> index = 0; index < credits.length; index += 2)
		{
			PrintLine(credits[index] + "　" + credits[index + 1]);
		}

		SetPrint(Screen_W - 400, Screen_H - 20, 0);
		SetFSize(20);
		PrintLine("画面をクリックするとタイトルに戻ります");

		yield 1;
	}
	ClearMouseDown();
}
