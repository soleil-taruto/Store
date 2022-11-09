using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		#region EventManager

		public class EventManager
		{
			private bool TimerStarted = false;

			/// <summary>
			/// タイマー開始
			/// </summary>
			public void StartTimer()
			{
				this.TimerStarted = true;
			}

			/// <summary>
			/// タイマー終了
			/// </summary>
			public void EndTimer()
			{
				this.TimerStarted = false;
			}

			/// <summary>
			/// タイマーのイベント実行
			/// </summary>
			/// <param name="routine">イベントロジック</param>
			public void EventHandlerForTimer(Action routine)
			{
				if (this.TimerStarted)
					this.EventHandler(true, routine);
			}

			private bool Busy = false;

			/// <summary>
			/// タイマー以外のイベント実行
			/// </summary>
			/// <param name="background">バックグラウンドで動くイベントか</param>
			/// <param name="routine">イベントロジック</param>
			public void EventHandler(bool background, Action routine)
			{
				if (this.Busy)
					return;

				this.Busy = true;
				try
				{
					routine();
				}
				catch (Exception e)
				{
					if (background)
						ProcMain.WriteLog(e);
					else
						MessageBox.Show("" + e, "失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				finally
				{
					this.Busy = false;
				}
			}
		}

		#endregion

		#region PostShown

		/// <summary>
		/// _Shown メソッドの最後でこのメソッドを呼び出さなければならない。
		/// </summary>
		/// <param name="f">呼び出し側のフォーム</param>
		public static void PostShown(Form f)
		{
			PostShown_Main(f);
			AntiMessageOutOfWindow(f);
			AntiWindowOutOfScreen(f);
		}

		private static void PostShown_Main(Form f)
		{
			PostShown_GetAllControl(f, control =>
			{
				Control c = new Control[]
				{
					control as TextBox,
					control as NumericUpDown,
				}
				.FirstOrDefault(v => v != null);

				if (c != null)
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
			});
		}

		private static void PostShown_GetAllControl(Form f, Action<Control> reaction)
		{
			Queue<Control.ControlCollection> controlTable = new Queue<Control.ControlCollection>();

			controlTable.Enqueue(f.Controls);

			while (1 <= controlTable.Count)
			{
				foreach (Control control in controlTable.Dequeue())
				{
					GroupBox gb = control as GroupBox;

					if (gb != null)
					{
						controlTable.Enqueue(gb.Controls);
					}
					TabControl tc = control as TabControl;

					if (tc != null)
					{
						foreach (TabPage tp in tc.TabPages)
						{
							controlTable.Enqueue(tp.Controls);
						}
					}
					SplitContainer sc = control as SplitContainer;

					if (sc != null)
					{
						controlTable.Enqueue(sc.Panel1.Controls);
						controlTable.Enqueue(sc.Panel2.Controls);
					}
					Panel p = control as Panel;

					if (p != null)
					{
						controlTable.Enqueue(p.Controls);
					}
					reaction(control);
				}
			}
		}

		private static void AntiMessageOutOfWindow(Form f)
		{
			int w = 0;

			PostShown_GetAllControl(f, c =>
			{
				if (c is Label) // 今のところ Label のみ
				{
					w = Math.Max(w, c.Right);
				}
			});

			w += 30; // margin

			if (f.Width < w)
			{
				f.Left -= (w - f.Width) / 2;
				f.Width = w;
			}
		}

		private static void AntiWindowOutOfScreen(Form f)
		{
			Screen screen = GetScreen_Inside(f);

			if (screen == null)
				return;

			I4Rect winRect = new I4Rect(f.Left, f.Top, f.Width, f.Height);
			I4Rect scrRect = new I4Rect(
				screen.Bounds.Left,
				screen.Bounds.Top,
				screen.Bounds.Width,
				screen.Bounds.Height
				);

			winRect.L = Math.Min(winRect.L, scrRect.R - winRect.W);
			winRect.T = Math.Min(winRect.T, scrRect.B - winRect.H);

			winRect.L = Math.Max(winRect.L, scrRect.L);
			winRect.T = Math.Max(winRect.T, scrRect.T);

			if (f.Left != winRect.L)
				f.Left = winRect.L;

			if (f.Top != winRect.T)
				f.Top = winRect.T;
		}

		private static Screen GetScreen_Inside(Form f)
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
					return screen;
			}
			return null;
		}

		#endregion

		public static void HelloWorld()
		{
			Console.WriteLine("Hello, world!");
		}
	}
}
