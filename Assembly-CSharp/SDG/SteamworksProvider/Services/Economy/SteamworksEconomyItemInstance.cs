using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	// Token: 0x02000026 RID: 38
	public class SteamworksEconomyItemInstance : IEconomyItemInstance, INetworkStreamable
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000042A4 File Offset: 0x000024A4
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000042AC File Offset: 0x000024AC
		public SteamItemInstanceID_t steamItemInstanceID { get; protected set; }

		// Token: 0x060000EB RID: 235 RVA: 0x000042B5 File Offset: 0x000024B5
		public void readFromStream(NetworkStream networkStream)
		{
			this.steamItemInstanceID = (SteamItemInstanceID_t)networkStream.readUInt64();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000042C8 File Offset: 0x000024C8
		public void writeToStream(NetworkStream networkStream)
		{
			networkStream.writeUInt64((ulong)this.steamItemInstanceID);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000042DB File Offset: 0x000024DB
		public SteamworksEconomyItemInstance(SteamItemInstanceID_t newSteamItemInstanceID)
		{
			this.steamItemInstanceID = newSteamItemInstanceID;
		}
	}
}
