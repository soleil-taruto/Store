using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Charlotte
{
	public class 入力補助TBLB
	{
		public delegate string GetEntry_d(int index);

		public TextBox TB;
		public ListBox LB;
		public GetEntry_d GetEntry;

		// <---- prm

		private string Pattern;
		private int EntryIndex;
		private HashSet<string> KnownEntries = new HashSet<string>();

		public void TextChanged()
		{
			this.Pattern = this.TB.Text;
			this.EntryIndex = int.MaxValue;
			this.KnownEntries.Clear();

			this.LB.Items.Clear();
		}

		public void IntoList()
		{
			if (1 <= this.LB.Items.Count)
			{
				this.LB.SelectedIndex = 0;
				this.LB.Focus();
			}
		}

		public void ListSelected()
		{
			int index = this.LB.SelectedIndex;

			if (index == -1)
				return;

			this.TB.Text = this.LB.Items[index].ToString();
			this.TB.Focus();
		}

		public void EachTimer()
		{
			for (int c = 0; c < 100; c++) // ループ回数
			{
				this.EntryIndex = Math.Min(this.EntryIndex, this.GetEntryCount());

				if (this.EntryIndex <= 0)
					break;

				this.EntryIndex--;
				string entry = this.GetEntry(this.EntryIndex);

				{
					string a = entry;
					string b = this.Pattern;

					a = this.MatchConv(a);
					b = this.MatchConv(b);

					if (a.Contains(b) == false)
						continue;
				}

				if (this.KnownEntries.Contains(entry))
					continue;

				if (100 <= this.LB.Items.Count) // アイテム多すぎ
					continue;

				this.LB.Items.Add(entry);
				this.KnownEntries.Add(entry);

				//break; // zantei
			}
		}

		private int GetEntryCount()
		{
			return Ground.I.ReceiptInfos.Count;
		}

		private string MatchConv(string str)
		{
			str = Strings.StrConv(str, VbStrConv.Wide);
			str = Strings.StrConv(str, VbStrConv.Hiragana);
			str = Strings.StrConv(str, VbStrConv.Uppercase);

			return str;
		}
	}
}
