/*
	部屋(ステージ)選択メニュー
*/

var<int> @@_PANEL_L = 50; // 左上パネルの左座標
var<int> @@_PANEL_T = 50; // 左上パネルの上座標
var<int> @@_PANEL_W = 200;
var<int> @@_PANEL_H = 120;
var<int> @@_PANEL_X_GAP = 30;
var<int> @@_PANEL_Y_GAP = 20;
var<int> @@_PANEL_X_NUM = 3;
var<int> @@_PANEL_Y_NUM = 3;

var<int> @@_RETURN_BUTTON_L = 50;
var<int> @@_RETURN_BUTTON_T = 480;
var<int> @@_RETURN_BUTTON_W = 500;
var<int> @@_RETURN_BUTTON_H = 100;

function* <generatorForTask> GameProgressMaster()
{
	SetCurtain();
	FreezeInput();

	// 選択している項目インデックス
	// 0 == タイトルへ戻る
	// 1 ～ (GetMapCount() - 1) == ステージ・インデックス
	//
	var<int> selectIndex = 1;

	for (; ; )
	{
		if (DEBUG && GetKeyInput(85) == 1) // ? U -> 全ステージ開放 -- (デバッグ用)
		{
			CanPlayStageIndex = GetMapCount() - 1;
			SaveLocalStorage();
			SE(S_Dead);
		}

		var<int> mapIndex = -1; // -1 == 無効

		if (GetMouseDown() == -1)
		{
			var<double> mx = GetMouseX();
			var<double> my = GetMouseY();

			var<int> index = 1;

			for (var<int> y = 0; y < @@_PANEL_Y_NUM; y++)
			for (var<int> x = 0; x < @@_PANEL_X_NUM; x++)
			{
				var<int> l = @@_PANEL_L + x * (@@_PANEL_W + @@_PANEL_X_GAP);
				var<int> t = @@_PANEL_T + y * (@@_PANEL_H + @@_PANEL_Y_GAP);
				var<int> w = @@_PANEL_W;
				var<int> h = @@_PANEL_H;

				if (!IsOut(
					CreateD2Point(mx, my),
					CreateD4Rect(l, t, w, h),
					0.0
					))
				{
					mapIndex = index;
				}

				index++;
			}

			// タイトルメニューに戻るボタン
			{
				var<int> l = @@_RETURN_BUTTON_L;
				var<int> t = @@_RETURN_BUTTON_T;
				var<int> w = @@_RETURN_BUTTON_W;
				var<int> h = @@_RETURN_BUTTON_H;

				if (!IsOut(
					CreateD2Point(mx, my),
					CreateD4Rect(l, t, w, h),
					0.0
					))
				{
					mapIndex = 0;
				}
			}
		}
		if (IsPound(GetInput_8()))
		{
			if (selectIndex == 0)
			{
				selectIndex = -1;
			}
			else
			{
				selectIndex -= @@_PANEL_X_NUM;

				if (selectIndex < 0)
				{
					selectIndex = 0;
				}
			}
		}
		if (IsPound(GetInput_2()))
		{
			if (selectIndex == 0)
			{
				selectIndex = 1;
			}
			else
			{
				selectIndex += @@_PANEL_X_NUM;

				if (GetMapCount() <= selectIndex)
				{
					selectIndex = 0;
				}
			}
		}
		if (IsPound(GetInput_4()))
		{
			selectIndex--;
		}
		if (IsPound(GetInput_6()))
		{
			selectIndex++;
		}
		if (IsPound(GetInput_A()))
		{
			mapIndex = selectIndex;
		}
		if (IsPound(GetInput_B()))
		{
			if (selectIndex == 0)
			{
				break;
			}
			selectIndex = 0;
		}

		if (mapIndex != -1)
		{
			if (mapIndex == 0) // タイトルメニューへ戻る。
			{
				break;
			}

			if (CanPlayStageIndex < mapIndex) // ? プレイ不可 -- 直前のステージをクリアしていない。
			{
				// noop
			}
			else // ? プレイ可能
			{
				yield* @@_Game(mapIndex);

				selectIndex = @@_LastMapIndex;
			}
		}

		{
			var<int> LIMIT = GetMapCount();

			selectIndex += LIMIT;
			selectIndex %= LIMIT;
		}

		// 描画ここから

		@@_DrawWall();

		mapIndex = 1;

		for (var<int> y = 0; y < @@_PANEL_Y_NUM; y++)
		for (var<int> x = 0; x < @@_PANEL_X_NUM; x++)
		{
			var<int> l = @@_PANEL_L + x * (@@_PANEL_W + @@_PANEL_X_GAP);
			var<int> t = @@_PANEL_T + y * (@@_PANEL_H + @@_PANEL_Y_GAP);

			if (selectIndex == mapIndex)
			{
				if (CanPlayStageIndex < mapIndex)
				{
					SetColor("#808000");
				}
				else
				{
					SetColor("#ffff00");
				}
			}
			else
			{
				if (CanPlayStageIndex < mapIndex)
				{
					SetColor("#808080");
				}
				else
				{
					SetColor("#ffffff");
				}
			}

			PrintRect(l, t, @@_PANEL_W, @@_PANEL_H);
			SetColor("#000000");
			SetPrint(l + 50, t + 90, 0);
			SetFSize(80);
			PrintLine(ZPad(mapIndex, 2, "0"));

			mapIndex++;
		}

		// タイトルメニューに戻るボタン
		{
			if (selectIndex == 0)
			{
				SetColor("#ffff00");
			}
			else
			{
				SetColor("#ffffff");
			}

			PrintRect(
				@@_RETURN_BUTTON_L,
				@@_RETURN_BUTTON_T,
				@@_RETURN_BUTTON_W,
				@@_RETURN_BUTTON_H
				);
			SetColor("#000000");
			SetPrint(@@_RETURN_BUTTON_L + 20, @@_RETURN_BUTTON_T + 75, 0);
			SetFSize(60);
			PrintLine("タイトルへ戻る");
		}

		@@_DrawFront();

		yield 1;
	}

	FreezeInput();
}

/*
	この画面から離れる時のモーション
*/
function* <generatorForTask> @@_LeaveMotion()
{
	FreezeInput();
	Fadeout();
	SetCurtain_FD(30, -1.0);

	for (var<Scene_t> scene of CreateScene(40))
	{
		@@_DrawWall();
		@@_DrawFront();

		yield 1;
	}

	ClearAllEffect();
}

/*
	この画面へ戻って来る時のモーション
*/
function* <generatorForTask> @@_ReturnMotion()
{
	SetCurtain();
	FreezeInput();
	FreezeInputUntilRelease();

	Play(M_Title);
}

/*
	背景描画
*/
function <void> @@_DrawWall()
{
	SetColor("#004060");
	PrintRect(0, 0, Screen_W, Screen_H);
}

/*
	前面描画
*/
function <void> @@_DrawFront()
{
	// none
}

var<int> @@_LastMapIndex;

function* <generatorForTask> @@_Game(<int> startMapIndex)
{
	yield* @@_LeaveMotion();

gameBlock:
	{
		for (var<int> mapIndex = startMapIndex; mapIndex < GetMapCount(); mapIndex++)
		{
			@@_LastMapIndex = mapIndex;
			yield* GameMain(mapIndex);

			if (GameEndReason == GameEndReason_e_RETURN_MENU)
			{
				break gameBlock;
			}

			CanPlayStageIndex = Math.max(CanPlayStageIndex, mapIndex + 1);
			SaveLocalStorage();
		}
		yield* Ending();
	}

	yield* @@_ReturnMotion();
}
