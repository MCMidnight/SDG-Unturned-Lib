using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200075B RID: 1883
	public class LightLODTool
	{
		// Token: 0x06003DA3 RID: 15779 RVA: 0x001289DC File Offset: 0x00126BDC
		public static void applyLightLOD(Transform transform)
		{
			if (transform == null)
			{
				return;
			}
			LightLODTool.lightsInChildren.Clear();
			transform.GetComponentsInChildren<Light>(true, LightLODTool.lightsInChildren);
			for (int i = 0; i < LightLODTool.lightsInChildren.Count; i++)
			{
				Light light = LightLODTool.lightsInChildren[i];
				if (light.type != LightType.Area && light.type != LightType.Directional)
				{
					light.gameObject.AddComponent<LightLOD>().targetLight = light;
				}
			}
		}

		// Token: 0x040026DB RID: 9947
		private static List<Light> lightsInChildren = new List<Light>();
	}
}
