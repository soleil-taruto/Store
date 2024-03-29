/*
	当たり判定
*/

/@(ASTR)

/// Crash_t
{
	// 当たり判定の種類
	// 1 == 円形
	// 2 == 矩形
	//
	<int> Kind
}

@(ASTR)/

function <Crash_t> CreateCrash_Circle(<double> x, <double> y, <double> r)
{
	var ret =
	{
		Kind: 1,

		// 以下円形固有

		<double> X: x, // 中心の X-座標
		<double> Y: y, // 中心の Y-座標
		<double> R: r, // 半径
	};

	return ret;
}

function <Crash_t> CreateCrash_Rect(<D4Rect_t> rect)
{
	var ret =
	{
		Kind: 2,

		// 以下矩形固有

		<D4Rect_t> Rect: rect, // 領域
	};

	return ret;
}

var<double> LastCrashed_矩形の角から見た円形_Angle = null;

function <boolean> IsCrashed(<Crash_t> a, <Crash_t> b)
{
	// reset
	{
		LastCrashed_矩形の角から見た円形_Angle = null;
	}

	if (a == null || b == null)
	{
		return false;
	}

	if (b.Kind < a.Kind)
	{
		var tmp = a;
		a = b;
		b = tmp;
	}

	// この時点で a.Kind <= b.Kind となっている。

	if (a.Kind == 1 && b.Kind == 1) // ? 円形 vs 円形
	{
		var<double> d = GetDistance(a.X - b.X, a.Y - b.Y);

		return d < a.R + b.R;
	}
	if (a.Kind == 1 && b.Kind == 2) // ? 円形 vs 矩形
	{
		var<double> x = a.X;
		var<double> y = a.Y;
		var<double> rad = a.R;

		var<double> l2 = b.Rect.L;
		var<double> t2 = b.Rect.T;
		var<double> r2 = b.Rect.L + b.Rect.W;
		var<double> b2 = b.Rect.T + b.Rect.H;

		if (x < l2) // 左
		{
			if (y < t2) // 左上
			{
				return @@_IsCrashed_Circle_Point(x, y, rad, l2, t2);
			}
			else if (b2 < y) // 左下
			{
				return @@_IsCrashed_Circle_Point(x, y, rad, l2, b2);
			}
			else // 左中段
			{
				return l2 < x + rad;
			}
		}
		else if (r2 < x) // 右
		{
			if (y < t2) // 右上
			{
				return @@_IsCrashed_Circle_Point(x, y, rad, r2, t2);
			}
			else if (b2 < y) // 右下
			{
				return @@_IsCrashed_Circle_Point(x, y, rad, r2, b2);
			}
			else // 右中段
			{
				return x - rad < r2;
			}
		}
		else // 真上・真ん中・真下
		{
			return t2 - rad < y && y < b2 + rad;
		}
	}
	if (a.Kind == 2 && b.Kind == 2) // ? 矩形 vs 矩形
	{
		var<double> l1 = a.Rect.L;
		var<double> t1 = a.Rect.T;
		var<double> r1 = a.Rect.L + a.Rect.W;
		var<double> b1 = a.Rect.T + a.Rect.H;

		var<double> l2 = b.Rect.L;
		var<double> t2 = b.Rect.T;
		var<double> r2 = b.Rect.L + b.Rect.W;
		var<double> b2 = b.Rect.T + b.Rect.H;

		return l1 < r2 && l2 < r1 && t1 < b2 && t2 < b1;
	}
	error(); // 不明な組み合わせ
}

function <boolean> @@_IsCrashed_Circle_Point(<double> x, <double> y, <double> rad, <double> x2, <double> y2)
{
	LastCrashed_矩形の角から見た円形_Angle = GetAngle(x - x2, y - y2);

	var<double> d = GetDistance(x - x2, y - y2)

	return d < rad;
}
