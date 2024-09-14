using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004AB RID: 1195
	public class ArenaCompactorVolumeManager : VolumeManager<ArenaCompactorVolume, ArenaCompactorVolumeManager>
	{
		// Token: 0x0600250A RID: 9482 RVA: 0x00093A27 File Offset: 0x00091C27
		public ArenaCompactorVolumeManager()
		{
			base.FriendlyName = "Arena Mode Compactor";
			base.SetDebugColor(new Color32(20, 20, 20, byte.MaxValue));
		}
	}
}
