using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006A5 RID: 1701
	public class ServerListComparer_PerspectiveDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003929 RID: 14633 RVA: 0x0010CDE0 File Offset: 0x0010AFE0
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.cameraMode == rhs.cameraMode)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			return lhs.cameraMode.CompareTo(rhs.cameraMode);
		}
	}
}
