/*
	タスクマネージャー
*/

/@(ASTR)

/// TaskManager_t
{
	<generatorForTask[]> Tasks
}

@(ASTR)/

function <TaskManager_t> CreateTaskManager()
{
	var ret =
	{
		Tasks: [],
	};

	return ret;
}

function <void> AddTask(<TaskManager_t> info, <generatorForTask> task)
{
	info.Tasks.push(task);
}

function <int> GetTaskCount(<TaskManager_t> info)
{
	return info.Tasks.length;
}

function <void> ClearAllTask(<TaskManager_t> info)
{
	info.Tasks = [];
}

function <void> ExecuteAllTask(<TaskManager_t> info)
{
	for (var<int> index = 0; index < info.Tasks.length; index++)
	{
		var<generatorForTask> task = info.Tasks[index];

		if (task.next().value)
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
