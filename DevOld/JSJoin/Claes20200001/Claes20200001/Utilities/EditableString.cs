using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utilities
{
	public class EditableString
	{
		private List<char> Chars;

		public EditableString(string str)
		{
			this.Chars = str.ToList();
		}

		public int Count
		{
			get
			{
				return this.Chars.Count;
			}
		}

		public char this[int index]
		{
			get
			{
				return this.Chars[index];
			}
		}

		public void Add(string strForAdd)
		{
			this.Chars.AddRange(strForAdd.ToCharArray());
		}

		public string Substring(int start, int count)
		{
			return new string(this.Chars.GetRange(start, count).ToArray());
		}

		public void Replace(int start, int count, string strForInsert)
		{
			this.Chars.RemoveRange(start, count);
			this.Chars.InsertRange(start, strForInsert.ToCharArray());
		}

		public override string ToString()
		{
			return new string(this.Chars.ToArray());
		}
	}
}
