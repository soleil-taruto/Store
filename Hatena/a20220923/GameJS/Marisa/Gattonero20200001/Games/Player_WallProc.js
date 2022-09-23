/*
	プレイヤーの壁抜け処理
*/

function <void> PlayerWallProc()
{
	var<double> SHIFT_SPAN = 0.333;
	var<int> SHIFT_MAX = 100;

	var<boolean[]> touch = @@_CheckTouch();

	if (
		// 縦横方向
		//
		touch[4] ||
		touch[6] ||
		touch[8] ||
		touch[2] ||

		// 斜め方向
		//
		touch[1] ||
		touch[3] ||
		touch[7] ||
		touch[9]
		)
	{
		PlayerX = ToInt(PlayerX);
		PlayerY = ToInt(PlayerY);

		for (var<int> sftCnt = 0; sftCnt < SHIFT_MAX; sftCnt++)
		{
			touch = @@_CheckTouch();

			if (
				touch[4] ||
				touch[6] ||
				touch[8] ||
				touch[2]
				)
			{
				// noop
			}
			else
			{
				break;
			}

			if (touch[4])
			{
				PlayerX += SHIFT_SPAN;
			}
			if (touch[6])
			{
				PlayerX -= SHIFT_SPAN;
			}
			if (touch[8])
			{
				PlayerY += SHIFT_SPAN;
			}
			if (touch[2])
			{
				PlayerY -= SHIFT_SPAN;
			}
		}

		for (var<int> sftCnt = 0; sftCnt < SHIFT_MAX; sftCnt++)
		{
			touch = @@_CheckTouch();

			if (
				touch[1] ||
				touch[3] ||
				touch[7] ||
				touch[9]
				)
			{
				// noop
			}
			else
			{
				break;
			}

			// 壁から抜け出す処理なので NANAME_SHIFT_SPAN は使わないよ！

			if (touch[1])
			{
				PlayerX += SHIFT_SPAN;
				PlayerY -= SHIFT_SPAN;
			}
			if (touch[3])
			{
				PlayerX -= SHIFT_SPAN;
				PlayerY -= SHIFT_SPAN;
			}
			if (touch[7])
			{
				PlayerX += SHIFT_SPAN;
				PlayerY += SHIFT_SPAN;
			}
			if (touch[9])
			{
				PlayerX -= SHIFT_SPAN;
				PlayerY += SHIFT_SPAN;
			}
		}

		PlayerX = ToInt(PlayerX);
		PlayerY = ToInt(PlayerY);
	}
}

/*
	壁との当たり判定
*/
function <boolean[]> @@_CheckTouch()
{
	var<double> x = PlayerX;
	var<double> y = PlayerY;

//	x += 0.0;
//	y += 0.0;

	var<double> R = 15.0;

	var<boolean> touch_4 = IsPtWall_XY(x - R , y     );
	var<boolean> touch_6 = IsPtWall_XY(x + R , y     );
	var<boolean> touch_8 = IsPtWall_XY(x     , y - R );
	var<boolean> touch_2 = IsPtWall_XY(x     , y + R );

	var<double> N = R / Math.SQRT2;

	var<boolean> touch_1 = IsPtWall_XY(x - N, y + N);
	var<boolean> touch_3 = IsPtWall_XY(x + N, y + N);
	var<boolean> touch_7 = IsPtWall_XY(x - N, y - N);
	var<boolean> touch_9 = IsPtWall_XY(x + N, y - N);

	var ret =
	[
		null,
		touch_1,
		touch_2,
		touch_3,
		touch_4,
		null,
		touch_6,
		touch_7,
		touch_8,
		touch_9,
	];

	return ret;
}
