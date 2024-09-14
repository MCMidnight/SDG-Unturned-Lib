using System;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000367 RID: 871
	public class SpawnTable
	{
		/// <summary>
		/// Helper method for plugins because IDs are internal.
		/// </summary>
		// Token: 0x06001A54 RID: 6740 RVA: 0x0005ECE4 File Offset: 0x0005CEE4
		public Asset FindAsset(EAssetType legacyAssetType)
		{
			if (!GuidExtension.IsEmpty(this.targetGuid))
			{
				return Assets.find(this.targetGuid);
			}
			if (this.legacyAssetId > 0)
			{
				return Assets.find(legacyAssetType, this.legacyAssetId);
			}
			if (this.legacySpawnId > 0)
			{
				return Assets.find(EAssetType.SPAWN, this.legacySpawnId) as SpawnAsset;
			}
			return null;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0005ED40 File Offset: 0x0005CF40
		internal bool TryParse(Asset assetContext, DatDictionary datDictionary)
		{
			this.targetGuid = datDictionary.ParseGuid("Guid", default(Guid));
			this.legacyAssetId = datDictionary.ParseUInt16("LegacyAssetId", 0);
			this.legacySpawnId = datDictionary.ParseUInt16("LegacySpawnId", 0);
			this.isOverride = datDictionary.ParseBool("IsOverride", false);
			this.weight = datDictionary.ParseInt32("Weight", this.isOverride ? 1 : 0);
			if (this.legacySpawnId == 0 && this.legacyAssetId == 0 && GuidExtension.IsEmpty(this.targetGuid))
			{
				Assets.reportError(assetContext, "contains an entry with neither a LegacyAssetId, LegacySpawnId, or Guid set!");
				return false;
			}
			if (this.weight <= 0)
			{
				Assets.reportError(assetContext, "contains an entry with no weight!");
				return false;
			}
			return true;
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0005EDFC File Offset: 0x0005CFFC
		internal void Write(DatWriter writer, EAssetType legacyAssetType)
		{
			if (!GuidExtension.IsEmpty(this.targetGuid))
			{
				Asset asset = Assets.find(this.targetGuid);
				string message = (asset != null) ? (asset.FriendlyName + " (" + asset.GetTypeFriendlyName() + ")") : string.Format("Unknown {0}", legacyAssetType);
				writer.WriteComment(message);
				writer.WriteKeyValue("Guid", this.targetGuid, null);
			}
			else if (this.legacyAssetId > 0)
			{
				Asset asset2 = Assets.find(legacyAssetType, this.legacyAssetId);
				string message2 = (asset2 != null) ? (asset2.FriendlyName + " (" + asset2.GetTypeFriendlyName() + ")") : string.Format("Unknown {0}", legacyAssetType);
				writer.WriteComment(message2);
				writer.WriteKeyValue("LegacyAssetId", this.legacyAssetId, null);
			}
			else if (this.legacySpawnId > 0)
			{
				SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, this.legacySpawnId) as SpawnAsset;
				string message3 = (spawnAsset != null) ? (spawnAsset.FriendlyName + " (" + spawnAsset.GetTypeFriendlyName() + ")") : "Unknown";
				writer.WriteComment(message3);
				writer.WriteKeyValue("LegacySpawnId", this.legacySpawnId, null);
			}
			if (this.isOverride)
			{
				writer.WriteKeyValue("IsOverride", this.isOverride, null);
			}
			writer.WriteKeyValue("Weight", this.weight, null);
		}

		/// <summary>
		/// If non-zero, legacy ID of final Asset to return.
		/// </summary>
		// Token: 0x04000C25 RID: 3109
		internal ushort legacyAssetId;

		/// <summary>
		/// If non-zero, legacy ID of SpawnAsset to resolve.
		/// </summary>
		// Token: 0x04000C26 RID: 3110
		internal ushort legacySpawnId;

		/// <summary>
		/// If both legacy IDs are zero this GUID will be used. If the target asset is
		/// a SpawnAsset it will be further resolved, otherwise the found asset is returned.
		/// </summary>
		// Token: 0x04000C27 RID: 3111
		internal Guid targetGuid;

		// Token: 0x04000C28 RID: 3112
		public int weight;

		// Token: 0x04000C29 RID: 3113
		internal float normalizedWeight;

		// Token: 0x04000C2A RID: 3114
		public bool isLink;

		/// <summary>
		/// Can be enabled by spawn tables that insert themselves into other spawn tables using the roots list.
		/// If true, zeros the weight of child tables in the parent spawn table.
		/// </summary>
		// Token: 0x04000C2B RID: 3115
		public bool isOverride;

		/// <summary>
		/// Has this spawn been added as a root of its child spawn table?
		/// Used for debugging spawn hierarchy in editor.
		/// </summary>
		// Token: 0x04000C2C RID: 3116
		public bool hasNotifiedChild;
	}
}
