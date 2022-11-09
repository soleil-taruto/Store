/*
	タスクマネージャー
*/

/@(ASTR)

/// TaskManager_t
{
	<boolean> ClearEveryFrameMode
	<int> LastProcFrame
	<generatorForTask[]> Tasks
}

@(ASTR)/

function <TaskManager_t> CreateTaskManager()
{
	return CreateTaskManager_CEFM(false);
}

function <TaskManager_t> CreateTaskManager_CEFM(<boolean> clearEveryFrameMode)
{
	var ret =
	{
		ClearEveryFrameMode: clearEveryFrameMode,
		LastProcFrame: -1,
		Tasks: [],
	};

	return ret;
}

/*
	全ての公開関数の最初に呼び出すこと。
*/
function <void> @@_Before(<TaskManager_t> info)
{
	if (info.ClearEveryFrameMode)
	{
		if (info.LastProcFrame != ProcFrame)
		{
			info.LastProcFrame = ProcFrame;
			info.Tasks = [];
		}
	}
}

function <void> AddTask(<TaskManager_t> info, <generatorForTask> task)
{
	@@_Before(info);

	info.Tasks.push(task);
}

function <int> GetTaskCount(<TaskManager_t> info)
{
	@@_Before(info);

	return info.Tasks.length;
}

function <void> ClearAllTask(<TaskManager_t> info)
{
	@@_Before(info);

	info.Tasks = [];
}

function <void> ExecuteAllTask(<TaskManager_t> info)
{
	@@_Before(info);

	for (var<int> index = 0; index < info.Tasks.length; index++)
	{
		var<generatorForTask> task = info.Tasks[index];

		if (NextVal(task))
		{
			// noop
		}
		else
		{
			info.Tasks[index] = null;
		}
	}
	RemoveFalse(info.Tasks);
}
