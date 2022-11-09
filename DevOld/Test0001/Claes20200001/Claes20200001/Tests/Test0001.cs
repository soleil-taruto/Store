using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			Test01_a(2, 2);
			Test01_a(3, 2);
			Test01_a(3, 3);
			//Test01_a(4, 3); // 重すぎ
			//Test01_a(4, 4); // 重すぎ
			Test01_a(4, 2);
		}

		private void Test01_a(int w, int h)
		{
			TableInfo table = new TableInfo(w, h);
			table.DebugPrint();
			table.Shuffle();
			table.DebugPrint();
			table = Solve(table);
			table.DebugPrint();
		}

		public void Test02()
		{
			Test02_a(2, 2);
			Test02_a(3, 2);
			Test02_a(3, 3);
			//Test02_a(4, 3); // 重すぎ
			//Test02_a(4, 4); // 重すぎ
			Test02_a(4, 2);
		}

		private void Test02_a(int w, int h)
		{
			ProcMain.WriteLog(w + ", " + h);

			for (int testcnt = 0; testcnt < 10; testcnt++)
			{
				TableInfo table = new TableInfo(w, h);
				table.Shuffle();
				table = Solve(table);

				ProcMain.WriteLog(testcnt + " --> " + table.SolveRoute.Count);
			}
			ProcMain.WriteLog("done");
		}

		public void Test03()
		{
			Test03_a(3, 3);
			Test03_a(4, 2);
		}

		private void Test03_a(int w, int h)
		{
			double maxMillis = 0.0;
			int maxSolveRouteCount = 0;

			for (int testcnt = 0; testcnt < 79; testcnt++)
			{
				Console.Write(".");
				//Console.WriteLine("testcnt: " + testcnt);

				TableInfo table = new TableInfo(w, h);
				table.Shuffle();

				DateTime stTm = DateTime.Now;
				table = Solve(table);
				DateTime edTm = DateTime.Now;
				double millis = (edTm - stTm).TotalSeconds;

				maxMillis = Math.Max(maxMillis, millis);
				maxSolveRouteCount = Math.Max(maxSolveRouteCount, table.SolveRoute.Count);

				//Console.WriteLine(maxMillis.ToString("F3"));
				//Console.WriteLine(maxSolveRouteCount);
			}
			Console.WriteLine("");

			Console.WriteLine(w + ", " + h + " ->");
			Console.WriteLine(maxMillis.ToString("F3"));
			Console.WriteLine(maxSolveRouteCount);
		}

		private class TableInfo
		{
			/// <summary>
			/// 幅
			/// </summary>
			public int W;

			/// <summary>
			/// 高さ
			/// </summary>
			public int H;

			/// <summary>
			/// 添字の範囲：
			/// -- [0 ～ (W - 1), 0 ～ (H - 1)]
			/// 値：
			/// -- 0 == 空間
			/// -- 1 ～ (W * H) == パネルの番号
			/// </summary>
			public int[,] Cells;

			/// <summary>
			/// 空間の位置
			/// </summary>
			public I2Point SpacePos;

			/// <summary>
			/// 解法(操作履歴)
			/// </summary>
			public List<int> SolveRoute = new List<int>();

			public TableInfo(int w, int h)
			{
				if (
					w < 1 || SCommon.IMAX < w ||
					h < 1 || SCommon.IMAX < h
					)
					throw new ArgumentException();

				this.W = w;
				this.H = h;
				this.Cells = new int[w, h];

				for (int y = 0; y < h; y++)
				{
					for (int x = 0; x < w; x++)
					{
						this.Cells[x, y] = 1 + x + y * w;
					}
				}
				this.Cells[w - 1, h - 1] = 0;

				this.SpacePos = new I2Point(w - 1, h - 1);
			}

			private TableInfo()
			{ }

			public TableInfo GetClone()
			{
				TableInfo dest = new TableInfo();

				dest.W = this.W;
				dest.H = this.H;
				dest.Cells = new int[this.W, this.H];
				dest.SpacePos = this.SpacePos;
				dest.SolveRoute = new List<int>();

				for (int y = 0; y < this.H; y++)
				{
					for (int x = 0; x < this.W; x++)
					{
						dest.Cells[x, y] = this.Cells[x, y];
					}
				}
				dest.SolveRoute.AddRange(this.SolveRoute);

				return dest;
			}

			public void Shuffle()
			{
				for (int c = 0; c < this.W * this.H * 100; c++)
				{
					int nx = this.SpacePos.X;
					int ny = this.SpacePos.Y;

					switch (SCommon.CRandom.GetInt(4))
					{
						case 0: nx--; break;
						case 1: ny--; break;
						case 2: nx++; break;
						case 3: ny++; break;

						default:
							throw null; // never
					}

					if (
						nx < 0 ||
						ny < 0 ||
						this.W <= nx ||
						this.H <= ny
						)
						continue;

					this.Cells[this.SpacePos.X, this.SpacePos.Y] = this.Cells[nx, ny];
					this.Cells[nx, ny] = 0;
					this.SpacePos = new I2Point(nx, ny);
				}
			}

			public string GetCellsString()
			{
				return string.Join(":", this.IterateCells());
			}

			private IEnumerable<int> IterateCells()
			{
				for (int y = 0; y < this.H; y++)
				{
					for (int x = 0; x < this.W; x++)
					{
						yield return this.Cells[x, y];
					}
				}
			}

			public void DebugPrint()
			{
				Console.WriteLine("{");

				for (int y = 0; y < this.H; y++)
				{
					for (int x = 0; x < this.W; x++)
						Console.Write("\t" + this.Cells[x, y]);

					Console.WriteLine("");
				}
				Console.WriteLine(this.SpacePos);
				Console.WriteLine(this.GetCellsString());
				Console.WriteLine(string.Join(" -> ", this.SolveRoute));
				Console.WriteLine("}");
			}

			public bool IsSolved()
			{
				for (int y = 0; y < this.H; y++)
				{
					for (int x = 0; x < this.W; x++)
					{
						int expect = 1 + x + y * this.W;

						if (x == this.W - 1 && y == this.H - 1)
							expect = 0;

						if (this.Cells[x, y] != expect)
							return false;
					}
				}
				return true;
			}
		}

		private TableInfo Solve(TableInfo table)
		{
			Queue<TableInfo> q = new Queue<TableInfo>();
			q.Enqueue(table);

			HashSet<string> knownCellsStrings = new HashSet<string>();

			while (1 <= q.Count)
			{
				table = q.Dequeue();
				string cellsString = table.GetCellsString();

				if (knownCellsStrings.Contains(cellsString))
					continue;

				knownCellsStrings.Add(cellsString);

				if (table.IsSolved())
					return table;

				for (int d = 0; d < 4; d++)
				{
					TableInfo nextTable = table.GetClone();

					int nx = table.SpacePos.X;
					int ny = table.SpacePos.Y;

					switch (d)
					{
						case 0: nx--; break;
						case 1: ny--; break;
						case 2: nx++; break;
						case 3: ny++; break;

						default:
							throw null; // never
					}

					if (
						nx < 0 ||
						ny < 0 ||
						table.W <= nx ||
						table.H <= ny
						)
						continue;

					nextTable.Cells[nextTable.SpacePos.X, nextTable.SpacePos.Y] = nextTable.Cells[nx, ny];
					nextTable.Cells[nx, ny] = 0;
					nextTable.SpacePos = new I2Point(nx, ny);
					nextTable.SolveRoute.Add(d);

					q.Enqueue(nextTable);
				}
			}
			throw new Exception("Can not solve !!!");
		}
	}
}
