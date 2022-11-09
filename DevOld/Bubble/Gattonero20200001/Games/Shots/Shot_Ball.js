/*
	自弾 - ボール
*/

function <Shot_t> CreateShot_Ball(<doule> x, <double> y, <double> xAdd, <double> yAdd, <BallColor_e> color)
{
	var ret =
	{
		Kind: Shot_Kind_e_BALL,
		X: x,
		Y: y,
		AttackPoint: 1,
		Crash: null,

		// ここから固有

		<double> XAdd: xAdd, // X-速度
		<double> YAdd: yAdd, // Y-速度

		<BallColor_e> Color: color,
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Shot_t> shot)
{
	for (; ; )
	{
		shot.X += shot.XAdd;
		shot.Y += shot.YAdd;

		shot.Crash = CreateCrash_Circle(shot.X, shot.Y, 15.0);

		Draw(P_Balls[shot.Color], shot.X, shot.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Dead(<Shot_t> shot)
{
	// noop
}
