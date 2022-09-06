using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public partial class MainWin : Form
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

		public MainWin()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			Ground.I = new Ground();
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			Common.PostShown(this);

			this.Idling = true;
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.Idling = false; // 念のため -- this.CloseWindow()
		}

		/// <summary>
		/// フォームを閉じる。
		/// </summary>
		private void CloseWindow()
		{
			// ここからダミーなので消して下さい。-- ★要削除
			if (MessageBox.Show(
				"アプリケーションを終了します。",
				"確認",
				MessageBoxButtons.OKCancel,
				MessageBoxIcon.Warning
				) != DialogResult.OK
				)
			{
				return;
			}
			// ここまでダミーなので消して下さい。-- ★要削除

			this.Idling = false;
			this.Close();
		}

		/// <summary>
		/// タイマーイベントを発火して良いか
		/// -- 初期値：false
		/// -- フォームを表示し終えてから、フォームを閉る前まで true にする。
		/// -- その間、タイマーイベントを抑止したい時だけ false にする。
		/// </summary>
		private bool Idling = false;

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			if (!this.Idling)
				return;

			this.Idling = false;
			try
			{
				// none
			}
			finally
			{
				this.Idling = true;
			}
		}

		/// <summary>
		/// ダミーなので消して下さい。-- ★メソッド要削除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DummyButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(
				"アプリケーションを終了しますか？",
				"質問",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
				) == DialogResult.Yes
				)
			{
				this.CloseWindow();
				return;
			}

			MessageBox.Show("イベントは続行しました。");
		}
	}
}
