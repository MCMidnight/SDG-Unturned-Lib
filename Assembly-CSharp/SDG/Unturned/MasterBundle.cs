using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Remaps asset load requests into a large asset bundle rather than small individual asset bundles.
	/// </summary>
	// Token: 0x02000313 RID: 787
	public class MasterBundle : Bundle
	{
		/// <summary>
		/// Config that contains the actual large AssetBundle.
		/// </summary>
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060017B8 RID: 6072 RVA: 0x000577E8 File Offset: 0x000559E8
		// (set) Token: 0x060017B9 RID: 6073 RVA: 0x000577F0 File Offset: 0x000559F0
		public MasterBundleConfig cfg { get; protected set; }

		/// <summary>
		/// Asset path relative to the master AssetBundle.
		/// </summary>
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x000577F9 File Offset: 0x000559F9
		// (set) Token: 0x060017BB RID: 6075 RVA: 0x00057801 File Offset: 0x00055A01
		public string relativePath { get; protected set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x0005780A File Offset: 0x00055A0A
		protected override bool willBeUnloadedDuringUse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00057810 File Offset: 0x00055A10
		public override void loadDeferred<T>(string name, out IDeferredAsset<T> asset, LoadedAssetDeferredCallback<T> callback)
		{
			if (Assets.shouldDeferLoadingAssets)
			{
				asset = new DeferredMasterAsset<T>
				{
					masterBundle = this,
					name = name,
					callback = callback
				};
				return;
			}
			base.loadDeferred<T>(name, out asset, callback);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00057858 File Offset: 0x00055A58
		public override T load<T>(string name)
		{
			if (this.cfg.assetBundle == null)
			{
				UnturnedLog.warn("Failed to load '{0}' from master bundle '{1}' because asset bundle was null", new object[]
				{
					name,
					this.cfg.assetBundleName
				});
				return default(T);
			}
			string text = this.cfg.formatAssetPath(this.relativePath + "/" + name);
			string[] array;
			if (!MasterBundle.typeExtensions.TryGetValue(typeof(T), ref array))
			{
				string text2 = "Unknown extension for type: ";
				Type typeFromHandle = typeof(T);
				UnturnedLog.warn(text2 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				return default(T);
			}
			foreach (string text3 in array)
			{
				T t = this.cfg.assetBundle.LoadAsset<T>(text + text3);
				if (t != null)
				{
					this.processLoadedObject<T>(t);
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0005795F File Offset: 0x00055B5F
		public MasterBundle(MasterBundleConfig cfg, string relativePath, string name) : base(name)
		{
			this.cfg = cfg;
			this.relativePath = relativePath;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00057978 File Offset: 0x00055B78
		// Note: this type is marked as 'beforefieldinit'.
		static MasterBundle()
		{
			Dictionary<Type, string[]> dictionary = new Dictionary<Type, string[]>();
			dictionary.Add(typeof(Material), new string[]
			{
				".mat"
			});
			dictionary.Add(typeof(Texture2D), new string[]
			{
				".png",
				".jpg"
			});
			dictionary.Add(typeof(GameObject), new string[]
			{
				".prefab"
			});
			dictionary.Add(typeof(AudioClip), new string[]
			{
				".wav",
				".ogg",
				".mp3"
			});
			MasterBundle.typeExtensions = dictionary;
		}

		// Token: 0x04000AC0 RID: 2752
		private static Dictionary<Type, string[]> typeExtensions;
	}
}
