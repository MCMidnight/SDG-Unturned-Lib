using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200012F RID: 303
	public class TeleporterExitVolumeManager : VolumeManager<TeleporterExitVolume, TeleporterExitVolumeManager>
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x0001C188 File Offset: 0x0001A388
		public TeleporterExitVolume FindExitVolume(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			List<TeleporterExitVolume> list;
			if (!this.idToVolumes.TryGetValue(id, ref list))
			{
				return null;
			}
			return list.RandomOrDefault<TeleporterExitVolume>();
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001C1B7 File Offset: 0x0001A3B7
		public override void AddVolume(TeleporterExitVolume volume)
		{
			base.AddVolume(volume);
			this.AddVolumeToIdDictionary(volume);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001C1C7 File Offset: 0x0001A3C7
		public override void RemoveVolume(TeleporterExitVolume volume)
		{
			this.RemoveVolumeFromIdDictionary(volume);
			base.RemoveVolume(volume);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001C1D8 File Offset: 0x0001A3D8
		internal void AddVolumeToIdDictionary(TeleporterExitVolume volume)
		{
			if (string.IsNullOrEmpty(volume.id))
			{
				return;
			}
			List<TeleporterExitVolume> list;
			if (!this.idToVolumes.TryGetValue(volume.id, ref list))
			{
				list = new List<TeleporterExitVolume>();
				this.idToVolumes.Add(volume.id, list);
			}
			list.Add(volume);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001C228 File Offset: 0x0001A428
		internal void RemoveVolumeFromIdDictionary(TeleporterExitVolume volume)
		{
			if (string.IsNullOrEmpty(volume.id))
			{
				return;
			}
			List<TeleporterExitVolume> list;
			if (this.idToVolumes.TryGetValue(volume.id, ref list))
			{
				list.RemoveFast(volume);
				if (list.Count < 1)
				{
					this.idToVolumes.Remove(volume.id);
				}
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001C27C File Offset: 0x0001A47C
		protected override void OnUpdateGizmos(RuntimeGizmos runtimeGizmos)
		{
			base.OnUpdateGizmos(runtimeGizmos);
			foreach (TeleporterExitVolume teleporterExitVolume in this.allVolumes)
			{
				Color color = teleporterExitVolume.isSelected ? Color.yellow : this.debugColor;
				runtimeGizmos.Arrow(teleporterExitVolume.transform.position, teleporterExitVolume.transform.forward, 1f, color, 0f, EGizmoLayer.World);
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001C310 File Offset: 0x0001A510
		public TeleporterExitVolumeManager()
		{
			base.FriendlyName = "Teleporter Exit";
			base.SetDebugColor(new Color32(0, 0, byte.MaxValue, byte.MaxValue));
		}

		// Token: 0x040002CC RID: 716
		internal Dictionary<string, List<TeleporterExitVolume>> idToVolumes = new Dictionary<string, List<TeleporterExitVolume>>();
	}
}
