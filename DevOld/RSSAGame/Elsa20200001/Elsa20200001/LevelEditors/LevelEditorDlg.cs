using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using Charlotte.Commons;
using Charlotte.Games.Tiles;
using Charlotte.Games.Enemies;
using Charlotte.Games;
using Charlotte.GameCommons;

namespace Charlotte.LevelEditors
{
	// memo:
	// xxxxxxxxxx // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
	// という行は xxxxxxxxxx の部分について難読化による単語の置き換えを行わない。(xxxxxxxxxx は任意の文字列)

	public partial class LevelEditorDlg : Form
	{
		#region ALT_F4 抑止

		public bool XPressed = false;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				this.XPressed = true;
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public LevelEditorDlg()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void LevelEditorDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void LevelEditorDlg_Shown(object sender, EventArgs e)
		{
			this.TileGroup_L.Items.Clear();
			this.TileGroup_R.Items.Clear();
			this.EnemyGroup.Items.Clear();

			foreach (LevelEditor.GroupInfo group in LevelEditor.TileGroups)
			{
				this.TileGroup_L.Items.Add(group.Name);
				this.TileGroup_R.Items.Add(group.Name);
			}
			foreach (LevelEditor.GroupInfo group in LevelEditor.EnemyGroups)
				this.EnemyGroup.Items.Add(group.Name);

			P_PostSetItems(this.TileGroup_L);
			P_PostSetItems(this.TileGroup_R);
			P_PostSetItems(this.EnemyGroup);

			this.SetMode(LevelEditor.Mode_e.TILE);

			this.TileEnemySw
				.Focus(); // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
		}

