/*
	アプリケーション用メインモジュール
*/

var APP_IDENT = "{ccd34185-1876-4fd0-a703-aa3b57d4b360}"; // アプリ毎に変更する。

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
		SetSize(16);
		SetPrint(10, 25, 50);
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
