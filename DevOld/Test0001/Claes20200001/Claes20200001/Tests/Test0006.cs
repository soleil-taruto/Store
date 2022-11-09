using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0006
	{
		public void Test01()
		{
			using (FileStream reader = new FileStream(@"C:\temp\1.txt", FileMode.Open, FileAccess.Read))
			using (FileStream writer = new FileStream(@"C:\temp\2.txt", FileMode.Create, FileAccess.Write))
			{
				SCommon.ReadToEnd(reader.Read, (buff, offset, count) =>
				{
					Console.WriteLine("Position: " + reader.Position); // cout

					for (int index = 0; index < count; index++)
					{
						byte chr = buff[offset + index];

						if (chr != 0x0d) // ? not CR
						{
							writer.WriteByte(chr);
						}
					}
				});
			}
		}

		public void Test02()
		{
			using (FileStream reader = new FileStream(@"C:\temp\1.txt", FileMode.Open, FileAccess.Read))
			using (FileStream writer = new FileStream(@"C:\temp\2.txt", FileMode.Create, FileAccess.Write))
			{
				SCommon.ReadToEnd(reader.Read, (buff, offset, count) =>
				{
					Console.WriteLine("Position: " + reader.Position); // cout

					// Erase CR
					{
						int w = 0;

						for (int r = 0; r < count; r++)
						{
							byte chr = buff[offset + r];

							if (chr != 0x0d) // ? not CR
							{
								buff[offset + w] = chr;
								w++;
							}
						}
						count = w;
					}

					writer.Write(buff, offset, count);
				});
			}
		}
	}
}
