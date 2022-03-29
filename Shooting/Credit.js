/*
	クレジット
*/

function* CreditMain()
{
	ClearMouseDown();

	for (; ; )
	{
		if (GetMouseDown() == -1)
		{
			break;
		}

		SetColor("#a0a080");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(50, 80, 80);
		SetFont("24px 'メイリオ'");

		PrintLine("■素材提供(敬称略)");
		PrintLine("HMIX　http://www.hmix.net/");
		PrintLine("ぴぽや倉庫　https://pipoya.net/sozai/");
		PrintLine("小森平　https://taira-komori.jpn.org/");

		yield 1;
	}
	ClearMouseDown();
}
