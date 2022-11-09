using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Enemies.アイテムs
{
	public class Enemy_Item_マグネットエアー : Enemy_Item
	{
		public Enemy_Item_マグネットエアー(double x, double y)
			: base(x, y, 2)
		{ }

		protected override bool IsAlreadyCollected()
		{
			return Game.I.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_マグネットエアー];
		}

		protected override void Collected()
		{
			Game.I.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_マグネットエアー] = true;
		}

		protected override string GetTitle()
		{
			return "マグネットエアー";
		}
	}
}
