using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class KeyBundles
	{
		#region RawKeys

		public static string RES_RAW_KEY_01 = @"

852361b1089a2a286d6ec289c57a03327c4cccfc00d189a3f2c7864cce44beb0
4155de005b11e85429e053cd58a55daaf00b9a2a0402aabdf728a70cc736f82b
6b9da652ca8bd79f0d37c24409a5f5a959a9c4142ae946f371231cf21164430d
9aa57c3417ec97baaa895ccf5ffc1a7ceb02d9ced315b3082ddd961108a234b0
9895f9f77540fe0afbf0bd2821c227d5917ed58820b618b46a1cb9bbd312ab9a

";

		public static string RES_RAW_KEY_02 = @"

b51a09c15000777ae15c2b533a78b0e36575960acad8c808b4caf1258b879062
30f278948330618e9118c685b1a3317c77cb3be95ec0065adbe56675af612fa2
0fdc329a5fc29f83ef78fa8aef88e3b73e964b6fd864abb25899d90965eee8de
88c82f604b083d89a4a0097c027b88f45f51e598688c00f2786c5457505f2ebc
a1ba34822378a5be2baedb95b2a2e1c30900ff0bbd31a24c28bb66db94abd116

";

		public static string RES_RAW_KEY_03 = @"

6d46734af660b302e40c5dc9f8c79b62a8104b414971f269eb66685abc1e84a7
a1c07f7d10ec4dea72803d7ceaa9f636dce040e991154f2b6bd9a480b3b04737
d6e6a4088e9457b1bfd3811b942ab78a2223963eaf33a56762985aea6ec2ee1c
389d37f56d6178b42a7d8189dfdb6c6bef4a3c67d26e7a28d8518c1801ded9c2
c2c6dbe17c38a213c2057718cdcd5f9004a91af57b4a63c948f5264abe3337cf

";

		#endregion

		// ====
		// ====
		// ====

		public static byte[] RAW_KEY_01 = SCommon.Hex.ToBytes(new string(RES_RAW_KEY_01.Where(v => ' ' < v).ToArray()));
		public static byte[] RAW_KEY_02 = SCommon.Hex.ToBytes(new string(RES_RAW_KEY_02.Where(v => ' ' < v).ToArray()));
		public static byte[] RAW_KEY_03 = SCommon.Hex.ToBytes(new string(RES_RAW_KEY_03.Where(v => ' ' < v).ToArray()));
	}
}
