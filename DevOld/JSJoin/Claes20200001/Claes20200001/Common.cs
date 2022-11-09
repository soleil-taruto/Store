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

		/// <summary>
		/// パス文字列を比較する。
		/// 同じフォルダ内のローカル名が辞書順になるようにする。
		/// </summary>
		/// <param name="a">パス文字列_A</param>
		/// <param name="b">パス文字列_B</param>
		/// <returns>比較結果</returns>
		public static int CompPath(string a, string b)
		{
			a = a.Replace('\\', '\t');
			b = b.Replace('\\', '\t');

			return SCommon.CompIgnoreCase(a, b);
		}

		/// <summary>
		/// ランダムな識別子を生成する。
		/// 重複を考慮しなくて良いランダムな文字列を返す。
		/// </summary>
		/// <returns>新しい識別子</returns>
		public static string CreateRandIdent()
		{
			return
				"A_" +
				SCommon.CRandom.GetUInt64().ToString("D20") + "_" +
				SCommon.CRandom.GetUInt64().ToString("D20") +
				"_Z";
		}

		/// <summary>
		/// リソースファイル(テキストファイル)を読み込む
		/// </summary>
		/// <param name="localName">リソースファイルのローカル名</param>
		/// <returns>リソースファイルの内容</returns>
		public static string ReadResTextFile(string localName)
		{
			string resFile = Path.Combine(ProcMain.SelfDir, localName);

			if (!File.Exists(resFile))
			{
				resFile = Path.Combine(@"..\..\..\..\doc", localName);

				if (!File.Exists(resFile))
					throw new Exception("リソースファイル \"" + localName + "\" が存在しません。");
			}
			return File.ReadAllText(resFile, SCommon.ENCODING_SJIS);
		}
	}
}
