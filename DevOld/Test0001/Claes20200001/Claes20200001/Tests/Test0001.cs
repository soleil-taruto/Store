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
			Test01_a(100, 30000);
			Test01_a(300, 10000);
			Test01_a(1000, 3000);
			Test01_a(3000, 1000);
			Test01_a(10000, 300);
			Test01_a(30000, 100);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int dataSizeLmt, int testCount)
		{
			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				byte[] src = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(dataSizeLmt));
				string enc = SCommon.Base64.I.Encode(src);
				byte[] dec = SCommon.Base64.I.Decode(enc);

				if (SCommon.Comp(src, dec) != 0) // ? 不一致
					throw null;
			}
			Console.WriteLine("OK");
		}

		public void Test02()
		{
			Test02_a(100, 30000);
			Test02_a(300, 10000);
			Test02_a(1000, 3000);
			Test02_a(3000, 1000);
			Test02_a(10000, 300);
			Test02_a(30000, 100);

			Console.WriteLine("OK!");
		}

		private void Test02_a(int dataSizeLmt, int testCount)
		{
			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				byte[] src = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(dataSizeLmt));
				string enc = SCommon.Base64.I.Encode(src);

				// Change enc
				{
					enc = enc.Replace("=", "");

					for (int c = 80; c + 30 < enc.Length; c += 100)
					{
						enc = enc.Substring(0, c) + "\r\n\t" + enc.Substring(c);
					}
				}

				byte[] dec = SCommon.Base64.I.Decode(enc);

				if (SCommon.Comp(src, dec) != 0) // ? 不一致
					throw null;
			}
			Console.WriteLine("OK");
		}
	}
}
