using System;

namespace SDG.Unturned
{
	// Token: 0x020006EF RID: 1775
	public class GameStatusData
	{
		// Token: 0x06003B12 RID: 15122 RVA: 0x00114628 File Offset: 0x00112828
		public string FormatApplicationVersion()
		{
			return string.Format("3.{0}.{1}.{2}", this.Major_Version, this.Minor_Version, this.Patch_Version);
		}

		// Token: 0x040024EA RID: 9450
		public byte Major_Version;

		// Token: 0x040024EB RID: 9451
		public byte Minor_Version;

		// Token: 0x040024EC RID: 9452
		public byte Patch_Version;

		// Token: 0x040024ED RID: 9453
		public int[] GrantPackageIDs;

		// Token: 0x040024EE RID: 9454
		public string GrantPackageURL;
	}
}
