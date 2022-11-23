using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Charlotte.Commons;
using Charlotte.WebServices;

namespace Charlotte.Actions
{
	public static class Action_BatchService
	{
		/// <summary>
		/// 実行するバッチファイル
		/// 読み込んだら勝手に削除することに注意！
		/// </summary>
		private const string BATCH_FILE = @"C:\temp\HTTCmd-P_BatchService_Batch.bat";

		/// <summary>
		/// バッチファイル実行時の作業ディレクトリ
		/// </summary>
		private const string BATCH_WORK_DIR = @"C:\temp";

		public static void Perform(HTTPServerChannel channel)
		{
			try
			{
				ProcMain.WriteLog("BatchService-ST");

				if (!File.Exists(BATCH_FILE))
					throw new Exception("no BATCH_FILE");

				if (!Directory.Exists(BATCH_WORK_DIR))
					throw new Exception("no BATCH_WORK_DIR");

				string[] commands = File.ReadAllLines(BATCH_FILE, SCommon.ENCODING_SJIS);
				SCommon.DeletePath(BATCH_FILE);
				SCommon.Batch(commands, BATCH_WORK_DIR, SCommon.StartProcessWindowStyle_e.MINIMIZED);

				ProcMain.WriteLog("BatchService-ED");
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog("BatchService-Error: " + ex);
			}

			channel.ResStatus = 200;
			channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
			channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes("BatchService-OK") };
		}
	}
}
