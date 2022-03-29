/*
	アプリケーション用メインモジュール
*/

var APP_IDENT = "{38e2fd96-2424-4de8-a020-88d6cfbebef9}"; // アプリ毎に変更する。

window.onload = function() { Main(); }; // エントリーポイント呼び出し

// エントリーポイント
function Main()
{
	ProcMain(@@_Main());
}

// メイン
function* @@_Main()
{
	// リソース読み込み中は待機
	while (1 <= Loading)
	{
		SetColor("#ffffff");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(10, 25, 50);
		SetFont("16px 'メイリオ'");
		PrintLine("リソースを読み込んでいます ...　残り " + Loading + " 個");

		yield 1;
	}

	// -- choose one --

	yield* @@_Main2();
//	yield* Test01();
//	yield* Test02();
//	yield* Test03();

	// --
}

// 本番用メイン
function* @@_Main2()
{
	yield* TitleMain();
}
