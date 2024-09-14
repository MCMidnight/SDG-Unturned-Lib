using System;

namespace SDG.Unturned
{
	// Token: 0x02000389 RID: 905
	public enum EWorkshopDownloadRestrictionResult
	{
		/// <summary>
		/// Workshop item does not have any IP restrictions in place.
		/// </summary>
		// Token: 0x04000D42 RID: 3394
		NoRestrictions,
		/// <summary>
		/// Workshop item has an IP whitelist, and server IP is not on it.
		/// </summary>
		// Token: 0x04000D43 RID: 3395
		NotWhitelisted,
		/// <summary>
		/// Workshop item has an IP blacklist, and server IP is on it.
		/// </summary>
		// Token: 0x04000D44 RID: 3396
		Blacklisted,
		/// <summary>
		/// Workshop item does have IP restrictions, and server IP is allowed.
		/// </summary>
		// Token: 0x04000D45 RID: 3397
		Allowed,
		/// <summary>
		/// Workshop item has been banned by an admin.
		/// </summary>
		// Token: 0x04000D46 RID: 3398
		Banned,
		/// <summary>
		/// Workshop item is hidden from everyone.
		/// </summary>
		// Token: 0x04000D47 RID: 3399
		PrivateVisibility
	}
}
