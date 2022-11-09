using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0007
	{
		public void Test01()
		{
			foreach (int[] indexes in Combination(10, 4))
				Console.WriteLine(string.Join(", ", indexes));

			Console.WriteLine(Combination(10, 4).Count());

			// ----

			foreach (int[] indexes in Combination(5, 3))
				Console.WriteLine(string.Join(", ", indexes));
		}

		public static IEnumerable<int[]> Combination(int n, int r)
		{
			if (r < 1 || n < r)
				throw new Exception(); // Bad params

			int[] indexes = Enumerable.Range(0, r).ToArray();

			for (; ; )
			{
				yield return indexes;

				for (int d = 0; ; d++)
				{
					indexes[d]++;

					if (d + 1 < r)
					{
						if (indexes[d] < indexes[d + 1])
							break;
					}
					else
					{
						if (indexes[d] < n)
							break;

						yield break;
					}

					if (1 <= d)
						indexes[d] = indexes[d - 1] + 1;
					else
						indexes[d] = 0;
				}
			}
		}
	}
}
