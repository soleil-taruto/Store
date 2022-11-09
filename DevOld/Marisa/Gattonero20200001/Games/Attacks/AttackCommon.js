/*
	Attack 共通
*/

// プレイヤー動作セット
// -- この辺やっとけば良いんじゃないか的な
//
// AttackProcPlayer_Move();
// AttackProcPlayer_WallProc();
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
	var<boolean> dir2 = false;
	var<boolean> dir4 = false;
	var<boolean> dir6 = false;
	var<boolean> dir8 = false;

	if (1 <= GetInput_2())
	{
		dir2 = true;
	}
	if (1 <= GetInput_4())
	{
		dir4 = true;
	}
	if (1 <= GetInput_6())
	{
		dir6 = true;
	}
	if (1 <= GetInput_8())
	{
		dir8 = true;
	}

	var<int> dir; // 移動方向 { 1 ～ 4, 6 ～ 9 } == 8方向_テンキー方式, 5 == 移動していない。

	if (dir2 && dir4)
	{
		dir = 1;
	}
	else if (dir2 && dir6)
	{
		dir = 3;
	}
	else if (dir4 && dir8)
	{
		dir = 7;
	}
	else if (dir6 && dir8)
	{
		dir = 9;
	}
	else if (dir2)
	{
		dir = 2;
	}
	else if (dir4)
	{
		dir = 4;
	}
	else if (dir6)
	{
		dir = 6;
	}
	else if (dir8)
	{
		dir = 8;
	}
	else
	{
		dir = 5;
	}

	var<double> speed = PLAYER_SPEED;
	var<double> nanameSpeed = speed / Math.SQRT2;

	switch (dir)
	{
	case 4: PlayerX -= speed; break;
	case 6: PlayerX += speed; break;
	case 8: PlayerY -= speed; break;
	case 2: PlayerY += speed; break;

	case 1:
		PlayerX -= nanameSpeed;
		PlayerY += nanameSpeed;
		break;

	case 3:
		PlayerX += nanameSpeed;
		PlayerY += nanameSpeed;
		break;

	case 7:
		PlayerX -= nanameSpeed;
		PlayerY -= nanameSpeed;
		break;

	case 9:
		PlayerX += nanameSpeed;
		PlayerY -= nanameSpeed;
		break;

	case 5:
		// 立ち止まったら座標を整数に矯正
		PlayerX = ToInt(PlayerX);
		PlayerY = ToInt(PlayerY);
		break;

	default:
		error(); // never
	}
}

/*
	壁抜け処理
*/
function <void> AttackProcPlayer_WallProc()
{
	PlayerWallProc();
}

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
	PlayerCrash = CreateCrash_Circle(PlayerX, PlayerY, 10.0);
}

// =====================================
// ==== プレイヤー動作系 (ここまで) ====
// =====================================
