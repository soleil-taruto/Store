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
			using (CsvFileWriter writer = new CsvFileWriter(SCommon.NextOutputPath() + ".csv"))
			{
				writer.WriteRow(new string[]
				{
					"GetBoolean",
					"GetSign",
					"GetReal1",
					"GetReal2",
					"GetReal3(100,200)",
				});

				for (int testcnt = 0; testcnt < 1000; testcnt++)
				{
					writer.WriteRow(new string[]
					{
						SCommon.CRandom.GetBoolean().ToString(),
						SCommon.CRandom.GetSign().ToString(),
						SCommon.CRandom.GetReal1().ToString("F9"),
						SCommon.CRandom.GetReal2().ToString("F9"),
						SCommon.CRandom.GetReal3(100, 200).ToString("F9"),
					});
				}
			}
		}
	}
}
