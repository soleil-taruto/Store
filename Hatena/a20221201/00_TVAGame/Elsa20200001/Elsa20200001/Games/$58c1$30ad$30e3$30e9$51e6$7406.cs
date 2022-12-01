using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

namespace Charlotte.Games
{
	public static class 壁キャラ処理
	{
		private static Predicate<Tile> IsWall;
		private static int IX;
		private static int IY;
		private static Around A2;
		private static Around A3;

		public static void Perform(ref double x, ref double y, Predicate<Tile> isWall)
		{
			IsWall = isWall;

			// 整数化
			IX = SCommon.ToInt(x);
			IY = SCommon.ToInt(y);

			A2 = new Around(IX, IY, 2);
			A3 = new Around(IX, IY, 3);

			I2Point a2RelPtBk = A2.RelativePoint;

			Perform_A();

			if (
				a2RelPtBk.X != A2.RelativePoint.X ||
				a2RelPtBk.Y != A2.RelativePoint.Y
				)
			{
				x = A2.CenterPoint.X + A2.RelativePoint.X;
				y = A2.CenterPoint.Y + A2.RelativePoint.Y;
			}
		}

		private static void Perform_A()
		{
			switch (
				(IsWall(A2.Table[0, 1].Tile) ? 1 : 0) | // 左下
				(IsWall(A2.Table[1, 1].Tile) ? 2 : 0) | // 右下
				(IsWall(A2.Table[0, 0].Tile) ? 4 : 0) | // 左上
				(IsWall(A2.Table[1, 0].Tile) ? 8 : 0)   // 右上
				)
			{
				case 0: // 壁なし
					break;

				case 15: // 壁の中
					for (int x = 0; x < 3; x++)
					{
						for (int y = 0; y < 3; y++)
						{
							if (A3.Table[x, y].Tile.GetKind() == Tile.Kind_e.SPACE)
							{
								A2.RelativePoint.X += (x - 1) * 10;
								A2.RelativePoint.Y += (y - 1) * 10;

								goto endInsideWall;
							}
						}
					}
					A2.RelativePoint.X += DDUtils.Random.ChooseOne(new int[] { -10, 10 });
					A2.RelativePoint.Y += DDUtils.Random.ChooseOne(new int[] { -10, 10 });

				endInsideWall:
					break;

				case 1: // 左下のみ
				case 4: // 左上のみ
				case 5: // 左
				case 7: // 右上のみ空き
				case 13: // 右下のみ空き
					A2.XTurn();
					Perform_A();
					A2.RelativePoint.X *= -1;
					break;

				case 8: // 右上のみ
				case 9: // 右上と左下
				case 12: // 上
				case 14: // 左下のみ空き
					A2.YTurn();
					Perform_A();
					A2.RelativePoint.Y *= -1;
					break;

				case 2: // 右下のみ
					if (-16 < A2.RelativePoint.X && -16 < A2.RelativePoint.Y)
					{
						const int SLIDE_BORDER = 5;
						const int SLIDE_SPEED = 1;

						if (A2.RelativePoint.X < A2.RelativePoint.Y)
						{
							A2.RelativePoint.X = -16;

							if (A2.RelativePoint.Y < SLIDE_BORDER)
								A2.RelativePoint.Y -= SLIDE_SPEED;
						}
						else
						{
							A2.RelativePoint.Y = -16;

							if (A2.RelativePoint.X < SLIDE_BORDER)
								A2.RelativePoint.X -= SLIDE_SPEED;
						}
					}
					break;

				case 10: // 右
					A2.RelativePoint.X = -16;
					break;

				case 3: // 下
					A2.RelativePoint.Y = -16;
					break;

				case 6: // 左上と右下
					if (A2.RelativePoint.X < A2.RelativePoint.Y)
					{
						A2.RelativePoint.X = -16;
						A2.RelativePoint.Y = 16;
					}
					else
					{
						A2.RelativePoint.X = 16;
						A2.RelativePoint.Y = -16;
					}
					break;

				case 11: // 左上のみ空き
					A2.RelativePoint.X = -16;
					A2.RelativePoint.Y = -16;
					break;

				default:
					throw null; // never
			}
		}
	}
}
