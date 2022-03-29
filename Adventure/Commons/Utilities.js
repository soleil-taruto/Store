/*
	ユーティリティ
*/

/*
	始点・終点・速度から XY-速度を得る。

	x: 始点-X
	y: 始点-Y
	targetX: 終点-X
	targetY: 終点-Y
	speed: 速度

	ret:
	{
		X: 速度-X
		Y: 速度-Y
	}
*/
function MakeXYSpeed(x, y, targetX, targetY, speed)
{
	var pt = AngleToPoint(GetAngle(targetX - x, targetY - y), speed);

	var ret =
	{
		X: pt.X,
		Y: pt.Y,
	};

	return ret;
}

/*
	原点から見て指定位置の角度を得る。
	角度は X-軸プラス方向を 0 度として時計回りのラジアン角である。
	但し X-軸プラス方向は右 Y-軸プラス方向は下である。
	例えば、真下は Math.PI / 2, 真左は真上は Math.PI * 1.5 になる。

	x: 指定位置-X
	y: 指定位置-Y

	ret: 角度(ラジアン角)
*/
function GetAngle(x, y)
{
	if (y < 0.0)
	{
		return Math.PI * 2.0 - GetAngle(x, -y);
	}
	if (x < 0.0)
	{
		return Math.PI - GetAngle(-x, y);
	}
	if (x <= 0.0)
	{
		return Math.PI / 2.0;
	}
	if (y <= 0.0)
	{
		return 0.0;
	}

	var r1 = 0.0;
	var r2 = Math.PI / 2.0;
	var t = y / x;
	var rm;
	var rmt;

	for (var c = 1; ; c++)
	{
		rm = (r1 + r2) / 2.0;

		if (10 <= c)
		{
			break;
		}

		rmt = Math.tan(rm);

		if (t < rmt)
		{
			r2 = rm;
		}
		else
		{
			r1 = rm;
		}
	}
	return rm;
}

/*
	角度と距離を元に原点から見た位置を得る。

	angle: 角度(ラジアン角)
	distance: 距離

	ret:
	{
		X: 位置-X
		Y: 位置-Y
	}
*/
function AngleToPoint(angle, distance)
{
	var ret =
	{
		X: distance * Math.cos(angle),
		Y: distance * Math.sin(angle),
	};

	return ret;
}

/*
	原点から指定位置までの距離を得る。

	x: 指定位置-X
	y: 指定位置-Y

	ret: 距離
*/
function GetDistance(x, y)
{
	return Math.sqrt(x * x + y * y);
}

/*
	現在値を目的値に近づけます。

	value: 現在値
	dest: 目的値
	rate: 0.0 ～ 1.0 (めっちゃ近づける ～ あんまり近づけない)

	ret: 近づけた後の値
*/
function Approach(value, dest, rate)
{
	value -= dest;
	value *= rate;
	value += dest;

	return value;
}

/*
	旧 forscene, DDSceneUtils.Create() と同様のもの

	使用例：
		for (var scene of CreateScene(30))
		{
			// ここへフレーム毎の処理を記述する。

			yield 1;
		}

		列挙回数：31
		列挙：
			{ Numer: 0, Denom: 30, Rem: 30, Rate: 0.0,    RemRate: 1.0     }
			{ Numer: 1, Denom: 30, Rem: 29, Rate: 1 / 30, RemRate: 29 / 30 }
			{ Numer: 2, Denom: 30, Rem: 28, Rate: 2 / 30, RemRate: 28 / 30 }
			...
			{ Numer: 28, Denom: 30, Rem: 2, Rate: 28 / 30, RemRate: 2 / 30 }
			{ Numer: 29, Denom: 30, Rem: 1, Rate: 29 / 30, RemRate: 1 / 30 }
			{ Numer: 30, Denom: 30, Rem: 0, Rate: 1.0,     RemRate: 0.0    }
*/
function* CreateScene(frameMax)
{
	for (var frame = 0; frame <= frameMax; frame++)
	{
		var rate = frame / frameMax;

		var scene =
		{
			Numer: frame,
			Denom: frameMax,
			Rate: rate,
			Rem: frameMax - frame,
			RemRate: 1.0 - rate,
		};

		yield scene;
	}
}
