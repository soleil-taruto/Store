/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

/*
	次以降の自弾リスト
	----
	長さ：SUBSEQUENT_BALL_MAX
	[0] == 次
	[SUBSEQUENT_BALL_MAX - 1] == 最後
*/
var<BallColor_e[]> SubsequentBallColors;

/*
	射出チャージ期間
	----
	0 == 無効
	1～ == チャージ期間中
*/
var<int> ChargeFrame = 0;

function* <generatorForTask> GameMain()
{
	// init SubsequentBallColors
	{
		SubsequentBallColors = [];

		for (var<int> c = 0; c < SUBSEQUENT_BALL_MAX; c++)
		{
			SubsequentBallColors.push(GetRand(P_Balls.length));
		}
	}

	// 初期ブロック配置
	{
		for (var<int> y = 0; y < 10; y++)
		{
			for (var<int> x = 0; ; x++)
			{
				var<double> px = 17 + 15 * (y % 2) + 30 * x;
				var<double> py = 15 + 25 * y;

				if (FIELD_R - 15 < px)
				{
					break;
				}
				@@_Enemies.push(CreateEnemy_Ball(px, py, 1, GetRand(P_Balls.length)));
			}
		}
	}

	BubbleRelation_孤立したブロックを除去_Start();

	for (; ; )
	{
		@@_DrawWall();

		var<double> mx = GetMouseX();
		var<double> my = GetMouseY();

		if (ChargeFrame == 0) // ? 射出可能
		{
			var<double> sx = FIELD_W / 2;
			var<double> sy = FIELD_H;

			var<double> shootRad = GetAngle(mx - sx, my - sy);

			if (shootRad < Math.PI / 2)
			{
				shootRad += Math.PI * 2;
			}
			var<double> RAD_LMT = 0.4;

			shootRad = ToRange(shootRad, Math.PI + RAD_LMT, Math.PI * 2 - RAD_LMT);

			for (var<int> c = 1; c <= 5; c++)
			{
				var<D2Point_t> pt = AngleToPoint(shootRad, c * 32.0);

				pt.X += sx;
				pt.Y += sy;

				Draw(P_軌道, pt.X, pt.Y, 0.5, 0.0, 1.0);
			}

			Draw(P_Balls[SubsequentBallColors[0]], sx, sy, 0.5, 0.0, 1.0); // 描画：射出位置

			if (GetMouseDown() == -1) // 射出可能 ⇒ チャージ中
			{
				var<D2Point_t> speed = AngleToPoint(shootRad, BALL_SPEED);

				ChargeFrame = 1;
				@@_Shots.push(CreateShot_Ball(sx, sy, speed.X, speed.Y, SubsequentBallColors[0]));
				SubsequentBallColors.shift();
				SubsequentBallColors.push(GetRand(P_Balls.length));
				@@_Subsequent_ShiftXRate = 1.0;
			}
		}
		else // ? チャージ中
		{
			var<boolean> ended = false; // ? チャージ終了

			if (ChargeFrame == CHARGE_FRAME_MAX)
			{
				ended = true;
			}

			if (ended) // チャージ中 ⇒ 射出可能
			{
				ChargeFrame = 0;
			}
			else // まだチャージ中
			{
				ChargeFrame++;
			}
		}

		// ====
		// 描画ここから
		// ====

		// memo: 死亡しているかチェックするのは「当たり判定」から

		// 敵の描画
		//
		for (var<int> index = 0; index < @@_Enemies.length; index++)
		{
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
			@@_Shots[index].Crash = null; // reset

			if (!DrawShot(@@_Shots[index]))
			{
				@@_Shots[index].AttackPoint = -1;
			}
		}

		@@_DrawFront();

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

				if (IsCrashed(enemy.Crash, shot.Crash)) // ? 衝突している。
				{
					var<D2Point_t> pt = MakeXYSpeed(enemy.X, enemy.Y, shot.X, shot.Y, 30.0);

					var<double> x = enemy.X + pt.X;
					var<double> y = enemy.Y + pt.Y;

					@@_Enemies.push(CreateEnemy_Ball(x, y, 1, shot.Color));

					KillShot(shot);

					BubbleRelation_着弾による爆発(@@_Enemies, @@_Enemies[@@_Enemies.length - 1]);
				}
			}
		}

		for (var<int> index = 0; index < @@_Shots.length; index++) // 側面・上部反射
		{
			var<Shot_t> shot = @@_Shots[index];

			if (shot.AttackPoint == -1) // ? 既に死亡
			{
				continue;
			}

			if (shot.X < FIELD_L)
			{
				shot.XAdd = Math.abs(shot.XAdd);
			}
			if (FIELD_R < shot.X)
			{
				shot.XAdd = Math.abs(shot.XAdd) * -1;
			}
			if (shot.Y < FIELD_T)
			{
				var<double> x = shot.X;
				var<double> y = 15.0;

				x = ToRange(x, FIELD_L + 15.0, FIELD_R - 15.0);

				@@_Enemies.push(CreateEnemy_Ball(x, y, 1, shot.Color));

				KillShot(shot);

				BubbleRelation_着弾による爆発(@@_Enemies, @@_Enemies[@@_Enemies.length - 1]);
			}
			if (FIELD_B < shot.Y)
			{
				shot.AttackPoint = -1;
			}
		}

		// 下すぎるブロックを消去する。
		//
		for (var<Enemy_t> enemy of @@_Enemies)
		{
			if (FIELD_B - 100.0 < enemy.Y) // ? 下すぎる。
			{
				KillEnemy(enemy);
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
}

function <void> @@_DrawWall()
{
	SetColor(I3ColorToString(CreateI3Color(0, 0, 0)));
	PrintRect(0, 0, Screen_W, Screen_H);
}

var<double> @@_Subsequent_ShiftXRate = 0.0; // 0.0 ～ 1.0

function <void> @@_DrawFront()
{
	SetColor(I3ColorToString(CreateI3Color(80, 60, 40)));
	PrintRect(0, Screen_H - 30, Screen_W, 30);

	for (var<int> c = SubsequentBallColors.length - 1; 0 <= c; c--)
	{
		Draw(
			P_Balls[SubsequentBallColors[c]],
			15 + (Screen_W - 30) / (SubsequentBallColors.length - 1) * (c + @@_Subsequent_ShiftXRate),
			Screen_H - 15,
			1.0,
			0.0,
			1.0
			);
	}

	@@_Subsequent_ShiftXRate = Approach(@@_Subsequent_ShiftXRate, 0.0, 0.9);
}

function <Enemy_t[]> GetEnemies()
{
	return @@_Enemies;
}
