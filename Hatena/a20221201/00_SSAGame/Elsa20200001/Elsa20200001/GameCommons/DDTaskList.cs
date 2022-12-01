using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons
{
	public class DDTaskList
	{
		private DDList<Func<bool>> Tasks = new DDList<Func<bool>>();

		public void Add(Func<bool> task)
		{
			this.Tasks.Add(task);
		}

		public void ExecuteAllTask()
		{
			for (int index = 0; index < this.Tasks.Count; index++)
				if (!this.Tasks[index]())
					this.Tasks[index] = null;

			this.Tasks.RemoveAll(task => task == null);
		}

		public void ExecuteAllTask_Reverse()
		{
			for (int index = this.Tasks.Count - 1; 0 <= index; index--)
				if (!this.Tasks[index]())
					this.Tasks[index] = null;

			this.Tasks.RemoveAll(task => task == null);
		}

		public void Clear()
		{
			this.Tasks.Clear();
		}

		public int Count
		{
			get
			{
				return this.Tasks.Count;
			}
		}

		// ====
		// ここから便利機能
		// ====

		public void Delay(int delayFrame, Action routine)
		{
			int endFrame = DDEngine.ProcFrame + delayFrame;

			this.Add(() =>
			{
				if (DDEngine.ProcFrame < endFrame)
					return true;

				routine();
				return false;
			});
		}

		public void Keep(int keepFrame, Action routine)
		{
			int endFrame = DDEngine.ProcFrame + keepFrame;

			this.Add(() =>
			{
				routine();
				return DDEngine.ProcFrame < endFrame;
			});
		}

		public void Once(Action routine)
		{
			this.Add(() =>
			{
				routine();
				return false;
			});
		}
	}
}
