using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons
{
	public class DDActionList
	{
		private bool ClearEveryFrameMode;
		private long LastProcFrame = -1L;

		private DDList<Action> Routines = new DDList<Action>();

		public DDActionList(bool clearEveryFrameMode = false)
		{
			this.ClearEveryFrameMode = clearEveryFrameMode;
		}

		/// <summary>
		/// 全ての公開メソッドの最初で呼ぶこと。
		/// </summary>
		private void Before()
		{
			if (this.ClearEveryFrameMode)
			{
				if (this.LastProcFrame != DDEngine.ProcFrame)
				{
					this.LastProcFrame = DDEngine.ProcFrame;
					this.Routines.Clear();
				}
			}
		}

		public void Clear()
		{
			this.Before();
			this.Routines.Clear();
		}

		public void Add(Action routine)
		{
			this.Before();
			this.Routines.Add(routine);
		}

		public void ExecuteAllAction()
		{
			this.Before();

			foreach (Action routine in this.Routines.Iterate())
				routine();

			this.Routines.Clear();
		}
	}
}
