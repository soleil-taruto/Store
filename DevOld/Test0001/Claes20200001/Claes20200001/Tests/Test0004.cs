using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0004
	{
		private const string INPUT_DIR = @"C:\temp";

		public void Test01()
		{
			string[] jpegFiles = Directory
				.GetFiles(INPUT_DIR)
				.Where(v =>
					SCommon.EndsWithIgnoreCase(v, ".jpg") ||
					SCommon.EndsWithIgnoreCase(v, ".jpeg")
					)
				.ToArray();

			foreach (string jpegFile in jpegFiles)
			{
				Console.WriteLine(jpegFile);

				using (Bitmap bmp = new Bitmap(jpegFile))
				{
					foreach (PropertyItem pi in bmp.PropertyItems)
					{
						if (pi.Id == 0x0112)
						{
							Console.WriteLine(pi.Value[0]);
						}
					}
				}
			}
		}
	}
}
