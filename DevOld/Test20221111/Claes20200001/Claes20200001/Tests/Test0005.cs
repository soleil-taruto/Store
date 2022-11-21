using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0005
	{
		public void Test01()
		{
			const int ARR_LEN = 50000000;

			{
				int[] arr = Enumerable.Range(1, ARR_LEN).ToArray();

				PrintExecuteTime(() => Console.WriteLine(arr.Count())); // 0.0s
				PrintExecuteTime(() => Console.WriteLine(arr.Count())); // 0.0s
				PrintExecuteTime(() => Console.WriteLine(arr.Count())); // 0.0s
			}

			{
				List<int> list = new List<int>();

				foreach (int value in Enumerable.Range(1, ARR_LEN))
					list.Add(value);

				PrintExecuteTime(() => Console.WriteLine(list.Count())); // 0.0s
				PrintExecuteTime(() => Console.WriteLine(list.Count())); // 0.0s
				PrintExecuteTime(() => Console.WriteLine(list.Count())); // 0.0s
			}

			{
				int[] arr = Enumerable.Range(1, ARR_LEN).ToArray();

				IEnumerable<int> enu = arr.Take(arr.Length);

				PrintExecuteTime(() => Console.WriteLine(enu.Count())); // 0.3s
				PrintExecuteTime(() => Console.WriteLine(enu.Count())); // 0.3s
				PrintExecuteTime(() => Console.WriteLine(enu.Count())); // 0.3s
			}

			{
				List<int> list = new List<int>();

				foreach (int value in Enumerable.Range(1, ARR_LEN))
					list.Add(value);

				IEnumerable<int> enu = list.Take(list.Count);

				PrintExecuteTime(() => Console.WriteLine(enu.Count())); // 0.5s
				PrintExecuteTime(() => Console.WriteLine(enu.Count())); // 0.5s
				PrintExecuteTime(() => Console.WriteLine(enu.Count())); // 0.5s
			}
		}

		private static void PrintExecuteTime(Action routine)
		{
			DateTime stTm = DateTime.Now;
			routine();
			DateTime edTm = DateTime.Now;
			Console.WriteLine((edTm - stTm).TotalSeconds.ToString("F9") + " 秒");
		}

		public void Test02()
		{
			Test02_a(3, 100);
			Test02_a(5, 100);
			Test02_a(7, 100);
			Test02_a(10, 100);
			Test02_a(100, 100);
			Test02_a(1000, 100);
			Test02_a(10000, 100);

			Console.WriteLine("OK!");
		}

		private int T2_ErrorCount;

		private void Test02_a(int scale, int testCount)
		{
			ProcMain.WriteLog(scale + ", " + testCount);

			T2_ErrorCount = 0;

			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				Test02_b(scale);
			}

			ProcMain.WriteLog("OK " + T2_ErrorCount + " / " + (testCount * 4));
		}

		private void Test02_b(int scale)
		{
			string[] arr =
				Enumerable.Range(0, SCommon.CRandom.GetInt(scale)).Select(dummy => "" + SCommon.CRandom.GetULong()).ToArray();
			string[] arrForInsert =
				Enumerable.Range(0, SCommon.CRandom.GetInt(scale)).Select(dummy => "" + SCommon.CRandom.GetULong()).ToArray();
			int index = SCommon.CRandom.GetInt(scale);
			int count = SCommon.CRandom.GetInt(scale);

			Action<Func<string[]>, Func<string[]>> tester = (f1, f2) =>
			{
				string[] ret1;
				bool error;

				try
				{
					ret1 = f1();
					error = false;
				}
				catch
				{
					ret1 = null;
					error = true;
				}

				if (error)
				{
					SCommon.ToThrow(() => f2());
				}
				else
				{
					string[] ret2 = f2();

					if (ret1 == null)
						throw null;

					if (ret2 == null)
						throw null;

					if (SCommon.Comp(ret1, ret2, SCommon.Comp) != 0)
						throw null;
				}

				if (error)
					T2_ErrorCount++;
			};

			tester(
				() => SCommon.E_RemoveRange(arr, index, count).ToArray(),
				() => SCommon.A_RemoveRange(arr, index, count));
			tester(
				() => SCommon.E_RemoveTrail(arr, count).ToArray(),
				() => SCommon.A_RemoveTrail(arr, count));
			tester(
				() => SCommon.E_InsertRange(arr, index, arrForInsert).ToArray(),
				() => SCommon.A_InsertRange(arr, index, arrForInsert));
			tester(
				() => SCommon.E_AddRange(arr, arrForInsert).ToArray(),
				() => SCommon.A_AddRange(arr, arrForInsert));
		}
	}
}
