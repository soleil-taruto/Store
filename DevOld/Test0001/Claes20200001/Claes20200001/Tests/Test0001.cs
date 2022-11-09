using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			string text = File.ReadAllText(@"C:\temp\Input.txt", SCommon.ENCODING_SJIS);
			string dest = "";

			for (; ; )
			{
				string[] encl = SCommon.ParseEnclosed(text, "[http://stackprobe.ccsp.mydns.jp", "]");

				if (encl == null)
					break;

				Action skip = () =>
				{
					dest += encl[0] + encl[1] + encl[2] + encl[3];
					text = encl[4];
				};

				int start = encl[2].IndexOf(":image=http://stackprobe.ccsp.mydns.jp");

				if (start == -1) // ? イメージじゃないっぽい。
				{
					skip();
					continue;
				}

				string url = encl[2].Substring(start + 7);
				url = url.Replace(":58946", "");
				string mediaType = URLToMediaType(url);
				byte[] data;

				using (WorkingDir wd = new WorkingDir())
				{
					HTTPClient hc = new HTTPClient(url);
					hc.ResFile = wd.MakePath();
					hc.Get();
					data = File.ReadAllBytes(hc.ResFile);
				}

				string dataUrl = "data:" + mediaType + ";base64," + SCommon.Base64.I.Encode(data);

				dest += encl[0] + "><img src=\"" + dataUrl + "\"/><";
				text = encl[4];
			}
			dest += text;

			File.WriteAllText(SCommon.NextOutputPath() + ".txt", dest, SCommon.ENCODING_SJIS);
		}

		private string URLToMediaType(string url)
		{
			if (url.EndsWith(".png"))
			{
				return "image/png";
			}

			throw new Exception("不明なメディアタイプ");
		}
	}
}
