using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x02000102 RID: 258
	public class FoliageVolumeUtility
	{
		// Token: 0x0600069F RID: 1695 RVA: 0x000199CF File Offset: 0x00017BCF
		[Obsolete]
		public static bool isTileBakeable(FoliageTile tile)
		{
			return VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsTileBakeable(tile);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000199DC File Offset: 0x00017BDC
		[Obsolete]
		public static bool isPointValid(Vector3 point, bool instancedMeshes, bool resources, bool objects)
		{
			return VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsPositionBakeable(point, instancedMeshes, resources, objects);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x000199EC File Offset: 0x00017BEC
		[Obsolete]
		public static bool isPointInsideVolume(FoliageVolume volume, Vector3 point)
		{
			return volume.IsPositionInsideVolume(point);
		}
	}
}
