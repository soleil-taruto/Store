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
			Test01_a(SCommon.CreateSet());
			Test01_a(new HashSet<string>(new IECompTest()));
		}

		private void Test01_a(HashSet<string> hs)
		{
			//const int C_MAX = 3000;
			const int C_MAX = 10000;

			ProcMain.WriteLog("ST");

			for (int c = 0; c < C_MAX; c++)
				hs.Add("" + c);

			ProcMain.WriteLog("P1");

			for (int c = 0; c < C_MAX; c++)
				if (!hs.Contains("" + c))
					throw null;

			ProcMain.WriteLog("P2");

			for (int c = 0; c < C_MAX; c++)
				if (hs.Contains("" + (C_MAX + c)))
					throw null;

			ProcMain.WriteLog("P3");

			for (int c = 0; c < C_MAX; c++)
				hs.Remove("" + c);

			ProcMain.WriteLog("P4");

			for (int c = 0; c < C_MAX; c++)
				if (hs.Contains("" + c))
					throw null;

			ProcMain.WriteLog("ED");
		}

		private class IECompTest : IEqualityComparer<string>
		{
			public bool Equals(string a, string b)
			{
				return a == b;
			}

			public int GetHashCode(string a)
			{
				return 0;
			}
		}
	}
}
