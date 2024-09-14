using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Water
{
	// Token: 0x0200007A RID: 122
	public class WaterVolumeManager : VolumeManager<WaterVolume, WaterVolumeManager>
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000BF42 File Offset: 0x0000A142
		public static float worldSeaLevel
		{
			get
			{
				if (WaterVolumeManager.seaLevelVolume != null)
				{
					return WaterVolumeManager.seaLevelVolume.transform.TransformPoint(0f, 0.5f, 0f).y;
				}
				return -1024f;
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000BF7A File Offset: 0x0000A17A
		public WaterVolumeManager()
		{
			base.FriendlyName = "Water";
			base.SetDebugColor(new Color32(50, 200, 200, byte.MaxValue));
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000BFB0 File Offset: 0x0000A1B0
		private void OnGraphicsSettingsApplied()
		{
			EGraphicQuality waterQuality = GraphicsSettings.waterQuality;
			if (this.oldWaterQuality != waterQuality)
			{
				this.oldWaterQuality = waterQuality;
				foreach (WaterVolume waterVolume in this.allVolumes)
				{
					waterVolume.SyncWaterQuality();
				}
				bool flag = waterQuality == EGraphicQuality.ULTRA;
				if (this.wasPlanarReflectionEnabled != flag)
				{
					this.wasPlanarReflectionEnabled = flag;
					foreach (WaterVolume waterVolume2 in this.allVolumes)
					{
						waterVolume2.SyncPlanarReflectionEnabled();
					}
				}
			}
		}

		/// <summary>
		/// Water volume marked as being sea level.
		/// </summary>
		// Token: 0x0400014E RID: 334
		public static WaterVolume seaLevelVolume;

		// Token: 0x0400014F RID: 335
		private EGraphicQuality oldWaterQuality;

		// Token: 0x04000150 RID: 336
		private bool wasPlanarReflectionEnabled;
	}
}
