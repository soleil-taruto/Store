/*
	ゲーム・ローディング画面
*/

var<int> @@_LOADING_MAX = -1;

function <void> PrintGameLoading()
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

function <void> PrintGameLoaded()
{
	PrintGameLoading(); // force init

	CanvasBox.innerHTML = "";
	CanvasBox = null;
}
