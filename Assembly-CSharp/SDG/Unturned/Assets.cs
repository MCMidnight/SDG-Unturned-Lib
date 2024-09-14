using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Devkit;
using SDG.Framework.Modules;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unturned.SystemEx;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x02000290 RID: 656
	public class Assets : MonoBehaviour
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00046F39 File Offset: 0x00045139
		public static TypeRegistryDictionary assetTypes
		{
			get
			{
				return Assets._assetTypes;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00046F40 File Offset: 0x00045140
		public static TypeRegistryDictionary useableTypes
		{
			get
			{
				return Assets._useableTypes;
			}
		}

		/// <summary>
		/// Has initial client UGC loading step run yet?
		/// Used to defer asset loading for workshop installs that occured during startup.
		/// </summary>
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00046F47 File Offset: 0x00045147
		// (set) Token: 0x06001383 RID: 4995 RVA: 0x00046F4E File Offset: 0x0004514E
		public static bool hasLoadedUgc { get; protected set; }

		/// <summary>
		/// Has initial map loading step run yet?
		/// Used to defer map loading for workshop installs that occured during startup.
		/// </summary>
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x00046F56 File Offset: 0x00045156
		// (set) Token: 0x06001385 RID: 4997 RVA: 0x00046F5D File Offset: 0x0004515D
		public static bool hasLoadedMaps { get; protected set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x00046F65 File Offset: 0x00045165
		public static bool isLoading
		{
			get
			{
				return Assets.isLoadingAllAssets || Assets.isLoadingFromUpdate;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x00046F75 File Offset: 0x00045175
		internal static bool ShouldWaitForNewAssetsToFinishLoading
		{
			get
			{
				return Assets.isLoading || Assets.instance.worker.IsWorking;
			}
		}

		/// <summary>
		/// Should some specific asset types which opt-in be allowed to defer loading from asset bundles until used?
		/// Disabled by asset validation because all assets need to be loaded.
		/// </summary>
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00046F8F File Offset: 0x0004518F
		public static bool shouldDeferLoadingAssets
		{
			get
			{
				return Assets.wantsDeferLoadingAssets && !Assets.shouldValidateAssets;
			}
		}

		/// <summary>
		/// While an asset is being loaded, this is the master bundle for that asset.
		/// Used by master bundle pointer as a default.
		/// </summary>
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x00046FAC File Offset: 0x000451AC
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x00046FB3 File Offset: 0x000451B3
		public static MasterBundleConfig currentMasterBundle { get; private set; }

		// Token: 0x0600138B RID: 5003 RVA: 0x00046FBB File Offset: 0x000451BB
		private static string getExceptionMessage(Exception e)
		{
			if (e == null)
			{
				return "Exception = Null";
			}
			if (e.InnerException != null)
			{
				return e.InnerException.Message;
			}
			return e.Message;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00046FE0 File Offset: 0x000451E0
		public static void reportError(string error)
		{
			Assets.errors.Add(error);
			UnturnedLog.warn(error);
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00046FF3 File Offset: 0x000451F3
		public static void reportError(Asset offendingAsset, string error)
		{
			error = offendingAsset.getTypeNameAndIdDisplayString() + ": " + error;
			Assets.reportError(error);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00047010 File Offset: 0x00045210
		public static void reportError(Asset offendingAsset, string format, params object[] args)
		{
			string error = string.Format(format, args);
			Assets.reportError(offendingAsset, error);
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0004702C File Offset: 0x0004522C
		public static void reportError(Asset offendingAsset, string format, object arg0)
		{
			string error = string.Format(format, arg0);
			Assets.reportError(offendingAsset, error);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00047048 File Offset: 0x00045248
		public static void reportError(Asset offendingAsset, string format, object arg0, object arg1)
		{
			string error = string.Format(format, arg0, arg1);
			Assets.reportError(offendingAsset, error);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00047068 File Offset: 0x00045268
		public static void reportError(Asset offendingAsset, string format, object arg0, object arg1, object arg2)
		{
			string error = string.Format(format, arg0, arg1, arg2);
			Assets.reportError(offendingAsset, error);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00047087 File Offset: 0x00045287
		public static List<string> getReportedErrorsList()
		{
			return Assets.errors;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00047090 File Offset: 0x00045290
		internal static AssetOrigin FindWorkshopFileOrigin(ulong workshopFileId)
		{
			foreach (AssetOrigin assetOrigin in Assets.assetOrigins)
			{
				if (assetOrigin.workshopFileId == workshopFileId)
				{
					return assetOrigin;
				}
			}
			return null;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000470EC File Offset: 0x000452EC
		private static AssetOrigin FindLevelOrigin(LevelInfo level)
		{
			if (level.publishedFileId != 0UL)
			{
				return Assets.FindWorkshopFileOrigin(level.publishedFileId);
			}
			string text = "Map \"" + level.name + "\"";
			foreach (AssetOrigin assetOrigin in Assets.assetOrigins)
			{
				if (string.Equals(assetOrigin.name, text))
				{
					return assetOrigin;
				}
			}
			return null;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00047178 File Offset: 0x00045378
		internal static AssetOrigin FindOrAddWorkshopFileOrigin(ulong workshopFileId, bool shouldOverrideIds)
		{
			AssetOrigin assetOrigin = Assets.FindWorkshopFileOrigin(workshopFileId);
			if (assetOrigin != null)
			{
				return assetOrigin;
			}
			AssetOrigin assetOrigin2 = new AssetOrigin();
			assetOrigin2.name = string.Format("Workshop File ({0})", workshopFileId);
			assetOrigin2.workshopFileId = workshopFileId;
			assetOrigin2.shouldAssetsOverrideExistingIds = shouldOverrideIds;
			Assets.assetOrigins.Add(assetOrigin2);
			return assetOrigin2;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000471C8 File Offset: 0x000453C8
		internal static AssetOrigin FindOrAddLevelOrigin(LevelInfo level)
		{
			if (level.publishedFileId != 0UL)
			{
				return Assets.FindOrAddWorkshopFileOrigin(level.publishedFileId, false);
			}
			string text = "Map \"" + level.name + "\"";
			foreach (AssetOrigin assetOrigin in Assets.assetOrigins)
			{
				if (string.Equals(assetOrigin.name, text))
				{
					return assetOrigin;
				}
			}
			AssetOrigin assetOrigin2 = new AssetOrigin();
			assetOrigin2.name = text;
			Assets.assetOrigins.Add(assetOrigin2);
			return assetOrigin2;
		}

		/// <summary>
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		// Token: 0x06001397 RID: 5015 RVA: 0x00047270 File Offset: 0x00045470
		public static Asset find(EAssetType type, ushort id)
		{
			if (type == EAssetType.NONE || id == 0)
			{
				return null;
			}
			Asset asset;
			Assets.currentAssetMapping.legacyAssetsTable[type].TryGetValue(id, ref asset);
			int num = 0;
			do
			{
				RedirectorAsset redirectorAsset = asset as RedirectorAsset;
				if (redirectorAsset == null)
				{
					return asset;
				}
				Assets.currentAssetMapping.assetDictionary.TryGetValue(redirectorAsset.TargetGuid, ref asset);
				num++;
			}
			while (num <= 32);
			asset = null;
			UnturnedLog.warn(string.Format("Infinite asset director loop encountered when resolving Type: {0} Legacy ID: {1}", type, id));
			return asset;
		}

		/// <summary>
		/// Find an asset by GUID reference.
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		/// <returns>Asset with matching GUID if it exists, null otherwise.</returns>
		// Token: 0x06001398 RID: 5016 RVA: 0x000472EC File Offset: 0x000454EC
		public static T find<T>(AssetReference<T> reference) where T : Asset
		{
			if (!reference.isValid)
			{
				return default(T);
			}
			return Assets.find(reference.GUID) as T;
		}

		/// <summary>
		/// Find an asset by GUID reference.
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// Maybe considered a hack? Ignores the current per-server asset mapping.
		/// </summary>
		/// <returns>Asset with matching GUID if it exists, null otherwise.</returns>
		// Token: 0x06001399 RID: 5017 RVA: 0x00047324 File Offset: 0x00045524
		public static T Find_UseDefaultAssetMapping<T>(AssetReference<T> reference) where T : Asset
		{
			if (reference.isNull)
			{
				return default(T);
			}
			Asset asset;
			Assets.defaultAssetMapping.assetDictionary.TryGetValue(reference.GUID, ref asset);
			int num = 0;
			do
			{
				RedirectorAsset redirectorAsset = asset as RedirectorAsset;
				if (redirectorAsset == null)
				{
					goto IL_70;
				}
				Assets.currentAssetMapping.assetDictionary.TryGetValue(redirectorAsset.TargetGuid, ref asset);
				num++;
			}
			while (num <= 32);
			asset = null;
			UnturnedLog.warn(string.Format("Infinite asset director loop encountered when resolving: {0}", reference));
			IL_70:
			return asset as T;
		}

		/// <summary>
		/// Load content from an assetbundle.
		/// </summary>
		// Token: 0x0600139A RID: 5018 RVA: 0x000473AC File Offset: 0x000455AC
		public static T load<T>(ContentReference<T> reference) where T : Object
		{
			if (!reference.isValid)
			{
				return default(T);
			}
			MasterBundleConfig masterBundleConfig = Assets.findMasterBundleByName(reference.name, true);
			if (masterBundleConfig != null && masterBundleConfig.assetBundle != null)
			{
				string text = masterBundleConfig.formatAssetPath(reference.path);
				T t = masterBundleConfig.assetBundle.LoadAsset<T>(text);
				if (t == null)
				{
					UnturnedLog.warn("Failed to load content reference '{0}' from master bundle '{1}' as {2}", new object[]
					{
						text,
						reference.name,
						typeof(T).Name
					});
				}
				return t;
			}
			return default(T);
		}

		/// <summary>
		/// Find an asset by GUID reference.
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		/// <returns>Asset with matching GUID if it exists, null otherwise.</returns>
		// Token: 0x0600139B RID: 5019 RVA: 0x00047450 File Offset: 0x00045650
		public static Asset find(Guid GUID)
		{
			Asset asset;
			Assets.currentAssetMapping.assetDictionary.TryGetValue(GUID, ref asset);
			int num = 0;
			do
			{
				RedirectorAsset redirectorAsset = asset as RedirectorAsset;
				if (redirectorAsset == null)
				{
					return asset;
				}
				Assets.currentAssetMapping.assetDictionary.TryGetValue(redirectorAsset.TargetGuid, ref asset);
				num++;
			}
			while (num <= 32);
			asset = null;
			UnturnedLog.warn(string.Format("Infinite asset director loop encountered when resolving: {0}", GUID));
			return asset;
		}

		/// <summary>
		/// Find an asset by GUID reference.
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		/// <returns>Asset with matching GUID if it exists, null otherwise.</returns>
		// Token: 0x0600139C RID: 5020 RVA: 0x000474B5 File Offset: 0x000456B5
		public static T find<T>(Guid guid) where T : Asset
		{
			return Assets.find(guid) as T;
		}

		/// <summary>
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		// Token: 0x0600139D RID: 5021 RVA: 0x000474C7 File Offset: 0x000456C7
		public static EffectAsset FindEffectAssetByGuidOrLegacyId(Guid guid, ushort legacyId)
		{
			if (GuidExtension.IsEmpty(guid))
			{
				return Assets.find(EAssetType.EFFECT, legacyId) as EffectAsset;
			}
			return Assets.find<EffectAsset>(guid);
		}

		/// <summary>
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		// Token: 0x0600139E RID: 5022 RVA: 0x000474E4 File Offset: 0x000456E4
		public static T FindNpcAssetByGuidOrLegacyId<T>(Guid guid, ushort legacyId) where T : Asset
		{
			if (GuidExtension.IsEmpty(guid))
			{
				return Assets.find(EAssetType.NPC, legacyId) as T;
			}
			return Assets.find<T>(guid);
		}

		/// <summary>
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// Note: this method doesn't handle redirects by VehicleRedirectorAsset.
		/// </summary>
		// Token: 0x0600139F RID: 5023 RVA: 0x00047507 File Offset: 0x00045707
		public static VehicleAsset FindVehicleAssetByGuidOrLegacyId(Guid guid, ushort legacyId)
		{
			if (GuidExtension.IsEmpty(guid))
			{
				return Assets.find(EAssetType.VEHICLE, legacyId) as VehicleAsset;
			}
			return Assets.find<VehicleAsset>(guid);
		}

		/// <summary>
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// Note: this method doesn't handle redirects by VehicleRedirectorAsset.
		/// </summary>
		// Token: 0x060013A0 RID: 5024 RVA: 0x00047524 File Offset: 0x00045724
		public static Asset FindBaseVehicleAssetByGuidOrLegacyId(Guid guid, ushort legacyId)
		{
			if (GuidExtension.IsEmpty(guid))
			{
				return Assets.find(EAssetType.VEHICLE, legacyId);
			}
			return Assets.find(guid);
		}

		/// <summary>
		/// This method supports <see cref="T:SDG.Unturned.RedirectorAsset" />.
		/// </summary>
		// Token: 0x060013A1 RID: 5025 RVA: 0x0004753C File Offset: 0x0004573C
		internal static T FindItemByGuidOrLegacyId<T>(Guid guid, ushort legacyId) where T : ItemAsset
		{
			if (GuidExtension.IsEmpty(guid))
			{
				return Assets.find(EAssetType.ITEM, legacyId) as T;
			}
			return Assets.find<T>(guid);
		}

		/// <summary>
		/// Append assets that extend from result type.
		/// </summary>
		// Token: 0x060013A2 RID: 5026 RVA: 0x0004755E File Offset: 0x0004575E
		public static void find<T>(List<T> results) where T : Asset
		{
			Assets.FindAssetsInListByType<T>(Assets.currentAssetMapping.assetList, results);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00047570 File Offset: 0x00045770
		internal static bool HasDefaultAssetMappingChanged(ref int counter)
		{
			bool result = Assets.defaultAssetMapping.modificationCounter != counter;
			counter = Assets.defaultAssetMapping.modificationCounter;
			return result;
		}

		/// <summary>
		/// Maybe considered a hack? Ignores the current per-server asset mapping.
		/// Append assets that extend from result type.
		/// </summary>
		// Token: 0x060013A4 RID: 5028 RVA: 0x0004758F File Offset: 0x0004578F
		internal static void FindAssetsByType_UseDefaultAssetMapping<T>(List<T> results) where T : Asset
		{
			Assets.FindAssetsInListByType<T>(Assets.defaultAssetMapping.assetList, results);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000475A4 File Offset: 0x000457A4
		private static void FindAssetsInListByType<T>(List<Asset> assetList, List<T> results) where T : Asset
		{
			foreach (Asset asset in assetList)
			{
				T t = asset as T;
				if (t != null)
				{
					results.Add(t);
				}
			}
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00047604 File Offset: 0x00045804
		public static Asset findByAbsolutePath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			path = Path.GetFullPath(path);
			foreach (Asset asset in Assets.currentAssetMapping.assetList)
			{
				if (path.Equals(asset.absoluteOriginFilePath))
				{
					return asset;
				}
			}
			return null;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0004767C File Offset: 0x0004587C
		internal static Asset CreateAtRuntime(Type type, ushort legacyId)
		{
			try
			{
				Asset asset = Activator.CreateInstance(type) as Asset;
				if (asset != null)
				{
					asset.GUID = Guid.NewGuid();
					asset.id = legacyId;
					Assets.AddToMapping(asset, false, Assets.defaultAssetMapping);
					if (asset is IDirtyable)
					{
						(asset as IDirtyable).isDirty = true;
					}
					asset.OnCreatedAtRuntime();
					return asset;
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e);
			}
			return null;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000476F0 File Offset: 0x000458F0
		internal static T CreateAtRuntime<T>(ushort legacyId) where T : Asset
		{
			return Assets.CreateAtRuntime(typeof(T), legacyId) as T;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0004770C File Offset: 0x0004590C
		internal static void AddToMapping(Asset asset, bool overrideExistingID, Assets.AssetMapping assetMapping)
		{
			if (asset == null)
			{
				return;
			}
			EAssetType assetCategory = asset.assetCategory;
			if (assetCategory == EAssetType.SPAWN)
			{
				Assets.hasUnlinkedSpawns = true;
			}
			bool flag = false;
			if (assetCategory == EAssetType.OBJECT)
			{
				if (overrideExistingID)
				{
					Asset asset2;
					if (assetMapping.legacyAssetsTable[assetCategory].TryGetValue(asset.id, ref asset2))
					{
						assetMapping.legacyAssetsTable[assetCategory].Remove(asset.id);
						asset2.hasBeenReplaced = true;
						flag = true;
					}
					assetMapping.legacyAssetsTable[assetCategory].Add(asset.id, asset);
				}
				else if (!assetMapping.legacyAssetsTable[assetCategory].ContainsKey(asset.id))
				{
					assetMapping.legacyAssetsTable[assetCategory].Add(asset.id, asset);
				}
			}
			else if (assetCategory != EAssetType.NONE)
			{
				if (asset.id != 0)
				{
					if (overrideExistingID)
					{
						Asset asset3;
						if (assetMapping.legacyAssetsTable[assetCategory].TryGetValue(asset.id, ref asset3))
						{
							assetMapping.legacyAssetsTable[assetCategory].Remove(asset.id);
							asset3.hasBeenReplaced = true;
							flag = true;
						}
					}
					else if (assetMapping.legacyAssetsTable[assetCategory].ContainsKey(asset.id))
					{
						Asset asset4;
						assetMapping.legacyAssetsTable[assetCategory].TryGetValue(asset.id, ref asset4);
						Assets.reportError(asset, "short ID is already taken by " + asset4.getTypeNameAndIdDisplayString() + "!");
						return;
					}
					assetMapping.legacyAssetsTable[assetCategory].Add(asset.id, asset);
				}
				else
				{
					bool flag2;
					switch (assetCategory)
					{
					case EAssetType.ITEM:
					{
						ItemAsset itemAsset = asset as ItemAsset;
						flag2 = (itemAsset == null || !itemAsset.isPro);
						goto IL_1BA;
					}
					case EAssetType.EFFECT:
					case EAssetType.VEHICLE:
						break;
					case EAssetType.OBJECT:
					case EAssetType.RESOURCE:
						goto IL_1B7;
					default:
						if (assetCategory - EAssetType.SPAWN > 1)
						{
							goto IL_1B7;
						}
						break;
					}
					flag2 = false;
					goto IL_1BA;
					IL_1B7:
					flag2 = true;
					IL_1BA:
					if (flag2)
					{
						Assets.reportError(asset, "needs a non-zero ID");
					}
				}
			}
			if (asset.GUID != Guid.Empty)
			{
				if (overrideExistingID)
				{
					Asset asset5;
					if (assetMapping.assetDictionary.TryGetValue(asset.GUID, ref asset5))
					{
						assetMapping.assetDictionary.Remove(asset5.GUID);
						assetMapping.assetList.Remove(asset5);
						asset5.hasBeenReplaced = true;
						flag = true;
					}
				}
				else if (assetMapping.assetDictionary.ContainsKey(asset.GUID))
				{
					Asset asset6;
					assetMapping.assetDictionary.TryGetValue(asset.GUID, ref asset6);
					Assets.reportError(asset, string.Concat(new string[]
					{
						"long GUID ",
						asset.GUID.ToString("N"),
						" is already taken by ",
						asset6.getTypeNameAndIdDisplayString(),
						"!"
					}));
					return;
				}
				assetMapping.assetDictionary.Add(asset.GUID, asset);
				assetMapping.assetList.Add(asset);
			}
			assetMapping.modificationCounter++;
			if (flag && assetCategory == EAssetType.VEHICLE && Level.isLoaded && Provider.isServer && VehicleManager.vehicles != null && VehicleManager.vehicles.Count > 0)
			{
				VehicleManager.shouldRespawnReloadedVehicles = true;
			}
			if (asset.origin != null && asset.origin.workshopFileId != 0UL && Assets.shouldLogWorkshopAssets)
			{
				UnturnedLog.info(asset.getTypeNameAndIdDisplayString());
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00047A34 File Offset: 0x00045C34
		private static void AddAssetsFromOriginToCurrentMapping(AssetOrigin origin)
		{
			UnturnedLog.info(string.Format("Adding {0} asset(s) from origin {1} to server mapping", origin.assets.Count, origin.name));
			foreach (Asset asset in origin.assets)
			{
				Assets.AddToMapping(asset, true, Assets.currentAssetMapping);
			}
		}

		/// <summary>
		/// While playing on server the client will use the same dictionary/list of assets the server uses in order
		/// to reduce issues with ID conflicts.
		///
		/// 2023-05-27: server now ALSO uses the same logic to ensure IDs are applied in consistent order regardless
		/// of multi-threaded loading order.
		/// </summary>
		// Token: 0x060013AB RID: 5035 RVA: 0x00047AB0 File Offset: 0x00045CB0
		internal static void ApplyServerAssetMapping(LevelInfo pendingLevel, List<PublishedFileId_t> serverWorkshopFileIds)
		{
			Assets.currentAssetMapping = new Assets.AssetMapping();
			List<AssetOrigin> list = new List<AssetOrigin>();
			list.Add(Assets.coreOrigin);
			AssetOrigin assetOrigin = null;
			if (pendingLevel != null)
			{
				assetOrigin = Assets.FindLevelOrigin(pendingLevel);
				if (assetOrigin != null)
				{
					list.Add(assetOrigin);
				}
			}
			if (serverWorkshopFileIds != null)
			{
				foreach (PublishedFileId_t publishedFileId_t in serverWorkshopFileIds)
				{
					AssetOrigin assetOrigin2 = Assets.FindWorkshopFileOrigin(publishedFileId_t.m_PublishedFileId);
					if (assetOrigin2 != null)
					{
						if (assetOrigin2 != assetOrigin)
						{
							list.Add(assetOrigin2);
						}
					}
					else
					{
						UnturnedLog.info(string.Format("Unable to find assets for server mapping (file ID {0})", publishedFileId_t));
					}
				}
			}
			foreach (AssetOrigin assetOrigin3 in Assets.assetOrigins)
			{
				if (assetOrigin3 != Assets.reloadOrigin && assetOrigin3.assets.Count >= 1 && !list.Contains(assetOrigin3))
				{
					UnturnedLog.info("Inserting asset origin " + assetOrigin3.name + " before other assets to reduce chances of ID conflicts because otherwise it was not included");
					list.Insert(0, assetOrigin3);
				}
			}
			foreach (AssetOrigin origin in list)
			{
				Assets.AddAssetsFromOriginToCurrentMapping(origin);
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00047C20 File Offset: 0x00045E20
		internal static void ClearServerAssetMapping()
		{
			Assets.currentAssetMapping = Assets.defaultAssetMapping;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00047C2C File Offset: 0x00045E2C
		public static void RequestReloadAllAssets()
		{
			if (Assets.hasFinishedInitialStartupLoading && !Assets.isLoading)
			{
				Assets.instance.StartCoroutine(Assets.instance.LoadAllAssets());
			}
		}

		/// <summary>
		/// Search all loaded master bundles for one in path's hierarchy.
		/// </summary>
		// Token: 0x060013AE RID: 5038 RVA: 0x00047C54 File Offset: 0x00045E54
		public static MasterBundleConfig findMasterBundleByPath(string path)
		{
			int num = 0;
			MasterBundleConfig result = null;
			foreach (MasterBundleConfig masterBundleConfig in Assets.allMasterBundles)
			{
				if (masterBundleConfig.directoryPath.Length >= num && path.StartsWith(masterBundleConfig.directoryPath))
				{
					if (path.Length > masterBundleConfig.directoryPath.Length)
					{
						char c = path.get_Chars(masterBundleConfig.directoryPath.Length);
						if (c != '/' && c != '\\')
						{
							continue;
						}
					}
					num = masterBundleConfig.directoryPath.Length;
					result = masterBundleConfig;
				}
			}
			return result;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00047D00 File Offset: 0x00045F00
		public static MasterBundleConfig findMasterBundleInListByName(List<MasterBundleConfig> list, string name, bool matchExtension = true)
		{
			foreach (MasterBundleConfig masterBundleConfig in list)
			{
				if ((matchExtension ? masterBundleConfig.assetBundleName : masterBundleConfig.assetBundleNameWithoutExtension).Equals(name, 3))
				{
					return masterBundleConfig;
				}
			}
			return null;
		}

		/// <summary>
		/// Find loaded master bundle by name.
		/// </summary>
		// Token: 0x060013B0 RID: 5040 RVA: 0x00047D68 File Offset: 0x00045F68
		public static MasterBundleConfig findMasterBundleByName(string name, bool matchExtension = true)
		{
			return Assets.findMasterBundleInListByName(Assets.allMasterBundles, name, matchExtension);
		}

		/// <summary>
		/// Unload all asset bundles from memory, and empty known list.
		/// Called when reloading assets.
		/// </summary>
		// Token: 0x060013B1 RID: 5041 RVA: 0x00047D78 File Offset: 0x00045F78
		private static void UnloadAllMasterBundles()
		{
			foreach (MasterBundleConfig masterBundleConfig in Assets.allMasterBundles)
			{
				masterBundleConfig.unload();
			}
			Assets.allMasterBundles.Clear();
		}

		/// <summary>
		/// Catches exceptions thrown by LoadFile to avoid breaking loading.
		/// </summary>
		// Token: 0x060013B2 RID: 5042 RVA: 0x00047DD4 File Offset: 0x00045FD4
		private static void TryLoadFile(AssetsWorker.AssetDefinition file)
		{
			try
			{
				Assets.loadingStats.totalFilesLoaded++;
				Assets.LoadFile(file);
			}
			catch (Exception e)
			{
				UnturnedLog.error("Exception loading file {0}:", new object[]
				{
					file.path
				});
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00047E30 File Offset: 0x00046030
		private static void LoadFile(AssetsWorker.AssetDefinition file)
		{
			string path = file.path;
			DatDictionary assetData = file.assetData;
			byte[] array = file.hash;
			if (!string.IsNullOrEmpty(file.assetError))
			{
				Assets.reportError(string.Concat(new string[]
				{
					"Error parsing \"",
					path,
					"\": \"",
					file.assetError,
					"\""
				}));
			}
			string directoryName = Path.GetDirectoryName(path);
			string text = path.EndsWith("Asset.dat", 5) ? Path.GetFileName(directoryName) : Path.GetFileNameWithoutExtension(path);
			Guid guid = default(Guid);
			Type type = null;
			DatDictionary datDictionary;
			if (assetData.TryGetDictionary("Metadata", out datDictionary))
			{
				if (!datDictionary.TryParseGuid("GUID", out guid))
				{
					Assets.reportError("Unable to parse Metadata.GUID in \"" + path + "\"");
					return;
				}
				type = datDictionary.ParseType("Type", null);
				if (type == null)
				{
					Assets.reportError("Unable to parse Metadata.Type in \"" + path + "\"");
					return;
				}
			}
			else
			{
				if (!assetData.ContainsKey("GUID"))
				{
					guid = Guid.NewGuid();
					try
					{
						string text2 = File.ReadAllText(path);
						text2 = "GUID " + guid.ToString("N") + "\n" + text2;
						File.WriteAllText(path, text2);
						UnturnedLog.info(string.Format("Assigned GUID {0:N} to asset \"{1}\"", guid, path));
						goto IL_186;
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "Caught IO exception adding GUID to \"" + path + "\":");
						goto IL_186;
					}
				}
				if (!assetData.TryParseGuid("GUID", out guid))
				{
					Assets.reportError("Unable to parse GUID in \"" + path + "\"");
					return;
				}
			}
			IL_186:
			if (GuidExtension.IsEmpty(guid))
			{
				Assets.reportError("Cannot use empty GUID in \"" + path + "\"");
				return;
			}
			DatDictionary datDictionary2 = assetData;
			DatDictionary datDictionary3;
			if (assetData.TryGetDictionary("Asset", out datDictionary3))
			{
				datDictionary2 = datDictionary3;
			}
			if (type == null)
			{
				string @string = datDictionary2.GetString("Type", null);
				if (string.IsNullOrEmpty(@string))
				{
					Assets.reportError("Missing asset Type in \"" + path + "\"");
					return;
				}
				type = Assets.assetTypes.getType(@string);
				if (type == null)
				{
					type = datDictionary2.ParseType("Type", null);
					if (type == null)
					{
						Assets.reportError(string.Concat(new string[]
						{
							"Unhandled asset type \"",
							@string,
							"\" in \"",
							path,
							"\""
						}));
						return;
					}
				}
			}
			if (!typeof(Asset).IsAssignableFrom(type))
			{
				Assets.reportError(string.Format("Type \"{0}\" is not a valid asset type in \"{1}\"", type, path));
				return;
			}
			MasterBundleConfig masterBundleConfig = Assets.findMasterBundleByPath(path);
			string string2 = datDictionary2.GetString("Master_Bundle_Override", null);
			if (string2 != null)
			{
				masterBundleConfig = Assets.findMasterBundleByName(string2, true);
				if (masterBundleConfig == null)
				{
					UnturnedLog.warn("Unable to find master bundle override '{0}' for '{1}'", new object[]
					{
						string2,
						path
					});
				}
			}
			else if (datDictionary2.ContainsKey("Exclude_From_Master_Bundle"))
			{
				masterBundleConfig = null;
			}
			if (masterBundleConfig != null && masterBundleConfig.assetBundle == null)
			{
				UnturnedLog.warn("Skipping master bundle '{0}' for '{1}' because asset bundle is null", new object[]
				{
					masterBundleConfig.assetBundleName,
					path
				});
				masterBundleConfig = null;
			}
			Assets.currentMasterBundle = masterBundleConfig;
			int a = -1;
			Bundle bundle;
			if (masterBundleConfig != null)
			{
				string text3;
				if (!datDictionary2.TryGetString("Bundle_Override_Path", out text3))
				{
					text3 = directoryName.Substring(masterBundleConfig.directoryPath.Length);
					text3 = text3.Replace('\\', '/');
				}
				bundle = new MasterBundle(masterBundleConfig, text3, text);
				string text4 = text3.ToLowerInvariant() + "/" + text.ToLowerInvariant();
				array = Hash.combine(new byte[][]
				{
					array,
					Hash.SHA1(text4)
				});
				a = masterBundleConfig.version;
			}
			else if (datDictionary2.ContainsKey("Bundle_Override_Path"))
			{
				string text5 = datDictionary2.GetString("Bundle_Override_Path", null);
				int num = text5.LastIndexOf('/');
				string text6;
				if (num == -1)
				{
					text6 = text5;
				}
				else
				{
					text6 = text5.Substring(num + 1);
				}
				text5 = text5 + "/" + text6 + ".unity3d";
				bundle = new Bundle(text5, false, text);
			}
			else
			{
				bundle = new Bundle(directoryName + "/" + text + ".unity3d", false, null);
			}
			int num2 = datDictionary2.ParseInt32("Asset_Bundle_Version", 1);
			if (num2 < 1)
			{
				Assets.reportError(text + " Lowest individual asset bundle version is 1 (default), associated with 5.5.");
				num2 = 1;
			}
			else if (num2 > 5)
			{
				Assets.reportError(text + " Highest individual asset bundle version is 5, associated with 2021 LTS.");
				num2 = 5;
			}
			int num3 = Mathf.Max(a, num2);
			bundle.convertShadersToStandard = (num3 < 2);
			bundle.consolidateShaders = (num3 < 3 || (datDictionary2.ContainsKey("Enable_Shader_Consolidation") && !datDictionary2.ContainsKey("Disable_Shader_Consolidation")));
			Local localization = new Local(file.translationData, file.fallbackTranslationData);
			ushort id = datDictionary2.ParseUInt16("ID", 0);
			Asset asset;
			try
			{
				asset = (Activator.CreateInstance(type) as Asset);
			}
			catch (Exception e2)
			{
				Assets.reportError(string.Format("Caught exception while constructing {0} in \"{1}\": {2}", type, path, Assets.getExceptionMessage(e2)));
				UnturnedLog.exception(e2);
				bundle.unload();
				Assets.currentMasterBundle = null;
				return;
			}
			if (asset == null)
			{
				Assets.reportError(string.Format("Failed to construct {0} in \"{1}\"", type, path));
				bundle.unload();
				Assets.currentMasterBundle = null;
				return;
			}
			try
			{
				asset.id = id;
				asset.GUID = guid;
				asset.hash = array;
				asset.requiredShaderUpgrade = (bundle.convertShadersToStandard || bundle.consolidateShaders);
				asset.absoluteOriginFilePath = path;
				asset.origin = file.origin;
				asset.PopulateAsset(bundle, datDictionary2, localization);
				asset.origin.assets.Add(asset);
				Assets.AddToMapping(asset, file.origin.shouldAssetsOverrideExistingIds, Assets.defaultAssetMapping);
				bundle.unload();
			}
			catch (Exception e3)
			{
				Assets.reportError("Caught exception while populating \"" + path + "\": " + Assets.getExceptionMessage(e3));
				UnturnedLog.exception(e3);
				bundle.unload();
			}
			Assets.currentMasterBundle = null;
		}

		/// <summary>
		/// Called when a new workshop item is installed either on client or server. 
		/// </summary>
		// Token: 0x060013B4 RID: 5044 RVA: 0x00048448 File Offset: 0x00046648
		public static void RequestAddSearchLocation(string absoluteDirectoryPath, AssetOrigin origin)
		{
			Assets.instance.AddSearchLocation(absoluteDirectoryPath, origin);
		}

		/// <summary>
		/// Reload assets in given folder.
		/// </summary>
		// Token: 0x060013B5 RID: 5045 RVA: 0x00048456 File Offset: 0x00046656
		public static void reload(string absolutePath)
		{
			if (Assets.hasFinishedInitialStartupLoading && !Assets.isLoading)
			{
				Assets.loadingStats.Reset();
				Assets.RequestAddSearchLocation(absolutePath, Assets.reloadOrigin);
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0004847B File Offset: 0x0004667B
		public static void linkSpawnsIfDirty()
		{
			if (Assets.hasUnlinkedSpawns)
			{
				UnturnedLog.info("Linking spawns because changes were detected");
				Assets.linkSpawns();
				return;
			}
			UnturnedLog.info("Skipping link spawns because no changes were detected");
		}

		/// <summary>
		/// Can now be safely called multiple times on client in order to handle spawns for downloaded maps.
		/// Spawn tables have "roots" which allow mods to insert custom spawns into the vanilla spawn tables.
		/// This method is used after workshop assets are loaded on client, or after the dedicated server is done downloading workshop content.
		/// </summary>
		// Token: 0x060013B7 RID: 5047 RVA: 0x000484A0 File Offset: 0x000466A0
		public static void linkSpawns()
		{
			if (Assets.hasUnlinkedSpawns)
			{
				Assets.hasUnlinkedSpawns = false;
				List<SpawnAsset> list = new List<SpawnAsset>();
				Assets.FindAssetsByType_UseDefaultAssetMapping<SpawnAsset>(list);
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (SpawnAsset spawnAsset in list)
				{
					if (spawnAsset.insertRoots.Count >= 1)
					{
						foreach (SpawnTable spawnTable in spawnAsset.insertRoots)
						{
							SpawnAsset spawnAsset2;
							if (spawnTable.legacySpawnId != 0)
							{
								spawnAsset2 = (Assets.find(EAssetType.SPAWN, spawnTable.legacySpawnId) as SpawnAsset);
								if (spawnAsset2 == null)
								{
									Assets.reportError(spawnAsset, "unable to find root {0} during link", spawnTable.legacySpawnId);
									continue;
								}
							}
							else
							{
								if (GuidExtension.IsEmpty(spawnTable.targetGuid))
								{
									continue;
								}
								Asset asset = Assets.find(spawnTable.targetGuid);
								if (asset == null)
								{
									Assets.reportError(spawnAsset, "unable to find root {0} during link", spawnTable.targetGuid);
									continue;
								}
								spawnAsset2 = (asset as SpawnAsset);
								if (spawnAsset2 == null)
								{
									Assets.reportError(spawnAsset, string.Format("root {0} found as {1} {2} (not a spawn table)", spawnTable.targetGuid, asset.GetTypeFriendlyName(), asset.FriendlyName));
									continue;
								}
							}
							spawnTable.legacySpawnId = 0;
							spawnTable.targetGuid = spawnAsset.GUID;
							spawnTable.isLink = true;
							spawnAsset2.tables.Add(spawnTable);
							if (spawnTable.isOverride)
							{
								spawnAsset2.markOverridden();
							}
							spawnAsset2.markTablesDirty();
							num++;
							if (Assets.shouldLogSpawnInsertions)
							{
								if (spawnTable.isOverride)
								{
									UnturnedLog.info("Spawn {0} overriding {1}", new object[]
									{
										spawnAsset.name,
										spawnAsset2.name
									});
								}
								else
								{
									UnturnedLog.info("Spawn {0} inserted into {1}", new object[]
									{
										spawnAsset.name,
										spawnAsset2.name
									});
								}
							}
						}
						spawnAsset.insertRoots.Clear();
					}
				}
				foreach (SpawnAsset spawnAsset3 in list)
				{
					if (spawnAsset3.areTablesDirty)
					{
						spawnAsset3.sortAndNormalizeWeights();
						num2++;
					}
				}
				foreach (SpawnAsset spawnAsset4 in list)
				{
					foreach (SpawnTable spawnTable2 in spawnAsset4.tables)
					{
						if (!spawnTable2.hasNotifiedChild)
						{
							spawnTable2.hasNotifiedChild = true;
							SpawnAsset spawnAsset5;
							if (spawnTable2.legacySpawnId != 0)
							{
								spawnAsset5 = (Assets.find(EAssetType.SPAWN, spawnTable2.legacySpawnId) as SpawnAsset);
								if (spawnAsset5 == null)
								{
									Assets.reportError(spawnAsset4, "unable to find child table {0} during link", spawnTable2.legacySpawnId);
									continue;
								}
							}
							else
							{
								if (GuidExtension.IsEmpty(spawnTable2.targetGuid))
								{
									continue;
								}
								Asset asset2 = Assets.find(spawnTable2.targetGuid);
								if (asset2 == null)
								{
									Assets.reportError(spawnAsset4, "unable to find child {0} during link", spawnTable2.targetGuid);
									continue;
								}
								spawnAsset5 = (asset2 as SpawnAsset);
								if (spawnAsset5 == null)
								{
									continue;
								}
							}
							SpawnTable spawnTable3 = new SpawnTable();
							spawnTable3.targetGuid = spawnAsset4.GUID;
							spawnTable3.weight = spawnTable2.weight;
							spawnTable3.normalizedWeight = spawnTable2.normalizedWeight;
							spawnTable3.isLink = spawnTable2.isLink;
							spawnAsset5.roots.Add(spawnTable3);
							num3++;
						}
					}
				}
				UnturnedLog.info("Link spawns: {0} children, {1} sorted/normalized and {2} parents", new object[]
				{
					num,
					num2,
					num3
				});
				return;
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000488FC File Offset: 0x00046AFC
		public static void initializeMasterBundleValidation()
		{
			MasterBundleValidation.initialize(Assets.allMasterBundles);
		}

		/// <summary>
		/// Look through all item blueprints and log errors if there are duplicates.
		/// </summary>
		// Token: 0x060013B9 RID: 5049 RVA: 0x00048908 File Offset: 0x00046B08
		private void CheckForBlueprintErrors()
		{
			Func<Blueprint, Blueprint, bool> func = delegate(Blueprint myBlueprint, Blueprint yourBlueprint)
			{
				if (myBlueprint.type != yourBlueprint.type)
				{
					return false;
				}
				if (myBlueprint.outputs.Length != yourBlueprint.outputs.Length)
				{
					return false;
				}
				if (myBlueprint.supplies.Length != yourBlueprint.supplies.Length)
				{
					return false;
				}
				if (myBlueprint.questConditions.Length != yourBlueprint.questConditions.Length)
				{
					return false;
				}
				if (myBlueprint.questRewards != null != (yourBlueprint.questRewards != null))
				{
					return false;
				}
				if (myBlueprint.questRewards != null && myBlueprint.questRewards.Length != yourBlueprint.questRewards.Length)
				{
					return false;
				}
				if (myBlueprint.tool != yourBlueprint.tool)
				{
					return false;
				}
				byte b5 = 0;
				while ((int)b5 < myBlueprint.outputs.Length)
				{
					if (myBlueprint.outputs[(int)b5].id != yourBlueprint.outputs[(int)b5].id)
					{
						return false;
					}
					b5 += 1;
				}
				byte b6 = 0;
				while ((int)b6 < myBlueprint.supplies.Length)
				{
					if (myBlueprint.supplies[(int)b6].id != yourBlueprint.supplies[(int)b6].id)
					{
						return false;
					}
					b6 += 1;
				}
				for (int k = 0; k < myBlueprint.questConditions.Length; k++)
				{
					if (!myBlueprint.questConditions[k].Equals(yourBlueprint.questConditions[k]))
					{
						return false;
					}
				}
				if (myBlueprint.questRewards != null)
				{
					for (int l = 0; l < myBlueprint.questRewards.Length; l++)
					{
						if (!myBlueprint.questRewards[l].Equals(yourBlueprint.questRewards[l]))
						{
							return false;
						}
					}
				}
				return true;
			};
			List<ItemAsset> list = new List<ItemAsset>();
			Assets.find<ItemAsset>(list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					ItemAsset itemAsset = list[i];
					byte b = 0;
					while ((int)b < itemAsset.blueprints.Count)
					{
						Blueprint blueprint = itemAsset.blueprints[(int)b];
						byte b2 = 0;
						while ((int)b2 < itemAsset.blueprints.Count)
						{
							if (b2 != b)
							{
								Blueprint blueprint2 = itemAsset.blueprints[(int)b2];
								if (func.Invoke(blueprint, blueprint2))
								{
									Asset offendingAsset = itemAsset;
									string text = "has an identical blueprint: ";
									Blueprint blueprint3 = blueprint;
									Assets.reportError(offendingAsset, text + ((blueprint3 != null) ? blueprint3.ToString() : null));
								}
							}
							b2 += 1;
						}
						b += 1;
					}
					for (int j = 0; j < list.Count; j++)
					{
						if (j != i)
						{
							ItemAsset itemAsset2 = list[j];
							byte b3 = 0;
							while ((int)b3 < itemAsset.blueprints.Count)
							{
								Blueprint blueprint4 = itemAsset.blueprints[(int)b3];
								byte b4 = 0;
								while ((int)b4 < itemAsset2.blueprints.Count)
								{
									Blueprint blueprint5 = itemAsset2.blueprints[(int)b4];
									if (func.Invoke(blueprint4, blueprint5))
									{
										Asset offendingAsset2 = itemAsset;
										string text2 = "shares an identical blueprint with ";
										string itemName = itemAsset2.itemName;
										string text3 = ": ";
										Blueprint blueprint6 = blueprint4;
										Assets.reportError(offendingAsset2, text2 + itemName + text3 + ((blueprint6 != null) ? blueprint6.ToString() : null));
									}
									b4 += 1;
								}
								b3 += 1;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Look through all dialogue and check that their referenced
		/// dialogueID or vendorID is an actual loaded asset.
		/// </summary>
		// Token: 0x060013BA RID: 5050 RVA: 0x00048AAC File Offset: 0x00046CAC
		private void CheckForNpcErrors()
		{
			List<DialogueAsset> list = new List<DialogueAsset>();
			Assets.find<DialogueAsset>(list);
			foreach (DialogueAsset dialogueAsset in list)
			{
				int num = dialogueAsset.responses.Length;
				for (int i = 0; i < num; i++)
				{
					DialogueResponse dialogueResponse = dialogueAsset.responses[i];
					if (!dialogueResponse.IsDialogueRefNull() && dialogueResponse.FindDialogueAsset() == null)
					{
						Assets.reportError(dialogueAsset, "unable to find dialogue asset for response " + i.ToString());
					}
					if (!dialogueResponse.IsVendorRefNull() && dialogueResponse.FindVendorAsset() == null)
					{
						Assets.reportError(dialogueAsset, "unable to find vendor asset for response " + i.ToString());
					}
				}
			}
		}

		/// <summary>
		/// Manually run asset unload and garbage collection in the hope
		/// that it will minimize RAM allocated during loading.
		/// </summary>
		// Token: 0x060013BB RID: 5051 RVA: 0x00048B74 File Offset: 0x00046D74
		private void CleanupMemory()
		{
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		/// <summary>
		/// Helper for Assets.init.
		/// Load all non-map assets from:
		/// 	/Bundles/Workshop/Content
		/// 	/Servers/ServerID/Workshop/Content
		/// 	/Servers/ServerID/Bundles
		/// </summary>
		// Token: 0x060013BC RID: 5052 RVA: 0x00048B84 File Offset: 0x00046D84
		private void AddDedicatedServerUgcSearchLocations()
		{
			string path = Path.Combine(ReadWrite.PATH, "Bundles", "Workshop", "Content");
			if (ReadWrite.folderExists(path, false))
			{
				this.AddSearchLocation(path, Assets.legacyServerSharedOrigin);
			}
			string path2 = Path.Combine(new string[]
			{
				ReadWrite.PATH,
				ServerSavedata.directoryName,
				Provider.serverID,
				"Workshop",
				"Content"
			});
			if (ReadWrite.folderExists(path2, false))
			{
				this.AddSearchLocation(path2, Assets.legacyPerServerOrigin);
			}
			string path3 = Path.Combine(ReadWrite.PATH, ServerSavedata.directoryName, Provider.serverID, "Bundles");
			if (ReadWrite.folderExists(path3, false))
			{
				this.AddSearchLocation(path3, Assets.legacyPerServerOrigin);
			}
		}

		/// <summary>
		/// Helper for Assets.init.
		/// Load all non-map assets from subscribed UGC.
		/// </summary>
		// Token: 0x060013BD RID: 5053 RVA: 0x00048C38 File Offset: 0x00046E38
		private void AddClientUgcSearchLocations()
		{
			if (Provider.provider.workshopService.ugc != null)
			{
				SteamContent[] array = Provider.provider.workshopService.ugc.ToArray();
				Assets.hasLoadedUgc = true;
				foreach (SteamContent steamContent in array)
				{
					if (LocalWorkshopSettings.get().getEnabled(steamContent.publishedFileID) && (steamContent.type == ESteamUGCType.OBJECT || steamContent.type == ESteamUGCType.ITEM || steamContent.type == ESteamUGCType.VEHICLE))
					{
						AssetOrigin origin = Assets.FindOrAddWorkshopFileOrigin(steamContent.publishedFileID.m_PublishedFileId, false);
						this.AddSearchLocation(steamContent.path, origin);
					}
				}
			}
		}

		/// <summary>
		/// Helper for modders creating workshop content.
		/// Loads folders in the "Sandbox" directory the same way workshop files are loaded.
		/// </summary>
		// Token: 0x060013BE RID: 5054 RVA: 0x00048CD0 File Offset: 0x00046ED0
		private void AddSandboxSearchLocations()
		{
			string text = Path.Combine(ReadWrite.PATH, "Sandbox");
			if (Directory.Exists(text))
			{
				foreach (string text2 in ReadWrite.getFolders(text, false))
				{
					UnturnedLog.info("Sandbox: {0}", new object[]
					{
						text2
					});
					AssetOrigin assetOrigin = new AssetOrigin();
					assetOrigin.name = "Sandbox Folder \"" + text2 + "\"";
					assetOrigin.shouldAssetsOverrideExistingIds = true;
					Assets.assetOrigins.Add(assetOrigin);
					this.AddSearchLocation(text2, assetOrigin);
				}
				return;
			}
			Directory.CreateDirectory(text);
		}

		/// <summary>
		/// Helper for Assets.init.
		/// Load all assets in each map's Bundles folder, and content in map's Content folder.
		/// </summary>
		// Token: 0x060013BF RID: 5055 RVA: 0x00048D68 File Offset: 0x00046F68
		private void AddMapSearchLocations()
		{
			LevelInfo[] levels = Level.getLevels(ESingleplayerMapCategory.ALL);
			Assets.hasLoadedMaps = true;
			foreach (LevelInfo levelInfo in levels)
			{
				if (levelInfo != null)
				{
					string path = Path.Combine(levelInfo.path, "Bundles");
					if (ReadWrite.folderExists(path, false))
					{
						AssetOrigin origin = Assets.FindOrAddLevelOrigin(levelInfo);
						this.AddSearchLocation(path, origin);
					}
				}
			}
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00048DC2 File Offset: 0x00046FC2
		private void AddSearchLocation(string path, AssetOrigin origin)
		{
			path = Path.GetFullPath(path);
			UnturnedLog.info(origin.name + " added asset search location \"" + path + "\"");
			this.worker.RequestSearch(path, origin);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00048DF4 File Offset: 0x00046FF4
		private MasterBundleConfig FindAndRemoveLoadedPendingMasterBundle()
		{
			for (int i = Assets.pendingMasterBundles.Count - 1; i >= 0; i--)
			{
				MasterBundleConfig masterBundleConfig = Assets.pendingMasterBundles[i];
				if (masterBundleConfig.assetBundleCreateRequest.isDone)
				{
					Assets.pendingMasterBundles.RemoveAtFast(i);
					return masterBundleConfig;
				}
			}
			return null;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00048E3F File Offset: 0x0004703F
		private IEnumerator LoadAssetsFromWorkerThread()
		{
			double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
			int gcFrameCount = 0;
			while (this.worker.IsWorking || Assets.pendingMasterBundles.Count > 0)
			{
				AssetsWorker.MasterBundle masterBundle;
				if (this.worker.TryDequeueMasterBundle(out masterBundle))
				{
					MasterBundleConfig config = masterBundle.config;
					Assets.pendingMasterBundles.Add(config);
					config.StartLoad(masterBundle.assetBundleData, masterBundle.hash);
					Assets.loadingStats.isLoadingAssetBundles = true;
				}
				else
				{
					AssetsWorker.AssetDefinition file;
					if (Assets.pendingMasterBundles.Count > 0)
					{
						MasterBundleConfig masterBundleConfig = this.FindAndRemoveLoadedPendingMasterBundle();
						if (masterBundleConfig != null)
						{
							masterBundleConfig.FinishLoad();
							Assets.loadingStats.totalMasterBundlesLoaded++;
							if (masterBundleConfig.assetBundle != null)
							{
								if (masterBundleConfig.origin == Assets.coreOrigin)
								{
									Assets.coreMasterBundle = masterBundleConfig;
								}
								Assets.allMasterBundles.Add(masterBundleConfig);
							}
							else
							{
								MasterBundleConfig masterBundleConfig2 = Assets.findMasterBundleByName(masterBundleConfig.assetBundleName, true);
								if (masterBundleConfig2 != null)
								{
									masterBundleConfig.CopyAssetBundleFromDuplicateConfig(masterBundleConfig2);
									if (masterBundleConfig.assetBundle != null)
									{
										UnturnedLog.info(string.Concat(new string[]
										{
											"Using \"",
											masterBundleConfig2.assetBundleName,
											"\" in \"",
											masterBundleConfig2.directoryPath,
											"\" as fallback asset bundle for \"",
											masterBundleConfig.directoryPath,
											"\""
										}));
										Assets.allMasterBundles.Add(masterBundleConfig);
									}
									else
									{
										UnturnedLog.info(string.Concat(new string[]
										{
											"Unable to use \"",
											masterBundleConfig2.assetBundleName,
											"\" in \"",
											masterBundleConfig2.directoryPath,
											"\" as fallback asset bundle for \"",
											masterBundleConfig.directoryPath,
											"\""
										}));
									}
								}
								else
								{
									UnturnedLog.info("Unable to find a fallback asset bundle for \"" + masterBundleConfig.assetBundleName + "\"");
								}
							}
						}
						if (Assets.pendingMasterBundles.Count < 1)
						{
							Assets.loadingStats.isLoadingAssetBundles = false;
						}
					}
					else if (Assets.coreMasterBundle != null && this.worker.TryDequeueAssetDefinition(out file))
					{
						Assets.TryLoadFile(file);
					}
					if (Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble > 0.05)
					{
						Assets.SyncAssetDefinitionLoadingProgress();
						int num = gcFrameCount + 1;
						gcFrameCount = num;
						if (gcFrameCount % 25 == 0 && Assets.shouldCollectGarbageAggressively)
						{
							this.CleanupMemory();
						}
						yield return null;
						realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
					}
				}
			}
			yield break;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00048E50 File Offset: 0x00047050
		internal static void SyncAssetDefinitionLoadingProgress()
		{
			Assets.loadingStats.totalRegisteredSearchLocations = Assets.instance.worker.totalSearchLocationRequests;
			Assets.loadingStats.totalSearchLocationsFinishedSearching = Assets.instance.worker.totalSearchLocationsFinishedSearching;
			Assets.loadingStats.totalMasterBundlesFound = Assets.instance.worker.totalMasterBundlesFound;
			Assets.loadingStats.totalFilesFound = Assets.instance.worker.totalAssetDefinitionsFound;
			Assets.loadingStats.totalFilesRead = Assets.instance.worker.totalAssetDefinitionsRead;
			LoadingUI.NotifyAssetDefinitionLoadingProgress();
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00048EDF File Offset: 0x000470DF
		private IEnumerator LoadAllAssets()
		{
			Assets.isLoadingAllAssets = true;
			double startTime = Time.realtimeSinceStartupAsDouble;
			if (Assets.errors == null)
			{
				Assets.errors = new List<string>();
			}
			else
			{
				Assets.errors.Clear();
			}
			Assets.defaultAssetMapping = new Assets.AssetMapping();
			Assets.currentAssetMapping = Assets.defaultAssetMapping;
			Assets.coreMasterBundle = null;
			if (Assets.allMasterBundles == null)
			{
				Assets.allMasterBundles = new List<MasterBundleConfig>();
				Assets.pendingMasterBundles = new List<MasterBundleConfig>();
			}
			else
			{
				Assets.UnloadAllMasterBundles();
			}
			Assets.assetOrigins = new List<AssetOrigin>();
			Assets.loadingStats.Reset();
			Assets.coreOrigin = new AssetOrigin();
			Assets.coreOrigin.name = "Vanilla Built-in Assets";
			Assets.assetOrigins.Add(Assets.coreOrigin);
			Assets.reloadOrigin = new AssetOrigin();
			Assets.reloadOrigin.name = "Reloaded Assets (Debug)";
			Assets.reloadOrigin.shouldAssetsOverrideExistingIds = true;
			Assets.assetOrigins.Add(Assets.reloadOrigin);
			Assets.legacyServerSharedOrigin = new AssetOrigin();
			Assets.legacyServerSharedOrigin.name = "Server Common (Legacy)";
			Assets.assetOrigins.Add(Assets.legacyServerSharedOrigin);
			Assets.legacyPerServerOrigin = new AssetOrigin();
			Assets.legacyPerServerOrigin.name = "Per-Server (Legacy)";
			Assets.assetOrigins.Add(Assets.legacyPerServerOrigin);
			yield return null;
			if (Assets.shouldLoadAnyAssets)
			{
				this.AddSearchLocation(Path.Combine(ReadWrite.PATH, "Bundles"), Assets.coreOrigin);
				this.AddDedicatedServerUgcSearchLocations();
				this.AddSandboxSearchLocations();
				this.AddMapSearchLocations();
				yield return null;
				yield return this.LoadAssetsFromWorkerThread();
			}
			LoadingUI.SetLoadingText("Loading_Blueprints");
			yield return null;
			if (Assets.shouldValidateAssets)
			{
				this.CheckForBlueprintErrors();
			}
			LoadingUI.SetLoadingText("Loading_Spawns");
			yield return null;
			if (Assets.shouldValidateAssets)
			{
				this.CheckForNpcErrors();
			}
			this.CleanupMemory();
			LoadingUI.SetLoadingText("Loading_Misc");
			yield return null;
			AssetsRefreshed assetsRefreshed = Assets.onAssetsRefreshed;
			if (assetsRefreshed != null)
			{
				assetsRefreshed();
			}
			yield return null;
			UnturnedLog.info(string.Format("Loading all assets took {0}s", Time.realtimeSinceStartupAsDouble - startTime));
			Assets.isLoadingAllAssets = false;
			yield break;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00048EEE File Offset: 0x000470EE
		private IEnumerator StartupAssetLoading()
		{
			yield return this.LoadAllAssets();
			Assets.hasFinishedInitialStartupLoading = true;
			Provider.host();
			IL_6D:
			yield break;
			UnturnedLog.info("Launching main menu");
			SceneManager.LoadScene("Menu");
			goto IL_6D;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00048EFD File Offset: 0x000470FD
		private IEnumerator LoadNewAssetsFromUpdate()
		{
			Assets.isLoadingFromUpdate = true;
			double startTime = Time.realtimeSinceStartupAsDouble;
			yield return this.LoadAssetsFromWorkerThread();
			Assets.linkSpawnsIfDirty();
			this.CleanupMemory();
			UnturnedLog.info(string.Format("Loading new assets took {0}s", Time.realtimeSinceStartupAsDouble - startTime));
			Assets.isLoadingFromUpdate = false;
			Action onNewAssetsFinishedLoading = Assets.OnNewAssetsFinishedLoading;
			if (onNewAssetsFinishedLoading != null)
			{
				onNewAssetsFinishedLoading.Invoke();
			}
			yield break;
		}

		/// <summary>
		/// Not the tidiest place for this, but it allows startup to pause and show error message.
		/// Occasionally there have been reports of the steamclient redist files being out of date on the dedicated
		/// server causing problems. For example: https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/2866#issuecomment-965945985
		/// </summary>
		// Token: 0x060013C7 RID: 5063 RVA: 0x00048F0C File Offset: 0x0004710C
		private bool TestDedicatedServerSteamRedist()
		{
			string text = PathEx.Join(UnityPaths.GameDirectory, "steamclient64.dll");
			if (!File.Exists(text))
			{
				CommandWindow.LogError("Missing steamclient redist file at: " + text);
				return false;
			}
			bool result;
			try
			{
				FileInfo fileInfo = new FileInfo(text);
				DateTime dateTime;
				dateTime..ctor(2021, 9, 14, 21, 30, 0, 1);
				if (fileInfo.LastWriteTimeUtc >= dateTime)
				{
					result = true;
				}
				else
				{
					CommandWindow.LogError(string.Format("Out-of-date steamclient redist file (expected: {0} actual: {1})", dateTime, fileInfo.LastWriteTimeUtc));
					result = false;
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Unable to get steamclient redist file info");
				result = false;
			}
			return result;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00048FB8 File Offset: 0x000471B8
		private void Start()
		{
			Module moduleByName = ModuleHook.getModuleByName("Rocket.Unturned");
			if (moduleByName != null)
			{
				uint uint32FromIP = Parser.getUInt32FromIP("4.9.3.1");
				if (moduleByName.config.Version_Internal < uint32FromIP)
				{
					CommandWindow.LogError("Upgrading to the officially maintained version of Rocket, or a custom fork of it, is required.");
					CommandWindow.LogErrorFormat("Installed version: {0} Maintained version: 4.9.3.3+", new object[]
					{
						moduleByName.config.Version
					});
					CommandWindow.Log(string.Empty);
					CommandWindow.Log("--- Overview ---");
					CommandWindow.Log(string.Empty);
					CommandWindow.Log("SDG maintains a fork of Rocket called the Legally Distinct Missile (or LDM) after the resignation of its original community team. Using this fork is important because it preserves compatibility, and has fixes for important legacy Rocket issues like multithreading exceptions and teleportation exploits.");
					CommandWindow.Log(string.Empty);
					CommandWindow.Log("--- Installation ---");
					CommandWindow.Log(string.Empty);
					CommandWindow.Log("The dedicated server includes the latest version, so an external download is not necessary:");
					CommandWindow.Log("1. Copy the Rocket.Unturned module from the game's Extras directory.");
					CommandWindow.Log("2. Paste it into the game's Modules directory.");
					CommandWindow.Log(string.Empty);
					CommandWindow.Log("--- Resources ---");
					CommandWindow.Log(string.Empty);
					CommandWindow.Log("https://github.com/SmartlyDressedGames/Legally-Distinct-Missile");
					CommandWindow.Log("https://www.reddit.com/r/UnturnedLDM/");
					CommandWindow.Log("https://forum.smartlydressedgames.com/c/modding/ldm");
					CommandWindow.Log("https://steamcommunity.com/app/304930/discussions/17/");
					return;
				}
			}
			if (!this.TestDedicatedServerSteamRedist())
			{
				return;
			}
			this.worker = new AssetsWorker();
			this.worker.Initialize();
			base.StartCoroutine(this.StartupAssetLoading());
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x000490F1 File Offset: 0x000472F1
		private void Awake()
		{
			Assets.instance = this;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000490F9 File Offset: 0x000472F9
		private void Update()
		{
			this.worker.Update();
			if (!Assets.isLoading && this.worker.IsWorking)
			{
				base.StartCoroutine(this.LoadNewAssetsFromUpdate());
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00049127 File Offset: 0x00047327
		private void OnDestroy()
		{
			this.worker.Shutdown();
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00049134 File Offset: 0x00047334
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveKickForInvalidGuid(Guid guid)
		{
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.CUSTOM;
			Asset asset = Assets.find(guid);
			if (asset != null)
			{
				Provider._connectionFailureReason = string.Format("Server missing asset: \"{0}\" File: \"{1}\" Id: {2:N}", asset.FriendlyName, asset.name, guid) + "File path: \"" + asset.absoluteOriginFilePath + "\"" + "Client asset is from " + asset.GetOriginName() + ".";
			}
			else
			{
				Provider._connectionFailureReason = "Client and server are both missing unknown asset! ID: " + guid.ToString("N") + "\nThis probably means either an invalid ID was sent by the server," + "\nthe ID got corrupted for example by plugins modifying network traffic," + "\nor a required level asset like materials/foliage/trees/objects is missing.";
			}
			Provider.RequestDisconnect(string.Format("Kicked for sending invalid asset guid: {0:N}", guid));
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000491F0 File Offset: 0x000473F0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveKickForHashMismatch(Guid guid, string serverName, string serverFriendlyName, byte[] serverHash, string serverAssetBundleNameWithoutExtension, string serverAssetOrigin)
		{
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.CUSTOM;
			Asset asset = Assets.find(guid);
			if (asset != null)
			{
				AssetOrigin origin = asset.origin;
				string text = (origin != null) ? origin.name : null;
				if (string.IsNullOrEmpty(text))
				{
					text = "Unknown";
				}
				string text2;
				if (string.Equals(asset.name, serverName) && string.Equals(asset.FriendlyName, serverFriendlyName))
				{
					if (!string.IsNullOrEmpty(serverAssetBundleNameWithoutExtension) && asset.originMasterBundle != null && !string.Equals(asset.originMasterBundle.assetBundleNameWithoutExtension, serverAssetBundleNameWithoutExtension))
					{
						text2 = string.Format("Client and server loaded \"{0}\" from different asset bundles! (File: \"{1}\" ID: {2:N})", serverFriendlyName, asset.name, guid);
						text2 = string.Concat(new string[]
						{
							text2,
							"\nClient asset bundle is \"",
							asset.originMasterBundle.assetBundleNameWithoutExtension,
							"\", whereas server asset bundle is \"",
							serverAssetBundleNameWithoutExtension,
							"\"."
						});
					}
					else if (!string.IsNullOrEmpty(serverAssetBundleNameWithoutExtension) && asset.originMasterBundle == null)
					{
						text2 = string.Format("Client loaded \"{0}\" from legacy asset bundle but server did not! (File: \"{1}\" ID: {2:N})", serverFriendlyName, asset.name, guid);
						text2 = text2 + "\nServer asset bundle name: \"" + serverAssetBundleNameWithoutExtension + "\".";
					}
					else if (string.IsNullOrEmpty(serverAssetBundleNameWithoutExtension) && asset.originMasterBundle != null)
					{
						text2 = string.Format("Server loaded \"{0}\" from legacy asset bundle but client did not! (File: \"{1}\" ID: {2:N})", serverFriendlyName, asset.name, guid);
						text2 = text2 + "\nClient asset bundle name: \"" + asset.originMasterBundle.assetBundleNameWithoutExtension + "\"";
					}
					else if (Hash.verifyHash(asset.hash, serverHash))
					{
						text2 = string.Format("Server asset bundle hash out of date for \"{0}\"! (File: \"{1}\" ID: {2:N})", serverFriendlyName, asset.name, guid);
						text2 = text2 + "\nThis probably means the mod creator should re-export the \"" + serverAssetBundleNameWithoutExtension + "\" asset bundle.";
					}
					else
					{
						text2 = string.Format("Client and server disagree on asset \"{0}\" configuration. (File: \"{1}\" ID: {2:N})", asset.FriendlyName, asset.name, guid);
						text2 += "\nUsually this means the files are different versions in which case updating the client and server might fix it.";
						text2 += "\nAlternatively the file may have been corrupted, locally modified, or modified on the server.";
						text2 = string.Concat(new string[]
						{
							text2,
							"\nClient hash is ",
							Hash.toString(asset.hash),
							", whereas server hash is ",
							Hash.toString(serverHash),
							"."
						});
					}
				}
				else
				{
					text2 = string.Format("Client and server have different assets with the same ID! ({0:N})", guid);
					text2 += "\nThis probably means an existing file was copied, but the mod creator can fix it by changing the ID.";
					if (string.Equals(asset.FriendlyName, serverFriendlyName))
					{
						text2 = text2 + "\nDisplay name \"" + serverFriendlyName + "\" matches between client and server.";
					}
					else
					{
						text2 = string.Concat(new string[]
						{
							text2,
							"\nClient display name is \"",
							asset.FriendlyName,
							"\", whereas server display name is \"",
							serverFriendlyName,
							"\"."
						});
					}
					if (string.Equals(asset.name, serverName))
					{
						text2 = text2 + "\nFile name \"" + asset.name + "\" matches between client and server.";
					}
					else
					{
						text2 = string.Concat(new string[]
						{
							text2,
							"\nClient file name is \"",
							asset.name,
							"\", whereas server file name is \"",
							serverName,
							"\"."
						});
					}
				}
				if (string.Equals(text, serverAssetOrigin))
				{
					text2 = text2 + "\nClient and server agree this asset is from " + text + ".";
				}
				else
				{
					text2 = string.Concat(new string[]
					{
						text2,
						"\nClient asset is from ",
						text,
						", whereas server asset is from ",
						serverAssetOrigin,
						"."
					});
				}
				Provider._connectionFailureReason = text2;
			}
			else
			{
				Provider._connectionFailureReason = string.Format("Unknown asset hash mismatch? (should never happen) Name: \"{0}\" File: \"{1}\" Id: {2:N}", serverFriendlyName, serverName, guid);
			}
			Provider.RequestDisconnect(string.Format("Kicked for asset hash mismatch guid: {0:N} serverName: \"{1}\" serverFriendlyName: \"{2}\" serverHash: {3} serverAssetBundleName: \"{4}\" serverAssetOrigin: \"{5}\"", new object[]
			{
				guid,
				serverName,
				serverFriendlyName,
				Hash.toString(serverHash),
				serverAssetBundleNameWithoutExtension,
				serverAssetOrigin
			}));
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004958A File Offset: 0x0004778A
		[Obsolete("Renamed to RequestAddSearchLocation")]
		public static void load(string absoluteDirectoryPath, AssetOrigin origin, bool overrideExistingIDs)
		{
			Assets.RequestAddSearchLocation(absoluteDirectoryPath, origin);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00049593 File Offset: 0x00047793
		[Obsolete("Renamed to RequestReloadAllAssets")]
		public static void refresh()
		{
			Assets.RequestReloadAllAssets();
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004959A File Offset: 0x0004779A
		[Obsolete]
		public static void rename(Asset asset, string newName)
		{
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004959C File Offset: 0x0004779C
		[Obsolete]
		public static AssetOrigin ConvertLegacyOrigin(EAssetOrigin legacyOrigin)
		{
			if (legacyOrigin == EAssetOrigin.OFFICIAL)
			{
				if (Assets.legacyOfficialOrigin == null)
				{
					Assets.legacyOfficialOrigin = new AssetOrigin();
					Assets.legacyOfficialOrigin.name = "Official (Legacy)";
					Assets.assetOrigins.Add(Assets.legacyOfficialOrigin);
				}
				return Assets.legacyOfficialOrigin;
			}
			if (legacyOrigin == EAssetOrigin.MISC)
			{
				if (Assets.legacyMiscOrigin == null)
				{
					Assets.legacyMiscOrigin = new AssetOrigin();
					Assets.legacyMiscOrigin.name = "Misc (Legacy)";
					Assets.assetOrigins.Add(Assets.legacyMiscOrigin);
				}
				return Assets.legacyMiscOrigin;
			}
			if (Assets.legacyWorkshopOrigin == null)
			{
				Assets.legacyWorkshopOrigin = new AssetOrigin();
				Assets.legacyWorkshopOrigin.name = "Workshop File (Legacy)";
				Assets.assetOrigins.Add(Assets.legacyWorkshopOrigin);
			}
			return Assets.legacyWorkshopOrigin;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004964E File Offset: 0x0004784E
		[Obsolete]
		public static Asset find(EAssetType type, string name)
		{
			return null;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00049651 File Offset: 0x00047851
		[Obsolete]
		public static void add(Asset asset, bool overrideExistingID)
		{
			Assets.AddToMapping(asset, overrideExistingID, Assets.defaultAssetMapping);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004965F File Offset: 0x0004785F
		[Obsolete]
		public static void load(string path, bool usePath, bool loadFromResources, bool canUse, EAssetOrigin origin, bool overrideExistingIDs)
		{
			Assets.load(path, usePath, loadFromResources, canUse, origin, overrideExistingIDs, 0UL);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00049670 File Offset: 0x00047870
		[Obsolete("Remove unused loadFromResources which was used by vanilla assets before masterbundles, and canUse which was for timed curated maps.")]
		public static void load(string path, bool usePath, bool loadFromResources, bool canUse, EAssetOrigin origin, bool overrideExistingIDs, ulong workshopFileId)
		{
			Assets.load(path, usePath, origin, overrideExistingIDs, workshopFileId);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00049680 File Offset: 0x00047880
		[Obsolete("Replaced origin enum with class")]
		public static void load(string path, bool usePath, EAssetOrigin legacyOrigin, bool overrideExistingIDs, ulong workshopFileId)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			AssetOrigin origin = Assets.ConvertLegacyOrigin(legacyOrigin);
			Assets.load(path, origin, overrideExistingIDs);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x000496AC File Offset: 0x000478AC
		[Obsolete("Please use the method which takes a List instead.")]
		public static Asset[] find(EAssetType type)
		{
			if (type == EAssetType.NONE)
			{
				return null;
			}
			if (type == EAssetType.OBJECT)
			{
				throw new NotSupportedException();
			}
			Asset[] array = new Asset[Assets.currentAssetMapping.legacyAssetsTable[type].Values.Count];
			int num = 0;
			foreach (KeyValuePair<ushort, Asset> keyValuePair in Assets.currentAssetMapping.legacyAssetsTable[type])
			{
				array[num] = keyValuePair.Value;
				num++;
			}
			return array;
		}

		// Token: 0x040006A4 RID: 1700
		private static TypeRegistryDictionary _assetTypes = new TypeRegistryDictionary(typeof(Asset));

		// Token: 0x040006A5 RID: 1701
		private static TypeRegistryDictionary _useableTypes = new TypeRegistryDictionary(typeof(Useable));

		// Token: 0x040006A6 RID: 1702
		private static Assets instance;

		/// <summary>
		/// The first time asset loading finishes it will load the main menu.
		/// </summary>
		// Token: 0x040006A7 RID: 1703
		private static bool hasFinishedInitialStartupLoading;

		/// <summary>
		/// If true, either loading during initial startup or full refresh.
		/// </summary>
		// Token: 0x040006A8 RID: 1704
		private static bool isLoadingAllAssets;

		/// <summary>
		/// If true, currently searching locations added after initial startup loading.
		/// </summary>
		// Token: 0x040006A9 RID: 1705
		private static bool isLoadingFromUpdate;

		// Token: 0x040006AC RID: 1708
		public static AssetsRefreshed onAssetsRefreshed;

		// Token: 0x040006AD RID: 1709
		internal static Action OnNewAssetsFinishedLoading;

		// Token: 0x040006AE RID: 1710
		internal static Assets.AssetMapping defaultAssetMapping;

		/// <summary>
		/// In singleplayer and the level editor this is the same as defaultAssetMapping,
		/// but when playing on a server a subset of assets based on the server's workshop files is used.
		/// </summary>
		// Token: 0x040006AF RID: 1711
		private static Assets.AssetMapping currentAssetMapping;

		/// <summary>
		/// Should folders be scanned for and load .dat and asset bundle files?
		/// Plugin developers find it useful to quickly launch the server.
		/// </summary>
		// Token: 0x040006B0 RID: 1712
		public static CommandLineFlag shouldLoadAnyAssets = new CommandLineFlag(true, "-SkipAssets");

		/// <summary>
		/// Do we want to enable shouldDeferLoadingAssets?
		/// </summary>
		// Token: 0x040006B1 RID: 1713
		public static CommandLineFlag wantsDeferLoadingAssets = new CommandLineFlag(true, "-NoDeferAssets");

		/// <summary>
		/// Should extra validation be performed on assets as they load?
		/// Useful for developing, but it does slow down loading.
		/// </summary>
		// Token: 0x040006B2 RID: 1714
		public static CommandLineFlag shouldValidateAssets = new CommandLineFlag(false, "-ValidateAssets");

		/// <summary>
		/// Should workshop asset names and IDs be logged while loading?
		/// Useful when debugging unknown workshop content.
		/// </summary>
		// Token: 0x040006B3 RID: 1715
		public static CommandLineFlag shouldLogWorkshopAssets = new CommandLineFlag(false, "-LogWorkshopAssets");

		/// <summary>
		/// Should GC and clear unused assets be called after every loading frame?
		/// Potentially useful for players running out of RAM, refer to:
		/// https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/1352#issuecomment-751138105
		/// </summary>
		// Token: 0x040006B4 RID: 1716
		private static CommandLineFlag shouldCollectGarbageAggressively = new CommandLineFlag(false, "-AggressiveGC");

		/// <summary>
		/// Should modded spawn tables being inserted into parents be logged?
		/// Useful for debugging workshop spawn table problems.
		/// </summary>
		// Token: 0x040006B5 RID: 1717
		private static CommandLineFlag shouldLogSpawnInsertions = new CommandLineFlag(false, "-LogSpawnInsertions");

		/// <summary>
		/// Loaded master bundles.
		/// </summary>
		// Token: 0x040006B6 RID: 1718
		private static List<MasterBundleConfig> allMasterBundles;

		/// <summary>
		/// Loading master bundles.
		/// </summary>
		// Token: 0x040006B7 RID: 1719
		private static List<MasterBundleConfig> pendingMasterBundles;

		/// <summary>
		/// Master bundle from root /Bundles directory containing vanilla assets.
		/// </summary>
		// Token: 0x040006B8 RID: 1720
		private static MasterBundleConfig coreMasterBundle;

		// Token: 0x040006BA RID: 1722
		internal static List<AssetOrigin> assetOrigins;

		// Token: 0x040006BB RID: 1723
		internal static AssetOrigin coreOrigin;

		// Token: 0x040006BC RID: 1724
		internal static AssetOrigin reloadOrigin;

		// Token: 0x040006BD RID: 1725
		private static AssetOrigin legacyServerSharedOrigin;

		// Token: 0x040006BE RID: 1726
		private static AssetOrigin legacyPerServerOrigin;

		// Token: 0x040006BF RID: 1727
		private static List<string> errors;

		/// <summary>
		/// Do we have any new spawn assets that have not been linked yet?
		/// Used to skip linking spawns if not required when downloading assets.
		/// </summary>
		// Token: 0x040006C0 RID: 1728
		private static bool hasUnlinkedSpawns;

		// Token: 0x040006C1 RID: 1729
		internal static readonly ClientStaticMethod<Guid> SendKickForInvalidGuid = ClientStaticMethod<Guid>.Get(new ClientStaticMethod<Guid>.ReceiveDelegate(Assets.ReceiveKickForInvalidGuid));

		// Token: 0x040006C2 RID: 1730
		internal static readonly ClientStaticMethod<Guid, string, string, byte[], string, string> SendKickForHashMismatch = ClientStaticMethod<Guid, string, string, byte[], string, string>.Get(new ClientStaticMethod<Guid, string, string, byte[], string, string>.ReceiveDelegate(Assets.ReceiveKickForHashMismatch));

		// Token: 0x040006C3 RID: 1731
		internal static AssetLoadingStats loadingStats = new AssetLoadingStats();

		// Token: 0x040006C4 RID: 1732
		private AssetsWorker worker;

		// Token: 0x040006C5 RID: 1733
		internal static AssetOrigin legacyOfficialOrigin;

		// Token: 0x040006C6 RID: 1734
		internal static AssetOrigin legacyMiscOrigin;

		// Token: 0x040006C7 RID: 1735
		internal static AssetOrigin legacyWorkshopOrigin;

		// Token: 0x0200090D RID: 2317
		internal class AssetMapping
		{
			// Token: 0x06004A49 RID: 19017 RVA: 0x001B03AC File Offset: 0x001AE5AC
			public AssetMapping()
			{
				this.legacyAssetsTable = new Dictionary<EAssetType, Dictionary<ushort, Asset>>();
				this.legacyAssetsTable.Add(EAssetType.ITEM, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.EFFECT, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.OBJECT, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.RESOURCE, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.VEHICLE, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.ANIMAL, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.MYTHIC, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.SKIN, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.SPAWN, new Dictionary<ushort, Asset>());
				this.legacyAssetsTable.Add(EAssetType.NPC, new Dictionary<ushort, Asset>());
				this.assetDictionary = new Dictionary<Guid, Asset>();
				this.assetList = new List<Asset>();
				this.modificationCounter = 0;
			}

			/// <summary>
			/// Calling this "legacy" is a bit of a stretch because even most of the vanilla assets are
			/// built around the 16-bit IDs. Ideally no new code should be relying on 16-bit IDs however.
			/// </summary>
			// Token: 0x0400321B RID: 12827
			public Dictionary<EAssetType, Dictionary<ushort, Asset>> legacyAssetsTable;

			// Token: 0x0400321C RID: 12828
			public Dictionary<Guid, Asset> assetDictionary;

			// Token: 0x0400321D RID: 12829
			public List<Asset> assetList;

			/// <summary>
			/// Incremented when assets are added or removed.
			/// Used by boombox UI to only refresh songs list if assets have changed.
			/// </summary>
			// Token: 0x0400321E RID: 12830
			public int modificationCounter;
		}
	}
}
