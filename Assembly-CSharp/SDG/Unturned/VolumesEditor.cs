using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200041E RID: 1054
	public class VolumesEditor : SelectionTool
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x000794D1 File Offset: 0x000776D1
		// (set) Token: 0x06001F56 RID: 8022 RVA: 0x000794D9 File Offset: 0x000776D9
		public VolumeManagerBase activeVolumeManager
		{
			get
			{
				return this._activeVolumeManager;
			}
			set
			{
				DevkitSelectionManager.clear();
				this._activeVolumeManager = value;
			}
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000794E7 File Offset: 0x000776E7
		protected override bool RaycastSelectableObjects(Ray ray, out RaycastHit hitInfo)
		{
			if (this.activeVolumeManager != null)
			{
				return this.activeVolumeManager.Raycast(ray, out hitInfo, 8192f);
			}
			hitInfo = default(RaycastHit);
			return false;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0007950C File Offset: 0x0007770C
		protected override void RequestInstantiation(Vector3 position)
		{
			if (this.activeVolumeManager != null)
			{
				this.activeVolumeManager.InstantiateVolume(position, Quaternion.identity, Vector3.one);
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0007952C File Offset: 0x0007772C
		protected override bool HasBoxSelectableObjects()
		{
			return this.activeVolumeManager != null;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x00079537 File Offset: 0x00077737
		protected override IEnumerable<GameObject> EnumerateBoxSelectableObjects()
		{
			if (this.activeVolumeManager == null)
			{
				yield break;
			}
			foreach (VolumeBase volumeBase in this.activeVolumeManager.EnumerateAllVolumes())
			{
				if (volumeBase.CanBeSelected)
				{
					yield return volumeBase.areaSelectGameObject;
				}
			}
			IEnumerator<VolumeBase> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000F9D RID: 3997
		private VolumeManagerBase _activeVolumeManager;
	}
}
