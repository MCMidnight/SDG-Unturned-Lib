using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000131 RID: 305
	public class UndergroundWhitelistVolumeManager : VolumeManager<UndergroundWhitelistVolume, UndergroundWhitelistVolumeManager>
	{
		// Token: 0x060007BE RID: 1982 RVA: 0x0001C352 File Offset: 0x0001A552
		public UndergroundWhitelistVolumeManager()
		{
			base.FriendlyName = "Underground Whitelist";
			base.SetDebugColor(new Color32(63, 63, 63, byte.MaxValue));
		}
	}
}
