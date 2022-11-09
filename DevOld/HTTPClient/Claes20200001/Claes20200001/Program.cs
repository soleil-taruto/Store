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

			Main4(new ArgsReader(new string[] { "http://ccsp.mydns.jp" }));
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
				//hc.ResBodySizeMax = 20000000; // 20 MB
			}
			else
			{
				hc.ConnectTimeoutMillis = 3600000; // 1 hour
				hc.TimeoutMillis = 86400000; // 1 day
				hc.IdleTimeoutMillis = 180000; // 3 min
				hc.ResBodySizeMax = 300000000; // 300 MB
			}

			if (ar.ArgIs("/B"))
			{
				string user = ar.NextArg();
				string password = ar.NextArg();

				hc.SetAuthorization(user, password);
			}

			byte[] body;

			if (ar.ArgIs("/P"))
			{
				if (ar.ArgIs("*"))
				{
					body = SCommon.EMPTY_BYTES;
				}
				else
				{
					body = File.ReadAllBytes(ar.NextArg());
				}
			}
			else
			{
				body = null;
			}

			string resBodyFile;

			if (ar.ArgIs("/R"))
			{
				resBodyFile = ar.NextArg();
			}
			else
			{
				resBodyFile = null;
			}

			ar.End();

			hc.Send(body);

			foreach (KeyValuePair<string, string> pair in hc.ResHeaders)
				Console.WriteLine(SCommon.ToJString(Encoding.ASCII.GetBytes(pair.Key + " = " + pair.Value), false, false, false, true));

			if (resBodyFile != null)
			{
				File.WriteAllBytes(resBodyFile, hc.ResBody);
			}
			else
			{
				Console.WriteLine("");
				Console.WriteLine(SCommon.ToJString(hc.ResBody, true, true, true, true));
			}
		}
	}
}
