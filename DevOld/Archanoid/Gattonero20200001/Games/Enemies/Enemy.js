/*
	敵
*/

/@(ASTR)

/// Enemy_t
{
	<Enemy_Kind_e> Kind // 敵の種類

	<double> X // X-位置
	<double> Y // Y-位置

	// 体力
	// 0 == 無敵
	// -1 == 死亡
	// 1～ == 残り体力
	//
	<int> HP

	// 行動と描画
	// 処理すべきこと：
	// -- 行動
	// -- 当たり判定の設置
	// -- 描画
	// 偽を返すとブロックを破棄する。
	//
	<generatorForTask> Draw

	<Crash_t> Crash // 今フレームの当たり判定置き場

	<Action_Enemy_t> Dead // 死亡イベント
}

@(ASTR)/

function <boolean> DrawEnemy(<Enemy_t> enemy) // ret: ? 生存
{
	return enemy.Draw.next().value;
}

function <void> KillEnemy(<Enemy_t> enemy)
{
	if (enemy.HP != -1) // ? まだ死亡していない。
	{
		enemy.HP = -1; // 死亡させる。
		EnemyDead(enemy);
	}
}

// 死亡イベント実行
function <void> EnemyDead(<Enemy_t> enemy)
{
	enemy.Dead(enemy);
}
