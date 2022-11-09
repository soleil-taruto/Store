/*
	アクター (ゲーム・オブジェクト)
*/

/@(ASTR)

/// Actor_t
{
	<int> Kind // アクターの種類

	<double> X // X-位置
	<double> Y // Y-位置

	// 行動と描画
	// 処理すべきこと：
	// -- 行動
	// -- 当たり判定の設置
	// -- 描画
	// 偽を返すとこのアクターを破棄する。
	//
	<generatorForTask> Draw

	<boolean> Killed // 死亡フラグ
}

@(ASTR)/

/*
	行動と描画
*/
function <boolean> DrawActor(<Actor_t> actor) // ret: ? 生存
{
	return NextVal(actor.Draw);
}
