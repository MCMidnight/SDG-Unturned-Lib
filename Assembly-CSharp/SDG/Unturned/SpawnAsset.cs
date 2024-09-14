using System;
using System.Collections.Generic;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000368 RID: 872
	public class SpawnAsset : Asset
	{
		/// <summary>
		/// Parent spawn assets this would like to be inserted into.
		/// </summary>
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0005EF68 File Offset: 0x0005D168
		// (set) Token: 0x06001A59 RID: 6745 RVA: 0x0005EF70 File Offset: 0x0005D170
		public List<SpawnTable> insertRoots { get; protected set; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0005EF79 File Offset: 0x0005D179
		public List<SpawnTable> roots
		{
			get
			{
				return this._roots;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0005EF81 File Offset: 0x0005D181
		public List<SpawnTable> tables
		{
			get
			{
				return this._tables;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0005EF89 File Offset: 0x0005D189
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.SPAWN;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0005EF8D File Offset: 0x0005D18D
		// (set) Token: 0x06001A5E RID: 6750 RVA: 0x0005EF95 File Offset: 0x0005D195
		public bool hasBeenOverridden { get; protected set; }

		/// <summary>
		/// Zero weights of child spawn tables.
		/// Called when inserting a root marked isOverride.
		/// </summary>
		// Token: 0x06001A5F RID: 6751 RVA: 0x0005EFA0 File Offset: 0x0005D1A0
		public void markOverridden()
		{
			if (this.hasBeenOverridden)
			{
				return;
			}
			this.hasBeenOverridden = true;
			foreach (SpawnTable spawnTable in this.tables)
			{
				if (!spawnTable.isOverride)
				{
					spawnTable.weight = 0;
				}
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0005F00C File Offset: 0x0005D20C
		internal SpawnTable PickRandomEntry(Func<string> errorContextCallback)
		{
			if (this.tables.Count < 1)
			{
				UnturnedLog.warn(string.Concat(new string[]
				{
					"Spawn table ",
					this.name,
					" from ",
					base.GetOriginName(),
					" resolved by ",
					((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
					" while empty"
				}));
				return null;
			}
			if (this.areTablesDirty)
			{
				UnturnedLog.warn(string.Concat(new string[]
				{
					"Spawn table ",
					this.name,
					" from ",
					base.GetOriginName(),
					" resolved by ",
					((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
					" while dirty"
				}));
				this.sortAndNormalizeWeights();
			}
			if (this.tables.Count == 1)
			{
				return this.tables[0];
			}
			float value = Random.value;
			for (int i = 0; i < this.tables.Count; i++)
			{
				if (value < this.tables[i].normalizedWeight || i == this.tables.Count - 1)
				{
					return this.tables[i];
				}
			}
			UnturnedLog.error(string.Concat(new string[]
			{
				"Spawn table ",
				this.name,
				" from ",
				base.GetOriginName(),
				" resolved by ",
				((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
				" had no valid entry (should never happen)"
			}));
			return null;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0005F1B0 File Offset: 0x0005D3B0
		[Obsolete]
		public void resolve(out ushort id, out bool isSpawn)
		{
			id = 0;
			isSpawn = false;
			SpawnTable spawnTable = this.PickRandomEntry(null);
			if (spawnTable != null)
			{
				if (spawnTable.legacySpawnId != 0)
				{
					id = spawnTable.legacySpawnId;
					isSpawn = true;
					return;
				}
				if (spawnTable.legacyAssetId != 0)
				{
					id = spawnTable.legacyAssetId;
					isSpawn = false;
					return;
				}
			}
		}

		/// <summary>
		/// Do tables need to be sorted and normalized?
		/// </summary>
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x0005F1F6 File Offset: 0x0005D3F6
		// (set) Token: 0x06001A63 RID: 6755 RVA: 0x0005F1FE File Offset: 0x0005D3FE
		public bool areTablesDirty { get; protected set; }

		/// <summary>
		/// Sort children by weight ascending, and calculate their normalized chance as a percentage of total weight.
		/// </summary>
		// Token: 0x06001A64 RID: 6756 RVA: 0x0005F208 File Offset: 0x0005D408
		public void sortAndNormalizeWeights()
		{
			if (!this.areTablesDirty)
			{
				return;
			}
			this.areTablesDirty = false;
			if (this.tables.Count < 1)
			{
				return;
			}
			if (this.tables.Count == 1)
			{
				this.tables[0].normalizedWeight = 1f;
				return;
			}
			this.tables.Sort(SpawnAsset.comparator);
			float num = 0f;
			foreach (SpawnTable spawnTable in this.tables)
			{
				num += (float)spawnTable.weight;
			}
			float num2 = 0f;
			foreach (SpawnTable spawnTable2 in this.tables)
			{
				num2 += (float)spawnTable2.weight;
				spawnTable2.normalizedWeight = num2 / num;
			}
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0005F314 File Offset: 0x0005D514
		public void markTablesDirty()
		{
			this.areTablesDirty = true;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0005F320 File Offset: 0x0005D520
		public void EditorAddChild(Asset newChild)
		{
			SpawnAsset spawnAsset = newChild as SpawnAsset;
			if (spawnAsset != null)
			{
				SpawnTable spawnTable = new SpawnTable();
				spawnTable.targetGuid = this.GUID;
				spawnTable.isLink = true;
				spawnAsset.roots.Add(spawnTable);
			}
			SpawnTable spawnTable2 = new SpawnTable();
			spawnTable2.targetGuid = newChild.GUID;
			this.tables.Add(spawnTable2);
			this.markTablesDirty();
		}

		/// <summary>
		/// Remove from roots, and if reference is valid remove us from their children.
		/// </summary>
		// Token: 0x06001A67 RID: 6759 RVA: 0x0005F380 File Offset: 0x0005D580
		public void EditorRemoveParentAtIndex(int parentIndex)
		{
			SpawnTable spawnTable = this.roots[parentIndex];
			SpawnAsset spawnAsset;
			if (spawnTable.legacySpawnId != 0)
			{
				spawnAsset = (Assets.find(EAssetType.SPAWN, spawnTable.legacySpawnId) as SpawnAsset);
			}
			else
			{
				spawnAsset = (Assets.find(spawnTable.targetGuid) as SpawnAsset);
			}
			if (spawnAsset != null)
			{
				for (int i = 0; i < spawnAsset.tables.Count; i++)
				{
					SpawnTable spawnTable2 = spawnAsset.tables[i];
					if ((spawnTable2.legacySpawnId != 0 && spawnTable2.legacySpawnId == this.id) || spawnTable2.targetGuid == this.GUID)
					{
						spawnAsset.tables.RemoveAt(i);
						spawnAsset.markTablesDirty();
						break;
					}
				}
			}
			this.roots.RemoveAt(parentIndex);
		}

		/// <summary>
		/// Remove from tables, and if referencing a child table remove us from their roots.
		/// </summary>
		// Token: 0x06001A68 RID: 6760 RVA: 0x0005F438 File Offset: 0x0005D638
		public void EditorRemoveChildAtIndex(int childIndex)
		{
			SpawnTable spawnTable = this.tables[childIndex];
			SpawnAsset spawnAsset;
			if (spawnTable.legacySpawnId != 0)
			{
				spawnAsset = (Assets.find(EAssetType.SPAWN, spawnTable.legacySpawnId) as SpawnAsset);
			}
			else
			{
				spawnAsset = (Assets.find(spawnTable.targetGuid) as SpawnAsset);
			}
			if (spawnAsset != null)
			{
				for (int i = 0; i < spawnAsset.roots.Count; i++)
				{
					SpawnTable spawnTable2 = spawnAsset.roots[i];
					if ((spawnTable2.legacySpawnId != 0 && spawnTable2.legacySpawnId == this.id) || spawnTable2.targetGuid == this.GUID)
					{
						spawnAsset.roots.RemoveAt(i);
						break;
					}
				}
			}
			this.tables.RemoveAt(childIndex);
			this.markTablesDirty();
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0005F4EF File Offset: 0x0005D6EF
		public void setTableWeightAtIndex(int tableIndex, int weight)
		{
			this.tables[tableIndex].weight = weight;
			this.markTablesDirty();
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0005F514 File Offset: 0x0005D714
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			DatList datList;
			if (data.TryGetList("Roots", out datList))
			{
				this.insertRoots = new List<SpawnTable>(datList.Count);
				this._roots = new List<SpawnTable>(datList.Count);
				using (List<IDatNode>.Enumerator enumerator = datList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDatNode datNode = enumerator.Current;
						DatDictionary datDictionary = datNode as DatDictionary;
						if (datDictionary != null)
						{
							SpawnTable spawnTable = new SpawnTable();
							if (spawnTable.TryParse(this, datDictionary))
							{
								this.insertRoots.Add(spawnTable);
							}
						}
					}
					goto IL_1EA;
				}
			}
			int num = data.ParseInt32("Roots", 0);
			this.insertRoots = new List<SpawnTable>(num);
			for (int i = 0; i < num; i++)
			{
				SpawnTable spawnTable2 = new SpawnTable();
				spawnTable2.legacySpawnId = data.ParseUInt16("Root_" + i.ToString() + "_Spawn_ID", 0);
				spawnTable2.targetGuid = data.ParseGuid("Root_" + i.ToString() + "_GUID", default(Guid));
				spawnTable2.isOverride = data.ContainsKey("Root_" + i.ToString() + "_Override");
				spawnTable2.weight = data.ParseInt32("Root_" + i.ToString() + "_Weight", spawnTable2.isOverride ? 1 : 0);
				spawnTable2.normalizedWeight = 0f;
				if (spawnTable2.legacySpawnId == 0 && GuidExtension.IsEmpty(spawnTable2.targetGuid))
				{
					Assets.reportError(this, "root " + i.ToString() + " has neither a Spawn_ID or GUID set!");
				}
				if (spawnTable2.weight <= 0)
				{
					Assets.reportError(this, "root " + i.ToString() + " has no weight!");
				}
				this.insertRoots.Add(spawnTable2);
			}
			this._roots = new List<SpawnTable>(num);
			IL_1EA:
			DatList datList2;
			if (data.TryGetList("Tables", out datList2))
			{
				this._tables = new List<SpawnTable>(datList2.Count);
				using (List<IDatNode>.Enumerator enumerator = datList2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDatNode datNode2 = enumerator.Current;
						DatDictionary datDictionary2 = datNode2 as DatDictionary;
						if (datDictionary2 != null)
						{
							SpawnTable spawnTable3 = new SpawnTable();
							if (spawnTable3.TryParse(this, datDictionary2))
							{
								this.tables.Add(spawnTable3);
							}
						}
					}
					goto IL_3AE;
				}
			}
			int num2 = data.ParseInt32("Tables", 0);
			this._tables = new List<SpawnTable>(num2);
			for (int j = 0; j < num2; j++)
			{
				SpawnTable spawnTable4 = new SpawnTable();
				spawnTable4.legacyAssetId = data.ParseUInt16("Table_" + j.ToString() + "_Asset_ID", 0);
				spawnTable4.legacySpawnId = data.ParseUInt16("Table_" + j.ToString() + "_Spawn_ID", 0);
				spawnTable4.targetGuid = data.ParseGuid("Table_" + j.ToString() + "_GUID", default(Guid));
				spawnTable4.weight = data.ParseInt32("Table_" + j.ToString() + "_Weight", 0);
				spawnTable4.normalizedWeight = 0f;
				if (spawnTable4.legacySpawnId == 0 && spawnTable4.legacyAssetId == 0 && GuidExtension.IsEmpty(spawnTable4.targetGuid))
				{
					Assets.reportError(this, "table " + j.ToString() + " has neither a Spawn_ID, Asset_ID, or GUID set!");
				}
				if (spawnTable4.weight <= 0)
				{
					Assets.reportError(this, "table " + j.ToString() + " has no weight!");
				}
				this.tables.Add(spawnTable4);
			}
			IL_3AE:
			this.areTablesDirty = true;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0005F8F4 File Offset: 0x0005DAF4
		internal override void OnCreatedAtRuntime()
		{
			base.OnCreatedAtRuntime();
			this.insertRoots = new List<SpawnTable>();
			this._roots = new List<SpawnTable>();
			this._tables = new List<SpawnTable>();
		}

		// Token: 0x04000C2D RID: 3117
		private static SpawnAsset.SpawnTableWeightComparator comparator = new SpawnAsset.SpawnTableWeightComparator();

		// Token: 0x04000C2F RID: 3119
		protected List<SpawnTable> _roots;

		// Token: 0x04000C30 RID: 3120
		protected List<SpawnTable> _tables;

		// Token: 0x02000928 RID: 2344
		private class SpawnTableWeightComparator : IComparer<SpawnTable>
		{
			// Token: 0x06004A86 RID: 19078 RVA: 0x001B1A37 File Offset: 0x001AFC37
			public int Compare(SpawnTable a, SpawnTable b)
			{
				return b.weight - a.weight;
			}
		}
	}
}
