/*
	îwåi

	----

	108 = 9 * 12
	108 = 12 * 9
	108 = 18 * 6

	- - -

	108 = 18 * 6
	108 = 27 * 4
	108 = 36 * 3

*/

var<TaskManager_t> @@_Tasks = CreateTaskManager();
var<TaskManager_t> @@_Tasks_01 = CreateTaskManager();
var<TaskManager_t> @@_Tasks_02 = CreateTaskManager();
var<TaskManager_t> @@_Tasks_03 = CreateTaskManager();

function* <generatorForTask> BackgroundTask()
{
	var<Func boolean> f_Main = Supplier(@@_Main());

	for (; ; )
	{
		ExecuteAllTask(@@_Tasks);
		ExecuteAllTask(@@_Tasks_01);
		ExecuteAllTask(@@_Tasks_02);
		ExecuteAllTask(@@_Tasks_03);

		if (!f_Main())
		{
			error();
		}

		yield 1;
	}
}

function* <generatorForTask> @@_Main()
{
	AddTask(@@_Tasks, function* <generatorForTask> ()
	{
		for (; ; )
		{
			for (var<int> x = 0; x < 7; x++)
			{
				AddTask(@@_Tasks_01, function* <generatorForTask> ()
				{
					for (var<int> y = 0; ; y++)
					{
						var<double> dx = FIELD_L + 26 + x * 108;
						var<double> dy = FIELD_T - 54 + y * 9;

						dx += 26 * -1;

						if (FIELD_B + 54 < dy)
						{
							break;
						}

						Draw(P_Wall0001, dx, dy, 1.0, 0.0, 1.0);

						yield 1;
					}
				}());
			}

			yield* Wait(12);
		}
	}());

	AddTask(@@_Tasks, function* <generatorForTask> ()
	{
		for (; ; )
		{
			for (var<int> x = 0; x < 7; x++)
			{
				if (GetRand1() < 0.25)
				{
					continue;
				}

				AddTask(@@_Tasks_02, function* <generatorForTask> ()
				{
					for (var<int> y = 0; ; y++)
					{
						var<double> dx = FIELD_L + 26 + x * 108;
						var<double> dy = FIELD_T - 54 + y * 12;

						dx += 26 * 0;

						if (FIELD_B + 54 < dy)
						{
							break;
						}

						Draw(P_Wall0002, dx, dy, 1.0, 0.0, 1.0);

						yield 1;
					}
				}());
			}

			yield* Wait(9);
		}
	}());

	AddTask(@@_Tasks, function* <generatorForTask> ()
	{
		for (; ; )
		{
			for (var<int> x = 0; x < 7; x++)
			{
				if (GetRand1() < 0.5)
				{
					continue;
				}

				AddTask(@@_Tasks_03, function* <generatorForTask> ()
				{
					for (var<int> y = 0; ; y++)
					{
						var<double> dx = FIELD_L + 26 + x * 108;
						var<double> dy = FIELD_T - 54 + y * 18;

						dx += 26 * 1;

						if (FIELD_B + 54 < dy)
						{
							break;
						}

						Draw(P_Wall0003, dx, dy, 1.0, 0.0, 1.0);

						yield 1;
					}
				}());
			}

			yield* Wait(6);
		}
	}());

	for (; ; )
	{
		// none

		yield 1;
	}
}
