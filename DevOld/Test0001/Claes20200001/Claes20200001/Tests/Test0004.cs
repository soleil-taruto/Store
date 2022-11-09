using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0004
	{
		public void Test01()
		{
			Judgement_e ans = Judge(new GameStatusInfo()
			{
				TurnWho = Player_e.Alice,
				Table = CreateTable(),
			});

			Console.WriteLine(ans);
		}

		private enum Player_e
		{
			/// <summary>
			/// 先攻
			/// </summary>
			Alice = 1,

			/// <summary>
			/// 後攻
			/// </summary>
			Bob,
		}

		private enum Cell_e
		{
			None = 1,
			Alice,
			Bob,
		}

		private class GameStatusInfo
		{
			public Player_e TurnWho;
			public Cell_e[,] Table;
		}

		private Cell_e[,] CreateTable()
		{
			return new Cell_e[,]
			{
				{ Cell_e.None, Cell_e.None, Cell_e.None },
				{ Cell_e.None, Cell_e.None, Cell_e.None },
				{ Cell_e.None, Cell_e.None, Cell_e.None },
			};
		}

		private Cell_e[,] CloneTable(Cell_e[,] src)
		{
			Cell_e[,] dest = CreateTable();

			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					dest[x, y] = src[x, y];
				}
			}
			return dest;
		}

		private enum Judgement_e
		{
			Draw = 1,
			Alice,
			Bob,
		}

		private Judgement_e Judge(GameStatusInfo gameStatus)
		{
			{
				Judgement_e? ans;

				for (int c = 0; c < 3; c++)
				{
					ans = IsComplete(gameStatus, c, 0, c, 1, c, 2);

					if (ans != null)
						return ans.Value;

					ans = IsComplete(gameStatus, 0, c, 1, c, 2, c);

					if (ans != null)
						return ans.Value;
				}

				ans = IsComplete(gameStatus, 0, 0, 1, 1, 2, 2);

				if (ans != null)
					return ans.Value;

				ans = IsComplete(gameStatus, 0, 2, 1, 1, 2, 0);

				if (ans != null)
					return ans.Value;
			}

			bool canDraw = false;
			bool canAliceWin = false;
			bool canBobWin = false;

			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					if (gameStatus.Table[x, y] == Cell_e.None)
					{
						GameStatusInfo nextGameStatus = new GameStatusInfo()
						{
							TurnWho = gameStatus.TurnWho == Player_e.Alice ? Player_e.Bob : Player_e.Alice,
							Table = CloneTable(gameStatus.Table),
						};

						switch (gameStatus.TurnWho)
						{
							case Player_e.Alice:
								nextGameStatus.Table[x, y] = Cell_e.Alice;
								break;

							case Player_e.Bob:
								nextGameStatus.Table[x, y] = Cell_e.Bob;
								break;

							default:
								throw null; // never
						}

						Judgement_e nextJudgement = Judge(nextGameStatus);

						switch (nextJudgement)
						{
							case Judgement_e.Draw:
								canDraw = true;
								break;

							case Judgement_e.Alice:
								canAliceWin = true;
								break;

							case Judgement_e.Bob:
								canBobWin = true;
								break;

							default:
								throw null; // never
						}
					}
				}
			}

			if (
				!canDraw &&
				!canAliceWin &&
				!canBobWin
				)
				return Judgement_e.Draw;

			switch (gameStatus.TurnWho)
			{
				case Player_e.Alice:
					if (canAliceWin) return Judgement_e.Alice;
					if (canDraw) return Judgement_e.Draw;
					return Judgement_e.Bob;

				case Player_e.Bob:
					if (canBobWin) return Judgement_e.Bob;
					if (canDraw) return Judgement_e.Draw;
					return Judgement_e.Alice;

				default:
					throw null; // never
			}
		}

		private Judgement_e? IsComplete(GameStatusInfo gameStatus, int x1, int y1, int x2, int y2, int x3, int y3)
		{
			if (
				gameStatus.Table[x1, y1] != Cell_e.None &&
				gameStatus.Table[x1, y1] == gameStatus.Table[x2, y2] &&
				gameStatus.Table[x1, y1] == gameStatus.Table[x3, y3]
				)
				return gameStatus.Table[x1, y1] == Cell_e.Alice ?
					Judgement_e.Alice :
					Judgement_e.Bob;

			return null;
		}
	}
}
