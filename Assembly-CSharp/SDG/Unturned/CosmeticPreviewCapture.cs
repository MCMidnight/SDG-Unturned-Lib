using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200074A RID: 1866
	public class CosmeticPreviewCapture : MonoBehaviour
	{
		// Token: 0x06003D0B RID: 15627 RVA: 0x0012364F File Offset: 0x0012184F
		public void CaptureCosmetics()
		{
			base.StartCoroutine(this.CaptureAllCosmeticsCoroutine());
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00123660 File Offset: 0x00121860
		public void CaptureOutfit(Guid guid)
		{
			OutfitAsset outfitAsset = Assets.find(guid) as OutfitAsset;
			if (outfitAsset != null)
			{
				List<OutfitAsset> list = new List<OutfitAsset>();
				list.Add(outfitAsset);
				List<OutfitAsset> outfitAssets = list;
				base.StartCoroutine(this.CaptureOutfitsCoroutine(outfitAssets));
			}
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00123698 File Offset: 0x00121898
		public void CaptureAllOutfits()
		{
			List<OutfitAsset> list = new List<OutfitAsset>();
			Assets.find<OutfitAsset>(list);
			base.StartCoroutine(this.CaptureOutfitsCoroutine(list));
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x001236BF File Offset: 0x001218BF
		private IEnumerator CaptureAllCosmeticsCoroutine()
		{
			yield return this.RenderDefaultCharacter();
			string dirPath2048 = PathEx.Join(UnturnedPaths.RootDirectory, "Extras", "CosmeticPreviews_2048x2048");
			string dirPath2049 = PathEx.Join(UnturnedPaths.RootDirectory, "Extras", "CosmeticPreviews_400x400");
			string dirPath2050 = PathEx.Join(UnturnedPaths.RootDirectory, "Extras", "CosmeticPreviews_200x200");
			List<ItemAsset> list = new List<ItemAsset>();
			Assets.find<ItemAsset>(list);
			foreach (ItemAsset itemAsset in list)
			{
				if (itemAsset.isPro && (itemAsset.type == EItemType.SHIRT || itemAsset.type == EItemType.PANTS || itemAsset.type == EItemType.HAT || itemAsset.type == EItemType.BACKPACK || itemAsset.type == EItemType.VEST || itemAsset.type == EItemType.GLASSES || itemAsset.type == EItemType.MASK))
				{
					string text = Path.Combine(dirPath2048, itemAsset.GUID.ToString("N") + ".png");
					string filePath400 = Path.Combine(dirPath2049, itemAsset.GUID.ToString("N") + ".png");
					string filePath401 = Path.Combine(dirPath2050, itemAsset.GUID.ToString("N") + ".png");
					if (!File.Exists(text) || !File.Exists(filePath400) || !File.Exists(filePath401))
					{
						this.ResetOutfit();
						this.ApplyItemToOutfit(itemAsset);
						this.clothes.apply();
						Camera itemCamera = this.GetCamera(itemAsset);
						itemCamera = this.GetCamera(itemAsset);
						GameObject clothingGameObject = this.GetClothingGameObject(itemAsset);
						if (clothingGameObject != null)
						{
							Bounds worldBounds = this.GetWorldBounds(clothingGameObject);
							this.FitCameraToBounds(itemCamera, worldBounds);
						}
						yield return this.Render(itemCamera, this.targetTexture4096, this.downsampleTexture2048, this.exportTexture2048, text);
						yield return this.Render(itemCamera, this.targetTexture800, this.downsampleTexture400, this.exportTexture400, filePath400);
						yield return this.Render(itemCamera, this.targetTexture400, this.downsampleTexture200, this.exportTexture200, filePath401);
						filePath400 = null;
						filePath401 = null;
						itemCamera = null;
					}
				}
			}
			List<ItemAsset>.Enumerator enumerator = default(List<ItemAsset>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x001236CE File Offset: 0x001218CE
		private IEnumerator CaptureOutfitsCoroutine(List<OutfitAsset> outfitAssets)
		{
			yield return this.RenderDefaultCharacter();
			string dirPath2048 = PathEx.Join(UnturnedPaths.RootDirectory, "Extras", "OutfitPreviews_2048x2048");
			string dirPath2049 = PathEx.Join(UnturnedPaths.RootDirectory, "Extras", "OutfitPreviews_400x400");
			string dirPath2050 = PathEx.Join(UnturnedPaths.RootDirectory, "Extras", "OutfitPreviews_200x200");
			foreach (OutfitAsset outfitAsset in outfitAssets)
			{
				this.ResetOutfit();
				foreach (AssetReference<ItemAsset> assetReference in outfitAsset.itemAssets)
				{
					ItemAsset itemAsset = assetReference.Find();
					if (itemAsset != null)
					{
						this.ApplyItemToOutfit(itemAsset);
					}
				}
				this.clothes.apply();
				Bounds worldBounds = new Bounds(base.transform.position + new Vector3(0f, 0.95f, 0f), new Vector3(0.1f, 1.9f, 0.1f));
				if (this.clothes.hatModel != null)
				{
					worldBounds.Encapsulate(this.GetWorldBounds(this.clothes.hatModel.gameObject));
				}
				if (this.clothes.backpackModel != null)
				{
					worldBounds.Encapsulate(this.GetWorldBounds(this.clothes.backpackModel.gameObject));
				}
				if (this.clothes.vestModel != null)
				{
					worldBounds.Encapsulate(this.GetWorldBounds(this.clothes.vestModel.gameObject));
				}
				if (this.clothes.glassesModel != null)
				{
					worldBounds.Encapsulate(this.GetWorldBounds(this.clothes.glassesModel.gameObject));
				}
				if (this.clothes.maskModel != null)
				{
					worldBounds.Encapsulate(this.GetWorldBounds(this.clothes.maskModel.gameObject));
				}
				worldBounds.Expand(-0.2f);
				this.FitCameraToBounds(this.outfitCamera, worldBounds);
				string exportFilePath = Path.Combine(dirPath2048, outfitAsset.GUID.ToString("N") + ".png");
				string filePath400 = Path.Combine(dirPath2049, outfitAsset.GUID.ToString("N") + ".png");
				string filePath401 = Path.Combine(dirPath2050, outfitAsset.GUID.ToString("N") + ".png");
				yield return this.Render(this.outfitCamera, this.targetTexture4096, this.downsampleTexture2048, this.exportTexture2048, exportFilePath);
				yield return this.Render(this.outfitCamera, this.targetTexture800, this.downsampleTexture400, this.exportTexture400, filePath400);
				yield return this.Render(this.outfitCamera, this.targetTexture400, this.downsampleTexture200, this.exportTexture200, filePath401);
				filePath400 = null;
				filePath401 = null;
			}
			List<OutfitAsset>.Enumerator enumerator = default(List<OutfitAsset>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x001236E4 File Offset: 0x001218E4
		private void ResetOutfit()
		{
			this.clothes.shirtGuid = Guid.Empty;
			this.clothes.pantsGuid = Guid.Empty;
			this.clothes.hatGuid = Guid.Empty;
			this.clothes.backpackGuid = Guid.Empty;
			this.clothes.vestGuid = Guid.Empty;
			this.clothes.glassesGuid = Guid.Empty;
			this.clothes.maskGuid = Guid.Empty;
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x00123764 File Offset: 0x00121964
		private void ApplyItemToOutfit(ItemAsset itemAsset)
		{
			switch (itemAsset.type)
			{
			case EItemType.HAT:
				this.clothes.hatGuid = itemAsset.GUID;
				return;
			case EItemType.PANTS:
				this.clothes.pantsGuid = itemAsset.GUID;
				return;
			case EItemType.SHIRT:
				this.clothes.shirtGuid = itemAsset.GUID;
				return;
			case EItemType.MASK:
				this.clothes.maskGuid = itemAsset.GUID;
				return;
			case EItemType.BACKPACK:
				this.clothes.backpackGuid = itemAsset.GUID;
				return;
			case EItemType.VEST:
				this.clothes.vestGuid = itemAsset.GUID;
				return;
			case EItemType.GLASSES:
				this.clothes.glassesGuid = itemAsset.GUID;
				return;
			default:
				return;
			}
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x00123818 File Offset: 0x00121A18
		private GameObject GetClothingGameObject(ItemAsset itemAsset)
		{
			switch (itemAsset.type)
			{
			case EItemType.HAT:
			{
				Transform hatModel = this.clothes.hatModel;
				if (hatModel == null)
				{
					return null;
				}
				return hatModel.gameObject;
			}
			case EItemType.MASK:
			{
				Transform maskModel = this.clothes.maskModel;
				if (maskModel == null)
				{
					return null;
				}
				return maskModel.gameObject;
			}
			case EItemType.BACKPACK:
			{
				Transform backpackModel = this.clothes.backpackModel;
				if (backpackModel == null)
				{
					return null;
				}
				return backpackModel.gameObject;
			}
			case EItemType.VEST:
			{
				Transform vestModel = this.clothes.vestModel;
				if (vestModel == null)
				{
					return null;
				}
				return vestModel.gameObject;
			}
			case EItemType.GLASSES:
			{
				Transform glassesModel = this.clothes.glassesModel;
				if (glassesModel == null)
				{
					return null;
				}
				return glassesModel.gameObject;
			}
			}
			return null;
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x001238C4 File Offset: 0x00121AC4
		private Camera GetCamera(ItemAsset itemAsset)
		{
			switch (itemAsset.type)
			{
			case EItemType.HAT:
				return this.hatCamera;
			case EItemType.PANTS:
				return this.pantsCamera;
			case EItemType.SHIRT:
				return this.shirtCamera;
			case EItemType.MASK:
				return this.hatCamera;
			case EItemType.BACKPACK:
				return this.backpackCamera;
			case EItemType.VEST:
				return this.vestCamera;
			case EItemType.GLASSES:
				return this.hatCamera;
			default:
				return null;
			}
		}

		/// <summary>
		/// Render character with hair and skin otherwise it might be cyan.
		/// (public issue #3615)
		/// </summary>
		// Token: 0x06003D14 RID: 15636 RVA: 0x0012392E File Offset: 0x00121B2E
		private IEnumerator RenderDefaultCharacter()
		{
			yield return new WaitForEndOfFrame();
			this.clothes.hair = 1;
			this.clothes.apply();
			this.outfitCamera.targetTexture = this.targetTexture400;
			this.outfitCamera.Render();
			this.outfitCamera.targetTexture = null;
			this.clothes.hair = 0;
			yield return new WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x0012393D File Offset: 0x00121B3D
		private IEnumerator Render(Camera cameraComponent, RenderTexture targetTexture, RenderTexture downsampleTexture, Texture2D exportTexture, string exportFilePath)
		{
			yield return new WaitForEndOfFrame();
			cameraComponent.targetTexture = targetTexture;
			cameraComponent.Render();
			cameraComponent.targetTexture = null;
			Graphics.Blit(targetTexture, downsampleTexture);
			RenderTexture.active = downsampleTexture;
			exportTexture.ReadPixels(new Rect(0f, 0f, (float)downsampleTexture.width, (float)downsampleTexture.height), 0, 0);
			RenderTexture.active = null;
			byte[] array = ImageConversion.EncodeToPNG(exportTexture);
			File.WriteAllBytes(exportFilePath, array);
			yield break;
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x0012396C File Offset: 0x00121B6C
		private void OnEnable()
		{
			this.clothes = base.GetComponent<HumanClothes>();
			this.clothes.skin = new Color32(210, 210, 210, byte.MaxValue);
			this.clothes.color = new Color32(175, 175, 175, byte.MaxValue);
			this.targetTexture4096 = RenderTexture.GetTemporary(4096, 4096, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.targetTexture4096.filterMode = FilterMode.Bilinear;
			this.targetTexture800 = RenderTexture.GetTemporary(800, 800, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.targetTexture800.filterMode = FilterMode.Bilinear;
			this.targetTexture400 = RenderTexture.GetTemporary(400, 400, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.targetTexture400.filterMode = FilterMode.Bilinear;
			this.downsampleTexture2048 = RenderTexture.GetTemporary(2048, 2048, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.downsampleTexture400 = RenderTexture.GetTemporary(400, 400, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.downsampleTexture200 = RenderTexture.GetTemporary(200, 200, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.exportTexture2048 = new Texture2D(2048, 2048, TextureFormat.ARGB32, false, false);
			this.exportTexture400 = new Texture2D(400, 400, TextureFormat.ARGB32, false, false);
			this.exportTexture200 = new Texture2D(200, 200, TextureFormat.ARGB32, false, false);
			base.GetComponent<Animation>()["Idle_Stand"].speed = 0f;
			base.GetComponent<Animation>()["Idle_Stand"].normalizedTime = 0.2f;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00123B10 File Offset: 0x00121D10
		private void OnDisable()
		{
			RenderTexture.ReleaseTemporary(this.targetTexture4096);
			RenderTexture.ReleaseTemporary(this.targetTexture800);
			RenderTexture.ReleaseTemporary(this.targetTexture400);
			RenderTexture.ReleaseTemporary(this.downsampleTexture2048);
			RenderTexture.ReleaseTemporary(this.downsampleTexture400);
			RenderTexture.ReleaseTemporary(this.downsampleTexture200);
			Object.Destroy(this.exportTexture2048);
			Object.Destroy(this.exportTexture400);
			Object.Destroy(this.exportTexture200);
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00123B80 File Offset: 0x00121D80
		private Bounds GetWorldBounds(GameObject parent)
		{
			Bounds result = default(Bounds);
			bool flag = false;
			ParticleSystem.Particle[] array = new ParticleSystem.Particle[1024];
			parent.GetComponentsInChildren<Renderer>(this.renderers);
			foreach (Renderer renderer in this.renderers)
			{
				ParticleSystemRenderer particleSystemRenderer = renderer as ParticleSystemRenderer;
				if (particleSystemRenderer != null)
				{
					int particles = particleSystemRenderer.GetComponent<ParticleSystem>().GetParticles(array);
					for (int i = 0; i < particles; i++)
					{
						ParticleSystem.Particle particle = array[i];
						Vector3 center = renderer.transform.TransformPoint(particle.position);
						if (flag)
						{
							result.Encapsulate(new Bounds(center, new Vector3(0.1f, 0.1f, 0.1f)));
						}
						else
						{
							result = new Bounds(center, Vector3.zero);
							flag = true;
						}
					}
				}
				else if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
				{
					if (flag)
					{
						result.Encapsulate(renderer.bounds);
					}
					else
					{
						result = renderer.bounds;
						flag = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x00123CA8 File Offset: 0x00121EA8
		private void FitCameraToBounds(Camera cameraComponent, Bounds worldBounds)
		{
			float magnitude = worldBounds.extents.magnitude;
			float f = cameraComponent.fieldOfView * 0.5f * 0.017453292f;
			float num = magnitude / Mathf.Sin(f);
			num = Mathf.Max(0.55f, num);
			float d = num + cameraComponent.nearClipPlane;
			cameraComponent.transform.position = worldBounds.center - cameraComponent.transform.forward * d;
		}

		// Token: 0x04002657 RID: 9815
		public Camera shirtCamera;

		// Token: 0x04002658 RID: 9816
		public Camera pantsCamera;

		// Token: 0x04002659 RID: 9817
		public Camera backpackCamera;

		// Token: 0x0400265A RID: 9818
		public Camera vestCamera;

		// Token: 0x0400265B RID: 9819
		public Camera hatCamera;

		// Token: 0x0400265C RID: 9820
		public Camera outfitCamera;

		// Token: 0x0400265D RID: 9821
		private List<Renderer> renderers = new List<Renderer>();

		// Token: 0x0400265E RID: 9822
		private HumanClothes clothes;

		// Token: 0x0400265F RID: 9823
		private RenderTexture targetTexture4096;

		// Token: 0x04002660 RID: 9824
		private RenderTexture targetTexture800;

		// Token: 0x04002661 RID: 9825
		private RenderTexture targetTexture400;

		// Token: 0x04002662 RID: 9826
		private RenderTexture downsampleTexture2048;

		// Token: 0x04002663 RID: 9827
		private RenderTexture downsampleTexture400;

		// Token: 0x04002664 RID: 9828
		private RenderTexture downsampleTexture200;

		// Token: 0x04002665 RID: 9829
		private Texture2D exportTexture2048;

		// Token: 0x04002666 RID: 9830
		private Texture2D exportTexture400;

		// Token: 0x04002667 RID: 9831
		private Texture2D exportTexture200;
	}
}
