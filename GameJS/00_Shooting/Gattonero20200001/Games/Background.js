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

var<int> BackgroundPhase = 0;

var<Func boolean> Task_01;
var<Func boolean> Task_02;

var<double> Alpha = 1.0;

function* <generatorForTask> CreateBackgroundTask()
{
	Task_01 = function() { return true; };
	Task_02 = function() { return true; };

	var<int> currBgPh = -1;
	var<double> a = 0.0;

	for (; ; )
	{
		if (currBgPh != BackgroundPhase)
		{
			var<generatorForTask> task;

			switch (BackgroundPhase)
			{
			case 0: task = @@_Stage01(); break;
			case 1: task = @@_Stage01Boss(); break;
			case 2: task = @@_Stage02(); break;
			case 3: task = @@_Stage02Boss(); break;
			case 4: task = @@_Stage03(); break;
			case 5: task = @@_Stage03Boss(); break;
			case 6: task = @@_Ending(); break;

			default:
				error();
			}

			Task_01 = Task_02;
			Task_02 = Supplier(task);

			currBgPh = BackgroundPhase;
			a = 0.0;
		}

		a = Approach(a, 1.0, 0.99);

		Alpha = 1.0;
		Task_01();
		Alpha = a;
		Task_02();

		yield 1;
	}
}

function* <generatorForTask> @@_Stage01()
{
	yield* @@_Field(P_Wall0001, 10.0);
}

function* <generatorForTask> @@_Stage02()
{
	yield* @@_Field(P_Wall0002, 17.0);
}

function* <generatorForTask> @@_Stage03()
{
	yield* @@_Field(P_Wall0003, 30.0);
}

function* <generatorForTask> @@_Field(<Picture_t> picture, <double> speedMax)
{
	var<double> speed = 0.0;
	var<double> t = 0.0;

	for (var<int> frame = 0; ; frame++)
	{
		speed = Approach(speed, speedMax, 0.997);
		t += speed;

		while (0.0 < t)
		{
			t -= 108.0;
		}

		for (var<int> x = 0; x < 8; x++)
		for (var<int> y = 0;      ; y++)
		{
			var<double> dx = 22.0 + x * 108.0;
			var<double> dy = t    + y * 108.0;

			if (Screen_H + 54.0 < dy)
			{
				break;
			}

			Draw(picture, dx, dy, Alpha, 0.0, 1.0);
		}

		yield 1;
	}
}

function* <generatorForTask> @@_Stage01Boss()
{
	yield* @@_Boss([ P_Wall0001, P_Wall0002, P_Wall0003 ]);
}

function* <generatorForTask> @@_Stage02Boss()
{
	yield* @@_Boss([ P_Wall0002, P_Wall0003, P_Wall0001 ]);
}

function* <generatorForTask> @@_Stage03Boss()
{
	yield* @@_Boss([ P_Wall0003, P_Wall0001, P_Wall0002 ]);
}

function* <generatorForTask> @@_Boss(<Picture_t[]> pictures)
{
	var<double> lw = 0.0;
	var<double> md = 0.0;
	var<double> hi = 0.0;

	for (var<int> frame = 0; ; frame++)
	{
		lw += 10.0;
		md += 17.0;
		hi += 30.0;

		if (0.0 < lw) { lw -= 108.0; }
		if (0.0 < md) { md -= 108.0; }
		if (0.0 < hi) { hi -= 108.0; }

		@@_DrawVertTiles(54 + 99 * 0, lw, pictures[0]);
		@@_DrawVertTiles(54 + 99 * 7, lw, pictures[0]);
		@@_DrawVertTiles(54 + 99 * 1, md, pictures[1]);
		@@_DrawVertTiles(54 + 99 * 6, md, pictures[1]);
		@@_DrawVertTiles(54 + 99 * 2, hi, pictures[2]);
		@@_DrawVertTiles(54 + 99 * 5, hi, pictures[2]);
		@@_DrawVertTiles(54 + 99 * 3, hi, pictures[2]);
		@@_DrawVertTiles(54 + 99 * 4, hi, pictures[2]);

		yield 1;
	}
}

function <void> @@_DrawVertTiles(<double> dx, <double> t, <Picture_t> picture)
{
	for (var<double> dy = t; dy < Screen_W + 54.0; dy += 108.0)
	{
		Draw(picture, dx, dy, Alpha, 0.0, 1.0);
	}
}

function* <generatorForTask> @@_Ending()
{
	var<TaskManager_t> tasks = CreateTaskManager();
	var<double> rate = 0.0;

	for (; ; )
	{
		rate = Approach(rate, 0.4, 0.993);

		if (GetRand1() < rate)
		{
			AddTask(tasks, function* <generatorForTask> ()
			{
				var<Picture_t> picture = ChooseOne([ P_Wall0001, P_Wall0002, P_Wall0003 ]);
				var<double> dx = GetRand3(-54.0, Screen_W + 54.0);
				var<double> dy = Screen_H + 54.0;
				var<double> speed = GetRand3(3.0, 10.0);

				while (-54.0 < dy)
				{
					Draw(picture, dx, dy, Alpha * 0.5, 0.0, 1.0);

					dy -= speed;

					yield 1;
				}
			}());
		}

		SetColor(I4ColorToString(D4ColorToI4Color(CreateD4Color(0.0, 0.0, 0.0, Alpha))));
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		ExecuteAllTask(tasks);

		yield 1;
	}
}
