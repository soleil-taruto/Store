using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Tests
{
	public class Test0002
	{
		//private const string ROOT_DIR = @"C:\Dev";
		private const string ROOT_DIR = @"C:\temp";

		public void Test01()
		{
			foreach (string file in Directory.GetFiles(ROOT_DIR, "*", SearchOption.AllDirectories))
			{
				string ext = Path.GetExtension(file).ToLower();

				if (ext == ".cs")
				{
					string[] lines = File.ReadAllLines(file, Encoding.UTF8);

					for (int index = 1; index + 1 < lines.Length; index++)
					{
						string trLine = lines[index].Trim();

						if (
							trLine == "else" ||
							trLine.StartsWith("else ") ||
							trLine.StartsWith("else\t")
							)
						{
							int match = 0;

							trLine = lines[index - 1].Trim();
							if (
								trLine == "}" ||
								trLine.StartsWith("} ") ||
								trLine.StartsWith("}\t")
								)
								match++;

							trLine = lines[index + 1].Trim();
							if (
								trLine == "{" ||
								trLine.StartsWith("{ ") ||
								trLine.StartsWith("{\t")
								)
								match++;

							if (match == 1)
							{
								Console.WriteLine(file + " (" + (index + 1) + ")");
							}
						}
					}
				}
			}
		}
	}
}
