using System;

namespace SDG.Unturned
{
	// Token: 0x0200058C RID: 1420
	public class ResourceRegion
	{
		// Token: 0x06002D79 RID: 11641 RVA: 0x000C64E5 File Offset: 0x000C46E5
		public ResourceRegion()
		{
			this.isNetworked = false;
			this.respawnResourceIndex = 0;
		}

		// Token: 0x04001886 RID: 6278
		public bool isNetworked;

		// Token: 0x04001887 RID: 6279
		public ushort respawnResourceIndex;
	}
}
