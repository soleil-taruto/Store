/*
	ã§í ÅEÇªÇÃëº
*/

function <Func boolean> Supplier(<generatorForTask> task)
{
	var ret = function <Func boolean> ()
	{
		return task.next().value;
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

function <boolean> IsOutOfScreen(<D2Point_t> pt, <Number> margin)
{
	return IsOut(pt, CreateD4Rect(0, 0, Screen_W, Screen_H), margin);
}
