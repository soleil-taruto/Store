using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Tests
{
	public class Test0012
	{
		private const string TARGET_DIR = @"C:\temp";
		//private const string TARGET_DIR = @"C:\Dev";

		public void Test01()
		{
			foreach (string file in Directory.GetFiles(TARGET_DIR, "*.cs", SearchOption.AllDirectories))
			{
				string[] lines = File.ReadAllLines(file, Encoding.UTF8);

				for (int index = 1; index + 1 < lines.Length; index++)
				{
					if (lines[index].TrimStart().StartsWith("else"))
					{
						int m = 0;

						if (lines[index - 1].TrimStart().StartsWith("}")) m++;
						if (lines[index + 1].TrimStart().StartsWith("{")) m++;

						if (m == 1)
						{
							Console.WriteLine(file + " (" + (index + 1) + ")");
						}
					}
				}
			}
		}
	}
}
