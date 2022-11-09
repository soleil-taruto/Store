using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	/// <summary>
	/// データサイズ_小
	/// </summary>
	public class Test0002
	{
		public void Test01()
		{
			// GetSHA512(byte[] src)
			// GetSHA512(IEnumerable<byte[]> src)
			// GetSHA512(Action<Write_d> execute) ...をテスト_1
			//
			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("1_testcnt: " + testcnt); // cout

				byte[] testData = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));

				byte[] hash1 = SCommon.GetSHA512(testData);
				byte[] hash2 = SCommon.GetSHA512(new byte[][] { testData });
				byte[] hash3 = SCommon.GetSHA512(writePart => writePart(testData, 0, testData.Length));

				if (SCommon.Comp(hash1, hash2) != 0) // ? 不一致
					throw null; // 想定外

				if (SCommon.Comp(hash1, hash3) != 0) // ? 不一致
					throw null; // 想定外
			}

			// GetSHA512(byte[] src)
			// GetSHA512(IEnumerable<byte[]> src)
			// GetSHA512(Action<Write_d> execute) ...をテスト_2
			//
			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("2_testcnt: " + testcnt); // cout

				byte[][] testData = Enumerable.Range(0, SCommon.CRandom.GetInt(10))
					.Select(dummy => SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(100)))
					.ToArray();

				byte[] hash1 = SCommon.GetSHA512(SCommon.Join(testData));
				byte[] hash2 = SCommon.GetSHA512(testData);
				byte[] hash3 = SCommon.GetSHA512(writePart =>
				{
					foreach (byte[] part in testData)
						writePart(part, 0, part.Length);
				});

				if (SCommon.Comp(hash1, hash2) != 0) // ? 不一致
					throw null; // 想定外

				if (SCommon.Comp(hash1, hash3) != 0) // ? 不一致
					throw null; // 想定外
			}

			// GetSHA512(byte[] src)
			// GetSHA512(Read_d reader) ...をテスト
			//
			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("3_testcnt: " + testcnt); // cout

				byte[] testData = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));

				byte[] hash1 = SCommon.GetSHA512(testData);
				byte[] hash2;

				using (MemoryStream mem = new MemoryStream(testData))
				{
					hash2 = SCommon.GetSHA512(mem.Read);
				}

				if (SCommon.Comp(hash1, hash2) != 0) // ? 不一致
					throw null; // 想定外
			}

			// GetSHA512(byte[] src)
			// GetSHA512File(string file) ...をテスト
			//
			using (WorkingDir wd = new WorkingDir())
			{
				for (int testcnt = 0; testcnt < 10000; testcnt++)
				{
					if (testcnt % 100 == 0) Console.WriteLine("4_testcnt: " + testcnt); // cout

					byte[] testData = SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(1000));
					string testFile = wd.MakePath();
					File.WriteAllBytes(testFile, testData);

					byte[] hash1 = SCommon.GetSHA512(testData);
					byte[] hash2 = SCommon.GetSHA512File(testFile);

					if (SCommon.Comp(hash1, hash2) != 0) // ? 不一致
						throw null; // 想定外
				}
			}

			Console.WriteLine("OK!");
		}
	}
}
