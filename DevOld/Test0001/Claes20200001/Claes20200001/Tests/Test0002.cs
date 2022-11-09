using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			Test01_a(1000, 10);
			Test01_a(100, 100);
			Test01_a(10, 300);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int testCount, int nMax)
		{
			char[] S_CHARS = "dp".ToArray();

			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				string s = new string(Enumerable.Range(0, SCommon.CRandom.GetRange(0, nMax)).Select(dummy => SCommon.CRandom.ChooseOne(S_CHARS)).ToArray());

				string ans1 = Test01_b1(s);
				string ans2 = Test01_b2(s);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private string Test01_b1(string s)
		{
			string best = s;
			int p = s.IndexOf('p');

			if (p != -1)
			{
				int q = p;

				for (; ; )
				{
					q = s.IndexOf("pd", q);

					if (q == -1)
						break;

					q++;
					string t = s.Substring(0, p) + Turn(s.Substring(p, q - p)) + s.Substring(q);

					if (SCommon.Comp(best, t) > 0)
						best = t;
				}

				{
					string t = s.Substring(0, p) + Turn(s.Substring(p));

					if (SCommon.Comp(best, t) > 0)
						best = t;
				}
			}
			return best;
		}

		private string Turn(string str)
		{
			str = new string(str.Reverse().ToArray());
			str = str.Replace('d', 'x');
			str = str.Replace('p', 'd');
			str = str.Replace('x', 'p');
			return str;
		}

		private string Test01_b2(string s)
		{
			string best = s;

			for (int start = 0; start < s.Length; start++)
			{
				for (int count = 1; start + count <= s.Length; count++)
				{
					char[] buff = s.ToArray();

					int l = start;
					int r = start + count - 1;

					while (l < r)
					{
						char a = buff[l];
						char b = buff[r];

						a = ChangeDP(a);
						b = ChangeDP(b);

						buff[l] = b;
						buff[r] = a;

						l++;
						r--;
					}
					if (l == r)
						buff[l] = ChangeDP(buff[l]);

					string t = new string(buff);

					if (SCommon.Comp(best, t) > 0)
						best = t;
				}
			}
			return best;
		}

		private char ChangeDP(char chr)
		{
			return chr == 'd' ? 'p' : 'd';
		}
	}
}
