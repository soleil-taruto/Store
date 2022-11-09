/*
	プレイヤー情報
*/

/*
	プレイヤーの位置
*/
var<double> PlayerX = FIELD_L + FIELD_W / 2;
var<double> PlayerY = FIELD_T + FIELD_H / 2;

/*
	今フレームの当たり判定, null == 当たり判定無し
*/
var<Crash_t> PlayerCrash = null;

/*
	再登場フレーム
	-- 再登場を開始するには 1 をセットすること。
	0 == 無効
	1 ～ PLAYER_BORN_FRAME_MAX == 再登場中
*/
var<int> PlayerBornFrame = 0;

/*
	再登場中の描画位置
*/
var<double> @@_Born_X;
var<double> @@_Born_Y;

/*
	無敵状態フレーム
	-- 無敵状態を開始するには 1 をセットすること。
	0 == 無効
	1 ～ PLAYER_INVINCIBLE_FRAME_MAX == 無敵状態中
*/
var<int> PlayerInvincibleFrame = 0;

/*
	攻撃レベル
	1 ～ PLAYER_ATTACK_LV_MAX
*/
var<int> PlayerAttackLv = 1;

/*
	残機
	0 ～
*/
var<int> PlayerZankiNum = 3;

/*
	行動と描画
	処理すべきこと：
	-- 行動
	-- 当たり判定の設置
	-- 描画
*/
function <void> DrawPlayer()
{
	// 移動
	{
		var<double> SPEED;

		if (1 <= GetInput_A()) // ? 低速移動
		{
			SPEED = 2.5;
		}
		else // ? 高速移動
		{
			SPEED = 5.0;
		}

		if (1 <= GetInput_2())
		{
			PlayerY += SPEED;
		}
		if (1 <= GetInput_4())
		{
			PlayerX -= SPEED;
		}
		if (1 <= GetInput_6())
		{
			PlayerX += SPEED;
		}
		if (1 <= GetInput_8())
		{
			PlayerY -= SPEED;
		}

		PlayerX = ToRange(PlayerX, FIELD_L, FIELD_R);
		PlayerY = ToRange(PlayerY, FIELD_T, FIELD_B);
	}

	// 再登場中は、移動は可能、攻撃は不可とする。

	if (1 <= PlayerBornFrame) // ? 再登場中
	{
		var<double> rate = PlayerBornFrame / PLAYER_BORN_FRAME_MAX;
		var<double> remRate = 1.0 - rate;

		if (PlayerBornFrame == 1) // ? 初回
		{
			@@_Born_X = FIELD_L + FIELD_W / 2;
			@@_Born_Y = FIELD_B + 100.0;
		}

		@@_Born_X = Approach(@@_Born_X, PlayerX, 1.0 - rate * rate * rate);
		@@_Born_Y = Approach(@@_Born_Y, PlayerY, 1.0 - rate * rate * rate);

		PlayerCrash = null; // 当たり判定無し。

		Draw(P_Player, @@_Born_X, @@_Born_Y, 0.5, remRate * remRate * 30.0, 1.0);

		// 再登場フレーム・インクリメント
		//
		if (PlayerBornFrame < PLAYER_BORN_FRAME_MAX)
		{
			PlayerBornFrame++;
		}
		else // ? 再登場_終了
		{
			PlayerBornFrame = 0;
			PlayerInvincibleFrame = 1;
		}
		return;
	}

	if (1 <= GetInput_B() && ProcFrame % 4 == 0) // 攻撃
	{
		switch (PlayerAttackLv)
		{
		case 1:
			GetShots().push(CreateShot_Normal(PlayerX, PlayerY, Math.PI * 1.5, 20.0));
			break;

		case 2:
			GetShots().push(CreateShot_Normal(PlayerX - 10, PlayerY, Math.PI * 1.5, 20.0));
			GetShots().push(CreateShot_Normal(PlayerX + 10, PlayerY, Math.PI * 1.5, 20.0));
			break;

		case 3:
			GetShots().push(CreateShot_Normal(PlayerX - 20, PlayerY, Math.PI * 1.5, 20.0));
			GetShots().push(CreateShot_Normal(PlayerX,      PlayerY, Math.PI * 1.5, 20.0));
			GetShots().push(CreateShot_Normal(PlayerX + 20, PlayerY, Math.PI * 1.5, 20.0));
			break;

		default:
			error();
		}

		SE(S_PlayerShoot);
	}

	if (1 <= PlayerInvincibleFrame) // ? 無敵状態
	{
//		PlayerCrash = null; // 当たり判定無し。
		PlayerCrash = CreateCrash_Point(PlayerX, PlayerY); // アイテムを取れるように当たり判定は必要！

		Draw(P_Player, PlayerX, PlayerY, 0.5, 0.0, 1.0);

		// 無敵状態フレーム・インクリメント
		//
		if (PlayerInvincibleFrame < PLAYER_INVINCIBLE_FRAME_MAX)
		{
			PlayerInvincibleFrame++;
		}
		else // ? 無敵状態_終了
		{
			PlayerInvincibleFrame = 0;
		}
	}
	else // 通常
	{
		PlayerCrash = CreateCrash_Point(PlayerX, PlayerY);

		Draw(P_Player, PlayerX, PlayerY, 1.0, 0.0, 1.0);
	}
}
