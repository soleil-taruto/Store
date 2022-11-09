using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0011
	{
		public void Test01()
		{
			Enumerable.Range(1, 3)
				.Test0011_ForEach(v => Console.WriteLine("1 : " + v))
				.Test0011_ForEach(v => Console.WriteLine("2 : " + v))
				.Test0011_ForEach(v => Console.WriteLine("3 : " + v))
				.ToArray();

			// ----

			Enumerable.Range(1, 3)
				.Concat(Enumerable.Range(1, 3))
				.Test0011_ForEach(v => Console.WriteLine("A : " + v))
				.DistinctOrderBy((a, b) => a - b)
				.Test0011_ForEach(v => Console.WriteLine("B : " + v))
				.ToArray();

			// ----

			Enumerable.Range(1, 3)
				.Select(v => new int[] { v })
				.Test0011_ForEach(v => Console.WriteLine("1 : " + v[0]))
				.Test0011_ForEach(v => v[0] *= -1)
				.Test0011_ForEach(v => Console.WriteLine("2 : " + v[0])) // { -1, -2, -3 } にならない。-- { 1, 2, 3 } のまま。★分からん。
				.DistinctOrderBy((a, b) => a[0] - b[0])
				.Test0011_ForEach(v => Console.WriteLine("3 : " + v[0]))
				.ToArray();

			// ----

			Enumerable.Range(1, 3)
				.Select(v => new int[] { v })
				.Test0011_ForEach(v => Console.WriteLine("A : " + v[0]))
				.ForEach(v => v[0] *= -1)
				.Test0011_ForEach(v => Console.WriteLine("B : " + v[0])) // ちゃんと { -1, -2, -3 } になる。
				.DistinctOrderBy((a, b) => a[0] - b[0])
				.Test0011_ForEach(v => Console.WriteLine("C : " + v[0]))
				.ToArray();

			// ----

			Enumerable.Range(1, 3)
				.Select(v => new int[] { v })
				.Test0011_ForEach(v => Console.WriteLine("1 : " + v[0]))
				.Test0011_ForEach(v => { v[0] *= -1; Console.WriteLine("2 : " + v[0]); }) // 下の 3 : の前に表示されて { -1, -2, -3 } と表示されるけど ...
				.Test0011_ForEach(v => Console.WriteLine("3 : " + v[0])) // { -1, -2, -3 } にならない。-- { 1, 2, 3 } のまま。★分からん。
				.DistinctOrderBy((a, b) => a[0] - b[0])
				.Test0011_ForEach(v => Console.WriteLine("4 : " + v[0]))
				.ToArray();

			// ----

			Enumerable.Range(1, 3)
				.Select(v => new int[] { v })
				.Test0011_ForEach(v => Console.WriteLine("A : " + v[0]))
				.ForEach(v => v[0] *= -1)
				.ForEach(v => Console.WriteLine("B : " + v[0])) // ちゃんと { -1, -2, -3 } になる。
				.DistinctOrderBy((a, b) => a[0] - b[0])
				.Test0011_ForEach(v => Console.WriteLine("C : " + v[0]))
				.ToArray();

			// ----

			Enumerable.Range(1, 3)
				.Select(v => new int[] { v })
				.Test0011_ForEach(v => Console.WriteLine("1 : " + v[0]))
				.ForEach(v => { v[0] *= -1; Console.WriteLine("2 : " + v[0]); }) // 2 : -> 3 : と交互に表示される。{ -1, -2, -3 } と表示される。
				.ForEach(v => Console.WriteLine("3 : " + v[0])) // ちゃんと { -1, -2, -3 } になる。
				.DistinctOrderBy((a, b) => a[0] - b[0])
				.Test0011_ForEach(v => Console.WriteLine("4 : " + v[0]))
				.ToArray();
		}
	}
}
