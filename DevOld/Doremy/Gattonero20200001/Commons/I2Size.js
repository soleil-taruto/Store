/*
	I2Size_t Œ^
*/

/@(ASTR)

/// I2Size_t
{
	<int> W
	<int> H
}

@(ASTR)/

function <I2Size_t> CreateI2Size(<int> w, <int> h)
{
	var ret =
	{
		W: w,
		H: h,
	};

	return ret;
}

function <D2Size_t> I2SizeToD2Size(<I2Size_t> src)
{
	return CreateD2Size(src.W, src.H);
}
