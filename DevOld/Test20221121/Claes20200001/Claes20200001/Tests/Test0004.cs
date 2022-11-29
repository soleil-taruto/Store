using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0004
	{
		public void Test01()
		{
			Test01_a(1, 3, 3, 7);
		}

		private class OperandInfo
		{
			public int Numer;
			public int Denom;
			public string Str;
		}

		private void Test01_a(params int[] ns)
		{
			Test01_b(ns.Select(v => new OperandInfo() { Numer = v, Denom = 1, Str = "" + v }).ToArray());
		}

		private void Test01_b(OperandInfo[] os)
		{
			if (os.Length == 1)
			{
				if (
					os[0].Numer / os[0].Denom == 10 &&
					os[0].Numer % os[0].Denom == 0
					)
					Console.WriteLine(os[0].Str);

				return;
			}

			for (int b = 1; b < os.Length; b++)
			{
				for (int a = 0; a < b; a++)
				{
					OperandInfo[] next = os;

					next = SCommon.A_RemoveRange(next, b, 1);
					next = SCommon.A_RemoveRange(next, a, 1);

					// a + b
					{
						OperandInfo o = new OperandInfo()
						{
							Numer = os[a].Numer * os[b].Denom + os[b].Numer * os[a].Denom,
							Denom = os[a].Denom * os[b].Denom,
							Str = "(" + os[a].Str + " + " + os[b].Str + ")",
						};

						Test01_b(SCommon.A_AddRange(next, new OperandInfo[] { o }));
					}

					// a - b
					{
						OperandInfo o = new OperandInfo()
						{
							Numer = os[a].Numer * os[b].Denom - os[b].Numer * os[a].Denom,
							Denom = os[a].Denom * os[b].Denom,
							Str = "(" + os[a].Str + " - " + os[b].Str + ")",
						};

						Test01_b(SCommon.A_AddRange(next, new OperandInfo[] { o }));
					}

					// b - a
					{
						OperandInfo o = new OperandInfo()
						{
							Numer = os[b].Numer * os[a].Denom - os[a].Numer * os[b].Denom,
							Denom = os[a].Denom * os[b].Denom,
							Str = "(" + os[b].Str + " - " + os[a].Str + ")",
						};

						Test01_b(SCommon.A_AddRange(next, new OperandInfo[] { o }));
					}

					// a * b
					{
						OperandInfo o = new OperandInfo()
						{
							Numer = os[a].Numer * os[b].Numer,
							Denom = os[a].Denom * os[b].Denom,
							Str = "(" + os[a].Str + " * " + os[b].Str + ")",
						};

						Test01_b(SCommon.A_AddRange(next, new OperandInfo[] { o }));
					}

					// a / b
					if (os[b].Numer != 0)
					{
						OperandInfo o = new OperandInfo()
						{
							Numer = os[a].Numer * os[b].Denom,
							Denom = os[a].Denom * os[b].Numer,
							Str = "(" + os[a].Str + " / " + os[b].Str + ")",
						};

						Test01_b(SCommon.A_AddRange(next, new OperandInfo[] { o }));
					}

					// b / a
					if (os[a].Numer != 0)
					{
						OperandInfo o = new OperandInfo()
						{
							Numer = os[a].Denom * os[b].Numer,
							Denom = os[a].Numer * os[b].Denom,
							Str = "(" + os[b].Str + " / " + os[a].Str + ")",
						};

						Test01_b(SCommon.A_AddRange(next, new OperandInfo[] { o }));
					}
				}
			}
		}

		// ====

		public void Test02()
		{
			//Test02_a(1, 3, 3, 7);
			//Test02_a(3, 4, 7, 8);
			//Test02_a(5, 6, 6, 9);
			//Test02_a(4, 6, 7, 9);
			//Test02_a(1, 3, 8, 8);
			//Test02_a(3, 4, 8, 8);
			//Test02_a(1, 1, 5, 8);
			//Test02_a(4, 8, 8, 8);
			//Test02_a(2, 2, 8, 9);
			//Test02_a(3, 4, 6, 6);
			//Test02_a(1, 1, 9, 9);
			//Test02_a(4, 6, 6, 9);
			//Test02_a(6, 6, 7, 8);
			//Test02_a(6, 6, 9, 9);
			Test02_a(3, 4, 7, 7);
		}

		private class Operand
		{
			public int Numer;
			public int Denom;
			public string Str;

			public bool IsInt()
			{
				return this.Numer % this.Denom == 0;
			}

			public int GetIntValue()
			{
				return this.Numer / this.Denom;
			}
		}

		private void Test02_a(params int[] ns)
		{
			Test02_b(ns.Select(v => new Operand() { Numer = v, Denom = 1, Str = "" + v }).ToArray());
		}

		private void Test02_b(Operand[] os)
		{
			if (os.Length == 1)
			{
				Operand o = os[0];

				if (
					o.IsInt() &&
					o.GetIntValue() == 10
					)
					Console.WriteLine(o.Str);

				return;
			}

			for (int a = 0; a < os.Length; a++)
			{
				for (int b = 0; b < os.Length; b++)
				{
					if (a == b)
						continue;

					Operand[] next = os.ToArray();
					next[a] = null;
					next[b] = null;
					next = next.Where(v => v != null).Concat(new Operand[] { null }).ToArray();
					int x = next.Length - 1;

					next[x] = new Operand()
					{
						Numer = os[a].Numer * os[b].Denom + os[b].Numer * os[a].Denom,
						Denom = os[a].Denom * os[b].Denom,
						Str = "(" + os[a].Str + " + " + os[b].Str + ")",
					};

					Test02_b(next);

					next[x] = new Operand()
					{
						Numer = os[a].Numer * os[b].Denom - os[b].Numer * os[a].Denom,
						Denom = os[a].Denom * os[b].Denom,
						Str = "(" + os[a].Str + " - " + os[b].Str + ")",
					};

					Test02_b(next);

					next[x] = new Operand()
					{
						Numer = os[a].Numer * os[b].Numer,
						Denom = os[a].Denom * os[b].Denom,
						Str = "(" + os[a].Str + " * " + os[b].Str + ")",
					};

					Test02_b(next);

					if (os[b].Numer != 0)
					{
						next[x] = new Operand()
						{
							Numer = os[a].Numer * os[b].Denom,
							Denom = os[a].Denom * os[b].Numer,
							Str = "(" + os[a].Str + " / " + os[b].Str + ")",
						};

						Test02_b(next);
					}
				}
			}
		}
	}
}
