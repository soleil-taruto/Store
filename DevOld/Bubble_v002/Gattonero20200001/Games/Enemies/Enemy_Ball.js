/*
	敵 - ボール
*/

function <Enemy_t> CreateEnemy_Ball(<double> x, <double> y, <int> hp, <BallColor_e> color)
{
	var ret =
	{
		Kind: Enemy_Kind_e_BALL,
		X: x,
		Y: y,
		HP: hp,
		Crash: null,

		// ここから固有

		<BallColor_e> Color: color,
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	for (; ; )
	{
		enemy.Y += 0.1;
//		enemy.Y += ProcFrame % 10 == 0 ? 1 : 0;

		// 当たり判定_設置
		{
			var<double> BLOCK_RAD = 15;

			enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, BLOCK_RAD);
		}

		// ====
		// 描画ここから
		// ====

		var<Picture_t> picture = P_Balls[enemy.Color];

		Draw(picture, enemy.X, enemy.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_Dead(enemy);
}
