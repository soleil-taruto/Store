using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Utilities
{
	public class CrossStringDictionary
	{
		private Dictionary<string, string> KeyToValue;
		private Dictionary<string, string> ValueToKey;

		public CrossStringDictionary()
		{
			this.KeyToValue = SCommon.CreateDictionary<string>();
			this.ValueToKey = SCommon.CreateDictionary<string>();
		}

		public void Add(string key, string value)
		{
			this.KeyToValue.Add(key, value);
			this.ValueToKey.Add(value, key);
		}

		public string this[string key]
		{
			get
			{
				return this.KeyToValue[key];
			}
		}

		public bool ContainsKey(string key)
		{
			return this.KeyToValue.ContainsKey(key);
		}

		public bool ContainsValue(string value)
		{
			return this.ValueToKey.ContainsKey(value);
		}
	}
}
