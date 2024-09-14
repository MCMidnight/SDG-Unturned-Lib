using System;
using SDG.Provider.Services.Store;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Store
{
	// Token: 0x02000018 RID: 24
	public class SteamworksStorePackageID : IStorePackageID
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003711 File Offset: 0x00001911
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003719 File Offset: 0x00001919
		public AppId_t appID { get; protected set; }

		// Token: 0x06000077 RID: 119 RVA: 0x00003722 File Offset: 0x00001922
		public SteamworksStorePackageID(uint appID)
		{
			this.appID = new AppId_t(appID);
		}
	}
}
