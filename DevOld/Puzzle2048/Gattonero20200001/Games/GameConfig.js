/*
	ゲーム・設定
*/

// ----
// ボタン押下イベント

// 上段

function <void> GameConfig_Press_0101()
{
	Field_XNum++;
	@@_FieldXYNumChanged();
}

function <void> GameConfig_Press_0201()
{
	Field_YNum++;
	@@_FieldXYNumChanged();
}

function <void> GameConfig_Press_0301()
{
	NewPanelExponentLmt++;
	@@_NewPanelExponentLmtChanged();
}

function <void> GameConfig_Press_0401()
{
	AutoMode = !AutoMode || AutoMode_CCW;
	AutoMode_CCW = false;
}

// 下段

function <void> GameConfig_Press_0102()
{
	Field_XNum--;
	@@_FieldXYNumChanged();
}

function <void> GameConfig_Press_0202()
{
	Field_YNum--;
	@@_FieldXYNumChanged();
}

function <void> GameConfig_Press_0302()
{
	NewPanelExponentLmt--;
	@@_NewPanelExponentLmtChanged();
}

function <void> GameConfig_Press_0402()
{
	AutoMode = !AutoMode || !AutoMode_CCW;
	AutoMode_CCW = true;
}

// ----

function <void> @@_FieldXYNumChanged()
{
	Field_XNum = ToRange(Field_XNum, 7, 12);
	Field_YNum = ToRange(Field_YNum, 7, 12);

	// フィールド縦横セル数変更による、各種定数の更新
	{
		Field_W = Cell_W * Field_XNum;
		Field_H = Cell_H * Field_YNum;

		Field_L = GameArea_L + ToInt((GameArea_W - Field_W) / 2);
		Field_T = GameArea_T + ToInt((GameArea_H - Field_H) / 2);

		var<Panel_t[][]> rTable = Game_GetTable();
		var<Panel_t[][]> wTable = [];

		for (var<int> x = 0; x < Field_XNum; x++)
		{
			wTable[x] = [];

			for (var<int> y = 0; y < Field_YNum; y++)
			{
				var<Panel_t> panel = null;

				if (x < rTable.length && y < rTable[x].length)
				{
					panel = rTable[x][y];
				}
				wTable[x][y] = panel;
			}
		}
		Game_SetTable(wTable);
	}
}

function <void> @@_NewPanelExponentLmtChanged()
{
	NewPanelExponentLmt = ToRange(NewPanelExponentLmt, 1, 33);
//	NewPanelExponentLmt = ToRange(NewPanelExponentLmt, 1, P_数字パネル.length);
}
