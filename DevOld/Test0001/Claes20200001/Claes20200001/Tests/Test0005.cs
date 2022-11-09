using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0005
	{
		private const int PLAYER_NUM = 8;

		private class GameInfo
		{
			public PlayerInfo[] Players = new PlayerInfo[PLAYER_NUM];
			public PlayerInfo[] RankOrder;
		}

		private class PlayerInfo
		{
			public int SelfIndex; // 0～
			public int[] Points = new int[PLAYER_NUM];
			public int Rank; // 1～

			public int Point
			{
				get
				{
					return this.Points.Sum();
				}
			}
		}

		private GameInfo CreateGame()
		{
			GameInfo game = new GameInfo();

			for (int index = 0; index < PLAYER_NUM; index++)
			{
				game.Players[index] = new PlayerInfo();
				game.Players[index].SelfIndex = index;
			}
			for (int b = 1; b < PLAYER_NUM; b++)
			{
				for (int a = 0; a < b; a++)
				{
					int aPoint = Math.Max(
						SCommon.CRandom.GetInt(3) * 5,
						SCommon.CRandom.GetInt(3) * 5
						);
					int bPoint = 10 - aPoint;

					game.Players[a].Points[b] = aPoint;
					game.Players[b].Points[a] = bPoint;
				}
			}

			for (int index = 0; index < PLAYER_NUM; index++)
				game.Players[index].Rank = game.Players.Where(v => game.Players[index].Point < v.Point).Count() + 1;

			game.RankOrder = game.Players.ToArray(); // Cloning

			Array.Sort(game.RankOrder, (a, b) => a.Rank - b.Rank);

			return game;
		}

		public void Test01()
		{
			for (int c = 0; c < 10; )
			{
				GameInfo game = CreateGame();

				if (SCommon.HasSame(game.Players, (a, b) => a.Point == b.Point))
					continue;

				if (
					game.RankOrder[1].Point !=
					game.RankOrder[4].Point +
					game.RankOrder[5].Point +
					game.RankOrder[6].Point +
					game.RankOrder[7].Point
					)
					continue;

				Console.WriteLine(game.RankOrder[2].Points[game.RankOrder[6].SelfIndex]);
				c++;
			}
		}
	}
}
