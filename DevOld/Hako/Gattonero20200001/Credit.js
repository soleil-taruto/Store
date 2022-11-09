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

	var<double> w_dest = ToInt(Screen_W * 0.7);
	var<double> w = 0.0;

	for (; ; )
	{
		if (GetMouseDown() == -1 || GetInput_A() == 1 || GetInput_B() == 1)
		{
			break;
		}

		w = Approach(w, w_dest, 0.93);

		DrawTitleBackground();

		SetColor("#00000080");
		PrintRect(0, 0, w, Screen_H);

		SetColor("#ffffff");
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
