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
			Main4(new ArgsReader(new string[] { "/R" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//Common.Pause();
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

		private const string INPUT_DIR = @"C:\temp";

		private const string INPUT_LOCAL_FILE_PREFIX = "Biscuit_";
		private const string INPUT_LOCAL_FILE_SUFFIX = ".txt";

		private void Main5(ArgsReader ar)
		{
			byte[] data = DecodeInputTxt();

			if (ar.ArgIs("/R"))
			{
				data = SCommon.Decompress(data);
				DecodeFolder(data, Common.NextOutputPath() + "\\Ricecake");
			}
			else
			{
				File.WriteAllBytes(Common.NextOutputPath() + ".txt", data);
			}
		}

		private byte[] DecodeInputTxt()
		{
			string[] files = Directory.GetFiles(INPUT_DIR)
				.Where(v =>
					SCommon.StartsWithIgnoreCase(Path.GetFileName(v), INPUT_LOCAL_FILE_PREFIX) &&
					SCommon.EndsWithIgnoreCase(v, INPUT_LOCAL_FILE_SUFFIX)
					)
				.OrderBy(SCommon.CompIgnoreCase)
				.ToArray();

			List<byte[]> buff = new List<byte[]>();

			foreach (string file in files)
			{
				string[][] headers = File.ReadAllLines(file, Encoding.ASCII)
					.Select(v => v.Split(new char[] { ':' }, 2).Select(w => w.Trim()).Where(w => w != "").ToArray())
					.Where(v => v.Length == 2)
					.ToArray();

				foreach (string[] header in headers)
				{
					if (header[0].ToLower() == "cookie")
					{
						string[][] pairs = header[1].Split(';')
							.Select(v => v.Split(new char[] { '=' }, 2).Select(w => w.Trim()).Where(w => w != "").ToArray())
							.Where(v => v.Length == 2)
							.ToArray();

						foreach (string[] pair in pairs)
						{
							if (pair[0].ToLower() == "biscuit")
							{
								string sData = pair[1];
								sData = Common.ZEnc(sData);
								byte[] data = SCommon.Base64.I.Decode(sData);
								buff.Add(data);
							}
						}
					}
				}
			}
			return SCommon.Join(buff);
		}

		private const string INPUT_PATH_BASE = @"C:\work\Benzo\workspace\";

		private void DecodeFolder(byte[] data, string destDir)
		{
			SCommon.CreateDir(destDir);

			byte[][] dataParts = SCommon.Split(data);

			for (int index = 0; index < dataParts.Length; index += 2)
			{
				string filePath = SCommon.ToJString(dataParts[index], false, false, false, true);
				byte[] fileData = dataParts[index + 1];

				filePath = SCommon.ChangeRoot(filePath, INPUT_PATH_BASE);
				filePath = Common.ToFairRelPath(filePath, destDir.Length);
				filePath = Path.Combine(destDir, filePath);

				ProcMain.WriteLog(filePath); // cout

				SCommon.CreateDir(Path.GetDirectoryName(filePath));
				File.WriteAllBytes(filePath, fileData);
			}
		}
	}
}
