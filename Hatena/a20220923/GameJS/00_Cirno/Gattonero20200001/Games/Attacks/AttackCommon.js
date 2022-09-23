/*
	Attack 共通
*/

// プレイヤー動作セット
// -- この辺やっとけば良いんじゃないか的な
//
// AttackProcPlayer_Move();  -- 移動
// AttackProcPlayer_Fall();
//
// AttackProcPlayer_Side();     -- 側面
// AttackProcPlayer_Ceiling();  -- 脳天
// AttackProcPlayer_Ground();   -- 接地
//
// AttackProcPlayer_Status();
// AttackProcPlayer_Atari();
//
// AddTask(PlayerDrawTasks, プレイヤー描画タスク );
//

// ========================
// ==== プレイヤー動作 ====
// ========================

/*
	移動
*/
function <void> AttackProcPlayer_Move()
{
	var<double> speed;

	/*
	if (1 <= GetInput_B()) // ? 低速移動
	{
		speed = PLAYER_SLOW_SPEED;
	}
	else
	{
		speed = PLAYER_SPEED;
	}
	*/
	speed = PLAYER_SPEED;

	if (1 <= GetInput_4())
	{
		PlayerX -= speed;
	}
	if (1 <= GetInput_6())
	{
		PlayerX += speed;
	}
}

/*
	Fall
*/
function <void> AttackProcPlayer_Fall()
{
	if (1 <= PlayerJumpFrame) // ? ジャンプ中(だった)
	{
		if (GetInput_A() <= 0) // ? ジャンプを中断・終了した。
		{
			PlayerJumpFrame = 0;

			if (PlayerYSpeed < 0.0)
			{
				PlayerYSpeed /= 2.0;
			}
		}
	}

	// 重力による加速
	PlayerYSpeed += PLAYER_GRAVITY;

	// 自由落下の最高速度を超えないように矯正
	PlayerYSpeed = Math.min(PlayerYSpeed, PLAYER_FALL_SPEED_MAX);

	// 自由落下
	PlayerY += PlayerYSpeed;
}

// ====================================
// ==== プレイヤー動作・接地系判定 ====
// ====================================

function <boolean> AttackCheckPlayer_IsSide()
{
	return AttackProcPlayer_GetSide() != 0;
}

function <int> AttackCheckPlayer_GetSide()
{
	var<boolean> touchSide_L =
		IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY - PLAYER_側面判定Pt_YT ) ||
		IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY                        ) ||
		IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY + PLAYER_側面判定Pt_YB );

	var<boolean> touchSide_R =
		IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY - PLAYER_側面判定Pt_YT ) ||
		IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY                        ) ||
		IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY + PLAYER_側面判定Pt_YB );

	return (touchSide_L ? 1 : 0) | (touchSide_R ? 2 : 0);
}

function <int> AttackCheckPlayer_GetSideSub()
{
	var<boolean> touchSide_L = IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY);
	var<boolean> touchSide_R = IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY);

	return (touchSide_L ? 1 : 0) | (touchSide_R ? 2 : 0);
}

function <boolean> AttackCheckPlayer_GetCeiling()
{
	var<boolean> touchCeiling_L = IsPtWall_XY(PlayerX - PLAYER_脳天判定Pt_X , PlayerY - PLAYER_脳天判定Pt_Y);
	var<boolean> touchCeiling_M = IsPtWall_XY(PlayerX                       , PlayerY - PLAYER_脳天判定Pt_Y);
	var<boolean> touchCeiling_R = IsPtWall_XY(PlayerX + PLAYER_脳天判定Pt_X , PlayerY - PLAYER_脳天判定Pt_Y);

	return (touchCeiling_L && touchCeiling_R) || touchCeiling_M;
}

function <boolean> AttackCheckPlayer_GetGround()
{
	var<boolean> touchGround =
		IsPtWall_XY(PlayerX - PLAYER_接地判定Pt_X, PlayerY + PLAYER_接地判定Pt_Y) ||
		IsPtWall_XY(PlayerX + PLAYER_接地判定Pt_X, PlayerY + PLAYER_接地判定Pt_Y);

	return touchGround;
}

// ====================================
// ==== プレイヤー動作・接地系処理 ====
// ====================================

function <boolean> AttackProcPlayer_Side()
{
	var<int> flag = AttackCheckPlayer_GetSide();

	if (flag == 3) // 左右両方 -> 壁抜け防止のため再チェック
	{
		flag = AttackCheckPlayer_GetSideSub();
	}

	if (flag == 3) // 左右両方
	{
		// noop
	}
	else if (flag == 1) // 左側面
	{
		PlayerX = ToTileCenterX(PlayerX - PLAYER_側面判定Pt_X) + TILE_W / 2.0 + PLAYER_側面判定Pt_X;
	}
	else if (flag == 2) // 右側面
	{
		PlayerX = ToTileCenterX(PlayerX + PLAYER_側面判定Pt_X) - TILE_W / 2.0 - PLAYER_側面判定Pt_X;
	}
	else if (flag == 0) // なし
	{
		// noop
	}
	else
	{
		error(); // never
	}
	return flag != 0;
}

function <boolean> AttackProcPlayer_Ceiling()
{
	var<boolean> ret = AttackCheckPlayer_GetCeiling();

	if (ret)
	{
		PlayerY = ToTileCenterY(PlayerY - PLAYER_脳天判定Pt_Y) + TILE_H / 2.0 + PLAYER_脳天判定Pt_Y;
		PlayerYSpeed = Math.max(0.0, PlayerYSpeed);
	}
	return ret;
}

function <boolean> AttackProcPlayer_Ground()
{
	var<boolean> ret = AttackCheckPlayer_GetGround();

	if (ret)
	{
		PlayerY = ToTileCenterY(PlayerY + PLAYER_接地判定Pt_Y) - TILE_H / 2.0 - PLAYER_接地判定Pt_Y;
		PlayerYSpeed = Math.min(0.0, PlayerYSpeed);
	}
	return ret;
}

// ================================
// ==== プレイヤー動作・その他 ====
// ================================

function <void> AttackProcPlayer_Status()
{
	if (1 <= PlayerDamageFrame && PLAYER_DAMAGE_FRAME_MAX < ++PlayerDamageFrame)
	{
		PlayerDamageFrame = 0;
		PlayerInvincibleFrame = 1;
	}
	if (1 <= PlayerInvincibleFrame && PLAYER_INVINCIBLE_FRAME_MAX < ++PlayerInvincibleFrame)
	{
		PlayerInvincibleFrame = 0;
	}
}

function <void> AttackProcPlayer_Atari()
{
	PlayerCrash = CreateCrash_Circle(
		PlayerX,
		PlayerY,
		10.0
		);
}

// =====================================
// ==== プレイヤー動作系 (ここまで) ====
// =====================================
