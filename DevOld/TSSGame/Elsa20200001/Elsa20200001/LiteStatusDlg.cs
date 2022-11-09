using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Windows.Forms;
using Charlotte.GameCommons;

namespace Charlotte
{
	public partial class LiteStatusDlg : Form
	{
		#region ALT_F4 抑止

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
				return;

			base.WndProc(ref m);
		}

		#endregion

		private static LiteStatusDlg Dlg = null;

		public static void StartDisplay(string message)
		{
			EndDisplay();

			MainWin.I.BeginInvoke((MethodInvoker)delegate
			{
				Dlg = new LiteStatusDlg();
				Dlg.Prm_StatusMessage = message;
				Dlg.Show();
			});
		}

		public static void EndDisplayDelay()
		{
#if true
			DDGround.SystemTasks.Delay(DDConfig.LOG_ENABLED ? 3 : 30, EndDisplay);
#else // old same
			int DELAY_FRAME = DDConfig.LOG_ENABLED ? 3 : 30;
			int endFrame = DDEngine.ProcFrame + DELAY_FRAME;

			DDGround.SystemTasks.Add(() =>
			{
				if (DDEngine.ProcFrame < endFrame)
					return true;

				EndDisplay();
				return false;
			});
#endif
		}

		public static void EndDisplay()
		{
			MainWin.I.BeginInvoke((MethodInvoker)delegate
			{
				if (Dlg != null)
				{
					Dlg.Close();
					Dlg.Dispose(); // 2bs -- .Show() の場合は Dispose 不要である。
					Dlg = null;
				}
			});
		}

		private string Prm_StatusMessage;

		public LiteStatusDlg()
		{
			InitializeComponent();

			float fontSize;

			if (
				DDGround.MonitorRect.W < 1920 ||
				DDGround.MonitorRect.H < 1080
				)
				fontSize = 24f;
			else
				fontSize = 48f;

			this.BackColor = Color.FromArgb(0, 64, 64); // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			this.FormBorderStyle = FormBorderStyle.None; // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			this.StatusMessage
				.Font = new Font( // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
					"メイリオ",
					fontSize
					);
			this.StatusMessage
				.ForeColor = Color.White; // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
		}

		private void LiteStatusDlg_Load(object sender, EventArgs e)
		{
			// nop
		}

		private void LiteStatusDlg_Shown(object sender, EventArgs e)
		{
			this.StatusMessage.Text = this.Prm_StatusMessage;

			const int MARGIN = 30;

			this.Width = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				DDGround.MonitorRect.W;
			this.Height = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				MARGIN + this.StatusMessage
				.Height + // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				MARGIN;
			this.Left = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				DDGround.MonitorRect.L;
			this.Top = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				(DDGround.MonitorRect.H -
				this.Height) / 2; // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			this.StatusMessage
				.Left = (this.Width - this // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				.StatusMessage
				.Width) / 2; // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			this.StatusMessage
				.Top = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
					MARGIN;
		}

		private void LiteStatusDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// nop
		}

		private void LiteStatusDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			// nop
		}
	}
}
