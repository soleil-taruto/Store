using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDWin32
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
		}

		[DllImport("user32.dll")]
		private static extern bool
			ClientToScreen // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			(IntPtr hWnd, out POINT lpPoint);

		public static bool W_ClientToScreen(IntPtr hWnd, out POINT lpPoint)
		{
			return
				ClientToScreen // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				(hWnd, out lpPoint);
		}

		public delegate bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool
			EnumWindows // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			(EnumWindowsCallback callback, IntPtr lParam);

		public static bool W_EnumWindows(EnumWindowsCallback callback, IntPtr lParam)
		{
			return
				EnumWindows // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				(callback, lParam);
		}

		[DllImport("user32.dll")]
		private static extern int
			GetWindowText // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			(IntPtr hWnd, StringBuilder buff, int buffLenMax);

		public static int W_GetWindowText(IntPtr hWnd, StringBuilder buff, int buffLenMax)
		{
			return
				GetWindowText // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				(hWnd, buff, buffLenMax);
		}

		public static string GetWindowTitleByHandle(IntPtr hWnd)
		{
			const int BUFF_SIZE = 1000;
			const int MARGIN = 10;

			StringBuilder buff = new StringBuilder(BUFF_SIZE + MARGIN);

			W_GetWindowText(hWnd, buff, BUFF_SIZE);

			return buff.ToString();
		}

		public static void EnumWindowsHandleTitle(Func<IntPtr, string, bool> routine)
		{
			W_EnumWindows((hWnd, lParam) => routine(hWnd, GetWindowTitleByHandle(hWnd)), IntPtr.Zero);
		}

		private static IntPtr? MainWindowHandle = null;

		public static IntPtr GetMainWindowHandle()
		{
			if (MainWindowHandle == null)
			{
				string markTitle = Guid.NewGuid().ToString("B");
				IntPtr handle = IntPtr.Zero;
				bool handleFound = false;

				DX.SetMainWindowText(markTitle);

				EnumWindowsHandleTitle((hWnd, title) =>
				{
					if (title == markTitle)
					{
						handle = hWnd;
						handleFound = true;
						return false;
					}
					return true;
				});

				if (!handleFound)
					throw new DDError();

				DDMain.SetMainWindowTitle();

				MainWindowHandle = handle;
			}
			return MainWindowHandle.Value;
		}

		public const uint FR_PRIVATE = 0x10;

		[DllImport("gdi32.dll")]
		private static extern int
			AddFontResourceEx // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			(string file, uint fl, IntPtr res);

		public static int W_AddFontResourceEx(string file, uint fl, IntPtr res)
		{
			return
				AddFontResourceEx // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				(file, fl, res);
		}

		[DllImport("gdi32.dll")]
		private static extern int
			RemoveFontResourceEx // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
			(string file, uint fl, IntPtr res);

		public static int W_RemoveFontResourceEx(string file, uint fl, IntPtr res)
		{
			return
				RemoveFontResourceEx // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				(file, fl, res);
		}

		public static I2Point GetMousePosition()
		{
			return new I2Point(
				Cursor.Position.
					X // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				,
				Cursor.Position.
					Y // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
				);
		}

		private static I4Rect[] Monitors = null;

		public static I4Rect[] GetAllMonitor()
		{
			if (Monitors == null)
				Monitors = Screen.AllScreens.Select(screen => new I4Rect(
					screen.Bounds.
						Left // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						,
					screen.Bounds.
						Top // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						,
					screen.Bounds.
						Width // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
						,
					screen.Bounds.
						Height // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
					))
					.ToArray();

			return Monitors;
		}
	}
}
