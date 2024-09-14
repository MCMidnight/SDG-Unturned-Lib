using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Water
{
	// Token: 0x02000078 RID: 120
	public class WaterUtility
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000BA09 File Offset: 0x00009C09
		[Obsolete]
		public static bool isPointInsideVolume(Vector3 point)
		{
			return VolumeManager<WaterVolume, WaterVolumeManager>.Get().IsPositionInsideAnyVolume(point);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000BA16 File Offset: 0x00009C16
		[Obsolete]
		public static bool isPointInsideVolume(Vector3 point, out WaterVolume volume)
		{
			volume = VolumeManager<WaterVolume, WaterVolumeManager>.Get().GetFirstOverlappingVolume(point);
			return volume != null;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000BA30 File Offset: 0x00009C30
		public static float getWaterSurfaceElevation(WaterVolume volume, Vector3 point)
		{
			point.y += 1024f;
			Ray ray = new Ray(point, new Vector3(0f, -1f, 0f));
			RaycastHit raycastHit;
			if (volume.volumeCollider.Raycast(ray, out raycastHit, 2048f))
			{
				return raycastHit.point.y;
			}
			return 0f;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000BA90 File Offset: 0x00009C90
		[Obsolete]
		public static bool isPointInsideVolume(WaterVolume volume, Vector3 point)
		{
			return volume.IsPositionInsideVolume(point);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000BA99 File Offset: 0x00009C99
		public static bool isPointUnderwater(Vector3 point)
		{
			return VolumeManager<WaterVolume, WaterVolumeManager>.Get().IsPositionInsideAnyVolume(point);
		}

		/// <param name="volume">Null if under old water level, otherwise the volume.</param>
		// Token: 0x060002E1 RID: 737 RVA: 0x0000BAA6 File Offset: 0x00009CA6
		public static bool isPointUnderwater(Vector3 point, out WaterVolume volume)
		{
			volume = VolumeManager<WaterVolume, WaterVolumeManager>.Get().GetFirstOverlappingVolume(point);
			return volume != null;
		}

		/// <summary>
		/// Find the water elevation underneath point, or above point if underwater.
		/// </summary>
		// Token: 0x060002E2 RID: 738 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		public static float getWaterSurfaceElevation(Vector3 point)
		{
			bool flag = false;
			float num = -1024f;
			foreach (WaterVolume waterVolume in VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes())
			{
				if (waterVolume.IsPositionInsideVolume(point))
				{
					return WaterUtility.getWaterSurfaceElevation(waterVolume, point);
				}
				Ray ray = new Ray(point, new Vector3(0f, -1f, 0f));
				RaycastHit raycastHit;
				if (waterVolume.volumeCollider.Raycast(ray, out raycastHit, 2048f) && raycastHit.point.y > num)
				{
					num = raycastHit.point.y;
					flag = true;
				}
			}
			if (flag)
			{
				return num;
			}
			return -1024f;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000BB8C File Offset: 0x00009D8C
		public static void getUnderwaterInfo(Vector3 point, out bool isUnderwater, out float surfaceElevation)
		{
			WaterVolume firstOverlappingVolume = VolumeManager<WaterVolume, WaterVolumeManager>.Get().GetFirstOverlappingVolume(point);
			if (firstOverlappingVolume != null)
			{
				isUnderwater = true;
				surfaceElevation = WaterUtility.getWaterSurfaceElevation(firstOverlappingVolume, point);
				return;
			}
			isUnderwater = false;
			surfaceElevation = -1024f;
		}
	}
}
