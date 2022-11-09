using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Commons;

namespace Charlotte
{
	public static class TimeAdjustment
	{
		/// <summary>
		/// 許容誤差
		/// 秒単位
		/// </summary>
		private const int SEC_ERROR_MARGIN = 1;

		public static void AdjustToServerTime()
		{
			Console.WriteLine("時刻調整-ST");

			SCommon.SimpleDateTime clientTime = WaitAndGetNextTime();
			SCommon.SimpleDateTime serverTime = TimeClient.GetTime();

			Console.WriteLine("clientTime: " + clientTime);
			Console.WriteLine("serverTime: " + serverTime);
			Console.WriteLine("difference: " + (clientTime - serverTime));

			if (clientTime + SEC_ERROR_MARGIN < serverTime) // ? クライアント側が遅れている。-> クライアント側を進める。
			{
				Console.WriteLine("時計を進めます。");
				SyncTime(1);
				Console.WriteLine("時計を進めました。");
			}
			else if (serverTime + SEC_ERROR_MARGIN < clientTime) // ? クライアント側が進んでいる。-> クライアント側を遅らせる。
			{
				Console.WriteLine("時計を遅らせます。");
				SyncTime(0);
				Console.WriteLine("時計を遅らせました。");
			}
			Console.WriteLine("時刻調整-ED");
		}

		private static void SyncTime(int secAdd)
		{
			SCommon.SimpleDateTime currTime = WaitAndGetNextTime();

			if (
				currTime.Hour == 23 && currTime.Minute > 55 ||
				currTime.Hour == 0 && currTime.Minute < 5
				)
			{
				Console.WriteLine("日付変更の際なので、調整を中止します。");
				return;
			}

			// currTime + 0.5 sec になってからシステム時計更新を行う。
			// secAdd == 0 の場合、システム時計は currTime にセットされるので 0.5 sec 遅らせることになる。
			// secAdd == 1 の場合、システム時計は currTime + 1 sec にセットされるので 0.5 sec 進めることになる。
			Thread.Sleep(500);

			currTime += secAdd;

			// 日付変更の際で実行した場合、システム日付を変更した直後に日付変更が発生し、その後でシステム時刻を変更すると丸1日ずれるんじゃないか？
			// --> 念のため時刻から変更する。日付変更の際で実行しないようにする。
			// --> 日付変更の際で実行しないのなら、日付の設定は不要である。
			Microsoft.VisualBasic.DateAndTime.TimeString = currTime.ToString("{4:D2}:{5:D2}:{6:D2}");
			//Microsoft.VisualBasic.DateAndTime.DateString = currTime.ToString("{0}/{1:D2}/{2:D2}"); // 日付の設定は不要
		}

		private static SCommon.SimpleDateTime WaitAndGetNextTime()
		{
			SCommon.SimpleDateTime lastTime = SCommon.SimpleDateTime.Now();
			SCommon.SimpleDateTime currTime;

			for (; ; )
			{
				Thread.Sleep(100);
				currTime = SCommon.SimpleDateTime.Now();

				if (currTime != lastTime)
					break;
			}
			return currTime;
		}
	}
}
