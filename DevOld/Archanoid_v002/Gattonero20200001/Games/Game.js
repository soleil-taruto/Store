/*
	ゲーム・メイン
*/

// 敵リスト
var<Enemy_t[]> @@_Enemies = [];

// 自弾リスト
var<Shot_t[]> @@_Shots = [];

/*
	射出可能ボール数
*/
var<int> @@_BallStockNum = 10;

/*
	射出中フレーム
	0 == 無効
	1～ == 射出中
*/
var<boolean> @@_ShootingFrame = 0;

/*
	射出角度
*/
var<double> @@_ShootingAngle;

/*
	射出する X-位置
*/
var<double> @@_Shooter_X = Screen_W / 2;

/*
	帰ってきたボールの X-位置
	null == 無効
*/
var<double> @@_Return_X = null;

function* <generatorForTask> GameMain()
{
	for (; ; )
	{
		@@_DrawWall();

		var<double> mx = GetMouseX();
		var<double> my = GetMouseY();

		if (@@_ShootingFrame == 0) // ? 射出中ではない -> 射出方向を描画する。射出可能
		{
			var<double> sx = @@_Shooter_X;
			var<double> sy = Screen_H;

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

				Draw(P_弾道, pt.X, pt.Y, 0.5, 0.0, 1.0);
			}

			Draw(P_Ball, @@_Shooter_X, Screen_H, 0.5, 0.0, 1.0); // 描画：射出位置

			if (GetMouseDown() == -1) // 射出可能 ⇒ 射出中
			{
				@@_ShootingFrame = 1;
				@@_ShootingAngle = shootRad;
				@@_Return_X = null;
			}
		}
		else // ? 射出中 -- 射出不可
		{
			var<int> SHOOT_PER_FRAME = 10;
			var<boolean> completed = false;

			@@_ShootingFrame = Math.max(@@_ShootingFrame, SHOOT_PER_FRAME); // フレーム・カウント調整

			if (@@_ShootingFrame / SHOOT_PER_FRAME <= @@_BallStockNum) // ? 射出未完了
			{
				if (@@_ShootingFrame % SHOOT_PER_FRAME == 0)
				{
					var<double> x = @@_Shooter_X;
					var<double> y = Screen_H;

					var<D2Point_t> speed = AngleToPoint(@@_ShootingAngle, BALL_SPEED);

					var<double> xAdd = speed.X;
					var<double> yAdd = speed.Y;

					@@_Shots.push(CreateShot_Ball(x, y, xAdd, yAdd));
				}

				Draw(P_Ball, @@_Shooter_X, Screen_H, 0.5, 0.0, 1.0); // 描画：射出位置
			}
			else // ? 射出完了
			{
				if (@@_Shots.length == 0)
				{
					completed = true;
				}
			}

			if (@@_Return_X != null)
			{
				Draw(P_Ball, @@_Return_X, Screen_H, 0.5, 0.0, 1.0); // 描画：次回射出位置
			}

			if (completed) // 射出中 ⇒ 射出可能
			{
				@@_ShootingFrame = 0;
				@@_Shooter_X = @@_Return_X;

				for (var<int> c = 0; c < 7; c++) // 追加
				{
					var<double> x = 30.0 + c * 60.0;
					var<double> y = -30.0;

					var<Enemy_t> enemy;

					switch (GetRand(9))
					{
					case 0: enemy = CreateEnemy_CircleBlock(x, y, 10, Enemy_Block_Kind_e_SOFT); break;
					case 1: enemy = CreateEnemy_CircleBlock(x, y, 20, Enemy_Block_Kind_e_NORM); break;
					case 2: enemy = CreateEnemy_CircleBlock(x, y, 40, Enemy_Block_Kind_e_HARD); break;
					case 3: enemy = CreateEnemy_SquareBlock(x, y, 10, Enemy_Block_Kind_e_SOFT); break;
					case 4: enemy = CreateEnemy_SquareBlock(x, y, 20, Enemy_Block_Kind_e_NORM); break;
					case 5: enemy = CreateEnemy_SquareBlock(x, y, 40, Enemy_Block_Kind_e_HARD); break;
					case 6: enemy = null; break;
					case 7: enemy = null; break;
					case 8: enemy = null; break;
					}

					if (enemy != null)
					{
						@@_Enemies.push(enemy);
					}
				}
				for (var<Enemy_t> enemy of @@_Enemies) // 落下
				{
					enemy.DestPt = CreateD2Point(enemy.X, enemy.Y + 60.0);
				}
			}
			else
			{
				@@_ShootingFrame++;
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
					// ブロックを撃破した場合でも跳ね返らせるため、跳ね返り処理をダメージ処理の前に行う。

					// 跳ね返り
					{
						var<double> crashedAngle;

						if (enemy.Kind == Enemy_Kind_e_CIRCLE)
						{
							crashedAngle = GetAngle(shot.X - enemy.X, shot.Y - enemy.Y); // 円形ブロックの中心から見たボールの角度
						}
						else if (enemy.Kind == Enemy_Kind_e_SQUARE)
						{
							if (LastCrashed_矩形の角から見た円形_Angle != null)
							{
								crashedAngle = LastCrashed_矩形の角から見た円形_Angle;
							}
							else
							{
								crashedAngle = GetAngle(shot.X - enemy.X, shot.Y - enemy.Y); // 正方形ブロックの中心から見たボールの角度

								if (crashedAngle < (Math.PI / 4) * 1)
								{
									crashedAngle = 0.0;
								}
								else if (crashedAngle < (Math.PI / 4) * 3)
								{
									crashedAngle = Math.PI / 2;
								}
								else if (crashedAngle < (Math.PI / 4) * 5)
								{
									crashedAngle = Math.PI;
								}
								else if (crashedAngle < (Math.PI / 4) * 7)
								{
									crashedAngle = Math.PI * 1.5;
								}
								else
								{
									crashedAngle = 0.0;
								}
							}
						}
						else
						{
							error();
						}

						var<double> shotAngle = GetAngle(shot.XAdd, shot.YAdd);

						shotAngle -= crashedAngle; // 相対角度に変換

						while (Math.PI < shotAngle)
						{
							shotAngle -= Math.PI * 2;
						}
						while (shotAngle < -Math.PI)
						{
							shotAngle += Math.PI * 2;
						}

						if (Math.PI / 2 < shotAngle)
						{
							shotAngle = Math.PI - shotAngle;
						}
						else if (shotAngle < -Math.PI / 2)
						{
							shotAngle = -Math.PI - shotAngle;
						}
						else
						{
							shotAngle /= 2; // zantei
						}

						shotAngle += crashedAngle; // 相対角度を解除

						var<D2Point_t> shotSpeed = AngleToPoint(shotAngle, BALL_SPEED);

						shot.XAdd = shotSpeed.X;
						shot.YAdd = shotSpeed.Y;
					}
					// 跳ね返り end

					enemy.HP -= shot.AttackPoint;

					if (enemy.HP <= 0) // ? 死亡した。
					{
						enemy.HP = 0; // 削りすぎを解消

						KillEnemy(enemy);

						break; // この敵は死亡したので、以降の自弾については判定不要、次の敵へ。
					}
				}
			}
		}

		for (var<int> index = 0; index < @@_Shots.length; index++) // 側面・上部反射
		{
			var<Shot_t> shot = @@_Shots[index];

			if (shot.X < 0.0)
			{
				shot.XAdd = Math.abs(shot.XAdd);
			}
			if (Screen_W < shot.X)
			{
				shot.XAdd = Math.abs(shot.XAdd) * -1;
			}
			if (shot.Y < 0.0)
			{
				shot.YAdd = Math.abs(shot.YAdd);
			}
			if (Screen_H < shot.Y)
			{
				shot.AttackPoint = -1;

				if (@@_Return_X == null)
				{
					@@_Return_X = shot.X;
				}

				AddEffect(function* <generatorForTask> ()
				{
					var<double> x = shot.X;

					for (var<int> c = 0; c < 20; c++)
					{
						x = Approach(x, @@_Return_X, 0.9);

						Draw(P_Ball, x, Screen_H, 0.5, 0.0, 1.0);

						yield 1;
					}
				}());
			}
		}

		// ====
		// 当たり判定ここまで
		// ====

		// 画面下部に達したブロックの消去
		for (var<Enemy_t> enemy of @@_Enemies)
		{
			if (
				enemy.DestPt == null && // ? 移動完了
				Screen_H - 100.0 < enemy.Y // ? 画面下部に達した。
				)
			{
				KillEnemy(enemy);
			}
		}

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

function @@_DrawWall()
{
	SetColor(I3ColorToString(CreateI3Color(0, 0, 0)));
	PrintRect(0, 0, Screen_W, Screen_H);
}
