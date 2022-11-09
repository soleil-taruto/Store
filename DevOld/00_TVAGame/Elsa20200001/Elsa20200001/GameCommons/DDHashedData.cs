using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public class DDHashedData
	{
		public DDHash Hash;
		public byte[] Entity;

		public DDHashedData(byte[] data)
		{
			this.Hash = new DDHash(data);
			this.Entity = data;
		}
	}
}
