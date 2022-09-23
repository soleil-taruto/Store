/*
	当たり判定
*/

/@(ASTR)

/// Crash_t
{
	// 当たり判定の種類
	// 1 == 円形(点)
	// 2 == 矩形
	//
	<int> Kind
}

@(ASTR)/

// 当たり判定を生成 -- 点
function <Crash_t> CreateCrash_Point(<double> x, <double> y)
{
	return CreateCrash_Circle(x, y, MICRO);
}

// 当たり判定を生成 -- 円形
function <Crash_t> CreateCrash_Circle(<double> x, <double> y, <double> r)
{
	var ret =
	{
		Kind: 1,

		// ここから固有

		<double> X: x, // 中心の X-座標
		<double> Y: y, // 中心の Y-座標
		<double> R: r, // 半径
	};

	return ret;
}

// 当たり判定を生成 -- 矩形
function <Crash_t> CreateCrash_Rect(<D4Rect_t> rect)
{
	var ret =
	{
		Kind: 2,

		// ここから固有

		<D4Rect_t> Rect: rect, // 領域
	};

	return ret;
}

function <boolean> IsCrashed(<Crash_t> a, <Crash_t> b)
{
	/*
		Enemy_t.Crash, Shot_t.Crash が設定されずに当たり判定に突入する場合を想定して。
		当たり判定無しの場合 Crash は null のまま。
	*/
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
		return GetDistanceLessThan(a.X - b.X, a.Y - b.Y, a.R + b.R);
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
	return GetDistanceLessThan(x - x2, y - y2, rad);
}

/*
	当たり判定の描画 (デバッグ用)
	呼び出す前に SetColor をセットすること。
*/
function <void> DrawCrash(<Crash_t> crash)
{
	if (crash == null) // ? 当たり判定無し
	{
		// noop
	}
	else if (crash.Kind == 1) // ? 円形
	{
		PrintCircle(crash.X - Camera.X, crash.Y - Camera.Y, Math.max(3.0, crash.R));
	}
	else if (crash.Kind == 2) // ? 矩形
	{
		PrintRect(crash.Rect.L - Camera.X, crash.Rect.T - Camera.Y, crash.Rect.W, crash.Rect.H);
	}
	else
	{
		error(); // 不正な種類
	}
}
