/*
	ゲーム用メインモジュール
*/

// 画面の幅
var Screen_W = 960;

// 画面の高さ
var Screen_H = 540;

// アプリケーション側の処理
// ジェネレータ関数であること。
var @@_AppMain;

// *_INIT イベントのリスト
var @@_INIT_Events = [ @(INIT) ];

// *_EACH イベントのリスト
var @@_EACH_Events = [ @(EACH) ];

// 描画先Canvasタグ
var Canvas;

// Canvasを入れておくDivタグ
var CanvasBox;

// ゲーム用メイン
// appMain: アプリケーション側の処理
// -- ジェネレータ関数であること。
function ProcMain(appMain)
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

	for (var event of @@_INIT_Events)
	{
		LOGPOS;
		event();
		LOGPOS;
	}

	@@_Anime();
}

function @@_Resized()
{
	var sw = window.innerWidth;
	var sh = window.innerHeight;

	var w = sw;
	var h = Math.round((Screen_H * sw) / Screen_W);

	if (sh < h)
	{
		h = sh;
		w = Math.round((Screen_W * sh) / Screen_H);
	}
	var l = Math.round((sw - w) / 2);
	var t = Math.round((sh - h) / 2);

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
var @@_HzChaserTime = 0;

// プロセスフレームカウンタ
var ProcFrame = 0;

// 描画先コンテキスト(描画先スクリーン)
var Context = null;

function @@_Anime()
{
	var currTime = new Date().getTime();

	@@_HzChaserTime = ToRange(
		@@_HzChaserTime,
		currTime - 100,
		currTime + 100
		);

	if (@@_HzChaserTime < currTime)
	{
		Context = Canvas.getContext("2d");
		@@_AppMain.next();

		for (var event of @@_EACH_Events)
		{
			event();
		}

		Context = null;
		@@_HzChaserTime += 16;
		ProcFrame++;
	}
	requestAnimationFrame(@@_Anime);
}
