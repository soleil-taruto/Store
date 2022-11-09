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
using Charlotte.Camellias;

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

			Main4(new ArgsReader(new string[] { "/E", @"C:\temp\1.txt", @"C:\temp\2.txt" }));
			//Main4(new ArgsReader(new string[] { "/D", @"C:\temp\2.txt", @"C:\temp\3.txt" }));
			//Main4(new ArgsReader(new string[] { "/M", "/E", @"C:\temp\1.txt", @"C:\temp\2.txt" }));
			//Main4(new ArgsReader(new string[] { "/M", "/D", @"C:\temp\2.txt", @"C:\temp\3.txt" }));
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
			bool onMemoryMode = ar.ArgIs("/M");
			bool encryptMode;

			if (ar.ArgIs("/E")) // ? 暗号化
			{
				encryptMode = true;
			}
			else if (ar.ArgIs("/D")) // ? 復号
			{
				encryptMode = false;
			}
			else
			{
				throw new Exception("不明なオプション");
			}

			string rFile = ar.NextArg();
			string wFile = ar.NextArg();

			ar.End();

			if (wFile == "*")
			{
				rFile = SCommon.MakeFullPath(rFile);
				wFile = rFile;

				Console.WriteLine("< " + rFile);
				Console.WriteLine("> " + wFile);

				if (!File.Exists(rFile))
					throw new Exception("入出力ファイルが見つかりません。");
			}
			else
			{
				rFile = SCommon.MakeFullPath(rFile);
				wFile = SCommon.MakeFullPath(wFile);

				Console.WriteLine("< " + rFile);
				Console.WriteLine("> " + wFile);

				if (!File.Exists(rFile))
					throw new Exception("入力ファイルが見つかりません。");

				if (SCommon.EqualsIgnoreCase(rFile, wFile))
					throw new Exception("入力ファイルと出力ファイルが同じです。");

				File.WriteAllBytes(wFile, SCommon.EMPTY_BYTES); // 書き出しテスト
				SCommon.DeletePath(wFile);

				File.Copy(rFile, wFile);
			}

			rFile = null; // もう使わない。

			try
			{
				if (onMemoryMode)
				{
					using (RingCipher cipher = new RingCipher(KeyBundle.RAW_KEY))
					{
						byte[] fileData = File.ReadAllBytes(wFile);

						if (encryptMode)
							fileData = cipher.Encrypt(fileData);
						else
							fileData = cipher.Decrypt(fileData);

						File.WriteAllBytes(wFile, fileData);
					}
				}
				else
				{
					using (FileCipher cipher = new FileCipher(KeyBundle.RAW_KEY))
					{
						if (encryptMode)
							cipher.Encrypt(wFile);
						else
							cipher.Decrypt(wFile);
					}
				}
			}
			catch
			{
				SCommon.DeletePath(wFile);
				throw;
			}

			Console.WriteLine("Done");
		}
	}
}
