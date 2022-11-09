using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class KeyBundle
	{
		#region RawKey

		public static string RES_RAW_KEY = @"

852361b1089a2a286d6ec289c57a03327c4cccfc00d189a3f2c7864cce44beb0
4155de005b11e85429e053cd58a55daaf00b9a2a0402aabdf728a70cc736f82b
6b9da652ca8bd79f0d37c24409a5f5a959a9c4142ae946f371231cf21164430d
9aa57c3417ec97baaa895ccf5ffc1a7ceb02d9ced315b3082ddd961108a234b0
9895f9f77540fe0afbf0bd2821c227d5917ed58820b618b46a1cb9bbd312ab9a

";

		#endregion

		// ====
		// ====
		// ====

		public static byte[] RAW_KEY = SCommon.Hex.ToBytes(new string(RES_RAW_KEY.Where(v => ' ' < v).ToArray()));
	}
}
