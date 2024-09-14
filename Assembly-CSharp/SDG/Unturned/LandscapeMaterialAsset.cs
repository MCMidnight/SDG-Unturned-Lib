using System;
using SDG.Framework.Foliage;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200030F RID: 783
	public class LandscapeMaterialAsset : Asset
	{
		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x00056C20 File Offset: 0x00054E20
		public override string FriendlyName
		{
			get
			{
				string text = this.name;
				if (this.name.EndsWith("_Material"))
				{
					text = text.Substring(0, text.Length - 9);
				}
				return text.Replace('_', ' ');
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x00056C61 File Offset: 0x00054E61
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NONE;
			}
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00056C64 File Offset: 0x00054E64
		public AssetReference<LandscapeMaterialAsset> getHolidayRedirect()
		{
			switch (HolidayUtil.getActiveHoliday())
			{
			case ENPCHoliday.HALLOWEEN:
				return this.halloweenRedirect;
			case ENPCHoliday.CHRISTMAS:
				return this.christmasRedirect;
			case ENPCHoliday.APRIL_FOOLS:
				return this.aprilFoolsRedirect;
			default:
				return AssetReference<LandscapeMaterialAsset>.invalid;
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00056CA8 File Offset: 0x00054EA8
		public TerrainLayer getOrCreateLayer()
		{
			if (this.layer == null)
			{
				this.layer = new TerrainLayer();
				this.layer.hideFlags = HideFlags.HideAndDontSave;
				this.layer.diffuseTexture = Assets.load<Texture2D>(this.texture);
				if (this.layer.diffuseTexture == null)
				{
					this.layer.diffuseTexture = Texture2D.blackTexture;
				}
				this.layer.normalMapTexture = Assets.load<Texture2D>(this.mask);
				if (this.layer.normalMapTexture == null)
				{
					this.layer.normalMapTexture = Texture2D.blackTexture;
				}
				if (this.layer.diffuseTexture.isReadable)
				{
					this.layer.tileSize = new Vector2((float)this.layer.diffuseTexture.width * 0.25f, (float)this.layer.diffuseTexture.height * 0.25f);
				}
				else
				{
					this.layer.tileSize = new Vector2(16f, 16f);
				}
			}
			return this.layer;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00056DC4 File Offset: 0x00054FC4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.texture = data.ParseStruct<ContentReference<Texture2D>>("Texture", default(ContentReference<Texture2D>));
			this.mask = data.ParseStruct<ContentReference<Texture2D>>("Mask", default(ContentReference<Texture2D>));
			this.physicsMaterialName = data.GetString("Physics_Material", null);
			if (Enum.TryParse<EPhysicsMaterial>(this.physicsMaterialName, ref this.physicsMaterial))
			{
				this.physicsMaterialName = PhysicsTool.GetNameOfLegacyMaterial(this.physicsMaterial);
			}
			this.foliage = data.ParseStruct<AssetReference<FoliageInfoCollectionAsset>>("Foliage", default(AssetReference<FoliageInfoCollectionAsset>));
			this.christmasRedirect = data.ParseStruct<AssetReference<LandscapeMaterialAsset>>("Christmas_Redirect", default(AssetReference<LandscapeMaterialAsset>));
			this.halloweenRedirect = data.ParseStruct<AssetReference<LandscapeMaterialAsset>>("Halloween_Redirect", default(AssetReference<LandscapeMaterialAsset>));
			this.aprilFoolsRedirect = data.ParseStruct<AssetReference<LandscapeMaterialAsset>>("AprilFools_Redirect", default(AssetReference<LandscapeMaterialAsset>));
			this.useAutoSlope = data.ParseBool("Use_Auto_Slope", false);
			this.autoMinAngleBegin = data.ParseFloat("Auto_Min_Angle_Begin", 0f);
			this.autoMinAngleEnd = data.ParseFloat("Auto_Min_Angle_End", 0f);
			this.autoMaxAngleBegin = data.ParseFloat("Auto_Max_Angle_Begin", 0f);
			this.autoMaxAngleEnd = data.ParseFloat("Auto_Max_Angle_End", 0f);
			this.useAutoFoundation = data.ParseBool("Use_Auto_Foundation", false);
			this.autoRayRadius = data.ParseFloat("Auto_Ray_Radius", 0f);
			this.autoRayLength = data.ParseFloat("Auto_Ray_Length", 0f);
			this.autoRayMask = data.ParseEnum<ERayMask>("Auto_Ray_Mask", (ERayMask)0);
		}

		// Token: 0x04000A92 RID: 2706
		public ContentReference<Texture2D> texture;

		// Token: 0x04000A93 RID: 2707
		public ContentReference<Texture2D> mask;

		// Token: 0x04000A94 RID: 2708
		[Obsolete]
		public EPhysicsMaterial physicsMaterial;

		// Token: 0x04000A95 RID: 2709
		public string physicsMaterialName;

		// Token: 0x04000A96 RID: 2710
		public AssetReference<FoliageInfoCollectionAsset> foliage;

		// Token: 0x04000A97 RID: 2711
		public bool useAutoSlope;

		// Token: 0x04000A98 RID: 2712
		public float autoMinAngleBegin;

		// Token: 0x04000A99 RID: 2713
		public float autoMinAngleEnd;

		// Token: 0x04000A9A RID: 2714
		public float autoMaxAngleBegin;

		// Token: 0x04000A9B RID: 2715
		public float autoMaxAngleEnd;

		// Token: 0x04000A9C RID: 2716
		public bool useAutoFoundation;

		// Token: 0x04000A9D RID: 2717
		public float autoRayRadius;

		// Token: 0x04000A9E RID: 2718
		public float autoRayLength;

		// Token: 0x04000A9F RID: 2719
		public ERayMask autoRayMask;

		/// <summary>
		/// Material to use during the Christmas event instead.
		/// </summary>
		// Token: 0x04000AA0 RID: 2720
		public AssetReference<LandscapeMaterialAsset> christmasRedirect;

		/// <summary>
		/// Material to use during the Halloween event instead.
		/// </summary>
		// Token: 0x04000AA1 RID: 2721
		public AssetReference<LandscapeMaterialAsset> halloweenRedirect;

		/// <summary>
		/// Material to use during the April Fools event instead.
		/// </summary>
		// Token: 0x04000AA2 RID: 2722
		public AssetReference<LandscapeMaterialAsset> aprilFoolsRedirect;

		// Token: 0x04000AA3 RID: 2723
		private TerrainLayer layer;
	}
}
