using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			for (int c = -20; c <= 20; c++)
			{
				ProcMain.WriteLog(c + " -> " + (c % 8));
			}
		}
	}
}
