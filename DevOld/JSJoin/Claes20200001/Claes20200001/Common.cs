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

		public static void HelloWorld()
		{
			Console.WriteLine("Hello, world!");
		}

		/// <summary>
		/// 指定パスの拡張子が指定拡張子であるか判定する。
		/// </summary>
		/// <param name="path">パス</param>
		/// <param name="ext">拡張子</param>
		/// <returns>指定パスの拡張子が指定拡張子であるか</returns>
		public static bool ExtIs(string path, string ext)
		{
			return SCommon.EqualsIgnoreCase(Path.GetExtension(path), ext);
		}

		/// <summary>
		/// リストの指定範囲を列挙する。
		/// -- 開始位置と終了位置を指定することに注意すること。
		/// </summary>
		/// <typeparam name="T">リストの要素の型</typeparam>
		/// <param name="list">リスト</param>
		/// <param name="start">開始位置(この位置の要素を含む)</param>
		/// <param name="end">終了位置(この位置の要素を含まない)</param>
		/// <returns>部分リスト</returns>
		public static IEnumerable<T> GetBetween<T>(IList<T> list, int start, int end)
		{
			for (int index = start; index < end; index++)
			{
				yield return list[index];
			}
		}
	}
}
