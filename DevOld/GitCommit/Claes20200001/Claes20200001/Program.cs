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
using Charlotte.Utilities;

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

			Main4(new ArgsReader(new string[] { "ss", @"C:\home\GitHub\StoreP" }));
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
			string commitComment = ar.NextArg();
			string dir = ar.NextArg();

			ar.End();

			Commit(dir, commitComment);
		}

		/// <summary>
		/// 追加・更新・削除されたファイルをリポジトリに追加・更新・削除(git.exe add *)して
		/// コミット(git.exe commit -m commitComment)する。
		/// </summary>
		/// <param name="dir">リポジトリ-DIR</param>
		/// <param name="commitComment">コミット時のコメント</param>
		private void Commit(string dir, string commitComment)
		{
			dir = SCommon.MakeFullPath(dir);

			if (!Directory.Exists(dir))
				throw new Exception("no dir: " + dir);

			if (!Directory.Exists(Path.Combine(dir, ".git")))
				throw new Exception("no .git: " + dir);

			if (!File.Exists(Path.Combine(dir, ".gitattributes")))
				throw new Exception("no .gitattributes: " + dir);

			if (string.IsNullOrEmpty(commitComment))
				throw new Exception("no commitComment");

			SCommon.Batch(
				new string[]
				{
					GitCommandUtils.GetGitProgram() + " add *",
					GitCommandUtils.GetGitProgram() + " commit -m \"" + commitComment + "\"",
				},
				dir
				);
		}
	}
}
