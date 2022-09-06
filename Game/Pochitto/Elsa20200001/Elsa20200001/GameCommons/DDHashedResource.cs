using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDHashedResource
	{
		private static Dictionary<DDHash, DDPicture> PictureCache = new Dictionary<DDHash, DDPicture>(new DDHash.IEComp());

		public static DDPicture GetPicture(DDHashedData data)
		{
			if (!PictureCache.ContainsKey(data.Hash))
				PictureCache.Add(data.Hash, DDPictureLoaders.Standard(() => data.Entity));

			return PictureCache[data.Hash];
		}

		private static Dictionary<DDHash, DDMusic> MusicCache = new Dictionary<DDHash, DDMusic>(new DDHash.IEComp());

		public static DDMusic GetMusic(DDHashedData data)
		{
			if (!MusicCache.ContainsKey(data.Hash))
				MusicCache.Add(data.Hash, new DDMusic(() => data.Entity));

			return MusicCache[data.Hash];
		}

		private static Dictionary<DDHash, DDSE> SECache = new Dictionary<DDHash, DDSE>(new DDHash.IEComp());

		public static DDSE GetSE(DDHashedData data)
		{
			if (!SECache.ContainsKey(data.Hash))
				SECache.Add(data.Hash, new DDSE(() => data.Entity));

			return SECache[data.Hash];
		}

		// ====
		// ここから開放(キャッシュを空にする)
		// ====

		public static void ClearPicture()
		{
			DDCCResource.Clear(PictureCache, DDPictureUtils.Pictures, picture => picture.Unload());
		}

		/// <summary>
		/// クリア対象の音楽は停止していること。
		/// -- 再生中に Unload したらマズいのかどうかは不明。多分マズいだろう。
		/// </summary>
		public static void ClearMusic()
		{
			DDCCResource.Clear(MusicCache, DDMusicUtils.Musics, music => music.Sound.Unload());
		}

		/// <summary>
		/// クリア対象の効果音は停止していること。
		/// -- 再生中に Unload したらマズいのかどうかは不明。多分マズいだろう。
		/// </summary>
		public static void ClearSE()
		{
			DDCCResource.Clear(SECache, DDSEUtils.SEList, se => se.Sound.Unload());
		}
	}
}
