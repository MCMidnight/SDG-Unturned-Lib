using System;
using System.IO;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000314 RID: 788
	public class MasterBundleConfig
	{
		// Token: 0x060017C1 RID: 6081 RVA: 0x00057A24 File Offset: 0x00055C24
		public MasterBundleConfig(string absoluteDirectory, DatDictionary data, AssetOrigin origin)
		{
			this.directoryPath = absoluteDirectory;
			this.origin = origin;
			this.assetBundleName = data.GetString("Asset_Bundle_Name", null);
			if (string.IsNullOrEmpty(this.assetBundleName))
			{
				throw new Exception("Unspecified Asset_Bundle_Name! This should be the file name and extension of the master asset bundle exported from Unity.");
			}
			this.assetBundleNameWithoutExtension = Path.GetFileNameWithoutExtension(this.assetBundleName);
			this.assetPrefix = data.GetString("Asset_Prefix", null);
			if (string.IsNullOrEmpty(this.assetPrefix))
			{
				throw new Exception("Unspecified Asset_Prefix! This should be the portion of the Unity asset path prior to the /Bundles/ path, e.g. Assets/Bundles/");
			}
			if (data.ContainsKey("Master_Bundle_Version"))
			{
				this.version = data.ParseInt32("Master_Bundle_Version", 0);
			}
			else
			{
				this.version = data.ParseInt32("Asset_Bundle_Version", 2);
			}
			if (this.version < 2)
			{
				throw new Exception("Lowest master bundle version is 2 (default), associated with 2017.4 LTS.");
			}
			if (this.version > 5)
			{
				throw new Exception("Highest master bundle version is 5, associated with 2021 LTS.");
			}
			string assetBundlePath = this.getAssetBundlePath();
			if (!File.Exists(assetBundlePath))
			{
				throw new Exception("Unable to find specified Asset_Bundle_Name next to the config file! Expected path: " + assetBundlePath);
			}
			this.doesHashFileExist = File.Exists(this.getHashFilePath());
		}

		/// <summary>
		/// Absolute path to directory containing bundle and .dat file.
		/// </summary>
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x00057B36 File Offset: 0x00055D36
		// (set) Token: 0x060017C3 RID: 6083 RVA: 0x00057B3E File Offset: 0x00055D3E
		public string directoryPath { get; protected set; }

		/// <summary>
		/// Name of the actual asset bundle file, e.g. Hawaii.unity3d
		/// Asset bundle should be next to this config file.
		/// </summary>
		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x00057B47 File Offset: 0x00055D47
		// (set) Token: 0x060017C5 RID: 6085 RVA: 0x00057B4F File Offset: 0x00055D4F
		public string assetBundleName { get; protected set; }

		/// <summary>
		/// assetBundleName without final .* extension.
		/// </summary>
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060017C6 RID: 6086 RVA: 0x00057B58 File Offset: 0x00055D58
		// (set) Token: 0x060017C7 RID: 6087 RVA: 0x00057B60 File Offset: 0x00055D60
		public string assetBundleNameWithoutExtension { get; protected set; }

		/// <summary>
		/// Prefixed to all asset paths loaded from asset bundle.
		/// Final path is built from assetPrefix + pathRelativeToBundlesFolder + assetName,
		/// e.g. Assets/Hawaii/Bundles + /Objects/Large/House/ + Object.prefab
		/// </summary>
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00057B69 File Offset: 0x00055D69
		// (set) Token: 0x060017C9 RID: 6089 RVA: 0x00057B71 File Offset: 0x00055D71
		public string assetPrefix { get; protected set; }

		/// <summary>
		/// Custom asset bundle version used by Unturned to detect whether imports need
		/// fixing up because they were exported from an older version of Unity.
		/// </summary>
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x00057B7A File Offset: 0x00055D7A
		// (set) Token: 0x060017CB RID: 6091 RVA: 0x00057B82 File Offset: 0x00055D82
		public int version { get; protected set; }

		/// <summary>
		/// Get absolute path to asset bundle file.
		/// </summary>
		// Token: 0x060017CC RID: 6092 RVA: 0x00057B8B File Offset: 0x00055D8B
		public string getAssetBundlePath()
		{
			return Path.Combine(this.directoryPath, this.assetBundleName);
		}

		/// <summary>
		/// Get absolute path to file with per-platform hashes.
		/// </summary>
		// Token: 0x060017CD RID: 6093 RVA: 0x00057B9E File Offset: 0x00055D9E
		public string getHashFilePath()
		{
			return MasterBundleHelper.getHashFileName(Path.Combine(this.directoryPath, this.assetBundleName));
		}

		/// <summary>
		/// Insert path prefix if set.
		/// </summary>
		// Token: 0x060017CE RID: 6094 RVA: 0x00057BB8 File Offset: 0x00055DB8
		public string formatAssetPath(string assetPath)
		{
			if (string.IsNullOrEmpty(this.assetPrefix))
			{
				return assetPath;
			}
			if (this.assetPrefix.EndsWith("/") || assetPath.StartsWith("/"))
			{
				return this.assetPrefix + assetPath;
			}
			return this.assetPrefix + "/" + assetPath;
		}

		/// <summary>
		/// Loaded asset bundle.
		/// </summary>
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00057C11 File Offset: 0x00055E11
		// (set) Token: 0x060017D0 RID: 6096 RVA: 0x00057C19 File Offset: 0x00055E19
		public AssetBundle assetBundle { get; protected set; }

		/// <summary>
		/// Hash of loaded asset bundle file.
		/// This is per-platform, so the server loads a hash file with all platform hashes.
		/// </summary>
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00057C22 File Offset: 0x00055E22
		// (set) Token: 0x060017D2 RID: 6098 RVA: 0x00057C2A File Offset: 0x00055E2A
		public byte[] hash { get; protected set; }

		// Token: 0x060017D3 RID: 6099 RVA: 0x00057C34 File Offset: 0x00055E34
		internal void CopyAssetBundleFromDuplicateConfig(MasterBundleConfig otherConfig)
		{
			this.sourceConfig = otherConfig;
			this.version = otherConfig.version;
			this.assetBundle = otherConfig.assetBundle;
			this.hash = otherConfig.hash;
			this.doesHashFileExist = otherConfig.doesHashFileExist;
			this.serverHashes = otherConfig.serverHashes;
			this.assetBundleCreateRequest = null;
			this.CheckOwnerCustomDataAndMaybeUnload();
		}

		/// <summary>
		/// Load the underlying asset bundle.
		/// </summary>
		// Token: 0x060017D4 RID: 6100 RVA: 0x00057C94 File Offset: 0x00055E94
		public void StartLoad(byte[] inputData, byte[] inputHash)
		{
			UnturnedLog.info(string.Concat(new string[]
			{
				"Loading asset bundle \"",
				this.assetBundleName,
				"\" from \"",
				this.directoryPath,
				"\"..."
			}));
			this.assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(inputData);
			this.hash = inputHash;
			this.loadStartTime = Time.realtimeSinceStartupAsDouble;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00057CFC File Offset: 0x00055EFC
		public void FinishLoad()
		{
			this.assetBundle = this.assetBundleCreateRequest.assetBundle;
			this.CheckOwnerCustomDataAndMaybeUnload();
			if (this.assetBundle != null)
			{
				double num = Time.realtimeSinceStartupAsDouble - this.loadStartTime;
				UnturnedLog.info(string.Format("Loading asset bundle \"{0}\" from \"{1}\" took {2}s", this.assetBundleName, this.directoryPath, num));
				return;
			}
			UnturnedLog.warn(string.Concat(new string[]
			{
				"Failed to load asset bundle \"",
				this.assetBundleName,
				"\" from \"",
				this.directoryPath,
				"\""
			}));
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00057D97 File Offset: 0x00055F97
		public void unload()
		{
			if (this.sourceConfig != null)
			{
				this.assetBundle = null;
				return;
			}
			if (this.assetBundle != null)
			{
				this.assetBundle.Unload(false);
				this.assetBundle = null;
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00057DCC File Offset: 0x00055FCC
		private void CheckOwnerCustomDataAndMaybeUnload()
		{
			if (this.assetBundle == null)
			{
				return;
			}
			string text = this.formatAssetPath("AssetBundleCustomData.asset");
			AssetBundleCustomData assetBundleCustomData = this.assetBundle.LoadAsset<AssetBundleCustomData>(text);
			if (assetBundleCustomData == null)
			{
				return;
			}
			UnturnedLog.info(string.Concat(new string[]
			{
				"Loaded \"",
				this.assetBundleName,
				"\" custom data from \"",
				text,
				"\""
			}));
			bool flag = assetBundleCustomData.ownerWorkshopFileIds != null && assetBundleCustomData.ownerWorkshopFileIds.Count > 0;
			if (this.origin.workshopFileId > 0UL && (assetBundleCustomData.ownerWorkshopFileId > 0UL || flag) && this.origin.workshopFileId != assetBundleCustomData.ownerWorkshopFileId && (!flag || !assetBundleCustomData.ownerWorkshopFileIds.Contains(this.origin.workshopFileId)))
			{
				string text2;
				if (flag)
				{
					text2 = string.Join<ulong>(", ", assetBundleCustomData.ownerWorkshopFileIds);
					if (assetBundleCustomData.ownerWorkshopFileId > 0UL)
					{
						text2 += ", ";
						text2 += assetBundleCustomData.ownerWorkshopFileId.ToString();
					}
				}
				else
				{
					text2 = assetBundleCustomData.ownerWorkshopFileId.ToString();
				}
				UnturnedLog.warn(string.Format("Unloading \"{0}\" because source workshop file ID ({1}) does not match owner workshop file ID(s) ({2})", this.assetBundle, this.origin.workshopFileId, text2));
				this.unload();
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00057F28 File Offset: 0x00056128
		public AssetBundleRequest LoadAssetAsync<T>(string name) where T : Object
		{
			string text = this.formatAssetPath(name);
			return this.assetBundle.LoadAssetAsync<T>(text);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00057F49 File Offset: 0x00056149
		public override string ToString()
		{
			return string.Format("{0} in {1}", this.assetBundleNameWithoutExtension, this.directoryPath);
		}

		// Token: 0x04000AC8 RID: 2760
		internal AssetOrigin origin;

		/// <summary>
		/// True if the server .hash file exists.
		/// Hash file is not used by client, but client uses whether it exists to decide whether to include asset bundle hash in asset hash.
		/// </summary>
		// Token: 0x04000ACB RID: 2763
		internal bool doesHashFileExist;

		/// <summary>
		/// Hashes for Windows, Linux, and Mac asset bundles.
		/// Only loaded on the dedicated server. Null otherwise.
		/// </summary>
		// Token: 0x04000ACC RID: 2764
		internal MasterBundleHash serverHashes;

		// Token: 0x04000ACD RID: 2765
		internal AssetBundleCreateRequest assetBundleCreateRequest;

		// Token: 0x04000ACE RID: 2766
		private double loadStartTime;

		/// <summary>
		/// If true, the associated asset bundle couldn't be loaded and was instead copied from another config.
		/// </summary>
		// Token: 0x04000ACF RID: 2767
		internal MasterBundleConfig sourceConfig;
	}
}
