using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		public MainWin()
		{
			Ground.I = new Ground();

			ReceiptInfoHelper.Load(ReceiptInfoHelper.RECEIPT_FILE);

			InitializeComponent();

			this.MinimumSize = this.Size;

			Ground.I.Refine支払先 = new 入力補助TBLB()
			{
				TB = this.支払先,
				LB = this.支払先候補,
				GetEntry = (int index) => Ground.I.ReceiptInfos[index].支払先,
			};

			Ground.I.Refine但し書き = new 入力補助TBLB()
			{
				TB = this.但し書き,
				LB = this.但し書き候補,
				GetEntry = (int index) => Ground.I.ReceiptInfos[index].但し書き,
			};

			Ground.I.Known但し書き = new Known但し書き()
			{
				TB = this.支払先,
				UILabel = this.Known但し書きText,
			};

			this.Known但し書きText.Text = "";
			this.ErrorMessage.Text = "";
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// nop
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			this.Reset();
			this.MainTimer.Enabled = true;
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// nop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.MainTimer.Enabled = false;
		}

		private void Reset()
		{
			this.日付.Text = "";
			this.金額.Text = "";
			this.支払先.Text = "";
			this.但し書き.Text = "";

			this.日付.Focus();
		}

		private void 日付_TextChanged(object sender, EventArgs e)
		{
			try
			{
				long timeStamp = int.Parse(this.日付.Text) * 1000000L;

				if (SCommon.TimeStampToSec.ToTimeStamp(SCommon.TimeStampToSec.ToSec(timeStamp)) != timeStamp)
					throw new Exception("不正な日付です。");

				string suffix = "";

				{
					int d = int.Parse(this.日付.Text.Substring(0, 4)) - SCommon.SimpleDateTime.Now().Year;

					if (d == 0)
					{
						suffix = "　(今年)";
					}
					else if (d == -1)
					{
						suffix = "　(昨年)";
					}
				}

				this.日付Preview.Text =
					"--->　" +
					Common.ToZenDigit(this.日付.Text.Substring(0, 4)) +
					"　年　" +
					Common.ToZenDigit(this.日付.Text.Substring(4, 2)) +
					"　月　" +
					Common.ToZenDigit(this.日付.Text.Substring(6, 2)) +
					"　日" +
					suffix;
			}
			catch (Exception ex)
			{
				this.日付Preview.Text = "--->　" + ex.Message;
			}
		}

		private void 金額_TextChanged(object sender, EventArgs e)
		{
			try
			{
				int value = int.Parse(this.金額.Text);

				if (value < 1)
					throw new Exception("不正な金額です。");

				StringBuilder buff = new StringBuilder();

				buff.Append("--->　");

				if (10000 <= value)
				{
					buff.Append(Common.ToZenDigit("" + (value / 10000)));
					buff.Append("　万　");
				}
				if (1000 <= value)
				{
					buff.Append(Common.ToZenDigit("" + ((value / 1000) % 10)));
					buff.Append("　， ");
					buff.Append(Common.ToZenDigit((value % 1000).ToString("D3")));
					buff.Append("　円");
				}
				else
				{
					buff.Append(Common.ToZenDigit("" + value));
					buff.Append("　円");
				}
				this.金額Preview.Text = buff.ToString();
			}
			catch (Exception ex)
			{
				this.金額Preview.Text = "--->　" + ex.Message;
			}
		}

		private void 支払先_TextChanged(object sender, EventArgs e)
		{
			Ground.I.Refine支払先.TextChanged();
		}

		private void 但し書き_TextChanged(object sender, EventArgs e)
		{
			Ground.I.Refine但し書き.TextChanged();
		}

		private void 支払先候補_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Gnd.I.Refine支払先.ListSelected(); // moved
		}

		private void 但し書き候補_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Gnd.I.Refine但し書き.ListSelected(); // moved
		}

		private void 日付_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				this.金額.Focus();
				e.Handled = true;
			}
		}

		private void 金額_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				this.支払先.Focus();
				e.Handled = true;
			}
		}

		private void 支払先_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				this.但し書き.Focus();
				e.Handled = true;
			}
		}

		private void 支払先_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Down)
			{
				Ground.I.Refine支払先.IntoList();
			}
		}

		private void 支払先候補_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				Ground.I.Refine支払先.ListSelected();
				e.Handled = true;
			}
		}

		private void 但し書き_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				this.EndInput();
				e.Handled = true;
			}
		}

		private void 但し書き_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Down)
			{
				Ground.I.Refine但し書き.IntoList();
			}
			else if (e.KeyData == Keys.PageUp)
			{
				this.但し書き.Text = Ground.I.Known但し書き.GetPrev(this.但し書き.Text);
				this.但し書き.SelectAll();
			}
			else if (e.KeyData == Keys.PageDown)
			{
				this.但し書き.Text = Ground.I.Known但し書き.GetNext(this.但し書き.Text);
				this.但し書き.SelectAll();
			}
		}

		private void 但し書き候補_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				Ground.I.Refine但し書き.ListSelected();
				e.Handled = true;
			}
		}

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			Ground.I.Refine支払先.EachTimer();
			Ground.I.Refine但し書き.EachTimer();
			Ground.I.Known但し書き.EachTimer();
		}

		private void EndInput()
		{
			this.ErrorMessage.Text = "";

			try
			{
				ReceiptInfo ri = new ReceiptInfo()
				{
					日付 = int.Parse(this.日付.Text),
					金額 = int.Parse(this.金額.Text),
					支払先 = this.支払先.Text,
					但し書き = this.但し書き.Text,
					Option = DateTime.Now.ToString(),
				};

				{
					long timeStamp = ri.日付 * 1000000L;

					if (SCommon.TimeStampToSec.ToTimeStamp(SCommon.TimeStampToSec.ToSec(timeStamp)) != timeStamp)
						throw new Exception("不正な日付です。");
				}

				if (ri.日付 < 20170101)
					throw new Exception("2017年より前の日付です。" + ri.日付);

				if (SCommon.SimpleDateTime.Now().ToTimeStamp() / 1000000L < ri.日付)
					throw new Exception("明日以降の日付です。" + ri.日付);

				if (ri.金額 < 1)
					throw new Exception("不正な金額です。" + ri.金額);

				if (SCommon.IMAX < ri.金額)
					throw null; // never

				if (ri.支払先 == "")
					throw new Exception("支払先を入力して下さい。");

				if (ri.但し書き == "")
					throw new Exception("但し書きを入力して下さい。");

				if (Common.名前フィルタ(ri.支払先) != ri.支払先)
					throw new Exception("支払先に不正な文字が含まれています。" + Common.名前フィルタ(ri.支払先));

				if (Common.名前フィルタ(ri.但し書き) != ri.但し書き)
					throw new Exception("但し書きに不正な文字が含まれています。" + Common.名前フィルタ(ri.但し書き));

				if (MessageBox.Show(
					this,
					"この内容で登録します。\r\n" +
					"日付：" + ri.日付 + "\r\n" +
					"金額：" + ri.金額 + "\r\n" +
					"支払先：" + ri.支払先 + "\r\n" +
					"但し書き：" + ri.但し書き,
					"登録確認",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Information
					)
					!= System.Windows.Forms.DialogResult.OK
					)
					throw new Exception("キャンセルしました。");

				// ここで確定！

				Ground.I.ReceiptInfos.Add(ri);

				ReceiptInfoHelper.Add(ReceiptInfoHelper.RECEIPT_FILE, ri);

				this.Reset();
			}
			catch (Exception e)
			{
				this.ErrorMessage.Text = e.Message;
			}
		}
	}
}
