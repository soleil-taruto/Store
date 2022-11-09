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
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { @"C:\Dev\GameJS\HPStore" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			SCommon.Pause();
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

				MessageBox.Show("" + ex, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private string SrcRootDir;
		private string DestRootDir;

		private void Main5(ArgsReader ar)
		{
			this.SrcRootDir = SCommon.MakeFullPath(ar.NextArg());
			this.DestRootDir = SCommon.GetOutputDir();

			if (!Directory.Exists(this.SrcRootDir))
				throw new Exception("no SrcRootDir");

			if (!Directory.Exists(this.DestRootDir))
				throw new Exception("no DestRootDir");

			string[] rDirs = Directory.GetDirectories(this.SrcRootDir);

			foreach (string rDir in rDirs)
			{
				string outDir = Path.Combine(rDir, "out");

				if (Directory.Exists(outDir))
				{
					string archiveFile = GetOnlyOneArchiveFile(outDir);

					if (archiveFile == null)
					{
						string wDir = Path.Combine(this.DestRootDir, Path.GetFileName(rDir));

						Console.WriteLine("D");
						Console.WriteLine("< " + outDir);
						Console.WriteLine("> " + wDir);

						SCommon.CopyDir(outDir, wDir);

						Console.WriteLine("done");
					}
					else
					{
						string wFile = Path.Combine(this.DestRootDir, Path.GetFileName(archiveFile));

						Console.WriteLine("F");
						Console.WriteLine("< " + archiveFile);
						Console.WriteLine("> " + wFile);

						if (File.Exists(wFile))
							throw new Exception("出力アーカイブ名が重複しています。");

						File.Copy(archiveFile, wFile);

						Console.WriteLine("done");
					}
				}
			}
			Console.WriteLine("OK!");
		}

		private string GetOnlyOneArchiveFile(string outDir)
		{
			string[] dirs = Directory.GetDirectories(outDir);
			string[] files = Directory.GetFiles(outDir);

			if (
				dirs.Length == 0 &&
				files.Length == 1 &&
				SCommon.EqualsIgnoreCase(Path.GetExtension(files[0]), ".zip")
				)
				return files[0];

			return null;
		}
	}
}
