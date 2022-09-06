using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Novels
{
	public class NovelAct
	{
		/// <summary>
		/// アクションリスト
		/// </summary>
		private List<Func<bool>> InnerActs = new List<Func<bool>>();

		public void Add(Func<bool> innerAct)
		{
			if (innerAct == null)
				throw new ArgumentException("innerAct == null");

			this.InnerActs.Add(innerAct);
		}

		public void AddOnce(Action innerAct)
		{
			this.Add(() =>
			{
				innerAct();
				return false;
			});
		}

		public void Flush()
		{
			IsFlush = true;
			bool ret = this.Draw();
			IsFlush = false; // restore

			if (ret)
				throw new DDError();
		}

		public int Count
		{
			get
			{
				return this.InnerActs.Count;
			}
		}

		/// <summary>
		/// アクション即完了フラグ
		/// </summary>
		public static bool IsFlush = false;

		/// <summary>
		/// このアクションを描画する。
		/// Flush == ture の場合、アクションは即完了しなければならない。
		/// 戻り値：
		/// 真を返したとき == アクションを描画し、継続する可能性がある。
		/// 偽を返したとき == 全てのアクションは終了しているため描画しなかった。本来の描画を実行する必要がある。
		/// </summary>
		/// <returns>アクション継続か</returns>
		public bool Draw()
		{
			while (1 <= this.InnerActs.Count && !this.InnerActs[0]())
				this.InnerActs.RemoveAt(0);

			return 1 <= this.InnerActs.Count;
		}
	}
}
