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
		つまりゼロ方向で最寄りの整数を返す。自身が整数であれば自身を返す。
		例：
			2.4 -> 2
			-3.7 -> -3
			586.0 -> 586
*/
function <int> ToFix(<double> value)
{
	return Math.trunc(value);
}

/*
	ret: value 以下で最大の整数を返す。
		つまりマイナス無限大方向で最寄りの整数を返す。自身が整数であれば自身を返す。
		例：
			2.4 -> 2
			-3.7 -> -4
			586.0 -> 586
*/
function <int> ToFloor(<double> value)
{
	return Math.floor(value);
}

/*
	value を minval, maxval の範囲に矯正する。
*/
function <Number> ToRange(<Number> value, <Number> minval, <Number> maxval)
{
	return Math.min(Math.max(value, minval), maxval);
}

/*
	0 ～ (modulo - 1) の乱数を返す。(int)
*/
function <int> GetRand(<int> modulo)
{
	return Math.floor(Math.random() * modulo);
}

/*
	minval ～ maxval の乱数を返す。(int)
*/
function <int> GetRandRange(<int> minval, <int> maxval)
{
	return GetRand((maxval - minval) + 1) + minval;
}

/*
	-1 または 1 をランダムに返す。
*/
function <int> GetRandSign()
{
	return GetRand(2) * 2 - 1;
}

/*
	0.0 ～ 1.0 の乱数を返す。(double)
*/
function <double> GetRand1()
{
	return GetRand(IMAX + 1) / IMAX;
}

/*
	-1.0 ～ 1.0 の乱数を返す。(double)
*/
function <double> GetRand2()
{
	return GetRand1() * 2.0 - 1.0;
}

/*
	minval ～ maxval の乱数を返す。(double)
*/
function <double> GetRand3(<double> minval, <double> maxval)
{
	return GetRand1() * (maxval - minval) + minval;
}

/*
	配列の要素(1つ)をランダムで返す。
*/
function <T> ChooseOne(<T[]> arr)
{
	return arr[GetRand(arr.length)];
}
