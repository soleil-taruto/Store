/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

var<GameEndReason_e> GameEndReason = GameEndReason_e_STAGE_CLEAR;
var<int> GameLastPlayedStageIndex = 0;

/*
	ゲーム終了リクエスト(タイトルへ戻る)
*/
var<boolean> GameRequestReturnToTitleMenu = false;

/*
	ゲーム再スタート・リクエスト
*/
var<boolean> @@_RequestRestart = false;

function* <generatorForTask> GameMain(<int> mapIndex)
{
	var<Func boolean> f_ゴミ回収 = Supplier(@@_T_ゴミ回収());

	// reset
	{
		@@_Enemies = [];
		@@_Shots = [];

		GameEndReason = GameEndReason_e_STAGE_CLEAR;
		GameLastPlayedStageIndex = 0;
		GameRequestReturnToTitleMenu = false;
		@@_RequestRestart = false;

		ResetPlayer();
	}

	LoadMap(mapIndex);

	PlayerX = Map.StartPt.X;
	PlayerY = Map.StartPt.Y;

	GameLastPlayedStageIndex = Map.Index;

	SetCurtain();
	FreezeInput();

	yield* @@_StartMotion();

	Play(M_Field);

gameLoop:
	for (; ; )
	{
		if (GetInput_Pause() == 1) // ポーズ
		{
			yield* @@_PauseMenu();
		}
		if (GameRequestReturnToTitleMenu)
		{
			GameEndReason = GameEndReason_e_RETURN_MENU;
			break;
		}
		if (@@_RequestRestart)
		{
			yield* @@_DeadAndRestartMotion(true);

			continue gameLoop;
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
			var<Enemy_t> enemy = @@_Enemies[index];

			if (enemy.HP == -1) // ? 既に死亡
			{
				continue;
			}

			enemy.Crash = null; // reset

			if (!DrawEnemy(enemy))
			{
				enemy.HP = -1;
			}
		}

		// 自弾の描画
		//
		for (var<int> index = 0; index < @@_Shots.length; index++)
		{
			var<Shot_t> shot = @@_Shots[index];

			if (shot.AttackPoint == -1) // ? 既に死亡
			{
				continue;
			}

			shot.Crash = null; // reset

			if (!DrawShot(shot))
			{
				shot.AttackPoint = -1;
			}
		}

		@@_DrawFront();

		if (DEBUG && 1 <= GetKeyInput(17)) // ? コントロール押下中 -> 当たり判定表示 (デバッグ用)
		{
			SetColor("#000000a0");
			PrintRect(0, 0, Screen_W, Screen_H);
			SetColor("#00ff0030");
			PrintRect_LTRB(
				PlayerX - PLAYER_側面判定Pt_X,
				PlayerY - PLAYER_側面判定Pt_YT,
				PlayerX + PLAYER_側面判定Pt_X,
				PlayerY + PLAYER_側面判定Pt_YB,
				);
			PrintRect_LTRB(
				PlayerX - PLAYER_脳天判定Pt_X,
				PlayerY - PLAYER_脳天判定Pt_Y,
				PlayerX + PLAYER_脳天判定Pt_X,
				PlayerY
				);
			PrintRect_LTRB(
				PlayerX - PLAYER_接地判定Pt_X,
				PlayerY,
				PlayerX + PLAYER_接地判定Pt_X,
				PlayerY + PLAYER_接地判定Pt_Y
				);
			SetColor("#ff0000a0");
			DrawCrash(PlayerCrash);
			SetColor("#ffffffa0");

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

					EnemyDamaged(enemy, damagePoint);

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
				if (enemy.Kind == EnemyKind_Goal) // ? ゴール到達 -> 次のステージへ
				{
					yield* @@_GoalMotion();

					break gameLoop;
				}
				else
				{
					yield* @@_DeadAndRestartMotion(false);

					continue gameLoop;
				}
			}
		}

		// ====
		// 当たり判定ここまで
		// ====

		f_ゴミ回収();

		RemoveAll(@@_Enemies, function <boolean> (<Enemy_t> enemy)
		{
			return enemy.HP == -1; // ? 死亡
		});

		RemoveAll(@@_Shots, function <boolean> (<Shot_t> shot)
		{
			return shot.AttackPoint == -1; // ? 死亡
		});

		yield 1;

		// ★★★ ゲームループの終わり ★★★
	}

	SetCurtain_FD(30, -1.0);
//	Fadeout();

	for (var<Scene_t> scene of CreateScene(40))
	{
		@@_DrawWall();
		@@_DrawFront();

		yield 1;
	}

	ClearAllEffect(); // 時限消滅ではないエフェクトを考慮して、クリアは必須とする。
	FreezeInput();

	// ★★★ end of GameMain() ★★★
}

function* <generatorForTask> @@_T_ゴミ回収()
{
	for (; ; )
	{
		for (var<int> index = 0; index < @@_Enemies.length; index++)
		{
			var<Enemy_t> enemy = @@_Enemies[index];

			if (@@_IsProbablyEvacuated(enemy.X, enemy.Y))
			{
				enemy.HP = -1;
			}

			yield 1;
		}

		for (var<int> index = 0; index < @@_Shots.length; index++)
		{
			var<Shot_t> shot = @@_Shots[index];

			if (@@_IsProbablyEvacuated(shot.X, shot.Y))
			{
				shot.AttackPoint = -1;
			}

			yield 1;
		}

		yield 1; // @@_Enemies, @@_Shots が空の場合、ループ内の yield は実行されないので、ここにも yield を設置しておく。
	}
}

