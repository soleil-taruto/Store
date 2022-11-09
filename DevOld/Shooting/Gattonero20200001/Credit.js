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

	var<string[]> lines = [];

	lines.push("■素材 (文字コード順・敬称略)");
	lines.push("");

	for (var<int> index = 0; index < credits.length; index += 2)
	{
		lines.push(credits[index] + "　" + credits[index + 1]);
	}

	lines.push("");
	lines.push("Ｚ・Ｘキーまたは画面をクリックするとタイトルに戻ります");

	var<int> MARGIN_L = 30;
	var<int> MARGIN_B = 30;

	var<int> yStep = ToFix((Screen_H - MARGIN_B) / lines.length);

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
		SetPrint(MARGIN_L, yStep, yStep);
		SetFSize(20);

		for (var<string> line of lines)
		{
			PrintLine(line);
		}

		yield 1;
	}
	FreezeInput();
}
