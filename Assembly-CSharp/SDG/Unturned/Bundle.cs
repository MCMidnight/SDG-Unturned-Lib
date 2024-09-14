using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000297 RID: 663
	public class Bundle
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0004A551 File Offset: 0x00048751
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x0004A559 File Offset: 0x00048759
		public AssetBundle asset { get; protected set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x0004A562 File Offset: 0x00048762
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x0004A56A File Offset: 0x0004876A
		public string resource { get; protected set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x0004A573 File Offset: 0x00048773
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x0004A57B File Offset: 0x0004877B
		public string name { get; protected set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0004A584 File Offset: 0x00048784
		[Obsolete]
		public bool hasResource
		{
			get
			{
				return this.asset == null;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x0004A592 File Offset: 0x00048792
		protected virtual bool willBeUnloadedDuringUse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0004A598 File Offset: 0x00048798
		protected void fixupMaterialForRenderer(Transform rootTransform, Renderer renderer, Material sharedMaterial)
		{
			Shader shader = sharedMaterial.shader;
			if (this.convertShadersToStandard || shader == null)
			{
				sharedMaterial.shader = Shader.Find("Standard");
			}
			else if (this.consolidateShaders)
			{
				Shader shader2 = ShaderConsolidator.findConsolidatedShader(shader);
				if (shader2 != null)
				{
					sharedMaterial.shader = shader2;
				}
				else
				{
					Transform transform = renderer.transform;
					string text = transform.name;
					while (transform != rootTransform)
					{
						transform = transform.parent;
						text = transform.name + "/" + text;
					}
					UnturnedLog.warn("Unable to find consolidated version of shader {0} for material {1} in {2} {3}", new object[]
					{
						shader.name,
						sharedMaterial.name,
						this.name,
						text
					});
				}
			}
			else
			{
				UnturnedLog.error("fixupMaterialForRenderer should not have been called for {0}", new object[]
				{
					this.name
				});
			}
			StandardShaderUtils.maybeFixupMaterial(sharedMaterial);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0004A678 File Offset: 0x00048878
		protected virtual void processLoadedGameObject(GameObject gameObject)
		{
			if (!this.convertShadersToStandard)
			{
				bool flag = this.consolidateShaders;
				return;
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0004A698 File Offset: 0x00048898
		protected virtual void processLoadedMaterial(Material material)
		{
			if (!this.convertShadersToStandard && !this.consolidateShaders)
			{
				return;
			}
			Shader shader = material.shader;
			if (this.convertShadersToStandard || shader == null)
			{
				material.shader = Shader.Find("Standard");
			}
			else if (this.consolidateShaders)
			{
				Shader shader2 = ShaderConsolidator.findConsolidatedShader(shader);
				if (shader2 != null)
				{
					material.shader = shader2;
				}
				else
				{
					UnturnedLog.warn("Unable to find consolidated version of shader {0} for material {1} in {2}", new object[]
					{
						shader.name,
						material.name,
						this.name
					});
				}
			}
			StandardShaderUtils.maybeFixupMaterial(material);
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0004A734 File Offset: 0x00048934
		protected virtual void processLoadedObject<T>(T loadedObject) where T : Object
		{
			if (typeof(T) == typeof(GameObject))
			{
				this.processLoadedGameObject(loadedObject as GameObject);
				return;
			}
			if (typeof(T) == typeof(AudioClip))
			{
				bool willBeUnloadedDuringUse = this.willBeUnloadedDuringUse;
				return;
			}
			typeof(T) == typeof(Material);
		}

		/// <summary>
		/// Save a reference to an object in the asset bundle, but defer loading it until requested by game code.
		/// </summary>
		// Token: 0x06001404 RID: 5124 RVA: 0x0004A7AC File Offset: 0x000489AC
		public virtual void loadDeferred<T>(string name, out IDeferredAsset<T> asset, LoadedAssetDeferredCallback<T> callback = null) where T : Object
		{
			T t = this.load<T>(name);
			NonDeferredAsset<T> nonDeferredAsset;
			nonDeferredAsset.loadedObject = t;
			asset = nonDeferredAsset;
			if (callback != null)
			{
				callback(t);
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0004A7DC File Offset: 0x000489DC
		public virtual T load<T>(string name) where T : Object
		{
			if (this.asset == null)
			{
				return Resources.Load<T>(this.resource + "/" + name);
			}
			if (this.asset.Contains(name))
			{
				T t = this.asset.LoadAsset<T>(name);
				this.processLoadedObject<T>(t);
				return t;
			}
			return default(T);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004A83B File Offset: 0x00048A3B
		public T[] loadAll<T>() where T : Object
		{
			if (!(this.asset != null))
			{
				return null;
			}
			return this.asset.LoadAllAssets<T>();
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004A858 File Offset: 0x00048A58
		public void unload()
		{
			if (this.asset != null)
			{
				this.asset.Unload(false);
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004A874 File Offset: 0x00048A74
		protected Bundle(string name)
		{
			this.asset = null;
			this.resource = null;
			this.name = name;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004A898 File Offset: 0x00048A98
		public Bundle(string path, bool usePath, string nameOverride = null)
		{
			if (ReadWrite.fileExists(path, false, usePath))
			{
				if (this.asset == null)
				{
					this.asset = AssetBundle.LoadFromFile(usePath ? (ReadWrite.PATH + path) : path);
				}
			}
			else
			{
				this.asset = null;
			}
			this.name = ((nameOverride != null) ? nameOverride : ReadWrite.fileName(path));
			if (this.asset == null)
			{
				this.resource = ReadWrite.folderPath(path).Substring(1);
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004A921 File Offset: 0x00048B21
		[Obsolete]
		public Bundle()
		{
			this.asset = null;
			this.name = "#NAME";
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004A942 File Offset: 0x00048B42
		[Obsolete]
		public Object[] load()
		{
			if (this.asset != null)
			{
				return this.asset.LoadAllAssets();
			}
			return null;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0004A95F File Offset: 0x00048B5F
		[Obsolete]
		public Object[] load(Type type)
		{
			if (this.asset != null)
			{
				return this.asset.LoadAllAssets(type);
			}
			return null;
		}

		// Token: 0x040006E1 RID: 1761
		private static List<AudioSource> audioSources = new List<AudioSource>();

		// Token: 0x040006E2 RID: 1762
		private static List<Renderer> renderers = new List<Renderer>();

		// Token: 0x040006E6 RID: 1766
		public bool convertShadersToStandard;

		// Token: 0x040006E7 RID: 1767
		public bool consolidateShaders = true;
	}
}
