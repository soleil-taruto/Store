using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0006
	{
		public void Test01()
		{
			Test01_a(10000, 10);
			Test01_a(1000, 100);
			Test01_a(100, 1000);
			Test01_a(10, 10000);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int testCount, int countScale)
		{
			Test01_a2(testCount, countScale, 10000);
			Test01_a2(testCount, countScale, 1000);
			Test01_a2(testCount, countScale, 100);
			Test01_a2(testCount, countScale, 10);

			Console.WriteLine("OK");
		}

		private void Test01_a2(int testCount, int countScale, int valueScale)
		{
			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				int[] arr = Enumerable
					.Range(0, SCommon.CRandom.GetInt(countScale))
					.Select(dummy => SCommon.CRandom.GetInt(valueScale))
					.ToArray();

				IEnumerable<int> ans1 = Test01_b1(arr, SCommon.Comp);
				IEnumerable<int> ans2 = Test01_b2(arr, SCommon.Comp);

				IEnumerator<int> ite1 = ans1.GetEnumerator();
				IEnumerator<int> ite2 = ans2.GetEnumerator();

				for (; ; )
				{
					bool n1 = ite1.MoveNext();
					bool n2 = ite2.MoveNext();

					if (n1 != n2)
						throw null; // BUG !!!

					if (!n1)
						break;

					if (ite1.Current != ite2.Current)
						throw null; // BUG !!!
				}
			}
		}

		private IEnumerable<T> Test01_b1<T>(IEnumerable<T> src, Comparison<T> comp)
		{
			return src.OrderBy(comp);
		}

		private IEnumerable<T> Test01_b2<T>(IEnumerable<T> src, Comparison<T> comp)
		{
			Queue<IEnumerable<T>> q = new Queue<IEnumerable<T>>(src.Select(v => new T[] { v }));

			if (q.Count == 0)
				return new T[0];

			while (2 <= q.Count)
				q.Enqueue(E_Merge(q.Dequeue(), q.Dequeue(), comp));

			return q.Dequeue();
		}

#if true // これ用ver
		private IEnumerable<T> E_Merge<T>(IEnumerable<T> a, IEnumerable<T> b, Comparison<T> comp)
		{
			IEnumerator<T> ia = a.GetEnumerator();
			IEnumerator<T> ib = b.GetEnumerator();

			if (!ia.MoveNext())
				throw null; // never

			if (!ib.MoveNext())
				throw null; // never

			for (; ; )
			{
				int ret = comp(ia.Current, ib.Current);

				if (ret <= 0)
				{
					yield return ia.Current;

					if (!ia.MoveNext())
						break;
				}
				SCommon.Swap(ref ia, ref ib);
			}
			for (; ; )
			{
				yield return ib.Current;

				if (!ib.MoveNext())
					break;
			}
		}
#else // 汎用ver
		private IEnumerable<T> E_Merge<T>(IEnumerable<T> a, IEnumerable<T> b, Comparison<T> comp)
		{
			IEnumerator<T> ia = a.GetEnumerator();
			IEnumerator<T> ib = b.GetEnumerator();

			bool more1 = ia.MoveNext();
			bool more2 = ib.MoveNext();

			while (more1 && more2)
			{
				int ret = comp(ia.Current, ib.Current);

				if (ret <= 0)
				{
					yield return ia.Current;
					more1 = ia.MoveNext();
				}
				if (ret >= 0)
				{
					yield return ib.Current;
					more2 = ib.MoveNext();
				}
			}
			while (more1)
			{
				yield return ia.Current;
				more1 = ia.MoveNext();
			}
			while (more2)
			{
				yield return ib.Current;
				more2 = ib.MoveNext();
			}
		}
#endif
	}
}
