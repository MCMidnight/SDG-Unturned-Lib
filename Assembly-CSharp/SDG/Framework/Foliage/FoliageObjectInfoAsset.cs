using System;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000F0 RID: 240
	public class FoliageObjectInfoAsset : FoliageInfoAsset
	{
		// Token: 0x060005F1 RID: 1521 RVA: 0x00016804 File Offset: 0x00014A04
		public override void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!bakeSettings.bakeObjects)
			{
				return;
			}
			if (bakeSettings.bakeClear)
			{
				return;
			}
			base.bakeFoliage(bakeSettings, surface, bounds, surfaceWeight, collectionWeight);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00016828 File Offset: 0x00014A28
		public override int getInstanceCountInVolume(IShapeVolume volume)
		{
			Bounds worldBounds = volume.worldBounds;
			RegionBounds regionBounds = new RegionBounds(worldBounds);
			int num = 0;
			for (byte b = regionBounds.min.x; b <= regionBounds.max.x; b += 1)
			{
				for (byte b2 = regionBounds.min.y; b2 <= regionBounds.max.y; b2 += 1)
				{
					foreach (LevelObject levelObject in LevelObjects.objects[(int)b, (int)b2])
					{
						if (this.obj.isReferenceTo(levelObject.asset) && volume.containsPoint(levelObject.transform.position))
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00016908 File Offset: 0x00014B08
		protected override void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			ObjectAsset objectAsset = Assets.find<ObjectAsset>(this.obj);
			if (objectAsset == null)
			{
				return;
			}
			LevelObjects.addObject(position, rotation, scale, 0, objectAsset.GUID, clearWhenBaked ? ELevelObjectPlacementOrigin.GENERATED : ELevelObjectPlacementOrigin.PAINTED);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001693D File Offset: 0x00014B3D
		protected override bool isPositionValid(Vector3 position)
		{
			return VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsPositionBakeable(position, false, false, true);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00016954 File Offset: 0x00014B54
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.obj = data.ParseStruct<AssetReference<ObjectAsset>>("Object", default(AssetReference<ObjectAsset>));
			if (data.ContainsKey("Obstruction_Radius"))
			{
				this.obstructionRadius = data.ParseFloat("Obstruction_Radius", 0f);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000169A7 File Offset: 0x00014BA7
		protected virtual void resetObject()
		{
			this.obstructionRadius = 4f;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000169B4 File Offset: 0x00014BB4
		public FoliageObjectInfoAsset()
		{
			this.resetObject();
		}

		// Token: 0x04000232 RID: 562
		public AssetReference<ObjectAsset> obj;

		// Token: 0x04000233 RID: 563
		public float obstructionRadius;
	}
}
