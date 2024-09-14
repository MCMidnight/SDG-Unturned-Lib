using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Workshop;
using SDG.Unturned;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Workshop
{
	// Token: 0x02000016 RID: 22
	public class SteamworksWorkshopService : Service, IWorkshopService, IService
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000036C3 File Offset: 0x000018C3
		public bool canOpenWorkshop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000036C6 File Offset: 0x000018C6
		public void open(PublishedFileId_t id)
		{
			Provider.openURL("http://steamcommunity.com/sharedfiles/filedetails/?id=" + id.m_PublishedFileId.ToString());
		}
	}
}
