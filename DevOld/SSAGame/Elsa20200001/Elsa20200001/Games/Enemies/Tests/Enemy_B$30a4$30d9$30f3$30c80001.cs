using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Novels;

namespace Charlotte.Games.Enemies.Tests
{
	public class Enemy_Bイベント0001 : Enemy
	{
		public Enemy_Bイベント0001(double x, double y)
			: base(x, y, 0, 0, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				if (DDUtils.GetDistance(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y)) < 30.0)
				{
					this.イベント実行();
					break;
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
				{
					DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
					DDDraw.DrawRotate(DDEngine.ProcFrame / 100.0);
					DDDraw.DrawEnd();

					DDPrint.SetDebug((int)this.X - DDGround.ICamera.X, (int)this.Y - DDGround.ICamera.Y);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.PrintLine("イベント0001");
					DDPrint.Reset();

					// 当たり判定無し
				}
				yield return true;
			}
		}

		private static DDSubScreen _lastGameScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		private void イベント実行()
		{
			DDMain.KeepMainScreen();

			// swap
			{
				DDSubScreen tmp = _lastGameScreen;
				_lastGameScreen = DDGround.KeptMainScreen;
				DDGround.KeptMainScreen = tmp;
			}

			DDMusicUtils.Fade();

			DDCurtain.SetCurtain(10, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				DDDraw.DrawSimple(_lastGameScreen.ToPicture(), 0, 0);
				DDEngine.EachFrame();
			}

			DDCurtain.SetCurtain(10);

			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("Tests/テスト0001");
				//Novel.I.Status.Scenario = new Scenario("Tests/イベント0001"); // old
				Novel.I.Perform();

				if (Novel.I.ReturnToTitleMenu)
					Game.I.RequestReturnToTitleMenu = true;
			}

			MusicCollection.Get(Game.I.Map.MusicName).Play();

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain(10);

#if false // いらん
			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				DDDraw.DrawSimple(_lastGameScreen.ToPicture(), 0, 0);
				DDEngine.EachFrame();
			}
#endif
		}
	}
}
