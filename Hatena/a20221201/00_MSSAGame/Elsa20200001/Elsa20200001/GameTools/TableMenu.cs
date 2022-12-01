using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.GameTools
{
	public class TableMenu
	{
		private int T; // 描画する Y-座標 Top
		private int YStep;
		private int FontSize;
		private Action WallDrawer;
		private int Selected_X = 0;
		private int Selected_Y = 0;

		public TableMenu(int t, int yStep, int fontSize, Action wallDrawer)
		{
			this.T = t;
			this.YStep = yStep;
			this.FontSize = fontSize;
			this.WallDrawer = wallDrawer;
		}

		public void SetSelectedPosition(int x, int y)
		{
			this.Selected_X = x;
			this.Selected_Y = y;
		}

		private class ItemInfo
		{
			public bool GroupFlag;
			public string Title;
			public I3Color Color;
			public I3Color BorderColor;
			public Action A_Desided;
		}

		private class ColumnInfo
		{
			public int X; // 描画する X-座標
			public List<ItemInfo> Items = new List<ItemInfo>();
		}

		private List<ColumnInfo> Columns = new List<ColumnInfo>();

		public void AddColumn(int x)
		{
			this.Columns.Add(new ColumnInfo() { X = x });
		}

		public void AddItem(bool groupFlag, string title, I3Color color, I3Color borderColor, Action a_desided = null)
		{
			if (a_desided == null)
				a_desided = () => { };

			this.Columns[this.Columns.Count - 1].Items.Add(new ItemInfo()
			{
				GroupFlag = groupFlag,
				Title = title,
				Color = color,
				BorderColor = borderColor,
				A_Desided = a_desided,
			});
		}

		public void Perform()
		{
			if (this.Columns.Count == 0) // ? 列が無い。
				throw new DDError();

			foreach (ColumnInfo column in this.Columns)
				if (column.Items.Count == 0) // ? 列に項目が無い。
					throw new DDError();

			// 最終項目(一番右の列の一番下の項目)の位置
			//
			int lastItem_X = this.Columns.Count - 1;
			int lastItem_Y = this.Columns[lastItem_X].Items.Count - 1;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			for (; ; )
			{
				// 最終項目を「終了」と見なす。

				if (DDInput.PAUSE.GetInput() == 1) // 即_終了
				{
					this.Selected_X = lastItem_X;
					this.Selected_Y = lastItem_Y;
					break;
				}
				if (DDInput.A.GetInput() == 1) // 決定
				{
					break;
				}
				if (DDInput.B.GetInput() == 1) // 一旦カーソルを終了に合わせて、尚もボタンが押されたら終了する。
				{
					if (
						this.Selected_X == lastItem_X &&
						this.Selected_Y == lastItem_Y
						)
						break;

					this.Selected_X = lastItem_X;
					this.Selected_Y = lastItem_Y;
				}

				bool 上へ移動した = false;
				bool 横へ移動した = false;

				if (DDInput.DIR_8.IsPound())
				{
					this.Selected_Y--;
					上へ移動した = true;
				}
				if (DDInput.DIR_2.IsPound())
				{
					this.Selected_Y++;
				}
				if (DDInput.DIR_4.IsPound())
				{
					this.Selected_X--;
					横へ移動した = true;
				}
				if (DDInput.DIR_6.IsPound())
				{
					this.Selected_X++;
					横へ移動した = true;
				}

				this.Selected_X += this.Columns.Count;
				this.Selected_X %= this.Columns.Count;

				if (横へ移動した)
					this.Selected_Y = Math.Min(this.Selected_Y, this.Columns[this.Selected_X].Items.Count - 1);

				this.Selected_Y += this.Columns[this.Selected_X].Items.Count;
				this.Selected_Y %= this.Columns[this.Selected_X].Items.Count;

				if (this.Columns[this.Selected_X].Items[this.Selected_Y].GroupFlag)
				{
					if (上へ移動した)
						this.Selected_Y--;
					else
						this.Selected_Y++;

					this.Selected_Y += this.Columns[this.Selected_X].Items.Count;
					this.Selected_Y %= this.Columns[this.Selected_X].Items.Count;
				}

				this.WallDrawer();

				for (int x = 0; x < this.Columns.Count; x++)
				{
					ColumnInfo column = this.Columns[x];

					DDPrint.SetPrint(column.X, this.T, this.YStep, this.FontSize);

					for (int y = 0; y < column.Items.Count; y++)
					{
						ItemInfo item = column.Items[y];
						bool selected = x == this.Selected_X && y == this.Selected_Y;
						string line;

						if (item.GroupFlag)
						{
							if (selected)
								line = "* " + item.Title; // 通常はここに到達しない。
							else
								line = item.Title;
						}
						else
						{
							if (selected)
								line = " [>] " + item.Title;
							else
								line = " [ ] " + item.Title;
						}

						DDPrint.SetColor(item.Color);
						DDPrint.SetBorder(item.BorderColor);
						DDPrint.PrintLine(line);
						DDPrint.Reset();
					}
				}
				DDEngine.EachFrame();
			}

			{
				ColumnInfo column = this.Columns[this.Selected_X];
				ItemInfo item = column.Items[this.Selected_Y];

				item.A_Desided();
			}

			DDEngine.FreezeInput();

			this.Columns.Clear();
		}
	}
}
