using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			RndCtr = (ulong)DateTime.Now.Ticks;

			for (int c = 0; c < 9000; c++)
			{
				Console.WriteLine(GetRandULong());
			}
		}

		// ====
		// Random
		// ====

		public static ulong RndCtr = 1;

		public static ulong GetRandULong() // Xorshift-64
		{
			RndCtr ^= RndCtr << 13;
			RndCtr ^= RndCtr >> 7;
			RndCtr ^= RndCtr << 17;
			return RndCtr;
		}

		// ====
	}
}
