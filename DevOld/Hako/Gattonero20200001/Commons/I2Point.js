/*
	I2Point_t Œ^
*/

/@(ASTR)

/// I2Point_t
{
	<int> X
	<int> Y
}

@(ASTR)/

function <I2Point_t> CreateI2Point(<int> x, <int> y)
{
	var ret =
	{
		X: x,
		Y: y,
	};

	return ret;
}

function <D2Point_t> I2PointToD2Point(<I2Point_t> src)
{
	return CreateD2Point(src.X, src.Y);
}
