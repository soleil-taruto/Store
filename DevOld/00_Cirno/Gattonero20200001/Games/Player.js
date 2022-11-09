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
	プレイヤーの垂直方向の速度
*/
var<double> PlayerYSpeed = 0.0;

/*
	プレイヤーが左を向いているか
*/
var<boolean> PlayerFacingLeft = false;

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
	プレイヤー・ジャンプ・カウンタ
	0 == 無効
	1～ == ジャンプｎ回目
*/
var<int> PlayerJumpCount = 0;

/*
	プレイヤー・ジャンプ・フレーム
	0 == 無効
	1～ == ジャンプ中
*/
var<int> PlayerJumpFrame = 0;

/*
	プレイヤー滞空フレーム
	0 == 無効
	1～ == 滞空中
*/
var<int> PlayerAirborneFrame = 0;

/*
	プレイヤーしゃがみフレーム
	0 == 無効
	1～ == しゃがみ中
*/
var<int> PlayerShagamiFrame = 0;

/*
	プレイヤー上向きフレーム
	0 == 無効
	1～ == 上向き中
*/
var<int> PlayerUwamukiFrame = 0;

/*
	プレイヤー下向きフレーム
	0 == 無効
	1～ == 下向き中
*/
var<int> PlayerShitamukiFrame = 0;

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

var<boolean> @@_JumpLock = false;
var<boolean> @@_MoveSlow = false;

