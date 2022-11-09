using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Novels.Surfaces;

namespace Charlotte.Novels
{
	/// <summary>
	/// ゲームの現在の状態を保持する。
	/// </summary>
	public class NovelStatus
	{
		public Scenario Scenario = new Scenario(NovelConsts.DUMMY_SCENARIO_NAME); // 軽量な仮設オブジェクト
		public int CurrPageIndex = 0;
		public List<Surface> Surfaces = new List<Surface>(); // 軽量な仮設オブジェクト

		// <---- prm

		// 特別なサーフェス >

		public bool HasSelect()
		{
			return this.Surfaces.Any(v => v is Surface_Select);
		}

		public Surface_Select GetSelect()
		{
			return (Surface_Select)this.Surfaces.First(v => v is Surface_Select);
		}

		public void RemoveSelect()
		{
			this.Surfaces.RemoveAll(v => v is Surface_Select);
		}

		// < 特別なサーフェス

		public int GetSurfaceIndex(string instanceName, int defval)
		{
			for (int index = 0; index < this.Surfaces.Count; index++)
				if (this.Surfaces[index].InstanceName == instanceName)
					return index;

			return defval;
		}

		public int GetSurfaceIndex(string instanceName)
		{
			int index = this.GetSurfaceIndex(instanceName, -1);

			if (index == -1)
				throw new DDError("存在しないインスタンス名：" + instanceName);

			return index;
		}

		public Surface GetSurface(string instanceName)
		{
			return this.Surfaces[this.GetSurfaceIndex(instanceName)];
		}
	}
}
