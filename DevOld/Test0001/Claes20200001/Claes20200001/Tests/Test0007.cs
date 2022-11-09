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
			Func<bool> e1 = SCommon.Supplier(Test01_a1());
			Func<bool> e2 = SCommon.Supplier(Test01_a2());

			for (int count = 0; count < 65000; count++)
			{
				bool ans1 = e1();
				bool ans2 = e2();

				if (ans1 != ans2)
				{
					Console.WriteLine(count);
					throw null;
				}
			}
			Console.WriteLine("OK!");
		}

		private IEnumerable<bool> Test01_a1()
		{
			for (int value = 2; ; value++)
			{
				yield return IsPrime(value);
			}
		}

		private bool IsPrime(int value)
		{
			if (value == 2)
				return true;

			if (value % 2 == 0)
				return false;

			for (int d = 3; d * d <= value; d += 2)
				if (value % d == 0)
					return false;

			return true;
		}

#if true
		private IEnumerable<bool> Test01_a2()
		{
			Func<int> e = () => 1;

			for (int value = 2; ; value++)
			{
				bool ret = e() == 1;

				if (ret)
					e = Both(e, SCommon.Supplier(Eratosthenes(value)));

				yield return ret;
			}
		}

		private Func<int> Both(Func<int> e1, Func<int> e2)
		{
			return () => e1() & e2();
		}

		private IEnumerable<int> Eratosthenes(int value)
		{
			foreach (int n in Gaps(value))
			{
				for (int i = 0; i < n; i++)
					yield return 1;

				yield return 0;
			}
		}
#else // old same
		private IEnumerable<bool> Test01_a2()
		{
			Func<bool> e = () => true;

			for (int value = 2; ; value++)
			{
				bool ret = e();

				if (ret)
					e = Both(e, SCommon.Supplier(Eratosthenes(value)));

				yield return ret;
			}
		}

		private Func<bool> Both(Func<bool> e1, Func<bool> e2)
		{
			//return () => e1() && e2(); // <-- NG -- e1() == false のとき e2 を呼ばない。

			return () =>
			{
				bool ret1 = e1();
				bool ret2 = e2();

				return ret1 && ret2;
			};
		}

		private IEnumerable<bool> Eratosthenes(int value)
		{
			foreach (int n in Gaps(value))
			{
				for (int i = 0; i < n; i++)
					yield return true;

				yield return false;
			}
		}
#endif

		private IEnumerable<int> Gaps(int value)
		{
			yield return value * (value - 1) - 1;

			for (; ; )
			{
				yield return value - 1;
			}
		}
	}
}
