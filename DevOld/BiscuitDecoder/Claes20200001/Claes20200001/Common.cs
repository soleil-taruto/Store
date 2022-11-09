using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Commons;
using System.Text.RegularExpressions;

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

		#region ToFairLocalPath, ToFairRelPath

		/// <summary>
		/// ローカル名に使用できない予約名のリストを返す。
		/// https://github.com/stackprobe/Factory/blob/master/Common/DataConv.c#L460-L491
		/// </summary>
		/// <returns>予約名リスト</returns>
		private static IEnumerable<string> GetReservedWordsForWindowsPath()
		{
			yield return "AUX";
			yield return "CON";
			yield return "NUL";
			yield return "PRN";

			for (int no = 1; no <= 9; no++)
			{
				yield return "COM" + no;
				yield return "LPT" + no;
			}

			// グレーゾーン
			{
				yield return "COM0";
				yield return "LPT0";
				yield return "CLOCK$";
				yield return "CONFIG$";
			}
		}

		public const int MY_PATH_MAX = 240;

		/// <summary>
		/// 歴としたローカル名に変換する。
		/// https://github.com/stackprobe/Factory/blob/master/Common/DataConv.c#L503-L552
		/// </summary>
		/// <param name="str">対象文字列(対象パス)</param>
		/// <param name="dirSize">対象パスが存在するディレクトリのフルパスの長さ、考慮しない場合は 0 を指定すること。</param>
		/// <returns>ローカル名</returns>
		public static string ToFairLocalPath(string str, int dirSize)
		{
			const string CHRS_NG = "\"*/:<>?\\|";
			const string CHR_ALT = "_";

			int maxLen = Math.Max(0, MY_PATH_MAX - dirSize);

			if (maxLen < str.Length)
				str = str.Substring(0, maxLen);

			str = SCommon.ToJString(SCommon.ENCODING_SJIS.GetBytes(str), true, false, false, true);

			string[] words = str.Split('.');

			for (int index = 0; index < words.Length; index++)
			{
				string word = words[index];

				word = word.Trim();

				if (
					index == 0 &&
					GetReservedWordsForWindowsPath().Any(resWord => SCommon.EqualsIgnoreCase(resWord, word)) ||
					word.Any(chr => CHRS_NG.IndexOf(chr) != -1)
					)
					word = CHR_ALT;

				words[index] = word;
			}
			str = string.Join(".", words);

			if (str == "")
				str = CHR_ALT;

			if (str.EndsWith("."))
				str = str.Substring(0, str.Length - 1) + CHR_ALT;

			return str;
		}

		public static string ToFairRelPath(string path, int dirSize)
		{
			string[] ptkns = SCommon.Tokenize(path, "\\/", false, true);

			if (ptkns.Length == 0)
				ptkns = new string[] { "_" };

			for (int index = 0; index < ptkns.Length; index++)
				ptkns[index] = ToFairLocalPath(ptkns[index], 0);

			path = string.Join("\\", ptkns);

			int maxLen = Math.Max(0, MY_PATH_MAX - dirSize);

			if (maxLen < path.Length)
				path = ToFairLocalPath(path, dirSize);

			return path;
		}

		#endregion

		#region ZEnc

		public static string ZEnc(string str)
		{
			foreach (int es in new int[] { 2, 7, 11, 5, 13, 3, 17, 2, 19, 1, 19, 2, 17, 3, 13, 5, 11, 7, 2 })
			{
				str = ZEncP(str, 1, es);
			}
			return str;
		}

		private static string ZEncP(string str, int ss, int es)
		{
			char[] cs = str.ToCharArray();

			int s = 0;
			int e = cs.Length - es;

			while (s < e)
			{
				SCommon.Swap(ref cs[s], ref cs[e]);

				s += ss;
				e -= es;
			}
			return new string(cs);
		}

		#endregion

		public static double GetDistance(D2Point pt)
		{
			return Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
		}
	}
}
