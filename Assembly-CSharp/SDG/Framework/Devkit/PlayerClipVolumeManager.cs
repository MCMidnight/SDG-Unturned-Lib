using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000127 RID: 295
	public class PlayerClipVolumeManager : VolumeManager<PlayerClipVolume, PlayerClipVolumeManager>
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x0001BA6C File Offset: 0x00019C6C
		public PlayerClipVolumeManager()
		{
			base.FriendlyName = "Player Clip";
			base.SetDebugColor(new Color32(63, 0, 0, byte.MaxValue));
		}
	}
}
