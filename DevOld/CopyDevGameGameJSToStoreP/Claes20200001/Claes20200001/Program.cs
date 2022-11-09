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

			Main4(new ArgsReader(new string[] { "C" }));
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

		private string WRootDir;

		private void Main5(ArgsReader ar)
		{
			string alpha = ar.NextArg();

			ar.End();

			if (alpha.Length != 1)
				throw new Exception("Bad alpha (Length)");

			if (!SCommon.ALPHA.Contains(alpha[0]))
				throw new Exception("Bad alpha (not A-Z)");

			WRootDir = string.Format(Consts.W_ROOT_DIR_FORMAT, alpha);

			Console.WriteLine("< " + Consts.R_ROOT_DIR);
			Console.WriteLine("> " + WRootDir);

			if (!Directory.Exists(Consts.R_ROOT_DIR))
				throw new Exception("no R_ROOT_DIR");

			if (!Directory.Exists(WRootDir))
				throw new Exception("no WRootDir");

			// 出力先クリア
			SCommon.DeletePath(WRootDir);
			SCommon.CreateDir(WRootDir);

			Queue<string[]> q = new Queue<string[]>();

			q.Enqueue(new string[] { Consts.R_ROOT_DIR });

			while (1 <= q.Count)
			{
				foreach (string dir in q.Dequeue())
				{
					if (IsProjectDir(dir))
					{
						CopySourceDir(dir);
					}
					else
					{
						q.Enqueue(Directory.GetDirectories(dir));
					}
				}
			}
			Console.WriteLine("done!");
		}

		private bool IsProjectDir(string dir)
		{
			return Consts.SRC_LOCAL_DIRS.Any(v => Directory.Exists(Path.Combine(dir, v)));
		}

		private void CopySourceDir(string projectDir)
		{
			foreach (string srcLocalDir in Consts.SRC_LOCAL_DIRS)
			{
				string rDir = Path.Combine(projectDir, srcLocalDir);

				if (Directory.Exists(rDir))
				{
					string wDir = SCommon.ChangeRoot(rDir, Consts.R_ROOT_DIR, WRootDir);

					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("> " + wDir);

					SCommon.CopyDir(rDir, wDir);

					ProcMain.WriteLog("done");

					// ----

					if (srcLocalDir[0] == 'E') // Game
					{
						CopyResourceDir(projectDir, @"dat\dat", true); // 画像・音楽 etc.
						CopyResourceDir(projectDir, @"dat\res", false); // シナリオ・マップデータ etc.
					}
					else if (srcLocalDir[0] == 'G') // GameJS
					{
						CopyResourceDir(projectDir, @"res", true); // 画像・音楽 etc.
					}
				}
			}
		}

		private void CopyResourceDir(string projectDir, string resourceRelDir, bool outputFileListMode)
		{
			string rDir = Path.Combine(projectDir, resourceRelDir);

			if (Directory.Exists(rDir))
			{
				string wDir = SCommon.ChangeRoot(rDir, Consts.R_ROOT_DIR, WRootDir);

				if (outputFileListMode)
				{
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("T " + wDir);

					string treeFile = Path.Combine(wDir, "_Tree.txt");
					string[] treeFileData = MakeTreeFileData(rDir);

					SCommon.CreateDir(wDir);

					File.WriteAllLines(treeFile, treeFileData, Encoding.UTF8);
				}
				else
				{
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("> " + wDir);

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
