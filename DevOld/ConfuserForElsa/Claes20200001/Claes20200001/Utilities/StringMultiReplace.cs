using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utilities
{
	public class StringMultiReplace
	{
		private class RangeInfo
		{
			public int Start;
			public int End;
			public string PartNew;
		}

		private List<RangeInfo> Ranges = new List<RangeInfo>();

		/// <summary>
		/// 置き換え範囲の追加
		/// start ～ (end - 1) を textNew に置き換える。という指示を追加する。
		/// 以下に相当：
		/// text = text.SubString(0, start) + partNew + text.SubString(end);
		/// 開始位置、終了位置共に置き換え前のテキストの位置を指す。
		/// </summary>
		/// <param name="start">開始位置</param>
		/// <param name="end">終了位置</param>
		/// <param name="partNew">置き換え後の部分文字列</param>
		public void AddRange(int start, int end, string partNew)
		{
			this.Ranges.Add(new RangeInfo()
			{
				Start = start,
				End = end,
				PartNew = partNew,
			});
		}

		/// <summary>
		/// 置き換え実行
		/// </summary>
		/// <param name="text">置き換え前のテキスト</param>
		/// <returns>置き換え後のテキスト</returns>
		public string Perform(string text)
		{
			if (text == null)
				throw new ArgumentException();

			if (this.Ranges.Count == 0)
				return text;

			this.Ranges.Sort((a, b) => a.Start - b.Start);

			if (this.Ranges[0].Start < 0)
				throw new ArgumentException();

			if (this.Ranges[this.Ranges.Count - 1].End > text.Length)
				throw new ArgumentException();

			for (int index = 0; index < this.Ranges.Count; index++)
			{
				if (1 <= index && this.Ranges[index - 1].End > this.Ranges[index].Start)
					throw new ArgumentException();

				if (this.Ranges[index].Start > this.Ranges[index].End)
					throw new ArgumentException();

				if (this.Ranges[index].PartNew == null)
					throw new ArgumentException();
			}

			// 引数チェック_ここまで

			StringBuilder buff = new StringBuilder();
			int r = 0;

			foreach (RangeInfo range in this.Ranges)
			{
				buff.Append(text.Substring(r, range.Start - r));
				buff.Append(range.PartNew);
				r = range.End;
			}
			buff.Append(text.Substring(r));

			return buff.ToString();
		}
	}
}
