using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Utilities
{
	public class UniqueIssuer
	{
		private Func<string> Generator;
		private bool IgnoreCase;
		private HashSet<string> KnownValueSet;

		public UniqueIssuer(Func<string> generator, bool ignoreCase = false)
		{
			this.Generator = generator;
			this.IgnoreCase = ignoreCase;
			this.KnownValueSet = ignoreCase ?
				SCommon.CreateSetIgnoreCase() :
				SCommon.CreateSet();
		}

		/// <summary>
		/// 初回でユニークな値を引き当てられなかった傾向・圧力
		/// 生成可能な値を半分以上使い果たすと上昇する。
		/// </summary>
		private int FirstCollisionStress = 0;

		public string Next()
		{
			for (int retrycnt = 0; retrycnt < 1000; retrycnt++) // rough limit
			{
				string value = this.Generator();

				if (!this.KnownValueSet.Contains(value))
				{
					if (1 <= retrycnt)
					{
						this.FirstCollisionStress++;

						// rough limit
						if (1000 < this.FirstCollisionStress)
							throw new Exception("生成可能な値を半分以上使い果たした可能性があります。");
					}
					else
					{
						this.FirstCollisionStress = Math.Max(0, this.FirstCollisionStress - 1);
					}
					this.KnownValueSet.Add(value);
					return value;
				}
			}
			throw new Exception("生成可能な値を殆ど使い果たした可能性があります。");
		}
	}
}
