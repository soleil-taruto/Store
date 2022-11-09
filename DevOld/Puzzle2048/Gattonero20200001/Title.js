/*
	タイトル画面
*/

var @@_Buttons =
[
	{
		L : 300,
		T : 500,
		W : 200,
		H : 55,
		Draw : function()
		{
			SetColor("#ffffff");
			PrintRect(this.L, this.T, this.W, this.H);
			SetColor("#000000");
			SetPrint(this.L + 35, this.T + 40, 0);
			SetFSize(32);
			PrintLine("スタート");
		},
		Pressed : function* ()
		{
			LOGPOS();
			yield* GameMain();
			LOGPOS();
		},
	},
	{
		L : 300,
		T : 570,
		W : 200,
		H : 55,
		Draw : function()
		{
			SetColor("#ffff80");
			PrintRect(this.L, this.T, this.W, this.H);
			SetColor("#000080");
			SetPrint(this.L + 35, this.T + 40, 0);
			SetFSize(32);
			PrintLine("Credit");
		},
		Pressed : function* ()
		{
			LOGPOS();
			yield* CreditMain();
			LOGPOS();
		},
	},
	{
		L : 300,
		T : 640,
		W : 200,
		H : 55,
		Draw : function()
		{
			SetColor("#ffff80");
			PrintRect(this.L, this.T, this.W, this.H);
			SetColor("#000080");
			SetPrint(this.L + 35, this.T + 40, 0);
			SetFSize(32);
			PrintLine("Exit");
		},
		Pressed : function* ()
		{
			LOGPOS();
			window.location.href = "..";
//			window.location.href = "https://www.google.com/";
			LOGPOS();
		},
	},
];

function* <generatorForTask> TitleMain()
{
	for (; ; )
	{
		SetColor("#80a080");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(30, 400, 0);
		SetFSize(300);
		PrintLine("2048");

		for (var button of @@_Buttons)
		{
			button.Draw();

			if (GetMouseDown() == -1)
			{
				if (
					button.L < GetMouseX() && GetMouseX() < button.L + button.W &&
					button.T < GetMouseY() && GetMouseY() < button.T + button.H
					)
				{
					ClearMouseDown();
					yield* button.Pressed();
					ClearMouseDown();
					break;
				}
			}
		}
		yield 1;
	}
}
