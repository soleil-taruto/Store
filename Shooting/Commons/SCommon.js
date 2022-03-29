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
	value を minval, maxval の範囲に矯正する。
*/
function ToRange(value, minval, maxval)
{
	return Math.min(Math.max(value, minval), maxval);
}

/*
	ret: 0 ～ (modulo - 1) の整数をランダムに返す。
*/
function GetRand(modulo)
{
	return Math.floor(Math.random() * modulo);
}
