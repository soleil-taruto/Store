using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.WebServices;
using Charlotte.Commons;

namespace Charlotte.Actions
{
	public static class Action_Download
	{
		public static void Perform(HTTPServerChannel channel)
		{
			string host = GetHeaderValue(channel, "host");
			string downloadPath = GetDownloadPath(channel);
			string downloadUrl = "http://" + host + downloadPath;

			string resText = string.Format(@"

<center>
<h1>ファイルをダウンロードするには以下のリンクをクリックして下さい。</h1>
<hr/>
<a href=""{0}"">{1}</a>
</center>

".Trim()
				, downloadUrl
				, downloadUrl
				);

			channel.ResStatus = 200;
			channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/html; charset=UTF-8" });
			channel.ResBody = new byte[][] { Encoding.UTF8.GetBytes(resText) };
		}

		private static string GetHeaderValue(HTTPServerChannel channel, string name)
		{
			foreach (string[] pair in channel.HeaderPairs)
				if (SCommon.EqualsIgnoreCase(pair[0], name))
					return pair[1];

			throw new Exception("no header: " + name);
		}

		private static string GetDownloadPath(HTTPServerChannel channel)
		{
			string downloadPath = channel.PathQuery;
			int p = downloadPath.IndexOf('/', 1);

			if (p == -1)
				throw new Exception("Bad downloadPath");

			downloadPath = downloadPath.Substring(p);
			return downloadPath;
		}
	}
}
