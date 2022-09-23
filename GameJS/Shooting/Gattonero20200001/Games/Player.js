/*
	プレイヤー情報
*/

/*
	プレイヤーの位置
*/
var<double> PlayerX = Screen_W / 2.0;
var<double> PlayerY = Screen_H / 2.0;

/*
	今フレームの当たり判定, null == 当たり判定無し
*/
var<Crash_t> PlayerCrash = null;

/*
	再登場フレーム
	-- 再登場を開始するには 1 をセットすること。
	0 == 無効
	1 ～ PLAYER_REBORN_FRAME_MAX == 再登場中
*/
var<int> PlayerRebornFrame = 0;

/*
	再登場中の描画位置
*/
var<double> @@_Reborn_X;
var<double> @@_Reborn_Y;

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

function <void> ResetPlayer()
{
	PlayerX = Screen_W / 2.0;
	PlayerY = Screen_H / 2.0;
	PlayerRebornFrame = 0;
	PlayerInvincibleFrame = 0;
	PlayerAttackLv = 1;
	PlayerZankiNum = 3;
}

/*
	行動と描画
	処理すべきこと：
	-- 行動
	-- 当たり判定の設置
	-- 描画
*/
function <void> DrawPlayer()
{
	// reset
	{
		PlayerCrash = null;
	}

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

		PlayerX = ToRange(PlayerX, 0.0, Screen_W);
		PlayerY = ToRange(PlayerY, 0.0, Screen_H);
	}

rebornBlock:
	if (1 <= PlayerRebornFrame) // ? 再登場中
	{
		if (PLAYER_REBORN_FRAME_MAX < ++PlayerRebornFrame)
		{
			PlayerRebornFrame = 0;
			PlayerInvincibleFrame = 1;
			break rebornBlock;
		}
		var<int> frame = PlayerRebornFrame; // 値域 == 2 ～ PLAYER_REBORN_FRAME_MAX
		var<double> rate = RateAToB(2, PLAYER_REBORN_FRAME_MAX, frame);

		// 再登場中の処理
		{
			if (frame == 2) // 初回のみ
			{
				@@_Reborn_X = Screen_W / 2.0;
				@@_Reborn_Y = Screen_H + 100.0;
			}

			@@_Reborn_X = Approach(@@_Reborn_X, PlayerX, 1.0 - rate * rate * rate);
			@@_Reborn_Y = Approach(@@_Reborn_Y, PlayerY, 1.0 - rate * rate * rate);
		}

		PlayerCrash = null; // 当たり判定無し。

		// 描画ここから

		Draw(P_Player, @@_Reborn_X, @@_Reborn_Y, 0.5, (1.0 - rate * rate) * 30.0, 1.0);

		return;
	}

	// 攻撃
	{
		if (1 <= GetInput_B() && ProcFrame % 4 == 0)
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
	}

invincibleBlock:
	if (1 <= PlayerInvincibleFrame) // ? 無敵状態
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

		PlayerCrash = null; // 当たり判定無し。

		// 描画ここから

		Draw(P_Player, PlayerX, PlayerY, 0.5, 0.0, 1.0);

		return;
	}

	// 当たり判定セット

	PlayerCrash = CreateCrash_Point(PlayerX, PlayerY);

	// 描画ここから

	Draw(P_Player, PlayerX, PlayerY, 1.0, 0.0, 1.0);
}
