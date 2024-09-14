using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000123 RID: 291
	public class NavClipVolumeManager : VolumeManager<NavClipVolume, NavClipVolumeManager>
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x0001B82C File Offset: 0x00019A2C
		public NavClipVolumeManager()
		{
			base.FriendlyName = "Navmesh Clip";
			base.SetDebugColor(new Color32(63, 63, 0, byte.MaxValue));
		}
	}
}
