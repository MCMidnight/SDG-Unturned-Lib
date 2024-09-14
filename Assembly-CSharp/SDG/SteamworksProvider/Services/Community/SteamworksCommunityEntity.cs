using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Community
{
	// Token: 0x02000029 RID: 41
	public struct SteamworksCommunityEntity : ICommunityEntity, INetworkStreamable
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000045D7 File Offset: 0x000027D7
		public bool isValid
		{
			get
			{
				return this.steamID.IsValid();
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000045E4 File Offset: 0x000027E4
		public void readFromStream(NetworkStream networkStream)
		{
			this.steamID = (CSteamID)networkStream.readUInt64();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000045F7 File Offset: 0x000027F7
		public void writeToStream(NetworkStream networkStream)
		{
			networkStream.writeUInt64((ulong)this.steamID);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000460A File Offset: 0x0000280A
		public SteamworksCommunityEntity(CSteamID newSteamID)
		{
			this.steamID = newSteamID;
		}

		// Token: 0x0400006C RID: 108
		public static readonly SteamworksCommunityEntity INVALID = new SteamworksCommunityEntity(CSteamID.Nil);

		// Token: 0x0400006D RID: 109
		public CSteamID steamID;
	}
}
