using System;
using System.Collections.Generic;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Landscapes;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000FE RID: 254
	public class FoliageTile : IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x000190FE File Offset: 0x000172FE
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00019106 File Offset: 0x00017306
		public FoliageCoord coord
		{
			get
			{
				return this._coord;
			}
			protected set
			{
				this._coord = value;
				this.updateBounds();
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00019115 File Offset: 0x00017315
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x0001911D File Offset: 0x0001731D
		public Bounds worldBounds { get; protected set; }

		// Token: 0x06000683 RID: 1667 RVA: 0x00019126 File Offset: 0x00017326
		[Obsolete]
		public void addCut(IShapeVolume cut)
		{
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00019128 File Offset: 0x00017328
		internal void AddCut(FoliageCut cut)
		{
			this.cuts.Add(cut);
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				FoliageInstanceList value = keyValuePair.Value;
				for (int i = 0; i < value.matrices.Count; i++)
				{
					List<Matrix4x4> list = value.matrices[i];
					List<bool> list2 = value.clearWhenBaked[i];
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (cut.ContainsPoint(list[j].GetPosition()))
						{
							list.RemoveAt(j);
							list2.RemoveAt(j);
						}
					}
				}
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00019204 File Offset: 0x00017404
		internal void RemoveCut(FoliageCut cut)
		{
			this.cuts.RemoveFast(cut);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00019214 File Offset: 0x00017414
		public bool isInstanceCut(Vector3 point)
		{
			using (List<FoliageCut>.Enumerator enumerator = this.cuts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ContainsPoint(point))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Does this tile contain any placed foliage?
		/// </summary>
		// Token: 0x06000687 RID: 1671 RVA: 0x00019270 File Offset: 0x00017470
		public bool isEmpty()
		{
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				if (!keyValuePair.Value.IsListEmpty())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000192D4 File Offset: 0x000174D4
		public FoliageInstanceList getOrAddList(AssetReference<FoliageInstancedMeshInfoAsset> assetReference)
		{
			FoliageInstanceList foliageInstanceList;
			if (!this.instances.TryGetValue(assetReference, ref foliageInstanceList))
			{
				foliageInstanceList = PoolablePool<FoliageInstanceList>.claim();
				foliageInstanceList.assetReference = assetReference;
				this.instances.Add(assetReference, foliageInstanceList);
			}
			return foliageInstanceList;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001930C File Offset: 0x0001750C
		public void addInstance(FoliageInstanceGroup instance)
		{
			this.getOrAddList(instance.assetReference).addInstanceRandom(instance);
			this.updateBounds();
			this.hasUnsavedChanges = true;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001932D File Offset: 0x0001752D
		public void removeInstance(FoliageInstanceList list, int matricesIndex, int matrixIndex)
		{
			list.removeInstance(matricesIndex, matrixIndex);
			this.hasUnsavedChanges = true;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00019340 File Offset: 0x00017540
		public void clearAndReleaseInstances()
		{
			if (this.instances.Count > 0)
			{
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
				{
					PoolablePool<FoliageInstanceList>.release(keyValuePair.Value);
				}
			}
			this.instances.Clear();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000193B4 File Offset: 0x000175B4
		public void clearGeneratedInstances()
		{
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				keyValuePair.Value.clearGeneratedInstances();
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001940C File Offset: 0x0001760C
		public void applyScale()
		{
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				keyValuePair.Value.applyScale();
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00019464 File Offset: 0x00017664
		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.coord = reader.readValue<FoliageCoord>("Coord");
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001947F File Offset: 0x0001767F
		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<FoliageCoord>("Coord", this.coord);
			writer.endObject();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000194A0 File Offset: 0x000176A0
		public void updateBounds()
		{
			if (this.instances.Count > 0)
			{
				float num = Landscape.TILE_HEIGHT;
				float num2 = -Landscape.TILE_HEIGHT;
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
				{
					foreach (List<Matrix4x4> list in keyValuePair.Value.matrices)
					{
						foreach (Matrix4x4 matrix4x in list)
						{
							float m = matrix4x.m13;
							if (m < num)
							{
								num = m;
							}
							if (m > num2)
							{
								num2 = m;
							}
						}
					}
				}
				float num3 = num2 - num;
				this.worldBounds = new Bounds(new Vector3((float)this.coord.x * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f, num + num3 / 2f, (float)this.coord.y * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f), new Vector3(FoliageSystem.TILE_SIZE, num3, FoliageSystem.TILE_SIZE));
				return;
			}
			this.worldBounds = new Bounds(new Vector3((float)this.coord.x * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f, 0f, (float)this.coord.y * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f), new Vector3(FoliageSystem.TILE_SIZE, Landscape.TILE_HEIGHT, FoliageSystem.TILE_SIZE));
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001966C File Offset: 0x0001786C
		public FoliageTile(FoliageCoord newCoord)
		{
			this.instances = new Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList>();
			this.coord = newCoord;
			this.cuts = new List<FoliageCut>();
		}

		// Token: 0x0400027A RID: 634
		protected FoliageCoord _coord;

		// Token: 0x0400027C RID: 636
		public Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> instances;

		// Token: 0x0400027D RID: 637
		public bool hasUnsavedChanges;

		// Token: 0x0400027E RID: 638
		public bool isRelevantToViewer;

		// Token: 0x0400027F RID: 639
		private List<FoliageCut> cuts;
	}
}
