using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Novels;

namespace Charlotte.Tests.Novels
{
	public class NovelTest
	{
		public void Test01()
		{
			using (new Novel())
			{
				Novel.I.Perform();
			}
		}

		public void Test02()
		{
			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("Tests/テスト0001");
				Novel.I.Perform();
			}
		}

		public void Test03()
		{
			string name;

			// ---- choose one ----

			//name = "Tests/テスト0001";
			name = "Start";

			// ----

			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario(name);
				Novel.I.Perform();
			}
		}
	}
}
