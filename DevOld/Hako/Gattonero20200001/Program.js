/*
	アプリケーション用メインモジュール
*/

var<string> APP_IDENT = "{c9e92c41-52cf-44fe-8c46-b5139531e666}";

window.onload = function()
{
	Main();
};

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
	}

	var<int> pbn = ToInt((@@_LOADING_MAX - Loading) * 108.0 / @@_LOADING_MAX);

	document.body.innerHTML = "<div style='font-size: 108px;'>" + pbn + " PBN COMPLETE...</div>";
}

function <void> @@_PrintLoaded()
{
	document.body.innerHTML = "";
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

//		yield* Test01(); // 各ステージをプレイ
//		yield* Test02(); // エンディング
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
	yield* TitleMain();
}
