/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

/*
	スコア
*/
var<int> Score = 0;

function* <generatorForTask> GameMain()
{
	var<Func boolean> f_ゴミ回収 = Supplier(@@_T_ゴミ回収());

	// reset
	{
		@@_Enemies = [];
		@@_Shots = [];

		PlayerX = FIELD_L + FIELD_W / 2;
		PlayerY = FIELD_T + FIELD_H / 2;
		PlayerBornFrame = 0;
		PlayerInvincibleFrame = 0;
		PlayerAttackLv = 1;
		PlayerZankiNum = 3;

		Score = 0;
		BackgroundPhase = 0;
	}

	SetCurtain_FD(0, -1.0);
	SetCurtain();
	FreezeInput();

	var<Func boolean> f_scenarioTask   = Supplier(ScenarioTask());
	var<Func boolean> f_backgroundTask = Supplier(BackgroundTask());

gameLoop:
	for (; ; )
	{
		if (!f_scenarioTask())
		{
			break;
		}

		// ====
		// 描画ここから
		// ====

		@@_DrawWall();

		// 背景の描画
		//
		if (!f_backgroundTask())
		{
			error();
		}

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
				if (enemy.HP == 0) // ? 無敵 -> 自弾とは衝突しない。
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
				if (enemy.Kind == EnemyKind_Item) // ? アイテム -> 取得
				{
					var<EnemyItemType_e> itemType = GetEnemyItemType(enemy);

					switch (itemType)
					{
					case EnemyItemType_e_POWER_UP:
						PlayerAttackLv = Math.min(PlayerAttackLv + 1, PLAYER_ATTACK_LV_MAX);
						break;

					case EnemyItemType_e_ZANKI_UP:
						PlayerZankiNum++;
						break;

					default:
						error();
					}

					KillEnemy(enemy);
					break; // この敵(アイテム)は死亡したので、次の敵へ進む。
				}
				else if (
					1 <= PlayerBornFrame || // ? 再登場中
					1 <= PlayerInvincibleFrame // ? 無敵状態中
					)
				{
					// 被弾しない。
				}
				else // ? それ以外(敵) -> 被弾
				{
					yield* @@_PlayerDead();

					if (PlayerZankiNum < 1) // ? 残機ゼロ
					{
						break gameLoop;
					}

					PlayerZankiNum--;
					PlayerBornFrame = 1;
					break; // 被弾したので当たり判定終了
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

	Fadeout_F(90);

	for (var<Scene_t> scene of CreateScene(80)) // 余韻のような...
	{
		@@_DrawWall();

		// 背景の描画
		//
		if (!f_backgroundTask())
		{
			error();
		}

		@@_DrawFront();

		yield 1;
	}

	SetCurtain_FD(30, -1.0);

	for (var<Scene_t> scene of CreateScene(40))
	{
		@@_DrawWall();

		// 背景の描画
		//
		if (!f_backgroundTask())
		{
			error();
		}

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
}

/*
	前面描画
*/
function <void> @@_DrawFront()
{
	SetColor(I3ColorToString(CreateI3Color(50, 100, 150)));
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
	var<generatorForTask[]> bk_effects = EjectEffects(); // エフェクト全て除去

	SetColor("#ff000040");
	PrintRect(0, 0, Screen_W, Screen_H);

	for (var<Scene_t> scene of CreateScene(20))
	{
		yield 1;
	}

	SetEffects(bk_effects); // エフェクト復元

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
