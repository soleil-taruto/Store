using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.アイテムs
{
	public abstract class Enemy_Item : Enemy
	{
		private int CrystalColorIndex; // 0 - 4 == { 青, 赤, 緑, 黄色, 白黒 }

		public Enemy_Item(double x, double y, int crystalColorIndex)
			: base(x, y - 10.0, 0, 0, false)
		{
			this.CrystalColorIndex = crystalColorIndex;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			if (this.IsAlreadyCollected())
				yield break;

			for (; ; )
			{
				if (DDUtils.GetDistance(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y)) < 30.0) // ? 十分に接近 -> 取得する。
				{
					this.Collected();
					break;
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
				{
					DDDraw.SetMosaic();
					DDDraw.DrawBegin(Ground.I.Picture2.Crystals[this.CrystalColorIndex][DDEngine.ProcFrame / 5 % 3, 2], this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
					DDDraw.DrawZoom(2.0);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					DDPrint.SetDebug((int)this.X - DDGround.ICamera.X, (int)this.Y - DDGround.ICamera.Y);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.PrintLine("[" + this.GetTitle() + "]");
					DDPrint.Reset();

					// 当たり判定無し
				}
				yield return true;
			}
		}

		/// <summary>
		/// このアイテムは取得済みかどうか
		/// </summary>
		/// <returns>このアイテムは取得済み</returns>
		protected abstract bool IsAlreadyCollected();

		/// <summary>
		/// このアイテムを取得した時の動作
		/// </summary>
		protected abstract void Collected();

		/// <summary>
		/// このアイテムの名前を返す。
		/// </summary>
		/// <returns>このアイテムの名前</returns>
		protected abstract string GetTitle();
	}
}
