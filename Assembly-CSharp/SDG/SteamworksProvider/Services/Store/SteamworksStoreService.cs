using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Store;
using SDG.Unturned;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Store
{
	// Token: 0x02000019 RID: 25
	public class SteamworksStoreService : Service, IStoreService, IService
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00003738 File Offset: 0x00001938
		public void open(IStorePackageID packageID)
		{
			AppId_t appID = ((SteamworksStorePackageID)packageID).appID;
			if (SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToStore(appID, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
				return;
			}
			Provider.openURL("https://store.steampowered.com/app/" + appID.m_AppId.ToString());
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000377B File Offset: 0x0000197B
		public SteamworksStoreService(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
		}

		// Token: 0x0400003D RID: 61
		private SteamworksAppInfo appInfo;
	}
}
