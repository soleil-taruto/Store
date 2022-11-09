/*
	図形・文字列描画
*/

/*
	色をセットする。
	color: ex. "#ff0000", "red"
		アルファ値あり -> "#ff000080"
*/
function <void> SetColor(<string> color)
{
	Context.fillStyle = color;
}

/*
	矩形を描画する。
	(l, t, w, h): 左側 X-座標, 上側 Y-座標, 幅, 高さ
*/
function <void> PrintRect(<double> l, <double> t, <double> w, <double> h)
{
	Context.fillRect(l, t, w, h);
}

/*
	矩形を描画する。
	(x, y, w, h): 中心 X-座標, 中心 Y-座標, 幅, 高さ
*/
function <void> PrintRect_XYWH(<double> x, <double> y, <double> w, <double> h)
{
	Context.fillRect(x - w / 2, y - h / 2, w, h);
}

/*
	円を描画する。
	(x, y, r): 中心 X-座標, 中心 Y-座標, 半径
*/
function <void> PrintCircle(<double> x, <double> y, <double> r)
{
	Context.beginPath();
	Context.arc(x, y, r, 0, Math.PI * 2.0, false);
	Context.fill();
}

/*
	フォントをセットする。
	font: ex. "16px 'sans-serif'"
*/
function <void> SetFont(<string> font)
{
	Context.font = font;
}

/*
	フォントサイズをセットする。
	size: ex. 16
*/
function <void> SetFSize(<int> size)
{
	SetFont(size + "px 'sans-serif'");
}

var<int> @@_X = 0;
var<int> @@_Y = 0;
var<int> @@_YStep = 0;

/*
	文字列描画位置をセットする。
	x: 文字列の左側 X-座標
	y: 文字列の下側 Y-座標
	yStep: 改行ステップ Y-軸 (改行しない場合は 0 を指定すること)
*/
function <void> SetPrint(<int> x, <int> y, <int> yStep)
{
	@@_X = x;
	@@_Y = y;
	@@_YStep = yStep;
}

/*
	文字列を描画する。
	line: 文字列
*/
function <void> PrintLine(<string> line)
{
	Context.fillText(line, @@_X, @@_Y);
	@@_Y += @@_YStep;
}
