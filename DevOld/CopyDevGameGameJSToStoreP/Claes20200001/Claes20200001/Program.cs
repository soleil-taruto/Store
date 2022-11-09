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

			Main4(new ArgsReader(new string[] { }));
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

				Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			if (!Directory.Exists(Consts.R_ROOT_DIR))
				throw new Exception("no R_ROOT_DIR");

			if (!Directory.Exists(Consts.W_ROOT_DIR))
				throw new Exception("no W_ROOT_DIR");

			Console.WriteLine("start...");

			// 出力先クリア
			SCommon.DeletePath(Consts.W_ROOT_DIR);
			SCommon.CreateDir(Consts.W_ROOT_DIR);

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
					string wDir = SCommon.ChangeRoot(rDir, Consts.R_ROOT_DIR, Consts.W_ROOT_DIR);

					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("> " + wDir);

					SCommon.CopyDir(rDir, wDir);

					ProcMain.WriteLog("done");
				}
			}
		}
	}
}
