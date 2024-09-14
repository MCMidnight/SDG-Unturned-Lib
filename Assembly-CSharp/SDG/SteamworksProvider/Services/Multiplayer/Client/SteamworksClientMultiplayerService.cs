using System;
using System.IO;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.SteamworksProvider.Services.Community;

namespace SDG.SteamworksProvider.Services.Multiplayer.Client
{
	// Token: 0x02000020 RID: 32
	public class SteamworksClientMultiplayerService : Service, IClientMultiplayerService, IService
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003FE8 File Offset: 0x000021E8
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003FF0 File Offset: 0x000021F0
		public IServerInfo serverInfo { get; protected set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003FF9 File Offset: 0x000021F9
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004001 File Offset: 0x00002201
		public bool isConnected { get; protected set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000400A File Offset: 0x0000220A
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004012 File Offset: 0x00002212
		public bool isAttempting { get; protected set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000401B File Offset: 0x0000221B
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004023 File Offset: 0x00002223
		public MemoryStream stream { get; protected set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000402C File Offset: 0x0000222C
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004034 File Offset: 0x00002234
		public BinaryReader reader { get; protected set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000403D File Offset: 0x0000223D
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004045 File Offset: 0x00002245
		public BinaryWriter writer { get; protected set; }

		// Token: 0x060000CA RID: 202 RVA: 0x0000404E File Offset: 0x0000224E
		public void connect(IServerInfo newServerInfo)
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004050 File Offset: 0x00002250
		public void disconnect()
		{
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004052 File Offset: 0x00002252
		public bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel)
		{
			entity = SteamworksCommunityEntity.INVALID;
			length = 0UL;
			return false;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004065 File Offset: 0x00002265
		public void write(ICommunityEntity entity, byte[] data, ulong length)
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004067 File Offset: 0x00002267
		public void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel)
		{
		}
	}
}
