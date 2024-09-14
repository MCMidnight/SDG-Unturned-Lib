using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Browser;
using SDG.Unturned;

namespace SDG.SteamworksProvider.Services.Browser
{
	// Token: 0x0200002C RID: 44
	public class SteamworksBrowserService : Service, IBrowserService, IService
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000048E4 File Offset: 0x00002AE4
		public bool canOpenBrowser
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000048E7 File Offset: 0x00002AE7
		public void open(string url)
		{
			Provider.openURL(url);
		}
	}
}
