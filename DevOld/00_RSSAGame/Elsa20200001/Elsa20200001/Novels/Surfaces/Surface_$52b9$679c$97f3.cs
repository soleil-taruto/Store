using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_効果音 : Surface
	{
		public Surface_効果音(string typeName, string instanceName)
			: base(typeName, instanceName)
		{ }

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// noop

				yield return true;
			}
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "再生")
			{
				this.Act.Add(() =>
				{
					DDCCResource.GetSE(arguments[c++]).Play();
					return false;
				});
			}
			else
			{
				throw new DDError();
			}
		}
	}
}
