/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

// カメラ位置
// -- (0, 0) 固定
var<D2Point_t> Camera = CreateD2Point(0.0, 0.0);

// ゲーム用タスク
var<TaskManager_t> GameTasks = CreateTaskManager();

// 前面タスク
var<TaskManager_t> FrontTasks = CreateTaskManager_CEFM(true);

// シナリオ・タスク
var<generatorForTask> @@_ScenarioTask = null;

// 背景タスク
var<generatorForTask> @@_BackgroundTask = null;

// 当たり判定を表示するフラグ (デバッグ・テスト用)
var<boolean> @@_PrintAtariFlag = false;

/*
	ゲーム終了理由
*/
var<GameEndReason_e> GameEndReason = GameEndReason_e_RETURN_MENU;

/*
	ゲーム再スタート・リクエスト
*/
var<boolean> GameRequestRestartGame = false;

/*
	ゲーム終了リクエスト(タイトルへ戻る)
*/
var<boolean> GameRequestReturnToTitleMenu = false;

/*
	スコア
*/
var<int> Score = 0;

function* <generatorForTask> GameMain()
{
	// reset
	{
		@@_Enemies = [];
		@@_Shots = [];

		ClearAllTask(GameTasks);
		ClearAllTask(FrontTasks);
		@@_ScenarioTask = null;
		@@_BackgroundTask = null;
		GameEndReason = GameEndReason_e_RETURN_MENU;
		GameRequestRestartGame = false;
		GameRequestReturnToTitleMenu = false;

		ResetPlayer();

		Score = 0;
		BackgroundPhase = 0;
	}

//	var<Func boolean> f_ゴミ回収 = Supplier(@@_T_ゴミ回収()); // メソッド版_廃止
	AddTask(GameTasks, @@_T_ゴミ回収());

	SetCurtain_FD(0, -1.0);
	SetCurtain();
	FreezeInput();

	@@_ScenarioTask   = CreateScenarioTask();
	@@_BackgroundTask = CreateBackgroundTask();

gameLoop:
	for (; ; )
	{
		if (GetInput_Pause() == 1) // ポーズ
		{
			yield* @@_PauseMenu();
		}
		if (GameRequestRestartGame)
		{
			GameEndReason = GameEndReason_e_RESTART_GAME;
			break;
		}
		if (GameRequestReturnToTitleMenu)
		{
			GameEndReason = GameEndReason_e_RETURN_MENU;
			break;
		}

		if (!NextVal(@@_ScenarioTask)) // ? シナリオ終了 -> ゲーム終了
		{
			GameEndReason = GameEndReason_e_GAME_CLEAR;
			break;
		}

		// ====
		// 描画ここから
		// ====

		@@_DrawWall();

		DrawPlayer(); // プレイヤーの行動と描画

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

		ExecuteAllTask(GameTasks);
		ExecuteAllTask(FrontTasks);
		@@_DrawFront();

		if (DEBUG && GetKeyInput(17) == 1) // ? コントロール押下中 -> 当たり判定表示 (デバッグ用)
		{
			@@_PrintAtariFlag = !@@_PrintAtariFlag;
		}
		if (@@_PrintAtariFlag)
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
				if (enemy.HP == 0) // ? 無敵
				{
					continue;
				}

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
						KillEnemy_Destroyed(enemy, true);
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
				yield* @@_PlayerDead();

				if (PlayerZankiNum < 1) // ? 残機ゼロ
				{
					GameEndReason = GameEndReason_e_GAME_OVER;
					break gameLoop;
				}

				PlayerZankiNum--;
				PlayerRebornFrame = 1;
				break; // 被弾したので当たり判定終了
			}
		}

		// ====
		// 当たり判定ここまで
		// ====

//		f_ゴミ回収(); // メソッド版_廃止

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

	SaveLocalStorage(); // ハイスコア更新

	if (GameEndReason == GameEndReason_e_GAME_OVER)
	{
		Fadeout_F(90);

		for (var<Scene_t> scene of CreateScene(80))
		{
			@@_DrawWall();
			@@_DrawFront();

			yield 1;
		}

		SetCurtain_FD(30, -1.0);

		for (var<Scene_t> scene of CreateScene(40))
		{
			@@_DrawWall();
			@@_DrawFront();

			yield 1;
		}
	}
	else
	{
		SetCurtain_FD(30, -1.0);

		for (var<Scene_t> scene of CreateScene(40))
		{
			@@_DrawWall();
			@@_DrawFront();

			yield 1;
		}
	}

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
	NextRun(@@_BackgroundTask);
}

/*
	前面描画
*/
function <void> @@_DrawFront()
{
	var<string> strPower;

	switch (PlayerAttackLv)
	{
	case 1: strPower = "■□□"; break;
	case 2: strPower = "■■□"; break;
	case 3: strPower = "■■■"; break;

	default:
		error();
	}

	SetColor(I3ColorToString(CreateI3Color(255, 255, 255)));
	SetPrint(10, 25, 0);
	SetFSize(16);
	PrintLine("Zanki: " + PlayerZankiNum + "　Power: " + strPower + "　Score: " + Score);
}

/*
	プレイヤーの被弾モーション
*/
function* <generatorForTask> @@_PlayerDead()
{
	ClearAllEffect();

	SetColor("#ff000040");
	PrintRect(0, 0, Screen_W, Screen_H);

	for (var<Scene_t> scene of CreateScene(20))
	{
		yield 1;
	}

	// 敵クリア
	//
	for (var<Enemy_t> enemy of @@_Enemies)
	{
		// アイテムは除外
		if (IsEnemyItem(enemy))
		{
			// noop
		}
		// ボスクラスの敵も除外
		else if (IsEnemyBoss(enemy))
		{
			// noop
		}
		else
		{
			KillEnemy(enemy);
		}
	}

	// 自弾クリア
	//
	for (var<Shot_t> shot of @@_Shots)
	{
		KillShot(shot);
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
			50,
			260,
			600,
			50,
			true,
			true,
			[
				"最初からやり直す",
				"タイトルに戻る",
				"ゲームに戻る",
			]);

		if (DSM_Desided)
		switch (selectIndex)
		{
		case 0:
			GameRequestRestartGame = true;
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
