using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000105 RID: 261
	public class AmbianceVolumeManager : VolumeManager<AmbianceVolume, AmbianceVolumeManager>
	{
		// Token: 0x060006AD RID: 1709 RVA: 0x000199FD File Offset: 0x00017BFD
		public AmbianceVolumeManager()
		{
			base.FriendlyName = "Ambiance";
			base.SetDebugColor(new Color32(0, 127, 127, byte.MaxValue));
		}
	}
}
