using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.Tests
{
	public class Enemy_Bアイテム : Enemy
	{
		public enum 効用_e
		{
			WEAPON_B_NORMAL,
			WEAPON_B_FIRE_BALL,
			WEAPON_B_LASER,
			WEAPON_B_WAVE_BEAM,
		}

		public static string[] 効用_e_Names = new string[]
		{
			"ノーマル",
			"ファイアボール",
			"レーザー",
			"波動ビーム",
		};

		private 効用_e 効用;

		public Enemy_Bアイテム(double x, double y, 効用_e 効用)
			: base(x, y, 0, 0, false)
		{
			this.効用 = 効用;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				if (DDUtils.GetDistanceLessThan(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y), 30.0)) // 当たり判定
				{
					this.プレイヤーがアイテムを取得した();
					break;
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では描画しない。
				{
					DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
					DDDraw.DrawRotate(DDEngine.ProcFrame / 10.0);
					DDDraw.DrawEnd();

					DDPrint.SetDebug((int)this.X - DDGround.Camera.X, (int)this.Y - DDGround.Camera.Y);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.PrintLine("アイテム");
					DDPrint.PrintLine("効用：" + 効用_e_Names[(int)this.効用]);
					DDPrint.Reset();

					// アイテム系につき this.Crash への当たり判定セット無し
				}
				yield return true;
			}
		}

		private void プレイヤーがアイテムを取得した()
		{
			switch (this.効用)
			{
				case 効用_e.WEAPON_B_NORMAL:
					Game.I.Player.武器 = ShotCatalog.武器_e.B_NORMAL;
					break;

				case 効用_e.WEAPON_B_FIRE_BALL:
					Game.I.Player.武器 = ShotCatalog.武器_e.B_FIRE_BALL;
					break;

				case 効用_e.WEAPON_B_LASER:
					Game.I.Player.武器 = ShotCatalog.武器_e.B_LASER;
					break;

				case 効用_e.WEAPON_B_WAVE_BEAM:
					Game.I.Player.武器 = ShotCatalog.武器_e.B_WAVE_BEAM;
					break;

				default:
					throw null; // never
			}
		}
	}
}
