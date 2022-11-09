using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.JSSources
{
	public class JSToken
	{
		public enum Kind_e
		{
			SYMBOL,
			WORD,
		}

		public Kind_e Kind;
		public string Text;
		public int ArrayDepth;
		public int Index;

		// <---- prm
	}
}
