using System;
using System.Collections.Generic;
using SDG.Unturned;

namespace SDG.Framework.Foliage
{
	// Token: 0x02000101 RID: 257
	public static class FoliageVolumeSystem
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000199B7 File Offset: 0x00017BB7
		[Obsolete]
		public static List<FoliageVolume> additiveVolumes
		{
			get
			{
				return VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().additiveVolumes;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000199C3 File Offset: 0x00017BC3
		[Obsolete]
		public static List<FoliageVolume> subtractiveVolumes
		{
			get
			{
				return VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().subtractiveVolumes;
			}
		}
	}
}
