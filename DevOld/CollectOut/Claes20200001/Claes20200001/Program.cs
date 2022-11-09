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

			Main4(new ArgsReader(new string[] { @"C:\Dev" }));
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

		private string OutputDir;

		private void Main5(ArgsReader ar)
		{
			string rDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(rDir))
				throw new Exception("no rDir");

			this.OutputDir = SCommon.GetOutputDir();

			Console.WriteLine("< " + rDir);
			Console.WriteLine("> " + this.OutputDir);

			SearchOut(rDir);

			Console.WriteLine("done");
		}

		private void SearchOut(string rDir)
		{
			string outDir = Path.Combine(rDir, "out");

			if (Directory.Exists(outDir))
			{
				CollectOut(rDir, outDir);
			}
			else // out ディレクトリが見つからない -> 更に配下を探しに行く。
			{
				foreach (string dir in Directory.GetDirectories(rDir))
				{
					SearchOut(dir);
				}
			}
		}

		private void CollectOut(string parentDir, string outDir)
		{
			string archiveFile = GetOnlyOneArchiveFile(outDir);

			if (archiveFile == null)
			{
				SCommon.CopyDir(
					outDir,
					Path.Combine(this.OutputDir, Path.GetFileName(parentDir))
					);
			}
			else
			{
				File.Copy(
					archiveFile,
					Path.Combine(this.OutputDir, Path.GetFileName(archiveFile))
					);
			}
		}

		private string GetOnlyOneArchiveFile(string targDir)
		{
			string[] dirs = Directory.GetDirectories(targDir);
			string[] files = Directory.GetFiles(targDir);

			if (
				dirs.Length == 0 &&
				files.Length == 1 &&
				files[0].ToLower().EndsWith(".zip")
				)
				return files[0];

			return null;
		}
	}
}
