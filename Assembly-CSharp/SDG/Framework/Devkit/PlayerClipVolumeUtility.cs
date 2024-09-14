using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000128 RID: 296
	public class PlayerClipVolumeUtility
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x0001BA98 File Offset: 0x00019C98
		[Obsolete]
		public static bool isPointInsideVolume(Vector3 point)
		{
			return VolumeManager<PlayerClipVolume, PlayerClipVolumeManager>.Get().IsPositionInsideAnyVolume(point);
		}
	}
}
