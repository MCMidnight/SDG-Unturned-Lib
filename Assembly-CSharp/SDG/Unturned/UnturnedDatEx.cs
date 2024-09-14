using System;
using System.Globalization;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// These are methods moved from the Data class which rely on core types and so cannot go in the UnturnedDat assembly.
	/// </summary>
	// Token: 0x0200042E RID: 1070
	public static class UnturnedDatEx
	{
		// Token: 0x060020A7 RID: 8359 RVA: 0x0007E064 File Offset: 0x0007C264
		public static void ParseGuidOrLegacyId(this DatDictionary dictionary, string key, out Guid guid, out ushort legacyId)
		{
			string text;
			if (dictionary.TryGetString(key, out text) && !string.IsNullOrEmpty(text) && (text.Length != 1 || text.get_Chars(0) != '0'))
			{
				if (ushort.TryParse(text, 511, CultureInfo.InvariantCulture, ref legacyId))
				{
					guid = Guid.Empty;
					return;
				}
				if (Guid.TryParse(text, ref guid))
				{
					legacyId = 0;
					return;
				}
			}
			guid = Guid.Empty;
			legacyId = 0;
		}

		/// <summary>
		/// Intended as a drop-in replacement for existing assets with property uint16s.
		/// </summary>
		// Token: 0x060020A8 RID: 8360 RVA: 0x0007E0D4 File Offset: 0x0007C2D4
		public static ushort ParseGuidOrLegacyId(this DatDictionary dictionary, string key, out Guid guid)
		{
			ushort result;
			dictionary.ParseGuidOrLegacyId(key, out guid, out result);
			return result;
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x0007E0EC File Offset: 0x0007C2EC
		public static AssetReference<T> readAssetReference<T>(this DatDictionary dictionary, string key) where T : Asset
		{
			if (dictionary.ContainsKey(key))
			{
				return new AssetReference<T>(dictionary.ParseGuid(key, default(Guid)));
			}
			return AssetReference<T>.invalid;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x0007E120 File Offset: 0x0007C320
		public static AssetReference<T> readAssetReference<T>(this DatDictionary dictionary, string key, in AssetReference<T> defaultValue) where T : Asset
		{
			if (dictionary.ContainsKey(key))
			{
				return new AssetReference<T>(dictionary.ParseGuid(key, default(Guid)));
			}
			return defaultValue;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x0007E154 File Offset: 0x0007C354
		private static void ParseMasterBundleReference(string key, string value, out string name, out string path, MasterBundleConfig defaultMasterBundle)
		{
			int num = value.IndexOf(':');
			if (num < 0)
			{
				if (defaultMasterBundle != null)
				{
					name = defaultMasterBundle.assetBundleName;
				}
				else
				{
					name = string.Empty;
					Assets.reportError("MasterBundleRef \"" + key + "\" is not associated with a master bundle nor does it specify one");
				}
				path = value;
				return;
			}
			name = value.Substring(0, num);
			path = value.Substring(num + 1);
			if (string.IsNullOrEmpty(name))
			{
				Assets.reportError("MasterBundleRef \"" + key + "\" specified asset bundle name is empty");
			}
			if (string.IsNullOrEmpty(path))
			{
				Assets.reportError("MasterBundleRef \"" + key + "\" specified asset path is empty");
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x0007E1F0 File Offset: 0x0007C3F0
		public static MasterBundleReference<T> readMasterBundleReference<T>(this DatDictionary dictionary, string key, MasterBundleConfig defaultMasterBundle = null) where T : Object
		{
			string value;
			if (dictionary.TryGetString(key, out value))
			{
				string name;
				string path;
				UnturnedDatEx.ParseMasterBundleReference(key, value, out name, out path, defaultMasterBundle ?? Assets.currentMasterBundle);
				return new MasterBundleReference<T>(name, path);
			}
			return MasterBundleReference<T>.invalid;
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x0007E22A File Offset: 0x0007C42A
		public static MasterBundleReference<T> readMasterBundleReference<T>(this DatDictionary dictionary, string key, Bundle defaultBundle = null) where T : Object
		{
			MasterBundle masterBundle = defaultBundle as MasterBundle;
			return dictionary.readMasterBundleReference(key, (masterBundle != null) ? masterBundle.cfg : null);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x0007E248 File Offset: 0x0007C448
		public static AudioReference ReadAudioReference(this DatDictionary dictionary, string key, MasterBundleConfig defaultMasterBundle = null)
		{
			string value;
			if (dictionary.TryGetString(key, out value))
			{
				string assetBundleName;
				string path;
				UnturnedDatEx.ParseMasterBundleReference(key, value, out assetBundleName, out path, defaultMasterBundle ?? Assets.currentMasterBundle);
				return new AudioReference(assetBundleName, path);
			}
			return default(AudioReference);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x0007E286 File Offset: 0x0007C486
		public static AudioReference ReadAudioReference(this DatDictionary dictionary, string key, Bundle defaultBundle = null)
		{
			MasterBundle masterBundle = defaultBundle as MasterBundle;
			return dictionary.ReadAudioReference(key, (masterBundle != null) ? masterBundle.cfg : null);
		}
	}
}
