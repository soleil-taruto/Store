/*
	タイトル画面の背景
*/

var<generatorForTask> @@_Task = null;

function <void> DrawTitleBackground()
{
	if (@@_Task == null)
	{
		@@_Task = @@_CreateTask();
	}

	if (!NextVal(@@_Task))
	{
		error();
	}
}

function* <generatorForTask> @@_CreateTask()
{
	var<TaskManager_t> tasks = CreateTaskManager();

	AddTask(tasks, @@_CreateTask_01("#404060ff"));
	AddTask(tasks, @@_CreateTask_02(40, 40, 2, "#606090ff"));
	AddTask(tasks, @@_CreateTask_02(50, 50, 3, "#ffff0080"));
	AddTask(tasks, @@_CreateTask_02(70, 70, 5, "#ff800050"));

	for (; ; )
	{
		ExecuteAllTask(tasks);

		yield 1;
	}
}

function* <generatorForTask> @@_CreateTask_01(<string> color)
{
	for (; ; )
	{
		SetColor(color);
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		yield 1;
	}
}

function* <generatorForTask> @@_CreateTask_02(<int> tile_w, <int> tile_h, <int> scrollSpeed, <string> color)
{
	var<int> TILE_W = tile_w;
	var<int> TILE_H = tile_h;
	var<int> t = 0;
	var<int> m = 0;

	for (; ; )
	{
		for (var<int> x = 0; TILE_W * x + 0 < Screen_W; x++)
		for (var<int> y = 0; TILE_H * y + t < Screen_H; y++)
		{
			if ((x + y + m) % 2 == 0)
			{
				SetColor(color);
				PrintRect(
					TILE_W * x + 0,
					TILE_H * y + t,
					TILE_W,
					TILE_H
					);
			}
		}

		t -= scrollSpeed;

		if (t <= -TILE_H)
		{
			t += TILE_H;
			m = 1 - m;
		}

		yield 1;
	}
}
