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

		private bool OpenOutputDirIfNeeded = true;

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

			if (OpenOutputDirIfNeeded)
			{
				SCommon.OpenOutputDirIfCreated();
			}
		}

		private void Main3()
		{
			// -- choose one --

			//Main4(new ArgsReader(new string[] { @"C:\Dev" }));
			//Main4(new ArgsReader(new string[] { @"C:\Dev", "/D", @"C:\temp" }));
			Main4(new ArgsReader(new string[] { @"C:\temp\1", "/B", @"C:\temp\2", "/D", @"C:\temp\3" }));
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
		private string[] DistributeDirs;
		private string BackupDir;

		private void Main5(ArgsReader ar)
		{
			string rDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(rDir))
				throw new Exception("no rDir");

			if (ar.ArgIs("/B"))
			{
				this.BackupDir = SCommon.MakeFullPath(ar.NextArg());

				if (!Directory.Exists(this.BackupDir))
					throw new Exception("no BackupDir");
			}
			if (ar.ArgIs("/D"))
			{
				this.DistributeDirs = ar.TrailArgs().Select(v => SCommon.MakeFullPath(v)).ToArray();

				if (this.DistributeDirs.Length < 1)
					throw new Exception("no DistributeDirs");

				foreach (string dir in this.DistributeDirs)
					if (!Directory.Exists(dir))
						throw new Exception("no DistributeDir: " + dir);
			}
			ar.End();

			this.OutputDir = SCommon.GetOutputDir();

			Console.WriteLine("< " + rDir);
			Console.WriteLine("> " + this.OutputDir);
			Console.WriteLine("B " + (this.BackupDir == null ? "<NONE>" : this.BackupDir));

			SearchOut(rDir);

			if (this.DistributeDirs != null)
				this.Distribute();

			Console.WriteLine("done!");
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
			if (IsEmptyDir(outDir))
			{
				return;
			}

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

		private bool IsEmptyDir(string targDir)
		{
			string[] dirs = Directory.GetDirectories(targDir);
			string[] files = Directory.GetFiles(targDir);

			if (
				dirs.Length == 0 &&
				files.Length == 0
				)
				return true;

			return false;
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

		/// <summary>
		/// 配布
		/// </summary>
		private void Distribute()
		{
			Console.WriteLine("配布を行うので回収先フォルダは開きません。");
			OpenOutputDirIfNeeded = false;

			Console.WriteLine("Distribute-ST");

			string[] rPaths = Directory.GetDirectories(this.OutputDir)
				.Concat(Directory.GetFiles(this.OutputDir, "*.zip"))
				.ToArray();

			string[] wPaths = SCommon.Concat(this.DistributeDirs.Select(v => Directory.GetDirectories(v)
				.Concat(Directory.GetFiles(v, "*.zip"))))
				.ToArray();

			{
				List<string> only1 = new List<string>();
				List<string> both1 = new List<string>();
				List<string> both2 = new List<string>();
				List<string> only2 = new List<string>();

				SCommon.Merge(rPaths, wPaths, CompDistribute, only1, both1, both2, only2);

				foreach (string str in both1)
					Console.WriteLine("*< " + str);

				foreach (string str in both2)
					Console.WriteLine("*> " + str);

				foreach (string str in only1)
					Console.WriteLine("/< " + str);

				foreach (string str in only2)
					Console.WriteLine("/> " + str);

				if (only1.Count != 0)
					throw new Exception("未配信のプロジェクト：" + only1[0]);

				if (only2.Count != 0)
					throw new Exception("廃止されたプロジェクト：" + only2[0]);
			}

			if (SCommon.HasSame_Comp(rPaths, CompDistribute))
				throw new Exception("配布元のプロジェクトの重複");

			if (SCommon.HasSame_Comp(wPaths, CompDistribute))
				throw new Exception("配布先のプロジェクトの重複");

			int count = rPaths.Length;
			//int count = wPaths.Length; // どっちでも良い。

			for (int index = 0; index < count; index++)
			{
				string rPath = rPaths[index];
				string wPath = wPaths[index];
				string destPath = Path.Combine(Path.GetDirectoryName(wPath), Path.GetFileName(rPath));

				Console.WriteLine("< " + rPath);
				Console.WriteLine("W " + wPath);
				Console.WriteLine("> " + destPath);

				if (this.BackupDir != null && File.Exists(wPath))
					this.BackupOldVersion(wPath);

				SCommon.DeletePath(wPath);
				SCommon.CopyPath(rPath, destPath);

				Console.WriteLine("done");
			}

			Console.WriteLine("Distribute-ED");
		}

		private int CompDistribute(string a, string b)
		{
			a = CompDistributeFilter(a);
			b = CompDistributeFilter(b);

			return SCommon.CompIgnoreCase(a, b);
		}

		private string CompDistributeFilter(string path)
		{
			string name = Path.GetFileName(path);

			// Resolve version number
			{
				string fmt = name;

				foreach (char chr in SCommon.DECIMAL)
					fmt = fmt.Replace(chr, '9');

				if (SCommon.EndsWithIgnoreCase(fmt, "_v9999-999-99999.zip"))
				{
					name = name.Substring(0, name.Length - 20) + "_v9999-999-99999.zip";
				}
			}

			return name;
		}

		private void BackupOldVersion(string rFile)
		{
			string wFile = Path.Combine(this.BackupDir, Path.GetFileName(rFile));

			Console.WriteLine("B< " + rFile);
			Console.WriteLine("B> " + wFile);

			SCommon.DeletePath(wFile); // 2bs
			File.Copy(rFile, wFile);

			this.BOV_ToSlim(1);
			this.BOV_ToSlim(2);
		}

		private class BOV_FileInfo
		{
			public string FilePath;
			public string Title;
			public long TimeStamp;
			public bool OutOfRule;

			public BOV_FileInfo(string file)
			{
				this.FilePath = file;

				string name = Path.GetFileName(file);
				string fmt = name;
				SCommon.DECIMAL.ForEach(chr => fmt = fmt.Replace(chr, '9'));

				if (SCommon.EndsWithIgnoreCase(fmt, "_v9999-999-99999.zip"))
				{
					this.Title = name.Substring(0, name.Length - 20);
					this.TimeStamp = long.Parse(new string(name.Substring(name.Length - 20)
						.Where(chr => SCommon.DECIMAL.Contains(chr))
						.ToArray()));
					this.OutOfRule = false;
				}
				else
				{
					this.Title = name;
					this.TimeStamp = 0L;
					this.OutOfRule = true;
				}
			}
		}

		private void BOV_ToSlim(int mode)
		{
			BOV_FileInfo[] fileInfos = Directory.GetFiles(this.BackupDir)
				.Select(v => new BOV_FileInfo(v))
				.Where(v => !v.OutOfRule)
				.ToArray();

			string[] titles = fileInfos
				.Select(v => v.Title)
				.DistinctOrderBy(SCommon.CompIgnoreCase)
				.ToArray();

			foreach (string title in titles)
			{
				BOV_FileInfo[] titleFileInfos = fileInfos
					.Where(v => SCommon.EqualsIgnoreCase(v.Title, title))
					.OrderBy((a, b) => SCommon.Comp(a.TimeStamp, b.TimeStamp))
					.ToArray();

				switch (mode)
				{
					case 1:
						if (2 <= titleFileInfos.Count())
						{
							BOV_FileInfo a = titleFileInfos[titleFileInfos.Length - 1];
							BOV_FileInfo b = titleFileInfos[titleFileInfos.Length - 2];
							long timeStampDiff = a.TimeStamp - b.TimeStamp;

							//Console.WriteLine("timeStampDiff: " + timeStampDiff);

							if (timeStampDiff < 200000) // ? 推定2日未満
							{
								Console.WriteLine("B1 " + a.FilePath);

								SCommon.DeletePath(a.FilePath);
							}
						}
						break;

					case 2:
						if (3 < titleFileInfos.Length) // ? 古いバージョンが多すぎる。
						{
							BOV_FileInfo a = titleFileInfos[0];

							Console.WriteLine("B2 " + a.FilePath);

							SCommon.DeletePath(a.FilePath);
						}
						break;

					default:
						throw null; // never
				}
			}
		}
	}
}
