/*
	数学系
*/

/*
	ret: value を四捨五入した整数を返す。
*/
function <int> ToInt(<double> value)
{
	return Math.round(value);
}

/*
	ret: value の小数部を切り捨てた整数を返す。
		つまりゼロ方向で最寄りの整数を返す。
		例：
			2.4 -> 2
			-3.7 -> 3
*/
function <int> ToFix(<double> value)
{
	return Math.trunc(value);
}

/*
	value を minval, maxval の範囲に矯正する。
*/
function <Number> ToRange(<Number> value, <Number> minval, <Number> maxval)
{
	return Math.min(Math.max(value, minval), maxval);
}

/*
	ret: 0 ～ (modulo - 1) の整数をランダムに返す。
*/
function <int> GetRand(<int> modulo)
{
	return Math.floor(Math.random() * modulo);
}

/*
	0.0 ～ 1.0 の乱数を返す。
*/
function <double> GetRand1()
{
	return GetRand(IMAX + 1) / IMAX;
}

/*
	-1.0 ～ 1.0 の乱数を返す。
*/
function <double> GetRand2()
{
	return GetRand1() * 2.0 - 1.0;
}
