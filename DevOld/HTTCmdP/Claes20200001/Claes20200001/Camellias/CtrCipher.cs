using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Camellias
{
	public class CtrCipher : IDisposable
	{
		public static CtrCipher CreateTemporary()
		{
			return new CtrCipher(SCommon.CRandom.GetBytes(32));
		}

		private Camellia Camellia;
		private byte[] Counter = new byte[16];
		private byte[] Buffer = new byte[16];
		private int Index;

		public CtrCipher(byte[] rawKey)
		{
			this.Camellia = new Camellia(rawKey);
			this.Reset();
		}

		public void Reset()
		{
			Array.Clear(this.Counter, 0, 16);
			this.Index = 16;
		}

		public byte Next()
		{
			if (this.Index == 16)
			{
				this.Camellia.EncryptBlock(this.Counter, this.Buffer);
				this.Increment();
				this.Index = 0;
			}
			return this.Buffer[this.Index++];
		}

		private void Increment()
		{
			for (int index = 0; ; index++)
			{
				if (this.Counter[index] < 255)
				{
					this.Counter[index]++;
					break;
				}
				this.Counter[index] = 0;
			}
		}

		public void Mask(byte[] data, int offset = 0)
		{
			this.Mask(data, offset, data.Length - offset);
		}

		public void Mask(byte[] data, int offset, int count)
		{
			for (int index = 0; index < count; index++)
			{
				data[offset + index] ^= this.Next();
			}
		}

		public void Dispose()
		{
			if (this.Camellia != null)
			{
				this.Camellia.Dispose();
				this.Camellia = null;
			}
		}
	}
}
