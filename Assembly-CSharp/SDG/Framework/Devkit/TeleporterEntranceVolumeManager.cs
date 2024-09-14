using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200012E RID: 302
	public class TeleporterEntranceVolumeManager : VolumeManager<TeleporterEntranceVolume, TeleporterEntranceVolumeManager>
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x0001C03C File Offset: 0x0001A23C
		protected override void OnUpdateGizmos(RuntimeGizmos runtimeGizmos)
		{
			base.OnUpdateGizmos(runtimeGizmos);
			foreach (TeleporterEntranceVolume teleporterEntranceVolume in this.allVolumes)
			{
				Color color = teleporterEntranceVolume.isSelected ? Color.yellow : this.debugColor;
				runtimeGizmos.Arrow(teleporterEntranceVolume.transform.position, teleporterEntranceVolume.transform.forward, 1f, color, 0f, EGizmoLayer.World);
				List<TeleporterExitVolume> list;
				if (!string.IsNullOrEmpty(teleporterEntranceVolume.pairId) && VolumeManager<TeleporterExitVolume, TeleporterExitVolumeManager>.Get().idToVolumes.TryGetValue(teleporterEntranceVolume.pairId, ref list))
				{
					foreach (TeleporterExitVolume teleporterExitVolume in list)
					{
						runtimeGizmos.Line(teleporterEntranceVolume.transform.position, teleporterExitVolume.transform.position, color, 0f, EGizmoLayer.World);
					}
				}
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001C158 File Offset: 0x0001A358
		public TeleporterEntranceVolumeManager()
		{
			base.FriendlyName = "Teleporter Entrance";
			base.SetDebugColor(new Color32(0, 0, byte.MaxValue, byte.MaxValue));
		}
	}
}
