using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0004
	{
		public void Test01()
		{
			Test01_a(10);
			Test01_a(20);
			Test01_a(30);
			Test01_a(40);
			Test01_a(50);
			//Test01_a(60); // 重い
			//Test01_a(70); // 重すぎ
		}

		public void Test01_a(int scale)
		{
			PrimeNumbers = Enumerable
				.Range(2, scale)
				.Where(n => MillerRabin.IsPrime((ulong)n))
				.ToArray();

			Founds = new BitList();

			Search(0, 0);

			long index;
			for (index = 0; Founds[index]; index++) ;

			Console.WriteLine(scale + " --> " + index);

			// clear
			PrimeNumbers = null;
			Founds = null;
		}

		private int[] PrimeNumbers;
		private BitList Founds;

		private void Search(int index, long value)
		{
			if (PrimeNumbers.Length <= index)
				return;

			int n = PrimeNumbers[index];

			for (int m = -1; m <= 1; m++)
			{
				long nextValue = value + n * m;

				if (0 <= nextValue)
					Founds[nextValue] = true;

				Search(index + 1, nextValue);
			}
		}
	}
}
