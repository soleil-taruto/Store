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

			this.RandStatus.Text = "";
			this.MessageLabel.Text = "";

			this.Combo桁数.SelectedIndex = 3;
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

		private void RandStatus_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void RandText_TextChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void RandText_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 1) // ctrl_a
			{
				this.RandText.SelectAll();
				e.Handled = true;
			}
		}

		private void Label桁数_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void Label行数_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void Combo桁数_SelectedIndexChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void Numb行数_ValueChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void MessageLabel_Click(object sender, EventArgs e)
		{
			// noop
		}

		/// <summary>
		/// 生成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnGenRand_Click(object sender, EventArgs e)
		{
			int rowcnt = (int)this.Numb行数.Value;
			int colcnt;

			switch ((int)this.Combo桁数.SelectedIndex)
			{
				case 0: colcnt = 16; break;
				case 1: colcnt = 32; break;
				case 2: colcnt = 48; break;
				case 3: colcnt = 64; break;

				default:
					throw null; // never
			}

			// 乱数生成
			byte[][] cryptoRandBytesList = Enumerable.Range(0, rowcnt).Select(dummy => SCommon.CRandom.GetBytes(colcnt / 2)).ToArray();

			string text = SCommon.LinesToText(cryptoRandBytesList.Select(cryptoRandBytes => SCommon.Hex.ToString(cryptoRandBytes)).ToArray());
			this.RandText.Text = text;
			Clipboard.SetText(text);

			this.RandStatus.Text = string.Format(
				"行数：{0} , 桁数：{1}文字 ({2} バイト, {3} bit) , 長さ：{4}文字 ({5} バイト, {6} bit)",
				rowcnt,
				colcnt,
				colcnt / 2,
				colcnt * 4,
				rowcnt * colcnt,
				rowcnt * colcnt / 2,
				rowcnt * colcnt * 4
				);
			this.SetMessageLabel("生成した乱数をクリップボードにコピーしました。");
		}

		/// <summary>
		/// クリア
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnClear_Click(object sender, EventArgs e)
		{
			this.RandText.Text = "";
			Clipboard.Clear();

			this.RandStatus.Text = "";
			this.SetMessageLabel("クリップボードもクリアしました。");
		}

		private void SetMessageLabel(string message)
		{
			this.MessageLabel.Text = "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "] " + message;
		}

		private void Btn行数_5_Click(object sender, EventArgs e)
		{
			this.Numb行数.Value = 5;
		}

		private void Btn行数_32_Click(object sender, EventArgs e)
		{
			this.Numb行数.Value = 32;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (this.RandText.Text == "") // ? 生成_未実行
			{
				MessageBox.Show("先に生成を実行して下さい。", "なんか無理", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			string file = SaveCRandomFileDialog();

			if (file != null)
			{
				File.WriteAllText(file, this.RandText.Text, Encoding.ASCII);

				MessageBox.Show("ファイルを出力しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private static string SaveCRandomFileDialog()
		{
			string homeDir = Directory.GetCurrentDirectory();
			try
			{
				using (FileDialog dlg = new SaveFileDialog())
				{
					dlg.Title = "保存先";
					dlg.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
					dlg.FilterIndex = 1;
					dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					dlg.FileName = "Key_" + SCommon.SimpleDateTime.Now().ToTimeStamp() + ".txt";

					if (dlg.ShowDialog() == DialogResult.OK)
					{
						return dlg.FileName;
					}
				}
			}
			finally
			{
				Directory.SetCurrentDirectory(homeDir);
			}
			return null;
		}
	}
}
