using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Used to aid backwards compatibility as much as possible when transitioning Unity versions breaks asset bundles.
	/// </summary>
	// Token: 0x0200028E RID: 654
	public static class AssetBundleVersion
	{
		/// <summary>
		/// Unity 5.5 and earlier per-asset .unity3d file.
		/// </summary>
		// Token: 0x04000690 RID: 1680
		public const int UNITY_5 = 1;

		/// <summary>
		/// When "master bundles" were first introduced in order to convert older Unity 5.5 asset bundles in bulk.
		/// </summary>
		// Token: 0x04000691 RID: 1681
		public const int UNITY_2017_LTS = 2;

		/// <summary>
		/// Unity 2018 needed a new version number in order to convert materials from 2017 LTS asset bundles. 2019 did not need a
		/// new version number, but in retrospect it seems unfortunate that we cannot distinguish them, so 2020 does have its own.
		/// </summary>
		// Token: 0x04000692 RID: 1682
		public const int UNITY_2018_AND_2019_LTS = 3;

		// Token: 0x04000693 RID: 1683
		public const int UNITY_2020_LTS = 4;

		/// <summary>
		/// 2021 LTS+
		/// </summary>
		// Token: 0x04000694 RID: 1684
		public const int NEWEST = 5;
	}
}
