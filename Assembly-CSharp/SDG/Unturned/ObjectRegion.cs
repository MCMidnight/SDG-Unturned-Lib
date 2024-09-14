using System;

namespace SDG.Unturned
{
	// Token: 0x02000582 RID: 1410
	public class ObjectRegion
	{
		// Token: 0x06002D39 RID: 11577 RVA: 0x000C4DA7 File Offset: 0x000C2FA7
		public ObjectRegion()
		{
			this.isNetworked = false;
			this.updateObjectIndex = 0;
		}

		// Token: 0x04001864 RID: 6244
		public bool isNetworked;

		// Token: 0x04001865 RID: 6245
		public ushort updateObjectIndex;
	}
}
