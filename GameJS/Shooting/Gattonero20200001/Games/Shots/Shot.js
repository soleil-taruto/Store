/*
	自弾
*/

/@(ASTR)

/// Shot_t
{
	<int> Kind // 自弾の種類

	<double> X // X-位置
	<double> Y // Y-位置

	// 攻撃力
	// 0 == 不使用・予約
	// -1 == 死亡
	// 1～ == 残り攻撃力
	//
	<int> AttackPoint

	// 行動と描画
	// 処理すべきこと：
	// -- 行動
	// -- 当たり判定の設置
	// -- 描画
	// 偽を返すとこの自弾を破棄する。
	//
	<generatorForTask> Draw

	<Crash_t> Crash; // 今フレームの当たり判定置き場, null で初期化すること。null == 当たり判定無し

	<Action Shot_t> Dead // 死亡イベント
}

@(ASTR)/

/*
	行動と描画
*/
function <boolean> DrawShot(<Shot_t> shot) // ret: ? 生存
{
	return NextVal(shot.Draw);
}

/*
	死亡
*/
function <void> KillShot(<Shot_t> shot)
{
	if (shot.AttackPoint != -1) // ? まだ死亡していない。
	{
		shot.AttackPoint = -1; // 死亡させる。
		@@_DeadShot(shot);
	}
}

function <void> @@_DeadShot(<Shot_t> shot)
{
	shot.Dead(shot);
}
