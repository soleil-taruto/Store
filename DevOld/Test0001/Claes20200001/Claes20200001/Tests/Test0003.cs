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
			Test01_a(int.MinValue, int.MinValue + 10000);
			Test01_a(-10000, 10000);
			Test01_a(int.MaxValue - 10000, int.MaxValue);

			Test01_b(long.MinValue, long.MinValue + 10000);
			Test01_b((long)int.MinValue - 10000, (long)int.MinValue + 10000);
			Test01_b(-10000, 10000);
			Test01_b((long)int.MaxValue - 10000, (long)int.MaxValue + 10000);
			Test01_b(long.MaxValue - 10000, long.MaxValue);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int minval, int maxval)
		{
			for (int value = minval; ; value++)
			{
				byte[] data = SCommon.IntToBytes(value);
				int ret = SCommon.ToInt(data);

				if (ret != value)
					throw null;

				if (value == maxval)
					break;
			}
		}

		private void Test01_b(long minval, long maxval)
		{
			for (long value = minval; ; value++)
			{
				byte[] data = SCommon.LongToBytes(value);
				long ret = SCommon.ToLong(data);

				if (ret != value)
					throw null;

				if (value == maxval)
					break;
			}
		}
	}
}
