using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000117 RID: 279
	public class EffectVolumeManager : VolumeManager<EffectVolume, EffectVolumeManager>
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x0001ADFE File Offset: 0x00018FFE
		public EffectVolumeManager()
		{
			base.FriendlyName = "Effect";
			base.SetDebugColor(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		}
	}
}
