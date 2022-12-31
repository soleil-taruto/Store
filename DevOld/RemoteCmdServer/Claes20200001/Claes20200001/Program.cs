using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using Charlotte.WebServices;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4(ar);
			}
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { "80", @"..\..\..\..\dat\Batch", @"C:\temp" }));
			//Main4(new ArgsReader(new string[] { "/P", "80", @"..\..\..\..\dat\Batch", @"C:\temp" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			SCommon.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				//MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			using (EventWaitHandle evStop = new EventWaitHandle(false, EventResetMode.AutoReset, Consts.SERVER_STOP_EVENT_NAME))
			{
				HTTPServer hs = new HTTPServer()
				{
					//PortNo = 80,
					//Backlog = 300,
					//ConnectMax = 100,
					Interlude = () => !evStop.WaitOne(0) && !Console.KeyAvailable, // 停止イベント / キー押下
					HTTPConnected = P_Connected,
				};

				//SockChannel.ThreadTimeoutMillis = 100;

				//HTTPServer.KeepAliveTimeoutMillis = 5000;

				//HTTPServerChannel.RequestTimeoutMillis = -1;
				//HTTPServerChannel.ResponseTimeoutMillis = -1;
				//HTTPServerChannel.FirstLineTimeoutMillis = 2000;
				//HTTPServerChannel.IdleTimeoutMillis = 180000;
				HTTPServerChannel.BodySizeMax = 100000000000000; // 100 TB

				//SockCommon.TimeWaitMonitor.CTR_ROT_SEC = 60;
				//SockCommon.TimeWaitMonitor.COUNTER_NUM = 5;
				//SockCommon.TimeWaitMonitor.COUNT_LIMIT = 10000;

				// サーバーの設定ここまで

				ProcMain.WriteLog("RemoteCmdServer-ST");

				if (ar.ArgIs("/S"))
				{
					evStop.Set();
				}
				else
				{
					this.PostRequestOnlyMode = ar.ArgIs("/P");

					hs.PortNo = int.Parse(ar.NextArg());
					this.BatchDir = SCommon.MakeFullPath(ar.NextArg());
					this.StoreDir = SCommon.MakeFullPath(ar.NextArg());

					ar.End();

					ProcMain.WriteLog("PortNo: " + hs.PortNo);
					ProcMain.WriteLog("BatchDir: " + this.BatchDir);
					ProcMain.WriteLog("StoreDir: " + this.StoreDir);

					if (hs.PortNo < 1 || 65535 < hs.PortNo)
						throw new Exception("Bad PortNo");

					if (string.IsNullOrEmpty(this.BatchDir))
						throw new Exception("Bad BatchDir");

					if (!Directory.Exists(this.BatchDir))
						throw new Exception("no BatchDir");

					if (string.IsNullOrEmpty(this.StoreDir))
						throw new Exception("Bad StoreDir");

					if (!Directory.Exists(this.StoreDir))
						throw new Exception("no StoreDir");

					hs.Perform();
				}

				ProcMain.WriteLog("RemoteCmdServer-ED");
			}
		}

		private bool PostRequestOnlyMode;
		private string BatchDir;
		private string StoreDir;

		private void P_Connected(HTTPServerChannel channel)
		{
			try
			{
				P_Connected2(channel);
			}
			catch (Exception ex)
			{
				channel.ResStatus = 404;
				channel.ResBody = new byte[][] { Encoding.UTF8.GetBytes(ex.ToString()) };
				channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=UTF-8" });
			}
		}

		private void P_Connected2(HTTPServerChannel channel)
		{
			ProcMain.WriteLog("Client: " + channel.Channel.Handler.RemoteEndPoint);

			if (this.PostRequestOnlyMode && channel.Method != "POST")
				throw new Exception("POST-リクエスト以外は受け付けません。");

			string urlPath = channel.PathQuery;

			ProcMain.WriteLog("URL-Path：" + urlPath);

			string[] commandAndParams = urlPath.Substring(1).Split(new char[] { '/' }, 2);
			string command = commandAndParams[0];

			if (command == "B") // バッチ実行
			{
				string relPath = SCommon.ToFairRelPath(commandAndParams[1], this.BatchDir.Length);
				string path = Path.Combine(this.BatchDir, relPath);

				if (!File.Exists(path))
					throw new Exception("no file");

				string[] batch = new string[] { "TIMEOUT 2 > NUL" }
					.Concat(File.ReadAllLines(path, SCommon.ENCODING_SJIS))
					.ToArray();

				SCommon.Batch(batch, Path.GetDirectoryName(path), SCommon.StartProcessWindowStyle_e.MINIMIZED);

				channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes("B-OK") };
				channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
			}
			else
			{
				string relPath = SCommon.ToFairRelPath(commandAndParams[1], this.StoreDir.Length);
				string path = Path.Combine(this.StoreDir, relPath);

				if (command == "D") // ダウンロード
				{
					if (!File.Exists(path))
						throw new Exception("no file");

					channel.ResBody = E_ReadFile(path);
					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "application/octet-stream" });
				}
				else if (command == "U") // アップロード
				{
					SCommon.CreateDir(Path.GetDirectoryName(path));
					channel.Body.ToFile(path);

					channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes("U-OK") };
					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
				}
				else if (command == "K") // 削除
				{
					SCommon.DeletePath(path);

					channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes("K-OK") };
					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
				}
				else
				{
					throw new Exception("Bad command: " + command);
				}
			}

			channel.ResHeaderPairs.Add(new string[] { "Server", "RemoteCmdServer" });

			// ----

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-STATUS " + channel.ResStatus);

			foreach (string[] pair in channel.ResHeaderPairs)
				SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-HEADER " + pair[0] + " = " + pair[1]);

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-BODY " + (channel.ResBody != null));
		}

		private static IEnumerable<byte[]> E_ReadFile(string file)
		{
			long fileSize = new FileInfo(file).Length;

			for (long offset = 0L; offset < fileSize; )
			{
				int readSize = (int)Math.Min(2000000, fileSize - offset);
				byte[] buff = new byte[readSize];

				using (FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					reader.Seek(offset, SeekOrigin.Begin);
					reader.Read(buff, 0, readSize);
				}
				yield return buff;

				offset += (long)readSize;
			}
		}
	}
}
