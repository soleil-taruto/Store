/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

// カメラ位置(実数)
var<D2Point_t> @@_Camera = CreateD2Point(0.0, 0.0);

// カメラ位置(整数)
var<D2Point_t> Camera = CreateD2Point(0.0, 0.0);

// ゲーム用タスク
var<TaskManager_t> GameTasks = CreateTaskManager();

// 前面タスク
var<TaskManager_t> FrontTasks = CreateTaskManager_CEFM(true);

// プレイヤー描画タスク
var<TaskManager_t> PlayerDrawTasks = CreateTaskManager_CEFM(true);

// 当たり判定を表示するフラグ (デバッグ・テスト用)
var<boolean> @@_PrintAtariFlag = false;

/*
	ゲーム終了理由
*/
var<GameEndReason_e> GameEndReason = GameEndReason_e_STAGE_CLEAR;

/*
	ユーザーによるプレイヤー操作の抑止
*/
var<boolean> UserInputDisabled = false;

/*
	ゲーム終了リクエスト(タイトルへ戻る)
*/
var<boolean> GameRequestReturnToTitleMenu = false;

/*
	ゲーム終了リクエスト(ステージクリア)
*/
var<boolean> GameRequestStageClear = false;

var<int> @@_MapIndex = -1;
var<boolean> @@_RequestRestart = false;
var<Wall_t> @@_Wall = null;

