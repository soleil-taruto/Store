using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class GameCommon
	{
		private const string STR_NULL = "<NULL>";
		private const string STR_NOT_NULL_PREFIX = "<NOT-NULL>:";

		public static string WrapNullOrString(string str)
		{
			return str == null ? STR_NULL : STR_NOT_NULL_PREFIX + str;
		}

		public static string UnwrapNullOrString(string str)
		{
			if (str == null)
				throw new DDError();

			if (str == STR_NULL)
				return null;

			if (str.StartsWith(STR_NOT_NULL_PREFIX))
				return str.Substring(STR_NOT_NULL_PREFIX.Length);

			throw new DDError();
		}
	}
}
