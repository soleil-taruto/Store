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
		DrawTitleBackground();

		var<double> bure_x = Math.sin(ProcFrame / 100.0) * 20.0;
		var<double> bure_y = Math.sin(ProcFrame / 108.0) * 20.0;

		Draw(P_TitleLogo, Screen_W / 2.0 + bure_x, 200 + bure_y, 1.0, 0.0, 1.2);

		selectIndex = DrawSimpleMenu(selectIndex, 70, Screen_H - 330, 700, 30, @@_Buttons.map(v => v.Text));

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
