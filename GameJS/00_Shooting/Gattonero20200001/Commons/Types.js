/*
	型
*/

/@(ASTR)

/// P4Poly_t
{
	<D2Point_t> LT // 左上
	<D2Point_t> RT // 右上
	<D2Point_t> RB // 右下
	<D2Point_t> LB // 左下
}

@(ASTR)/

/*
	プリミティブな型
*/
/// int
/// double
/// string
/// void
/// Action
/// Func

/*
	int | double
*/
/// Number

/*
	長さ 1 の string
*/
/// char

/*
	ジェネレータ
	但し、タスク用
	yield には 1 を返し続ける。
*/
/// generatorForTask

/*
	連想配列
*/
/// map

/*
	標準クラス
*/
/// Audio
/// Image
/// Gamepad
/// GamepadButton
