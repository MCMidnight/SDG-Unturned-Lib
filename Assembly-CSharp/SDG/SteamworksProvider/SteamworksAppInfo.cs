using System;

namespace SDG.SteamworksProvider
{
	// Token: 0x02000014 RID: 20
	public class SteamworksAppInfo
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000031FC File Offset: 0x000013FC
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00003204 File Offset: 0x00001404
		public uint id { get; protected set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000320D File Offset: 0x0000140D
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00003215 File Offset: 0x00001415
		public string name { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000321E File Offset: 0x0000141E
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00003226 File Offset: 0x00001426
		public string version { get; protected set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000322F File Offset: 0x0000142F
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00003237 File Offset: 0x00001437
		public bool isDedicated { get; protected set; }

		// Token: 0x0600004F RID: 79 RVA: 0x00003240 File Offset: 0x00001440
		public SteamworksAppInfo(uint newID, string newName, string newVersion, bool newIsDedicated)
		{
			this.id = newID;
			this.name = newName;
			this.version = newVersion;
			this.isDedicated = newIsDedicated;
		}
	}
}
