/*
	D4Rect_t Œ^
*/

/@(ASTR)

/// D4Rect_t
{
	<double> L
	<double> T
	<double> W
	<double> H
}

@(ASTR)/

function <D4Rect_t> CreateD4Rect(<double> l, <double> t, <double> w, <double> h)
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

function <I4Rect_t> D4RectToI4Rect(<D4Rect_t> src)
{
	return CreateI4Rect(ToInt(src.L), ToInt(src.T), ToInt(src.W), ToInt(src.H));
}
