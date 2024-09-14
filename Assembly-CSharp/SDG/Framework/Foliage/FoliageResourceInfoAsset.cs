using System;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000F2 RID: 242
	public class FoliageResourceInfoAsset : FoliageInfoAsset
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x000169D2 File Offset: 0x00014BD2
		public override void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!bakeSettings.bakeResources)
			{
				return;
			}
			if (bakeSettings.bakeClear)
			{
				return;
			}
			base.bakeFoliage(bakeSettings, surface, bounds, surfaceWeight, collectionWeight);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000169F4 File Offset: 0x00014BF4
		public override int getInstanceCountInVolume(IShapeVolume volume)
		{
			Bounds worldBounds = volume.worldBounds;
			RegionBounds regionBounds = new RegionBounds(worldBounds);
			int num = 0;
			for (byte b = regionBounds.min.x; b <= regionBounds.max.x; b += 1)
			{
				for (byte b2 = regionBounds.min.y; b2 <= regionBounds.max.y; b2 += 1)
				{
					foreach (ResourceSpawnpoint resourceSpawnpoint in LevelGround.trees[(int)b, (int)b2])
					{
						if (this.resource.isReferenceTo(resourceSpawnpoint.asset) && volume.containsPoint(resourceSpawnpoint.point))
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00016AD0 File Offset: 0x00014CD0
		protected override void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			ResourceAsset resourceAsset = Assets.find<ResourceAsset>(this.resource);
			if (resourceAsset == null)
			{
				return;
			}
			LevelGround.addSpawn(position, resourceAsset.GUID, clearWhenBaked);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00016AFC File Offset: 0x00014CFC
		protected override bool isPositionValid(Vector3 position)
		{
			if (!VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsPositionBakeable(position, false, true, false))
			{
				return false;
			}
			int num = Physics.OverlapSphereNonAlloc(position, this.obstructionRadius, FoliageResourceInfoAsset.OBSTRUCTION_COLLIDERS, RayMasks.BLOCK_RESOURCE);
			for (int i = 0; i < num; i++)
			{
				ObjectAsset asset = LevelObjects.getAsset(FoliageResourceInfoAsset.OBSTRUCTION_COLLIDERS[i].transform);
				if (asset != null && !asset.isSnowshoe)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00016B60 File Offset: 0x00014D60
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.resource = data.ParseStruct<AssetReference<ResourceAsset>>("Resource", default(AssetReference<ResourceAsset>));
			if (data.ContainsKey("Obstruction_Radius"))
			{
				this.obstructionRadius = data.ParseFloat("Obstruction_Radius", 0f);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00016BB3 File Offset: 0x00014DB3
		protected virtual void resetResource()
		{
			this.obstructionRadius = 4f;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00016BC0 File Offset: 0x00014DC0
		public FoliageResourceInfoAsset()
		{
			this.resetResource();
		}

		// Token: 0x04000236 RID: 566
		private static readonly Collider[] OBSTRUCTION_COLLIDERS = new Collider[16];

		// Token: 0x04000237 RID: 567
		public AssetReference<ResourceAsset> resource;

		// Token: 0x04000238 RID: 568
		public float obstructionRadius;
	}
}
