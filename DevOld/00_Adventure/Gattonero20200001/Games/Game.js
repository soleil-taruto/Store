/*
	ゲーム・メイン
*/

// カメラ位置(整数)
var<D2Point_t> Camera = CreateD2Point(0.0, 0.0);

// ゲーム用タスク
var<TaskManager_t> GameTasks = CreateTaskManager();

function* <generatorForTask> GameMain()
{
	FreezeInput();
	ClearAllActor();
	ClearAllTask(GameTasks);

	// test
	var<Picture_t> pBgList =
	[
		P_Bg_PC室,
		P_Bg_廊下,
		P_Bg_教室L,
		P_Bg_教室M,
		P_Bg_教室R,
		P_Bg_教室空,
		P_Bg_校舎裏,
		P_Bg_校門,
		P_Bg_玄関,
		P_Bg_階段,
		P_Bg_階段上,
	];
	var<int> pBgPos = 0;

	// test
	var<Jikantai_e> jikantai = Jikantai_e_ASAHIRU;

	for (; ; )
	{
		ClearScreen();

		if (GetInput_Pause() == 1)
		{
			break;
		}

		if (GetMouseDown() == -1)
		{
			if (Screen_W / 2 > GetMouseX())
			{
				pBgPos++;
				pBgPos %= pBgList.length;
			}
			else
			{
				jikantai++;
				jikantai %= 4;
			}
		}

		Draw(pBgList[pBgPos][jikantai], Screen_W / 2, Screen_H / 2, 1.0, 0.0, 1.0);

//		Draw(P_Bg_PC室[Jikantai_e_YUUGATA], Screen_W / 2, Screen_H / 2, 1.0, 0.0, 1.0);

		ExecuteAllActor();
		ExecuteAllTask(GameTasks);
		yield 1;
	}
	FreezeInput();
	ClearAllActor();
	ClearAllTask(GameTasks);
}
