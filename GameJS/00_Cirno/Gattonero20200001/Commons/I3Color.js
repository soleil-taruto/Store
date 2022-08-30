/*
	I3Color_t 型
*/

/@(ASTR)

// アルファ値の無い色を表す。
// 各色は 0 ～ 255 を想定する。
// R を -1 にすることによって無効な色を示す。

/// I3Color_t
{
	<int> R // -1 == 無効
	<int> G
	<int> B
}

@(ASTR)/

function <I3Color_t> CreateI3Color(<double> r, <double> g, <double> b)
{
	var ret =
	{
		R: r,
		G: g,
		B: b,
	};

	return ret;
}

function <D3Color_t> I3ColorToD3Color(<I3Color_t> src)
{
	return CreateD3Color(
		src.R / 255.0,
		src.G / 255.0,
		src.B / 255.0
		);
}

function <I4Color_t> I3ColorToI4Color(<I3Color_t> src, <int> a)
{
	return CreateI4Color(
		src.R,
		src.G,
		src.B,
		a
		);
}

function <string> I3ColorToString(<I3Color_t> src)
{
	var ret = "";
	ret += "#";
	ret += ZPad(ToHex(src.R), 2, "0");
	ret += ZPad(ToHex(src.G), 2, "0");
	ret += ZPad(ToHex(src.B), 2, "0");

	return ret;
}
