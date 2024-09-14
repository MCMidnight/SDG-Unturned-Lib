using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004CB RID: 1227
	public class HordePurchaseVolumeManager : VolumeManager<HordePurchaseVolume, HordePurchaseVolumeManager>
	{
		// Token: 0x0600258A RID: 9610 RVA: 0x0009553F File Offset: 0x0009373F
		public HordePurchaseVolumeManager()
		{
			base.FriendlyName = "Horde Purchase";
			base.SetDebugColor(new Color32(20, 50, 20, byte.MaxValue));
		}
	}
}
