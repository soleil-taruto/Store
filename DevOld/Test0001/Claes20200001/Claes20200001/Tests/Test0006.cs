using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0006
	{
		public void Test01()
		{
			Test01_a("");
			Test01_a("A");
			Test01_a("AB");
			Test01_a("ABC");
			Test01_a("Wikipedia"); // -> 11E60398 hex @ https://ja.wikipedia.org/wiki/Adler-32
		}

		private void Test01_a(string input)
		{
			Console.WriteLine("[" + input + "] -> " + Adler32.ComputeHash(Encoding.ASCII.GetBytes(input)).ToString("x8"));
		}
	}
}
