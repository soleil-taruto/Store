/*
	D2Size_t Œ^
*/

/@(ASTR)

/// D2Size_t
{
	<double> W
	<double> H
}

@(ASTR)/

function <D2Size_t> CreateD2Size(<double> w, <double> h)
{
	var ret =
	{
		W: w,
		H: h,
	};

	return ret;
}

function <I2Size_t> D2SizeToI2Size(<D2Size_t> src)
{
	return CreateI2Size(ToInt(src.W), ToInt(src.H));
}
