/*
	I4Rect_t Œ^
*/

/@(ASTR)

/// I4Rect_t
{
	<double> L
	<double> T
	<double> W
	<double> H
}

@(ASTR)/

function <I4Rect_t> CreateI4Rect(<double> l, <double> t, <double> w, <double> h)
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

function <D4Rect_t> I4RectToD4Rect(<I4Rect_t> src)
{
	return CreateD4Rect(src.L, src.T, src.W, src.H);
}
