/*
	シンプル・メニュー
*/

var<int> @@_ITEM_H = 30;
var<int> @@_TEXT_L = 10;
var<int> @@_TEXT_T = 22;
var<int> @@_TEXT_FONT_SIZE = 20;

var<boolean> DSM_Desided = false;

/*
	メニューの入力処理と描画を行う。

	selectIndex: 選択中のインデックス (0 ～ (items.length - 1))
	l: メニュー表示位置・左座標
	t: メニュー表示位置・上座標
	item_w: アイテムの幅 (1～)
	margin: マージン (0～)
	items: アイテム・リスト

	ret: 新しい選択インデックス
		選択を確定した場合 DSM_Desided を true にセットする。
*/
function <int> DrawSimpleMenu(<int> selectIndex, <int> l, <int> t, <int> item_w, <int> margin, <string[]> items)
{
	return DrawSimpleMenu_CPNP(selectIndex, l, t, item_w, margin, false, false, items);
}

function <int> DrawSimpleMenu_CPNP(<int> selectIndex, <int> l, <int> t, <int> item_w, <int> margin, <boolean> cancelByPause, <boolean> noPound, <string[]> items)
{
	// reset
	{
		DSM_Desided = false;
	}

	var<int> mouseIndex = -1;

	{
		var<double> mx = GetMouseX();
		var<double> my = GetMouseY();

		for (var<int> index = 0; index < items.length; index++)
		{
			if (!IsOut(
				CreateD2Point(mx, my),
				CreateD4Rect(
					l + margin,
					t + margin + (@@_ITEM_H + margin) * index,
					item_w,
					@@_ITEM_H
					),
				0.0
				))
			{
				if (GetMouseDown() == -1)
				{
					selectIndex = index;
					DSM_Desided = true;
				}
				else
				{
					mouseIndex = index;
				}
				break;
			}
		}
	}

	if (noPound ? GetInput_8() == 1 : IsPound(GetInput_8()))
	{
		selectIndex--;
	}
	if (noPound ? GetInput_2() == 1 : IsPound(GetInput_2()))
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
	if (cancelByPause && GetInput_Pause() == 1)
	{
		DSM_Desided = true;
		selectIndex = items.length - 1;
	}

	selectIndex += items.length;
	selectIndex %= items.length;

	// 描画ここから

	SetColor("#00000080");
	PrintRect(l, t, margin + item_w + margin, margin + (@@_ITEM_H + margin) * items.length);

	for (var<int> index = 0; index < items.length; index++)
	{
		if (index == selectIndex)
		{
			SetColor("#ffff00c0");
			PrintRect(l + margin, t + margin + (@@_ITEM_H + margin) * index, item_w, @@_ITEM_H);
			SetColor("#000000");
		}
		else if (index == mouseIndex)
		{
			SetColor("#ffffff40");
			PrintRect(l + margin, t + margin + (@@_ITEM_H + margin) * index, item_w, @@_ITEM_H);
			SetColor("#ffffff");
		}
		else
		{
			SetColor("#ffffff");
		}
		SetPrint(l + margin + @@_TEXT_L, t + margin + (@@_ITEM_H + margin) * index + @@_TEXT_T, 0);
		SetFSize(@@_TEXT_FONT_SIZE);
		PrintLine((index == selectIndex ? "[>] " : "[ ] ") + items[index]);
	}
	return selectIndex;
}
