using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	// Token: 0x02000025 RID: 37
	public class SteamworksEconomyItemDefinition : IEconomyItemDefinition, INetworkStreamable
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004236 File Offset: 0x00002436
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000423E File Offset: 0x0000243E
		public SteamItemDef_t steamItemDef { get; protected set; }

		// Token: 0x060000E5 RID: 229 RVA: 0x00004247 File Offset: 0x00002447
		public void readFromStream(NetworkStream networkStream)
		{
			this.steamItemDef = (SteamItemDef_t)networkStream.readInt32();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000425A File Offset: 0x0000245A
		public void writeToStream(NetworkStream networkStream)
		{
			networkStream.writeInt32((int)this.steamItemDef);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004270 File Offset: 0x00002470
		public string getPropertyValue(string key)
		{
			uint num = 1024U;
			string result;
			SteamInventory.GetItemDefinitionProperty(this.steamItemDef, key, out result, ref num);
			return result;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004295 File Offset: 0x00002495
		public SteamworksEconomyItemDefinition(SteamItemDef_t newSteamItemDef)
		{
			this.steamItemDef = newSteamItemDef;
		}
	}
}
