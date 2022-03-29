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
		PrintLine("安野譲 (消失点) http://www.aj.undo.jp/material/bg/bg_material.html");

		yield 1;
	}
	ClearMouseDown();
}
