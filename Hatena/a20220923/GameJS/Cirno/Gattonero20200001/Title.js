/*
	タイトル画面
*/

var @@_Buttons =
[
	{
		Text: "スタート",
		Pressed : function* ()
		{
			LOGPOS();
			yield* GameProgressMaster();
			LOGPOS();
		},
	},
	{
		Text: "設定",
		Pressed : function* ()
		{
			LOGPOS();
			yield* SettingMain();
			LOGPOS();
		},
	},
	{
		Text: "Credit",
		Pressed : function* ()
		{
			LOGPOS();
			yield* CreditMain();
			LOGPOS();
		},
	},
	{
		Text: "Exit",
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
	var<int> selectIndex = 0;

	SetCurtain();
	FreezeInput();
	FreezeInputUntilRelease();

	Play(M_Title);

	for (; ; )
	{
		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(50, 150, 0);
		SetFSize(100);
		PrintLine("Cirno");

		selectIndex = DrawSimpleMenu(selectIndex, 70, 280, 600, 30, @@_Buttons.map(v => v.Text));

		if (DSM_Desided)
		{
			FreezeInput();

			yield* @@_Buttons[selectIndex].Pressed();

			SetCurtain();
			FreezeInput();

			Play(M_Title);
		}
		yield 1;
	}
}
