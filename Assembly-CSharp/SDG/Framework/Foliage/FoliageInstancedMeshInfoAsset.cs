using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000ED RID: 237
	public class FoliageInstancedMeshInfoAsset : FoliageInfoAsset
	{
		/// <summary>
		/// Get asset ref to replace this one for holiday, invalid to disable, or null if it should not be redirected.
		/// </summary>
		// Token: 0x060005CD RID: 1485 RVA: 0x00015FB0 File Offset: 0x000141B0
		public AssetReference<FoliageInstancedMeshInfoAsset>? getHolidayRedirect()
		{
			ENPCHoliday activeHoliday = HolidayUtil.getActiveHoliday();
			if (activeHoliday == ENPCHoliday.HALLOWEEN)
			{
				return this.halloweenRedirect;
			}
			if (activeHoliday == ENPCHoliday.CHRISTMAS)
			{
				return this.christmasRedirect;
			}
			return default(AssetReference<FoliageInstancedMeshInfoAsset>?);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00015FE2 File Offset: 0x000141E2
		public override void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!bakeSettings.bakeInstancesMeshes)
			{
				return;
			}
			if (bakeSettings.bakeClear)
			{
				return;
			}
			base.bakeFoliage(bakeSettings, surface, bounds, surfaceWeight, collectionWeight);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00016004 File Offset: 0x00014204
		public override int getInstanceCountInVolume(IShapeVolume volume)
		{
			Bounds worldBounds = volume.worldBounds;
			FoliageBounds foliageBounds = new FoliageBounds(worldBounds);
			int num = 0;
			for (int i = foliageBounds.min.x; i <= foliageBounds.max.x; i++)
			{
				for (int j = foliageBounds.min.y; j <= foliageBounds.max.y; j++)
				{
					FoliageTile tile = FoliageSystem.getTile(new FoliageCoord(i, j));
					FoliageInstanceList foliageInstanceList;
					if (tile != null && tile.instances != null && tile.instances.TryGetValue(base.getReferenceTo<FoliageInstancedMeshInfoAsset>(), ref foliageInstanceList))
					{
						foreach (List<Matrix4x4> list in foliageInstanceList.matrices)
						{
							foreach (Matrix4x4 matrix4x in list)
							{
								if (volume.containsPoint(matrix4x.GetPosition()))
								{
									num++;
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00016138 File Offset: 0x00014338
		protected override void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			FoliageSystem.addInstance(base.getReferenceTo<FoliageInstancedMeshInfoAsset>(), position, rotation, scale, clearWhenBaked);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001614A File Offset: 0x0001434A
		protected override bool isPositionValid(Vector3 position)
		{
			return VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsPositionBakeable(position, true, false, false);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00016160 File Offset: 0x00014360
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.mesh = data.ParseStruct<ContentReference<Mesh>>("Mesh", default(ContentReference<Mesh>));
			this.material = data.ParseStruct<ContentReference<Material>>("Material", default(ContentReference<Material>));
			if (data.ContainsKey("Cast_Shadows"))
			{
				this.castShadows = data.ParseBool("Cast_Shadows", false);
			}
			else
			{
				this.castShadows = false;
			}
			if (data.ContainsKey("Tile_Dither"))
			{
				this.tileDither = data.ParseBool("Tile_Dither", false);
			}
			else
			{
				this.tileDither = true;
			}
			if (data.ContainsKey("Draw_Distance"))
			{
				this.drawDistance = data.ParseInt32("Draw_Distance", 0);
			}
			else
			{
				this.drawDistance = -1;
			}
			if (data.ContainsKey("Christmas_Redirect"))
			{
				this.christmasRedirect = new AssetReference<FoliageInstancedMeshInfoAsset>?(data.ParseStruct<AssetReference<FoliageInstancedMeshInfoAsset>>("Christmas_Redirect", default(AssetReference<FoliageInstancedMeshInfoAsset>)));
			}
			if (data.ContainsKey("Halloween_Redirect"))
			{
				this.halloweenRedirect = new AssetReference<FoliageInstancedMeshInfoAsset>?(data.ParseStruct<AssetReference<FoliageInstancedMeshInfoAsset>>("Halloween_Redirect", default(AssetReference<FoliageInstancedMeshInfoAsset>)));
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001627A File Offset: 0x0001447A
		protected virtual void resetInstancedMeshInfo()
		{
			this.tileDither = true;
			this.drawDistance = -1;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001628A File Offset: 0x0001448A
		public FoliageInstancedMeshInfoAsset()
		{
			this.resetInstancedMeshInfo();
		}

		// Token: 0x0400021F RID: 543
		public ContentReference<Mesh> mesh;

		// Token: 0x04000220 RID: 544
		public ContentReference<Material> material;

		// Token: 0x04000221 RID: 545
		public bool castShadows;

		// Token: 0x04000222 RID: 546
		public bool tileDither;

		// Token: 0x04000223 RID: 547
		public int drawDistance;

		/// <summary>
		/// Foliage to use during the Christmas event instead.
		/// </summary>
		// Token: 0x04000224 RID: 548
		public AssetReference<FoliageInstancedMeshInfoAsset>? christmasRedirect;

		/// <summary>
		/// Foliage to use during the Halloween event instead.
		/// </summary>
		// Token: 0x04000225 RID: 549
		public AssetReference<FoliageInstancedMeshInfoAsset>? halloweenRedirect;
	}
}
