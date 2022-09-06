using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games;
using Charlotte.Games.Enemies;
using Charlotte.Games.Tiles;

namespace Charlotte.LevelEditors
{
	/// <summary>
	/// 編集モードに関する機能
	/// </summary>
	public static class LevelEditor
	{
		public enum Mode_e
		{
			TILE,
			ENEMY,
		}

		public static LevelEditorDlg Dlg = null;

		public static void ShowDialog()
		{
			if (Dlg != null)
				throw null; // never

			Dlg = new LevelEditorDlg();
			Dlg.Show();
		}

		public static void CloseDialog()
		{
			Dlg.Close();
			Dlg.Dispose();
			Dlg = null;
		}

		public static void DrawEnemy()
		{
			int cam_l = DDGround.Camera.X;
			int cam_t = DDGround.Camera.Y;
			int cam_r = cam_l + DDConsts.Screen_W;
			int cam_b = cam_t + DDConsts.Screen_H;

			I2Point lt = GameCommon.ToTablePoint(cam_l, cam_t);
			I2Point rb = GameCommon.ToTablePoint(cam_r, cam_b);

			for (int x = lt.X; x <= rb.X; x++)
			{
				for (int y = lt.Y; y <= rb.Y; y++)
				{
					MapCell cell = Game.I.Map.GetCell(x, y);

					if (cell.EnemyName != GameConsts.ENEMY_NONE)
					{
						int tileL = x * GameConsts.TILE_W;
						int tileT = y * GameConsts.TILE_H;

						DDDraw.SetAlpha(0.3);
						DDDraw.SetBright(new I3Color(0, 128, 255));
						DDDraw.DrawRect(
							Ground.I.Picture.WhiteBox,
							tileL - cam_l,
							tileT - cam_t,
							GameConsts.TILE_W,
							GameConsts.TILE_H
							);
						DDDraw.Reset();

						DDPrint.SetBorder(new I3Color(0, 128, 255));
						DDPrint.SetDebug(tileL - cam_l, tileT - cam_t);
						DDPrint.Print(cell.EnemyName);
						DDPrint.Reset();
					}
				}
			}
		}

		public class GroupInfo
		{
			public string Name;
			public List<MemberInfo> Members = new List<MemberInfo>();

			public class MemberInfo
			{
				public string Name;
				public int Index;
			}
		}

		#region EnemyGroups

		private static List<GroupInfo> _enemyGroups = null;

		public static List<GroupInfo> EnemyGroups
		{
			get
			{
				if (_enemyGroups == null)
					_enemyGroups = CreateEnemyGroups();

				return _enemyGroups;
			}
		}

		private static List<GroupInfo> CreateEnemyGroups()
		{
			List<GroupInfo> groups = new List<GroupInfo>();

			string[] groupNames = EnemyCatalog.GetGroupNames();
			string[] memberNames = EnemyCatalog.GetMemberNames();
			int count = groupNames.Length;

			for (int index = 0; index < count; index++)
			{
				string groupName = groupNames[index];
				string memberName = memberNames[index];

				GroupInfo group;

				{
					int p = SCommon.IndexOf(groups, v => v.Name == groupName);

					if (p != -1)
					{
						group = groups[p];
					}
					else
					{
						group = new GroupInfo()
						{
							Name = groupName,
						};

						groups.Add(group);
					}
				}

				group.Members.Add(new GroupInfo.MemberInfo()
				{
					Name = memberName,
					Index = index,
				});
			}
			return groups;
		}

		#endregion

		#region TileGroups

		private static List<GroupInfo> _tileGroups = null;

		public static List<GroupInfo> TileGroups
		{
			get
			{
				if (_tileGroups == null)
					_tileGroups = CreateTileGroups();

				return _tileGroups;
			}
		}

		private static List<GroupInfo> CreateTileGroups()
		{
			List<GroupInfo> groups = new List<GroupInfo>();

			string[] groupNames = TileCatalog.GetGroupNames();
			string[] memberNames = TileCatalog.GetMemberNames();
			int count = groupNames.Length;

			for (int index = 0; index < count; index++)
			{
				string groupName = groupNames[index];
				string memberName = memberNames[index];

				GroupInfo group;

				{
					int p = SCommon.IndexOf(groups, v => v.Name == groupName);

					if (p != -1)
					{
						group = groups[p];
					}
					else
					{
						group = new GroupInfo()
						{
							Name = groupName,
						};

						groups.Add(group);
					}
				}

				group.Members.Add(new GroupInfo.MemberInfo()
				{
					Name = memberName,
					Index = index,
				});
			}
			return groups;
		}

		#endregion
	}
}
