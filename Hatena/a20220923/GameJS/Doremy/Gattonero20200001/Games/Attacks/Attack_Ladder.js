/*
	Attack - Ladder
*/

function* <generatorForTask> CreateAttack_Ladder()
{
	var<int> SHOOTING_FRAME_MAX = 15;

	var<int> shootingFrame = 0; // 0 == 無効, 1～ == 射撃モーション

	PlayerX = ToTileCenterX(PlayerX); // 梯子の中央に寄せる。

	for (var<int> frame = 0; ; frame++)
	{
		if (GetInput_8() <= 0 && 1 <= GetInput_A()) // ? 上ボタンを離して、ジャンプボタン押下
		{
			if (1 <= GetInput_2()) // ? 下ボタン押下 -> ジャンプしない。
			{
				PlayerJumpFrame = 0;
				PlayerJumpCount = 0;

				PlayerYSpeed = 0.0;
			}
			else // ? 下ボタンを離している -> ジャンプする。
			{
				PlayerJumpFrame = 1;
				PlayerJumpCount = 1;

				PlayerYSpeed = PLAYER_JUMP_SPEED;
			}

			// スライディングさせないために滞空状態にする。
			// -- 入力猶予を考慮して、大きい値を設定する。
			PlayerAirborneFrame = ToFix(IMAX / 2);

			break;
		}

		if (1 <= GetInput_4())
		{
			PlayerFacingLeft = true;
		}
		if (1 <= GetInput_6())
		{
			PlayerFacingLeft = false;
		}

		// 移動
		//
		if (shootingFrame == 0) // 射撃モーション時は移動できない。
		{
			var<double> SPEED = 2.5;
			var<double> ACCEL_RATE = 0.5;

			var<boolean> moved = false;

			if (1 <= GetInput_8())
			{
				PlayerY -= Math.min(SPEED, GetInput_8() * ACCEL_RATE);
				moved = true;
			}
			if (1 <= GetInput_2())
			{
				PlayerY += Math.min(SPEED, GetInput_2() * ACCEL_RATE);
				moved = true;
			}

			if (moved)
			{
				// none
			}
		}

		var<boolean> shot = false;

		if (GetInput_B() == 1)
		{
			PlayerShoot(PlayerX + 28.0 * (PlayerFacingLeft ? -1 : 1), PlayerY - 4.0, PlayerFacingLeft);
			shot = true;
		}

		if (shot)
		{
			shootingFrame = 1;
		}
		if (1 <= shootingFrame)
		{
			shootingFrame++;

			if (SHOOTING_FRAME_MAX < shootingFrame)
			{
				shootingFrame = 0;
			}
		}

//		AttackProcPlayer_Move();
//		AttackProcPlayer_Fall();
//		AttackProcPlayer_Side();
		AttackProcPlayer_Ceiling();

		if (AttackProcPlayer_Ground())
		{
			PlayerJumpFrame = 0;
			PlayerJumpCount = 0;

			PlayerYSpeed = 0.0;

			break;
		}

		// ? 梯子の下に出た。
		if (
			!IsPtLadder_XY(PlayerX, PlayerY + TILE_H * 0) &&
			!IsPtLadder_XY(PlayerX, PlayerY + TILE_H * 1)
			)
		{
			PlayerJumpFrame = 0;
			PlayerJumpCount = 0;

			PlayerYSpeed = 0.0;

			break;
		}

		var<Picture_t> picture;

		if (1 <= shootingFrame)
		{
			picture = PlayerFacingLeft ? P_PlayerMirrorClimbAttack : P_PlayerClimbAttack;
		}
		else
		{
			if (IsPtLadder_XY(PlayerX, PlayerY))
			{
				picture = P_PlayerClimb[ToFix(PlayerY / 20) % 2];
			}
			else
			{
				picture = P_PlayerClimbTop;
			}
		}

		AttackProcPlayer_Status();

		var<double> plA = 1.0;

		if (1 <= PlayerInvincibleFrame) // ? 無敵時間中
		{
			plA = 0.5;
		}
		else
		{
			PlayerCrash = CreateCrash_Rect(CreateD4Rect_XYWH(PlayerX, PlayerY, 20.0, 30.0));
		}

		AddTask(PlayerDrawTasks, function* <generatorForTask> ()
		{
			Draw(picture, PlayerX - Camera.X, PlayerY - Camera.Y, plA, 0.0, 1.0);
		}());

		yield 1;
	}
}
