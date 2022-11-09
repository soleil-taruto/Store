﻿using System;
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

		public static bool ExtIs(string file, string ext)
		{
			return SCommon.EqualsIgnoreCase(Path.GetExtension(file), ext);
		}

		private static bool[,] S_JChars = null;

		public static bool IsJChar(byte lead, byte trail)
		{
			if (S_JChars == null)
			{
				S_JChars = new bool[256, 256];

				foreach (UInt16 chr in SCommon.GetJCharCodes())
				{
					S_JChars[chr >> 8, chr & 0xff] = true;
				}
			}
			return S_JChars[lead, trail];
		}
	}
}
