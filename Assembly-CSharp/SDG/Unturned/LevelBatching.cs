using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Merges textures used in the level into an atlas to assist runtime draw call batching.
	/// </summary>
	// Token: 0x020004DC RID: 1244
	internal class LevelBatching
	{
		// Token: 0x06002624 RID: 9764 RVA: 0x00098E2E File Offset: 0x0009702E
		public static LevelBatching Get()
		{
			return LevelBatching.instance;
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x00098E38 File Offset: 0x00097038
		public void Reset()
		{
			if (this.standardDecalableOpaque == null)
			{
				this.standardDecalableOpaque = new LevelBatching.ShaderGroup();
				this.standardDecalableOpaque.materialTemplate = Resources.Load<Material>("MaterialBatchingTemplates/StandardDecalableOpaque");
			}
			else
			{
				this.standardDecalableOpaque.Clear();
			}
			if (this.standardSpecularSetupDecalableOpaque == null)
			{
				this.standardSpecularSetupDecalableOpaque = new LevelBatching.ShaderGroup();
				this.standardSpecularSetupDecalableOpaque.materialTemplate = Resources.Load<Material>("MaterialBatchingTemplates/StandardSpecularSetupDecalableOpaque");
			}
			else
			{
				this.standardSpecularSetupDecalableOpaque.Clear();
			}
			if (this.batchableCard == null)
			{
				this.batchableCard = new LevelBatching.ShaderGroup();
				this.batchableCard.materialTemplate = Resources.Load<Material>("MaterialBatchingTemplates/Card");
			}
			else
			{
				this.batchableCard.Clear();
			}
			if (this.batchableFoliage == null)
			{
				this.batchableFoliage = new LevelBatching.ShaderGroup();
				this.batchableFoliage.materialTemplate = Resources.Load<Material>("MaterialBatchingTemplates/Foliage");
				this.batchableFoliage.filterMode = FilterMode.Trilinear;
			}
			else
			{
				this.batchableFoliage.Clear();
			}
			if (this.blitMaterial == null)
			{
				this.blitMaterial = Object.Instantiate<Material>(Resources.Load<Material>("Materials/AtlasBlit"));
				this.blitMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			this.loggedMeshes.Clear();
			this.loggedTextures.Clear();
			this.loggedMaterials.Clear();
			this.staticBatchingMeshRenderers.Clear();
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x00098F84 File Offset: 0x00097184
		public void Destroy()
		{
			foreach (Object obj in this.objectsToDestroy)
			{
				Object.Destroy(obj);
			}
			this.objectsToDestroy.Clear();
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00098FE0 File Offset: 0x000971E0
		public void AddLevelObject(LevelObject levelObject)
		{
			if (levelObject == null || levelObject.asset == null || levelObject.asset.shouldExcludeFromLevelBatching)
			{
				return;
			}
			if (levelObject.transform != null)
			{
				if (levelObject.rubble != null && levelObject.rubble.rubbleInfos != null)
				{
					foreach (RubbleInfo rubbleInfo in levelObject.rubble.rubbleInfos)
					{
						if (rubbleInfo.ragdolls != null)
						{
							foreach (RubbleRagdollInfo rubbleRagdollInfo in rubbleInfo.ragdolls)
							{
								if (rubbleRagdollInfo.ragdollGameObject != null)
								{
									this.ignoreTransforms.Add(rubbleRagdollInfo.ragdollGameObject.transform);
								}
							}
						}
					}
				}
				this.AddGameObject(levelObject.transform.gameObject);
				this.ignoreTransforms.Clear();
			}
			if (levelObject.skybox != null)
			{
				this.AddGameObject(levelObject.skybox.gameObject);
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000990D8 File Offset: 0x000972D8
		public void AddResourceSpawnpoint(ResourceSpawnpoint resourceSpawnpoint)
		{
			if (resourceSpawnpoint == null || resourceSpawnpoint.asset == null || resourceSpawnpoint.asset.shouldExcludeFromLevelBatching)
			{
				return;
			}
			if (resourceSpawnpoint.model != null)
			{
				this.AddGameObject(resourceSpawnpoint.model.gameObject);
			}
			if (resourceSpawnpoint.skybox != null)
			{
				this.AddGameObject(resourceSpawnpoint.skybox.gameObject);
			}
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0009913C File Offset: 0x0009733C
		public void ApplyTextureAtlas()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			bool shouldPreview = Provider.isServer && this.wantsToPreviewTextureAtlas;
			this.ApplyTextureAtlas(this.standardDecalableOpaque, shouldPreview);
			this.ApplyTextureAtlas(this.standardSpecularSetupDecalableOpaque, shouldPreview);
			this.ApplyTextureAtlas(this.batchableCard, shouldPreview);
			this.ApplyTextureAtlas(this.batchableFoliage, shouldPreview);
			this.DestroyColorTextures();
			stopwatch.Stop();
			UnturnedLog.info(string.Format("Level texture atlas generation took: {0}ms", stopwatch.ElapsedMilliseconds));
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000991C0 File Offset: 0x000973C0
		public void ApplyStaticBatching()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			GameObject[] array = new GameObject[this.staticBatchingMeshRenderers.Count];
			LevelBatching.StaticBatchingInitialState[] array2 = new LevelBatching.StaticBatchingInitialState[array.Length];
			List<AudioSource> list = new List<AudioSource>(array.Length);
			List<AudioSource> list2 = new List<AudioSource>(16);
			bool flag = Provider.isServer && this.wantsToPreviewUniqueMaterials;
			Dictionary<Material, List<MeshRenderer>> dictionary = null;
			if (flag)
			{
				dictionary = new Dictionary<Material, List<MeshRenderer>>();
			}
			for (int i = 0; i < array.Length; i++)
			{
				MeshRenderer meshRenderer = this.staticBatchingMeshRenderers[i];
				Transform transform = meshRenderer.transform;
				GameObject gameObject = meshRenderer.gameObject;
				LevelBatching.StaticBatchingInitialState staticBatchingInitialState = default(LevelBatching.StaticBatchingInitialState);
				staticBatchingInitialState.parent = transform.parent;
				staticBatchingInitialState.wasEnabled = meshRenderer.enabled;
				staticBatchingInitialState.wasActive = gameObject.activeSelf;
				array[i] = gameObject;
				array2[i] = staticBatchingInitialState;
				meshRenderer.enabled = true;
				if (staticBatchingInitialState.parent != null)
				{
					transform.parent = null;
				}
				if (!staticBatchingInitialState.wasActive)
				{
					gameObject.SetActive(true);
				}
				gameObject.GetComponentsInChildren<AudioSource>(true, list2);
				foreach (AudioSource audioSource in list2)
				{
					if (audioSource.enabled)
					{
						audioSource.enabled = false;
						list.Add(audioSource);
					}
				}
				if (flag && meshRenderer.sharedMaterial != null)
				{
					List<MeshRenderer> list3;
					if (!dictionary.TryGetValue(meshRenderer.sharedMaterial, ref list3))
					{
						list3 = new List<MeshRenderer>();
						dictionary[meshRenderer.sharedMaterial] = list3;
					}
					list3.Add(meshRenderer);
				}
			}
			if (flag)
			{
				List<List<MeshRenderer>> list4 = Enumerable.ToList<List<MeshRenderer>>(dictionary.Values);
				for (int j = list4.Count - 1; j >= 0; j--)
				{
					if (list4[j].Count < 2)
					{
						list4.RemoveAtFast(j);
					}
				}
				list4.Sort((List<MeshRenderer> lhs, List<MeshRenderer> rhs) => rhs.Count - lhs.Count);
				int count = list4.Count;
				float num = 1f / (float)count;
				float num2 = Random.value;
				for (int k = 0; k < count; k++)
				{
					float num3 = (float)k / (float)count;
					float s = 1f;
					float v = 1f - num3 * 0.75f;
					Color value = Color.HSVToRGB(num2, s, v);
					num2 = (num2 + num) % 1f;
					Material material = Object.Instantiate<Material>(this.standardDecalableOpaque.materialTemplate);
					this.objectsToDestroy.Add(material);
					material.SetColor(this.propertyID_Color, value);
					foreach (MeshRenderer meshRenderer2 in list4[k])
					{
						meshRenderer2.sharedMaterial = material;
					}
				}
			}
			GameObject staticBatchRoot = new GameObject("Static Batching Root (LevelBatching)");
			StaticBatchingUtility.Combine(array, staticBatchRoot);
			for (int l = 0; l < array.Length; l++)
			{
				MeshRenderer meshRenderer3 = this.staticBatchingMeshRenderers[l];
				Transform transform2 = meshRenderer3.transform;
				GameObject gameObject2 = meshRenderer3.gameObject;
				LevelBatching.StaticBatchingInitialState staticBatchingInitialState2 = array2[l];
				meshRenderer3.enabled = staticBatchingInitialState2.wasEnabled;
				if (!staticBatchingInitialState2.wasActive)
				{
					gameObject2.SetActive(staticBatchingInitialState2.wasActive);
				}
				if (staticBatchingInitialState2.parent != null)
				{
					transform2.parent = staticBatchingInitialState2.parent;
				}
			}
			foreach (AudioSource audioSource2 in list)
			{
				audioSource2.enabled = true;
			}
			stopwatch.Stop();
			UnturnedLog.info(string.Format("Level static batching took: {0}ms", stopwatch.ElapsedMilliseconds));
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000995AC File Offset: 0x000977AC
		private void AddGameObject(GameObject gameObject)
		{
			this.renderers.Clear();
			gameObject.GetComponentsInChildren<Renderer>(true, this.renderers);
			foreach (Renderer renderer in this.renderers)
			{
				if (this.ignoreTransforms.Count > 0)
				{
					bool flag = false;
					foreach (Transform parent in this.ignoreTransforms)
					{
						if (renderer.transform.IsChildOf(parent))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
				}
				MeshRenderer meshRenderer = renderer as MeshRenderer;
				if (meshRenderer != null)
				{
					MeshFilter component = renderer.GetComponent<MeshFilter>();
					Mesh mesh = (component != null) ? component.sharedMesh : null;
					if (mesh != null && this.CanBatchMesh(mesh, renderer))
					{
						this.AddMesh(component, meshRenderer);
						this.staticBatchingMeshRenderers.Add(meshRenderer);
					}
				}
				else
				{
					SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
					if (skinnedMeshRenderer != null)
					{
						Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
						if (sharedMesh != null && this.CanBatchMesh(sharedMesh, renderer))
						{
							this.AddSkinnedMesh(skinnedMeshRenderer);
						}
					}
				}
			}
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000996FC File Offset: 0x000978FC
		private LevelBatching.TextureUsers AddMeshCommon(Renderer renderer)
		{
			this.sharedMaterials.Clear();
			renderer.GetSharedMaterials(this.sharedMaterials);
			if (this.sharedMaterials.Count < 1)
			{
				if (this.shouldLogTextureAtlasExclusions)
				{
					UnturnedLog.info("Excluding renderer \"" + renderer.GetSceneHierarchyPath() + "\" from atlas because it has no materials");
				}
				return null;
			}
			if (this.sharedMaterials.Count > 1)
			{
				if (this.shouldLogTextureAtlasExclusions)
				{
					UnturnedLog.info("Excluding renderer \"" + renderer.GetSceneHierarchyPath() + "\" from atlas because more than one material is not supported (yet?)");
				}
				return null;
			}
			Material material = this.sharedMaterials[0];
			if (material == null)
			{
				if (this.shouldLogTextureAtlasExclusions)
				{
					UnturnedLog.info("Excluding renderer \"" + renderer.GetSceneHierarchyPath() + "\" from atlas because material is null");
				}
				return null;
			}
			if (material.name.EndsWith(" (Instance)"))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because it was probably instantiated for dynamic use"
					}));
				}
				return null;
			}
			Shader shader = material.shader;
			if (shader == null)
			{
				return null;
			}
			if (!material.HasProperty(this.propertyID_MainTex))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because shader \"",
						shader.name,
						"\" does not use a main texture"
					}));
				}
				return null;
			}
			LevelBatching.ShaderGroup shaderGroup = null;
			Texture2D texture2D = material.mainTexture as Texture2D;
			LevelBatching.UniqueTextureConfiguration texture = default(LevelBatching.UniqueTextureConfiguration);
			texture.texture = texture2D;
			texture.color = Color.white;
			bool isGeneratedTexture = false;
			if (texture2D == null)
			{
				if (shader.name == "Standard (Decalable)")
				{
					if (this.CanAtlasStandardMaterialSimpleOpaque(material, renderer, false))
					{
						texture.texture = this.GetOrAddColorTexture(material);
						isGeneratedTexture = true;
						shaderGroup = this.standardDecalableOpaque;
					}
				}
				else if (shader.name == "Standard (Specular setup) (Decalable)" && this.CanAtlasStandardMaterialSimpleOpaque(material, renderer, true))
				{
					texture.texture = this.GetOrAddColorTexture(material);
					isGeneratedTexture = true;
					shaderGroup = this.standardSpecularSetupDecalableOpaque;
				}
			}
			else
			{
				if (texture2D.width > 128 || texture2D.height > 128)
				{
					if (this.shouldLogTextureAtlasExclusions && this.loggedTextures.Add(texture2D))
					{
						UnturnedLog.info(string.Format("Excluding texture \"{0}\" in material \"{1}\" renderer \"{2}\" from atlas because dimensions ({3}x{4}) are higher than limit ({5}x{6})", new object[]
						{
							texture2D.name,
							material.name,
							renderer.GetSceneHierarchyPath(),
							texture2D.width,
							texture2D.height,
							128,
							128
						}));
					}
					return null;
				}
				if (texture2D.wrapMode != TextureWrapMode.Clamp)
				{
					if (this.shouldLogTextureAtlasExclusions && this.loggedTextures.Add(texture2D))
					{
						UnturnedLog.info(string.Format("Excluding texture \"{0}\" in material \"{1}\" renderer \"{2}\" from atlas because Wrap Mode ({3}) is not Clamp", new object[]
						{
							texture2D.name,
							material.name,
							renderer.GetSceneHierarchyPath(),
							texture2D.wrapMode
						}));
					}
					return null;
				}
				if (shader.name == "Standard (Decalable)")
				{
					if (this.CanAtlasStandardMaterialSimpleOpaque(material, renderer, false) && this.CanAtlasTextureFilterMode(texture2D, material, renderer, FilterMode.Point))
					{
						shaderGroup = this.standardDecalableOpaque;
						texture.color = material.GetColor(this.propertyID_Color);
					}
				}
				else if (shader.name == "Standard (Specular setup) (Decalable)")
				{
					if (this.CanAtlasStandardMaterialSimpleOpaque(material, renderer, true) && this.CanAtlasTextureFilterMode(texture2D, material, renderer, FilterMode.Point))
					{
						shaderGroup = this.standardSpecularSetupDecalableOpaque;
						texture.color = material.GetColor(this.propertyID_Color);
					}
				}
				else if (shader.name == "Custom/Card")
				{
					shaderGroup = this.batchableCard;
				}
				else if (shader.name == "Custom/Foliage" && this.CanAtlasTextureFilterMode(texture2D, material, renderer, FilterMode.Trilinear))
				{
					shaderGroup = this.batchableFoliage;
				}
			}
			if (shaderGroup != null)
			{
				LevelBatching.TextureUsers orAddListForTexture = shaderGroup.GetOrAddListForTexture(texture);
				orAddListForTexture.isGeneratedTexture = isGeneratedTexture;
				orAddListForTexture.renderersUsingTexture.Add(renderer);
				return orAddListForTexture;
			}
			return null;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x00099B6C File Offset: 0x00097D6C
		private void AddMesh(MeshFilter meshFilter, MeshRenderer meshRenderer)
		{
			LevelBatching.TextureUsers textureUsers = this.AddMeshCommon(meshRenderer);
			if (textureUsers != null)
			{
				textureUsers.AddMeshFilter(meshFilter);
			}
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00099B8C File Offset: 0x00097D8C
		private void AddSkinnedMesh(SkinnedMeshRenderer skinnedMeshRenderer)
		{
			LevelBatching.TextureUsers textureUsers = this.AddMeshCommon(skinnedMeshRenderer);
			if (textureUsers != null)
			{
				textureUsers.AddSkinnedMeshRenderer(skinnedMeshRenderer);
			}
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x00099BAC File Offset: 0x00097DAC
		private bool CanBatchMesh(Mesh mesh, Renderer renderer)
		{
			if (mesh.isReadable)
			{
				return true;
			}
			if (this.shouldLogTextureAtlasExclusions && this.loggedMeshes.Add(mesh))
			{
				UnturnedLog.info(string.Concat(new string[]
				{
					"Excluding mesh \"",
					mesh.name,
					"\" in renderer \"",
					renderer.GetSceneHierarchyPath(),
					"\" from level batching because it is not CPU-readable"
				}));
			}
			return false;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00099C1C File Offset: 0x00097E1C
		private bool CanAtlasTextureFilterMode(Texture2D texture, Material material, Renderer renderer, FilterMode requiredFilterMode)
		{
			if (texture.filterMode == requiredFilterMode)
			{
				return true;
			}
			if (this.shouldLogTextureAtlasExclusions && this.loggedTextures.Add(texture))
			{
				UnturnedLog.info(string.Format("Excluding texture \"{0}\" in material \"{1}\" renderer \"{2}\" from atlas because Filter Mode ({3}) is not {4}", new object[]
				{
					texture.name,
					material.name,
					renderer.GetSceneHierarchyPath(),
					texture.filterMode,
					requiredFilterMode
				}));
			}
			return false;
		}

		/// <summary>
		/// Most objects in Unturned use the standard shader without transparency/emissive/detail/etc.
		/// </summary>
		// Token: 0x06002631 RID: 9777 RVA: 0x00099C9C File Offset: 0x00097E9C
		private bool CanAtlasStandardMaterialSimpleOpaque(Material material, Renderer renderer, bool isSpecular)
		{
			if (!Mathf.Approximately(material.GetFloat(this.propertyID_Mode), 0f))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" in renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because Mode is not Opaque"
					}));
				}
				return false;
			}
			if (isSpecular)
			{
				if (!material.GetColor(this.propertyID_SpecColor).IsNearlyBlack(0.001f))
				{
					if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
					{
						UnturnedLog.info(string.Concat(new string[]
						{
							"Excluding material \"",
							material.name,
							"\" in renderer \"",
							renderer.GetSceneHierarchyPath(),
							"\" from atlas because Specular Color is not black"
						}));
					}
					return false;
				}
				if (material.IsKeywordEnabled("_SPECGLOSSMAP"))
				{
					if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
					{
						UnturnedLog.info(string.Concat(new string[]
						{
							"Excluding material \"",
							material.name,
							"\" in renderer \"",
							renderer.GetSceneHierarchyPath(),
							"\" from atlas because Specular Map is enabled"
						}));
					}
					return false;
				}
			}
			else
			{
				if (!Mathf.Approximately(material.GetFloat(this.propertyID_Metallic), 0f))
				{
					if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
					{
						UnturnedLog.info(string.Concat(new string[]
						{
							"Excluding material \"",
							material.name,
							"\" in renderer \"",
							renderer.GetSceneHierarchyPath(),
							"\" from atlas because Metallic is not zero"
						}));
					}
					return false;
				}
				if (material.IsKeywordEnabled("_METALLICGLOSSMAP"))
				{
					if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
					{
						UnturnedLog.info(string.Concat(new string[]
						{
							"Excluding material \"",
							material.name,
							"\" in renderer \"",
							renderer.GetSceneHierarchyPath(),
							"\" from atlas because Metallic Map is enabled"
						}));
					}
					return false;
				}
			}
			if (!Mathf.Approximately(material.GetFloat(this.propertyID_Glossiness), 0f))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" in renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because Smoothness is not zero"
					}));
				}
				return false;
			}
			if (material.IsKeywordEnabled("_NORMALMAP"))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" in renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because Normal Map is enabled"
					}));
				}
				return false;
			}
			if (material.IsKeywordEnabled("_EMISSION"))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" in renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because Emission is enabled"
					}));
				}
				return false;
			}
			if (material.IsKeywordEnabled("_PARALLAXMAP"))
			{
				if (this.shouldLogTextureAtlasExclusions && this.loggedMaterials.Add(material))
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Excluding material \"",
						material.name,
						"\" in renderer \"",
						renderer.GetSceneHierarchyPath(),
						"\" from atlas because Parallax Map is enabled"
					}));
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x0009A064 File Offset: 0x00098264
		private Texture2D GetOrAddColorTexture(Material material)
		{
			Texture2D texture2D;
			if (!this.colorTextures.TryGetValue(material, ref texture2D))
			{
				texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				texture2D.filterMode = FilterMode.Point;
				texture2D.SetPixel(0, 0, material.GetColor(this.propertyID_Color));
				texture2D.Apply(false, false);
				this.colorTextures.Add(material, texture2D);
			}
			return texture2D;
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0009A0C4 File Offset: 0x000982C4
		private void DestroyColorTextures()
		{
			foreach (KeyValuePair<Material, Texture2D> keyValuePair in this.colorTextures)
			{
				Object.Destroy(keyValuePair.Value);
			}
			this.colorTextures.Clear();
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0009A128 File Offset: 0x00098328
		private void ApplyTextureAtlas(LevelBatching.ShaderGroup group, bool shouldPreview)
		{
			Material materialTemplate = group.materialTemplate;
			Dictionary<LevelBatching.UniqueTextureConfiguration, LevelBatching.TextureUsers> batchableTextures = group.batchableTextures;
			UnturnedLog.info(string.Format("{0} texture(s) in {1} group", batchableTextures.Count, group.materialTemplate.shader.name));
			if (batchableTextures.Count > 0)
			{
				Texture2D texture2D = new Texture2D(16, 16);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				texture2D.filterMode = group.filterMode;
				Texture2D[] array = new Texture2D[batchableTextures.Count];
				LevelBatching.TextureUsers[] array2 = new LevelBatching.TextureUsers[batchableTextures.Count];
				RenderTexture active = RenderTexture.active;
				int num = 0;
				foreach (KeyValuePair<LevelBatching.UniqueTextureConfiguration, LevelBatching.TextureUsers> keyValuePair in batchableTextures)
				{
					LevelBatching.UniqueTextureConfiguration key = keyValuePair.Key;
					Texture2D texture = key.texture;
					RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
					this.blitMaterial.SetColor(this.propertyID_Color, key.color);
					Graphics.Blit(texture, temporary, this.blitMaterial);
					RenderTexture.active = temporary;
					Texture2D texture2D2 = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false, true);
					texture2D2.ReadPixels(new Rect(0f, 0f, (float)texture.width, (float)texture.height), 0, 0);
					array[num] = texture2D2;
					array2[num] = keyValuePair.Value;
					RenderTexture.ReleaseTemporary(temporary);
					num++;
				}
				RenderTexture.active = active;
				Rect[] array3 = texture2D.PackTextures(array, 0, 2048, true);
				if (array3 != null)
				{
					this.objectsToDestroy.Add(texture2D);
					Material material = Object.Instantiate<Material>(materialTemplate);
					this.objectsToDestroy.Add(material);
					if (!shouldPreview)
					{
						material.mainTexture = texture2D;
					}
					Vector2 vector = texture2D.texelSize * 0.001f;
					Vector2 vector2 = vector * 2f;
					for (int i = 0; i < array2.Length; i++)
					{
						LevelBatching.TextureUsers textureUsers = array2[i];
						foreach (KeyValuePair<Mesh, LevelBatching.MeshUsers> keyValuePair2 in textureUsers.componentsUsingMesh)
						{
							Mesh key2 = keyValuePair2.Key;
							Mesh mesh = Object.Instantiate<Mesh>(key2);
							this.objectsToDestroy.Add(mesh);
							this.uvs.Clear();
							mesh.GetUVs(0, this.uvs);
							if (textureUsers.isGeneratedTexture)
							{
								Rect rect = array3[i];
								Vector2 vector3 = new Vector2(rect.x + 0.5f * rect.width, rect.y + 0.5f * rect.height);
								for (int j = 0; j < this.uvs.Count; j++)
								{
									this.uvs[j] = vector3;
								}
							}
							else
							{
								if (this.shouldValidateUVs)
								{
									this.ValidateUVs(key2, keyValuePair2.Value);
								}
								for (int k = 0; k < this.uvs.Count; k++)
								{
									Rect rect2 = array3[i];
									Vector2 vector4 = this.uvs[k];
									vector4.x = rect2.x + vector.x + vector4.x * (rect2.width - vector2.x);
									vector4.y = rect2.y + vector.y + vector4.y * (rect2.height - vector2.y);
									this.uvs[k] = vector4;
								}
							}
							mesh.SetUVs(0, this.uvs, 0, this.uvs.Count);
							foreach (MeshFilter meshFilter in keyValuePair2.Value.meshFilters)
							{
								meshFilter.sharedMesh = mesh;
							}
							foreach (SkinnedMeshRenderer skinnedMeshRenderer in keyValuePair2.Value.skinnedMeshRenderers)
							{
								skinnedMeshRenderer.sharedMesh = mesh;
							}
						}
						foreach (Renderer renderer in textureUsers.renderersUsingTexture)
						{
							renderer.sharedMaterial = material;
						}
					}
				}
				else
				{
					Object.Destroy(texture2D);
				}
				Texture2D[] array4 = array;
				for (int l = 0; l < array4.Length; l++)
				{
					Object.Destroy(array4[l]);
				}
			}
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0009A640 File Offset: 0x00098840
		private void ValidateUVs(Mesh originalMesh, LevelBatching.MeshUsers meshUsers)
		{
			bool flag = false;
			foreach (Vector2 vector in this.uvs)
			{
				if (vector.x < 0f || vector.y < 0f || vector.x > 1f || vector.y > 1f)
				{
					flag = true;
					break;
				}
			}
			if (flag && this.loggedMeshes.Add(originalMesh))
			{
				Component component = meshUsers.meshFilters.HeadOrDefault<MeshFilter>();
				if (component == null)
				{
					component = meshUsers.skinnedMeshRenderers.HeadOrDefault<SkinnedMeshRenderer>();
				}
				UnturnedLog.error(string.Concat(new string[]
				{
					"Mesh \"",
					originalMesh.name,
					"\" in renderer \"",
					(component != null) ? component.GetSceneHierarchyPath() : null,
					"\" has UVs outside [0, 1] range (should be excluded from level batching)"
				}));
			}
		}

		/// <summary>
		/// Skip renderer children of these transforms, if any.
		/// For example we skip lights with material instances and rubble debris.
		/// </summary>
		// Token: 0x040013AD RID: 5037
		private List<Transform> ignoreTransforms = new List<Transform>();

		// Token: 0x040013AE RID: 5038
		private List<Renderer> renderers = new List<Renderer>();

		// Token: 0x040013AF RID: 5039
		private List<Vector2> uvs = new List<Vector2>();

		// Token: 0x040013B0 RID: 5040
		private List<Material> sharedMaterials = new List<Material>();

		// Token: 0x040013B1 RID: 5041
		private LevelBatching.ShaderGroup standardDecalableOpaque;

		// Token: 0x040013B2 RID: 5042
		private LevelBatching.ShaderGroup standardSpecularSetupDecalableOpaque;

		// Token: 0x040013B3 RID: 5043
		private LevelBatching.ShaderGroup batchableCard;

		// Token: 0x040013B4 RID: 5044
		private LevelBatching.ShaderGroup batchableFoliage;

		// Token: 0x040013B5 RID: 5045
		private Material blitMaterial;

		// Token: 0x040013B6 RID: 5046
		internal static LevelBatching instance;

		// Token: 0x040013B7 RID: 5047
		private int propertyID_MainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x040013B8 RID: 5048
		private int propertyID_Mode = Shader.PropertyToID("_Mode");

		// Token: 0x040013B9 RID: 5049
		private int propertyID_Color = Shader.PropertyToID("_Color");

		// Token: 0x040013BA RID: 5050
		private int propertyID_SpecColor = Shader.PropertyToID("_SpecColor");

		// Token: 0x040013BB RID: 5051
		private int propertyID_Metallic = Shader.PropertyToID("_Metallic");

		// Token: 0x040013BC RID: 5052
		private int propertyID_Glossiness = Shader.PropertyToID("_Glossiness");

		/// <summary>
		/// Meshes we logged an explanation for as to why they can't be atlased.
		/// </summary>
		// Token: 0x040013BD RID: 5053
		private HashSet<Mesh> loggedMeshes = new HashSet<Mesh>();

		/// <summary>
		/// Textures we logged an explanation for as to why they can't be atlased.
		/// </summary>
		// Token: 0x040013BE RID: 5054
		private HashSet<Texture2D> loggedTextures = new HashSet<Texture2D>();

		/// <summary>
		/// Materials we logged an explanation for as to why they can't be atlased.
		/// </summary>
		// Token: 0x040013BF RID: 5055
		private HashSet<Material> loggedMaterials = new HashSet<Material>();

		// Token: 0x040013C0 RID: 5056
		private List<MeshRenderer> staticBatchingMeshRenderers = new List<MeshRenderer>();

		/// <summary>
		/// Objects instantiated for the lifetime of the level that should be destroyed when exiting the level.
		/// </summary>
		// Token: 0x040013C1 RID: 5057
		private List<Object> objectsToDestroy = new List<Object>();

		/// <summary>
		/// If true, don't assign texture atlas to material so batched materials are obvious.
		/// </summary>
		// Token: 0x040013C2 RID: 5058
		private CommandLineFlag wantsToPreviewTextureAtlas = new CommandLineFlag(false, "-PreviewLevelBatchingTextureAtlas");

		/// <summary>
		/// If true, replace each unique material with a colored one before static batching.
		/// </summary>
		// Token: 0x040013C3 RID: 5059
		private CommandLineFlag wantsToPreviewUniqueMaterials = new CommandLineFlag(false, "-PreviewLevelBatchingUniqueMaterials");

		/// <summary>
		/// If true, log why texture/material can't be included in atlas.
		/// </summary>
		// Token: 0x040013C4 RID: 5060
		private CommandLineFlag shouldLogTextureAtlasExclusions = new CommandLineFlag(false, "-LogLevelBatchingTextureAtlasExclusions");

		/// <summary>
		/// If true, log if mesh has UVs outside [0, 1] range.
		/// </summary>
		// Token: 0x040013C5 RID: 5061
		private CommandLineFlag shouldValidateUVs = new CommandLineFlag(false, "-ValidateLevelBatchingUVs");

		/// <summary>
		/// We generate a 1x1 texture for materials without one.
		/// </summary>
		// Token: 0x040013C6 RID: 5062
		private Dictionary<Material, Texture2D> colorTextures = new Dictionary<Material, Texture2D>();

		/// <summary>
		/// Tracks which mesh filters and skinned mesh renderers were referencing a given mesh.
		/// </summary>
		// Token: 0x02000953 RID: 2387
		private class MeshUsers
		{
			// Token: 0x04003320 RID: 13088
			public List<MeshFilter> meshFilters = new List<MeshFilter>();

			// Token: 0x04003321 RID: 13089
			public List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
		}

		// Token: 0x02000954 RID: 2388
		private struct UniqueTextureConfiguration : IEquatable<LevelBatching.UniqueTextureConfiguration>
		{
			// Token: 0x06004B01 RID: 19201 RVA: 0x001B2F91 File Offset: 0x001B1191
			public bool Equals(LevelBatching.UniqueTextureConfiguration other)
			{
				return this.texture == other.texture && this.color == other.color;
			}

			// Token: 0x06004B02 RID: 19202 RVA: 0x001B2FB9 File Offset: 0x001B11B9
			public override int GetHashCode()
			{
				return this.texture.GetHashCode() ^ this.color.GetHashCode();
			}

			// Token: 0x04003322 RID: 13090
			public Texture2D texture;

			// Token: 0x04003323 RID: 13091
			public Color color;
		}

		/// <summary>
		/// Tracks which meshes and materials were referencing a given texture.
		/// </summary>
		// Token: 0x02000955 RID: 2389
		private class TextureUsers
		{
			// Token: 0x06004B03 RID: 19203 RVA: 0x001B2FD8 File Offset: 0x001B11D8
			public void AddMeshFilter(MeshFilter meshFilter)
			{
				this.GetOrAddListForMesh(meshFilter.sharedMesh).meshFilters.Add(meshFilter);
			}

			// Token: 0x06004B04 RID: 19204 RVA: 0x001B2FF1 File Offset: 0x001B11F1
			public void AddSkinnedMeshRenderer(SkinnedMeshRenderer skinnedMeshRenderer)
			{
				this.GetOrAddListForMesh(skinnedMeshRenderer.sharedMesh).skinnedMeshRenderers.Add(skinnedMeshRenderer);
			}

			// Token: 0x06004B05 RID: 19205 RVA: 0x001B300C File Offset: 0x001B120C
			private LevelBatching.MeshUsers GetOrAddListForMesh(Mesh mesh)
			{
				LevelBatching.MeshUsers meshUsers;
				if (!this.componentsUsingMesh.TryGetValue(mesh, ref meshUsers))
				{
					meshUsers = new LevelBatching.MeshUsers();
					this.componentsUsingMesh.Add(mesh, meshUsers);
				}
				return meshUsers;
			}

			/// <summary>
			/// If true, UVs should be centered and overridden because original mesh was not textured. 
			/// </summary>
			// Token: 0x04003324 RID: 13092
			public bool isGeneratedTexture;

			/// <summary>
			/// Maps original mesh to any mesh filters using it.
			/// When mesh's UVs are modified the mesh filters need to be pointed at the copied mesh.
			/// </summary>
			// Token: 0x04003325 RID: 13093
			public Dictionary<Mesh, LevelBatching.MeshUsers> componentsUsingMesh = new Dictionary<Mesh, LevelBatching.MeshUsers>();

			/// <summary>
			/// Renderers with a material using the texture.
			/// After combining texture the renderers need to be pointed at the combined material.
			/// </summary>
			// Token: 0x04003326 RID: 13094
			public List<Renderer> renderersUsingTexture = new List<Renderer>();
		}

		/// <summary>
		/// Tracks which textures were referencing a given shader.
		/// </summary>
		// Token: 0x02000956 RID: 2390
		private class ShaderGroup
		{
			// Token: 0x06004B07 RID: 19207 RVA: 0x001B305C File Offset: 0x001B125C
			public LevelBatching.TextureUsers GetOrAddListForTexture(LevelBatching.UniqueTextureConfiguration texture)
			{
				LevelBatching.TextureUsers textureUsers;
				if (!this.batchableTextures.TryGetValue(texture, ref textureUsers))
				{
					textureUsers = new LevelBatching.TextureUsers();
					this.batchableTextures.Add(texture, textureUsers);
				}
				return textureUsers;
			}

			// Token: 0x06004B08 RID: 19208 RVA: 0x001B308D File Offset: 0x001B128D
			public void Clear()
			{
				this.batchableTextures.Clear();
			}

			// Token: 0x04003327 RID: 13095
			public Material materialTemplate;

			// Token: 0x04003328 RID: 13096
			public Dictionary<LevelBatching.UniqueTextureConfiguration, LevelBatching.TextureUsers> batchableTextures = new Dictionary<LevelBatching.UniqueTextureConfiguration, LevelBatching.TextureUsers>();

			// Token: 0x04003329 RID: 13097
			public FilterMode filterMode;
		}

		/// <summary>
		/// StaticBatchingUtility.Combine requires input renderers are enabled and active in hierarchy,
		/// so we temporarily activate/enable them to keep this logic out of LevelObject/ResourceSpawnpoint.
		/// </summary>
		// Token: 0x02000957 RID: 2391
		private struct StaticBatchingInitialState
		{
			// Token: 0x0400332A RID: 13098
			public Transform parent;

			// Token: 0x0400332B RID: 13099
			public bool wasEnabled;

			// Token: 0x0400332C RID: 13100
			public bool wasActive;
		}
	}
}
