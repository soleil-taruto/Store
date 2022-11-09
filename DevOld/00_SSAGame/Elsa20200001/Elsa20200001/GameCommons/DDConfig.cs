using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDConfig
	{
		// 設定項目 >

		/// <summary>
		/// -2 == アプリ側で制御, -1 == デフォルト, { 0, 1, 2, ... } == { 最初のモニタ, 2番目のモニタ, 3番目のモニタ, ... }
		/// </summary>
		public static int DisplayIndex = -2;

		public static string LogFile = @"C:\temp\Game.log";
		public static int LogCountMax = SCommon.IMAX;
		public static bool LOG_ENABLED = true;
		public static string ApplicationLogSaveDirectory = @"C:\temp";
		public static bool DrawScreen_MosaicFlag = true;

		// Config_新しい項目をここへ追加...

		// < 設定項目

		public static void Load()
		{
			if (!File.Exists(DDConsts.ConfigFile))
			{
				if (!File.Exists(SCommon.EraseExt(ProcMain.SelfFile) + ".pdb")) // ? 開発環境ではないっぽい -> リリース版なのに設定ファイルが無いのは可怪しいのでエラーにする。
					throw new DDError();

				return;
			}

			string[] lines = File.ReadAllLines(DDConsts.ConfigFile, SCommon.ENCODING_SJIS).Select(line => line.Trim()).Where(line => line != "" && line[0] != ';').ToArray();
			int c = 0;

			if (lines.Length != int.Parse(lines[c++]))
				throw new DDError();

			// 設定項目 >

			DisplayIndex = int.Parse(lines[c++]);
			LogFile = lines[c++];
			LogCountMax = int.Parse(lines[c++]);
			LOG_ENABLED = int.Parse(lines[c++]) != 0;
			ApplicationLogSaveDirectory = lines[c++];
			DrawScreen_MosaicFlag = int.Parse(lines[c++]) != 0;

			// Config_新しい項目をここへ追加...

			// < 設定項目
		}
	}
}
