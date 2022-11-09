using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0005
	{
		public void Test01()
		{
			// h / 1.3 と計算しても (h * 10) / 13 と誤差は出ない。

			// 1.3 が 1.0100110011...(2) なので気になった。

			// 切り捨てによって割る数が 1.3 より小さくなる。-- 割り切れるとき端数が生じる方へ倒れる。
			// 1 / 13 ~= 0.0769 なので、端数は 0.0769 ～ 0.923 程度に収まる。
			// -> なので問題無いはず

			for (int h = 0; h <= 2100000000; h++)
			{
				// cout
				if (h % 100000000 == 0)
					Console.WriteLine(h);

				int a = (int)(h / 1.3);
				int b = (int)(((long)h * 10) / 13);

				if (a != b)
					throw null;
			}
		}

		public void Test02()
		{
			double a = 2.5;
			double b = -2.5;

			Console.WriteLine((int)a); // 2
			Console.WriteLine((int)b); // -2

			Console.WriteLine((long)a); // 2
			Console.WriteLine((long)b); // -2

			a = 0.5;
			b = -0.5;

			Console.WriteLine((int)a); // 0
			Console.WriteLine((int)b); // 0

			// 整数へのキャストはゼロ方向へ丸める。
		}
	}
}
