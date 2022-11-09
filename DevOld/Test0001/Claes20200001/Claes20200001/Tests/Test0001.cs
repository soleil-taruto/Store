using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 1, 2, 3, 4 }, 2, new int[] { 9, 8, 7 }))); // 1, 2, 9, 8, 7, 3, 4
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 1, 2, 3 }, 0, new int[] { 999 }))); // 999, 1, 2, 3
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 1, 2, 3 }, 3, new int[] { 999 }))); // 1, 2, 3, 999
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 1, 2, 3 }, 0, new int[] { 7, 8, 9 }))); // 7, 8, 9, 1, 2, 3
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 1, 2, 3 }, 3, new int[] { 7, 8, 9 }))); // 1, 2, 3, 7, 8, 9
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { }, 0, new int[] { 99, 98, 97 }))); // 99, 98, 97
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 99, 98, 97 }, 0, new int[] { }))); // 99, 98, 97
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 99, 98, 97 }, 3, new int[] { }))); // 99, 98, 97
			Console.WriteLine(string.Join(", ",
				SCommon.InsertRange(new int[] { 1, 2, 3, 4 }, 2, new int[] { }))); // 1, 2, 3, 4

			// ----

			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 1, 2))); // 1, 4
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 0, 4))); // 空
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { }, 0, 0))); // 空
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 0, 3))); // 4
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 1, 3))); // 1
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 0, 0))); // 1, 2, 3, 4
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 1, 0))); // 1, 2, 3, 4
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 2, 0))); // 1, 2, 3, 4
			Console.WriteLine(string.Join(", ",
				SCommon.RemoveRange(new int[] { 1, 2, 3, 4 }, 3, 0))); // 1, 2, 3, 4

			// ----

			SCommon.ToThrowPrint(() =>
				SCommon.InsertRange((int[])null, 0, null));
			SCommon.ToThrowPrint(() =>
				SCommon.InsertRange(null, 0, new int[] { 9, 8, 7 }));
			SCommon.ToThrowPrint(() =>
				SCommon.InsertRange(new int[] { 1, 2, 3 }, 0, null));
			SCommon.ToThrowPrint(() =>
				SCommon.InsertRange(new int[] { 1, 2, 3 }, -1, new int[] { 9, 8, 7 }));
			SCommon.ToThrowPrint(() =>
				SCommon.InsertRange(new int[] { 1, 2, 3 }, 4, new int[] { 9, 8, 7 }));

			// ----

			SCommon.ToThrowPrint(() =>
				SCommon.RemoveRange((int[])null, 0, 0));
			SCommon.ToThrowPrint(() =>
				SCommon.RemoveRange(new int[] { 1, 2, 3 }, 0, 4));
			SCommon.ToThrowPrint(() =>
				SCommon.RemoveRange(new int[] { 1, 2, 3 }, 4, 0));
			SCommon.ToThrowPrint(() =>
				SCommon.RemoveRange(new int[] { 1, 2, 3 }, -1, 0));
			SCommon.ToThrowPrint(() =>
				SCommon.RemoveRange(new int[] { 1, 2, 3 }, 0, -1));

			// ----

			Console.WriteLine("OK!");
		}
	}
}
