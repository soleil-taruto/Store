using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;
using Charlotte.Utilities;
using System.IO;
using System.Text.RegularExpressions;

namespace Charlotte.Tests
{
	public class Test0001
	{
		private string[] INPUT_ROOT_DIRS = new string[]
		{
			@"C:\Chest\20220919_DevBin",
			@"C:\DevBin",
		};

		private string OUTPUT_ROOT_DIR = @"C:\home\GitHub\Store\DevOld";

		/// <summary>
		/// ソースDIR の ローカル名
		/// </summary>
		public static readonly string[] SRC_LOCAL_DIRS = new string[]
		{
			"Elsa20200001", // Game
			"Claes20200001", // CUI
			"Silvia20200001", // GUI
			"Silvia20210001", // GUI -- 特例
			"Gattonero20200001", // GameJS
			"Petra20200001", // C-Lang
		};

		private class ProjectInfo
		{
			public int Date;
			public string Title;
			public string SourceDir;

			public ProjectInfo(string dir)
			{
				string[] pTkns = dir.Split('\\');
				int p = pTkns.Length;

				p--;
				if (
					!pTkns[p].EndsWith("20200001") &&
					!pTkns[p].EndsWith("20210001") // 特例
					)
					throw null;

				string title = pTkns[--p];
				int date;

				if (IsDatedLocalName(title))
				{
					date = int.Parse(title.Substring(0, 8));
					title = title.Substring(9);
				}
				else
				{
					while (!IsDatedLocalName(pTkns[--p])) ;
					date = int.Parse(pTkns[p].Substring(0, 8));
				}

				{
					int i = title.IndexOf('(');

					if (i != -1)
						title = title.Substring(0, i);
				}

				if (title == "") throw null; // 2bs

				this.Date = date;
				this.Title = title;
				this.SourceDir = dir;
			}
		}

		private static bool IsDatedLocalName(string title)
		{
			return Regex.IsMatch(title, "^[0-9]{8}_.+$");
		}

		private List<ProjectInfo> Projects = new List<ProjectInfo>();

		public void Test01()
		{
			foreach (string rootDir in INPUT_ROOT_DIRS)
				if (!Directory.Exists(rootDir))
					throw null;

			if (!Directory.Exists(OUTPUT_ROOT_DIR))
				throw null;

			// ---- check ここまで

			Queue<string> q = new Queue<string>();

			foreach (string rootDir in INPUT_ROOT_DIRS)
				foreach (string dir in Directory.GetDirectories(rootDir))
					q.Enqueue(dir);

			while (1 <= q.Count)
			{
				string dir = q.Dequeue();

				// ゲームのプロジェクトの dat, ret は配下が深いので回避
				{
					string localName = Path.GetFileName(dir);

					if (
						SCommon.EqualsIgnoreCase(localName, "dat") ||
						SCommon.EqualsIgnoreCase(localName, "ret")
						)
						continue;
				}

				if (SCommon.IndexOfIgnoreCase(SRC_LOCAL_DIRS, Path.GetFileName(dir)) != -1)
				{
					Projects.Add(new ProjectInfo(dir));
				}
				else
				{
					foreach (string subDir in Directory.GetDirectories(dir))
						q.Enqueue(subDir);
				}
			}

			// ---- cout+

			string[] titles = Projects.Select(v => v.Title).DistinctOrderBy(SCommon.CompIgnoreCase).ToArray();

			foreach (string title in titles)
			{
				Console.WriteLine(title + " ==> " + Projects.Where(v => SCommon.EqualsIgnoreCase(v.Title, title)).Count());
			}

			int maxHistCount = titles.Select(v => Projects.Where(w => SCommon.EqualsIgnoreCase(w.Title, v)).Count()).Max();

			Console.WriteLine("maxHistCount: " + maxHistCount);

			// ----

			Console.WriteLine("COPY-ST");
			//SCommon.Pause();

			for (int histIndex = 0; histIndex < maxHistCount; histIndex++)
			{
				SCommon.DeletePath(OUTPUT_ROOT_DIR);
				SCommon.CreateDir(OUTPUT_ROOT_DIR);

				List<string> logs = new List<string>();

				foreach (string title in titles)
				{
					ProjectInfo[] titleProjects = Projects.Where(v => SCommon.EqualsIgnoreCase(v.Title, title)).ToArray();

					Array.Sort(titleProjects, (a, b) => a.Date - b.Date);

					ProjectInfo currProject = titleProjects[Math.Min(histIndex, titleProjects.Length - 1)];

					string rDir = currProject.SourceDir;
					string wDir = Path.Combine(OUTPUT_ROOT_DIR, currProject.Title, Path.GetFileName(currProject.SourceDir));

					Console.WriteLine("< " + rDir);
					Console.WriteLine("> " + wDir);

					logs.Add("< " + rDir);
					logs.Add("> " + wDir);

					SCommon.DeletePath(wDir);
					SCommon.CopyDir(rDir, wDir);
				}

				File.WriteAllLines(Path.Combine(OUTPUT_ROOT_DIR, "Copy.log"), logs, Encoding.UTF8);

				Console.WriteLine("histIndex: " + histIndex);
				//SCommon.Pause();

				SCommon.Batch(
					new string[]
					{
						@"CALL C:\home\GitHub\GitHubコミット前に実行してね.bat",
						@"C:\apps\GitCommit\GitCommit.exe h" + histIndex.ToString("D2") + @" C:\home\GitHub\Store",
					},
					"",
					SCommon.StartProcessWindowStyle_e.MINIMIZED
					);
			}
			Console.WriteLine("COPY-ED");
		}
	}
}
