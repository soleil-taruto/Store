using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static void Pause()
		{
			Console.WriteLine("Press ENTER key.");
			Console.ReadLine();
		}

		#region GetOutputDir

		private static string GOD_Dir;

		public static string GetOutputDir()
		{
			if (GOD_Dir == null)
				GOD_Dir = GetOutputDir_Main();

			return GOD_Dir;
		}

		private static string GetOutputDir_Main()
		{
			for (int c = 1; c <= 999; c++)
			{
				string dir = "C:\\" + c;

				if (
					!Directory.Exists(dir) &&
					!File.Exists(dir)
					)
				{
					SCommon.CreateDir(dir);
					//SCommon.Batch(new string[] { "START " + dir });
					return dir;
				}
			}
			throw new Exception("C:\\1 ～ 999 は使用できません。");
		}

		public static void OpenOutputDir()
		{
			SCommon.Batch(new string[] { "START " + GetOutputDir() });
		}

		public static void OpenOutputDirIfCreated()
		{
			if (GOD_Dir != null)
			{
				OpenOutputDir();
			}
		}

		private static int NOP_Count = 0;

		public static string NextOutputPath()
		{
			return Path.Combine(GetOutputDir(), (++NOP_Count).ToString("D4"));
		}

		#endregion

		public static double GetDistance(D2Point pt)
		{
			return Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
		}

		public static bool Simple_IsPrime(ulong n)
		{
			if (n < 2)
				return false;

			if (n < 4)
				return true;

			if (n % 2 == 0)
				return false;

			uint e = IntRoot(n);

			for (uint d = 3; d <= e; d += 2)
				if (n % d == 0)
					return false;

			return true;
		}

		/// <summary>
		/// 指定値の平方根の整数部を返す。
		/// 言い換えると指定値の平方根以下の最大の整数を返す。
		/// </summary>
		/// <param name="n">指定値</param>
		/// <returns>指定値の平方根の整数部</returns>
		private static uint IntRoot(ulong n)
		{
			if ((ulong)uint.MaxValue * uint.MaxValue <= n)
				return uint.MaxValue;

			uint l = 0;
			uint r = uint.MaxValue;

			while (l + 1 < r)
			{
				uint m = (uint)(((ulong)l + r) / 2);

				if ((ulong)m * m <= n)
					l = m;
				else
					r = m;
			}
			return l;
		}
	}
}
