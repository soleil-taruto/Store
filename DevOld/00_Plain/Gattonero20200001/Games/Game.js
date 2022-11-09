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

	var<Actor_t> card = CreateActor_Trump(Screen_W + 300, -300, 1, 1, false);
	SetTrumpDest(card, Screen_W / 2.0, Screen_H / 2.0);
	AddActor(card);

	for (; ; )
	{
		ClearScreen();

		if (GetInput_Pause() == 1)
		{
			break;
		}

		if (GetMouseDown() == -1)
		{
			SetTrumpReversed(card, !IsTrumpReversed(card));
		}

		ExecuteAllActor();
		ExecuteAllTask(GameTasks);
		yield 1;
	}
	FreezeInput();
	ClearAllActor();
	ClearAllTask(GameTasks);
}
