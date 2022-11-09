using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public static class PostShown
	{
		public static void Perform(Form f)
		{
			AntiDefaultContextMenu(f);
			ResolveMessageOutOfWindow(f);
			ResolveWindowOutOfScreen(f);
		}

		private static void AntiDefaultContextMenu(Form f)
		{
			foreach (Control control in GetAllControl(f))
			{
				foreach (Control c in new Control[] { control as TextBox, control as NumericUpDown }.Where(v => v != null))
				{
					if (c.ContextMenuStrip == null)
					{
						ContextMenuStrip menu = new ContextMenuStrip();

						{
							ToolStripMenuItem item = new ToolStripMenuItem();

							item.Text = "項目なし";
							item.Enabled = false;

							menu.Items.Add(item);
						}

						c.ContextMenuStrip = menu;
					}
				}
			}
		}

		private static void ResolveMessageOutOfWindow(Form f)
		{
			int w = GetAllControl(f)
				.Where(v => v is Label)
				.Select(v => v.Right)
				.Concat(new int[] { 0 })
				.Max();

			w += 30; // margin

			if (f.Width < w)
			{
				f.Left -= (w - f.Width) / 2;
				f.Width = w;
			}
		}

		private static void ResolveWindowOutOfScreen(Form f)
		{
			I2Point winCenter = new I2Point((f.Left + f.Right) / 2, (f.Top + f.Bottom) / 2);

			foreach (Screen screen in Screen.AllScreens)
			{
				I4Rect scrRect = new I4Rect(
					screen.Bounds.Left,
					screen.Bounds.Top,
					screen.Bounds.Width,
					screen.Bounds.Height
					);

				if (
					scrRect.L <= winCenter.X && winCenter.X < scrRect.R &&
					scrRect.T <= winCenter.Y && winCenter.Y < scrRect.B
					)
				{
					I4Rect winRect = new I4Rect(f.Left, f.Top, f.Width, f.Height);

					winRect.L = Math.Min(winRect.L, scrRect.R - winRect.W);
					winRect.T = Math.Min(winRect.T, scrRect.B - winRect.H);

					winRect.L = Math.Max(winRect.L, scrRect.L);
					winRect.T = Math.Max(winRect.T, scrRect.T);

					if (f.Left != winRect.L)
						f.Left = winRect.L;

					if (f.Top != winRect.T)
						f.Top = winRect.T;

					return;
				}
			}
		}

		private static IEnumerable<Control> GetAllControl(Form f)
		{
			Queue<Control.ControlCollection> q = new Queue<Control.ControlCollection>();

			q.Enqueue(f.Controls);

			while (1 <= q.Count)
			{
				foreach (Control control in q.Dequeue())
				{
					{
						GroupBox gb = control as GroupBox;

						if (gb != null)
						{
							q.Enqueue(gb.Controls);
						}
					}

					{
						TabControl tc = control as TabControl;

						if (tc != null)
						{
							foreach (TabPage tp in tc.TabPages)
							{
								q.Enqueue(tp.Controls);
							}
						}
					}

					{
						SplitContainer sc = control as SplitContainer;

						if (sc != null)
						{
							q.Enqueue(sc.Panel1.Controls);
							q.Enqueue(sc.Panel2.Controls);
						}
					}

					{
						Panel p = control as Panel;

						if (p != null)
						{
							q.Enqueue(p.Controls);
						}
					}

					yield return control;
				}
			}
		}
	}
}
