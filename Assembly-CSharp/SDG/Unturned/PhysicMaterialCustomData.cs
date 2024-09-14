using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Work-in-progress plan to allow modders to create custom physics effects.
	/// </summary>
	// Token: 0x02000356 RID: 854
	internal static class PhysicMaterialCustomData
	{
		// Token: 0x060019E0 RID: 6624 RVA: 0x0005CD10 File Offset: 0x0005AF10
		public static OneShotAudioDefinition GetAudioDef(string materialName, string propertyName)
		{
			OneShotAudioDefinition result = null;
			using (List<PhysicMaterialCustomData.CombinedPhysicMaterialInfo>.Enumerator enumerator = PhysicMaterialCustomData.EnumerateInfo(materialName).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MasterBundleReference<OneShotAudioDefinition> masterBundleReference;
					if (enumerator.Current.audioDefs.TryGetValue(propertyName, ref masterBundleReference))
					{
						result = masterBundleReference.loadAsset(true);
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0005CD78 File Offset: 0x0005AF78
		public static AssetReference<EffectAsset> WipDoNotUseTemp_GetBulletImpactEffect(string materialName)
		{
			AssetReference<EffectAsset> result = default(AssetReference<EffectAsset>);
			foreach (PhysicMaterialCustomData.CombinedPhysicMaterialInfo combinedPhysicMaterialInfo in PhysicMaterialCustomData.EnumerateInfo(materialName))
			{
				if (combinedPhysicMaterialInfo.bulletImpactEffect.isValid)
				{
					result = combinedPhysicMaterialInfo.bulletImpactEffect;
					break;
				}
			}
			return result;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0005CDE4 File Offset: 0x0005AFE4
		public static AssetReference<EffectAsset> GetTireMotionEffect(string materialName)
		{
			AssetReference<EffectAsset> result = default(AssetReference<EffectAsset>);
			foreach (PhysicMaterialCustomData.CombinedPhysicMaterialInfo combinedPhysicMaterialInfo in PhysicMaterialCustomData.EnumerateInfo(materialName))
			{
				if (combinedPhysicMaterialInfo.tireMotionEffect.isValid)
				{
					result = combinedPhysicMaterialInfo.tireMotionEffect;
					break;
				}
			}
			return result;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0005CE50 File Offset: 0x0005B050
		public static PhysicsMaterialCharacterFrictionProperties GetCharacterFrictionProperties(string materialName)
		{
			PhysicsMaterialCharacterFrictionProperties result;
			result.mode = EPhysicsMaterialCharacterFrictionMode.ImmediatelyResponsive;
			result.accelerationMultiplier = 1f;
			result.decelerationMultiplier = 1f;
			result.maxSpeedMultiplier = 1f;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			foreach (PhysicMaterialCustomData.CombinedPhysicMaterialInfo combinedPhysicMaterialInfo in PhysicMaterialCustomData.EnumerateInfo(materialName))
			{
				if (!flag && combinedPhysicMaterialInfo.characterFrictionMode != EPhysicsMaterialCharacterFrictionMode.ImmediatelyResponsive)
				{
					result.mode = combinedPhysicMaterialInfo.characterFrictionMode;
					flag = true;
				}
				if (!flag2 && combinedPhysicMaterialInfo.characterAccelerationMultiplier != null)
				{
					flag2 = true;
					result.accelerationMultiplier = combinedPhysicMaterialInfo.characterAccelerationMultiplier.Value;
				}
				if (!flag3 && combinedPhysicMaterialInfo.characterDecelerationMultiplier != null)
				{
					flag3 = true;
					result.decelerationMultiplier = combinedPhysicMaterialInfo.characterDecelerationMultiplier.Value;
				}
				if (!flag4 && combinedPhysicMaterialInfo.characterMaxSpeedMultiplier != null)
				{
					flag4 = true;
					result.maxSpeedMultiplier = combinedPhysicMaterialInfo.characterMaxSpeedMultiplier.Value;
				}
				if (flag && flag2 && flag3 && flag4)
				{
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Can crops be planted on a given material?
		/// </summary>
		// Token: 0x060019E4 RID: 6628 RVA: 0x0005CF78 File Offset: 0x0005B178
		public static bool IsArable(string materialName)
		{
			bool result = false;
			foreach (PhysicMaterialCustomData.CombinedPhysicMaterialInfo combinedPhysicMaterialInfo in PhysicMaterialCustomData.EnumerateInfo(materialName))
			{
				if (combinedPhysicMaterialInfo.isArable != null)
				{
					result = combinedPhysicMaterialInfo.isArable.Value;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Can oil drills be placed on a given material?
		/// </summary>
		// Token: 0x060019E5 RID: 6629 RVA: 0x0005CFE4 File Offset: 0x0005B1E4
		public static bool HasOil(string materialName)
		{
			bool result = false;
			foreach (PhysicMaterialCustomData.CombinedPhysicMaterialInfo combinedPhysicMaterialInfo in PhysicMaterialCustomData.EnumerateInfo(materialName))
			{
				if (combinedPhysicMaterialInfo.hasOil != null)
				{
					result = combinedPhysicMaterialInfo.hasOil.Value;
					break;
				}
			}
			return result;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0005D050 File Offset: 0x0005B250
		public static void RegisterAsset(PhysicsMaterialAsset asset)
		{
			PhysicMaterialCustomData.baseAssets[asset.GUID] = asset;
			PhysicMaterialCustomData.needsRebuild = true;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x0005D069 File Offset: 0x0005B269
		public static void RegisterAsset(PhysicsMaterialExtensionAsset asset)
		{
			PhysicMaterialCustomData.extensionAssets[asset.GUID] = asset;
			PhysicMaterialCustomData.needsRebuild = true;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0005D082 File Offset: 0x0005B282
		public static Dictionary<Guid, PhysicsMaterialAsset> GetAssets()
		{
			return PhysicMaterialCustomData.baseAssets;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0005D089 File Offset: 0x0005B289
		public static Dictionary<Guid, PhysicsMaterialExtensionAsset> GetExtensionAssets()
		{
			return PhysicMaterialCustomData.extensionAssets;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0005D090 File Offset: 0x0005B290
		private static List<PhysicMaterialCustomData.CombinedPhysicMaterialInfo> EnumerateInfo(string materialName)
		{
			PhysicMaterialCustomData.enumerableInfos.Clear();
			if (string.IsNullOrEmpty(materialName))
			{
				return PhysicMaterialCustomData.enumerableInfos;
			}
			if (PhysicMaterialCustomData.needsRebuild)
			{
				PhysicMaterialCustomData.needsRebuild = false;
				PhysicMaterialCustomData.Rebuild();
			}
			PhysicMaterialCustomData.CombinedPhysicMaterialInfo fallback;
			if (PhysicMaterialCustomData.nameInfos.TryGetValue(materialName, ref fallback))
			{
				do
				{
					PhysicMaterialCustomData.enumerableInfos.Add(fallback);
					fallback = fallback.fallback;
				}
				while (fallback != null);
			}
			return PhysicMaterialCustomData.enumerableInfos;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0005D0F0 File Offset: 0x0005B2F0
		private static void PopulateInfo(PhysicMaterialCustomData.CombinedPhysicMaterialInfo info, PhysicsMaterialAssetBase asset)
		{
			if (asset.audioDefs != null)
			{
				foreach (KeyValuePair<string, MasterBundleReference<OneShotAudioDefinition>> keyValuePair in asset.audioDefs)
				{
					info.audioDefs.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0005D160 File Offset: 0x0005B360
		private static void Rebuild()
		{
			PhysicMaterialCustomData.nameInfos.Clear();
			Dictionary<Guid, PhysicMaterialCustomData.CombinedPhysicMaterialInfo> dictionary = new Dictionary<Guid, PhysicMaterialCustomData.CombinedPhysicMaterialInfo>();
			foreach (KeyValuePair<Guid, PhysicsMaterialAsset> keyValuePair in PhysicMaterialCustomData.baseAssets)
			{
				PhysicsMaterialAsset value = keyValuePair.Value;
				PhysicMaterialCustomData.CombinedPhysicMaterialInfo combinedPhysicMaterialInfo = null;
				foreach (string text in value.physicMaterialNames)
				{
					if (PhysicMaterialCustomData.nameInfos.TryGetValue(text, ref combinedPhysicMaterialInfo))
					{
						Assets.reportError(value, "physics material name \"" + text + "\" already taken by " + combinedPhysicMaterialInfo.baseAsset.name);
						break;
					}
				}
				if (combinedPhysicMaterialInfo == null)
				{
					if (dictionary.TryGetValue(value.GUID, ref combinedPhysicMaterialInfo))
					{
						Assets.reportError(value, string.Format("guid \"{0}\" already taken by {1}", value.GUID, combinedPhysicMaterialInfo.baseAsset.name));
					}
					else
					{
						combinedPhysicMaterialInfo = new PhysicMaterialCustomData.CombinedPhysicMaterialInfo();
						combinedPhysicMaterialInfo.baseAsset = value;
						foreach (string text2 in value.physicMaterialNames)
						{
							PhysicMaterialCustomData.nameInfos[text2] = combinedPhysicMaterialInfo;
						}
						dictionary[value.GUID] = combinedPhysicMaterialInfo;
						combinedPhysicMaterialInfo.bulletImpactEffect = value.bulletImpactEffect;
						combinedPhysicMaterialInfo.tireMotionEffect = value.tireMotionEffect;
						combinedPhysicMaterialInfo.characterFrictionMode = value.characterFrictionMode;
						combinedPhysicMaterialInfo.isArable = value.isArable;
						combinedPhysicMaterialInfo.hasOil = value.hasOil;
						combinedPhysicMaterialInfo.characterAccelerationMultiplier = value.characterAccelerationMultiplier;
						combinedPhysicMaterialInfo.characterDecelerationMultiplier = value.characterDecelerationMultiplier;
						combinedPhysicMaterialInfo.characterMaxSpeedMultiplier = value.characterMaxSpeedMultiplier;
						PhysicMaterialCustomData.PopulateInfo(combinedPhysicMaterialInfo, value);
					}
				}
			}
			foreach (KeyValuePair<string, PhysicMaterialCustomData.CombinedPhysicMaterialInfo> keyValuePair2 in PhysicMaterialCustomData.nameInfos)
			{
				PhysicMaterialCustomData.CombinedPhysicMaterialInfo value2 = keyValuePair2.Value;
				if (value2.baseAsset.fallbackRef.isValid && !dictionary.TryGetValue(value2.baseAsset.fallbackRef.GUID, ref value2.fallback))
				{
					Assets.reportError(value2.baseAsset, string.Format("unable to find fallback asset {0}", value2.baseAsset.fallbackRef));
				}
			}
			foreach (KeyValuePair<Guid, PhysicsMaterialExtensionAsset> keyValuePair3 in PhysicMaterialCustomData.extensionAssets)
			{
				PhysicsMaterialExtensionAsset value3 = keyValuePair3.Value;
				PhysicMaterialCustomData.CombinedPhysicMaterialInfo info;
				if (!dictionary.TryGetValue(value3.baseRef.GUID, ref info))
				{
					Assets.reportError(value3, string.Format("unable to find base asset {0}", value3.baseRef));
				}
				else
				{
					PhysicMaterialCustomData.PopulateInfo(info, value3);
				}
			}
		}

		// Token: 0x04000BCD RID: 3021
		private static Dictionary<string, PhysicMaterialCustomData.CombinedPhysicMaterialInfo> nameInfos = new Dictionary<string, PhysicMaterialCustomData.CombinedPhysicMaterialInfo>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000BCE RID: 3022
		private static Dictionary<Guid, PhysicsMaterialAsset> baseAssets = new Dictionary<Guid, PhysicsMaterialAsset>();

		// Token: 0x04000BCF RID: 3023
		private static Dictionary<Guid, PhysicsMaterialExtensionAsset> extensionAssets = new Dictionary<Guid, PhysicsMaterialExtensionAsset>();

		// Token: 0x04000BD0 RID: 3024
		private static List<PhysicMaterialCustomData.CombinedPhysicMaterialInfo> enumerableInfos = new List<PhysicMaterialCustomData.CombinedPhysicMaterialInfo>();

		// Token: 0x04000BD1 RID: 3025
		private static bool needsRebuild = false;

		// Token: 0x02000925 RID: 2341
		private class CombinedPhysicMaterialInfo
		{
			// Token: 0x04003278 RID: 12920
			public PhysicsMaterialAsset baseAsset;

			// Token: 0x04003279 RID: 12921
			public PhysicMaterialCustomData.CombinedPhysicMaterialInfo fallback;

			// Token: 0x0400327A RID: 12922
			public Dictionary<string, MasterBundleReference<OneShotAudioDefinition>> audioDefs = new Dictionary<string, MasterBundleReference<OneShotAudioDefinition>>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x0400327B RID: 12923
			public AssetReference<EffectAsset> bulletImpactEffect;

			// Token: 0x0400327C RID: 12924
			public AssetReference<EffectAsset> tireMotionEffect;

			// Token: 0x0400327D RID: 12925
			public EPhysicsMaterialCharacterFrictionMode characterFrictionMode;

			// Token: 0x0400327E RID: 12926
			public bool? isArable;

			// Token: 0x0400327F RID: 12927
			public bool? hasOil;

			// Token: 0x04003280 RID: 12928
			public float? characterAccelerationMultiplier;

			// Token: 0x04003281 RID: 12929
			public float? characterDecelerationMultiplier;

			// Token: 0x04003282 RID: 12930
			public float? characterMaxSpeedMultiplier;
		}
	}
}
