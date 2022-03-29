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
	角度は X-軸プラス方向を 0 度として時計回りのラジアン角です。
	但し X-軸プラス方向は右 Y-軸プラス方向は下です。
	例えば、真下は Math.PI / 2, 真上は Math.PI * 1.5 になります。

	x: 指定位置-X
	y: 指定位置-Y

	ret: 角度
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

	angle: 角度
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
