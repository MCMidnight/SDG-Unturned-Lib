using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A4 RID: 164
	public class LandscapeHoleUtility
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x00011260 File Offset: 0x0000F460
		[Obsolete("New code should probably be using Landscape.IsPointInsideHole")]
		public static bool isPointInsideHoleVolume(Vector3 point)
		{
			LandscapeHoleVolume landscapeHoleVolume;
			return LandscapeHoleUtility.isPointInsideHoleVolume(point, out landscapeHoleVolume);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00011278 File Offset: 0x0000F478
		[Obsolete("New code should probably be using Landscape.IsPointInsideHole")]
		public static bool isPointInsideHoleVolume(Vector3 point, out LandscapeHoleVolume volume)
		{
			List<LandscapeHoleVolume> list = VolumeManager<LandscapeHoleVolume, LandscapeHoleVolumeManager>.Get().InternalGetAllVolumes();
			for (int i = 0; i < list.Count; i++)
			{
				volume = list[i];
				if (LandscapeHoleUtility.isPointInsideHoleVolume(volume, point))
				{
					return true;
				}
			}
			volume = null;
			return false;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000112BA File Offset: 0x0000F4BA
		[Obsolete]
		public static bool isPointInsideHoleVolume(LandscapeHoleVolume volume, Vector3 point)
		{
			return volume.IsPositionInsideVolume(point);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000112C3 File Offset: 0x0000F4C3
		[Obsolete]
		public static bool doesRayIntersectHoleVolume(Ray ray, out RaycastHit hit, out LandscapeHoleVolume volume, float maxDistance)
		{
			return VolumeManager<LandscapeHoleVolume, LandscapeHoleVolumeManager>.Get().Raycast(ray, out hit, out volume, maxDistance);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000112D3 File Offset: 0x0000F4D3
		[Obsolete]
		public static bool doesRayIntersectHoleVolume(LandscapeHoleVolume volume, Ray ray, out RaycastHit hit, float maxDistance)
		{
			return volume.volumeCollider.Raycast(ray, out hit, maxDistance);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000112E4 File Offset: 0x0000F4E4
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool shouldRaycastIgnoreLandscape(Ray ray, float maxDistance)
		{
			RaycastHit raycastHit;
			LandscapeHoleVolume volume;
			RaycastHit raycastHit2;
			RaycastHit raycastHit3;
			return (LandscapeHoleUtility.doesRayIntersectHoleVolume(ray, out raycastHit, out volume, maxDistance) && Physics.Raycast(ray, out raycastHit2, maxDistance, 1048576) && LandscapeHoleUtility.isPointInsideHoleVolume(volume, raycastHit2.point)) || (LandscapeHoleUtility.isPointInsideHoleVolume(ray.origin, out volume) && Physics.Raycast(ray, out raycastHit3, maxDistance, 1048576) && LandscapeHoleUtility.isPointInsideHoleVolume(volume, raycastHit3.point));
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00011351 File Offset: 0x0000F551
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool shouldSpherecastIgnoreLandscape(Ray ray, float radius, float maxDistance)
		{
			ray.origin -= ray.direction * radius;
			maxDistance += radius * 2f;
			return LandscapeHoleUtility.shouldRaycastIgnoreLandscape(ray, maxDistance);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00011384 File Offset: 0x0000F584
		[Obsolete("Hole collision is handled by Unity now")]
		public static void raycastIgnoreLandscapeIfNecessary(Ray ray, float maxDistance, ref int layerMask)
		{
			if (LandscapeHoleUtility.shouldRaycastIgnoreLandscape(ray, maxDistance))
			{
				layerMask &= -1048577;
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00011399 File Offset: 0x0000F599
		[Obsolete("Hole collision is handled by Unity now")]
		public static void spherecastIgnoreLandscapeIfNecessary(Ray ray, float radius, float maxDistance, ref int layerMask)
		{
			if (LandscapeHoleUtility.shouldSpherecastIgnoreLandscape(ray, radius, maxDistance))
			{
				layerMask &= -1048577;
			}
		}
	}
}
