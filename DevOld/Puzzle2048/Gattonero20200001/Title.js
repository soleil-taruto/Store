/*
	タイトル画面
*/

var<int> @@_BUTTON_L = 400;
var<int> @@_BUTTON_T = 800;
var<int> @@_BUTTON_W = 200;
var<int> @@_BUTTON_H = 55;
var<int> @@_BUTTON_Y_STEP = 70;

var<I3Color_t> @@_BUTTON_BACK_COLOR = CreateI3Color(255, 255, 128);
var<I3Color_t> @@_BUTTON_TEXT_COLOR = CreateI3Color(0, 0, 0);

var<int> @@_BUTTON_TEXT_L = 35;
var<int> @@_BUTTON_TEXT_T = 40;
var<int> @@_BUTTON_TEXT_FONT_SIZE = 32;

var @@_Buttons =
[
	{
		L : @@_BUTTON_L,
		T : @@_BUTTON_T + @@_BUTTON_Y_STEP * 0,
		W : @@_BUTTON_W,
		H : @@_BUTTON_H,
		Draw : function()
		{
			SetColor("#ffffff");
			PrintRect(this.L, this.T, this.W, this.H);
			SetColor(I3ColorToString(@@_BUTTON_TEXT_COLOR));
			SetPrint(this.L + @@_BUTTON_TEXT_L, this.T + @@_BUTTON_TEXT_T, 0);
			SetFSize(@@_BUTTON_TEXT_FONT_SIZE);
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
		L : @@_BUTTON_L,
		T : @@_BUTTON_T + @@_BUTTON_Y_STEP * 1,
		W : @@_BUTTON_W,
		H : @@_BUTTON_H,
		Draw : function()
		{
			SetColor(I3ColorToString(@@_BUTTON_BACK_COLOR));
			PrintRect(this.L, this.T, this.W, this.H);
			SetColor(I3ColorToString(@@_BUTTON_TEXT_COLOR));
			SetPrint(this.L + @@_BUTTON_TEXT_L, this.T + @@_BUTTON_TEXT_T, 0);
			SetFSize(@@_BUTTON_TEXT_FONT_SIZE);
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
		L : @@_BUTTON_L,
		T : @@_BUTTON_T + @@_BUTTON_Y_STEP * 2,
		W : @@_BUTTON_W,
		H : @@_BUTTON_H,
		Draw : function()
		{
			SetColor(I3ColorToString(@@_BUTTON_BACK_COLOR));
			PrintRect(this.L, this.T, this.W, this.H);
			SetColor(I3ColorToString(@@_BUTTON_TEXT_COLOR));
			SetPrint(this.L + @@_BUTTON_TEXT_L, this.T + @@_BUTTON_TEXT_T, 0);
			SetFSize(@@_BUTTON_TEXT_FONT_SIZE);
			PrintLine("Exit");
		},
		Pressed : function* ()
		{
			LOGPOS();
			window.location.href = "/";
//			window.location.href = "..";
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
		SetPrint(30, 600, 0);
		SetFSize(380);
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
