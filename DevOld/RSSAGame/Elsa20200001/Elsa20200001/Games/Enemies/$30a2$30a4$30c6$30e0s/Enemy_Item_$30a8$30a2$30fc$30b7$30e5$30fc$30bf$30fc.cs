using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Enemies.アイテムs
{
	public class Enemy_Item_エアーシューター : Enemy_Item
	{
		public Enemy_Item_エアーシューター(double x, double y)
			: base(x, y, 0)
		{ }

		protected override bool IsAlreadyCollected()
		{
			return Game.I.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_エアーシューター];
		}

		protected override void Collected()
		{
			Game.I.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_エアーシューター] = true;
		}

		protected override string GetTitle()
		{
			return "エアーシューター";
		}
	}
}
