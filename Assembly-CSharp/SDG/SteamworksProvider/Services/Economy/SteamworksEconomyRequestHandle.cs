using System;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	// Token: 0x02000027 RID: 39
	public class SteamworksEconomyRequestHandle : IEconomyRequestHandle
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000042EA File Offset: 0x000024EA
		// (set) Token: 0x060000EF RID: 239 RVA: 0x000042F2 File Offset: 0x000024F2
		public SteamInventoryResult_t steamInventoryResult { get; protected set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000042FB File Offset: 0x000024FB
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004303 File Offset: 0x00002503
		private EconomyRequestReadyCallback economyRequestReadyCallback { get; set; }

		// Token: 0x060000F2 RID: 242 RVA: 0x0000430C File Offset: 0x0000250C
		public void triggerInventoryRequestReadyCallback(IEconomyRequestResult inventoryRequestResult)
		{
			if (this.economyRequestReadyCallback == null)
			{
				return;
			}
			this.economyRequestReadyCallback(this, inventoryRequestResult);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004324 File Offset: 0x00002524
		public SteamworksEconomyRequestHandle(SteamInventoryResult_t newSteamInventoryResult, EconomyRequestReadyCallback newEconomyRequestReadyCallback)
		{
			this.steamInventoryResult = newSteamInventoryResult;
			this.economyRequestReadyCallback = newEconomyRequestReadyCallback;
		}
	}
}
