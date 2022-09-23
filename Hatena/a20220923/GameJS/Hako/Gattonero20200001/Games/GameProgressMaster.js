/*
	部屋(ステージ)選択メニュー
*/

var<int> @@_PANEL_L = 50; // 左上パネルの左座標
var<int> @@_PANEL_T = 50; // 左上パネルの上座標
var<int> @@_PANEL_W = 185;
var<int> @@_PANEL_H = 160;
var<int> @@_PANEL_X_GAP = 20;
var<int> @@_PANEL_Y_GAP = 20;
var<int> @@_PANEL_X_NUM = 4;
var<int> @@_PANEL_Y_NUM = 4;

function* <generatorForTask> GameProgressMaster()
{
	var<int> selectX = 0;
	var<int> selectY = 0;

	SetCurtain();
	FreezeInput();

	Play(M_Title);

	var<double> curtain_wl_dest = -0.5;
	var<double> curtain_wl = 0.0;

	for (; ; )
	{
		if (DEBUG && GetKeyInput(85) == 1) // ? U -> 全ステージ開放 -- (デバッグ用)
		{
			AlreadyClearedStageIndex = GetMapCount() - 1;
			SaveLocalStorage();
			SE(S_Dead);
		}

		var<int> canPlayIndex = AlreadyClearedStageIndex;

		if (canPlayIndex == -1)
		{
			canPlayIndex = 1;
		}
		else
		{
			canPlayIndex++;
		}

		var<int> playIndex = -1; // -1 == 無効

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
					playIndex = index;
				}

				index++;
			}
		}
		if (IsPound(GetInput_2()))
		{
			selectY++;
		}
		if (IsPound(GetInput_4()))
		{
			selectX--;
		}
		if (IsPound(GetInput_6()))
		{
			selectX++;
		}
		if (IsPound(GetInput_8()))
		{
			selectY--;
		}
		if (IsPound(GetInput_A()))
		{
			playIndex = selectX + selectY * @@_PANEL_Y_NUM + 1;
		}
		if (IsPound(GetInput_B()))
		{
			break; // タイトルへ戻る
		}

		if (playIndex != -1)
		{
			var<int> index = playIndex;

			if (canPlayIndex < index) // ? プレイ不可 -- 直前のステージをクリアしていない。
			{
				// noop
			}
			else // ? プレイ可能
			{
				yield* @@_Game(index);

				index = GameLastPlayedStageIndex - 1;

				selectX = index % @@_PANEL_X_NUM;
				selectY = ToFix(index / @@_PANEL_Y_NUM);
			}
		}

		selectX += @@_PANEL_X_NUM;
		selectX %= @@_PANEL_X_NUM;

		selectY += @@_PANEL_Y_NUM;
		selectY %= @@_PANEL_Y_NUM;

		curtain_wl = Approach(curtain_wl, curtain_wl_dest, 0.99);

		// 描画ここから

		DrawTitleBackground();
		DrawCurtain(curtain_wl);

		var<int> index = 1;

		for (var<int> y = 0; y < @@_PANEL_Y_NUM; y++)
		for (var<int> x = 0; x < @@_PANEL_X_NUM; x++)
		{
			var<int> l = @@_PANEL_L + x * (@@_PANEL_W + @@_PANEL_X_GAP);
			var<int> t = @@_PANEL_T + y * (@@_PANEL_H + @@_PANEL_Y_GAP);

			if (x == selectX && y == selectY)
			{
				if (canPlayIndex < index)
				{
					SetColor("#808000c0");
				}
				else
				{
					SetColor("#ffff00e0");
				}
			}
			else
			{
				if (canPlayIndex < index)
				{
					SetColor("#808080c0");
				}
				else
				{
					SetColor("#ffffffe0");
				}
			}

			PrintRect(l, t, @@_PANEL_W, @@_PANEL_H);
			SetColor("#000000");
			SetPrint(l + 45, t + 110, 0);
			SetFSize(80);
			PrintLine(ZPad(index, 2, "0"));

			index++;
		}

		SetColor("#ffffff");
		SetPrint(Screen_W - 450, Screen_H - 15, 0);
		SetFSize(20);
		PrintLine("カーソル：選択　Ｚ：決定　Ｘ：タイトルに戻る");

		yield 1;
	}

	FreezeInput();
}

function* <generatorForTask> @@_Game(<int> startMapIndex)
{
	// Leave from menu
	{
		FreezeInput();
		Fadeout();
		SetCurtain_FD(30, -1.0);
		yield* Wait(40);
	}

gameBlock:
	{
		for (var<int> mapIndex = startMapIndex; mapIndex < GetMapCount(); mapIndex++)
		{
			yield* GameMain(mapIndex);

			if (GameEndReason == GameEndReason_e_RETURN_MENU)
			{
				break gameBlock;
			}

			AlreadyClearedStageIndex = Math.max(AlreadyClearedStageIndex, mapIndex);
			SaveLocalStorage();
		}
		yield* Ending();
	}

	// Return to menu
	{
		SetCurtain();
		FreezeInput();
		FreezeInputUntilRelease();

		Play(M_Title);
	}
}
