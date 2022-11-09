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

		private void Main5(ArgsReader ar)
		{
			string inputPath = SCommon.MakeFullPath(ar.NextArg());

			if (Directory.Exists(inputPath))
			{
				Main6(inputPath);
			}
			else if (File.Exists(inputPath))
			{
				using (WorkingDir wd = new WorkingDir())
				{
					string dir = wd.MakePath();

					SCommon.CreateDir(dir);
					File.Copy(inputPath, Path.Combine(dir, Path.GetFileName(inputPath)));

					Main6(dir);
				}
			}
			else
			{
				throw new Exception("Bad inputPath");
			}
			Console.WriteLine("done!");
		}

		private void Main6(string dir)
		{
			string outputDir = SCommon.GetOutputDir();
			string mirrorDir = Path.Combine(outputDir, "Mirror");
			string inverseDir = Path.Combine(outputDir, "Inverse");
			string mirrorInverseDir = Path.Combine(outputDir, "Mirror_Inverse");

			SCommon.CreateDir(mirrorDir);
			SCommon.CreateDir(inverseDir);
			SCommon.CreateDir(mirrorInverseDir);

			foreach (string file in Directory.GetFiles(dir))
			{
				if (Consts.IMAGE_EXTS.Any(imageExt => SCommon.EqualsIgnoreCase(imageExt, Path.GetExtension(file))))
				{
					Console.WriteLine("< " + file);

					Canvas canvas = Canvas.LoadFromFile(file);
					string outputLocalName = Path.GetFileNameWithoutExtension(file) + ".png";

					canvas.Mirror().Save(Path.Combine(mirrorDir, outputLocalName));
					canvas.Inverse().Save(Path.Combine(inverseDir, outputLocalName));
					canvas.Mirror().Inverse().Save(Path.Combine(mirrorInverseDir, outputLocalName));

					Console.WriteLine("done");
				}
			}
		}
	}
}
