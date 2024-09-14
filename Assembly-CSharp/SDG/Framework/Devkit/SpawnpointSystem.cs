using System;
using System.Collections.Generic;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200012B RID: 299
	[Obsolete("Made SpawnpointSystem no longer static")]
	public static class SpawnpointSystem
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001BEDC File Offset: 0x0001A0DC
		[Obsolete("Made SpawnpointSystem no longer static")]
		public static List<Spawnpoint> spawnpoints
		{
			get
			{
				return SpawnpointSystemV2.Get().spawnpoints;
			}
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001BEE8 File Offset: 0x0001A0E8
		[Obsolete("Made SpawnpointSystem no longer static")]
		public static Spawnpoint getSpawnpoint(string id)
		{
			return SpawnpointSystemV2.Get().FindSpawnpoint(id);
		}
	}
}