function <void> ResetPlayer()
{
	PlayerHP = PLAYER_HP_MAX;
	PlayerX = Screen_W / 2.0;
	PlayerY = Screen_H / 2.0;
	PlayerDamageFrame = 0;
	PlayerInvincibleFrame = 0;
	PlayerYSpeed = 0.0;
	PlayerFacingLeft = false;
	PlayerCrash = null;
	PlayerMoveFrame = 0;
	PlayerJumpCount = 0;
	PlayerJumpFrame = 0;
	PlayerAirborneFrame = ToFix(IMAX / 2); // ゲーム開始直後に空中でジャンプできないように
	PlayerShagamiFrame = 0;
	PlayerUwamukiFrame = 0;
	PlayerShitamukiFrame = 0;
	PlayerAttackFrame = 0;
	PlayerAttack = null;
	@@_JumpLock = false;
	@@_MoveSlow = false;
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
		var<boolean> move = false;
		var<boolean> slow = false;
		var<boolean> attack = false;
		var<boolean> shagami = false;
		var<boolean> uwamuki = false;
		var<boolean> shitamuki = false;
		var<int> jump = 0;

		if (!damageOrUID && 1 <= GetInput_8())
		{
			uwamuki = true;
		}
		if (!damageOrUID && 1 <= GetInput_2())
		{
			shagami = true;
			shitamuki = true;
		}
		if (!damageOrUID && 1 <= GetInput_4())
		{
			PlayerFacingLeft = true;
			move = true;
		}
		if (!damageOrUID && 1 <= GetInput_6())
		{
			PlayerFacingLeft = false;
			move = true;
		}
		if (!damageOrUID && 1 <= GetInput_B())
		{
//			slow = true;
			attack = true;
		}
		if (!damageOrUID && 1 <= GetInput_A())
		{
			jump = GetInput_A();
		}

		if (move)
		{
			PlayerMoveFrame++;
			shagami = false;
//			uwamuki = false;
//			shitamuki = false;
		}
		else
		{
			PlayerMoveFrame = 0;
		}

		@@_MoveSlow = move && slow;

		if (jump == 0)
		{
			@@_JumpLock = false;
		}

		if (1 <= PlayerJumpFrame) // ? ジャンプ中
		{
			if (1 <= jump)
			{
				PlayerJumpFrame++;
			}
			else
			{
				// ★ ジャンプを中断・終了した。

				PlayerJumpFrame = 0;

				if (PlayerYSpeed < 0.0)
				{
					PlayerYSpeed /= 2.0;
				}
			}
		}
		else // ? 接地中 || 滞空中
		{
			// 事前入力 == 着地前の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。
			// 入力猶予 == 落下(地面から離れた)直後の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。

			var<int> 事前入力時間 = 10;
			var<int> 入力猶予時間 = 5;

			if (PlayerAirborneFrame < 入力猶予時間 && PlayerJumpCount == 0)
			{
				if (1 <= jump && jump < 事前入力時間 && !@@_JumpLock) // ? 接地状態からのジャンプが可能な状態
				{
					// ★ ジャンプを開始した。

					PlayerJumpFrame = 1;
					PlayerJumpCount = 1;

					PlayerYSpeed = PLAYER_JUMP_SPEED;

					@@_JumpLock = true;
				}
				else
				{
					PlayerJumpCount = 0;
				}
			}
			else // ? 接地状態からのジャンプが「可能ではない」状態
			{
				// 滞空状態に入ったら「通常ジャンプの状態」にする。
				if (PlayerJumpCount < 1)
				{
					PlayerJumpCount = 1;
				}

				if (1 <= jump && jump < 事前入力時間 && PlayerJumpCount < PLAYER_JUMP_MAX && !@@_JumpLock)
				{
					// ★ 空中(n-段)ジャンプを開始した。

					PlayerJumpFrame = 1;
					PlayerJumpCount++;

					PlayerYSpeed = PLAYER_JUMP_SPEED;

					AddEffect(Effect_Jump(PlayerX, PlayerY + PLAYER_接地判定Pt_Y));

					@@_JumpLock = true;
				}
				else
				{
					// noop
				}
			}
		}

		if (PlayerJumpFrame == 1) // ? ジャンプ開始
		{
			SE(S_Jump);
		}

		if (1 <= PlayerAirborneFrame)
		{
			shagami = false;
//			uwamuki = false;
//			shitamuki = false;
		}

		if (shagami)
		{
			PlayerShagamiFrame++;
		}
		else
		{
			PlayerShagamiFrame = 0;
		}

		if (uwamuki)
		{
			PlayerUwamukiFrame++;
		}
		else
		{
			PlayerUwamukiFrame = 0;
		}

		if (shitamuki)
		{
			PlayerShitamukiFrame++;
		}
		else
		{
			PlayerShitamukiFrame = 0;
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

			PlayerX -= (1.0 - rate) * 9.0 * (PlayerFacingLeft ? -1 : 1);
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

	// 移動
	{
		if (1 <= PlayerMoveFrame)
		{
			var<double> speed;

			if (@@_MoveSlow)
			{
				speed = PlayerMoveFrame / 10.0;
				speed = Math.min(speed, PLAYER_SLOW_SPEED);
			}
			else
			{
				/*/
				// 走り出し時に加速する。
				{
					speed = (PlayerMoveFrame + 1) / 2.0;
					speed = Math.min(speed, PLAYER_SPEED);
				}
				/*/
				// 走り出し時に加速しない。
				{
					speed = PLAYER_SPEED;
				}
				//*/
			}
			speed *= PlayerFacingLeft ? -1.0 : 1.0;

			PlayerX += speed;
		}
		else
		{
			PlayerX = ToInt(PlayerX);
		}

		// 重力による加速
		PlayerYSpeed += PLAYER_GRAVITY;

		// 自由落下の最高速度を超えないように矯正
		PlayerYSpeed = Math.min(PlayerYSpeed, PLAYER_FALL_SPEED_MAX);

		// 自由落下
		PlayerY += PlayerYSpeed;
	}

	// 位置矯正
	{
		var<boolean> touchSide_L =
			IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY - PLAYER_側面判定Pt_YT ) ||
			IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY                        ) ||
			IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY + PLAYER_側面判定Pt_YB );

		var<boolean> touchSide_R =
			IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY - PLAYER_側面判定Pt_YT ) ||
			IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY                        ) ||
			IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY + PLAYER_側面判定Pt_YB );

		if (touchSide_L && touchSide_R) // -> 壁抜け防止のため再チェック
		{
			touchSide_L = IsPtWall_XY(PlayerX - PLAYER_側面判定Pt_X, PlayerY);
			touchSide_R = IsPtWall_XY(PlayerX + PLAYER_側面判定Pt_X, PlayerY);
		}

		if (touchSide_L && touchSide_R)
		{
			// noop
		}
		else if (touchSide_L)
		{
			PlayerX = ToTileCenterX(PlayerX - PLAYER_側面判定Pt_X) + TILE_W / 2.0 + PLAYER_側面判定Pt_X;
		}
		else if (touchSide_R)
		{
			PlayerX = ToTileCenterX(PlayerX + PLAYER_側面判定Pt_X) - TILE_W / 2.0 - PLAYER_側面判定Pt_X;
		}

		var<boolean> touchCeiling_L = IsPtWall_XY(PlayerX - PLAYER_脳天判定Pt_X , PlayerY - PLAYER_脳天判定Pt_Y);
		var<boolean> touchCeiling_M = IsPtWall_XY(PlayerX                       , PlayerY - PLAYER_脳天判定Pt_Y);
		var<boolean> touchCeiling_R = IsPtWall_XY(PlayerX + PLAYER_脳天判定Pt_X , PlayerY - PLAYER_脳天判定Pt_Y);

		if ((touchCeiling_L && touchCeiling_R) || touchCeiling_M)
		{
			if (PlayerYSpeed < 0.0)
			{
				// プレイヤーと天井の反発係数
				//
//				var<double> K = 1.0;
				var<double> K = 0.0;

				PlayerY = ToTileCenterY(PlayerY - PLAYER_脳天判定Pt_Y) + TILE_H / 2 + PLAYER_脳天判定Pt_Y;
				PlayerYSpeed = Math.abs(PlayerYSpeed) * K;
				PlayerJumpFrame = 0;
			}
		}
		else if (touchCeiling_L)
		{
			PlayerX = ToTileCenterX(PlayerX - PLAYER_脳天判定Pt_X) + TILE_W / 2.0 + PLAYER_脳天判定Pt_X;
		}
		else if (touchCeiling_R)
		{
			PlayerX = ToTileCenterX(PlayerX + PLAYER_脳天判定Pt_X) - TILE_W / 2.0 - PLAYER_脳天判定Pt_X;
		}

		var<boolean> touchGround =
			IsPtWall_XY(PlayerX - PLAYER_接地判定Pt_X, PlayerY + PLAYER_接地判定Pt_Y) ||
			IsPtWall_XY(PlayerX + PLAYER_接地判定Pt_X, PlayerY + PLAYER_接地判定Pt_Y);

		// memo: @ 2022.7.11
		// 上昇中(ジャンプ中)に接地判定が発生することがある。
		// 接地中は重力により PlayerYSpeed がプラスに振れる。
		// -> 接地による位置等の調整は PlayerYSpeed がプラスに振れている場合のみ行う。

		if (touchGround && 0.0 < PlayerYSpeed)
		{
			PlayerY = ToTileCenterY(PlayerY + PLAYER_接地判定Pt_Y) - TILE_H / 2.0 - PLAYER_接地判定Pt_Y;
			PlayerYSpeed = 0.0;
			PlayerJumpCount = 0;
			PlayerAirborneFrame = 0;
		}
		else
		{
			PlayerAirborneFrame++;
		}
	}

	// 攻撃
	{
		if (1 <= PlayerAttackFrame && ProcFrame % 4 == 0)
		{
			if (1 <= PlayerUwamukiFrame)
			{
				var<Shot_t> shot = CreateShot_Normal(PlayerX, PlayerY - 20.0, PlayerFacingLeft, true, false);

				GetShots().push(shot);
			}
			else
			{
				var<Shot_t> shot = CreateShot_Normal(PlayerX + 30.0 * (PlayerFacingLeft ? -1 : 1), PlayerY + 4.0, PlayerFacingLeft, false, false);

				GetShots().push(shot);
			}

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
		PlayerCrash = CreateCrash_Circle(
			PlayerX,
			PlayerY,
			10.0
			);
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

	if (1 <= PlayerAirborneFrame)
	{
		picture = PlayerFacingLeft ? P_PlayerMirrorJump : P_PlayerJump;
	}
	else if (1 <= PlayerMoveFrame)
	{
		var<int> koma = ToFix(ProcFrame / 6);

		if (koma < 4)
		{
			// none
		}
		else
		{
			koma %= 4;

			if (koma == 0)
			{
				koma = 2;
			}
		}

		picture = (PlayerFacingLeft ? P_PlayerMirrorRun : P_PlayerRun)[koma];
	}
	else if (1 <= PlayerAttackFrame && PlayerUwamukiFrame == 0)
	{
		picture = PlayerFacingLeft ? P_PlayerMirrorAttack : P_PlayerAttack;
	}
	else
	{
		picture = PlayerFacingLeft ? P_PlayerMirrorStand : P_PlayerStand;
	}

	Draw(picture, PlayerX - Camera.X, PlayerY - Camera.Y, plA, 0.0, 1.0);
}
