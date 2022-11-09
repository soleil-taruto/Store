using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0009
	{
		public void Test01()
		{
			Main();
		}

		public void Main()
		{
			for (int n = 1; ; n++)
			{
				Console.WriteLine(n + " 段の場合 " + (GetWinner(Enumerable.Range(1, n).ToArray()) == Player_e.Me ? "先手必勝" : "後手必勝") + " です。");
			}
		}

		private enum Player_e
		{
			Me,
			Another,
		}

		private Player_e GetWinner(IList<int> rows) // rows: 各行の(連続する)棒の本数 , ret: 自分(Player_e.Me)を手番とした場合のこの局面における勝者
		{
			if (rows.Count == 0) // 直前に相手が消した棒が最後なら、相手の負け。
			{
				return Player_e.Me; // つまり勝者は自分
			}

			for (int i = 0; i < rows.Count; i++) // 全ての行について
			{
				int row = rows[i];

				for (int s = 0; s < row; s++) // 消し始めの位置
				{
					for (int c = 1; s + c <= row; c++) // 消す本数
					{
						int l = s; // 分断された行の左側
						int r = row - (s + c); // 分断された行の右側

						List<int> next = new List<int>(); // 次の局面を作る。

						next.AddRange(rows.Take(i)); // 消した行の前側
						next.AddRange(rows.Skip(i + 1)); // 消した行の後側

						// 分断された部分を独立した行と見なして追加する。
						if (l != 0) next.Add(l);
						if (r != 0) next.Add(r);

						if (GetWinner(next) == Player_e.Another) // 次の局面以降において自分の勝ちが確定するなら ...
						{
							return Player_e.Me; // ... 勝者は自分
						}
					}
				}
			}

			return Player_e.Another; // 勝ち目が無ければ、勝者は相手側
		}
	}
}
