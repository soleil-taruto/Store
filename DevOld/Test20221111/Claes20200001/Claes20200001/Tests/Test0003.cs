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
			foreach (char chr in SCommon.E_InsertRange("ABCDEF", 3, "123"))
			{
				Console.Write(chr);
			}
			Console.WriteLine("");

			{
				string str = new string(SCommon.E_InsertRange("ABCDEF", 3, "123").ToArray());

				Console.WriteLine(str);
			}

			foreach (char chr in SCommon.E_RemoveTrail("ABCDEF", 3))
			{
				Console.Write(chr);
			}
			Console.WriteLine("");

			{
				string str = new string(SCommon.E_RemoveTrail("ABCDEF", 3).ToArray());

				Console.WriteLine(str);
			}
		}
	}
}
