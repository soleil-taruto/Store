using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte
{
	public class Ground
	{
		public static Ground I;

		public List<ReceiptInfo> ReceiptInfos = new List<ReceiptInfo>();

		public 入力補助TBLB Refine支払先;
		public 入力補助TBLB Refine但し書き;
		public Known但し書き Known但し書き;
	}
}
