using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	// Token: 0x02000024 RID: 36
	public class SteamworksEconomyItem : IEconomyItem, INetworkStreamable
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004194 File Offset: 0x00002394
		// (set) Token: 0x060000DB RID: 219 RVA: 0x0000419C File Offset: 0x0000239C
		public SteamItemDetails_t steamItemDetail { get; protected set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000041A5 File Offset: 0x000023A5
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000041AD File Offset: 0x000023AD
		public IEconomyItemDefinition itemDefinitionID { get; protected set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000041B6 File Offset: 0x000023B6
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000041BE File Offset: 0x000023BE
		public IEconomyItemInstance itemInstanceID { get; protected set; }

		// Token: 0x060000E0 RID: 224 RVA: 0x000041C7 File Offset: 0x000023C7
		public void readFromStream(NetworkStream networkStream)
		{
			this.itemDefinitionID.readFromStream(networkStream);
			this.itemInstanceID.readFromStream(networkStream);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000041E1 File Offset: 0x000023E1
		public void writeToStream(NetworkStream networkStream)
		{
			this.itemDefinitionID.writeToStream(networkStream);
			this.itemInstanceID.writeToStream(networkStream);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000041FB File Offset: 0x000023FB
		public SteamworksEconomyItem(SteamItemDetails_t newSteamItemDetail)
		{
			this.steamItemDetail = newSteamItemDetail;
			this.itemDefinitionID = new SteamworksEconomyItemDefinition(this.steamItemDetail.m_iDefinition);
			this.itemInstanceID = new SteamworksEconomyItemInstance(this.steamItemDetail.m_itemId);
		}
	}
}