function* <generatorForTask> GameMain(<int> mapIndex)
{
	// reset
	{
		@@_Enemies = [];
		@@_Shots = [];

		@@_Camera = CreateD2Point(0.0, 0.0);
		Camera    = CreateD2Point(0.0, 0.0);
		ClearAllTask(GameTasks);
		ClearAllTask(FrontTasks);
		ClearAllTask(PlayerDrawTasks);
		GameEndReason = GameEndReason_e_STAGE_CLEAR;
		GameRequestReturnToTitleMenu = false;
		GameRequestStageClear = false;

		@@_MapIndex = mapIndex;
		@@_RequestRestart = false;

		ResetPlayer();
	}

//	var<Func boolean> f_ゴミ回収 = Supplier(@@_T_ゴミ回収()); // メソッド版_廃止
	AddTask(GameTasks, @@_T_ゴミ回収());

	LoadMap(mapIndex);
	LoadEnemyOfMap();
	MoveToStartPtOfMap();
	@@_Wall = GetStageWall(mapIndex);

	SetCurtain();
	FreezeInput();

	@@_カメラ位置調整(true);

	yield* @@_StartMotion();

	PlayStageMusic(mapIndex);

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
		if (GameRequestStageClear)
		{
			GameEndReason = GameEndReason_e_STAGE_CLEAR;
			break;
		}
		if (@@_RequestRestart)
		{
			yield* @@_DeadAndRestartMotion(true);

			continue gameLoop;
		}

	movePlayerBlock:
		{
			if (1 <= PlayerDamageFrame) // 被弾したら即終了
			{
				PlayerAttack = null;
			}
			if (PlayerAttack != null)
			{
				if (PlayerAttack())
				{
					break movePlayerBlock;
				}
				PlayerAttack = null;
			}
			ActPlayer();
		}

		@@_カメラ位置調整(false);

		// ====
		// 描画ここから
		// ====

		@@_DrawWall();
		@@_DrawMap();

		if (1 <= GetTaskCount(PlayerDrawTasks))
		{
			ExecuteAllTask(PlayerDrawTasks);
		}
		else
		{
			DrawPlayer();
		}

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
			var<double> dPlX = PlayerX - Camera.X;
			var<double> dPlY = PlayerY - Camera.Y;

			SetColor("#000000a0");
			PrintRect(0, 0, Screen_W, Screen_H);
			SetColor("#00ff0030");
			PrintRect_LTRB(
				dPlX - PLAYER_側面判定Pt_X,
				dPlY - PLAYER_側面判定Pt_YT,
				dPlX + PLAYER_側面判定Pt_X,
				dPlY + PLAYER_側面判定Pt_YB
				);
			PrintRect_LTRB(
				dPlX - PLAYER_脳天判定Pt_X,
				dPlY - PLAYER_脳天判定Pt_Y,
				dPlX + PLAYER_脳天判定Pt_X,
				dPlY
				);
			PrintRect_LTRB(
				dPlX - PLAYER_接地判定Pt_X,
				dPlY,
				dPlX + PLAYER_接地判定Pt_X,
				dPlY + PLAYER_接地判定Pt_Y
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
				PlayerHP -= enemy.AttackPoint;

				if (1 <= PlayerHP) // ? プレイヤー生存
				{
					PlayerDamageFrame = 1;
				}
				else // ? プレイヤー死亡
				{
					PlayerHP = -1;

					yield* @@_DeadAndRestartMotion(false);

					continue gameLoop;
				}

				if (enemy.HitDie)
				{
					KillEnemy(enemy);
				}
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

	if (GameEndReason == GameEndReason_e_STAGE_CLEAR)
	{
		ClearAllEffect();

		yield* @@_GoalMotion();

		ClearAllEffect();
	}
	else if (GameEndReason == GameEndReason_e_RETURN_MENU)
	{
		// noop
	}
	else
	{
		error(); // never -- 不明な GameEndReason
	}

	SetCurtain_FD(30, -1.0);
//	Fadeout();

	for (var<Scene_t> scene of CreateScene(40))
	{
		@@_DrawWall();
		@@_DrawMap();
//		@@_DrawFront();

		yield 1;
	}

	ClearAllEffect(); // 時限消滅ではないエフェクトを考慮して、クリアは必須とする。
	FreezeInput();

	// ★★★ end of GameMain() ★★★
}

function <void> @@_カメラ位置調整(<boolean> 一瞬で)
{
	var<double> targCamX = PlayerX - Screen_W / 2;
	var<double> targCamY = PlayerY - Screen_H / 2;

	targCamX = ToRange(targCamX, 0.0, TILE_W * Map.W - Screen_W);
	targCamY = ToRange(targCamY, 0.0, TILE_H * Map.H - Screen_H);

	@@_Camera.X = Approach(@@_Camera.X, targCamX, 一瞬で ? 0.0 : 0.8);
	@@_Camera.Y = Approach(@@_Camera.Y, targCamY, 一瞬で ? 0.0 : 0.8);

	Camera = I2PointToD2Point(D2PointToI2Point(@@_Camera));
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
			TILE_W * Map.W + Screen_W * MGN_SCREEN_NUM,
			TILE_H * Map.H + Screen_H * MGN_SCREEN_NUM
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
	DrawWall(@@_Wall);
}

/*
	マップ描画
*/
function <void> @@_DrawMap()
{
	var<I2Point_t> lt = ToTablePoint(Camera);
	var<I2Point_t> rb = ToTablePoint_XY(Camera.X + Screen_W, Camera.Y + Screen_H);
	var<int> l = lt.X;
	var<int> t = lt.Y;
	var<int> r = rb.X;
	var<int> b = rb.Y;

	// マージン付与
	{
		var<int> MARGIN = 2; // マージン・セル数

		l -= MARGIN;
		t -= MARGIN;
		r += MARGIN;
		b += MARGIN;
	}

	for (var<int> x = l; x <= r; x++)
	for (var<int> y = t; y <= b; y++)
	{
		var<MapCell_t> cell = GetMapCell_XY(x, y);
		var<Tile_t> tile = cell.Tile;

		var<D2Point> dPt = ToFieldPoint_XY(x, y);
		var<double> dx = dPt.X;
		var<double> dy = dPt.Y;

		DrawTile(tile, dx - Camera.X, dy - Camera.Y);
	}
}

/*
	前面描画
*/
function <void> @@_DrawFront()
{
	var<int> HP_METER_L = 20;
	var<int> HP_METER_T = 20;
	var<int> HP_METER_W = 20;
	var<int> HP_METER_H = 100;

	var<int> h = ToInt((HP_METER_H * PlayerHP) / PLAYER_HP_MAX);
	var<int> d = HP_METER_H - h;

	if (1 <= d)
	{
		SetColor("#808080a0");
		PrintRect(HP_METER_L, HP_METER_T, HP_METER_W, d);
	}
	if (1 <= h)
	{
		SetColor("#ffff00a0");
		PrintRect(HP_METER_L, HP_METER_T + d, HP_METER_W, h);
	}
}

/*
	ゲーム開始モーション
*/
function* <generatorForTask> @@_StartMotion()
{
	for (var<Scene_t> scene of CreateScene(60))
	{
		@@_カメラ位置調整(false);

		@@_DrawWall();
		@@_DrawMap();

		for (var<int> c = 0; c < 4; c++)
		{
			var<double> dx = PlayerX - Camera.X;
			var<double> dy = PlayerY - Camera.Y;

			dy -= 14; // 接地したとき上に押し出される距離

			var<D2Point_t> pt = AngleToPoint(
				scene.RemRate * scene.RemRate * 10.0 + (Math.PI / 2.0) * c,
				scene.RemRate * scene.RemRate * 500.0
				);

			dx += pt.X;
			dy += pt.Y;

			Draw(P_PlayerStand, dx, dy, 0.5, 0.0, 1.0 + scene.RemRate * 2.0);
		}

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

		yield* Wait(30);

		AddEffect(Effect_Explode_L(PlayerX, PlayerY));
		SE(S_Dead);

		for (var<Scene_t> scene of CreateScene(30))
		{
			@@_DrawWall();
			@@_DrawMap();

			yield 1;
		}
	}

	// 再スタートのための処理
	{
		// 敵をクリアする前にちゃんと殺しておく。
		// -- 敵の死亡をモニタして終了(自滅)するタスクのため。
		@@_Enemies.forEach(enemy => enemy.HP = -1);

		@@_Enemies = [];
		@@_Shots = [];
		@@_RequestRestart = false;

		ResetPlayer();

//		ClearAllTask(GameTasks); // dont
		ClearAllTask(FrontTasks);
		ClearAllTask(PlayerDrawTasks);

		LoadEnemyOfMap();
		MoveToStartPtOfMap();
	}

	yield* @@_StartMotion();
}

/*
	ゲーム終了モーション
	-- ゴールした。
*/
function* <generatorForTask> @@_GoalMotion()
{
	yield* Wait(30);

	SE(S_Clear);

	for (var<int> c = 0; c < 50; c++)
	{
		AddEffect(function* <generatorForTask> ()
		{
			var<double> x = PlayerX;
			var<double> y = PlayerY;
			var<D2Point_t> speed = AngleToPoint(Math.PI * 2.0 * GetRand1(), GetRand3(5.0, 15.0));

			var<double> r = Math.PI * 2.0 * GetRand1();
			var<double> rAdd = 0.3 * GetRand2();

			for (; ; )
			{
				x += speed.X;
				y += speed.Y;
				r += rAdd;

				if (IsOutOfCamera(CreateD2Point(x, y), 50.0))
				{
					break;
				}

				Draw(P_PlayerStand, x - Camera.X, y - Camera.Y, 0.7, r, 2.0);

				yield 1;
			}
		}());
	}

	for (var<Scene_t> scene of CreateScene(60))
	{
		@@_DrawWall();
		@@_DrawMap();

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
		@@_DrawMap();

		selectIndex = DrawSimpleMenu_CPNP(
			selectIndex,
			50,
			160,
			600,
			50,
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
