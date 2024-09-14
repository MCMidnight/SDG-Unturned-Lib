using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Expands upon Unity physics material properties for gameplay features.
	/// </summary>
	// Token: 0x02000357 RID: 855
	public class PhysicsMaterialAsset : PhysicsMaterialAssetBase
	{
		// Token: 0x060019EE RID: 6638 RVA: 0x0005D4A4 File Offset: 0x0005B6A4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			DatList datList;
			if (data.TryGetList("UnityNames", out datList))
			{
				this.physicMaterialNames = new string[datList.Count];
				for (int i = 0; i < datList.Count; i++)
				{
					this.physicMaterialNames[i] = datList.GetString(i, null);
				}
			}
			else
			{
				this.physicMaterialNames = new string[]
				{
					data.GetString("UnityName", null)
				};
			}
			this.fallbackRef = data.ParseStruct<AssetReference<PhysicsMaterialAsset>>("Fallback", default(AssetReference<PhysicsMaterialAsset>));
			this.bulletImpactEffect = data.ParseStruct<AssetReference<EffectAsset>>("WipDoNotUseTemp_BulletImpactEffect", default(AssetReference<EffectAsset>));
			this.tireMotionEffect = data.ParseStruct<AssetReference<EffectAsset>>("TireMotionEffect", default(AssetReference<EffectAsset>));
			if (data.ContainsKey("Character_Friction_Mode"))
			{
				this.characterFrictionMode = data.ParseEnum<EPhysicsMaterialCharacterFrictionMode>("Character_Friction_Mode", EPhysicsMaterialCharacterFrictionMode.ImmediatelyResponsive);
				if (this.characterFrictionMode != EPhysicsMaterialCharacterFrictionMode.ImmediatelyResponsive)
				{
					if (data.ContainsKey("Character_Acceleration_Multiplier"))
					{
						this.characterAccelerationMultiplier = new float?(data.ParseFloat("Character_Acceleration_Multiplier", 0f));
					}
					if (data.ContainsKey("Character_Deceleration_Multiplier"))
					{
						this.characterDecelerationMultiplier = new float?(data.ParseFloat("Character_Deceleration_Multiplier", 0f));
					}
					if (data.ContainsKey("Character_Max_Speed_Multiplier"))
					{
						this.characterMaxSpeedMultiplier = new float?(data.ParseFloat("Character_Max_Speed_Multiplier", 0f));
					}
				}
			}
			if (data.ContainsKey("IsArable"))
			{
				this.isArable = new bool?(data.ParseBool("IsArable", false));
			}
			if (data.ContainsKey("HasOil"))
			{
				this.hasOil = new bool?(data.ParseBool("HasOil", false));
			}
			PhysicMaterialCustomData.RegisterAsset(this);
		}

		/// <summary>
		/// Originally considered assets for each legacy material with fallback to main material, but the fallback
		/// would mean a failed lookup for every property in the vast majority of cases.
		/// </summary>
		// Token: 0x04000BD2 RID: 3026
		public string[] physicMaterialNames;

		// Token: 0x04000BD3 RID: 3027
		public AssetReference<PhysicsMaterialAsset> fallbackRef;

		// Token: 0x04000BD4 RID: 3028
		public AssetReference<EffectAsset> bulletImpactEffect;

		// Token: 0x04000BD5 RID: 3029
		public AssetReference<EffectAsset> tireMotionEffect;

		// Token: 0x04000BD6 RID: 3030
		public EPhysicsMaterialCharacterFrictionMode characterFrictionMode;

		/// <summary>
		/// If true, crops can be planted on this material.
		/// </summary>
		// Token: 0x04000BD7 RID: 3031
		public bool? isArable;

		/// <summary>
		/// If true, oil drills can be placed on this material.
		/// </summary>
		// Token: 0x04000BD8 RID: 3032
		public bool? hasOil;

		/// <summary>
		/// For custom friction mode, multiplies character acceleration.
		/// </summary>
		// Token: 0x04000BD9 RID: 3033
		public float? characterAccelerationMultiplier;

		/// <summary>
		/// For custom friction mode, multiplies character deceleration.
		/// </summary>
		// Token: 0x04000BDA RID: 3034
		public float? characterDecelerationMultiplier;

		/// <summary>
		/// For custom friction mode, multiplies character max speed.
		/// </summary>
		// Token: 0x04000BDB RID: 3035
		public float? characterMaxSpeedMultiplier;
	}
}
