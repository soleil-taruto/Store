using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class WorldGameMaster : IDisposable
	{
		public World World;
		public GameStatus Status;

		// <---- prm

		public Game.EndReason_e EndReason = Game.EndReason_e.ReturnToTitleMenu;

		// <---- ret

		public static WorldGameMaster I;

		public WorldGameMaster()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			for (; ; )
			{
				using (new Game())
				{
					Game.I.World = this.World;
					Game.I.Status = this.Status;
					Game.I.Perform();

					this.EndReason = Game.I.EndReason;

					switch (this.Status.ExitDirection)
					{
						case 5:
							goto endLoop;

						case 4:
							this.World.Move(-1, 0);
							this.Status.StartPointDirection = 6;
							break;

						case 6:
							this.World.Move(1, 0);
							this.Status.StartPointDirection = 4;
							break;

						case 8:
							this.World.Move(0, -1);
							this.Status.StartPointDirection = 2;
							break;

						case 2:
							this.World.Move(0, 1);
							this.Status.StartPointDirection = 8;
							break;

						default:
							throw null; // never
					}
				}
			}
		endLoop:
			;
		}
	}
}
