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
			Canvas canvas = Canvas.LoadFromFile(@"C:\Dev\Game\Megaten\dat\dat\run\22350006_big_p24.jpg");

			canvas = canvas.Expand(960, 540);

			for (int x = 85; x < canvas.W - 85; x++)
			{
				for (int y = 10; y < 385; y++)
				{
					canvas[x, y] = new I4Color(0, 0, 0, 0);
				}
			}
			canvas.Save(Common.NextOutputPath() + ".png");
		}
	}
}
