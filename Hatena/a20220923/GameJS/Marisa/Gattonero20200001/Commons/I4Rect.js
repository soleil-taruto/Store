/*
	I4Rect_t 型
*/

/@(ASTR)

/// I4Rect_t
{
	<int> L
	<int> T
	<int> W
	<int> H
}

@(ASTR)/

/*
	{ 左上座標, 幅, 高さ } から矩形領域を生成する。
*/
function <I4Rect_t> CreateI4Rect(<int> l, <int> t, <int> w, <int> h)
{
	var ret =
	{
		L: l,
		T: t,
		W: w,
		H: h,
	};

	return ret;
}

/*
	{ 左上座標, 右下座標 } から矩形領域を生成する。
*/
function <I4Rect_t> CreateI4Rect_LTRB(<int> l, <int> t, <int> r, <int> b)
{
	return CreateI4Rect(l, t, r - l, b - t);
}

/*
	{ 中心座標, 幅, 高さ } から矩形領域を生成する。
*/
function <I4Rect_t> CreateI4Rect_XYWH(<int> x, <int> y, <int> w, <int> h)
{
	return CreateI4Rect(x - ToInt(w / 2), y - ToInt(h / 2), w, h);
}

function <D4Rect_t> I4RectToD4Rect(<I4Rect_t> src)
{
	return CreateD4Rect(src.L, src.T, src.W, src.H);
}
