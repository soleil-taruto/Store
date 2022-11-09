using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utilities
{
	public class BitList
	{
		private const int INNER_LEN_MAX = (int)(((long)int.MaxValue + 1) / 4); // 2GB

		private List<uint> Inner = new List<uint>();

		public BitList()
		{ }

		public BitList(IEnumerable<bool> data)
		{
			long index = 0L;

			foreach (bool value in data)
			{
				this[index++] = value;
			}
		}

		public bool this[long index]
		{
			get
			{
				if (index < 0L)
					throw new ArgumentOutOfRangeException("Bad index: " + index);

				if (this.Inner.Count <= index / 32)
					return false;

				return (this.Inner[(int)(index / 32)] & (1u << (int)(index % 32))) != 0u;
			}

			set
			{
				if (index < 0L || (long)INNER_LEN_MAX * 32 <= index)
					throw new ArgumentOutOfRangeException("Bad index: " + index);

				while (this.Inner.Count <= index / 32)
					this.Inner.Add(0u);

				if (value)
					this.Inner[(int)(index / 32)] |= 1u << (int)(index % 32);
				else
					this.Inner[(int)(index / 32)] &= ~(1u << (int)(index % 32));
			}
		}

		public IEnumerable<bool> Iterate()
		{
			this.P_Trim();

			for (int index = 0; index < this.Inner.Count; index++)
			{
				for (int bit = 0; bit < 32; bit++)
				{
					yield return (this.Inner[index] & (1u << bit)) != 0u;
				}
			}
		}

		private void P_Trim()
		{
			while (1 <= this.Inner.Count && this.Inner[this.Inner.Count - 1] == 0u)
			{
				this.Inner.RemoveAt(this.Inner.Count - 1);
			}
		}
	}
}
