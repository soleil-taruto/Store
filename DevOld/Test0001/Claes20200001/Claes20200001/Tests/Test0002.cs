using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		/// <summary>
		/// すごろくで特定のマスに止まる確率
		/// </summary>
		public void Test01()
		{
			int TEST_COUNT = 1000000;
			//int TEST_COUNT = 100000;
			//int TEST_COUNT = 10000;
			//int TEST_COUNT = 1000;
			//int TEST_COUNT = 100;

			int マス目_NUM = 38;
			int[] mapマス目回数 = new int[マス目_NUM];

			for (int testCnt = 0; testCnt < TEST_COUNT; testCnt++)
			{
				int マス目Pos = 0;

				for (; ; )
				{
					mapマス目回数[マス目Pos]++;
					マス目Pos += SCommon.CRandom.GetRange(1, 6);

					if (マス目_NUM <= マス目Pos)
						break;
				}
			}
			for (int index = 0; index < マス目_NUM; index++)
			{
				Console.WriteLine(index.ToString("D2") + " ==> " + (mapマス目回数[index] * 1.0 / TEST_COUNT));
			}
		}
	}
}