		private void P_PostSetItems(
			ComboBox // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				combo)
		{
			combo.SelectedIndex = 0;
			combo.MaxDropDownItems = Math.Min(combo.Items.Count, 100);
		}

		public string GetTile_L()
		{
			return TileCatalog.GetNames()[LevelEditor.TileGroups[this.TileGroup_L.SelectedIndex].Members[this.TileMember_L.SelectedIndex].Index];
		}

		public string GetTile_R()
		{
			return TileCatalog.GetNames()[LevelEditor.TileGroups[this.TileGroup_R.SelectedIndex].Members[this.TileMember_R.SelectedIndex].Index];
		}

		public string GetEnemy()
		{
			return EnemyCatalog.GetNames()[LevelEditor.EnemyGroups[this.EnemyGroup.SelectedIndex].Members[this.EnemyMember.SelectedIndex].Index];
		}

		public void SetTile_L(string tileName)
		{
			int index = SCommon.IndexOf(TileCatalog.GetNames(), name => name == tileName);

			if (index == -1)
				throw new DDError();

			for (int groupIndex = 0; groupIndex < LevelEditor.TileGroups.Count; groupIndex++)
			{
				for (int memberIndex = 0; memberIndex < LevelEditor.TileGroups[groupIndex].Members.Count; memberIndex++)
				{
					if (LevelEditor.TileGroups[groupIndex].Members[memberIndex].Index == index)
					{
						this.TileGroup_L.SelectedIndex = groupIndex;
						this.TileMember_L.SelectedIndex = memberIndex;
						return;
					}
				}
			}
			throw new DDError();
		}

		public void SetTile_R(string tileName)
		{
			int index = SCommon.IndexOf(TileCatalog.GetNames(), name => name == tileName);

			if (index == -1)
				throw new DDError();

			for (int groupIndex = 0; groupIndex < LevelEditor.TileGroups.Count; groupIndex++)
			{
				for (int memberIndex = 0; memberIndex < LevelEditor.TileGroups[groupIndex].Members.Count; memberIndex++)
				{
					if (LevelEditor.TileGroups[groupIndex].Members[memberIndex].Index == index)
					{
						this.TileGroup_R.SelectedIndex = groupIndex;
						this.TileMember_R.SelectedIndex = memberIndex;
						return;
					}
				}
			}
			throw new DDError();
		}

		public void SetEnemy(string enemyName)
		{
			int index = SCommon.IndexOf(EnemyCatalog.GetNames(), name => name == enemyName);

			if (index == -1)
				throw new DDError();

			for (int groupIndex = 0; groupIndex < LevelEditor.EnemyGroups.Count; groupIndex++)
			{
				for (int memberIndex = 0; memberIndex < LevelEditor.EnemyGroups[groupIndex].Members.Count; memberIndex++)
				{
					if (LevelEditor.EnemyGroups[groupIndex].Members[memberIndex].Index == index)
					{
						this.EnemyGroup.SelectedIndex = groupIndex;
						this.EnemyMember.SelectedIndex = memberIndex;
						return;
					}
				}
			}
			throw new DDError();
		}

		public bool IsShowTile()
		{
			return this.ShowTile.Checked;
		}

		public bool IsShowEnemy()
		{
			return this.ShowEnemy.Checked;
		}

		private const string TEXT_MODE_TILE = "タイル";
		private const string TEXT_MODE_ENEMY = "敵 / イベントオブジェクト";

		public LevelEditor.Mode_e GetMode()
		{
			return this.TileEnemySw.Text == TEXT_MODE_TILE ? LevelEditor.Mode_e.TILE : LevelEditor.Mode_e.ENEMY;
		}

		private void SetMode(LevelEditor.Mode_e mode)
		{
			Color COLOR_SELECTED = Color
				.Blue; // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			Color COLOR_UNSELECTED = Color
				.Gray; // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa

			if (mode == LevelEditor.Mode_e.TILE)
			{
				this.TileEnemySw.Text = TEXT_MODE_TILE;

				this.GroupTile
					.ForeColor = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						COLOR_SELECTED;
				this.GroupEnemy
					.ForeColor = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						COLOR_UNSELECTED;
			}
			else
			{
				this.TileEnemySw.Text = TEXT_MODE_ENEMY;

				this.GroupTile
					.ForeColor = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						COLOR_UNSELECTED;
				this.GroupEnemy
					.ForeColor = // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						COLOR_SELECTED;
			}
		}

		private void TileEnemySw_Click(object sender, EventArgs e)
		{
			if (this.TileEnemySw.Text == TEXT_MODE_TILE)
				this.SetMode(LevelEditor.Mode_e.ENEMY);
			else
				this.SetMode(LevelEditor.Mode_e.TILE);
		}

		private void TileClick(object sender, EventArgs e)
		{
			this.SetMode(LevelEditor.Mode_e.TILE);
		}

		private void EnemyClick(object sender, EventArgs e)
		{
			this.SetMode(LevelEditor.Mode_e.ENEMY);
		}

		private void GroupTile_Enter(object sender, EventArgs e)
		{
			// noop
		}

		private void TileGroup_L_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.TileMember_L.Items.Clear();

			foreach (LevelEditor.GroupInfo.MemberInfo member in LevelEditor.TileGroups[this.TileGroup_L.SelectedIndex].Members)
				this.TileMember_L.Items.Add(member.Name);

			P_PostSetItems(this.TileMember_L);
		}

		private void TileMember_L_SelectedIndexChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void TileGroup_R_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.TileMember_R.Items.Clear();

			foreach (LevelEditor.GroupInfo.MemberInfo member in LevelEditor.TileGroups[this.TileGroup_R.SelectedIndex].Members)
				this.TileMember_R.Items.Add(member.Name);

			P_PostSetItems(this.TileMember_R);
		}

		private void TileMember_R_SelectedIndexChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void GroupEnemy_Enter(object sender, EventArgs e)
		{
			// noop
		}

		private void EnemyGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.EnemyMember.Items.Clear();

			foreach (LevelEditor.GroupInfo.MemberInfo member in LevelEditor.EnemyGroups[this.EnemyGroup.SelectedIndex].Members)
				this.EnemyMember.Items.Add(member.Name);

			P_PostSetItems(this.EnemyMember);
		}

		private void EnemyMember_SelectedIndexChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void ShowTile_CheckedChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void ShowEnemy_CheckedChanged(object sender, EventArgs e)
		{
			// noop
		}
	}
}
