using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Essentially identical to ContentReference, but MasterBundle is more convenient.
	/// Perhaps in the future all asset/content systems will be consolidated.
	/// </summary>
	// Token: 0x02000316 RID: 790
	public struct MasterBundleReference<T> : IFormattedFileReadable, IFormattedFileWritable, IDatParseable where T : Object
	{
		// Token: 0x060017E0 RID: 6112 RVA: 0x00057FCD File Offset: 0x000561CD
		public MasterBundleReference(string name, string path)
		{
			this.name = name;
			this.path = path;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00057FE0 File Offset: 0x000561E0
		public bool TryParse(IDatNode node)
		{
			DatValue datValue = node as DatValue;
			if (datValue != null)
			{
				if (string.IsNullOrEmpty(datValue.value))
				{
					return false;
				}
				if (datValue.value.Length < 2)
				{
					return false;
				}
				int num = datValue.value.IndexOf(':');
				if (num < 0)
				{
					if (Assets.currentMasterBundle != null)
					{
						this.name = Assets.currentMasterBundle.assetBundleName;
					}
					this.path = datValue.value;
				}
				else
				{
					this.name = datValue.value.Substring(0, num);
					this.path = datValue.value.Substring(num + 1);
				}
				return true;
			}
			else
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary != null)
				{
					this.name = datDictionary.GetString("MasterBundle", null);
					this.path = datDictionary.GetString("AssetPath", null);
					return true;
				}
				return false;
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000580A8 File Offset: 0x000562A8
		public void read(IFormattedFileReader reader)
		{
			IFormattedFileReader formattedFileReader = reader.readObject();
			if (formattedFileReader == null)
			{
				if (Assets.currentMasterBundle != null)
				{
					this.name = Assets.currentMasterBundle.assetBundleName;
				}
				this.path = reader.readValue();
				return;
			}
			this.name = formattedFileReader.readValue("MasterBundle");
			this.path = formattedFileReader.readValue("AssetPath");
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00058105 File Offset: 0x00056305
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue("MasterBundle", this.name);
			writer.writeValue("AssetPath", this.path);
			writer.endObject();
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00058138 File Offset: 0x00056338
		public T loadAsset(bool logWarnings = true)
		{
			if (this.isNull)
			{
				return default(T);
			}
			MasterBundleConfig masterBundleConfig = Assets.findMasterBundleByName(this.name, true);
			if (masterBundleConfig == null || masterBundleConfig.assetBundle == null)
			{
				if (logWarnings)
				{
					UnturnedLog.warn("Unable to find master bundle '{0}' when loading asset '{1}' as {2}", new object[]
					{
						this.name,
						this.path,
						typeof(T).Name
					});
				}
				return default(T);
			}
			string text = masterBundleConfig.formatAssetPath(this.path);
			T t = masterBundleConfig.assetBundle.LoadAsset<T>(text);
			if (t == null && logWarnings)
			{
				UnturnedLog.warn("Failed to load asset '{0}' from master bundle '{1}' as {2}", new object[]
				{
					text,
					this.name,
					typeof(T).Name
				});
			}
			return t;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00058210 File Offset: 0x00056410
		public AssetBundleRequest LoadAssetAsync(bool logWarnings = true)
		{
			if (this.isNull)
			{
				return null;
			}
			MasterBundleConfig masterBundleConfig = Assets.findMasterBundleByName(this.name, true);
			if (masterBundleConfig == null || masterBundleConfig.assetBundle == null)
			{
				if (logWarnings)
				{
					UnturnedLog.warn("Unable to find master bundle '{0}' when async loading asset '{1}' as {2}", new object[]
					{
						this.name,
						this.path,
						typeof(T).Name
					});
				}
				return null;
			}
			string text = masterBundleConfig.formatAssetPath(this.path);
			return masterBundleConfig.assetBundle.LoadAssetAsync<T>(text);
		}

		/// <summary>
		/// Are name or path null or empty?
		/// </summary>
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x00058297 File Offset: 0x00056497
		public bool isNull
		{
			get
			{
				return string.IsNullOrEmpty(this.name) || string.IsNullOrEmpty(this.path);
			}
		}

		/// <summary>
		/// Are both name and path non-null and non-empty?
		/// </summary>
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x000582B3 File Offset: 0x000564B3
		public bool isValid
		{
			get
			{
				return !this.isNull;
			}
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x000582BE File Offset: 0x000564BE
		public override string ToString()
		{
			return string.Format("{0}:{1}", this.name, this.path);
		}

		// Token: 0x04000AD0 RID: 2768
		public static MasterBundleReference<T> invalid = new MasterBundleReference<T>(null, null);

		/// <summary>
		/// Name of master bundle file.
		/// </summary>
		// Token: 0x04000AD1 RID: 2769
		public string name;

		/// <summary>
		/// Path to Unity asset within asset bundle.
		/// </summary>
		// Token: 0x04000AD2 RID: 2770
		public string path;
	}
}
