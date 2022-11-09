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

			Main4(new ArgsReader(new string[] { @"C:\temp" }));
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

				MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private string R_Dir;
		private string W_File;

		private void Main5(ArgsReader ar)
		{
			R_Dir = SCommon.MakeFullPath(ar.NextArg());

			ar.End();

			if (!Directory.Exists(R_Dir))
				throw new Exception("no R_Dir");

			W_File = Path.Combine(R_Dir, "_auto.js");

			ProcMain.WriteLog("< " + R_Dir);
			ProcMain.WriteLog("> " + W_File);

			File.WriteAllBytes(W_File, SCommon.EMPTY_BYTES); // 出力テスト＆クリア

			string[] dirs = Directory
				.GetDirectories(R_Dir, "*", SearchOption.AllDirectories)
				.Where(v => IsTargetPath(v))
				.OrderBy(SCommon.Comp)
				.ToArray();

			string[] files = Directory
				.GetFiles(R_Dir, "*", SearchOption.AllDirectories)
				.Where(v => IsTargetPath(v))
				.OrderBy(SCommon.Comp)
				.ToArray();

			string[] lines = GetAutoJSLines(dirs, files)
				.ToArray();

			File.WriteAllLines(W_File, lines, Encoding.UTF8);

			ProcMain.WriteLog("done");
		}

		private IEnumerable<string> GetAutoJSLines(string[] dirs, string[] files)
		{
			yield return "// _auto.js Encoding=UTF-8";
			yield return "";
			yield return "let LS_DIRS =";
			yield return "[";

			foreach (string dir in dirs)
			{
				yield return "\t\"" + ToJSPath(dir) + "\",";
			}
			yield return "];";
			yield return "";
			yield return "let LS_FILES =";
			yield return "[";

			foreach (string file in files)
			{
				yield return "\t\"" + ToJSPath(file) + "\",";
			}
			yield return "];";
			yield return "";
			yield return "let LS_FILE_SIZE_LIST =";
			yield return "[";

			foreach (string file in files)
			{
				yield return "\t// " + ToJSPath(file);
				yield return "\t" + new FileInfo(file).Length + ",";
			}
			yield return "];";
			yield return "";
			yield return "let LS_FILE_INFO_LIST =";
			yield return "[";

			foreach (string file in files)
			{
				yield return "\t// " + ToJSPath(file);
				yield return "\t{";

				foreach (string[] pair in GetFileInfoPairs(file))
				{
					yield return "\t\t" + pair[0] + ": " + pair[1] + ",";
				}
				yield return "\t},";
				yield return "";
			}
			yield return "];";
			yield return "";
			yield return "// _auto.js END";
		}

		private bool IsTargetPath(string path)
		{
			// '_' で始まるローカル名とその配下は除外する。
			//
			if (
				path.StartsWith("_") ||
				path.Contains("\\_")
				)
				return false;

			return true;
		}

		private string ToJSPath(string path)
		{
			path = SCommon.ChangeRoot(path, R_Dir);
			path = path.Replace('\\', '/');
			return path;
		}

		private IEnumerable<string[]> GetFileInfoPairs(string file)
		{
			string ext = Path.GetExtension(file)
				.ToLower();

			if (Consts.IMAGE_EXTS.Contains(ext)) // ? 画像ファイル
			{
				Image image = Image.FromFile(file);

				yield return new string[] { "type", "\"image\"" };
				yield return new string[] { "width", "" + image.Width };
				yield return new string[] { "height", "" + image.Height };

				image.Dispose();
				image = null;

				yield break;
			}

			// その他のファイル
			{
				yield return new string[] { "type", "\"unknown\"" };
				yield break;
			}
		}
	}
}
