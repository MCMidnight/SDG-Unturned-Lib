using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200010D RID: 269
	[Obsolete]
	public class DevkitHierarchyWorldObject : DevkitHierarchyWorldItem
	{
		/// <summary>
		/// Devkit objects are now converted to regular objects and excluded from the file when re-saving.
		/// </summary>
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001A1F3 File Offset: 0x000183F3
		public override bool ShouldSave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001A1F8 File Offset: 0x000183F8
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.GUID = reader.readValue<Guid>("GUID");
			this.placementOrigin = reader.readValue<ELevelObjectPlacementOrigin>("Origin");
			this.customMaterialOverride = reader.readValue<AssetReference<MaterialPaletteAsset>>("Custom_Material_Override");
			if (reader.containsKey("Material_Index_Override"))
			{
				this.materialIndexOverride = reader.readValue<int>("Material_Index_Override");
			}
			else
			{
				this.materialIndexOverride = -1;
			}
			LevelHierarchy.instance.loadedAnyDevkitObjects = true;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001A271 File Offset: 0x00018471
		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001A279 File Offset: 0x00018479
		protected void OnDisable()
		{
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001A284 File Offset: 0x00018484
		protected void Start()
		{
			NetId devkitObjectNetId = LevelNetIdRegistry.GetDevkitObjectNetId(this.instanceID);
			byte b;
			byte b2;
			LevelObjects.registerDevkitObject(new LevelObject(base.inspectablePosition, base.inspectableRotation, base.inspectableScale, 0, this.GUID, this.placementOrigin, this.instanceID, this.customMaterialOverride, this.materialIndexOverride, devkitObjectNetId, true), out b, out b2);
		}

		// Token: 0x04000295 RID: 661
		public AssetReference<MaterialPaletteAsset> customMaterialOverride;

		// Token: 0x04000296 RID: 662
		public int materialIndexOverride = -1;

		// Token: 0x04000297 RID: 663
		public Guid GUID;

		// Token: 0x04000298 RID: 664
		public ELevelObjectPlacementOrigin placementOrigin;
	}
}
