using System;
using SDG.Framework.IO.Streams;

namespace SDG.Provider.Services.Economy
{
	// Token: 0x0200005E RID: 94
	public interface IEconomyItemDefinition : INetworkStreamable
	{
		// Token: 0x0600022D RID: 557
		string getPropertyValue(string key);
	}
}
