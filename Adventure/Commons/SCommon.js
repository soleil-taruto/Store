/*
	共通機能・便利機能はできるだけこのファイルに集約する。
*/

/*
	ret: value を四捨五入した整数を返す。
*/
function ToInt(value)
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
function ToFix(value)
{
	return Math.trunc(value);
}

/*
	value を minval, maxval の範囲に矯正する。
*/
function ToRange(value, minval, maxval)
{
	return Math.min(Math.max(value, minval), maxval);
}

/*
	modulo: 1以上の整数

	ret: 0 ～ (modulo - 1) の整数をランダムに返す。
*/
function GetRand(modulo)
{
	return ToFix(Math.random() * modulo);
}
