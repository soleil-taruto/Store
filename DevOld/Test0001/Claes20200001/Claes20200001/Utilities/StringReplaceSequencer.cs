using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utilities
{
	public class StringReplaceSequencer
	{
		private class RangeInfo
		{
			public int Start;
			public int RemoveLength;
			public string NewPart;
			public int SourceStart;
			public int SourceLength;
			public int Serial;

			public int End
			{
				get
				{
					return this.Start + this.RemoveLength;
				}
			}
		}

		private List<RangeInfo> Ranges = new List<RangeInfo>();

		public void Add(int start, int removeLength, string newPart)
		{
			this.Add(start, removeLength, newPart, 0, newPart.Length);
		}

		public void Add(int start, int removeLength, string newPart, int sourceStart, int sourceLength)
		{
			RangeInfo range = new RangeInfo()
			{
				Start = start,
				RemoveLength = removeLength,
				NewPart = newPart,
				SourceStart = sourceStart,
				SourceLength = sourceLength,
				Serial = this.Ranges.Count,
			};

			this.Ranges.Add(range);
		}

		public string Replace(string text)
		{
			if (text == null)
				throw new ArgumentException("Bad text");

			// 置き換える範囲を先頭に近い方から順に並べ替える。
			// 同じ位置(削除する長さ=0)の場合は、追加された順とする。
			// -- 例えば .Add(100, 0, PART_1) -> .Add(100, 0, PART_2) の順で実行された場合 100 の位置に PART_1 + PART_2 が挿入される。
			this.Ranges.Sort((a, b) =>
			{
				int ret = a.Start - b.Start;

				if (ret != 0)
					return ret;

				ret = a.RemoveLength - b.RemoveLength;

				if (ret != 0)
					return ret;

				return a.Serial - b.Serial;
			});

			foreach (RangeInfo range in this.Ranges)
			{
				if (
					range.Start < 0 || text.Length < range.Start ||
					range.RemoveLength < 0 || text.Length - range.Start < range.RemoveLength ||
					range.NewPart == null ||
					range.SourceStart < 0 || range.NewPart.Length < range.SourceStart ||
					range.SourceLength < 0 || range.NewPart.Length - range.SourceStart < range.SourceLength
					)
					throw new ArgumentException("Bad range");
			}

			for (int index = 1; index < this.Ranges.Count; index++)
				if (this.Ranges[index].Start < this.Ranges[index - 1].End)
					throw new ArgumentException("Overlapped range");

			// -- ここまで_引数チェック

			int capacity = text.Length;

			foreach (RangeInfo range in this.Ranges)
				capacity += range.SourceLength - range.RemoveLength;

			StringBuilder dest = new StringBuilder(capacity);
			int readPoint = 0;

			foreach (RangeInfo range in this.Ranges)
			{
				dest.Append(text, readPoint, range.Start - readPoint);
				dest.Append(range.NewPart, range.SourceStart, range.SourceLength);
				readPoint = range.End;
			}
			dest.Append(text, readPoint, text.Length - readPoint);
			return dest.ToString();
		}
	}
}
