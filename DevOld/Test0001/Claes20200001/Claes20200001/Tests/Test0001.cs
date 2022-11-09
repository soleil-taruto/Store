using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			const string TEST_DIR = @"C:\temp\Test0001";

			SCommon.DeletePath(TEST_DIR);

			for (int testcnt = 0; testcnt < 10; testcnt++)
			{
				CreateTestDir(TEST_DIR, 0);
				DeleteDir_01(TEST_DIR);

				if (Directory.Exists(TEST_DIR))
					throw null;
			}
			for (int testcnt = 0; testcnt < 10; testcnt++)
			{
				CreateTestDir(TEST_DIR, 0);
				DeleteDir_02(TEST_DIR);

				if (Directory.Exists(TEST_DIR))
					throw null;
			}
		}

		private void CreateTestDir(string dir, int depth)
		{
			SCommon.CreateDir(dir);

			int dirnum = SCommon.CRandom.GetRange(0, depth < 5 ? 5 : 0);
			int filenum = SCommon.CRandom.GetRange(0, 7);

			for (int index = 0; index < dirnum; index++)
			{
				CreateTestDir(Path.Combine(dir, CreateTestName()), depth + 1);
			}
			for (int index = 0; index < filenum; index++)
			{
				File.WriteAllBytes(Path.Combine(dir, CreateTestName()), SCommon.EMPTY_BYTES);
			}
		}

		private string CreateTestName()
		{
			return Guid.NewGuid().ToString("B");
		}

		private void DeleteDir_01(string rootDir)
		{
			const char CMD_CHECK = 'C';
			const char CMD_DELETE = 'D';

			Stack<string> s = new Stack<string>();

			s.Push(CMD_CHECK + rootDir);

			while (1 <= s.Count)
			{
				string commandDir = s.Pop();
				char command = commandDir[0];
				string dir = commandDir.Substring(1);

				if (command == CMD_CHECK)
				{
					s.Push(CMD_DELETE + dir);

					foreach (string subDir in Directory.GetDirectories(dir))
					{
						s.Push(CMD_CHECK + subDir);
					}
					foreach (string file in Directory.GetFiles(dir))
					{
						File.Delete(file);
					}
				}
				else if (command == CMD_DELETE)
				{
					Directory.Delete(dir);
				}
				else
				{
					throw new Exception("Bad command");
				}
			}
		}

		private void DeleteDir_02(string rootDir)
		{
			List<string> list = new List<string>();

			list.Add(rootDir);

			for (int index = 0; index < list.Count; index++)
			{
				list.AddRange(Directory.GetDirectories(list[index]));
			}
			while (1 <= list.Count)
			{
				string dir = SCommon.UnaddElement(list);

				foreach (string file in Directory.GetFiles(dir))
				{
					File.Delete(file);
				}
				Directory.Delete(dir);
			}
		}
	}
}
