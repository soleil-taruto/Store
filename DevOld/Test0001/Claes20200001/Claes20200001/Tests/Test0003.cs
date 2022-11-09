using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			Test01_a(1000, 10);
			Test01_a(100, 30);
			Test01_a(10, 100);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int testCount, int cardCountMax)
		{
			Test01_a2(testCount, cardCountMax, 9);
			Test01_a2(testCount, cardCountMax, 99);
			Test01_a2(testCount, cardCountMax, 999);
			Test01_a2(testCount, cardCountMax, 9999);
			Test01_a2(testCount, cardCountMax, 99999);
			Test01_a2(testCount, cardCountMax, 999999);
		}

		private void Test01_a2(int testCount, int cardCountMax, int cardValueMax)
		{
			Console.WriteLine(string.Join(", ", testCount, cardCountMax, cardValueMax));

			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				int[] cards = Enumerable
					.Range(0, SCommon.CRandom.GetRange(3, cardCountMax))
					.Select(dummy => SCommon.CRandom.GetRange(1, cardValueMax))
					.ToArray();

				long ans1 = Test01_b1(cards);
				long ans2 = Test01_b2(cards);

				if (ans1 != ans2) // ? 不正解
					throw null;
			}
			Console.WriteLine("OK");
		}

		private long Test01_b1(int[] cards)
		{
			cards = cards.ToArray(); // 複製

			Array.Sort(cards, SCommon.Comp);

			string[] sCards = cards
				.Skip(cards.Length - 3)
				.Select(card => card.ToString())
				.ToArray();

			long best = -1;

			ChooseDifferentThreeAllOrder(3, (index1, index2, index3) =>
			{
				long value = long.Parse(string.Join("", sCards[index1], sCards[index2], sCards[index3]));

				if (best < value)
					best = value;
			});

			return best;
		}

		private long Test01_b2(int[] cards)
		{
			long best = -1;

			ChooseDifferentThreeAllOrder(cards.Length, (index1, index2, index3) =>
			{
				string str = string.Format("{0}{1}{2}", cards[index1], cards[index2], cards[index3]);
				long value = long.Parse(str);

				if (best < value)
					best = value;
			});

			return best;
		}

		private void ChooseDifferentThreeAllOrder(int count, Action<int, int, int> routine)
		{
			for (int index1 = 0; index1 < count; index1++)
			{
				for (int index2 = 0; index2 < count; index2++)
				{
					if (index1 != index2)
					{
						for (int index3 = 0; index3 < count; index3++)
						{
							if (
								index1 != index3 &&
								index2 != index3
								)
							{
								routine(index1, index2, index3);
							}
						}
					}
				}
			}
		}
	}
}
