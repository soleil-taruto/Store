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
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			//Main4(new ArgsReader(new string[] { "http://ccsp.mydns.jp" }));
			Main4(new ArgsReader(new string[] { "http://ccsp.mydns.jp", "/P", "*" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			Common.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main5(ArgsReader ar)
		{
			bool liteMode = ar.ArgIs("/L");

			HTTPClient hc = new HTTPClient(ar.NextArg());

			if (liteMode)
			{
				hc.ConnectTimeoutMillis = 10000; // 10 sec
				hc.TimeoutMillis = 15000; // 15 sec
				hc.IdleTimeoutMillis = 5000; // 5 sec
			}
			else
			{
				hc.ConnectTimeoutMillis = 3600000; // 1 hour
				hc.TimeoutMillis = 86400000; // 1 day
				hc.IdleTimeoutMillis = 180000; // 3 min
			}

			if (ar.ArgIs("/B"))
			{
				string user = ar.NextArg();
				string password = ar.NextArg();

				hc.SetAuthorization(user, password);
			}

			HTTPClient.BodyInfo body;

			if (ar.ArgIs("/P"))
			{
				if (ar.ArgIs("*"))
				{
					body = new HTTPClient.BodyInfo()
					{
						MemoryEntity = SCommon.EMPTY_BYTES,
					};
				}
				else
				{
					body = new HTTPClient.BodyInfo()
					{
						EntityFilePath = ar.NextArg(),
					};
				}
			}
			else
			{
				body = null;
			}

			if (ar.ArgIs("/R"))
			{
				hc.ResBodyFile = ar.NextArg();
			}
			else
			{
				hc.ResBodyFile = null;
			}

			ar.End();

			hc.Send(body);

			foreach (KeyValuePair<string, string> pair in hc.ResHeaders)
				Console.WriteLine(SCommon.ToJString(Encoding.ASCII.GetBytes(pair.Key + " = " + pair.Value), false, false, false, true));

			Console.WriteLine("");
			Console.WriteLine("HTTP-受信完了");
		}
	}
}
