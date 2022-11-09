/*
	•¶Žš—ñ
*/

function <string> ZPad(<T> value, <int> minlen, <string> padding)
{
	var ret = "" + value;

	while (ret.length < minlen)
	{
		ret = padding + ret;
	}
	return ret;
}

function <string> ToHex(<int> value)
{
	if (value < 0)
	{
		return "-" + ToHex(value * -1);
	}
	var ret = "";

	while (0 < value)
	{
		var<int> i = value % 16;
		ret += "0123456789abcdef".substring(i, i + 1);
		value /= 16;
		value = ToFix(value);
	}
	if (ret == "")
	{
		ret = "0";
	}
	return ret;
}
