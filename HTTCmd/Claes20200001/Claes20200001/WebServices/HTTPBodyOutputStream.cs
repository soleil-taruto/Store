using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.WebServices
{
	public class HTTPBodyOutputStream : IDisposable
	{
		private class InnerInfo : IDisposable
		{
			public WorkingDir WD = new WorkingDir();
			public string BufferFile;
			public int WroteSize = 0;
			public CtrCipher CtrCipher = CtrCipher.CreateTemporary();

			public InnerInfo()
			{
				this.BufferFile = this.WD.MakePath();
			}

			public void Write(byte[] data, int offset = 0)
			{
				this.Write(data, offset, data.Length - offset);
			}

			public void Write(byte[] data, int offset, int count)
			{
				this.CtrCipher.Mask(data, offset, count);

				using (FileStream writer = new FileStream(this.BufferFile, FileMode.Append, FileAccess.Write))
				{
					writer.Write(data, offset, count);
				}
				this.WroteSize += count;
			}

			public int Count
			{
				get
				{
					return this.WroteSize;
				}
			}

			public byte[] ToByteArray()
			{
				byte[] data = File.ReadAllBytes(this.BufferFile);
				SCommon.DeletePath(this.BufferFile);
				this.WroteSize = 0;

				this.CtrCipher.Reset();
				this.CtrCipher.Mask(data);
				this.CtrCipher.Reset();

				return data;
			}

			public void Dispose()
			{
				if (this.WD != null)
				{
					this.WD.Dispose();
					this.WD = null;

					this.CtrCipher.Dispose();
					this.CtrCipher = null;
				}
			}
		}

		private InnerInfo Inner = null;

		public void Write(byte[] data, int offset = 0)
		{
			this.Write(data, offset, data.Length - offset);
		}

		public void Write(byte[] data, int offset, int count)
		{
			if (this.Inner == null)
				this.Inner = new InnerInfo();

			this.Inner.Write(data, offset, count);
		}

		public int Count
		{
			get
			{
				return this.Inner == null ? 0 : this.Inner.WroteSize;
			}
		}

		public byte[] ToByteArray()
		{
			return this.Inner == null ? SCommon.EMPTY_BYTES : this.Inner.ToByteArray();
		}

		public void Dispose()
		{
			if (this.Inner != null)
			{
				this.Inner.Dispose();
				this.Inner = null;
			}
		}
	}
}
