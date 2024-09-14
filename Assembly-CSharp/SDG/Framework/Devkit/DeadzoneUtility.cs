using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000108 RID: 264
	public class DeadzoneUtility
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x00019D2A File Offset: 0x00017F2A
		[Obsolete]
		public static bool isPointInsideVolume(Vector3 point, out DeadzoneVolume volume)
		{
			volume = VolumeManager<DeadzoneVolume, DeadzoneVolumeManager>.Get().GetMostDangerousOverlappingVolume(point);
			return volume != null;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00019D41 File Offset: 0x00017F41
		[Obsolete]
		public static bool isPointInsideVolume(DeadzoneVolume volume, Vector3 point)
		{
			return volume.IsPositionInsideVolume(point);
		}
	}
}
