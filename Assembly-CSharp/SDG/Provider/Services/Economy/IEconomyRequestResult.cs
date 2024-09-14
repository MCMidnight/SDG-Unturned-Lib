using System;

namespace SDG.Provider.Services.Economy
{
	// Token: 0x02000061 RID: 97
	public interface IEconomyRequestResult
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600022E RID: 558
		EEconomyRequestState economyRequestState { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600022F RID: 559
		IEconomyItem[] items { get; }
	}
}
