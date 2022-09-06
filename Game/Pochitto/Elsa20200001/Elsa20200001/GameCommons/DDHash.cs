using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public struct DDHash
	{
		public readonly ulong Hi;
		public readonly ulong Lw;

		public DDHash(byte[] data)
		{
			// SHA-512_128

			byte[] hash = SCommon.GetSHA512(data);

			this.Hi =
				((ulong)hash[0] << 56) |
				((ulong)hash[1] << 48) |
				((ulong)hash[2] << 40) |
				((ulong)hash[3] << 32) |
				((ulong)hash[4] << 24) |
				((ulong)hash[5] << 16) |
				((ulong)hash[6] << 8) |
				((ulong)hash[7] << 0);

			this.Lw =
				((ulong)hash[8] << 56) |
				((ulong)hash[9] << 48) |
				((ulong)hash[10] << 40) |
				((ulong)hash[11] << 32) |
				((ulong)hash[12] << 24) |
				((ulong)hash[13] << 16) |
				((ulong)hash[14] << 8) |
				((ulong)hash[15] << 0);
		}

		public static bool operator ==(DDHash a, DDHash b)
		{
			return a.Hi == b.Hi && a.Lw == b.Lw;
		}

		public static bool operator !=(DDHash a, DDHash b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return (int)(uint)this.Lw;
		}

		public override bool Equals(object another)
		{
			return another is DDHash && this == (DDHash)another;
		}

		public override string ToString()
		{
			return string.Format("{0:x16}{1:x16}", this.Hi, this.Lw);
		}

		public class IEComp : IEqualityComparer<DDHash>
		{
			public bool Equals(DDHash a, DDHash b)
			{
				return a == b;
			}

			public int GetHashCode(DDHash a)
			{
				return a.GetHashCode();
			}
		}
	}
}
