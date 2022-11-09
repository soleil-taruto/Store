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
			Test01_a(10000, 2);
			Test01_a(1000, 3);
			Test01_a(100, 4);
			Test01_a(10, 5);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int testCount, int nMax)
		{
			Test01_a2(testCount, nMax, 7);
			Test01_a2(testCount, nMax, 10);
			Test01_a2(testCount, nMax, 100);
			Test01_a2(testCount, nMax, 103);
			Test01_a2(testCount, nMax, 1000);
			Test01_a2(testCount, nMax, 1003);
		}

		private void Test01_a2(int testCount, int nMax, int mMax)
		{
			Console.WriteLine(string.Join(", ", testCount, nMax, mMax));

			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				int n = SCommon.CRandom.GetRange(1, nMax);
				int m = SCommon.CRandom.GetRange(2, mMax);

				int[] aArr = Enumerable.Range(0, n * 2).Select(dummy => SCommon.CRandom.GetRange(0, m - 1)).ToArray();

				string ans1 = Test01_b1(aArr, m);
				string ans2 = Test01_b2(aArr, m);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private string Test01_b1(int[] aArr, int m)
		{
			int[] aa = aArr.ToArray(); // 複製
			int n = aa.Length;
			int n_ = 0;
			int i;

			Array.Sort(aa, SCommon.Comp);

			for (i = 0; i < n; i++)
			{
				if (n_ == 0 || aa[n_ - 1] != aa[i])
				{
					aa[n_++] = aa[i];
				}
				else
				{
					n_--;
				}
			}
			n = n_;

			if (m % 2 != 0)
			{
				return n != 0 ? "Alice" : "Bob";
			}
			for (i = 0; i < n / 2; i++)
			{
				if (aa[i + n / 2] - aa[i] != m / 2)
				{
					return "Alice";
				}
			}
			return n % 4 != 0 ? "Alice" : "Bob";
		}

		private string Test01_b2(int[] aArr, int m)
		{
			GameStatusInfo gameStatus = new GameStatusInfo()
			{
				TurnWho = Player_e.Alice,
				AArr = aArr,
				AliceSum = 0,
				BobSum = 0,
				M = m,
			};

			Player_e winner = Judge(gameStatus);

			return winner == Player_e.Alice ? "Alice" : "Bob";
		}

		private Player_e Judge(GameStatusInfo gameStatus)
		{
			if (gameStatus.AArr.Length == 0)
			{
				if (gameStatus.AliceSum % gameStatus.M == gameStatus.BobSum % gameStatus.M)
				{
					return Player_e.Bob;
				}
				else
				{
					return Player_e.Alice;
				}
			}

			bool canAliceWin = false;
			bool canBobWin = false;

			for (int index = 0; index < gameStatus.AArr.Length; index++)
			{
				GameStatusInfo nextGameStatus = new GameStatusInfo()
				{
					TurnWho = gameStatus.TurnWho == Player_e.Alice ? Player_e.Bob : Player_e.Alice,
					AArr = gameStatus.AArr.Take(index).Concat(gameStatus.AArr.Skip(index + 1)).ToArray(),
					AliceSum =
						gameStatus.AliceSum + (
						gameStatus.TurnWho == Player_e.Alice ?
						gameStatus.AArr[index] : 0),
					BobSum =
						gameStatus.BobSum + (
						gameStatus.TurnWho == Player_e.Bob ?
						gameStatus.AArr[index] : 0),
					M = gameStatus.M,
				};

				Player_e nextJudgement = Judge(nextGameStatus);

				switch (nextJudgement)
				{
					case Player_e.Alice:
						canAliceWin = true;
						break;

					case Player_e.Bob:
						canBobWin = true;
						break;

					default:
						throw null; // never
				}
			}

			switch (gameStatus.TurnWho)
			{
				case Player_e.Alice:
					return canAliceWin ? Player_e.Alice : Player_e.Bob;

				case Player_e.Bob:
					return canBobWin ? Player_e.Bob : Player_e.Alice;

				default:
					throw null; // never
			}
		}

		private struct GameStatusInfo
		{
			public Player_e TurnWho;
			public int[] AArr;
			public int AliceSum;
			public int BobSum;
			public int M;
		}

		private enum Player_e
		{
			Alice,
			Bob,
		}
	}
}
