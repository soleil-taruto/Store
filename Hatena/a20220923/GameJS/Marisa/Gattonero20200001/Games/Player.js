/*
	プレイヤー情報
*/

/*
	プレイヤー体力
	-1 == 死亡
	0 == (不使用・予約)
	1～ == 残り体力
*/
var<int> PlayerHP = PLAYER_HP_MAX;

/*
	プレイヤーの位置
*/
var<double> PlayerX = 0.0;
var<double> PlayerY = 0.0;

/*
	プレイヤー・ダメージ・フレーム
	0 == 無効
	1～ == ダメージ中
*/
var<int> PlayerDamageFrame = 0;

/*
	プレイヤー無敵時間フレーム
	0 == 無効
	1～ == 無敵時間中
*/
var<int> PlayerInvincibleFrame = 0;

/*
	プレイヤーが向いている方向(8方向_テンキー方式)
*/
var<int> PlayerFaceDirection = 2;

/*
	今フレームの当たり判定, null == 当たり判定無し
*/
var<Crash_t> PlayerCrash = null;

/*
	プレイヤー移動フレーム
	0 == 無効
	1～ == 移動中
*/
var<int> PlayerMoveFrame = 0;

/*
	プレイヤー攻撃フレーム
	0 == 無効
	1～ == 攻撃中
*/
var<int> PlayerAttackFrame = 0;

/*
	プレイヤー攻撃モーション
	-- 攻撃(Attack)と言っても攻撃以外の利用(スライディング・梯子など)も想定する。
	null == 無効
	null != DrawPlayerの代わりに実行される。
*/
var<Func boolean> PlayerAttack = null;

function <void> ResetPlayer()
{
	PlayerHP = PLAYER_HP_MAX;
	PlayerX = Screen_W / 2.0;
	PlayerY = Screen_H / 2.0;
	PlayerDamageFrame = 0;
	PlayerInvincibleFrame = 0;
	PlayerFaceDirection = 2;
	PlayerCrash = null;
	PlayerMoveFrame = 0;
	PlayerAttackFrame = 0;
}

/*
	行動
	処理すべきこと：
	-- 行動
	-- 当たり判定の設置
*/
function <void> ActPlayer()
{
	// reset
	{
		PlayerCrash = null;
	}

	if (DEBUG && GetKeyInput(84) == 1) // ? T 押下 -> 攻撃テスト
	{
		PlayerAttack = Supplier(CreateAttack_BDummy());
		return; // HACK: このフレームのみ当たり判定無し問題 -- 1フレームなので看過する。様子見 @ 2022.7.31
	}

	// 入力
	{
		var<boolean> damageOrUID = 1 <= PlayerDamageFrame || UserInputDisabled;
		var<boolean> dir2 = false;
		var<boolean> dir4 = false;
		var<boolean> dir6 = false;
		var<boolean> dir8 = false;
		var<boolean> slow = false;
		var<boolean> attack = false;

		if (!damageOrUID && 1 <= GetInput_2())
		{
			dir2 = true;
		}
		if (!damageOrUID && 1 <= GetInput_4())
		{
			dir4 = true;
		}
		if (!damageOrUID && 1 <= GetInput_6())
		{
			dir6 = true;
		}
		if (!damageOrUID && 1 <= GetInput_8())
		{
			dir8 = true;
		}
		if (!damageOrUID && 1 <= GetInput_A())
		{
			slow = true;
		}
		if (!damageOrUID && 1 <= GetInput_B())
		{
			attack = true;
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

		if (slow)
		{
			speed = PLAYER_SLOW_SPEED;
		}

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
			break;

		default:
			error(); // never
		}

		if (dir != 5 && !slow && !attack)
		{
			PlayerFaceDirection = dir;
		}

		if (dir != 5)
		{
			PlayerMoveFrame++;
		}
		else
		{
			PlayerMoveFrame = 0;
		}

		if (PlayerMoveFrame == 0) // 立ち止まったら座標を整数に矯正
		{
			PlayerX = ToInt(PlayerX);
			PlayerY = ToInt(PlayerY);
		}

		if (attack)
		{
			PlayerAttackFrame++;
		}
		else
		{
			PlayerAttackFrame = 0;
		}
	}

damageBlock:
	if (1 <= PlayerDamageFrame) // ? ダメージ中
	{
		if (PLAYER_DAMAGE_FRAME_MAX < ++PlayerDamageFrame)
		{
			PlayerDamageFrame = 0;
			PlayerInvincibleFrame = 1;
			break damageBlock;
		}
		var<int> frame = PlayerDamageFrame; // 値域 == 2 ～ PLAYER_DAMAGE_FRAME_MAX
		var<double> rate = RateAToB(2, PLAYER_DAMAGE_FRAME_MAX, frame);

		// ダメージ中の処理
		{
			if (frame == 2) // 初回のみ
			{
				SE(S_Damaged);
			}

			var<D2Point_t> speed = GetXYSpeed(PlayerFaceDirection, -5.0);

			for (var<int> c = 0; c < 5; c++)
			{
				if (IsPtWall_XY(PlayerX, PlayerY)) // ? 歩行可能な場所ではない -> これ以上ヒットバックさせない。
				{
					break;
				}

				PlayerX += speed.X;
				PlayerY += speed.Y;
			}
		}
	}

invincibleBlock:
	if (1 <= PlayerInvincibleFrame) // ? 無敵時間中
	{
		if (PLAYER_INVINCIBLE_FRAME_MAX < ++PlayerInvincibleFrame)
		{
			PlayerInvincibleFrame = 0;
			break invincibleBlock;
		}
		var<int> frame = PlayerInvincibleFrame; // 値域 == 2 ～ PLAYER_INVINCIBLE_FRAME_MAX
		var<double> rate = RateAToB(2, PLAYER_INVINCIBLE_FRAME_MAX, frame);

		// 無適時間中の処理
		{
			// none
		}
	}

	// 移動 -> 入力と同時に行っている。

	// 位置矯正
	{
		PlayerWallProc();
	}

	// 攻撃
	{
		if (1 <= PlayerAttackFrame && ProcFrame % 4 == 0)
		{
			GetShots().push(CreateShot_Normal(PlayerX, PlayerY, PlayerFaceDirection));

			SE(S_Shoot);
		}
	}

	// 当たり判定をセットする。
	// -- ダメージ中・無敵時間中は null (当たり判定無し) をセットすること。

	PlayerCrash = null; // reset

	if (1 <= PlayerDamageFrame) // ? ダメージ中
	{
		// noop
	}
	else if (1 <= PlayerInvincibleFrame) // ? 無敵時間中
	{
		// noop
	}
	else
	{
		PlayerCrash = CreateCrash_Circle(PlayerX, PlayerY, 10.0);
	}
}

/*
	描画
	処理すべきこと：
	-- 描画
*/
function <void> DrawPlayer()
{
	var<double> plA = 1.0;
	var<Picture_t> picture = P_Dummy;

	if (
		1 <= PlayerDamageFrame ||
		1 <= PlayerInvincibleFrame
		)
	{
		plA = 0.5;
	}

	var<int> koma = 0;

	if (1 <= PlayerMoveFrame)
	{
		koma = ToFix(ProcFrame / 5) % 4;
	}

	picture = P_Player[PlayerFaceDirection][koma];

	var<double> dx = PlayerX - Camera.X;
	var<double> dy = PlayerY - Camera.Y - 15.0;

	Draw(picture, dx, dy, plA, 0.0, 1.0);
}
