using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0009
	{
		/// <summary>
		/// 1/1/1 ～ 9999/12/31
		/// 存在する日付
		/// </summary>
		public void Test01()
		{
			string file = Common.NextOutputPath() + ".txt";

			using (StreamWriter writer = new StreamWriter(file, false, Encoding.UTF8))
			{
				for (int y = 1; y <= 9999; y++)
				{
					for (int m = 1; m <= 12; m++)
					{
						int dNum = GetDayOfMonth(y, m);

						for (int d = 1; d <= dNum; d++)
						{
							JapaneseDate date = new JapaneseDate(y * 10000 + m * 100 + d);

							writer.WriteLine(string.Format("{0:D4}/{1:D2}/{2:D2} ⇒ {3}"
								, y
								, m
								, d
								, date.ToString()));
						}
					}
				}
			}
		}

		private static int GetDayOfMonth(int y, int m)
		{
			int d = new int[] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }[m - 1];

			if (d == -1)
			{
				bool leapYear = y % 4 == 0 && (y % 100 != 0 || y % 400 == 0);

				if (leapYear)
					d = 29;
				else
					d = 28;
			}
			return d;
		}

		/// <summary>
		/// 0/0/0 ～ 9999/12/31
		/// </summary>
		public void Test02()
		{
			string file = Common.NextOutputPath() + ".txt";

			using (StreamWriter writer = new StreamWriter(file, false, Encoding.UTF8))
			{
				for (int y = 0; y <= 9999; y++)
				{
					for (int m = 0; m <= 12; m++)
					{
						for (int d = 0; d <= 31; d++)
						{
							JapaneseDate date = new JapaneseDate(y * 10000 + m * 100 + d);

							writer.WriteLine(string.Format("{0:D4}/{1:D2}/{2:D2} ⇒ {3}"
								, y
								, m
								, d
								, date.ToString()));
						}
					}
				}
			}
		}

		/// <summary>
		/// 0/0/0 ～ 9999/99/99
		/// </summary>
		public void Test03()
		{
			string file = Common.NextOutputPath() + ".txt";

			using (StreamWriter writer = new StreamWriter(file, false, Encoding.UTF8))
			{
				for (int ymd = 0; ymd < 100000000; ymd += SCommon.CRandom.GetRange(1, 100))
				{
					JapaneseDate date = new JapaneseDate(ymd);

					int y = ymd / 10000;
					int m = (ymd / 100) % 100;
					int d = ymd % 100;

					writer.WriteLine(string.Format("{0:D4}/{1:D2}/{2:D2} ⇒ {3}"
						, y
						, m
						, d
						, date.ToString()));
				}
			}
		}

		public void Test04()
		{
			for (int ymd = 0; ymd < 21000000; ymd += SCommon.CRandom.GetRange(1, 100))
			{
				int y = ymd / 10000;
				int m = (ymd / 100) % 100;
				int d = ymd % 100;

				JapaneseDate date1 = new JapaneseDate(ymd);

				int y1 = date1.Y;
				int m1 = date1.M;
				int d1 = date1.D;

				string str1 = date1.ToString();

				JapaneseDate date2 = JapaneseDate.Create(str1);

				int y2 = date2.Y;
				int m2 = date2.M;
				int d2 = date2.D;

				string str2 = date2.ToString();

				Console.WriteLine(string.Format("{0:D4}/{1:D2}/{2:D2} -> {3} -> {4:D4}/{5:D2}/{6:D2} -> {7}"
					, y1, m1, d1
					, str1
					, y2, m2, d2
					, str2
					));

				if (
					y != y1 ||
					m != m1 ||
					d != d1 ||
					y1 != y2 ||
					m1 != m2 ||
					d1 != d2 ||
					str1 != str2
					)
					throw null;
			}
		}

		public void Test05()
		{
			for (int ymd = 0; ymd < 21000000; ymd++)
			{
				int y = ymd / 10000;
				int m = (ymd / 100) % 100;
				int d = ymd % 100;

				if (
					12 < m ||
					31 < d
					)
					continue;

				JapaneseDate date1 = new JapaneseDate(ymd);

				int y1 = date1.Y;
				int m1 = date1.M;
				int d1 = date1.D;

				string str1 = date1.ToString();

				JapaneseDate date2 = JapaneseDate.Create(str1);

				int y2 = date2.Y;
				int m2 = date2.M;
				int d2 = date2.D;

				string str2 = date2.ToString();

				Console.WriteLine(string.Format("{0:D4}/{1:D2}/{2:D2} -> {3} -> {4:D4}/{5:D2}/{6:D2} -> {7}"
					, y1, m1, d1
					, str1
					, y2, m2, d2
					, str2
					));

				if (
					y != y1 ||
					m != m1 ||
					d != d1 ||
					y1 != y2 ||
					m1 != m2 ||
					d1 != d2 ||
					str1 != str2
					)
					throw null;
			}
		}
	}
}
