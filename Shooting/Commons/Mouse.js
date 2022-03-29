/*
	マウス・画面タッチ制御
*/

var @@_Down = false;
var @@_X = 0;
var @@_Y = 0;

function @@_ScreenPosToCanvasPos()
{
	var canvasRect = Canvas.getBoundingClientRect();

	@@_X -= canvasRect.left;
	@@_Y -= canvasRect.top;
	@@_X *= Screen_W / canvasRect.width;
	@@_Y *= Screen_H / canvasRect.height;
}

function @@_TouchStart(x, y)
{
	@@_Down = true;
	@@_X = x;
	@@_Y = y;

	@@_ScreenPosToCanvasPos();
}

function @@_TouchMove(x, y)
{
	@@_X = x;
	@@_Y = y;

	@@_ScreenPosToCanvasPos();
}

function @@_TouchEnd(x, y)
{
	@@_Down = false;
	@@_X = x;
	@@_Y = y;

	@@_ScreenPosToCanvasPos();
}

function @@_GetEvTouch(touch)
{
	var ret = function(event)
	{
		touch(event.changedTouches[0].pageX, event.changedTouches[0].pageY);
	};

	return ret;
}

function @@_GetEvMouse(touch)
{
	var ret = function(event)
	{
		touch(event.x, event.y);
	};

	return ret;
}

function @@_INIT()
{
	if (window.ontouchstart === null)
	{
		CanvasBox.ontouchstart = @@_GetEvTouch(@@_TouchStart);
		CanvasBox.ontouchmove  = @@_GetEvTouch(@@_TouchMove);
		CanvasBox.ontouchend   = @@_GetEvTouch(@@_TouchEnd);
	}
	else
	{
		CanvasBox.onmousedown  = @@_GetEvMouse(@@_TouchStart);
		CanvasBox.onmousemove  = @@_GetEvMouse(@@_TouchMove);
		CanvasBox.onmouseup    = @@_GetEvMouse(@@_TouchEnd);
		CanvasBox.onmouseleave = @@_GetEvMouse(@@_TouchEnd);
	}
}

var @@_DownCount;

function @@_EACH()
{
	if (1 <= @@_DownCount) // ? 前回_押下
	{
		if (@@_Down)
		{
			@@_DownCount++;
		}
		else
		{
			@@_DownCount = -1;
		}
	}
	else // ? 前回_押下していない。
	{
		if (@@_Down)
		{
			@@_DownCount = 1;
		}
		else
		{
			@@_DownCount = 0;
		}
	}
}

/*
	マウスボタンの押下(スクリーン・タッチ)状態を得る。
	戻り値：
		-1  == 離した。(前回は押下, 今回は押下していない)
		0   == 押していない。(前回も今回も押下していない)
		1   == 押した。(前回は押下していない, 今回は押下)
		2～ == 押している。値は押下し続けている長さ(フレーム数)
*/
function GetMouseDown()
{
	return @@_DownCount;
}

/*
	マウスボタンの押下(スクリーン・タッチ)状態をクリアする。
*/
function ClearMouseDown()
{
	@@_DownCount = 0;
}

/*
	マウスカーソルの位置を返す。X-座標
*/
function GetMouseX()
{
	return @@_X;
}

/*
	マウスカーソルの位置を返す。Y-座標
*/
function GetMouseY()
{
	return @@_Y;
}
