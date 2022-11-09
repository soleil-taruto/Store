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
		/// 指定ファイルの拡張子が指定拡張子のどれかに一致するかどうか判定する。
		/// </summary>
		/// <param name="file">指定ファイル</param>
		/// <param name="exts">指定拡張子</param>
		/// <returns>指定ファイルの拡張子が指定拡張子のどれかに一致するか</returns>
		public static bool ExtIn(string file, params string[] exts)
		{
			return exts.Any(ext => SCommon.EqualsIgnoreCase(ext, Path.GetExtension(file)));
		}
	}
}
