using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Enemies.アイテムs
{
	public class Enemy_Item_跳ねる陰陽玉 : Enemy_Item
	{
		public Enemy_Item_跳ねる陰陽玉(double x, double y)
			: base(x, y, 3)
		{ }

		protected override bool IsAlreadyCollected()
		{
			return Game.I.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_跳ねる陰陽玉];
		}

		protected override void Collected()
		{
			Game.I.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_跳ねる陰陽玉] = true;
		}

		protected override string GetTitle()
		{
			return "跳ねる陰陽玉";
		}
	}
}
