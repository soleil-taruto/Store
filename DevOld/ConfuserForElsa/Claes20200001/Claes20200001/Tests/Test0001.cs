using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			// -- choose one --

			Test01_a(@"C:\Dev\Template\Games\Dungeon\Elsa20200001\Elsa20200001.sln");
			//Test01_a(@"C:\Dev\Template\Games\MSSAGame\Elsa20200001\Elsa20200001.sln");
			//Test01_a(@"C:\Dev\Template\Games\RSSAGame\Elsa20200001\Elsa20200001.sln");
			//Test01_a(@"C:\Dev\Template\Games\ShootingGame\Elsa20200001\Elsa20200001.sln");
			//Test01_a(@"C:\Dev\Template\Games\SSAGame\Elsa20200001\Elsa20200001.sln");
			//Test01_a(@"C:\Dev\Template\Games\TSSGame\Elsa20200001\Elsa20200001.sln");
			//Test01_a(@"C:\Dev\Template\Games\TVAGame\Elsa20200001\Elsa20200001.sln");

			// --
		}

		private void Test01_a(string solutionFile)
		{
			ConfuserForElsa.Perform(solutionFile, @"C:\temp");
		}
	}
}
