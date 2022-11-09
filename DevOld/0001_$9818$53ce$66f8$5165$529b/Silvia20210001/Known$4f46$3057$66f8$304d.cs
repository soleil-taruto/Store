using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public class Known但し書き
	{
		public TextBox TB;
		public Label UILabel;

		// <-- ここまで引数

		private string 支払先 = "";
		private List<string> Found但し書きList = new List<string>();
		private int ReceiptInfoIndex = 0;

		public void EachTimer()
		{
			{
				string text = this.TB.Text;

				if (this.支払先 != text)
				{
					this.支払先 = text;

					// Reset

					this.Found但し書きList.Clear();
					this.ReceiptInfoIndex = int.MaxValue;
					this.UpdateUI();
				}
			}

			for (int c = 0; c < 100; c++) // ループ回数
			{
				this.ReceiptInfoIndex = Math.Min(this.ReceiptInfoIndex, Ground.I.ReceiptInfos.Count);

				if (this.ReceiptInfoIndex <= 0)
					break;

				this.ReceiptInfoIndex--;
				ReceiptInfo info = Ground.I.ReceiptInfos[this.ReceiptInfoIndex];

				if (
					//this.支払先 != "" &&
					this.支払先 == info.支払先 &&
					this.Found但し書きList.Contains(info.但し書き) == false &&
					this.Found但し書きList.Count < 30 // ! アイテム多すぎ
					)
				{
					this.Found但し書きList.Add(info.但し書き);
					this.UpdateUI();
				}
			}
		}

		private void UpdateUI()
		{
			string text = string.Join(", ", this.Found但し書きList);

			if (100 < text.Length)
				text = text.Substring(0, 90) + " ...";

			if (this.UILabel.Text != text)
				this.UILabel.Text = text;
		}

		public string GetNext(string value)
		{
			if (1 <= this.Found但し書きList.Count)
			{
				int index = SCommon.IndexOf(this.Found但し書きList.ToArray(), v => v == value);

				index++;
				index %= this.Found但し書きList.Count;

				value = this.Found但し書きList[index];
			}
			return value;
		}

		public string GetPrev(string value)
		{
			if (1 <= this.Found但し書きList.Count)
			{
				int index = SCommon.IndexOf(this.Found但し書きList.ToArray(), v => v == value);

				index += this.Found但し書きList.Count - 1;
				index %= this.Found但し書きList.Count;

				value = this.Found但し書きList[index];
			}
			return value;
		}
	}
}
