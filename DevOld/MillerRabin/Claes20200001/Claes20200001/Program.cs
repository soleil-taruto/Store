using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using Charlotte.SubCommons;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4();
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			Common.Pause();
		}

		private void Main4()
		{
			// -- choose one --

			//Test01();
			//Test02();
			//Test03();
			//Test04();
			//Test05();
			//Test06(); // 時間掛かる
			//new Test0001().Test01(); // 素数の密度
			new Test0001().Test02(); // 素数の密度(2)
			//new Test0001().Test03();

			// --
		}

		private void Test01()
		{
			for (int n = 0; n < 1000; n++)
				if (MillerRabin.IsPrime((ulong)n))
					Console.WriteLine(n);
		}

		private void Test02()
		{
			int pc = 0;

			for (int n = 0; n <= 100000000; n++)
			{
				if (MillerRabin.IsPrime((ulong)n))
					pc++;

				if (
					n == 10 ||
					n == 100 ||
					n == 1000 ||
					n == 10000 ||
					n == 100000 ||
					n == 1000000 ||
					n == 10000000 ||
					n == 100000000
					)
					Console.WriteLine(n + " ==> " + pc);
			}
		}

		private void Test03()
		{
			int pc = 0;

			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				uint n = SCommon.CRandom.GetUInt();

				Console.WriteLine(n);

				bool ans1 = MillerRabin.IsPrime((ulong)n);
				bool ans2 = Common.Simple_IsPrime((ulong)n);

				Console.WriteLine(ans1);
				Console.WriteLine(ans2);

				if (ans1 != ans2)
					throw null;

				if (ans1)
					pc++;
			}
			Console.WriteLine(pc);
		}

		private void Test04()
		{
			int pc = 0;

			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				ulong n = SCommon.CRandom.GetUInt64_M(1000000000000) + 4300000000; // 43億～1.0043兆

				Console.WriteLine(n);

				bool ans1 = MillerRabin.IsPrime(n);
				bool ans2 = Common.Simple_IsPrime(n);

				Console.WriteLine(ans1);
				Console.WriteLine(ans2);

				if (ans1 != ans2)
					throw null;

				if (ans1)
					pc++;
			}
			Console.WriteLine(pc);
		}

		private void Test05()
		{
			for (ulong n = 0; n <= 10000; n++) // 0付近
				Test05a(n);

			for (ulong n = 30000; n <= 40000; n++) // 15ビットの際
				Test05a(n);

			for (ulong n = 60000; n <= 70000; n++) // 16ビットの際
				Test05a(n);

			for (ulong n = 2147480000; n <= 2147490000; n++) // 31ビットの際
				Test05a(n);

			for (ulong n = 4294960000; n <= 4294970000; n++) // 32ビットの際
				Test05a(n);

			for (int testcnt = 0; testcnt < 30000; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(1000000000000)); // 0～1兆

			for (int testcnt = 0; testcnt < 10000; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(10000000000000)); // 0～10兆

			for (int testcnt = 0; testcnt < 3000; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(100000000000000)); // 0～100兆

			for (int testcnt = 0; testcnt < 1000; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(1000000000000000)); // 0～1000兆

			for (int testcnt = 0; testcnt < 300; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(10000000000000000)); // 0～1京

			for (int testcnt = 0; testcnt < 100; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(100000000000000000)); // 0～10京

			for (int testcnt = 0; testcnt < 30; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(1000000000000000000)); // 0～100京

			for (int testcnt = 0; testcnt < 10; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(10000000000000000000)); // 0～1000京
		}

		private void Test05a(ulong n)
		{
			Console.WriteLine(n);

			bool ans1 = MillerRabin.IsPrime(n);
			bool ans2 = Common.Simple_IsPrime(n);

			if (ans1 != ans2)
				throw null;
		}

		private void Test06()
		{
			for (ulong n = 18446744073709551615; 18446744073709540000 <= n; n--) // 2^64付近
				Test06a(n);

			for (ulong n = 9223372036854770000; n <= 9223372036854780000; n++) // 2^63付近
				Test06a(n);

			for (int testcnt = 0; testcnt < 10000; testcnt++)
				Test05a(SCommon.CRandom.GetUInt64_M(10000000000000000000)); // 0～1垓
		}

		private void Test06a(ulong n)
		{
			Console.WriteLine(n);

			if (MillerRabin.IsPrime(n))
			{
				if (10000000000 < n) // ? 100億 <
				{
					if (n % 2 == 0)
						throw null;

					for (ulong d = 3; d < 100000; d += 2)
						if (n % d == 0)
							throw null;

					Console.WriteLine("多分素数");
				}
				else
				{
					if (!Common.Simple_IsPrime(n))
						throw null;
				}
			}
			else
			{
				if (Common.Simple_IsPrime(n))
					throw null;
			}
		}
	}
}
