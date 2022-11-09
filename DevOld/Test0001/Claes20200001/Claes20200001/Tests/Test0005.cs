using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0005
	{
		public void Test01()
		{
			if (LiteFormat("123456789") != "9") throw null;
			if (LiteFormat("2022/10/26") != "9/9/9") throw null;
			if (LiteFormat("T-1000") != "A-9") throw null;
			if (LiteFormat("いろはにほへと") != "J") throw null;

			Console.WriteLine("OK!");
		}

		private string LiteFormat(string str)
		{
			SCommon.DECIMAL.Substring(0, 9).ForEach(chr => str = str.Replace(chr, '9'));
			SCommon.ALPHA.Substring(1).ForEach(chr => str = str.Replace(chr, 'A'));
			SCommon.alpha.Substring(1).ForEach(chr => str = str.Replace(chr, 'a'));
			SCommon.GetJChars().ForEach(chr => str = str.Replace(chr, 'J'));

			for (int c = 0; c < 20; c++)
			{
				str = str.Replace("99", "9");
				str = str.Replace("AA", "A");
				str = str.Replace("aa", "a");
				str = str.Replace("JJ", "J");
			}
			return str;
		}
	}
}
