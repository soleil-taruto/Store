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

			Main4(new ArgsReader(new string[] { @"C:\temp\StoreP-main" }));
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
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				//MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			string rDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(rDir))
				throw new Exception("no rDir");

			string wDir = Common.GetOutputDir();
			ProcMain.WriteLog("出力先へコピーしています...");
			SCommon.CopyDir(rDir, wDir);
			ProcMain.WriteLog("出力先へコピーしました。");
			FilterMain(wDir);
			ProcMain.WriteLog("完了しました。");
		}

		private void FilterMain(string rootDir)
		{
			string[] dirs = Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories);
			string[] files = Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories);

			Array.Sort(dirs, (a, b) => SCommon.Comp(a, b) * -1); // 逆順 -- 配下のディレクトリから処理するため
			Array.Sort(files, SCommon.Comp);

			// 1. ファイルの編集
			// 2. ファイル名の変更
			// 3. ディレクトリ名の変更(配下のディレクトリから)
			// ...の順で実行すること。

			foreach (string file in files) // ファイルの編集
			{
				byte[] fileData = File.ReadAllBytes(file);

				if (IsEncodingUTF8WithBOM(fileData) || Common.ExtIs(file, ".js"))
				{
					fileData = NewLineToCRLF(fileData).ToArray();

					ProcMain.WriteLog("* " + file);

					File.WriteAllBytes(file, fileData);
				}
			}
			foreach (string file in files) // ファイル名の変更
			{
				string localName = Path.GetFileName(file);
				string localNameNew = RestoreLocalName(localName);

				if (localName != localNameNew)
				{
					string fileNew = Path.Combine(Path.GetDirectoryName(file), localNameNew);

					ProcMain.WriteLog("< " + file);
					ProcMain.WriteLog("> " + fileNew);

					File.Move(file, fileNew);
				}
			}
			foreach (string dir in dirs) // ディレクトリ名の変更(配下のディレクトリから)
			{
				string localName = Path.GetFileName(dir);
				string localNameNew = RestoreLocalName(localName);

				if (localName != localNameNew)
				{
					string dirNew = Path.Combine(Path.GetDirectoryName(dir), localNameNew);

					ProcMain.WriteLog("< " + dir);
					ProcMain.WriteLog("> " + dirNew);

					Directory.Move(dir, dirNew);
				}
			}
		}

		private bool IsEncodingUTF8WithBOM(byte[] fileData)
		{
			return
				3 <= fileData.Length &&
				fileData[0] == 0xef &&
				fileData[1] == 0xbb &&
				fileData[2] == 0xbf;
		}

		private IEnumerable<byte> NewLineToCRLF(byte[] fileData)
		{
			foreach (byte chr in fileData)
			{
				if (chr == Consts.CR)
				{
					// noop
				}
				else if (chr == Consts.LF)
				{
					yield return Consts.CR;
					yield return Consts.LF;
				}
				else
				{
					yield return chr;
				}
			}
		}

		private string RestoreLocalName(string str)
		{
			StringBuilder buff = new StringBuilder();

			for (int index = 0; index < str.Length; index++)
			{
				char chr = str[index];

				if (
					index + 4 < str.Length &&
					chr == '$' &&
					SCommon.hexadecimal.Contains(char.ToLower(str[index + 1])) &&
					SCommon.hexadecimal.Contains(char.ToLower(str[index + 2])) &&
					SCommon.hexadecimal.Contains(char.ToLower(str[index + 3])) &&
					SCommon.hexadecimal.Contains(char.ToLower(str[index + 4]))
					)
				{
					chr = (char)Convert.ToUInt16(str.Substring(index + 1, 4), 16);
					index += 4;
				}
				buff.Append(chr);
			}
			return SCommon.ToJString(buff.ToString(), true, false, false, true);
		}
	}
}
