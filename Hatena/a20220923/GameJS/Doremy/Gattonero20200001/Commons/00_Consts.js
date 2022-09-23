/*
	定数
*/

/*
	整数の上限として慣習的に決めた値
	・10億
	・10桁
	・9桁の最大値+1
	・2倍しても INT_MAX (2147483647 == 2^31-1) より小さい
*/
var<int> IMAX = 1000000000;

/*
	とても小さい正数として慣習的に決めた値
	・doubleの許容誤差として
*/
var<double> MICRO = 1.0 / IMAX;

/*
	半角・数字
*/
var<string> DECIMAL = "0123456789";

/*
	半角・英大文字
*/
var<string> ALPHA_UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

/*
	半角・英小文字
*/
var<string> ALPHA_LOWER = "abcdefghijklmnopqrstuvwxyz";
