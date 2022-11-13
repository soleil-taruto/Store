#pragma comment(lib, "user32.lib")

#include <windows.h>
#include <stdio.h>
#include <math.h>

typedef unsigned char uchar;
typedef unsigned __int16 uint16;
typedef signed int sint;
typedef unsigned int uint;
typedef signed __int64 sint64;
typedef unsigned __int64 uint64;

static void error(void)
{
	printf("ERROR\n");
	exit(1);
}

#define zeroclear(var) \
	(memset(&(var), 0x00, sizeof((var))))

sint64 d2i64(double value)
{
	return (sint64)(value < 0.0 ? value - 0.5 : value + 0.5);
}
sint d2i(double value)
{
	return (sint)d2i64(value);
}

#define m_min(v1, v2) ((v1) < (v2) ? (v1) : (v2))
#define m_max(v1, v2) ((v1) < (v2) ? (v2) : (v1))

#define m_minim(var, value) ((var) = m_min((var), (value)))
#define m_maxim(var, value) ((var) = m_max((var), (value)))

// Random.c >>>>

static uint X = 1;
static uint Y;
static uint Z;
static uint A;

static uint Xorshift128(void)
{
	uint t;

	t = X;
	t ^= X << 11;
	t ^= t >> 8;
	t ^= A;
	t ^= A >> 19;
	X = Y;
	Y = Z;
	Z = A;
	A = t;

	return t;
}
void InitRandom(void)
{
	uint64 seed = GetTickCount64();
	uint i;

	X = (uint)(seed % 65521) << 16 | (uint)(seed % 65519);
	Y = (uint)(seed % 65497) << 16 | (uint)(seed % 65479);
	Z = (uint)(seed % 65449) << 16 | (uint)(seed % 65447);
	A = 1;

	for(i = 16; i; i--)
		Xorshift128();
}
uint GetRandom(void)
{
	return Xorshift128();
}

// <<<< Random.c

#define SLEEP_ONCE_MILLIS 500

/*
	構造体参考

		[StructLayout(LayoutKind.Sequential)]
		private struct MOUSEINPUT
		{
			public int dx;
			public int dy;
			public int mouseData;
			public int dwFlags;
			public int time;
			public int dwExtraInfo;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct KEYBDINPUT
		{
			public short wVk;
			public short wScan;
			public int dwFlags;
			public int time;
			public int dwExtraInfo;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct HARDWAREINPUT
		{
			public int uMsg;
			public short wParamL;
			public short wParamH;
		};

		[StructLayout(LayoutKind.Explicit)]
		private struct INPUT
		{
			[FieldOffset(0)]
			public int type;
			[FieldOffset(4)]
			public MOUSEINPUT mi;
			[FieldOffset(4)]
			public KEYBDINPUT ki;
			[FieldOffset(4)]
			public HARDWAREINPUT hi;
		};
*/

