using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0005
	{
		public void Test01()
		{
			Test01_a(Enumerable.Range(1, 1).ToArray());
			Test01_a(Enumerable.Range(1, 2).ToArray());
			Test01_a(Enumerable.Range(1, 3).ToArray());
			Test01_a(Enumerable.Range(1, 4).ToArray());
			Test01_a(Enumerable.Range(1, 5).ToArray());
			Test01_a(Enumerable.Range(1, 6).ToArray());
			Test01_a(Enumerable.Range(1, 7).ToArray());
			Test01_a(Enumerable.Range(1, 8).ToArray());
			Test01_a(Enumerable.Range(1, 9).ToArray());
		}

		public void Test02()
		{
			Test01_a(new int[] { 3, 5, 7 });
		}

		public void Test03()
		{
			const int BAR_FIRST = 1;
			const int BAR_LAST = 10;

			for (int a = BAR_FIRST; a <= BAR_LAST; a++)
			{
				for (int b = a; b <= BAR_LAST; b++)
				{
					for (int c = b; c <= BAR_LAST; c++)
					{
						GameStatusInfo gameStatus = new GameStatusInfo()
						{
							TurnWho = Player_e.Alice,
							Parts = new int[] { a, b, c },
						};

						Player_e winner = Judge(gameStatus);

						if (winner == Player_e.Bob)
						{
							Console.WriteLine(string.Join(", ", a, b, c));
						}
					}
				}
			}
		}

		private void Test01_a(int[] parts)
		{
			GameStatusInfo gameStatus = new GameStatusInfo()
			{
				TurnWho = Player_e.Alice,
				Parts = parts.ToArray(), // Clone
			};

			Player_e winner = Judge(gameStatus);

			Console.WriteLine(string.Join(", ", parts) + " -> " + winner);
		}

		private enum Player_e
		{
			Alice,
			Bob,
		}

		private class GameStatusInfo
		{
			public Player_e TurnWho;
			public int[] Parts;
		}

		private Player_e Judge(GameStatusInfo gameStatus)
		{
			if (gameStatus.Parts.Length == 0)
			{
				// 直前の消しが最後だった(相手の負けな)ので、棒が無い状態でターンが回ってきたら勝ちで良いはず。

				if (gameStatus.TurnWho == Player_e.Alice)
				{
					return Player_e.Alice;
				}
				else
				{
					return Player_e.Bob;
				}
			}

			string cacheStrParts = GetCacheStrParts(gameStatus.Parts);

			if (CacheTurnWhoIsWinner.ContainsKey(cacheStrParts))
			{
				if (CacheTurnWhoIsWinner[cacheStrParts])
				{
					return gameStatus.TurnWho;
				}
				else
				{
					return gameStatus.TurnWho == Player_e.Alice ? Player_e.Bob : Player_e.Alice;
				}
			}

			bool canAliceWin = false;
			bool canBobWin = false;

			for (int index = 0; index < gameStatus.Parts.Length; index++)
			{
				for (int start = 0; start < gameStatus.Parts[index]; start++)
				{
					for (int end = start + 1; end <= gameStatus.Parts[index]; end++)
					{
						List<int> nextParts = gameStatus.Parts.ToList(); // Clone

						nextParts[index] = 0;
						nextParts.Add(start);
						nextParts.Add(gameStatus.Parts[index] - end);
						nextParts.RemoveAll(v => v == 0);

						GameStatusInfo nextGameStatus = new GameStatusInfo()
						{
							TurnWho = gameStatus.TurnWho == Player_e.Alice ? Player_e.Bob : Player_e.Alice,
							Parts = nextParts.ToArray(),
						};

						Player_e nextWinner = Judge(nextGameStatus);

						if (nextWinner == Player_e.Alice)
						{
							canAliceWin = true;
						}
						else
						{
							canBobWin = true;
						}
					}
				}
			}

			if (gameStatus.TurnWho == Player_e.Alice)
			{
				if (canAliceWin)
				{
					CacheTurnWhoIsWinner.Add(cacheStrParts, true);
					return Player_e.Alice;
				}
				else
				{
					CacheTurnWhoIsWinner.Add(cacheStrParts, false);
					return Player_e.Bob;
				}
			}
			else
			{
				if (canBobWin)
				{
					CacheTurnWhoIsWinner.Add(cacheStrParts, true);
					return Player_e.Bob;
				}
				else
				{
					CacheTurnWhoIsWinner.Add(cacheStrParts, false);
					return Player_e.Alice;
				}
			}
		}

		private Dictionary<string, bool> CacheTurnWhoIsWinner = SCommon.CreateDictionary<bool>();

		private string GetCacheStrParts(int[] parts)
		{
			parts = parts.Where(v => 1 <= v).ToArray(); // Clone + ゼロ除去

			Array.Sort(parts, SCommon.Comp);

			return string.Join("_", parts);
		}
	}
}
