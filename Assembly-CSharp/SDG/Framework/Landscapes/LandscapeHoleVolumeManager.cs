using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A6 RID: 166
	public class LandscapeHoleVolumeManager : VolumeManager<LandscapeHoleVolume, LandscapeHoleVolumeManager>
	{
		/// <summary>
		/// Called by loading after landscapes (and legacy conversion) have been loaded.
		/// </summary>
		// Token: 0x0600044A RID: 1098 RVA: 0x000113D0 File Offset: 0x0000F5D0
		public void ApplyToTerrain()
		{
			this.modifiedTiles.Clear();
			this.holeModifications.Clear();
			if (this.allVolumes.Count > 0)
			{
				this.ConvertHoleVolumesToModifications();
				UnturnedLog.info(string.Format("Applied {0} hole volume(s) to {1} terrain tile(s)", this.allVolumes.Count, this.modifiedTiles.Count));
			}
			if (Level.isEditor && !this.isListeningForUpdates)
			{
				this.isListeningForUpdates = true;
				this.ignoreHolesChanged = true;
				TimeUtility.updated += this.OnUpdateHoles;
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00011464 File Offset: 0x0000F664
		public LandscapeHoleVolumeManager()
		{
			base.FriendlyName = "Landscape Hole (legacy do NOT use!)";
			base.SetDebugColor(new Color32(71, 44, 20, byte.MaxValue));
			this.allowInstantiation = false;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000114BC File Offset: 0x0000F6BC
		private void OnUpdateHoles()
		{
			if (!Level.isEditor)
			{
				if (this.isListeningForUpdates)
				{
					this.isListeningForUpdates = false;
					TimeUtility.updated -= this.OnUpdateHoles;
				}
				return;
			}
			if (this.allVolumes.Count < 1)
			{
				return;
			}
			bool flag = false;
			foreach (LandscapeHoleVolume landscapeHoleVolume in this.allVolumes)
			{
				flag |= landscapeHoleVolume.transform.hasChanged;
				landscapeHoleVolume.transform.hasChanged = false;
			}
			if (this.ignoreHolesChanged)
			{
				this.ignoreHolesChanged = false;
				return;
			}
			if (!flag)
			{
				return;
			}
			this.modifiedTiles.Clear();
			this.UndoHoleModifications();
			this.ConvertHoleVolumesToModifications();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00011588 File Offset: 0x0000F788
		private void UndoHoleModifications()
		{
			foreach (LandscapeHoleVolumeManager.HoleModification holeModification in this.holeModifications)
			{
				if (holeModification.tile != null)
				{
					this.modifiedTiles.Add(holeModification.tile);
					holeModification.tile.holes[holeModification.splatmapCoord.x, holeModification.splatmapCoord.y] = true;
				}
			}
			this.holeModifications.Clear();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00011620 File Offset: 0x0000F820
		private void ConvertHoleVolumesToModifications()
		{
			foreach (LandscapeHoleVolume landscapeHoleVolume in this.allVolumes)
			{
				Bounds worldBounds = landscapeHoleVolume.CalculateWorldBounds();
				LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
				for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
				{
					for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
					{
						LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
						LandscapeTile tile = Landscape.getTile(landscapeCoord);
						if (tile != null)
						{
							this.modifiedTiles.Add(tile);
							SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
							for (int k = splatmapBounds.min.x; k <= splatmapBounds.max.x; k++)
							{
								for (int l = splatmapBounds.min.y; l <= splatmapBounds.max.y; l++)
								{
									SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
									Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
									Vector3 vector = landscapeHoleVolume.transform.InverseTransformPoint(worldPosition);
									Vector3 vector2 = new Vector3(2.828427f, 2.828427f, 2.828427f);
									vector2.x = Mathf.Abs(vector2.x / landscapeHoleVolume.transform.localScale.x);
									vector2.y = Mathf.Abs(vector2.y / landscapeHoleVolume.transform.localScale.y);
									vector2.z = Mathf.Abs(vector2.z / landscapeHoleVolume.transform.localScale.z);
									if (Mathf.Abs(vector.x) < 0.5f + vector2.x && Mathf.Abs(vector.y) < 0.5f + vector2.y && Mathf.Abs(vector.z) < 0.5f + vector2.z)
									{
										tile.holes[k, l] = false;
										tile.hasAnyHolesData = true;
										this.holeModifications.Add(new LandscapeHoleVolumeManager.HoleModification(tile, splatmapCoord));
									}
								}
							}
						}
					}
				}
			}
			foreach (LandscapeTile landscapeTile in this.modifiedTiles)
			{
				landscapeTile.data.SetHoles(0, 0, landscapeTile.holes);
			}
		}

		// Token: 0x040001C8 RID: 456
		private bool isListeningForUpdates;

		// Token: 0x040001C9 RID: 457
		private bool ignoreHolesChanged;

		// Token: 0x040001CA RID: 458
		private HashSet<LandscapeTile> modifiedTiles = new HashSet<LandscapeTile>();

		// Token: 0x040001CB RID: 459
		private List<LandscapeHoleVolumeManager.HoleModification> holeModifications = new List<LandscapeHoleVolumeManager.HoleModification>();

		// Token: 0x02000862 RID: 2146
		private struct HoleModification
		{
			// Token: 0x06004800 RID: 18432 RVA: 0x001AE3A4 File Offset: 0x001AC5A4
			public HoleModification(LandscapeTile tile, SplatmapCoord splatmapCoord)
			{
				this.tile = tile;
				this.splatmapCoord = splatmapCoord;
			}

			// Token: 0x04003165 RID: 12645
			public LandscapeTile tile;

			// Token: 0x04003166 RID: 12646
			public SplatmapCoord splatmapCoord;
		}
	}
}
