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

			Main4(new ArgsReader(new string[] { @"C:\temp\nc229908.png", "32", "32" }));
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

				//MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			string imageFile = SCommon.MakeFullPath(ar.NextArg());
			int tile_w = int.Parse(ar.NextArg());
			int tile_h = int.Parse(ar.NextArg());

			if (!File.Exists(imageFile))
				throw new Exception("no imageFile");

			if (tile_w < 1 || SCommon.IMAX < tile_w)
				throw new Exception("Bad tile_w");

			if (tile_h < 1 || SCommon.IMAX < tile_h)
				throw new Exception("Bad tile_h");

			Canvas canvas = Canvas.LoadFromFile(imageFile);

			for (int x = 0; ; x++)
			{
				int tile_l = x * tile_w;

				if (canvas.W < tile_l + tile_w)
					break;

				for (int y = 0; ; y++)
				{
					int tile_t = y * tile_h;

					if (canvas.H < tile_t + tile_h)
						break;

					canvas
						.GetSubImage(tile_l, tile_t, tile_w, tile_h)
						.Save(Path.Combine(
							SCommon.GetOutputDir(),
							string.Format("Tile_{0:D2}{1:D2}.png", x, y)));
				}
			}
		}
	}
}