static void DoMouseCursor(sint x, sint y)
{
	INPUT i;

	zeroclear(i);

	i.type = INPUT_MOUSE;
	i.mi.dx = d2i(x * (65536.0 / GetSystemMetrics(SM_CXSCREEN)));
	i.mi.dy = d2i(y * (65536.0 / GetSystemMetrics(SM_CYSCREEN)));
	i.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
	i.mi.dwExtraInfo = GetMessageExtraInfo();

	SendInput(1, &i, sizeof(INPUT));
}
static void DoMouseButton(uint kind, int downFlag)
{
	INPUT i;
	int flag;

	switch (kind)
	{
	case 1: flag = downFlag ? MOUSEEVENTF_LEFTDOWN   : MOUSEEVENTF_LEFTUP;   break;
	case 2: flag = downFlag ? MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP; break;
	case 3: flag = downFlag ? MOUSEEVENTF_RIGHTDOWN  : MOUSEEVENTF_RIGHTUP;  break;

	default:
		error();
	}

	zeroclear(i);

	i.type = INPUT_MOUSE;
	i.mi.dwFlags = flag;
	i.mi.dwExtraInfo = GetMessageExtraInfo();

	SendInput(1, &i, sizeof(INPUT));
}
static void DoMouseWheel(sint level) // level: -1 == 手前に1コロ, +1 == 奥へ1コロ
{
	INPUT i;

	zeroclear(i);

	i.type = INPUT_MOUSE;
	i.mi.mouseData = level * WHEEL_DELTA;
	i.mi.dwFlags = MOUSEEVENTF_WHEEL;
	i.mi.dwExtraInfo = GetMessageExtraInfo();

	SendInput(1, &i, sizeof(INPUT));
}
static void DoKeyboard(uint16 vk, int downFlag)
{
	INPUT i;

	zeroclear(i);

	i.type = INPUT_KEYBOARD;
	i.ki.wVk = vk;
	i.ki.wScan = MapVirtualKey(vk, 0);
	i.ki.dwFlags = KEYEVENTF_EXTENDEDKEY | (downFlag ? 0 : KEYEVENTF_KEYUP); // KEYEVENTF_EXTENDEDKEY を指定しないとシフトが押せない？？？
	i.mi.dwExtraInfo = GetMessageExtraInfo();

	SendInput(1, &i, sizeof(INPUT));
}
static void CheckBreak(void)
{
#if 0
	if (GetAsyncKeyState(VK_SCROLL) != 0)
	{
		cout("VK_SCROLL PRESSED !!!\n");
		termination(1);
	}
#endif
}
int main(int argc, char **argv)
{
	int argi = 1;

	uint xRndRng = 0;
	uint yRndRng = 0;

	while (argi < argc)
	{
		CheckBreak();

		if (argi + 2 < argc && !_stricmp(argv[argi], "RR"))
		{
			argi++;
			xRndRng = atoi(argv[argi++]);
			yRndRng = atoi(argv[argi++]);

			m_minim(xRndRng, 1000000000); // HACK: rough limit
			m_minim(yRndRng, 1000000000); // HACK: rough limit

			InitRandom();
			continue;
		}
		if (argi + 2 < argc && !_stricmp(argv[argi], "MC"))
		{
			sint x;
			sint y;

			argi++;
			x = atoi(argv[argi++]);
			y = atoi(argv[argi++]);

			if(xRndRng)
				x += (sint)(GetRandom() % (xRndRng * 2 + 1)) - (sint)xRndRng;

			if(yRndRng)
				y += (sint)(GetRandom() % (yRndRng * 2 + 1)) - (sint)yRndRng;

//			printf("MOUSE_CURSOR: %d, %d\n", x, y);

			DoMouseCursor(x, y);
			continue;
		}
		if (argi + 2 < argc && !_stricmp(argv[argi], "MB"))
		{
			uint kind;
			int downFlag;

			argi++;
			kind = atoi(argv[argi++]);
			downFlag = atoi(argv[argi++]);

//			printf("MOUSE_BUTTON: %u, %d\n", kind, downFlag);

			DoMouseButton(kind, downFlag);
			continue;
		}
		if (argi + 1 < argc && !_stricmp(argv[argi], "MW"))
		{
			sint level;

			argi++;
			level = atoi(argv[argi++]);

//			printf("MOUSE_WHEEL: %d\n", level);

			DoMouseWheel(level);
			continue;
		}
		if (argi + 2 < argc && !_stricmp(argv[argi], "KB"))
		{
			uint vk;
			int downFlag;

			argi++;
			vk = atoi(argv[argi++]);
			downFlag = atoi(argv[argi++]);

//			printf("KEYBOARD: %02x (%u), %d\n", vk, vk, downFlag);

			DoKeyboard(vk, downFlag);
			continue;
		}
		if (argi + 1 < argc && !_stricmp(argv[argi], "T"))
		{
			uint millis;

			argi++;
			millis = atoi(argv[argi++]);

//			printf("SLEEP %u\n", millis);

			for (; ; )
			{
				uint t = m_min(SLEEP_ONCE_MILLIS, millis);

				Sleep(t);
				millis -= t;

				if (!millis)
					break;

				CheckBreak();
			}
			continue;
		}
		error(); // 不明な引数が指定されました。
	}
}
