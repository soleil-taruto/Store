using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	/// <summary>
	/// ここで取得した DDPicture は Unload する必要あり
	/// 必要なし -> DDPictureLoadersNNUL
	/// </summary>
	public static class DDPictureLoaders
	{
		/// <summary>
		/// 標準
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <returns>画像</returns>
		public static DDPicture Standard(string file)
		{
			return Standard(() => DDPictureLoaderUtils.File2FileData(file));
		}

		/// <summary>
		/// 標準
		/// </summary>
		/// <param name="getFileData">画像データ取得先</param>
		/// <returns>画像</returns>
		public static DDPicture Standard(Func<byte[]> getFileData)
		{
			return new DDPicture(
				() => DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(DDPictureLoaderUtils.FileData2SoftImage(getFileData()))),
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 各色を反転する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <returns>画像</returns>
		public static DDPicture Inverse(string file)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							dot.R ^= 0xff;
							dot.G ^= 0xff;
							dot.B ^= 0xff;

							DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 幅を2倍にして、左に元の画像、右に元の画像を左右反転した画像を配置する。
		/// 左右対称で「元画像には左側しかない場合」用
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <returns>画像</returns>
		public static DDPicture Mirror(string file)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					{
						int h2 = DDPictureLoaderUtils.CreateSoftImage(w * 2, h);

						for (int x = 0; x < w; x++)
						{
							for (int y = 0; y < h; y++)
							{
								DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

								DDPictureLoaderUtils.SetSoftImageDot(h2, x, y, dot);
								DDPictureLoaderUtils.SetSoftImageDot(h2, w * 2 - 1 - x, y, dot);
							}
						}
						DDPictureLoaderUtils.ReleaseSoftImage(siHandle);
						siHandle = h2;
					}

					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 指定領域を元画像として処理する。
		/// 幅を2倍にして、左に元の画像、右に元の画像を左右反転した画像を配置する。
		/// 左右対称で「元画像には左側しかない場合」用
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <param name="derRect">指定領域</param>
		/// <returns>画像</returns>
		public static DDPicture Mirror(string file, I4Rect derRect)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					//int w;
					//int h;

					//DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					{
						int h2 = DDPictureLoaderUtils.CreateSoftImage(derRect.W * 2, derRect.H);

						for (int x = 0; x < derRect.W; x++)
						{
							for (int y = 0; y < derRect.H; y++)
							{
								DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(
									siHandle,
									derRect.L + x,
									derRect.T + y
									);

								DDPictureLoaderUtils.SetSoftImageDot(
									h2,
									x,
									y,
									dot
									);
								DDPictureLoaderUtils.SetSoftImageDot(
									h2,
									derRect.W * 2 - 1 - x,
									y,
									dot
									);
							}
						}
						DDPictureLoaderUtils.ReleaseSoftImage(siHandle);
						siHandle = h2;
					}

					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 左上隅のピクセルの色を透明色として処理する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <returns>画像</returns>
		public static DDPicture BgTrans(string file)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					DDPictureLoaderUtils.Dot targetDot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, 0, 0); // 左上隅のピクセル

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							if (
								targetDot.R == dot.R &&
								targetDot.G == dot.G &&
								targetDot.B == dot.B
								)
							{
								dot.A = 0;

								DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
							}
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 指定された色を透明色として処理する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <param name="targetColor">指定色</param>
		/// <returns>画像</returns>
		public static DDPicture RGBToTrans(string file, I3Color targetColor)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					//DDPictureLoaderUtils.Dot targetDot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, 0, 0); // 左上隅のピクセル

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							if (
								targetColor.R == dot.R &&
								targetColor.G == dot.G &&
								targetColor.B == dot.B
								)
							{
								dot.A = 0;

								DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
							}
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 各色を入れ替える。
		/// 色の入れ替え指定：
		/// -- 4文字の文字列
		/// -- 各文字は "RGBA" の何れか, 重複可, ex. "RRRR", "GBRA"
		/// -- [0] == 出力 R に適用する色を指定する。
		/// -- [1] == 出力 G に適用する色を指定する。
		/// -- [2] == 出力 B に適用する色を指定する。
		/// -- [3] == 出力 A に適用する色を指定する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <param name="mode">色の入れ替え指定</param>
		/// <returns>画像</returns>
		public static DDPicture SelectRGBA(string file, string mode)
		{
			const string s_rgba = "RGBA";

			int ir = s_rgba.IndexOf(mode[0]);
			int ig = s_rgba.IndexOf(mode[1]);
			int ib = s_rgba.IndexOf(mode[2]);
			int ia = s_rgba.IndexOf(mode[3]);

			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							int[] rgba = new int[]
							{
								dot.R,
								dot.G,
								dot.B,
								dot.A,
							};

							dot.R = rgba[ir];
							dot.G = rgba[ig];
							dot.B = rgba[ib];
							dot.A = rgba[ia];

							DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 縮小する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <param name="denom">拡大率の逆数</param>
		/// <returns>画像</returns>
		public static DDPicture Reduct(string file, int denom)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					{
						int new_w = w / denom;
						int new_h = h / denom;
						int new_si_h = DDPictureLoaderUtils.CreateSoftImage(new_w, new_h);

						for (int x = 0; x < new_w; x++)
						{
							for (int y = 0; y < new_h; y++)
							{
								int tR = 0;
								int tG = 0;
								int tB = 0;
								int tA = 0;

								for (int sx = 0; sx < denom; sx++)
								{
									for (int sy = 0; sy < denom; sy++)
									{
										DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x * denom + sx, y * denom + sy);

										tR += dot.R;
										tG += dot.G;
										tB += dot.B;
										tA += dot.A;
									}
								}
								double div = (double)(denom * denom);

								{
									DDPictureLoaderUtils.Dot dot = new DDPictureLoaderUtils.Dot()
									{
										R = SCommon.ToInt(tR / div),
										G = SCommon.ToInt(tG / div),
										B = SCommon.ToInt(tB / div),
										A = SCommon.ToInt(tA / div),
									};

									DDPictureLoaderUtils.SetSoftImageDot(new_si_h, x, y, dot);
								}
							}
						}
						DDPictureLoaderUtils.ReleaseSoftImage(siHandle);
						siHandle = new_si_h;
					}

					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 拡大する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <param name="expNum">拡大率</param>
		/// <returns>画像</returns>
		public static DDPicture Expand(string file, int expNum)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					{
						int new_w = w * expNum;
						int new_h = h * expNum;
						int new_si_h = DDPictureLoaderUtils.CreateSoftImage(new_w, new_h);

						for (int x = 0; x < w; x++)
						{
							for (int y = 0; y < h; y++)
							{
								DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

								for (int sx = 0; sx < expNum; sx++)
									for (int sy = 0; sy < expNum; sy++)
										DDPictureLoaderUtils.SetSoftImageDot(new_si_h, x * expNum + sx, y * expNum + sy, dot);
							}
						}
						DDPictureLoaderUtils.ReleaseSoftImage(siHandle);
						siHandle = new_si_h;
					}

					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		/// <summary>
		/// 左上隅のピクセルの色を透明色として処理する。
		/// 拡大する。
		/// </summary>
		/// <param name="file">画像ファイル</param>
		/// <param name="expNum">拡大率</param>
		/// <returns>画像</returns>
		public static DDPicture BgTransExpand(string file, int expNum)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					DDPictureLoaderUtils.Dot targetDot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, 0, 0); // 左上隅のピクセル

					{
						int new_w = w * expNum;
						int new_h = h * expNum;
						int new_si_h = DDPictureLoaderUtils.CreateSoftImage(new_w, new_h);

						for (int x = 0; x < w; x++)
						{
							for (int y = 0; y < h; y++)
							{
								DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

								if (
									targetDot.R == dot.R &&
									targetDot.G == dot.G &&
									targetDot.B == dot.B
									)
									dot.A = 0;

								for (int sx = 0; sx < expNum; sx++)
									for (int sy = 0; sy < expNum; sy++)
										DDPictureLoaderUtils.SetSoftImageDot(new_si_h, x * expNum + sx, y * expNum + sy, dot);
							}
						}
						DDPictureLoaderUtils.ReleaseSoftImage(siHandle);
						siHandle = new_si_h;
					}

					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}
	}
}
