using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDCCResource
	{
		private static Dictionary<string, DDPicture> PictureCache = SCommon.CreateDictionaryIgnoreCase<DDPicture>();

		public static DDPicture GetPicture(string file)
		{
			if (!PictureCache.ContainsKey(file))
				PictureCache.Add(file, DDPictureLoaders.Standard(file));

			return PictureCache[file];
		}

		private static Dictionary<string, DDMusic> MusicCache = SCommon.CreateDictionaryIgnoreCase<DDMusic>();

		public static DDMusic GetMusic(string file)
		{
			if (!MusicCache.ContainsKey(file))
				MusicCache.Add(file, new DDMusic(file));

			return MusicCache[file];
		}

		private static Dictionary<string, DDSE> SECache = SCommon.CreateDictionaryIgnoreCase<DDSE>();

		public static DDSE GetSE(string file)
		{
			if (!SECache.ContainsKey(file))
				SECache.Add(file, new DDSE(file));

			return SECache[file];
		}

		// ====
		// ここから開放(キャッシュを空にする)
		// ====

		public static void ClearAll()
		{
			ClearPicture();
			ClearMusic();
			ClearSE();
		}

		public static void ClearPicture()
		{
			Clear(PictureCache, DDPictureUtils.Pictures, picture => picture.Unload());
		}

		public static void ClearMusic()
		{
			Clear(MusicCache, DDMusicUtils.Musics, music => music.Sound.Unload(), music => !music.Sound.IsPlaying());
		}

		public static void ClearSE()
		{
			Clear(SECache, DDSEUtils.SEList, se => se.Sound.Unload(), se => !se.Sound.IsPlaying());
		}

		public static void Clear<K, T>(Dictionary<K, T> cache, List<T> store, Action<T> a_unload)
		{
			Clear(cache, store, a_unload, handle => true);
		}

		public static void Clear<K, T>(Dictionary<K, T> cache, List<T> store, Action<T> a_unload, Predicate<T> match)
		{
			HashSet<T> handles = new HashSet<T>(cache.Values.Where(handle => match(handle)));

			foreach (T handle in handles)
				a_unload(handle);

			store.RemoveAll(handle => handles.Contains(handle));
			P_RemoveWhereValue(cache, handle => handles.Contains(handle));
		}

		private static void P_RemoveWhereValue<K, T>(Dictionary<K, T> map, Predicate<T> match)
		{
			foreach (K key in map.Keys.ToArray()) // .ToArray() as shallow copy
				if (match(map[key]))
					P_Remove(map, key);
		}

		private static void P_Remove<K, T>(Dictionary<K, T> map, K key)
		{
			map.Remove(key);
		}
	}
}
