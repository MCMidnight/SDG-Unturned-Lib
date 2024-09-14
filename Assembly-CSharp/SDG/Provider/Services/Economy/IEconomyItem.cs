using System;
using SDG.Framework.IO.Streams;

namespace SDG.Provider.Services.Economy
{
	// Token: 0x0200005D RID: 93
	public interface IEconomyItem : INetworkStreamable
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600022B RID: 555
		IEconomyItemDefinition itemDefinitionID { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600022C RID: 556
		IEconomyItemInstance itemInstanceID { get; }
	}
}
