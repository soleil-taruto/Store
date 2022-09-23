/*
	配列
*/

function <void> InsertElement(<T[]> arr, <int> index, <T> element)
{
	if (index < 0 || arr.length < index)
	{
		error;
	}
	arr.push(-1);

	for (var<int> i = arr.length - 1; index < i; i--)
	{
		arr[i] = arr[i - 1];
	}
	arr[index] = element;
}

function <void> AddElement(<T[]> arr, <T> element)
{
	InsertElement(arr, arr.length, element);
}

function <void> AddElements(<T[]> arr, <T[]> elements)
{
	for (var<T> element of elements)
	{
		AddElement(arr, element);
	}
}

function <void> DesertElement(<T[]> arr, <int> index)
{
	if (index < 0 || arr.length <= index)
	{
		error;
	}
	var ret = arr[index];

	for (var<int> i = index; i < arr.length - 1; i++)
	{
		arr[i] = arr[i + 1];
	}
	arr.pop();
	return ret;
}

function <T> FastDesertElement(<T[]> arr, <int> index)
{
	if (index < 0 || arr.length <= index)
	{
		error;
	}
	var ret = arr[index];
	arr[index] = arr[arr.length - 1];
	arr.pop();
	return ret;
}

function <T> UnaddElement(<T[]> arr)
{
	return DesertElement(arr, arr.length - 1);
}

function <void> SwapElement(<T[]> arr, <int> index_A, <int> index_B)
{
	var<T> tmp = arr[index_A];
	arr[index_A] = arr[index_B];
	arr[index_B] = tmp;
}

function <void> Shuffle(<T[]> arr)
{
	for (var<int> i = arr.length - 1; 2 <= i; i--)
	{
		SwapElement(arr, GetRand(i), i);
	}
}

/*
	部分配列を得る。

	arr: 配列
	offset: 開始位置
	count: 長さ

	ret: 部分配列
*/
function <T[]> GetSubArray(<T[]> arr, <int> offset, <int> count)
{
	if (offset < 0 || arr.length < offset)
	{
		error;
	}
	if (count < 0 || arr.length - offset < count)
	{
		error;
	}
	var dest = [];

	for (var<int> index = 0; index < count; index++)
	{
		dest.push(arr[offset + index]);
	}
	return dest;
}

/*
	部分配列を得る。

	arr: 配列
	offset: 開始位置

	ret: 部分配列
*/
function <T[]> GetTrailArray(<T[]> arr, <int> offset)
{
	return GetSubArray(arr, offset, arr.length - offset);
}

/*
	配列を複製する。

	arr: 配列

	ret: 配列の複製
*/
function <T[]> CloneArray(<T[]> arr)
{
	return GetTrailArray(arr, 0);
}

/*
	マッチする要素を削除する。

	arr: 配列
	match: 判定メソッド
*/
function <void> RemoveAll(<T[]> arr, <Func T bool> match)
{
	var<int> w = 0;

	for (var<int> r = 0; r < arr.length; r++)
	{
		// マッチしなければ配列に残すので、w の指す位置へ書き込む。
		if (!match(arr[r]))
		{
			arr[w] = arr[r];
			w++;
		}
	}
	while (w < arr.length)
	{
		arr.pop();
	}
}

/*
	偽となる要素(false, null, undefined, 0, 空文字列)を削除する。

	arr: 配列
*/
function <void> RemoveFalse(<T[]> arr)
{
	var match = function(token)
	{
		return !token;
	};

	RemoveAll(arr, match);
}

/*
	配列の要素を全て削除する。

	arr: 配列
*/
function <void> ClearArray(<T[]> arr)
{
	arr.length = 0;
}

/*
	配列またはジェネレータを配列に変換する。
*/
function <T[]> ToArray(<T[]> src)
{
	var<T[]> dest = [];

	for (var<T> element of src)
	{
		dest.push(element);
	}
	return dest;
}

/*
	配列またはジェネレータをジェネレータに変換する。
*/
function* <T[]> ToGenerator(<T[]> src)
{
	for (var<T> element of src)
	{
		yield element;
	}
}

/*
	ジェネレータの次の値を取得する。
*/
function <T> NextVal(generator)
{
	return generator.next().value;
}
