using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using System.Globalization;

namespace Charlotte.Tests
{
	public class Test0010
	{
		public void Test01()
		{
			for (int count = 1; count < 10000; count++)
			{
				Test01_a(count);
			}

			Console.WriteLine("OK!");
		}

		private void Test01_a(int count)
		{
			string[] lines = new string[count];

			lines[0] = "Ines";

			// ----

			int range;
			for (range = 1; range * 2 <= count; range *= 2)
			{
				CopyRangeToPoint(lines, 0, range, range);
			}
			if (range < count)
			{
				CopyRangeToPoint(lines, 0, count - range, range);
			}

			// ----

			if (lines.Any(v => v == null)) throw null; // assert
		}

		private void CopyRangeToPoint<T>(T[] list, int startPoint, int endPoint, int outputStartPoint)
		{
			for (int c = 0; ; c++)
			{
				int p = startPoint + c;
				int q = outputStartPoint + c;

				if (endPoint <= p)
					break;

				if (list[p] == null) throw null; // assert
				if (list[q] != null) throw null; // assert

				list[q] = list[p];
			}
		}

		public void Test02()
		{
			for (int y = 1800; y <= 2200; y++)
			//for (int y = 1; y <= 9999; y++)
			{
				int m = 1;
				int d = 1;

				string gengou;
				try
				{
					CultureInfo ci = new CultureInfo("ja-JP", false);
					ci.DateTimeFormat.Calendar = new JapaneseCalendar();
					gengou = new DateTime(y, m, d).ToString("gg", ci);
				}
				catch
				{
					gengou = "----";
				}

				Console.WriteLine(string.Format("{0:D4}/{1:D2}/{2:D2} ⇒ {3}", y, m, d, gengou));
			}
		}

		public void Test03()
		{
			const int MIN_YMD = 10101;
			const int MAX_YMD = 99991231;

			if (P_GetGengou(MIN_YMD) != null) throw null; // 想定外
			if (P_GetGengou(MAX_YMD) == null) throw null; // 想定外

			List<string[]> eraFirstDateList = new List<string[]>();
			int ymd = MAX_YMD;

			for (; ; )
			{
				string gengou = P_GetGengou(ymd);

				if (gengou == null)
					break;

				int l = P_DateToDay(MIN_YMD);
				int r = P_DateToDay(ymd);

				while (l + 1 < r)
				{
					int m = (l + r) / 2;
					int mYMD = P_DayToDate(m);
					string mGengou = P_GetGengou(mYMD);

					if (mGengou == gengou)
						r = m;
					else
						l = m;
				}
				ymd = P_DayToDate(l);
				eraFirstDateList.Add(new string[] { "" + P_DayToDate(r), gengou });
			}
			eraFirstDateList.Reverse();

			// ----

			foreach (string[] eraFirstDate in eraFirstDateList)
			{
				Console.WriteLine(eraFirstDate[0] + " is first date of " + eraFirstDate[1]);
			}
		}

		private static int P_DateToDay(int ymd)
		{
			return (int)(SCommon.TimeStampToSec.ToSec(ymd * 1000000L) / 86400L);
		}

		private static int P_DayToDate(int day)
		{
			return (int)(SCommon.TimeStampToSec.ToTimeStamp(day * 86400L) / 1000000L);
		}

		private static string P_GetGengou(int ymd)
		{
			if (ymd < 10101 || 99991231 < ymd)
				throw new ArgumentException();

			int y = ymd / 10000;
			int m = (ymd / 100) % 100;
			int d = ymd % 100;

			string gengou;
			try
			{
				CultureInfo ci = new CultureInfo("ja-JP", false);
				ci.DateTimeFormat.Calendar = new JapaneseCalendar();
				gengou = new DateTime(y, m, d).ToString("gg", ci);
			}
			catch
			{
				gengou = null;
			}
			return gengou;
		}
	}
}
