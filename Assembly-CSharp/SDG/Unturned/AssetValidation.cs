using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000292 RID: 658
	public static class AssetValidation
	{
		// Token: 0x060013E4 RID: 5092 RVA: 0x00049B08 File Offset: 0x00047D08
		public static void ValidateLayersEqual(Asset owningAsset, GameObject gameObject, int expectedLayer)
		{
			int layer = gameObject.layer;
			if (layer != expectedLayer)
			{
				Assets.reportError(owningAsset, "expected '{0}' to have layer {1}, but it actually has layer {2}", gameObject.name, expectedLayer, layer);
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00049B40 File Offset: 0x00047D40
		public static void ValidateLayersEqualRecursive(Asset owningAsset, GameObject gameObject, int expectedLayer)
		{
			AssetValidation.ValidateLayersEqual(owningAsset, gameObject, expectedLayer);
			foreach (object obj in gameObject.transform)
			{
				Transform transform = (Transform)obj;
				AssetValidation.ValidateLayersEqualRecursive(owningAsset, transform.gameObject, expectedLayer);
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00049BA8 File Offset: 0x00047DA8
		public static void ValidateClothComponents(Asset owningAsset, GameObject gameObject)
		{
			AssetValidation.clothComponents.Clear();
			gameObject.GetComponentsInChildren<Cloth>(AssetValidation.clothComponents);
			foreach (Cloth cloth in AssetValidation.clothComponents)
			{
				if (cloth.capsuleColliders.Length != 0 || cloth.sphereColliders.Length != 0)
				{
					Assets.reportError(owningAsset, gameObject.name + " cloth component \"" + cloth.name + "\" has colliders which is problematic because unfortunately the game does not yet have a way for weapons to ignore them");
				}
			}
		}

		/// <summary>
		/// Relatively efficiently find mesh components, and log an error if their mesh is missing, among other checks.
		/// </summary>
		// Token: 0x060013E7 RID: 5095 RVA: 0x00049C3C File Offset: 0x00047E3C
		public static void searchGameObjectForErrors(Asset owningAsset, GameObject gameObject)
		{
			if (gameObject == null)
			{
				throw new ArgumentNullException("gameObject");
			}
			if (!Assets.shouldValidateAssets)
			{
				return;
			}
			AssetValidation.staticMeshComponents.Clear();
			gameObject.GetComponentsInChildren<MeshFilter>(AssetValidation.staticMeshComponents);
			foreach (MeshFilter meshFilter in AssetValidation.staticMeshComponents)
			{
				AssetValidation.internalValidateMesh(owningAsset, gameObject, meshFilter, meshFilter.sharedMesh, 50000);
			}
			AssetValidation.meshColliderComponents.Clear();
			gameObject.GetComponentsInChildren<MeshCollider>(AssetValidation.meshColliderComponents);
			foreach (MeshCollider meshCollider in AssetValidation.meshColliderComponents)
			{
				AssetValidation.internalValidateMesh(owningAsset, gameObject, meshCollider, meshCollider.sharedMesh, 25000);
			}
			AssetValidation.allRenderers.Clear();
			gameObject.GetComponentsInChildren<Renderer>(AssetValidation.allRenderers);
			foreach (Renderer renderer in AssetValidation.allRenderers)
			{
				if (renderer.motionVectorGenerationMode == MotionVectorGenerationMode.ForceNoMotion)
				{
					Assets.reportError(owningAsset, "{0} Renderer \"{1}\" motion vectors disabled could be a problem for TAA", gameObject.name, renderer.name);
				}
			}
			AssetValidation.meshRenderers.Clear();
			gameObject.GetComponentsInChildren<MeshRenderer>(AssetValidation.meshRenderers);
			foreach (MeshRenderer meshRenderer in AssetValidation.meshRenderers)
			{
				if (meshRenderer.GetComponent<MeshFilter>() == null)
				{
					if (meshRenderer.GetComponent<TextMeshPro>() == null)
					{
						Assets.reportError(owningAsset, "{0} missing MeshFilter or TextMesh for MeshRenderer '{1}'", gameObject.name, meshRenderer.name);
					}
				}
				else if (meshRenderer.name != "DepthMask")
				{
					AssetValidation.internalValidateRendererMaterials(owningAsset, gameObject, meshRenderer);
				}
			}
			AssetValidation.skinnedMeshRenderers.Clear();
			gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(AssetValidation.skinnedMeshRenderers);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in AssetValidation.skinnedMeshRenderers)
			{
				AssetValidation.internalValidateMesh(owningAsset, gameObject, skinnedMeshRenderer, skinnedMeshRenderer.sharedMesh, 50000);
				AssetValidation.internalValidateRendererMaterials(owningAsset, gameObject, skinnedMeshRenderer);
			}
			AssetValidation.lodGroupComponents.Clear();
			gameObject.GetComponentsInChildren<LODGroup>(true, AssetValidation.lodGroupComponents);
			foreach (LODGroup component in AssetValidation.lodGroupComponents)
			{
				AssetValidation.InternalValidateLodGroupComponent(owningAsset, component);
			}
			AssetValidation.InternalValidateRendererMultiLodRegistration(owningAsset);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00049F1C File Offset: 0x0004811C
		private static void internalValidateMesh(Asset owningAsset, GameObject gameObject, Component component, Mesh sharedMesh, int maximumVertexCount)
		{
			if (!(sharedMesh == null))
			{
				if (sharedMesh.vertexCount > maximumVertexCount)
				{
					Assets.reportError(owningAsset, "{0} mesh for {1} '{2}' has {3} vertices (ideal maximum of {4}) and might have room for optimization.", new object[]
					{
						gameObject.name,
						component.GetType().Name,
						component.name,
						sharedMesh.vertexCount,
						maximumVertexCount
					});
				}
				return;
			}
			if (component.GetComponent<TextMeshPro>() != null)
			{
				return;
			}
			Assets.reportError(owningAsset, "{0} missing mesh for {1} '{2}'", gameObject.name, component.GetType().Name, component.name);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00049FB8 File Offset: 0x000481B8
		private static void internalValidateRendererMaterials(Asset owningAsset, GameObject gameObject, Renderer component)
		{
			int num = component.sharedMaterials.Length;
			if (num == 0)
			{
				Assets.reportError(owningAsset, "{0} missing materials for Renderer '{1}'", gameObject.name, component.name);
				return;
			}
			if (num > 4)
			{
				Assets.reportError(owningAsset, "{0} Renderer '{1}' has {2} separate materials (ideal maximum of {3}) which should be optimized to reduce draw calls.", new object[]
				{
					gameObject.name,
					component.name,
					num,
					4
				});
			}
			for (int i = 0; i < num; i++)
			{
				Material material = component.sharedMaterials[i];
				if (material == null)
				{
					Assets.reportError(owningAsset, "{0} missing material[{1}] for Renderer '{2}'", gameObject.name, i, component.name);
				}
				else
				{
					AssetValidation.internalValidateMaterialTextures(owningAsset, gameObject, component, material);
				}
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0004A068 File Offset: 0x00048268
		private static void internalValidateMaterialTextures(Asset owningAsset, GameObject gameObject, Renderer component, Material sharedMaterial)
		{
			AssetValidation.texturePropertyNameIDs.Clear();
			sharedMaterial.GetTexturePropertyNameIDs(AssetValidation.texturePropertyNameIDs);
			foreach (int nameID in AssetValidation.texturePropertyNameIDs)
			{
				Texture texture = sharedMaterial.GetTexture(nameID);
				if (texture != null && !owningAsset.ignoreTextureReadable && texture.isReadable)
				{
					Assets.reportError(owningAsset, "{0} texture '{1}' referenced by material '{2}' used by Renderer '{3}' can save memory by disabling read/write.", new object[]
					{
						gameObject.name,
						texture.name,
						sharedMaterial.name,
						component.name
					});
				}
				Texture2D texture2D = texture as Texture2D;
				if (texture2D != null && !owningAsset.ignoreNPOT && (!Mathf.IsPowerOfTwo(texture2D.width) || !Mathf.IsPowerOfTwo(texture2D.height)))
				{
					Assets.reportError(owningAsset, "{0} texture '{1}' referenced by material '{2}' used by Renderer '{3}' has NPOT dimensions ({4} x {5})", new object[]
					{
						gameObject.name,
						texture.name,
						sharedMaterial.name,
						component.name,
						texture2D.width,
						texture2D.height
					});
				}
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0004A1A8 File Offset: 0x000483A8
		private static void InternalValidateLodGroupComponent(Asset owningAsset, LODGroup component)
		{
			LOD[] lods = component.GetLODs();
			for (int i = 0; i < lods.Length; i++)
			{
				LOD lod = lods[i];
				if (lod.renderers.Length < 1)
				{
					Assets.reportError(owningAsset, "LOD group on \"{0}\" LOD level {1} is empty", component.GetSceneHierarchyPath(), i);
				}
				else
				{
					int num = 0;
					for (int j = 0; j < lod.renderers.Length; j++)
					{
						if (lod.renderers[j] == null)
						{
							num++;
						}
					}
					if (num > 0)
					{
						Assets.reportError(owningAsset, "LOD group on \"{0}\" LOD level {1} missing {2} renderer(s)", component.GetSceneHierarchyPath(), i, num);
					}
				}
			}
		}

		/// <summary>
		/// Unity warns about renderers registered with more than one LOD group, so we do our own validation as part of
		/// asset loading to make it easier to find these.
		/// </summary>
		// Token: 0x060013EC RID: 5100 RVA: 0x0004A248 File Offset: 0x00048448
		private static void InternalValidateRendererMultiLodRegistration(Asset owningAsset)
		{
			using (List<Renderer>.Enumerator enumerator = AssetValidation.allRenderers.GetEnumerator())
			{
				IL_101:
				while (enumerator.MoveNext())
				{
					Renderer renderer = enumerator.Current;
					LODGroup lodgroup = null;
					int num = 0;
					foreach (LODGroup lodgroup2 in AssetValidation.lodGroupComponents)
					{
						LOD[] lods = lodgroup2.GetLODs();
						for (int i = 0; i < lods.Length; i++)
						{
							Renderer[] renderers = lods[i].renderers;
							for (int j = 0; j < renderers.Length; j++)
							{
								if (renderers[j] == renderer)
								{
									if (lodgroup == null)
									{
										lodgroup = lodgroup2;
										num = i;
									}
									else if (lodgroup2 != lodgroup)
									{
										Assets.reportError(owningAsset, "renderer on \"{0}\" is registered with more than one LOD group, found in \"{1}\" LOD level {2} and \"{3}\" LOD level {4}", new object[]
										{
											renderer.GetSceneHierarchyPath(),
											lodgroup.GetSceneHierarchyPath(),
											num,
											lodgroup2.GetSceneHierarchyPath(),
											i
										});
										goto IL_101;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040006D6 RID: 1750
		private static List<int> texturePropertyNameIDs = new List<int>();

		// Token: 0x040006D7 RID: 1751
		private static List<MeshFilter> staticMeshComponents = new List<MeshFilter>();

		// Token: 0x040006D8 RID: 1752
		private static List<MeshCollider> meshColliderComponents = new List<MeshCollider>();

		// Token: 0x040006D9 RID: 1753
		private static List<Renderer> allRenderers = new List<Renderer>();

		// Token: 0x040006DA RID: 1754
		private static List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

		// Token: 0x040006DB RID: 1755
		private static List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x040006DC RID: 1756
		private static List<Cloth> clothComponents = new List<Cloth>();

		// Token: 0x040006DD RID: 1757
		private static List<LODGroup> lodGroupComponents = new List<LODGroup>();
	}
}
