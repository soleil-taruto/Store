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

			Main4(new ArgsReader(new string[] { @"C:\temp\nc25161.png", "0", "0", "40", "40", "2" }));
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
			int allTiles_l = int.Parse(ar.NextArg());
			int allTiles_t = int.Parse(ar.NextArg());
			int tile_w = int.Parse(ar.NextArg());
			int tile_h = int.Parse(ar.NextArg());
			int extend = int.Parse(ar.NextArg());

			if (!File.Exists(imageFile))
				throw new Exception("no imageFile");

			if (allTiles_l < 0 || SCommon.IMAX < allTiles_l)
				throw new Exception("Bad allTiles_l");

			if (allTiles_t < 0 || SCommon.IMAX < allTiles_t)
				throw new Exception("Bad allTiles_t");

			if (tile_w < 1 || SCommon.IMAX < tile_w)
				throw new Exception("Bad tile_w");

			if (tile_h < 1 || SCommon.IMAX < tile_h)
				throw new Exception("Bad tile_h");

			if (extend < 1 || SCommon.IMAX < extend)
				throw new Exception("Bad extend");

			Canvas canvas = Canvas.LoadFromFile(imageFile);

			for (int x = 0; ; x++)
			{
				int tile_l = allTiles_l + x * tile_w;

				if (canvas.W < tile_l + tile_w)
					break;

				for (int y = 0; ; y++)
				{
					int tile_t = allTiles_t + y * tile_h;

					if (canvas.H < tile_t + tile_h)
						break;

					// タイル生成・保存
					{
						Canvas tile = canvas.GetSubImage(tile_l, tile_t, tile_w, tile_h);

						if (2 <= extend)
							tile = tile.Expand(tile_w * extend, tile_h * extend, 1); // 整数倍なのでサンプリングは1回で良い。

						tile.Save(Path.Combine(
							SCommon.GetOutputDir(),
							string.Format("Tile_{0:D2}{1:D2}.png", x, y)));
					}
				}
			}
		}
	}
}
