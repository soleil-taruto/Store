/*
	Attack - Sliding
*/

function* <generatorForTask> CreateAttack_Sliding()
{
	AddEffect(Effect_Sliding(PlayerX, PlayerY + 20.0));

	for (var<int> frame = 0; ; frame++)
	{
		if (10 < frame) // ? 最低持続時間経過
		{
			if (AttackCheckPlayer_GetSide_Mode(4) != 3) // ? ここは立ち上がれる場所である。
			{
				if (GetInput_A() <= 0 || GetInput_2() <= 0) // ? 下・ジャンプボタン少なくともどちらかを離している。
				{
					break;
				}
			}
		}

		if (AttackCheckPlayer_GetSide_Mode(2) != 0) // ? 壁に当たっている。
		{
			if (AttackCheckPlayer_GetSide_Mode(4) != 3) // ? ここは立ち上がれる場所である。
			{
				if (GetInput_A() == 1) // ? ジャンプボタンを押した。
				{
					break;
				}
			}
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
		{
			var<double> SPEED = 10.0;

			if (PlayerFacingLeft)
			{
				PlayerX -= SPEED;
			}
			else
			{
				PlayerX += SPEED;
			}
		}

//		AttackProcPlayer_Move(); // 不要
		AttackProcPlayer_Fall();
		AttackProcPlayer_Side_Mode(2);
		AttackProcPlayer_Ceiling();

		if (!AttackProcPlayer_Ground())
		{
//			PlayerAirborneFrame = 1; // 崖の淵に立ってしまわないように滞空状態にする。
			PlayerAirborneFrame = ToFix(IMAX / 2); // ジャンプしてしまわないように十分大きな値をセットする。
			break;
		}

		AttackProcPlayer_Status();

		var<double> plA = 1.0;

		if (1 <= PlayerInvincibleFrame) // ? 無敵時間中
		{
			plA = 0.5;
		}
		else
		{
			PlayerCrash = CreateCrash_Circle(
				PlayerX,
				PlayerY + 10.0,
				10.0
				);
		}

		AddTask(PlayerDrawTasks, function* <generatorForTask> ()
		{
			Draw(
				PlayerFacingLeft ? P_PlayerMirrorSliding : P_PlayerSliding,
				PlayerX - Camera.X,
				PlayerY - Camera.Y,
				plA,
				0.0,
				1.0
				);
		}());

		yield 1;
	}
}
