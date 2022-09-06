using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDUtils
	{
		// DX.* >

		public static bool IsWindowActive()
		{
			return DX.GetActiveFlag() != 0;
		}

		/// <summary>
		/// コンピュータを起動してから経過した時間を返す。
		/// ミリ秒
		/// </summary>
		/// <returns>時間</returns>
		public static long GetCurrTime()
		{
			return DX.GetNowHiPerformanceCount() / 1000L;
		}

		public static bool GetMouseDispMode()
		{
			return DX.GetMouseDispFlag() != 0;
		}

		public static void SetMouseDispMode(bool mode)
		{
			DX.SetMouseDispFlag(mode ? 1 : 0);
		}

		public static uint GetColor(I3Color color)
		{
			return DX.GetColor(color.R, color.G, color.B);
		}

		// < DX.*

		public static byte[] SplitableJoin(string[] lines)
		{
			return SCommon.SplittableJoin(lines.Select(line => Encoding.UTF8.GetBytes(line)).ToArray());
		}

		public static string[] Split(byte[] data)
		{
			return SCommon.Split(data).Select(bLine => Encoding.UTF8.GetString(bLine)).ToArray();
		}

		public static void Noop(params object[] dummyPrms)
		{
			// noop
		}

		public static T FastDesertElement<T>(List<T> list, Predicate<T> match, T defval = default(T))
		{
			for (int index = 0; index < list.Count; index++)
				if (match(list[index]))
					return SCommon.FastDesertElement(list, index);

			return defval;
		}

		public static bool CountDown(ref int count)
		{
			if (count < 0)
				count++;
			else if (0 < count)
				count--;
			else
				return false;

			return true;
		}

		public static void Approach(ref double value, double target, double rate)
		{
			value -= target;
			value *= rate;
			value += target;
		}

		public static void ToRange(ref double value, double minval, double maxval)
		{
			value = SCommon.ToRange(value, minval, maxval);
		}

		public static void ToRange(ref int value, int minval, int maxval)
		{
			value = SCommon.ToRange(value, minval, maxval);
		}

		public static void Minim(ref double value, double minval)
		{
			value = Math.Min(value, minval);
		}

		public static void Minim(ref int value, int minval)
		{
			value = Math.Min(value, minval);
		}

		public static void Maxim(ref double value, double minval)
		{
			value = Math.Max(value, minval);
		}

		public static void Maxim(ref int value, int minval)
		{
			value = Math.Max(value, minval);
		}

		public static void Rotate(ref double x, ref double y, double rot)
		{
			double w;

			w = x * Math.Cos(rot) - y * Math.Sin(rot);
			y = x * Math.Sin(rot) + y * Math.Cos(rot);
			x = w;
		}

		public static void Rotate(ref double x, ref double y, D2Point origin, double rot)
		{
			x -= origin.X;
			y -= origin.Y;

			Rotate(ref x, ref y, rot);

			x += origin.X;
			y += origin.Y;
		}

		public static double GetDistance(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		public static double GetDistance(D2Point pt)
		{
			return GetDistance(pt.X, pt.Y);
		}

		public static double GetDistance(D2Point pt, D2Point origin)
		{
			return GetDistance(pt - origin);
		}

		public static bool GetDistanceLessThan(double x, double y, double r)
		{
			// ざっくり処理時間を計測したら、自乗より平方根の方が速かった。@ 2022.7.24
#if true
			return GetDistance(x, y) < r;
#else
			return x * x + y * y < r * r;
#endif
		}

		public static bool GetDistanceLessThan(D2Point pt, double r)
		{
			return GetDistanceLessThan(pt.X, pt.Y, r);
		}

		public static bool GetDistanceLessThan(D2Point pt, D2Point origin, double r)
		{
			return GetDistanceLessThan(pt - origin, r);
		}

		/// <summary>
		/// 原点から指定座標への角度を返す。
		/// ラジアン角 (0.0 ～ Math.PI * 2.0)
		/// 右真横 (0,0 -> 1,0 方向) を 0.0 として時計回り。但し、X軸プラス方向は右、Y軸プラス方向は下である。
		/// 1周は Math.PI * 2.0
		/// </summary>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns>角度</returns>
		public static double GetAngle(double x, double y)
		{
			if (y < 0.0) return Math.PI * 2.0 - GetAngle(x, -y);
			if (x < 0.0) return Math.PI - GetAngle(-x, y);

			if (x <= 0.0) return Math.PI / 2.0;
			if (y <= 0.0) return 0.0;

			double r1 = 0.0;
			double r2 = Math.PI / 2.0;
			double t = y / x;
			double rm;

			for (int c = 1; ; c++)
			{
				rm = (r1 + r2) / 2.0;

				if (10 <= c)
					break;

				double rmt = Math.Tan(rm);

				if (t < rmt)
					r2 = rm;
				else
					r1 = rm;
			}
			return rm;
		}

		public static double GetAngle(D2Point pt)
		{
			return GetAngle(pt.X, pt.Y);
		}

		public static D2Point AngleToPoint(double angle, double distance)
		{
			return new D2Point(
				distance * Math.Cos(angle),
				distance * Math.Sin(angle)
				);
		}

		/// <summary>
		/// よく使っていた古い関数
		/// 始点から終点へ指定速度で向かう場合の(単位時間の)移動量を返す。
		/// </summary>
		/// <param name="x">始点X</param>
		/// <param name="y">始点Y</param>
		/// <param name="targetX">終点X</param>
		/// <param name="targetY">終点Y</param>
		/// <param name="speed">速度</param>
		/// <param name="xa">移動量X</param>
		/// <param name="ya">移動量Y</param>
		public static void MakeXYSpeed(double x, double y, double targetX, double targetY, double speed, out double xa, out double ya)
		{
			D2Point pt = new D2Point(targetX - x, targetY - y);

			pt = AngleToPoint(GetAngle(pt), speed);

			xa = pt.X;
			ya = pt.Y;
		}

		/// <summary>
		/// 円1と円2が衝突しているか判定する。
		/// </summary>
		/// <param name="pt1">円1の中心</param>
		/// <param name="r1">円1の半径</param>
		/// <param name="pt2">円2の中心</param>
		/// <param name="r2">円2の半径</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Circle_Circle(D2Point pt1, double r1, D2Point pt2, double r2)
		{
#if true
			return GetDistanceLessThan(pt1 - pt2, r1 + r2);
#else // old same
			return GetDistance(pt1 - pt2) < r1 + r2;
#endif
		}

		/// <summary>
		/// 円1と点2が衝突しているか判定する。
		/// </summary>
		/// <param name="pt1">円1の中心</param>
		/// <param name="r1">円1の半径</param>
		/// <param name="pt2">点2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Circle_Point(D2Point pt1, double r1, D2Point pt2)
		{
#if true
			return GetDistanceLessThan(pt1 - pt2, r1);
#else // old same
			return GetDistance(pt1 - pt2) < r1;
#endif
		}

		/// <summary>
		/// 円1と矩形2が衝突しているか判定する。
		/// </summary>
		/// <param name="pt1">円1の中心</param>
		/// <param name="r1">円1の半径</param>
		/// <param name="rect2">矩形2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Circle_Rect(D2Point pt1, double r1, D4Rect rect2)
		{
			if (pt1.X < rect2.L) // 左
			{
				if (pt1.Y < rect2.T) // 左上
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.L, rect2.T));
				}
				else if (rect2.B < pt1.Y) // 左下
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.L, rect2.B));
				}
				else // 左中段
				{
					return rect2.L < pt1.X + r1;
				}
			}
			else if (rect2.R < pt1.X) // 右
			{
				if (pt1.Y < rect2.T) // 右上
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.R, rect2.T));
				}
				else if (rect2.B < pt1.Y) // 右下
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.R, rect2.B));
				}
				else // 右中段
				{
					return pt1.X - r1 < rect2.R;
				}
			}
			else // 真上・真ん中・真下
			{
				return rect2.T - r1 < pt1.Y && pt1.Y < rect2.B + r1;
			}
		}

		/// <summary>
		/// 矩形1と点2が衝突しているか判定する。
		/// </summary>
		/// <param name="rect1">矩形1の座標</param>
		/// <param name="pt2">点2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Rect_Point(D4Rect rect1, D2Point pt2)
		{
			return
				rect1.L < pt2.X && pt2.X < rect1.R &&
				rect1.T < pt2.Y && pt2.Y < rect1.B;
		}

		/// <summary>
		/// 矩形1と矩形2が衝突しているか判定する。
		/// </summary>
		/// <param name="rect1">矩形1の座標</param>
		/// <param name="rect2">矩形2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Rect_Rect(D4Rect rect1, D4Rect rect2)
		{
			return
				rect1.L < rect2.R && rect2.L < rect1.R &&
				rect1.T < rect2.B && rect2.T < rect1.B;
		}

		public static bool IsOut(D2Point pt, D4Rect rect, double margin = 0.0)
		{
			return
				pt.X < rect.L - margin || rect.R + margin < pt.X ||
				pt.Y < rect.T - margin || rect.B + margin < pt.Y;
		}

		public static bool IsOutOfScreen(D2Point pt, double margin = 0.0)
		{
			return IsOut(pt, new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H), margin);
		}

		public static bool IsOutOfCamera(D2Point pt, double margin = 0.0)
		{
			return IsOut(pt, new D4Rect(DDGround.Camera.X, DDGround.Camera.Y, DDConsts.Screen_W, DDConsts.Screen_H), margin);
		}

		public static void UpdateInput(ref int counter, bool status)
		{
			if (1 <= counter) // ? 前回_押していた。
			{
				if (status) // ? 今回_押している。
				{
					counter++; // 押している。
				}
				else // ? 今回_離している。
				{
					counter = -1; // 離し始めた。
				}
			}
			else // ? 前回_離していた。
			{
				if (status) // ? 今回_押している。
				{
					counter = 1; // 押し始めた。
				}
				else // ? 今回_離している。
				{
					counter = 0; // 離している。
				}
			}
		}

		private const int POUND_FIRST_DELAY = 17;
		private const int POUND_DELAY = 4;

		public static bool IsPound(int counter)
		{
			return counter == 1 || POUND_FIRST_DELAY < counter && (counter - POUND_FIRST_DELAY) % POUND_DELAY == 1;
		}

		public static DDRandom Random = new DDRandom();

		/// <summary>
		/// (0, 0), (0.5, 1), (1, 0) を通る放物線
		/// </summary>
		/// <param name="x">X軸の値</param>
		/// <returns>Y軸の値</returns>
		public static double Parabola(double x)
		{
			return (x - x * x) * 4.0;
		}

		/// <summary>
		/// S字曲線
		/// (0, 0), (0.5, 0.5), (1, 1) を通る曲線
		/// x == ～0.5 の区間は加速(等加速)する。
		/// x == 0.5～ の区間は減速(等加速)する。
		/// </summary>
		/// <param name="x">X軸の値</param>
		/// <returns>Y軸の値</returns>
		public static double SCurve(double x)
		{
			if (x < 0.5)
				return (1.0 - Parabola(x + 0.5)) * 0.5;
			else
				return (1.0 + Parabola(x - 0.5)) * 0.5;
		}

		/// <summary>
		/// サイズを(アスペクト比を維持して)矩形領域いっぱいに広げる。
		/// </summary>
		/// <param name="size">サイズ</param>
		/// <param name="rect">矩形領域</param>
		/// <param name="interior">矩形領域の内側に張り付く場合の出力先</param>
		/// <param name="exterior">矩形領域の外側に張り付く場合の出力先</param>
		public static void AdjustRect(D2Size size, D4Rect rect, out D4Rect interior, out D4Rect exterior)
		{
			double w_h = (rect.H * size.W) / size.H; // 高さを基準にした幅
			double h_w = (rect.W * size.H) / size.W; // 幅を基準にした高さ

			D4Rect rect1;
			D4Rect rect2;

			rect1.L = rect.L + (rect.W - w_h) / 2.0;
			rect1.T = rect.T;
			rect1.W = w_h;
			rect1.H = rect.H;

			rect2.L = rect.L;
			rect2.T = rect.T + (rect.H - h_w) / 2.0;
			rect2.W = rect.W;
			rect2.H = h_w;

			if (w_h < rect.W)
			{
				interior = rect1;
				exterior = rect2;
			}
			else
			{
				interior = rect2;
				exterior = rect1;
			}
		}

		public static D4Rect AdjustRectInterior(D2Size size, D4Rect rect)
		{
			D4Rect interior;
			D4Rect exterior;

			AdjustRect(size, rect, out interior, out exterior);

			return interior;
		}

		private static D4Rect Prv_AdjustRectExterior(D2Size size, D4Rect rect)
		{
			D4Rect interior;
			D4Rect exterior;

			AdjustRect(size, rect, out interior, out exterior);

			return exterior;
		}

		/// <summary>
		/// サイズを(アスペクト比を維持して)矩形領域(入力)の外側に張り付く矩形領域(出力)を生成する。
		/// スライドレート：
		/// -- 0.0 ～ 1.0
		/// -- 0.0 == 矩形領域(出力)を最も左・上側に寄せる == 矩形領域(出力)の右・下側と矩形領域(入力)の右・下側が重なる
		/// -- 0.5 == 中央
		/// -- 1.0 == 矩形領域(出力)を最も右・下側に寄せる == 矩形領域(出力)の左・上側と矩形領域(入力)の左・上側が重なる
		/// </summary>
		/// <param name="size">サイズ</param>
		/// <param name="rect">矩形領域(入力)</param>
		/// <param name="xRate">スライドレート(X_方向)</param>
		/// <param name="yRate">スライドレート(Y_方向)</param>
		/// <param name="extraZoom">倍率(外側に張り付くサイズからの倍率)</param>
		/// <returns>矩形領域(出力)</returns>
		public static D4Rect AdjustRectExterior(D2Size size, D4Rect rect, double xRate = 0.5, double yRate = 0.5, double extraZoom = 1.0)
		{
			D4Rect exterior = Prv_AdjustRectExterior(size, rect);

			exterior.W *= extraZoom;
			exterior.H *= extraZoom;

			double rangeX = exterior.W - rect.W;
			double rangeY = exterior.H - rect.H;

			exterior.L = rect.L + rangeX * (xRate - 1.0);
			exterior.T = rect.T + rangeY * (yRate - 1.0);

			return exterior;
		}

		/// <summary>
		/// 始点から終点までの間の指定レートの位置の値を返す。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="rate">レート</param>
		/// <returns>レートの値</returns>
		public static double AToBRate(double a, double b, double rate)
		{
			return a + (b - a) * rate;
		}

		/// <summary>
		/// 始点から終点までの間の指定レートの位置を返す。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="rate">レート</param>
		/// <returns>レートの位置</returns>
		public static D2Point AToBRate(D2Point a, D2Point b, double rate)
		{
			return a + (b - a) * rate;
		}

		/// <summary>
		/// 始点から終点までの間の位置をレートに変換する。
		/// </summary>
		/// <param name="a">始点</param>
		/// <param name="b">終点</param>
		/// <param name="value">位置</param>
		/// <returns>レート</returns>
		public static double RateAToB(double a, double b, double value)
		{
			return (value - a) / (b - a);
		}

		public static int Sign(double value)
		{
			if (value < 0) return -1;
			if (value > 0) return 1;
			return 0;
		}

		/// <summary>
		/// ラジアン角の正規化
		/// </summary>
		/// <param name="angle">角度</param>
		/// <returns>角度</returns>
		public static double ToInRangeAngle(double angle)
		{
			while (angle < 0.0)
			{
				angle += Math.PI * 2.0;
			}
			while (Math.PI * 2.0 < angle)
			{
				angle -= Math.PI * 2.0;
			}
			return angle;
		}
	}
}
