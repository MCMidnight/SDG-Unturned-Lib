using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200011C RID: 284
	public class KillVolumeManager : VolumeManager<KillVolume, KillVolumeManager>
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x0001B15A File Offset: 0x0001935A
		public KillVolumeManager()
		{
			base.FriendlyName = "Kill";
			base.SetDebugColor(new Color32(220, 100, 20, byte.MaxValue));
		}
	}
}
