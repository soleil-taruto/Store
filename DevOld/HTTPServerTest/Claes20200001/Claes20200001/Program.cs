using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using Charlotte.WebServices;
using System.Threading;

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
				Main4();
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			//Common.Pause();
			Thread.Sleep(500);
		}

		private void Main4()
		{
			// -- choose one --

			Main_応答開始まで30秒();
			//Main_応答中に30秒中断();
			//Main_応答に30秒();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}

		private void Main_応答開始まで30秒()
		{
			Console.WriteLine("★★★応答開始まで30秒★★★");

			HTTPServer hs = new HTTPServer()
			{
				//PortNo = 80,
				//Backlog = 300,
				//ConnectMax = 100,
				Interlude = () => !Console.KeyAvailable, // キー押下
				HTTPConnected = channel =>
				{
					Console.WriteLine("応答開始まで30秒スリープ-ST");
					for (int sec = 0; sec < 30; sec++)
					{
						if (Console.KeyAvailable)
						{
							Console.WriteLine("キー押下によりスリープ中断");
							break;
						}
						Thread.Sleep(1000);
					}
					Console.WriteLine("応答開始まで30秒スリープ-ED");

					//channel.ResStatus = 200;
					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
					channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes("OK") };
				},
			};

			//SockChannel.ThreadTimeoutMillis = 100;

			//HTTPServer.KeepAliveTimeoutMillis = 5000;

			//HTTPServerChannel.RequestTimeoutMillis = -1;
			//HTTPServerChannel.ResponseTimeoutMillis = -1;
			//HTTPServerChannel.FirstLineTimeoutMillis = 2000;
			//HTTPServerChannel.IdleTimeoutMillis = 180000; // 3 min
			//HTTPServerChannel.BodySizeMax = 300000000; // 300 MB

			//SockCommon.TimeWaitMonitor.CTR_ROT_SEC = 60;
			//SockCommon.TimeWaitMonitor.COUNTER_NUM = 5;
			//SockCommon.TimeWaitMonitor.COUNT_LIMIT = 10000;

			// サーバーの設定ここまで

			hs.Perform();
		}

		private void Main_応答中に30秒中断()
		{
			Console.WriteLine("★★★応答中に30秒中断★★★");

			HTTPServer hs = new HTTPServer()
			{
				//PortNo = 80,
				//Backlog = 300,
				//ConnectMax = 100,
				Interlude = () => !Console.KeyAvailable, // キー押下
				HTTPConnected = channel =>
				{
					//channel.ResStatus = 200;
					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
					channel.ResBody = E_ResBody_応答中に30秒中断(
						Encoding.ASCII.GetBytes("TEST-"),
						Encoding.ASCII.GetBytes("SERVER-"),
						Encoding.ASCII.GetBytes("OK")
						);
				},
			};

			//SockChannel.ThreadTimeoutMillis = 100;

			//HTTPServer.KeepAliveTimeoutMillis = 5000;

			//HTTPServerChannel.RequestTimeoutMillis = -1;
			//HTTPServerChannel.ResponseTimeoutMillis = -1;
			//HTTPServerChannel.FirstLineTimeoutMillis = 2000;
			//HTTPServerChannel.IdleTimeoutMillis = 180000; // 3 min
			//HTTPServerChannel.BodySizeMax = 300000000; // 300 MB

			//SockCommon.TimeWaitMonitor.CTR_ROT_SEC = 60;
			//SockCommon.TimeWaitMonitor.COUNTER_NUM = 5;
			//SockCommon.TimeWaitMonitor.COUNT_LIMIT = 10000;

			// サーバーの設定ここまで

			hs.Perform();
		}

		private IEnumerable<byte[]> E_ResBody_応答中に30秒中断(byte[] resBody1, byte[] resBody2, byte[] resBody3)
		{
			yield return resBody1;
			yield return resBody2;

			Console.WriteLine("応答中に30秒中断スリープ-ST");
			for (int sec = 0; sec < 30; sec++)
			{
				if (Console.KeyAvailable)
				{
					Console.WriteLine("キー押下によりスリープ中断");
					break;
				}
				Thread.Sleep(1000);
			}
			Console.WriteLine("応答中に30秒中断スリープ-ED");

			yield return resBody3;
		}

		private void Main_応答に30秒()
		{
			Console.WriteLine("★★★応答に30秒★★★");

			HTTPServer hs = new HTTPServer()
			{
				//PortNo = 80,
				//Backlog = 300,
				//ConnectMax = 100,
				Interlude = () => !Console.KeyAvailable, // キー押下
				HTTPConnected = channel =>
				{
					//channel.ResStatus = 200;
					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
					channel.ResBody = E_ResBody_応答に30秒(
						Encoding.ASCII.GetBytes("NERU-"),
						Encoding.ASCII.GetBytes("NERUNE")
						);
				},
			};

			//SockChannel.ThreadTimeoutMillis = 100;

			//HTTPServer.KeepAliveTimeoutMillis = 5000;

			//HTTPServerChannel.RequestTimeoutMillis = -1;
			//HTTPServerChannel.ResponseTimeoutMillis = -1;
			//HTTPServerChannel.FirstLineTimeoutMillis = 2000;
			//HTTPServerChannel.IdleTimeoutMillis = 180000; // 3 min
			//HTTPServerChannel.BodySizeMax = 300000000; // 300 MB

			//SockCommon.TimeWaitMonitor.CTR_ROT_SEC = 60;
			//SockCommon.TimeWaitMonitor.COUNTER_NUM = 5;
			//SockCommon.TimeWaitMonitor.COUNT_LIMIT = 10000;

			// サーバーの設定ここまで

			hs.Perform();
		}

		private IEnumerable<byte[]> E_ResBody_応答に30秒(byte[] resBody1, byte[] resBody2)
		{
			Console.WriteLine("応答に30秒-ST");

			for (int sec = 0; sec < 30; sec++)
			{
				yield return resBody1;

				if (Console.KeyAvailable)
				{
					Console.WriteLine("キー押下により応答中断");
					break;
				}
				Thread.Sleep(1000);
			}
			Console.WriteLine("応答に30秒-ED");

			yield return resBody2;
		}
	}
}
