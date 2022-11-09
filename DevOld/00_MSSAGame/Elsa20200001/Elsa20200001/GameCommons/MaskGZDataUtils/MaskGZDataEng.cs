using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons.MaskGZDataUtils
{
	public class MaskGZDataEng
	{
		private uint X;

		private void AvoidXIsZero()
		{
			this.X = this.X % 0xffffffffu + 1u;
		}

		private uint Rand()
		{
			// Xorshift-32

			this.X ^= this.X << 13;
			this.X ^= this.X >> 17;
			this.X ^= this.X << 5;

			return this.X;
		}

		private void Shuffle(int[] values)
		{
			for (int index = values.Length; 2 <= index; index--)
			{
				int a = index - 1;
				int b = (int)(this.Rand() % (uint)index);

				int tmp = values[a];
				values[a] = values[b];
				values[b] = tmp;
			}
		}

		private int GetMaskSize(int size)
		{
			return Math.Min(13, size / 2);
		}

		private void Mask(byte[] data)
		{
			int size = this.GetMaskSize(data.Length);

			for (int index = 0; index < size; index++)
			{
				data[index] ^= (byte)0xf5;
			}
		}

		private void Swap(byte[] data, int[] swapIdxLst)
		{
			for (int index = 0; index < swapIdxLst.Length; index++)
			{
				int a = index;
				int b = data.Length - swapIdxLst[index];

				byte tmp = data[a];
				data[a] = data[b];
				data[b] = tmp;
			}
		}

		private int GetTransposeSize(int size)
		{
			return Math.Min(307, size / 2);
		}

		private void Transpose(byte[] data, string seed)
		{
			int[] swapIdxLst = Enumerable.Range(1, this.GetTransposeSize(data.Length)).ToArray();

			this.X = (uint)data.Length;
			this.Rand();
			this.X ^= uint.Parse(seed);
			this.AvoidXIsZero();
			this.Shuffle(swapIdxLst);

			this.Mask(data);
			this.Swap(data, swapIdxLst);
			this.Mask(data);
		}

		public void Transpose(byte[] data)
		{
			this.Transpose(data, "2022032707"); // 難読化貢献のため seed を文字列化しておく
		}
	}
}
