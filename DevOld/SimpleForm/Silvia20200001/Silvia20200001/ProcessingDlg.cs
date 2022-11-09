using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public partial class ProcessingDlg : Form
	{
		#region WndProc

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				this.BeginInvoke((MethodInvoker)this.CloseWindow);
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public ProcessingDlg()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void ProcessingDlg_Load(object sender, EventArgs e)
		{
			// none
		}

		private void ProcessingDlg_Shown(object sender, EventArgs e)
		{
			this.Busyness = 0;
			PostShown.Perform(this);
		}

		private int BlockAltF4MessageKeepCountDown = 0;

		private void CloseWindow()
		{
			if (1 <= this.Busyness)
				return;

			if (this.ProgressPct != -1)
			{
				this.BlockAltF4Message.Visible = true;
				this.BlockAltF4MessageKeepCountDown = 10;
				return;
			}

			this.Busyness = SCommon.IMAX;
			this.Close();
			return;
		}

		private int Busyness = SCommon.IMAX;

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			if (1 <= this.Busyness)
				return;

			if (1 <= this.BlockAltF4MessageKeepCountDown)
			{
				this.BlockAltF4MessageKeepCountDown--;

				if (this.BlockAltF4MessageKeepCountDown == 0)
					this.BlockAltF4Message.Visible = false;
			}

			if (this.ProgressPct != -1)
			{
				this.ProgressPct = Math.Min(100, this.ProgressPct + 2);

				{
					int value = (this.ProgressPct / 20) * 20;

					if (this.ProgressBar.Value != value)
						this.ProgressBar.Value = value;
				}

				if (this.ProgressPct == 100)
				{
					this.ProgressPct = -1;
					this.StartBtn.Enabled = true;
				}
			}
		}

		/// <summary>
		/// 進捗パーセント値
		/// -1 == 停止中
		/// 0 ～ 100 == 実行中
		/// </summary>
		private int ProgressPct = -1;

		private void StartBtn_Click(object sender, EventArgs e)
		{
			this.StartBtn.Enabled = false;
			this.ProgressPct = 0;
		}
	}
}
