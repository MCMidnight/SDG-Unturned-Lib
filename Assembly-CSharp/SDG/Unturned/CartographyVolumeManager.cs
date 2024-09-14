using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004B0 RID: 1200
	public class CartographyVolumeManager : VolumeManager<CartographyVolume, CartographyVolumeManager>
	{
		// Token: 0x06002519 RID: 9497 RVA: 0x00093C7F File Offset: 0x00091E7F
		public CartographyVolume GetMainVolume()
		{
			return this.allVolumes.HeadOrDefault<CartographyVolume>();
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x00093C8C File Offset: 0x00091E8C
		protected override void OnUpdateGizmos(RuntimeGizmos runtimeGizmos)
		{
			base.OnUpdateGizmos(runtimeGizmos);
			foreach (CartographyVolume cartographyVolume in this.allVolumes)
			{
				Color color = cartographyVolume.isSelected ? Color.yellow : this.debugColor;
				runtimeGizmos.Arrow(cartographyVolume.transform.position, cartographyVolume.transform.forward, 1f, color, 0f, EGizmoLayer.World);
			}
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x00093D20 File Offset: 0x00091F20
		public CartographyVolumeManager()
		{
			base.FriendlyName = "Cartography (GPS/Chart)";
			base.SetDebugColor(new Color32(150, 125, 100, byte.MaxValue));
		}
	}
}
