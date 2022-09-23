/*
	D3Color_t 型
*/

/@(ASTR)

// アルファ値の無い色または色の比率を表す。
// 各色は 0.0 ～ 1.0 を想定する。

/// D3Color_t
{
	<double> R
	<double> G
	<double> B
}

@(ASTR)/

function <D3Color_t> CreateD3Color(<double> r, <double> g, <double> b)
{
	var ret =
	{
		R: r,
		G: g,
		B: b,
	};

	return ret;
}

function <I3Color_t> D3ColorToI3Color(<D3Color_t> src)
{
	return CreateI3Color(
		ToInt(src.R * 255.0),
		ToInt(src.G * 255.0),
		ToInt(src.B * 255.0)
		);
}

function <D4Color_t> D3ColorToD4Color(<D3Color_t> src, <double> a)
{
	return CreateD4Color(
		src.R,
		src.G,
		src.B,
		a
		);
}
