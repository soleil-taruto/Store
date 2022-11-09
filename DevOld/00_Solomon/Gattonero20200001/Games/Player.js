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
	行動と描画
	処理すべきこと：
	-- 行動
	-- 当たり判定の設置
	-- 描画
*/
function <void> DrawPlayer()
{
	// ★サンプル -- 要削除
	{
		// 移動
		{
			var<double> SPEED;

			if (1 <= GetInput_A()) // ? 低速移動
			{
				SPEED = 1.0;
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

		if (1 <= GetInput_B() && ProcFrame % 10 == 0) // 攻撃
		{
			GetShots().push(CreateShot_BDummy(PlayerX, PlayerY, 0.0, -10.0));
		}

		PlayerCrash = CreateCrash_Circle(PlayerX, PlayerY, MICRO);

		Draw(P_Dummy, PlayerX, PlayerY, 1.0, Math.PI / 4, 1.0);
	}
}
