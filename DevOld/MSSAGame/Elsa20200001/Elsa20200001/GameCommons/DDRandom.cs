using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	/// <summary>
	/// 擬似乱数列
	/// </summary>
	public class DDRandom
	{
		private ulong X;

		public DDRandom()
			: this(SCommon.CRandom.GetUInt())
		{ }

		public DDRandom(uint seed)
		{
			this.X = (ulong)seed;
		}

		private ulong Next()
		{
			return this.X = 1103515245 * (ulong)(uint)this.X + 12345;
		}

		/// <summary>
		/// 32ビットの乱数を返す。
		/// </summary>
		/// <returns>乱数</returns>
		public uint GetUInt()
		{
			ulong uu = this.Next();
			uint ret = (uint)(uu % 4294967311ul); // 2^32 より大きい最小の素数
			return ret;
		}

		/// <summary>
		/// 0以上modulo未満の乱数を返す。
		/// </summary>
		/// <param name="modulo">上限値(1～)</param>
		/// <returns>乱数</returns>
		public uint GetUInt_M(uint modulo)
		{
			return this.GetUInt() % modulo;
		}

		/// <summary>
		/// 0以上modulo未満の乱数を返す。
		/// </summary>
		/// <param name="modulo">上限値(1～)</param>
		/// <returns>乱数</returns>
		public int GetInt(int modulo)
		{
			return (int)this.GetUInt_M((uint)modulo);
		}

		/// <summary>
		/// 0以上1以下の乱数を返す。
		/// </summary>
		/// <returns>乱数</returns>
		public double Single()
		{
			return (double)this.GetUInt() / uint.MaxValue;
		}

		/// <summary>
		/// -1以上1以下の乱数を返す。
		/// </summary>
		/// <returns>乱数</returns>
		public double Double()
		{
			return this.Single() * 2.0 - 1.0;
		}

		public int GetRange(int minval, int maxval)
		{
			return this.GetInt(maxval - minval + 1) + minval;
		}

		public void Shuffle<T>(IList<T> list)
		{
			for (int index = list.Count; 1 < index; index--)
			{
				SCommon.Swap(list, this.GetInt(index), index - 1);
			}
		}

		public T ChooseOne<T>(IList<T> list)
		{
			return list[this.GetInt(list.Count)];
		}
	}
}
