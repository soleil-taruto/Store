/*
	爆発処理
*/

/*
	Enemy_t 追加フィールド
	{
		<boolean> @@_Reached // 0: 未到達, 1: 到達, 2: 到達 && チェック済み
	}
*/

var<double> @@_EXPLODE_R = 45.0;

var<boolean> @@_Busy = false;

function <void> BubbleRelation_着弾による爆発(<Enemy_t[]> enemies, <Enemy_t> baseEnemy)
{
	enemies = CloneArray(enemies);

	AddEffect(function* ()
	{
		while (@@_Busy)
		{
			yield 1;
		}
		@@_Busy = true;

		for (var<Enemy_t> enemy of enemies)
		{
			enemy.@@_Reached = 0;
		}

		baseEnemy.@@_Reached = 1;

		@@_A_Reached(baseEnemy);

		for (var<boolean> extended = true; extended; )
		{
			extended = false;

			for (var<Enemy_t> enemy of enemies)
			{
				if (enemy.@@_Reached == 1)
				{
					enemy.@@_Reached = 2;
					@@_Extend(enemies, enemy);
					extended = true;
					yield 1;
				}
			}
		}

		var<int> count = 0;

		for (var<Enemy_t> enemy of enemies)
		{
			if (enemy.@@_Reached == 2)
			{
				count++;
			}
		}

		if (3 <= count)
		{
			for (var<int> w = 0; w < 30; w++) // ウェイト
			{
				yield 1;
			}

			for (var<Enemy_t> enemy of enemies)
			{
				if (enemy.@@_Reached == 2)
				{
					KillEnemy(enemy);

					for (var<int> w = 0; w < 5; w++) // ウェイト
					{
						yield 1;
					}
				}
			}
		}

		@@_Busy = false;
	}());
}

function <void> @@_Extend(<Enemy_t[]> enemies, <Enemy_t> baseEnemy)
{
	for (var<Enemy_t> enemy of enemies)
	{
		if (
			enemy.@@_Reached == 0 &&
			enemy.Color == baseEnemy.Color && // 同じ色のみ伝搬する。
			Math.abs(enemy.X - baseEnemy.X) < @@_EXPLODE_R &&
			Math.abs(enemy.Y - baseEnemy.Y) < @@_EXPLODE_R &&
			GetDistance(enemy.X - baseEnemy.X, enemy.Y - baseEnemy.Y) < @@_EXPLODE_R
			)
		{
			enemy.@@_Reached = 1;

			@@_A_Reached(enemy);
		}
	}
}

function <void> @@_A_Reached(<Enemy_t> enemy)
{
	AddEffect(function* ()
	{
		for (var<Scene_t> scene of CreateScene(20))
		{
			Draw(
				P_Balls[enemy.Color],
				enemy.X,
				enemy.Y,
				1.0 * scene.RemRate,
				0.0,
				2.0 + 3.0 * scene.Rate,
				);

			yield 1;
		}
	}());
}
