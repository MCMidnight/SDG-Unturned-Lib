using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200050E RID: 1294
	public class SafezoneVolumeManager : VolumeManager<SafezoneVolume, SafezoneVolumeManager>
	{
		// Token: 0x06002898 RID: 10392 RVA: 0x000ACE43 File Offset: 0x000AB043
		public SafezoneVolumeManager()
		{
			base.FriendlyName = "Safezone";
			base.SetDebugColor(new Color32(205, 145, 205, byte.MaxValue));
		}
	}
}
