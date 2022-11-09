/*
	孤立したブロックを除去
*/

/*
	Enemy_t 追加フィールド
	{
		<boolean> @@_Reached // 0: 未到達, 1: 到達, 2: 到達 && チェック済み
	}
*/

function <void> BubbleRelation_孤立したブロックを除去_Start()
{
	AddEffect(function* <generatorForTask> ()
	{
		for (; ; )
		{
			yield* @@_Main();

			for (var<int> w = 0; w < 30; w++) // ウェイト
			{
				yield 1;
			}
		}
	}());
}

var<double> @@_EXPLODE_R = 45.0;

function* <generatorForTask> @@_Main()
{
	var<Enemy_t[]> enemies = GetEnemies();

	enemies = CloneArray(enemies);

	for (var<Enemy_t> enemy of enemies)
	{
		enemy.@@_Reached = 0;
	}

	for (var<Enemy_t> enemy of enemies)
	{
		if (enemy.Y < @@_EXPLODE_R)
		{
			enemy.@@_Reached = 1;

			@@_A_Reached(enemy);
		}
	}

	Shuffle(enemies);

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

	enemies.sort(function <int> (<Enemy_t> a, <Enemy_t> b)
	{
		return (a.X + a.Y) - (b.X + b.Y);
	});

	for (var<Enemy_t> enemy of enemies)
	{
		if (enemy.@@_Reached == 0) // ? 孤立
		{
			KillEnemy(enemy);

			/*
			for (var<int> w = 0; w < 5; w++) // ウェイト
			{
				yield 1;
			}
			*/
			yield 1;
		}
	}
}

function <void> @@_Extend(<Enemy_t[]> enemies, <Enemy_t> baseEnemy)
{
	for (var<Enemy_t> enemy of enemies)
	{
		if (
			enemy.@@_Reached == 0 &&
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
	if (enemy.HP == -1) // ? 既に死亡
	{
		return;
	}

	AddEffect(function* ()
	{
		for (var<Scene_t> scene of CreateScene(20))
		{
			Draw(
				P_Balls[enemy.Color],
				enemy.X,
				enemy.Y,
				0.5,
				scene.RemRate * Math.PI * 2,
				1.0,
				);

			yield 1;
		}
	}());
}
