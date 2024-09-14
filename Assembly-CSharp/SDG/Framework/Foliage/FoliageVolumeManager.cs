using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x02000100 RID: 256
	public class FoliageVolumeManager : VolumeManager<FoliageVolume, FoliageVolumeManager>
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x000197E8 File Offset: 0x000179E8
		public bool IsTileBakeable(FoliageTile tile)
		{
			if (this.additiveVolumes.Count > 0)
			{
				Vector3 center = tile.worldBounds.center;
				for (int i = 0; i < this.additiveVolumes.Count; i++)
				{
					if (this.additiveVolumes[i].IsPositionInsideVolume(center))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00019844 File Offset: 0x00017A44
		public bool IsPositionBakeable(Vector3 point, bool instancedMeshes, bool resources, bool objects)
		{
			bool flag;
			if (this.additiveVolumes.Count > 0)
			{
				flag = false;
				for (int i = 0; i < this.additiveVolumes.Count; i++)
				{
					FoliageVolume foliageVolume = this.additiveVolumes[i];
					if ((!instancedMeshes || foliageVolume.instancedMeshes) && (!resources || foliageVolume.resources) && (!objects || foliageVolume.objects) && foliageVolume.IsPositionInsideVolume(point))
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			for (int j = 0; j < this.subtractiveVolumes.Count; j++)
			{
				FoliageVolume foliageVolume2 = this.subtractiveVolumes[j];
				if ((!instancedMeshes || foliageVolume2.instancedMeshes) && (!resources || foliageVolume2.resources) && (!objects || foliageVolume2.objects) && foliageVolume2.IsPositionInsideVolume(point))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00019912 File Offset: 0x00017B12
		public override void AddVolume(FoliageVolume volume)
		{
			base.AddVolume(volume);
			if (volume.mode == FoliageVolume.EFoliageVolumeMode.ADDITIVE)
			{
				this.additiveVolumes.Add(volume);
				return;
			}
			this.subtractiveVolumes.Add(volume);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001993C File Offset: 0x00017B3C
		public override void RemoveVolume(FoliageVolume volume)
		{
			base.RemoveVolume(volume);
			if (volume.mode == FoliageVolume.EFoliageVolumeMode.ADDITIVE)
			{
				this.additiveVolumes.RemoveFast(volume);
				return;
			}
			this.subtractiveVolumes.RemoveFast(volume);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00019968 File Offset: 0x00017B68
		public FoliageVolumeManager()
		{
			base.FriendlyName = "Foliage";
			this.additiveVolumes = new List<FoliageVolume>();
			this.subtractiveVolumes = new List<FoliageVolume>();
			base.SetDebugColor(new Color32(44, 114, 34, byte.MaxValue));
		}

		// Token: 0x04000284 RID: 644
		internal List<FoliageVolume> additiveVolumes;

		// Token: 0x04000285 RID: 645
		internal List<FoliageVolume> subtractiveVolumes;
	}
}
