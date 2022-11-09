using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Item : Enemy
	{
		private GameStatus.Inventory_e Inventory;

		public Enemy_Item(double x, double y, GameStatus.Inventory_e inventory)
			: base(x, y - 10.0, 0, 0, false)
		{
			this.Inventory = inventory;
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
					DDDraw.DrawCenter(Ground.I.Picture.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);

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
		private bool IsAlreadyCollected()
		{
			return Game.I.Status.InventoryFlags[this.Inventory];
		}

		/// <summary>
		/// このアイテムを取得した時の動作
		/// </summary>
		private void Collected()
		{
			Game.I.Status.InventoryFlags[this.Inventory] = true;
		}

		/// <summary>
		/// このアイテムの名前を返す。
		/// </summary>
		/// <returns>このアイテムの名前</returns>
		private string GetTitle()
		{
			return "アイテム番号-(" + (int)this.Inventory + ")";
		}
	}
}
