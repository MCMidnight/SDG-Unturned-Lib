using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	// Token: 0x02000804 RID: 2052
	public class Layerer
	{
		// Token: 0x06004656 RID: 18006 RVA: 0x001A3B48 File Offset: 0x001A1D48
		public static void relayer(Transform target, int layer)
		{
			if (target == null)
			{
				return;
			}
			target.gameObject.layer = layer;
			for (int i = 0; i < target.childCount; i++)
			{
				Layerer.relayer(target.GetChild(i), layer);
			}
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x001A3B8C File Offset: 0x001A1D8C
		public static void viewmodel(Transform target)
		{
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
				target.GetComponent<Renderer>().receiveShadows = false;
				target.tag = "Viewmodel";
				target.gameObject.layer = 11;
				return;
			}
			LODGroup component = target.GetComponent<LODGroup>();
			if (component != null)
			{
				foreach (Renderer renderer in new LodGroupEnumerator(component))
				{
					renderer.shadowCastingMode = ShadowCastingMode.Off;
					renderer.receiveShadows = false;
					renderer.gameObject.tag = "Viewmodel";
					renderer.gameObject.layer = 11;
				}
			}
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x001A3C54 File Offset: 0x001A1E54
		public static void enemy(Transform target)
		{
			if (target.GetComponent<Renderer>() != null)
			{
				target.tag = "Enemy";
				target.gameObject.layer = 10;
				return;
			}
			LODGroup component = target.GetComponent<LODGroup>();
			if (component != null)
			{
				foreach (Renderer renderer in new LodGroupEnumerator(component))
				{
					renderer.gameObject.tag = "Enemy";
					renderer.gameObject.layer = 10;
				}
			}
		}
	}
}
