/*
	ゲーム用メインモジュール
*/

// *_INIT イベントのリスト
var<Action[]> @@_INIT_Events = [ @(INIT) ];

// *_EACH イベントのリスト
var<Action[]> @@_EACH_Events = [ @(EACH) ];

// アプリケーション側の処理
// ジェネレータ関数であること。
var<generatorForTask> @@_AppMain;

// 描画先Canvasタグ
var<Tag_Canvas> Canvas;

// Canvasを入れておくDivタグ
var<Tag_Div> CanvasBox;

// ゲーム用メイン
// appMain: アプリケーション側の処理
// -- ジェネレータ関数であること。
function <void> ProcMain(<generatorForTask> appMain)
{
	@@_AppMain = appMain;

	Canvas = document.createElement("canvas");
	Canvas.style.position = "fixed";
	Canvas.width  = Screen_W;
	Canvas.height = Screen_H;

	CanvasBox = document.createElement("div");
	CanvasBox.style.position = "fixed";
	CanvasBox.appendChild(Canvas);
	document.body.appendChild(CanvasBox);

	AddResized(@@_Resized);
	@@_Resized();

	for (var<Action> event of @@_INIT_Events)
	{
		LOGPOS();
		event();
		LOGPOS();
	}

	LoadLocalStorage();
	@@_Anime();
}

function <void> @@_Resized()
{
	var sw = window.innerWidth;
	var sh = window.innerHeight;

	var w = sw;
	var h = ToInt((Screen_H * sw) / Screen_W);

	if (sh < h)
	{
		h = sh;
		w = ToInt((Screen_W * sh) / Screen_H);
	}
	var<int> l = ToInt((sw - w) / 2);
	var<int> t = ToInt((sh - h) / 2);

	Canvas.style.left   = l + "px";
	Canvas.style.top    = t + "px";
	Canvas.style.width  = w + "px";
	Canvas.style.height = h + "px";

	CanvasBox.style.left   = l + "px";
	CanvasBox.style.top    = t + "px";
	CanvasBox.style.width  = w + "px";
	CanvasBox.style.height = h + "px";
}

// リフレッシュレート高過ぎ検知用時間
var<int> @@_HzChaserTime = 0;

// プロセスフレームカウンタ
var<int> ProcFrame = 0;

// 描画先コンテキスト(描画先スクリーン)
var<Context2d> Context = null;

function <void> @@_Anime()
{
	var<int> currTime = new Date().getTime();

	@@_HzChaserTime = Math.max(@@_HzChaserTime, currTime - 100);
	@@_HzChaserTime = Math.min(@@_HzChaserTime, currTime + 100);

	if (@@_HzChaserTime < currTime)
	{
		Context = Canvas.getContext("2d");
		@@_AppMain.next();

		for (var<Action> event of @@_EACH_Events)
		{
			event();
		}

		Context = null;
		@@_HzChaserTime += 16;
		ProcFrame++;
	}
	requestAnimationFrame(@@_Anime);
}
