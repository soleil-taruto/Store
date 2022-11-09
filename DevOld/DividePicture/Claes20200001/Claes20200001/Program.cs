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
using Charlotte.SubCommons;

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
				Main4();
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			//Common.Pause();
		}

		private void Main4()
		{
			// -- choose one --

			Main5();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}

		private void Main5()
		{
			// -- choose one --

			DividePicture(@"C:\Dev\Game\MSSAGame\dat\dat\ぴぽや倉庫\sentou-effect-anime2\640x480\pipo-btleffect038.png", 240, 240);

			// --
		}

		private void DividePicture(string srcImgFile, int div_w, int div_h)
		{
			Canvas canvas = Canvas.LoadFromFile(srcImgFile);

			int w = canvas.W;
			int h = canvas.H;

			int num_x = w / div_w;
			int num_y = h / div_h;

			for (int x = 0; x < num_x; x++)
			{
				for (int y = 0; y < num_y; y++)
				{
					canvas.SubRect(new I4Rect(div_w * x, div_h * y, div_w, div_h)).Save(Common.NextOutputPath() + ".png");
				}
			}
		}
	}
}
