using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200035E RID: 862
	internal static class PrefabUtil
	{
		// Token: 0x06001A09 RID: 6665 RVA: 0x0005DAC8 File Offset: 0x0005BCC8
		public static void DestroyCollidersInChildren(GameObject gameObject, bool includeInactive)
		{
			gameObject.GetComponentsInChildren<Collider>(includeInactive, PrefabUtil.workingColliders);
			foreach (Collider obj in PrefabUtil.workingColliders)
			{
				Object.Destroy(obj);
			}
			PrefabUtil.workingColliders.Clear();
		}

		// Token: 0x04000BE7 RID: 3047
		private static List<Collider> workingColliders = new List<Collider>();
	}
}
