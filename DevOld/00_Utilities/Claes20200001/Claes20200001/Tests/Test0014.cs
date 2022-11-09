using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	/// <summary>
	/// Canvas.cs テスト
	/// </summary>
	public class Test0014
	{
		public void Test01()
		{
			Canvas canvas = new Canvas(100, 100);
			canvas.Fill(new I4Color(255, 128, 0, 255));
			canvas.Save(SCommon.NextOutputPath() + ".png");
		}
	}
}
