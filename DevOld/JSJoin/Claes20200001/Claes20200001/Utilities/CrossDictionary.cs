using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Utilities
{
	public class CrossDictionary
	{
		private Dictionary<string, string> KeyToValue;
		private Dictionary<string, string> ValueToKey;
		private bool IgnoreCaseKey;
		private bool IgnoreCaseValue;

		public CrossDictionary(bool ignoreCase = false)
			: this(ignoreCase, ignoreCase)
		{ }

		public CrossDictionary(bool ignoreCaseKey, bool ignoreCaseValue)
		{
			this.KeyToValue = ignoreCaseKey ?
				SCommon.CreateDictionaryIgnoreCase<string>() :
				SCommon.CreateDictionary<string>();
			this.ValueToKey = ignoreCaseValue ?
				SCommon.CreateDictionaryIgnoreCase<string>() :
				SCommon.CreateDictionary<string>();
			this.IgnoreCaseKey = ignoreCaseKey;
			this.IgnoreCaseValue = ignoreCaseValue;
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

		public string GetKey(string value)
		{
			return this.ValueToKey[value];
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
