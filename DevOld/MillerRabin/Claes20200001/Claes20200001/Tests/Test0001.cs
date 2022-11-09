using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;
using Charlotte.SubCommons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		/// <summary>
		/// 素数の密度
		/// </summary>
		public void Test01()
		{
			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				GetSosuuMitsudo(writer, 0);
				GetSosuuMitsudo(writer, 100000000);
				GetSosuuMitsudo(writer, 200000000);
				GetSosuuMitsudo(writer, 300000000);
				GetSosuuMitsudo(writer, 400000000);
				GetSosuuMitsudo(writer, 500000000);
				GetSosuuMitsudo(writer, 600000000);
				GetSosuuMitsudo(writer, 700000000);
				GetSosuuMitsudo(writer, 800000000);
				GetSosuuMitsudo(writer, 900000000);

				GetSosuuMitsudo(writer, 1000000000);
				GetSosuuMitsudo(writer, 3000000000);
				GetSosuuMitsudo(writer, 10000000000);
				GetSosuuMitsudo(writer, 30000000000);
				GetSosuuMitsudo(writer, 100000000000);
				GetSosuuMitsudo(writer, 300000000000);
				GetSosuuMitsudo(writer, 1000000000000);
				GetSosuuMitsudo(writer, 3000000000000);
				GetSosuuMitsudo(writer, 10000000000000);
				GetSosuuMitsudo(writer, 30000000000000);
				GetSosuuMitsudo(writer, 100000000000000);
				GetSosuuMitsudo(writer, 300000000000000);
				GetSosuuMitsudo(writer, 1000000000000000);
				GetSosuuMitsudo(writer, 3000000000000000);
				GetSosuuMitsudo(writer, 10000000000000000);
				GetSosuuMitsudo(writer, 30000000000000000);
				GetSosuuMitsudo(writer, 100000000000000000);
				GetSosuuMitsudo(writer, 300000000000000000);
				GetSosuuMitsudo(writer, 1000000000000000000);
				GetSosuuMitsudo(writer, 3000000000000000000);
				GetSosuuMitsudo(writer, 10000000000000000000);
			}
		}

		private void GetSosuuMitsudo(CsvFileWriter writer, ulong valBase)
		{
			Console.WriteLine(valBase); // cout

			const int TEST_CNT_MAX = 1000000;
			//const int TEST_CNT_MAX = 10000; // テスト用

			const int VAL_RANGE = 100000000;

			int pc = 0;

			for (int testcnt = 0; testcnt < TEST_CNT_MAX; testcnt++)
			{
				ulong val = valBase + SCommon.CRandom.GetUInt_M(VAL_RANGE);

				if (MillerRabin.IsPrime(val))
					pc++;
			}

			writer.WriteCell(valBase.ToString());
			writer.WriteCell((valBase + VAL_RANGE).ToString());
			writer.WriteCell(((double)pc / TEST_CNT_MAX).ToString("F3"));
			writer.EndRow();
		}

		public void Test02()
		{
			// 2022.3.26
			// (10^n)以下の自然数には、だいたい(2.2*n)個に1個の割合で素数が含まれている。(n=1～19)
			// ----
			// この(2.2)は(10^10)でほぼ一致する。
			// 10^5  -> 2.0876 (実測)
			// 10^10 -> 2.1953 (実測)
			// 10^15 -> 2.2229 (実測)
			// 10^19 -> 2.2423 (実測)
			// 10^28 -> 2.266 (Wikiの記事から)
			// ----
			// というわけで、以下のとおり覚えておくことにする。
			// (10^10)以下の自然数に含まれる素数の割合は、ほぼ(1/22)
			// (10^20)以下の自然数に含まれる素数の割合は、ほぼ(1/45)
			// (10^n)以下の自然数に含まれる素数の割合は、だいたい(1/(2.2*n)), 但しnが小さい場合のみ(n=1～20)

			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				GetSosuuMitsudo2(writer, 10); // 10^1
				GetSosuuMitsudo2(writer, 100);
				GetSosuuMitsudo2(writer, 1000);
				GetSosuuMitsudo2(writer, 10000);
				GetSosuuMitsudo2(writer, 100000); // 10^5
				GetSosuuMitsudo2(writer, 1000000);
				GetSosuuMitsudo2(writer, 10000000);
				GetSosuuMitsudo2(writer, 100000000);
				GetSosuuMitsudo2(writer, 1000000000);
				GetSosuuMitsudo2(writer, 10000000000); // 10^10
				GetSosuuMitsudo2(writer, 100000000000);
				GetSosuuMitsudo2(writer, 1000000000000);
				GetSosuuMitsudo2(writer, 10000000000000);
				GetSosuuMitsudo2(writer, 100000000000000);
				GetSosuuMitsudo2(writer, 1000000000000000); // 10^15
				GetSosuuMitsudo2(writer, 10000000000000000);
				GetSosuuMitsudo2(writer, 100000000000000000);
				GetSosuuMitsudo2(writer, 1000000000000000000);
				GetSosuuMitsudo2(writer, 10000000000000000000); // 10^19
			}
		}

		private void GetSosuuMitsudo2(CsvFileWriter writer, ulong valMax)
		{
			Console.WriteLine(valMax); // cout

			const int TEST_CNT_MAX = 1000000;

			int pc = 0;

			for (int testcnt = 0; testcnt < TEST_CNT_MAX; testcnt++)
			{
				ulong val = SCommon.CRandom.GetUInt64_M(valMax) + 1;

				if (MillerRabin.IsPrime(val))
					pc++;
			}

			writer.WriteCell(valMax.ToString());
			writer.WriteCell(((double)pc / TEST_CNT_MAX).ToString("F3"));
			writer.WriteCell(((double)TEST_CNT_MAX / pc).ToString("F3"));
			writer.EndRow();
		}
	}
}