function <boolean> @@_IsProbablyEvacuated(<double> x, <double> y)
{
	var<int> MGN_SCREEN_NUM = 3;

	var<boolean> ret = IsOut(
		CreateD2Point(x, y),
		CreateD4Rect_LTRB(
			-Screen_W * MGN_SCREEN_NUM,
			-Screen_H * MGN_SCREEN_NUM,
			Screen_W * (MGN_SCREEN_NUM + 1),
			Screen_H * (MGN_SCREEN_NUM + 1)
			),
		0.0
		);

	return ret;
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

	for (var<int> x = 0; x < MAP_W; x++)
	for (var<int> y = 0; y < MAP_H; y++)
	{
		var<double> dx = FIELD_L + x * TILE_W + TILE_W / 2;
		var<double> dy = FIELD_T + y * TILE_H + TILE_H / 2;

		if (Map.Table[x][y].WallFlag)
		{
			Draw(P_Wall, dx, dy, 1.0, 0.0, 1.0);

			if (IsMapCellType_EnemyGreen(Map.Table[x][y].Type)) // 敵緑の中心
			{
				SetColor("#00ff0080");
				PrintRect_XYWH(dx, dy, 20.0, 20.0);
			}
		}
		else
		{
			Draw(P_背景, dx, dy, 1.0, 0.0, 1.0);
		}

		/*
		if (Map.Table[x][y].NarrowFlag) // test
		{
			SetColor("#ff00ff80");
			PrintCircle(dx, dy, 20.0);
		}
		*/
	}
}

/*
	前面描画
*/
function <void> @@_DrawFront()
{
	SetColor(I3ColorToString(CreateI3Color(40, 40, 20)));
	PrintRect(
		0,
		0,
		FIELD_L,
		Screen_H
		);
	PrintRect(
		FIELD_R,
		0,
		Screen_W - FIELD_R,
		Screen_H
		);
	SetColor(I3ColorToString(CreateI3Color(80, 80, 40)));
	PrintRect(
		0,
		0,
		Screen_W,
		FIELD_T
		);
	PrintRect(
		0,
		FIELD_B,
		Screen_W,
		Screen_H - FIELD_B
		);

	SetColor("#ffffff");
	SetPrint(20, 80, 0);
	SetFSize(80);
	PrintLine("STAGE " + Map.Index);
}

/*
	ゲーム開始モーション
*/
function* <generatorForTask> @@_StartMotion()
{
	SE(S_Start);

	for (var<Scene_t> scene of CreateScene(40))
	{
		@@_DrawWall();

		Draw(
			P_Player,
			PlayerX,
			PlayerY,
			0.5 + 0.5 * scene.Rate,
			10.0 * scene.RemRate * scene.RemRate,
			1.0 + 29.0 * scene.RemRate
			);

		@@_DrawFront();

		yield 1;
	}
}

/*
	死亡＆再スタート・モーション

	restartRequested: ? 再スタートの要求により実行された
*/
function* <generatorForTask> @@_DeadAndRestartMotion(<boolean> restartRequested)
{
	if (!restartRequested)
	{
		// 赤カーテン
		SetColor("#ff000040");
		PrintRect(0, 0, Screen_W, Screen_H);

		SE(S_Crashed);

		for (var<Scene_t> scene of CreateScene(30))
		{
			yield 1;
		}

		AddEffect_Explode(PlayerX, PlayerY);
		SE(S_Dead);

		for (var<Scene_t> scene of CreateScene(30))
		{
			@@_DrawWall();

			yield 1;
		}
	}

	// リスタートのための処理
	{
		@@_Enemies = [];
		@@_Shots = [];

		GameRequestReturnToTitleMenu = false;
		@@_RequestRestart = false;

		ResetPlayer();

		LoadEnemyOfMap();

		PlayerX = Map.StartPt.X;
		PlayerY = Map.StartPt.Y;
	}

	yield* @@_StartMotion();
}

/*
	ゲーム終了モーション
	-- ゴールした。
*/
function* <generatorForTask> @@_GoalMotion()
{
	for (var<Scene_t> scene of CreateScene(30))
	{
		yield 1;
	}

	SE(S_Goal);

	for (var<Scene_t> scene of CreateScene(40))
	{
		@@_DrawWall();

		Draw(
			P_Player,
			PlayerX,
			PlayerY,
			0.5 + 0.5 * scene.RemRate,
			10.0 * scene.Rate * scene.Rate,
			1.0 + 29.0 * scene.Rate
			);

		@@_DrawFront();

		yield 1;
	}
}

/*
	ポーズ画面
*/
function* <generatorForTask> @@_PauseMenu()
{
	var<int> selectIndex = 0;

	FreezeInput();

gameLoop:
	for (; ; )
	{
		@@_DrawWall();

		selectIndex = DrawSimpleMenu_CPNP(
			selectIndex,
			0,
			220,
			760,
			70,
			true,
			true,
			[
				"再スタート",
				"タイトルに戻る",
				"ゲームに戻る",
			]);

		if (DSM_Desided)
		switch (selectIndex)
		{
		case 0:
			@@_RequestRestart = true;
			break gameLoop;

		case 1:
			GameRequestReturnToTitleMenu = true;
			break gameLoop;

		case 2:
			break gameLoop;

		default:
			error(); // never
		}
		yield 1;
	}
	FreezeInput();
	FreezeInputUntilRelease();
}
