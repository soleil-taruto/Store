/*
	D2Point_t Œ^
*/

/@(ASTR)

/// D2Point_t
{
	<double> X
	<double> Y
}

@(ASTR)/

function <D2Point_t> CreateD2Point(<double> x, <double> y)
{
	var ret =
	{
		X: x,
		Y: y,
	};

	return ret;
}

function <I2Point_t> D2PointToI2Point(<D2Point_t> src)
{
	return CreateI2Point(ToInt(src.X), ToInt(src.Y));
}
