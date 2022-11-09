using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utilities
{
	public class UniqueFilter<T>
	{
		private Func<T> Generator;

		public UniqueFilter(Func<T> generator)
		{
			this.Generator = generator;
		}

		private HashSet<T> KnownValues = new HashSet<T>();

		public T Next()
		{
			for (int c = 0; c < 1000; c++)
			{
				T value = this.Generator();

				if (!this.KnownValues.Contains(value))
				{
					this.KnownValues.Add(value);
					return value;
				}
			}
			throw new Exception("生成可能な値をほぼ使い果たしました。");
		}
	}
}
