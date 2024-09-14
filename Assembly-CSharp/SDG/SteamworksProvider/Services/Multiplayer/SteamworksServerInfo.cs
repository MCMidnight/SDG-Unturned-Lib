using System;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Multiplayer
{
	// Token: 0x0200001E RID: 30
	public class SteamworksServerInfo : IServerInfo
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003B1B File Offset: 0x00001D1B
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003B23 File Offset: 0x00001D23
		public ICommunityEntity entity { get; protected set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003B2C File Offset: 0x00001D2C
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003B34 File Offset: 0x00001D34
		public string name { get; protected set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003B3D File Offset: 0x00001D3D
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003B45 File Offset: 0x00001D45
		public byte players { get; protected set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003B4E File Offset: 0x00001D4E
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003B56 File Offset: 0x00001D56
		public byte capacity { get; protected set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003B5F File Offset: 0x00001D5F
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00003B67 File Offset: 0x00001D67
		public int ping { get; protected set; }

		// Token: 0x060000A6 RID: 166 RVA: 0x00003B70 File Offset: 0x00001D70
		public SteamworksServerInfo(gameserveritem_t server)
		{
			this.entity = new SteamworksCommunityEntity(server.m_steamID);
			this.name = server.GetServerName();
			this.players = (byte)server.m_nPlayers;
			this.capacity = (byte)server.m_nMaxPlayers;
			this.ping = server.m_nPing;
		}
	}
}
