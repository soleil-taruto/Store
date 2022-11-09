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

			Main4(new ArgsReader(new string[] { }));
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

		private static readonly string INPUT_ROOT_DIR_01 = @"C:\Chest";
		private static readonly string INPUT_ROOT_DIR_02 = @"C:\DevBin";
		private static readonly string OUTPUT_ROOT_DIR = @"C:\home\GitHub\Store\DevOld";

		private class ProjectInfo
		{
			public int Date;
			public string Title;
			public string SourceDir;

			public ProjectInfo(string dir)
			{
				string[] pTkns = dir.Split('\\');
				int p = pTkns.Length;

				if (SCommon.IndexOfIgnoreCase(Consts.SRC_LOCAL_DIRS, pTkns[--p]) == -1)
					throw null;

				string title = pTkns[--p];
				int date;

				if (Common.LiteFormatDIG(title).StartsWith("99999999_"))
				{
					date = int.Parse(title.Substring(0, 8));
					title = title.Substring(9);
				}
				else
				{
					while (!Common.LiteFormatDIG(pTkns[--p]).StartsWith("99999999_")) ;
					date = int.Parse(pTkns[p].Substring(0, 8));
				}

				// タイトルの "()" を除去
				{
					int i = title.IndexOf('(');

					if (i != -1)
						title = title.Substring(0, i);
				}

				if (title == "")
					throw new Exception("Bad title");

				this.Date = date;
				this.Title = title;
				this.SourceDir = dir;
			}
		}

		private List<ProjectInfo> Projects = new List<ProjectInfo>();
		private List<string> Logs = new List<string>();

		private void Main5(ArgsReader ar)
		{
			if (!Directory.Exists(INPUT_ROOT_DIR_01))
				throw new Exception("no INPUT_ROOT_DIR_01");

			if (!Directory.Exists(INPUT_ROOT_DIR_02))
				throw new Exception("no INPUT_ROOT_DIR_02");

			if (!Directory.Exists(OUTPUT_ROOT_DIR))
				throw new Exception("no OUTPUT_ROOT_DIR");

			Queue<string> q = new Queue<string>();

			foreach (string rootDir in Directory.GetDirectories(INPUT_ROOT_DIR_01))
				if (SCommon.EqualsIgnoreCase(Common.LiteFormatDIG(Path.GetFileName(rootDir)), "99999999_DevBin"))
					foreach (string dir in Directory.GetDirectories(rootDir))
						q.Enqueue(dir);

			foreach (string dir in Directory.GetDirectories(INPUT_ROOT_DIR_02))
				q.Enqueue(dir);

			while (1 <= q.Count)
			{
				string dir = q.Dequeue();

				// ゲームのリソースフォルダは除外する。
				{
					string localName = Path.GetFileName(dir);

					if (
						SCommon.EqualsIgnoreCase(localName, "dat") ||
						SCommon.EqualsIgnoreCase(localName, "res")
						)
						continue;
				}

				if (SCommon.IndexOfIgnoreCase(Consts.SRC_LOCAL_DIRS, Path.GetFileName(dir)) != -1)
				{
					Projects.Add(new ProjectInfo(dir));
				}
				else
				{
					foreach (string subDir in Directory.GetDirectories(dir))
						q.Enqueue(subDir);
				}
			}

			q = null;

			SCommon.DeletePath(OUTPUT_ROOT_DIR);
			SCommon.CreateDir(OUTPUT_ROOT_DIR);

			ProcMain.WriteLog("COPY-ST");

			string[] titles = Projects.Select(v => v.Title).DistinctOrderBy(SCommon.CompIgnoreCase).ToArray();

			foreach (string title in titles)
			{
				ProjectInfo[] titleProjects = Projects.Where(v => SCommon.EqualsIgnoreCase(v.Title, title)).ToArray();

				Array.Sort(titleProjects, (a, b) => a.Date - b.Date);

				ProjectInfo lastProject = titleProjects[titleProjects.Length - 1];

				string rDir = lastProject.SourceDir;
				string wDir = Path.Combine(OUTPUT_ROOT_DIR, lastProject.Title, Path.GetFileName(lastProject.SourceDir));

				ProcMain.WriteLog("< " + rDir);
				ProcMain.WriteLog("> " + wDir);

				Logs.Add("< " + rDir);
				Logs.Add("> " + wDir);

				SCommon.CopyDir(rDir, wDir);

				// ----

				CopyResourceDir(rDir, wDir, "dat", true);
				CopyResourceDir(rDir, wDir, "res", true);
				CopyResourceDir(rDir, wDir, "doc", false);
			}

			File.WriteAllLines(Path.Combine(OUTPUT_ROOT_DIR, "Copy.log"), Logs, Encoding.UTF8);

			ProcMain.WriteLog("COPY-ED");
		}

		private IDisposable WriteLogSection(Action<object, Action<object>> routine)
		{
			Action<object> writeLogOrig = ProcMain.WriteLog;

			ProcMain.WriteLog = message => routine(message, writeLogOrig);

			return SCommon.GetAnonyDisposable(() => ProcMain.WriteLog = writeLogOrig);
		}

		private void CopyResourceDir(string rDir, string wDir, string resourceRelDir, bool outputFileListMode)
		{
			rDir = Path.Combine(Path.GetDirectoryName(rDir), resourceRelDir);
			wDir = Path.Combine(Path.GetDirectoryName(wDir), resourceRelDir);

			if (Directory.Exists(rDir))
			{
				if (outputFileListMode)
				{
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("T " + wDir);

					Logs.Add("< " + rDir);
					Logs.Add("T " + wDir);

					string treeFile = Path.Combine(wDir, "_Tree.txt");
					string[] treeFileData = MakeTreeFileData(rDir);

					SCommon.CreateDir(wDir);

					File.WriteAllLines(treeFile, treeFileData, Encoding.UTF8);
				}
				else
				{
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("> " + wDir);

					Logs.Add("< " + rDir);
					Logs.Add("> " + wDir);

					SCommon.CopyDir(rDir, wDir);
				}
			}
		}

		private string[] MakeTreeFileData(string targDir)
		{
			string[] paths = Directory.GetDirectories(targDir, "*", SearchOption.AllDirectories)
				.Concat(Directory.GetFiles(targDir, "*", SearchOption.AllDirectories))
				.OrderBy(SCommon.Comp)
				.ToArray();

			List<string> dest = new List<string>();

			foreach (string path in paths)
			{
				dest.Add(SCommon.ChangeRoot(path, targDir));

				if (Directory.Exists(path))
				{
					dest.Add("\t-> Directory");
				}
				else
				{
					FileInfo info = new FileInfo(path);

					dest.Add(string.Format(
						"\t-> File {0} / {1} / {2:#,0}"
						, new SCommon.SimpleDateTime(info.CreationTime)
						, new SCommon.SimpleDateTime(info.LastWriteTime)
						, info.Length
						));
				}
			}
			return dest.ToArray();
		}
	}
}
