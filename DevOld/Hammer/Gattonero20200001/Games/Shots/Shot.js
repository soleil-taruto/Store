/*
	自弾
*/

/@(ASTR)

/// Shot_t
{
	<int> Kind // 自弾の種類

	<double> X // X-位置
	<double> Y // Y-位置

	<generatorForTask> Draw // 行動と描画

	<Crash_t> Crash; // 今フレームの当たり判定置き場, null で初期化すること。null == 当たり判定無し
}

@(ASTR)/

/*
	行動と描画
*/
function <boolean> DrawShot(<Shot_t> shot) // ret: ? 生存
{
	return shot.Draw.next().value;
}
