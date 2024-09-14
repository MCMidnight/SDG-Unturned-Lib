using System;

namespace SDG.Unturned
{
	// Token: 0x02000687 RID: 1671
	public enum ESteamCallValidation
	{
		// Token: 0x0400215B RID: 8539
		NONE,
		/// <summary>
		/// Only RPCs from the server will be allowed to invoke this method.
		/// </summary>
		// Token: 0x0400215C RID: 8540
		ONLY_FROM_SERVER,
		/// <summary>
		/// RPCs are only allowed to invoke this method if we're running as server.
		/// </summary>
		// Token: 0x0400215D RID: 8541
		SERVERSIDE,
		/// <summary>
		/// Only RPCs from the owner of the object will be allowed to invoke this method.
		/// </summary>
		// Token: 0x0400215E RID: 8542
		ONLY_FROM_OWNER
	}
}
