using System;

namespace SDG.Provider.Services.Economy
{
	// Token: 0x0200005B RID: 91
	public class EconomyRequestResult : IEconomyRequestResult
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000938A File Offset: 0x0000758A
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00009392 File Offset: 0x00007592
		public EEconomyRequestState economyRequestState { get; protected set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000939B File Offset: 0x0000759B
		// (set) Token: 0x06000229 RID: 553 RVA: 0x000093A3 File Offset: 0x000075A3
		public IEconomyItem[] items { get; protected set; }

		// Token: 0x0600022A RID: 554 RVA: 0x000093AC File Offset: 0x000075AC
		public EconomyRequestResult(EEconomyRequestState newEconomyRequestState, IEconomyItem[] newItems)
		{
			this.economyRequestState = newEconomyRequestState;
			this.items = newItems;
		}
	}
}
