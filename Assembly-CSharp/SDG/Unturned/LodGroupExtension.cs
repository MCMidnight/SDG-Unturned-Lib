using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000809 RID: 2057
	public static class LodGroupExtension
	{
		/// <summary>
		/// Lod group will be culled when screen size is smaller than this value.
		/// </summary>
		// Token: 0x0600466F RID: 18031 RVA: 0x001A3F6F File Offset: 0x001A216F
		public static float GetCullingScreenSize(this LODGroup lodGroup)
		{
			LOD[] lods = lodGroup.GetLODs();
			return lods[lods.Length - 1].screenRelativeTransitionHeight;
		}

		/// <summary>
		/// Clamp the culling screen percentage to be less than or equal to a maximum value.
		/// </summary>
		// Token: 0x06004670 RID: 18032 RVA: 0x001A3F88 File Offset: 0x001A2188
		public static void ClampCulling(this LODGroup lodGroup, float max)
		{
			LOD[] lods = lodGroup.GetLODs();
			int num = lods.Length - 1;
			if (num <= lods.Length && lods[num].screenRelativeTransitionHeight > max)
			{
				lods[num].screenRelativeTransitionHeight = max;
				lodGroup.SetLODs(lods);
			}
		}

		/// <summary>
		/// Prevent the lowest LOD from being culled.
		/// </summary>
		// Token: 0x06004671 RID: 18033 RVA: 0x001A3FCB File Offset: 0x001A21CB
		public static void DisableCulling(this LODGroup lodGroup)
		{
			lodGroup.ClampCulling(0f);
		}
	}
}
