using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.CSSolutions;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			CSRenameVarsFilter filter = new CSRenameVarsFilter();
			int[] mapNameLenCount = new int[100];

			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				string name = filter.ForTest_Get似非英語名();

				mapNameLenCount[name.Length]++;
			}
			for (int index = 0; index < mapNameLenCount.Length; index++)
			{
				Console.WriteLine(index.ToString("D3") + " ==> " + mapNameLenCount[index]);
			}
			Common.Pause();
		}
	}
}
