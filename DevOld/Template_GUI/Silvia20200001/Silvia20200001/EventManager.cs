using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public class EventManager
	{
		private bool TimerStarted = false;
		private bool Busy = false;

		/// <summary>
		/// タイマー開始
		/// </summary>
		public void StartTimer()
		{
			this.TimerStarted = true;
		}

		/// <summary>
		/// タイマー終了
		/// </summary>
		public void EndTimer()
		{
			this.TimerStarted = false;
		}

		/// <summary>
		/// タイマーまたはバックグラウンド・イベントの実行
		/// </summary>
		/// <param name="routine">イベントロジック</param>
		public void FireOnTimerOrBackground(Action routine)
		{
			if (!this.TimerStarted)
				return;

			if (this.Busy)
				return;

			this.Busy = true;
			try
			{
				routine();
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
			finally
			{
				this.Busy = false;
			}
		}

		/// <summary>
		/// フォアグラウンド・イベントの実行
		/// </summary>
		/// <param name="routine">イベントロジック</param>
		public void FireOnForeground(Action routine)
		{
			if (this.Busy)
				return;

			this.Busy = true;
			try
			{
				routine();
			}
			catch (Exception e)
			{
				MessageBox.Show("" + e, "失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				this.Busy = false;
			}
		}
	}
}
