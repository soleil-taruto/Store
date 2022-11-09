using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			Test01_a(new int[] { 1, 2, 3, 3 }, 3); // -> 12
			Test01_a(new int[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3 }, 7); // -> 108
		}

		private void Test01_a(int[] aArr, int k)
		{
			FoundCount = 0;
			K = k;
			KnownResults.Clear();

			Search(new int[0], aArr);

			Console.WriteLine(FoundCount);

			FoundCount = -1;
			K = -1;
			KnownResults.Clear();
		}

		private int FoundCount;
		private int K;
		private HashSet<string> KnownResults = new HashSet<string>();

		private void Search(int[] dest, int[] src)
		{
			if (src.Length == 0)
			{
				string result = string.Join(":", dest);

				if (!KnownResults.Contains(result))
				{
					KnownResults.Add(result);
					FoundCount++;
				}
				return;
			}

			for (int index = 0; index < src.Length; index++)
			{
#if true
				int[] nextDest = SCommon.E_AddRange(dest, new int[] { src[index] }).ToArray();
				int[] nextSrc = SCommon.E_RemoveRange(src, index, 1).ToArray();
#else // old same
				int[] nextDest = dest.Concat(new int[] { src[index] }).ToArray();
				int[] nextSrc = src.Take(index).Concat(src.Skip(index + 1)).ToArray();
#endif

				if (2 <= nextDest.Length)
					if (nextDest[nextDest.Length - 2] + nextDest[nextDest.Length - 1] < K) // ? 隣接する要素の和が K より小さい -> 条件不一致
						continue;

				Search(nextDest, nextSrc);
			}
		}
	}
}
