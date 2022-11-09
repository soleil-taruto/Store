using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

			Main4(new ArgsReader(new string[] { "GIT-HUB-REPO-UNSAFE-MOD", @"C:\home\GitHub" }));
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

			// 処理が一瞬で終わってもコンソールが見えるように
			Thread.Sleep(500);
		}

		private void Main5(ArgsReader ar)
		{
			if (!ar.ArgIs("GIT-HUB-REPO-UNSAFE-MOD"))
				throw new Exception("Need GIT-HUB-REPO-UNSAFE-MOD command-option");

			string rootDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(rootDir))
				throw new Exception("no rootDir");

			string[] repositoryDirs = GetRepositoryDirs(rootDir).ToArray();
			string[] paths = SCommon.Concat(repositoryDirs.Select(repositoryDir => GetCommitingPaths(repositoryDir))).ToArray();

			// ソート
			// 1. ファイル -> ディレクトリ
			// 2. 深いパス -> 浅いパス
			// 3. 辞書順
			{
				Func<string, int> order_01 = path =>
				{
					if (File.Exists(path))
						return 1;

					if (Directory.Exists(path))
						return 2;

					throw null; // never
				};

				Func<string, int> order_02 = path => path.Count(chr => chr == '\\') * -1;

				Array.Sort(paths, (a, b) =>
				{
					int ret = order_01(a) - order_01(b);

					if (ret == 0)
					{
						ret = order_02(a) - order_02(b);

						if (ret == 0)
							ret = SCommon.CompIgnoreCase(a, b);
					}
					return ret;
				});
			}

			foreach (string path in paths)
			{
				if (IsEmptyDir(path))
				{
					// HACK: 元からこのファイルがあったのか判別できない。
					// -- そもそも無いと思う。-- そんな名前のファイルを置かない。
					// -- 本プログラムをコミット前に複数回実行しても良いようにしたいので、エスケープするなどができない。
					// --> というわけで看過する。

					File.WriteAllBytes(Path.Combine(path, "$$GHRF_Empty"), SCommon.EMPTY_BYTES);
				}
			}

			// 注意：以下パス名変更を行うので、パスに対する処理(ファイル更新など)はここまでに行っておくこと。

			foreach (string path in paths)
			{
				string dir = Path.GetDirectoryName(path);
				string localName = Path.GetFileName(path);
				string localNameNew = ChangeLocalName(localName);
				string pathNew = Path.Combine(dir, localNameNew);

				if (!SCommon.EqualsIgnoreCase(path, pathNew)) // ? パス名変更有り
				{
					Console.WriteLine("< " + path);
					Console.WriteLine("> " + pathNew);

					if (Directory.Exists(path))
						Directory.Move(path, pathNew);
					else
						File.Move(path, pathNew);
				}
			}
		}

		private bool IsEmptyDir(string path)
		{
			return
				Directory.Exists(path) &&
				Directory.GetDirectories(path).Length == 0 &&
				Directory.GetFiles(path).Length == 0;
		}

		private string ChangeLocalName(string localName)
		{
			StringBuilder buff = new StringBuilder();

			foreach (char chr in localName)
			{
				// HACK: 元の名前に $xxxx を含む場合を想定していない。
				// -- そもそも無いと思う。-- そんな名前付けない。
				// -- 本プログラムをコミット前に複数回実行しても良いようにしたいので、エスケープするなどができない。
				// --> というわけで看過する。

				if (SCommon.HALF.Contains(chr))
				{
					buff.Append(chr);
				}
				else
				{
					buff.Append('$');
					buff.Append(((int)chr).ToString("x4"));
				}
			}
			return buff.ToString();
		}

		private IEnumerable<string> GetRepositoryDirs(string currDir)
		{
			if (Directory.Exists(Path.Combine(currDir, ".git")))
			{
				yield return currDir;
			}
			else
			{
				foreach (string dir in Directory.GetDirectories(currDir))
					foreach (string relay in GetRepositoryDirs(dir))
						yield return relay;
			}
		}

		private IEnumerable<string> GetCommitingPaths(string currDir)
		{
			foreach (string dir in Directory.GetDirectories(currDir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(dir), ".git"))
					continue; // 除外

				foreach (string relay in GetCommitingPaths(dir))
					yield return relay;

				yield return dir;
			}
			foreach (string file in Directory.GetFiles(currDir))
			{
				if (SCommon.EqualsIgnoreCase(Path.GetFileName(file), ".gitattributes"))
					continue; // 除外

				yield return file;
			}
		}
	}
}
