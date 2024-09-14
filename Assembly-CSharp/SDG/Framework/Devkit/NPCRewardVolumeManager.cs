using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000125 RID: 293
	public class NPCRewardVolumeManager : VolumeManager<NPCRewardVolume, NPCRewardVolumeManager>
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x0001B976 File Offset: 0x00019B76
		public NPCRewardVolumeManager()
		{
			base.FriendlyName = "NPC Reward";
			base.SetDebugColor(new Color32(220, 220, 20, byte.MaxValue));
		}
	}
}
