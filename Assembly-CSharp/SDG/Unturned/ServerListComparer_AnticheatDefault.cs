using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006A3 RID: 1699
	public class ServerListComparer_AnticheatDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003925 RID: 14629 RVA: 0x0010CD70 File Offset: 0x0010AF70
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.IsBattlEyeSecure == rhs.IsBattlEyeSecure)
			{
				if (lhs.IsVACSecure == rhs.IsVACSecure)
				{
					return lhs.name.CompareTo(rhs.name);
				}
				if (!lhs.IsVACSecure)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (!lhs.IsBattlEyeSecure)
				{
					return 1;
				}
				return -1;
			}
		}
	}
}
