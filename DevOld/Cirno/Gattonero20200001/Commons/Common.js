/*
	共通・その他
*/

function <Func boolean> Supplier(<generatorForTask> task)
{
	var ret = function <Func boolean> ()
	{
		return NextVal(task);
	};

	return ret;
}

function* <T[]> Repeat(<T> value, <int> count)
{
	for (var<int> index = 0; index < count; index++)
	{
		yield value;
	}
}

function* <generatorForTask> Wait(<int> count)
{
	yield* Repeat(1, count);
}

function <int> CountDown(<int> count)
{
	if (1 < count)
	{
		count--;
	}
	else if (count < -1)
	{
		count++;
	}
	else
	{
		count = 0;
	}

	return count;
}

function <boolean> IsOut(<D2Point_t> pt, <D4Rect_t> rect, <double> margin)
{
	var ret =
		pt.X < rect.L - margin || rect.L + rect.W + margin < pt.X ||
		pt.Y < rect.T - margin || rect.T + rect.H + margin < pt.Y;

	return ret;
}

function <boolean> IsOutOfScreen(<D2Point_t> pt, <double> margin)
{
	return IsOut(pt, CreateD4Rect(0.0, 0.0, Screen_W, Screen_H), margin);
}

function <boolean> IsOutOfCamera(<D2Point_t> pt, <double> margin)
{
	return IsOut(pt, CreateD4Rect(Camera.X, Camera.Y, Screen_W, Screen_H), margin);
}

/*
	始点から終点までの間の指定レートの位置を返す。

	a: 始点
	b: 終点
	rate: レート

	ret: 位置
*/
function <double> AToBRate(<double> a, <double> b, <double> rate)
{
	return a + (b - a) * rate;
}

/*
	始点から終点までの間の位置をレートに変換する。

	a: 始点
	b: 終点
	value: 位置

	ret: レート
*/
function <double> RateAToB(<double> a, <double> b, <double> value)
{
	return (value - a) / (b - a);
}

/*
	ラジアン角の正規化
*/
function <double> ToInRangeAngle(<double> angle)
{
	while (angle < 0.0)
	{
		angle += Math.PI * 2.0;
	}
	while (Math.PI * 2.0 < angle)
	{
		angle -= Math.PI * 2.0;
	}
	return angle;
}
