using System;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Provider.Services.Community
{
	// Token: 0x02000064 RID: 100
	public interface ICommunityService : IService
	{
		// Token: 0x06000236 RID: 566
		void setStatus(string status);

		// Token: 0x06000237 RID: 567
		Texture2D getIcon(int id);

		// Token: 0x06000238 RID: 568
		Texture2D getIcon(CSteamID steamID, bool shouldCache = false);

		// Token: 0x06000239 RID: 569
		SteamGroup getCachedGroup(CSteamID steamID);

		// Token: 0x0600023A RID: 570
		SteamGroup[] getGroups();

		// Token: 0x0600023B RID: 571
		bool checkGroup(CSteamID steamID);
	}
}
