using System;

namespace SDG.Unturned
{
	// Token: 0x02000758 RID: 1880
	public class ItemIconInfo
	{
		// Token: 0x040026B7 RID: 9911
		[Obsolete("Removed in favor of itemAsset")]
		public ushort id;

		// Token: 0x040026B8 RID: 9912
		[Obsolete("Removed in favor of skinAsset")]
		public ushort skin;

		// Token: 0x040026B9 RID: 9913
		public byte quality;

		// Token: 0x040026BA RID: 9914
		public byte[] state;

		// Token: 0x040026BB RID: 9915
		public ItemAsset itemAsset;

		// Token: 0x040026BC RID: 9916
		public SkinAsset skinAsset;

		// Token: 0x040026BD RID: 9917
		public string tags;

		// Token: 0x040026BE RID: 9918
		public string dynamic_props;

		// Token: 0x040026BF RID: 9919
		public int x;

		// Token: 0x040026C0 RID: 9920
		public int y;

		// Token: 0x040026C1 RID: 9921
		public bool scale;

		// Token: 0x040026C2 RID: 9922
		public bool readableOnCPU;

		// Token: 0x040026C3 RID: 9923
		internal bool isEligibleForCaching;

		// Token: 0x040026C4 RID: 9924
		public ItemIconReady callback;
	}
}
