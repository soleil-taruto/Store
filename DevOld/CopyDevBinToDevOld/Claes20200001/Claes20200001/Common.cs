using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static string LiteFormatDIG(string str)
		{
			string fmt = str;

			foreach (char chr in SCommon.DECIMAL)
				fmt = fmt.Replace(chr, '9');

			return fmt;
		}
	}
}
