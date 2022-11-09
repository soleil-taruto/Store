using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	/// <summary>
	/// BitList.cs テスト
	/// </summary>
	public class Test0007
	{
		public void Test01()
		{
			BitList bits = new BitList();

			Common.ToThrowPrint(() =>
			{
				bits[-1L].ToString();
			});

			bits[long.MaxValue].ToString(); // 未定義領域は false を返す。

			Common.ToThrowPrint(() =>
			{
				bits[-1L] = true;
			});

			Common.ToThrowPrint(() =>
			{
				bits[long.MaxValue] = true;
			});
		}
	}
}
