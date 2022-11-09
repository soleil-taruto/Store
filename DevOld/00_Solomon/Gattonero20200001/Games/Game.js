/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

function* <generatorForTask> GameMain(<int> mapIndex)
{
	// reset
	{
		@@_Enemies = [];
		@@_Shots = [];
	}

	LoadMap(mapIndex);

	PlayerX = Map.StartPt.X;
	PlayerY = Map.StartPt.Y;

	yield* @@_StartMotion();

	FreezeInput();

gameLoop:
	for (; ; )
	{
		if (GetMouseDown() == -1) // ★サンプル -- 要削除
		{
			var<double> x = GetMouseX();
			var<double> y = GetMouseY();

			AddEffect_Explode(x, y);
		}

		// ====
		// 描画ここから
		// ====

		@@_DrawWall();

		DrawPlayer(); // プレイヤーの描画

		// 敵の描画
		//
		for (var<int> index = 0; index < @@_Enemies.length; index++)
		{
			if (@@_Enemies[index].HP == -1) // ? 既に死亡
			{
				continue;
			}

			@@_Enemies[index].Crash = null; // reset

			if (!DrawEnemy(@@_Enemies[index]))
			{
				@@_Enemies[index].HP = -1;
			}
		}

		// 自弾の描画
		//
		for (var<int> index = 0; index < @@_Shots.length; index++)
		{
			if (@@_Shots[index].AttackPoint == -1) // ? 既に死亡
			{
				continue;
			}

			@@_Shots[index].Crash = null; // reset

			if (!DrawShot(@@_Shots[index]))
			{
				@@_Shots[index].AttackPoint = -1;
			}
		}

		// ★サンプル -- 要削除
		{
			SetColor("#ffffff");
			SetPrint(100, 100, 30);
			SetFSize(20);
			PrintLine(GetInput_2());
			PrintLine(GetInput_4());
			PrintLine(GetInput_6());
			PrintLine(GetInput_8());
			PrintLine(GetInput_A());
			PrintLine(GetInput_B());
		}

		@@_DrawFront();

		if (1 <= GetKeyInput(17)) // ? コントロール押下中 -> 当たり判定表示 (デバッグ用)
		{
			SetColor("#000000a0");
			PrintRect(0, 0, Screen_W, Screen_H);
			SetColor("#ffffffa0");

			DrawCrash(PlayerCrash);

			for (var<Enemy_t> enemy of @@_Enemies)
			{
				DrawCrash(enemy.Crash);
			}
			for (var<Shot_t> shot of @@_Shots)
			{
				DrawCrash(shot.Crash);
			}
		}

		// ====
		// 描画ここまで
		// ====

		// ====
		// 当たり判定ここから
		// ====

		for (var<int> enemyIndex = 0; enemyIndex < @@_Enemies.length; enemyIndex++)
		{
			var<Enemy_t> enemy = @@_Enemies[enemyIndex];

			if (enemy.HP == -1) // ? 既に死亡
			{
				continue;
			}

			for (var<int> shotIndex = 0; shotIndex < @@_Shots.length; shotIndex++)
			{
				var<Shot_t> shot = @@_Shots[shotIndex];

				if (shot.AttackPoint == -1) // ? 既に死亡
				{
					continue;
				}

				if (IsCrashed(enemy.Crash, shot.Crash)) // ? 衝突している。敵 vs 自弾
				{
					var<int> damagePoint = Math.min(enemy.HP, shot.AttackPoint);

					enemy.HP -= damagePoint;
					shot.AttackPoint -= damagePoint;

					if (enemy.HP <= 0) // ? 死亡した。
					{
						KillEnemy(enemy);
						break; // この敵は死亡したので、次の敵へ進む。
					}
					if (shot.AttackPoint <= 0) // ? 攻撃力を使い果たした。
					{
						KillShot(shot);
						continue; // この自弾は死亡したので、次の自弾へ進む。
					}
				}
			}

			if (enemy.HP == -1) // ? 既に死亡 (2回目)
			{
				continue;
			}

			if (IsCrashed(enemy.Crash, PlayerCrash)) // ? 衝突している。敵 vs 自機
			{
				// ★サンプル -- 要削除
				{
					if (enemy.Kind == Enemy_Kind_e_Goal) // ? ゴール到達 -> 次のステージへ
					{
						yield* @@_GoalMotion();

						break gameLoop;
					}
					else
					{
						AddEffect_Explode(PlayerX, PlayerY);

						PlayerX = FIELD_L + FIELD_W / 2;
						PlayerY = FIELD_T + FIELD_H / 2;
					}
				}
			}
		}

		// ====
		// 当たり判定ここまで
		// ====

		RemoveAll(@@_Enemies, function <boolean> (<Enemy_t> enemy)
		{
			return enemy.HP == -1; // ? 死亡
		});

		RemoveAll(@@_Shots, function <boolean> (<Shot_t> shot)
		{
			return shot.AttackPoint == -1; // ? 死亡
		});

		yield 1;
	}
	FreezeInput();
}

function <Enemy_t[]> GetEnemies()
{
	return @@_Enemies;
}

function <Shot_t[]> GetShots()
{
	return @@_Shots;
}

/*
	背面描画
*/
function <void> @@_DrawWall()
{
	SetColor(I3ColorToString(CreateI3Color(0, 0, 0)));
	PrintRect(FIELD_L, FIELD_T, FIELD_W, FIELD_H);

	for (var<int> x = 0; x < MAP_X_SIZE; x++)
	for (var<int> y = 0; y < MAP_Y_SIZE; y++)
	{
		var<int> dx = FIELD_L + x * MAP_CELL_W + MAP_CELL_W / 2;
		var<int> dy = FIELD_T + y * MAP_CELL_H + MAP_CELL_H / 2;

		if (Map.Table[x][y].WallFlag)
		{
			Draw(P_Wall, dx, dy, 1.0, 0.0, 1.0);
		}
	}
}

/*
	前面描画
*/
function <void> @@_DrawFront()
{
	SetColor(I3ColorToString(CreateI3Color(80, 60, 40)));
	PrintRect(0, 0, FIELD_L, Screen_H);
	PrintRect(0, 0, Screen_W, FIELD_T);
	PrintRect(
		0,
		FIELD_B,
		Screen_W,
		Screen_H - FIELD_B
		);
	PrintRect(
		FIELD_R,
		0,
		Screen_W - FIELD_R,
		Screen_H
		);
}

function* <generatorForTask> @@_StartMotion()
{
	// TODO
	// TODO
	// TODO
}

function* <generatorForTask> @@_GoalMotion()
{
	// TODO
	// TODO
	// TODO
}
