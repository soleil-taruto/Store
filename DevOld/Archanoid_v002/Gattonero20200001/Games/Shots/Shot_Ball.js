/*
	自弾 - ボール
*/

function <Shot_t> CreateShot_Ball(<doule> x, <double> y, <double> xAdd, <double> yAdd)
{
	var ret =
	{
		X: x,
		Y: y,
		AttackPoint: 1,
		Crash: null,

		// ここから固有

		<double> XAdd: xAdd, // X-速度
		<double> YAdd: yAdd, // Y-速度
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

/*
	行動と描画
	やるべきこと：
	-- 移動
	-- 当たり判定設置
	-- 描画
	戻り値：
	-- ? 生存
*/
function* <generatorForTask> @@_Draw(<Shot_t> shot)
{
	for (; ; )
	{
		shot.X += shot.XAdd;
		shot.Y += shot.YAdd;

		shot.Crash = CreateCrash_Circle(shot.X, shot.Y, 15.0);

		Draw(P_Ball, shot.X, shot.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Dead(<Shot_t> shot)
{
	// noop
}
