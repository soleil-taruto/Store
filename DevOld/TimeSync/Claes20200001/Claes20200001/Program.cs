using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

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
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			//Main4(new ArgsReader(new string[] { }));
			//new Test0001().Test01();
			new Test0001().Test02();
			//new Test0001().Test03();

			// --

			Common.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);

				Console.WriteLine("エラー終了(エンターキーを押して下さい)");
				Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			using (EventWaitHandle evStop = new EventWaitHandle(false, EventResetMode.ManualReset, Consts.PROC_STOP_EVENT_NAME))
			{
				if (ar.ArgIs("/S"))
				{
					evStop.Set();
				}
				else
				{
					Console.WriteLine("システム時計調整プロセス-ST");

					int waitSec;

					do
					{
						try
						{
							TimeAdjustment.AdjustToServerTime();
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex);
							Console.WriteLine("例外が発生しましたが、プロセスは続行します。");
						}

						waitSec = 60 - (SCommon.SimpleDateTime.Now().Second + 20) % 60; // 次の xx:xx:40 まで待つ

						Console.WriteLine("waitSec: " + waitSec);
					}
					while (!evStop.WaitOne(waitSec * 1000));

					Console.WriteLine("システム時計調整プロセス-ED");
				}
			}
		}
	}
}
