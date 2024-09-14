using System;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000288 RID: 648
	public abstract class Asset
	{
		// Token: 0x0600133A RID: 4922 RVA: 0x00046698 File Offset: 0x00044898
		public virtual string getFilePath()
		{
			return this.absoluteOriginFilePath;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000466A0 File Offset: 0x000448A0
		public AssetReference<T> getReferenceTo<T>() where T : Asset
		{
			return new AssetReference<T>(this.GUID);
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x000466AD File Offset: 0x000448AD
		// (set) Token: 0x0600133D RID: 4925 RVA: 0x000466E5 File Offset: 0x000448E5
		[Obsolete("Replaced by AssetOrigin class")]
		public EAssetOrigin assetOrigin
		{
			get
			{
				if (this.origin == null)
				{
					return EAssetOrigin.MISC;
				}
				if (this.origin == Assets.coreOrigin || this.origin == Assets.legacyOfficialOrigin)
				{
					return EAssetOrigin.OFFICIAL;
				}
				if (this.origin.workshopFileId != 0UL)
				{
					return EAssetOrigin.WORKSHOP;
				}
				return EAssetOrigin.MISC;
			}
			set
			{
			}
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000466E7 File Offset: 0x000448E7
		public string GetOriginName()
		{
			AssetOrigin assetOrigin = this.origin;
			return ((assetOrigin != null) ? assetOrigin.name : null) ?? "Unknown";
		}

		/// <summary>
		/// Master bundle this asset loaded from.
		/// </summary>
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00046704 File Offset: 0x00044904
		// (set) Token: 0x06001340 RID: 4928 RVA: 0x0004670C File Offset: 0x0004490C
		public MasterBundleConfig originMasterBundle { get; protected set; }

		/// <summary>
		/// Should read/write texture warnings be ignored?
		/// </summary>
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x00046715 File Offset: 0x00044915
		// (set) Token: 0x06001342 RID: 4930 RVA: 0x0004671D File Offset: 0x0004491D
		public bool ignoreTextureReadable { get; protected set; }

		/// <summary>
		/// Hash of the original input file.
		/// </summary>
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x00046726 File Offset: 0x00044926
		// (set) Token: 0x06001344 RID: 4932 RVA: 0x0004672E File Offset: 0x0004492E
		public byte[] hash { get; internal set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x00046737 File Offset: 0x00044937
		internal virtual bool ShouldVerifyHash
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0004673A File Offset: 0x0004493A
		public virtual string FriendlyName
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		/// Maybe temporary? Used when something in-game changes the asset so that it shouldn't be useable on the server anymore.
		/// </summary>
		// Token: 0x06001347 RID: 4935 RVA: 0x00046742 File Offset: 0x00044942
		public virtual void clearHash()
		{
			this.hash = new byte[20];
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00046751 File Offset: 0x00044951
		public void appendHash(byte[] otherHash)
		{
			this.hash = Hash.combineSHA1Hashes(this.hash, otherHash);
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x00046765 File Offset: 0x00044965
		public virtual EAssetType assetCategory
		{
			get
			{
				return EAssetType.NONE;
			}
		}

		/// <summary>
		/// Most asset classes end in "Asset", so in debug strings if asset is clear from context we can remove the unnecessary suffix.
		/// </summary>
		// Token: 0x0600134A RID: 4938 RVA: 0x00046768 File Offset: 0x00044968
		public string GetTypeNameWithoutSuffix()
		{
			string text = base.GetType().Name;
			if (text.EndsWith("Asset"))
			{
				return text.Substring(0, text.Length - 5);
			}
			return text;
		}

		/// <summary>
		/// Remove "Asset" suffix and convert to title case.
		/// </summary>
		// Token: 0x0600134B RID: 4939 RVA: 0x000467A0 File Offset: 0x000449A0
		public virtual string GetTypeFriendlyName()
		{
			string typeNameWithoutSuffix = this.GetTypeNameWithoutSuffix();
			StringBuilder stringBuilder = new StringBuilder(32);
			for (int i = 0; i < typeNameWithoutSuffix.Length; i++)
			{
				char c = typeNameWithoutSuffix.get_Chars(i);
				if (i > 0 && char.IsUpper(c))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x000467F8 File Offset: 0x000449F8
		public string getTypeNameAndIdDisplayString()
		{
			return string.Format("({0}) {1} [{2}]", this.GetTypeFriendlyName(), this.name, this.id);
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004681B File Offset: 0x00044A1B
		public Asset()
		{
			this.name = base.GetType().Name;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00046834 File Offset: 0x00044A34
		public virtual void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			if (bundle != null)
			{
				this.name = bundle.name;
			}
			else
			{
				this.name = "Asset_" + this.id.ToString();
			}
			MasterBundle masterBundle = bundle as MasterBundle;
			if (masterBundle != null)
			{
				this.originMasterBundle = masterBundle.cfg;
			}
			if (data != null)
			{
				this.ignoreNPOT = data.ContainsKey("Ignore_NPOT");
				this.ignoreTextureReadable = data.ContainsKey("Ignore_TexRW");
			}
		}

		/// <summary>
		/// Perform any initialization required when PopulateAsset won't be called.
		/// </summary>
		// Token: 0x0600134F RID: 4943 RVA: 0x000468A8 File Offset: 0x00044AA8
		internal virtual void OnCreatedAtRuntime()
		{
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x000468AA File Offset: 0x00044AAA
		public override string ToString()
		{
			return this.id.ToString() + " - " + this.name;
		}

		/// <summary>
		/// Planning ahead to potentially convert the game to use Unity's newer Addressables feature.
		/// </summary>
		// Token: 0x06001351 RID: 4945 RVA: 0x000468C8 File Offset: 0x00044AC8
		protected T LoadRedirectableAsset<T>(Bundle fromBundle, string defaultName, DatDictionary data, string key) where T : Object
		{
			string text;
			if (data.TryGetString(key, out text))
			{
				int num = text.IndexOf(':');
				MasterBundleConfig masterBundleConfig;
				string text2;
				if (num < 0)
				{
					MasterBundle masterBundle = fromBundle as MasterBundle;
					masterBundleConfig = ((masterBundle != null) ? masterBundle.cfg : Assets.currentMasterBundle);
					text2 = text;
					if (masterBundleConfig == null || masterBundleConfig.assetBundle == null)
					{
						Assets.reportError(this, "unable to load \"{0}\" without masterbundle", text);
						return default(T);
					}
				}
				else
				{
					string text3 = text.Substring(0, num);
					masterBundleConfig = Assets.findMasterBundleByName(text3, true);
					text2 = text.Substring(num + 1);
					if (masterBundleConfig == null || masterBundleConfig.assetBundle == null)
					{
						Assets.reportError(this, string.Concat(new string[]
						{
							"unable to find masterbundle \"",
							text3,
							"\" when loading asset \"",
							text2,
							"\""
						}));
						return default(T);
					}
				}
				string text4 = masterBundleConfig.formatAssetPath(text2);
				T t = masterBundleConfig.assetBundle.LoadAsset<T>(text4);
				if (t == null)
				{
					Assets.reportError(this, string.Concat(new string[]
					{
						"failed to load asset \"",
						text4,
						"\" from \"",
						masterBundleConfig.assetBundleName,
						"\" as ",
						typeof(T).Name
					}));
				}
				return t;
			}
			return fromBundle.load<T>(defaultName);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00046A20 File Offset: 0x00044C20
		protected T loadRequiredAsset<T>(Bundle fromBundle, string name) where T : Object
		{
			T t = fromBundle.load<T>(name);
			if (t == null)
			{
				Assets.reportError(this, "missing '{0}' {1}", name, typeof(T).Name);
			}
			else if (typeof(T) == typeof(GameObject))
			{
				AssetValidation.searchGameObjectForErrors(this, t as GameObject);
			}
			return t;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00046A8D File Offset: 0x00044C8D
		protected void validateAnimation(Animation animComponent, string name)
		{
			if (animComponent.GetClip(name) == null)
			{
				Assets.reportError(this, "{0} missing animation clip '{1}'", animComponent.gameObject, name);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00046AB0 File Offset: 0x00044CB0
		protected bool OriginAllowsVanillaLegacyId
		{
			get
			{
				return this.origin == Assets.coreOrigin || this.origin == Assets.reloadOrigin;
			}
		}

		// Token: 0x0400067A RID: 1658
		public string name;

		// Token: 0x0400067B RID: 1659
		public ushort id;

		// Token: 0x0400067C RID: 1660
		public Guid GUID;

		// Token: 0x0400067D RID: 1661
		internal AssetOrigin origin;

		/// <summary>
		/// If true, an asset with the same ID or GUID has been added to the current asset mapping, replacing this one.
		/// </summary>
		// Token: 0x0400067E RID: 1662
		internal bool hasBeenReplaced;

		/// <summary>
		/// Null or empty if created at runtime, otherwise set by <see cref="T:SDG.Unturned.Assets" /> when loading.
		/// </summary>
		// Token: 0x0400067F RID: 1663
		public string absoluteOriginFilePath;

		/// <summary>
		/// Were this asset's shaders set to Standard and/or consolidated?
		/// Needed for vehicle rotors special case.
		/// </summary>
		// Token: 0x04000681 RID: 1665
		public bool requiredShaderUpgrade;

		/// <summary>
		/// Should texture non-power-of-two warnings be ignored?
		/// Unfortunately some third party assets have odd setups.
		/// </summary>
		// Token: 0x04000682 RID: 1666
		public bool ignoreNPOT;
	}
}
