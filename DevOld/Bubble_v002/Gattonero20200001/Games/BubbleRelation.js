/*
	爆発処理
*/

/*
	Enemy_t 追加フィールド
	{
		<int> @@_Reached // 0: 未到達, 1: 到達, 2: 到達 && チェック済み
	}
*/

var<double> @@_EXPLODE_R = 45.0;

var<boolean> @@_Busy = false;
var<Enemy_t> @@_ComboBaseEnemy = null;

/*
	comboEnmey: null == 着弾先無し
*/
function <void> BubbleRelation_着弾による爆発(<Enemy_t[]> enemies, <Enemy_t> baseEnemy, <Enemy_t> comboEnemy)
{
	enemies = CloneArray(enemies);

	AddEffect(function* ()
	{
		yield* @@_LiteLock_Enter();

		if (
			comboEnemy != null &&
			comboEnemy.@@_Reached == 2 &&
			comboEnemy.Color != baseEnemy.Color
			)
		{
			@@_ComboBaseEnemy = baseEnemy;
			@@_A_Reached(baseEnemy);
		}

		while (@@_Busy)
		{
			yield 1;
		}
		@@_Busy = true;
		@@_LiteLock_Leave();

		for (var<Enemy_t> enemy of enemies)
		{
			enemy.@@_Reached = 0;
		}

		baseEnemy.@@_Reached = 1;

		@@_A_Reached(baseEnemy);

		var<int> extendCount = 0;

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
					extendCount++;

					if (extendCount % 5 == 0)
					{
						yield 1;
					}
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

			if (@@_ComboBaseEnemy == null) // ? 通常の爆発
			{
				Shuffle(enemies);

				for (var<Enemy_t> enemy of enemies)
				{
					if (enemy.@@_Reached == 2)
					{
						KillEnemy(enemy);
						extendCount++;

						if (extendCount % 2 == 0) // 重いので 5 -> 2
						{
							yield 1;
						}
					}
				}
			}
			else // ? コンボ
			{
				for (var<Enemy_t> enemy of enemies)
				{
					if (enemy.@@_Reached == 2)
					{
						enemy.@@_Reached = 0;
						enemy.Color = @@_ComboBaseEnemy.Color;
						@@_A_Reached(enemy);
						extendCount++;

						if (extendCount % 5 == 0)
						{
							yield 1;
						}
					}
				}
			}
		}

		@@_Busy = false;
		@@_ComboBaseEnemy = null;
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

var<boolean[]> @@_LLFlagStack =
[
	false, false, false, false, false, false, false, false, false, false,
	false, false, false, false, false, false, false, false, false, false,
	false, false, false, false, false, false, false, false, false, false,
];

function* <generatorForTask> @@_LiteLock_Enter()
{
	for (var<int> c = 0; c < @@_LLFlagStack.length; c++)
	{
		while (@@_LLFlagStack[c])
		{
			yield 1;
		}
		@@_LLFlagStack[c] = true;

		if (1 <= c)
		{
			@@_LLFlagStack[c - 1] = false;
		}
	}
}

function <void> @@_LiteLock_Leave()
{
	@@_LLFlagStack[@@_LLFlagStack.length - 1] = false;
}
