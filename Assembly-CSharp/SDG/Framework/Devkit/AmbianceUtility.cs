using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000106 RID: 262
	public class AmbianceUtility
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x00019A2A File Offset: 0x00017C2A
		[Obsolete]
		public static bool isPointInsideVolume(Vector3 point, out AmbianceVolume volume)
		{
			volume = VolumeManager<AmbianceVolume, AmbianceVolumeManager>.Get().GetFirstOverlappingVolume(point);
			return volume != null;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00019A41 File Offset: 0x00017C41
		[Obsolete]
		public static bool isPointInsideVolume(AmbianceVolume volume, Vector3 point)
		{
			return volume.IsPositionInsideVolume(point);
		}
	}
}
