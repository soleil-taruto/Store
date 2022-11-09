/*
	敵 - 円形ブロック
*/

function <Enemy_t> CreateEnemy_CircleBlock(<double> x, <double> y, <int> hp, <Enemy_Block_Kind_e> b_kind)
{
	var ret =
	{
		Kind: Enemy_Kind_e_CIRCLE,
		X: x,
		Y: y,
		HP: hp,
		Crash: null,
		DestPt: null,

		// ここから固有

		<Enemy_Block_Kind_e> B_Kind: b_kind,
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	for (; ; )
	{
		if (enemy.DestPt != null)
		{
			enemy.X = Approach(enemy.X, enemy.DestPt.X, 0.9);
			enemy.Y = Approach(enemy.Y, enemy.DestPt.Y, 0.9);

			// ? 十分近づいた。-> 移動完了
			if (
				Math.abs(enemy.X - enemy.DestPt.X) < 0.5 &&
				Math.abs(enemy.Y - enemy.DestPt.Y) < 0.5
				)
			{
				enemy.X = enemy.DestPt.X;
				enemy.Y = enemy.DestPt.Y;

				enemy.DestPt = null;
			}
		}

		// 当たり判定_設置
		{
			var<double> BLOCK_RAD = 30;

			enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, BLOCK_RAD);
		}

		// ====
		// 描画ここから
		// ====

		var<Picture_t> picture;

		if (enemy.B_Kind == Enemy_Block_Kind_e_SOFT)
		{
			picture = P_Circle_Soft;
		}
		else if (enemy.B_Kind == Enemy_Block_Kind_e_NORM)
		{
			picture = P_Circle_Norm;
		}
		else if (enemy.B_Kind == Enemy_Block_Kind_e_HARD)
		{
			picture = P_Circle_Hard;
		}
		else
		{
			error();
		}

		Draw(picture, enemy.X, enemy.Y, 1.0, 0.0, 1.0);

		var<string> text = "" + enemy.HP;

		SetColor(I3ColorToString(CreateI3Color(255, 255, 255)));
		SetPrint(enemy.X - 6 * text.length, enemy.Y + 8, 0);
		SetFSize(20);
		PrintLine(text);

		yield 1;
	}
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_Dead(enemy);
}
