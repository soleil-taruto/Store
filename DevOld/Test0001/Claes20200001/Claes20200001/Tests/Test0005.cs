using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0005
	{
		//private const int RANGE_SCALE = 10000;
		//private const int RANGE_SCALE = 100000;
		//private const int RANGE_SCALE = 1000000;
		private const int RANGE_SCALE = 10000000;
		//private const int RANGE_SCALE = 100000000;
		//private const int RANGE_SCALE = 1000000000;

		private static int[] Primes =
			Enumerable.Range(0, RANGE_SCALE).Where(n => IsPrime((ulong)n))
			.Concat(new int[] {
			Enumerable.Range(RANGE_SCALE, int.MaxValue - RANGE_SCALE).First(n => IsPrime((ulong)n)) })
			.ToArray();

		public void Test01()
		{
			for (int k = 1; k <= 100; k++)
				PrintPrimeVoid(k);

		}

		private static void PrintPrimeVoid(int k)
		{
			Console.WriteLine(string.Format("2 以上 {0} 以下の異なる 2 つの素数についてその間にある素数の個数が {1} 未満のもののうち差が最大となるものは：", Primes[Primes.Length - 1], k));

			int maxDiff = Enumerable.Range(0, Primes.Length - k).Select(i => Primes[i + k] - Primes[i]).Max();

			foreach (int i in Enumerable.Range(0, Primes.Length - k))
				if (maxDiff == Primes[i + k] - Primes[i])
					Console.WriteLine(string.Format("{0} と {1} その差は {2}", Primes[i], Primes[i + k], Primes[i + k] - Primes[i]));

			Console.WriteLine("");
		}

		public static bool IsPrime(ulong n)
		{
			return MillerRabin.IsPrime(n);
		}
	}
}
