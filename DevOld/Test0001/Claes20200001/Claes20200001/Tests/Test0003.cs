using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			string[] lLines = File.ReadAllLines(@"C:\temp\左側ファイル.txt");
			string[] rLines = File.ReadAllLines(@"C:\temp\右側ファイル.txt");

			int lNearly = 0;
			int rNearly = 0;

			for (int index = 0; index < lLines.Length; index++)
			{
				string[] lTokens = SCommon.Tokenize(lLines[index], " ", false, true);
				string[] rTokens = SCommon.Tokenize(rLines[index], " ", false, true);

				bool sinFlag = lTokens[0][0] == 'S';
				double degree = double.Parse(lTokens[1]);
				double lValue = double.Parse(lTokens[4]);
				double rValue = double.Parse(rTokens[4]);

				double mathClsValue = sinFlag ?
					Math.Sin(degree * (Math.PI / 180.0)) :
					Math.Cos(degree * (Math.PI / 180.0));

				double lDiff = Math.Abs(lValue - mathClsValue);
				double rDiff = Math.Abs(rValue - mathClsValue);

				if (lDiff < rDiff)
					lNearly++;

				if (rDiff < lDiff)
					rNearly++;

				Console.WriteLine(string.Format("{0:F20} {1:F20}", lDiff, rDiff));
			}
			Console.WriteLine(lNearly);
			Console.WriteLine(rNearly);
		}

		public void Test02()
		{
			for (int c = 0; c < 10; c++)
			{
				File.WriteAllText(ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02.txt")), "Test02.txt " + c, Encoding.ASCII);
				File.WriteAllText(ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02-Text")), "Test02-Text " + c, Encoding.ASCII);
				File.WriteAllText(ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02-Text.dat")), "Test02-Text.dat " + c, Encoding.ASCII);
				File.WriteAllText(ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02-Text.dat.txt")), "Test02-Text.dat.txt " + c, Encoding.ASCII);
			}
		}

		// memo: 連番の前の文字を '~' にしたところ Test02.txt -> Test02~1.txt , Test02~3.txt が存在しないのにスキップされる謎現象に遭遇。@ 2022.10.24
		// -- よくわからんので無難な文字列にしておく。

		public static string ToCreatablePath(string path)
		{
			string newPath = path;
			int n = 1;

			while (File.Exists(newPath) || Directory.Exists(newPath))
			{
				newPath = SCommon.EraseExt(path) + "-DUP-" + n + Path.GetExtension(path);
				n++;
			}
			return newPath;
		}

		public void Test03()
		{
			for (int c = 0; c < 10; c++)
			{
				File.WriteAllText(ToCreatablePath_v2(Path.Combine(SCommon.GetOutputDir(), "Test02.txt")), "Test02.txt " + c, Encoding.ASCII);
				File.WriteAllText(ToCreatablePath_v2(Path.Combine(SCommon.GetOutputDir(), "Test02-Text")), "Test02-Text " + c, Encoding.ASCII);
				File.WriteAllText(ToCreatablePath_v2(Path.Combine(SCommon.GetOutputDir(), "Test02-Text.dat")), "Test02-Text.dat " + c, Encoding.ASCII);
				File.WriteAllText(ToCreatablePath_v2(Path.Combine(SCommon.GetOutputDir(), "Test02-Text.dat.txt")), "Test02-Text.dat.txt " + c, Encoding.ASCII);
			}
		}

		private static long TCP_LastTime = -1L;

		public static string ToCreatablePath_v2(string path)
		{
			string newPath = path;

			while (File.Exists(newPath) || Directory.Exists(newPath))
			{
				if (TCP_LastTime == -1L)
				{
					long epoch = SCommon.TimeStampToSec.ToSec(19700101000000);
					long now = SCommon.SimpleDateTime.Now().ToSec();

					TCP_LastTime = (now - epoch) * 1000;
				}
				else
				{
					TCP_LastTime++;
				}

				newPath = SCommon.EraseExt(path) + "_" + TCP_LastTime + Path.GetExtension(path);
				//newPath = path + "_" + TCP_Count;
			}
			return newPath;
		}

		public void Test04()
		{
			for (int c = 0; c < 108; c++)
			{
				File.WriteAllText(SCommon.ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02.txt")), "Test02.txt " + c, Encoding.ASCII);
				File.WriteAllText(SCommon.ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02-Text")), "Test02-Text " + c, Encoding.ASCII);
				File.WriteAllText(SCommon.ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02-Text.dat")), "Test02-Text.dat " + c, Encoding.ASCII);
				File.WriteAllText(SCommon.ToCreatablePath(Path.Combine(SCommon.GetOutputDir(), "Test02-Text.dat.txt")), "Test02-Text.dat.txt " + c, Encoding.ASCII);
			}
		}
	}
}
