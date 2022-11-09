using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			Console.WriteLine(SCommon.EraseExt("Test02"));
			Console.WriteLine(SCommon.EraseExt("Test02.txt"));
			Console.WriteLine(SCommon.EraseExt(@".\Test02"));
			Console.WriteLine(SCommon.EraseExt(@".\Test02.txt"));
			Console.WriteLine(SCommon.EraseExt(@"SubDir\Test02"));
			Console.WriteLine(SCommon.EraseExt(@"SubDir\Test02.txt"));
			Console.WriteLine(SCommon.EraseExt(@"\TopDir\Test02"));
			Console.WriteLine(SCommon.EraseExt(@"\TopDir\Test02.txt"));
			Console.WriteLine(SCommon.EraseExt(@"C:\TopDir\Test02"));
			Console.WriteLine(SCommon.EraseExt(@"C:\TopDir\Test02.txt"));
			Console.WriteLine(SCommon.EraseExt(@"\TopDir\SubDir\Test02"));
			Console.WriteLine(SCommon.EraseExt(@"\TopDir\SubDir\Test02.txt"));
			Console.WriteLine(SCommon.EraseExt(@"C:\TopDir\SubDir\Test02"));
			Console.WriteLine(SCommon.EraseExt(@"C:\TopDir\SubDir\Test02.txt"));

			Console.WriteLine(Path.GetExtension("Test02"));
			Console.WriteLine(Path.GetExtension("Test02.txt"));
			Console.WriteLine(Path.GetExtension(@".\Test02"));
			Console.WriteLine(Path.GetExtension(@".\Test02.txt"));
			Console.WriteLine(Path.GetExtension(@"SubDir\Test02"));
			Console.WriteLine(Path.GetExtension(@"SubDir\Test02.txt"));
			Console.WriteLine(Path.GetExtension(@"\TopDir\Test02"));
			Console.WriteLine(Path.GetExtension(@"\TopDir\Test02.txt"));
			Console.WriteLine(Path.GetExtension(@"C:\TopDir\Test02"));
			Console.WriteLine(Path.GetExtension(@"C:\TopDir\Test02.txt"));
			Console.WriteLine(Path.GetExtension(@"\TopDir\SubDir\Test02"));
			Console.WriteLine(Path.GetExtension(@"\TopDir\SubDir\Test02.txt"));
			Console.WriteLine(Path.GetExtension(@"C:\TopDir\SubDir\Test02"));
			Console.WriteLine(Path.GetExtension(@"C:\TopDir\SubDir\Test02.txt"));
		}
	}
}
