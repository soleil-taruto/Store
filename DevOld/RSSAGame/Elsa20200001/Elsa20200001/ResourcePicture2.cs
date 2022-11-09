using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourcePicture2
	{
		//public DDPicture[] Dummy = DDDerivations.GetAnimation(Ground.I.Picture.Dummy, 0, 0, 25, 25, 2, 2).ToArray();

		public DDPicture 陰陽玉 = DDDerivations.GetPicture(Ground.I.Picture.陰陽玉, 58, 39, 451, 451);
		public DDPicture[] Laser = DDDerivations.GetAnimation_XY(Ground.I.Picture.Laser, 0, 0, 20, 256, 7, 1).ToArray();

		public DDPicture[][,] Crystals = Ground.I.Picture.Crystals
			.Select(crystal => DDDerivations.GetAnimation(crystal, 0, 0, 32, 32, 3, 4))
			.ToArray();
	}
}
