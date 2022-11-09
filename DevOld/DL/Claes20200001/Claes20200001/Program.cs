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
using Charlotte.WebServices;

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
			string resUrlList = @"

https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust1.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust2.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust3.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust4.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust5.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust6.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust7.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust8.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust9.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust10.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust11.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust12.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust13.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust14.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust15.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust16.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust17.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust18.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust19.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust20.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust21.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust22.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust23.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust24.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust25.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust26.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust27.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust28.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust29.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust30.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust31.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust32.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust33.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust34.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust35.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust36.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust37.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust38.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust39.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust40.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust41.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust42.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust43.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust44.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust45.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust46.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust47.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust48.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust49.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust50.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust51.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust52.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust53.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust54.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust55.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust56.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust57.png
https://chicodeza.com/wordpress/wp-content/uploads/torannpu-illust58.png

";

			string[] urlList = SCommon.TextToLines(resUrlList).Select(v => v.Trim()).Where(v => v != "").ToArray();

			foreach (string url in urlList)
			{
				HTTPClient hc = new HTTPClient(url);

				hc.Get();

				File.WriteAllBytes(Common.NextOutputPath() + ".png", hc.ResBody);
			}
		}
	}
}
