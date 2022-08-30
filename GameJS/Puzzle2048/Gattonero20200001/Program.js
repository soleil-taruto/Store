/*
	アプリケーション用メインモジュール
*/

setTimeout(Main, 0);

function <void> Main()
{
	@@_Loading();
}

function <void> @@_Loading()
{
	if (1 <= Loading)
	{
		@@_PrintLoading();
		setTimeout(@@_Loading, 100);
	}
	else
	{
		@@_PrintLoaded();
		@@_Loaded();
	}
}

var @@_LOADING_MAX = -1;

function <void> @@_PrintLoading()
{
	if (@@_LOADING_MAX == -1)
	{
		@@_LOADING_MAX = Loading;

		CanvasBox = document.getElementById("Gattonero20200001-CanvasBox");
		CanvasBox.style.width  = Screen_W;
		CanvasBox.style.height = Screen_H;
	}
	CanvasBox.innerHTML =
		"<div style='padding-top: " + ToFix(Screen_H / 2.0 - 10.0) + "px; text-align: center;'>" +
		ToFix((@@_LOADING_MAX - Loading) * 1000000000.0 / @@_LOADING_MAX) + " PPB LOADED..." +
		"</div>";
}

function <void> @@_PrintLoaded()
{
	@@_PrintLoading(); // force init

	CanvasBox.innerHTML = "";
	CanvasBox = null;
}

function <void> @@_Loaded()
{
	ProcMain(@@_Main());
}

function* <generatorForTask> @@_Main()
{
	if (DEBUG)
	{
		// -- choose one --

//		yield* Test01();
//		yield* Test02();
//		yield* Test03();
		yield* @@_Main2();

		// --
	}
	else
	{
		yield* @@_Main2();
	}
}

// 本番用メイン
function* <generatorForTask> @@_Main2()
{
	yield* GameMain();
//	yield* TitleMain();
}
