using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.WebServices;
using Charlotte.Commons;
using System.Text.RegularExpressions;

namespace Charlotte
{
	/// <summary>
	/// 何とかして日本標準時を取得する。
	/// </summary>
	public static class TimeClient
	{
		/// <summary>
		/// エポックタイムのゼロ秒
		/// </summary>
		private static long SEC_EPOCH_TIME_ORIGIN = SCommon.TimeStampToSec.ToSec(19700101000000);

		/// <summary>
		/// 日本標準時を返す。
		/// </summary>
		/// <returns>日本標準時</returns>
		public static SCommon.SimpleDateTime GetTime()
		{
			long startedTimeMillis = Common.CurrentTimeMillis();

			HTTPClient hc = new HTTPClient("http://www.nict.go.jp/JST/JST5.html");

			// 定番軽量設定
			hc.ConnectTimeoutMillis = 10000; // 10 sec
			hc.TimeoutMillis = 15000; // 15 sec
			hc.IdleTimeoutMillis = 5000; // 5 sec
			//hc.ResBodySizeMax = 20000000; // 20 MB

			AddChromelyRequestHeaders(hc);

			hc.Get();

			byte[] resBody = hc.ResBody;
			string resText = Encoding.UTF8.GetString(resBody);
			resText = SCommon.ToJString(SCommon.ENCODING_SJIS.GetBytes(resText), true, true, true, true); // 念のためフィルタしておく。
			string[] encl = SCommon.ParseEnclosed(resText, "var ServerList = [", "]");
			string textServerList = encl[2];
			List<string> serverList = new List<string>();

			for (; ; )
			{
				encl = SCommon.ParseEnclosed(textServerList, "\"", "\"");

				if (encl == null)
					break;

				serverList.Add(encl[2]);
				textServerList = encl[4];
			}

			if (serverList.Count < 1)
				throw new Exception("サーバーリストが空です。");

			string server = SCommon.CRandom.ChooseOne(serverList);

			if (!Regex.IsMatch(server, "//[0-9a-f]{40}.nict.go.jp/cgi-bin/json")) // ? 想定文字列ではない。
				throw new Exception("Bad server: " + server);

			hc = new HTTPClient("http:" + server);

			// 定番軽量設定
			hc.ConnectTimeoutMillis = 10000; // 10 sec
			hc.TimeoutMillis = 15000; // 15 sec
			hc.IdleTimeoutMillis = 5000; // 5 sec
			//hc.ResBodySizeMax = 20000000; // 20 MB

			AddChromelyRequestHeaders(hc);

			hc.Get();

			resBody = hc.ResBody;
			resText = Encoding.UTF8.GetString(resBody);
			resText = SCommon.ToJString(SCommon.ENCODING_SJIS.GetBytes(resText), true, true, true, true); // 念のためフィルタしておく。
			encl = SCommon.ParseEnclosed(resText, "\"st\":", ",");
			string st = encl[2];
			double t = double.Parse(st);

			long endedTimeMillis = Common.CurrentTimeMillis();
			long elapsedTimeMillis = endedTimeMillis - startedTimeMillis;
			Console.WriteLine("elapsedTimeMillis: " + elapsedTimeMillis);

			if (1000 <= elapsedTimeMillis)
				throw new Exception("通信に1秒以上掛かりました。");

			Console.WriteLine("t: " + t);
			t -= (elapsedTimeMillis / 2) / 1000.0; // 通信に掛かった時間の半分を引く
			Console.WriteLine("t: " + t);
			int it = (int)t; // ミリ秒_切り捨て
			it += 3600 * 9; // GMT -> JST
			SCommon.SimpleDateTime time = new SCommon.SimpleDateTime(SEC_EPOCH_TIME_ORIGIN + it);
			return time;
		}

		private static void AddChromelyRequestHeaders(HTTPClient hc)
		{
			hc.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
			hc.AddHeader("Accept-Encoding", "gzip, deflate");
			hc.AddHeader("Accept-Language", "ja,en-US;q=0.9,en;q=0.8");
			hc.AddHeader("Cache-Control", "max-age=0");
			//hc.AddHeader("Connection", "close"); // 例外を投げる。
			hc.AddHeader("Host", "www.nict.go.jp");
			hc.AddHeader("If-Modified-Since", "20110101000000");
			hc.AddHeader("Upgrade-Insecure-Requests", "1");
			hc.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36");
		}
	}
}
