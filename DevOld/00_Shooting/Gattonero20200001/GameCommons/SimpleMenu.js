/*
	シンプル・メニュー
*/

var<int> @@_ITEM_H = 30;
var<int> @@_TEXT_T = 22;
var<int> @@_TEXT_FONT_SIZE = 20;

var<int> DSM_Desided = false;

function <int> DrawSimpleMenu(<int> selectIndex, <int> x, <int> y, <int> yStep, <string[]> items)
{
	// reset
	{
		DSM_Desided = false;
	}

	if (GetMouseDown() == -1)
	{
		var<double> mx = GetMouseX();
		var<double> my = GetMouseY();

		for (var<int> index = 0; index < items.length; index++)
		{
			if (!IsOut(
				CreateD2Point(mx, my),
				CreateD4Rect(0, y + yStep * index, Screen_W, @@_ITEM_H),
				0.0
				))
			{
				selectIndex = index;
				DSM_Desided = true;
			}
		}
	}
	if (IsPound(GetInput_8()))
	{
		selectIndex--;
	}
	if (IsPound(GetInput_2()))
	{
		selectIndex++;
	}
	if (GetInput_A() == 1)
	{
		DSM_Desided = true;
	}
	if (GetInput_B() == 1)
	{
		if (selectIndex == items.length - 1)
		{
			DSM_Desided = true;
		}
		else
		{
			selectIndex = items.length - 1;
		}
	}

	selectIndex += items.length;
	selectIndex %= items.length;

	// 描画ここから

	for (var<int> index = 0; index < items.length; index++)
	{
		if (index == selectIndex)
		{
			SetColor("#ffff00");
		}
		else
		{
			SetColor("#ffffff");
		}
		PrintRect(0, y + yStep * index, Screen_W, @@_ITEM_H);
		SetColor("#000000");
		SetPrint(x, y + yStep * index + @@_TEXT_T, 0);
		SetFSize(@@_TEXT_FONT_SIZE);
		PrintLine((index == selectIndex ? "[>] " : "[ ] ") + items[index]);
	}
	return selectIndex;
}
