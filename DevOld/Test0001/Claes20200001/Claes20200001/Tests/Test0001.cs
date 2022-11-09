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
			SCommon.ToThrowPrint(() => SCommon.ToThrow(() => { }));
		}

		public void Test02()
		{
			for (int exponent = 33; exponent <= 64; exponent++)
			{
				double val = Math.Pow(2, exponent);

				Console.WriteLine(exponent + " --> " + T2_Fltr01(val.ToString("e0")));
			}
		}

		private string T2_Fltr01(string str)
		{
			str = str.Replace("+0", "+");
			str = str.Replace("+0", "+");
			return str;
		}

		public void Test03()
		{
			Console.WriteLine("めじょまっきーん");
		}
	}
}
