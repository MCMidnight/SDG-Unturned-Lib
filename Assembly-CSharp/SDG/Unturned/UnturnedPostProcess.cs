using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SDG.Unturned
{
	/// <summary>
	/// Manages global post-process volumes.
	/// </summary>
	// Token: 0x020005A1 RID: 1441
	public class UnturnedPostProcess : MonoBehaviour
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x000C8FF9 File Offset: 0x000C71F9
		// (set) Token: 0x06002E1E RID: 11806 RVA: 0x000C9001 File Offset: 0x000C7201
		public bool DisableAntiAliasingForScreenshot
		{
			get
			{
				return this._disableAntiAliasingForScreenshot;
			}
			set
			{
				if (this._disableAntiAliasingForScreenshot != value)
				{
					this._disableAntiAliasingForScreenshot = value;
					if (this.basePostProcess != null)
					{
						this.applyAntiAliasing(this.basePostProcess);
					}
				}
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x000C902D File Offset: 0x000C722D
		// (set) Token: 0x06002E20 RID: 11808 RVA: 0x000C9034 File Offset: 0x000C7234
		public static UnturnedPostProcess instance { get; private set; }

		// Token: 0x06002E21 RID: 11809 RVA: 0x000C903C File Offset: 0x000C723C
		public void setBaseCamera(Camera baseCamera)
		{
			this.basePostProcess = baseCamera.GetComponent<PostProcessLayer>();
			this.basePostProcess.fog.enabled = true;
			this.basePostProcess.fog.excludeSkybox = true;
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000C906C File Offset: 0x000C726C
		public void setOverlayCamera(Camera overlayCamera)
		{
			this.viewmodelPostProcess = overlayCamera.GetComponent<PostProcessLayer>();
			this.viewmodelPostProcess.fog.enabled = false;
			this.viewmodelPostProcess.fog.excludeSkybox = true;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000C909C File Offset: 0x000C729C
		public void setScopeCamera(Camera scopeCamera)
		{
			PostProcessLayer component = scopeCamera.GetComponent<PostProcessLayer>();
			component.fog.enabled = true;
			component.fog.excludeSkybox = true;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000C90BC File Offset: 0x000C72BC
		public void setIsHallucinating(bool isHallucinating)
		{
			this.baseProfile.colorGrading.active = isHallucinating;
			this.baseProfile.colorGrading.hueShift.Override(Random.Range(-180f, 180f));
			this.viewmodelProfile.colorGrading.active = isHallucinating;
			this.viewmodelProfile.colorGrading.hueShift.Override(Random.Range(-180f, 180f));
			this.baseProfile.vignette.active = isHallucinating;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000C9144 File Offset: 0x000C7344
		private void tickHallucinationColorGrading(UnturnedPostProcess.PostProcessProfileWrapper profile, float deltaTime)
		{
			float num = 2.5f;
			float num2 = profile.colorGrading.hueShift.value;
			num2 += deltaTime * num;
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			profile.colorGrading.hueShift.Override(num2);
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000C9190 File Offset: 0x000C7390
		public void tickIsHallucinating(float deltaTime, float hallucinationTimer)
		{
			this.tickHallucinationColorGrading(this.baseProfile, deltaTime);
			this.tickHallucinationColorGrading(this.viewmodelProfile, deltaTime);
			float num = 0.333f;
			float num2 = 4f;
			this.baseProfile.vignette.intensity.Override(Mathf.Abs(Mathf.Sin(hallucinationTimer / num2)) * num);
		}

		/// <summary>
		/// Callback when in-game graphic settings change.
		/// </summary>
		// Token: 0x06002E27 RID: 11815 RVA: 0x000C91E7 File Offset: 0x000C73E7
		public void applyUserSettings()
		{
			if (this.basePostProcess != null)
			{
				this.applyAntiAliasing(this.basePostProcess);
			}
			this.syncAmbientOcclusion();
			this.syncBloom();
			this.syncChromaticAberration();
			this.syncFilmGrain();
			this.syncScreenSpaceReflections();
		}

		/// <summary>
		/// Callback when player changes perspective.
		/// </summary>
		// Token: 0x06002E28 RID: 11816 RVA: 0x000C9221 File Offset: 0x000C7421
		public void notifyPerspectiveChanged()
		{
			this.syncBloom();
			this.syncChromaticAberration();
			this.syncFilmGrain();
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000C9235 File Offset: 0x000C7435
		private void syncAmbientOcclusion()
		{
			this.baseProfile.ambientOcclusion.active = GraphicsSettings.isAmbientOcclusionEnabled;
			this.viewmodelProfile.ambientOcclusion.active = GraphicsSettings.isAmbientOcclusionEnabled;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000C9264 File Offset: 0x000C7464
		private void syncBloom()
		{
			if (this.hasActiveOverlay)
			{
				this.baseProfile.bloom.active = false;
				this.viewmodelProfile.bloom.active = GraphicsSettings.bloom;
				return;
			}
			this.baseProfile.bloom.active = GraphicsSettings.bloom;
			this.viewmodelProfile.bloom.active = false;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000C92C8 File Offset: 0x000C74C8
		private void syncChromaticAberration()
		{
			if (this.hasActiveOverlay)
			{
				this.baseProfile.chromaticAberration.active = false;
				this.viewmodelProfile.chromaticAberration.active = GraphicsSettings.chromaticAberration;
				return;
			}
			this.baseProfile.chromaticAberration.active = GraphicsSettings.chromaticAberration;
			this.viewmodelProfile.chromaticAberration.active = false;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000C932C File Offset: 0x000C752C
		private void syncFilmGrain()
		{
			if (this.hasActiveOverlay)
			{
				this.baseProfile.filmGrain.active = false;
				this.viewmodelProfile.filmGrain.active = GraphicsSettings.filmGrain;
				return;
			}
			this.baseProfile.filmGrain.active = GraphicsSettings.filmGrain;
			this.viewmodelProfile.filmGrain.active = false;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000C9390 File Offset: 0x000C7590
		private void syncScreenSpaceReflections()
		{
			bool flag = GraphicsSettings.reflectionQuality != EGraphicQuality.OFF && GraphicsSettings.renderMode == ERenderMode.DEFERRED;
			this.baseProfile.screenSpaceReflections.active = flag;
			if (!flag)
			{
				return;
			}
			ScreenSpaceReflectionPreset screenSpaceReflectionPreset;
			switch (GraphicsSettings.reflectionQuality)
			{
			case EGraphicQuality.LOW:
				screenSpaceReflectionPreset = 1;
				break;
			case EGraphicQuality.MEDIUM:
				screenSpaceReflectionPreset = 2;
				break;
			case EGraphicQuality.HIGH:
				screenSpaceReflectionPreset = 3;
				break;
			case EGraphicQuality.ULTRA:
				screenSpaceReflectionPreset = 5;
				break;
			default:
				screenSpaceReflectionPreset = 1;
				break;
			}
			this.baseProfile.screenSpaceReflections.preset.Override(screenSpaceReflectionPreset);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000C940C File Offset: 0x000C760C
		private void applyAntiAliasing(PostProcessLayer layer)
		{
			if (this._disableAntiAliasingForScreenshot)
			{
				layer.antialiasingMode = 0;
				return;
			}
			switch (GraphicsSettings.antiAliasingType)
			{
			case EAntiAliasingType.OFF:
				layer.antialiasingMode = 0;
				return;
			case EAntiAliasingType.FXAA:
				layer.antialiasingMode = 1;
				return;
			case EAntiAliasingType.TAA:
				layer.antialiasingMode = 3;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000C945C File Offset: 0x000C765C
		private UnturnedPostProcess.PostProcessProfileWrapper createGlobalProfile(string name, int layer)
		{
			PostProcessVolume postProcessVolume = new GameObject(name)
			{
				transform = 
				{
					parent = base.transform
				},
				layer = layer
			}.AddComponent<PostProcessVolume>();
			postProcessVolume.isGlobal = true;
			postProcessVolume.priority = 1f;
			return new UnturnedPostProcess.PostProcessProfileWrapper(postProcessVolume.profile, layer == 11);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000C94AD File Offset: 0x000C76AD
		public void initialize()
		{
			Object.Destroy(base.gameObject);
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002E31 RID: 11825 RVA: 0x000C94BA File Offset: 0x000C76BA
		private bool hasActiveOverlay
		{
			get
			{
				return this.viewmodelPostProcess != null && this.viewmodelPostProcess.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x040018E1 RID: 6369
		public const int BASE_LAYER = 8;

		// Token: 0x040018E2 RID: 6370
		public const int VIEWMODEL_LAYER = 11;

		// Token: 0x040018E3 RID: 6371
		private bool _disableAntiAliasingForScreenshot;

		// Token: 0x040018E5 RID: 6373
		public Texture dirtTexture;

		// Token: 0x040018E6 RID: 6374
		private UnturnedPostProcess.PostProcessProfileWrapper baseProfile;

		// Token: 0x040018E7 RID: 6375
		private UnturnedPostProcess.PostProcessProfileWrapper viewmodelProfile;

		// Token: 0x040018E8 RID: 6376
		private PostProcessLayer basePostProcess;

		// Token: 0x040018E9 RID: 6377
		private PostProcessLayer viewmodelPostProcess;

		// Token: 0x02000985 RID: 2437
		private class PostProcessProfileWrapper
		{
			// Token: 0x06004B8A RID: 19338 RVA: 0x001B4DCC File Offset: 0x001B2FCC
			public PostProcessProfileWrapper(PostProcessProfile profile, bool viewmodel)
			{
				this.profile = profile;
				this.ambientOcclusion = profile.AddSettings<AmbientOcclusion>();
				this.ambientOcclusion.active = false;
				this.ambientOcclusion.intensity.Override(0.25f);
				this.bloom = profile.AddSettings<Bloom>();
				this.bloom.active = false;
				this.bloom.intensity.Override(1f);
				this.bloom.softKnee.Override(0f);
				this.colorGrading = profile.AddSettings<ColorGrading>();
				this.colorGrading.active = false;
				this.chromaticAberration = profile.AddSettings<ChromaticAberration>();
				this.chromaticAberration.active = false;
				this.filmGrain = profile.AddSettings<Grain>();
				this.filmGrain.active = false;
				this.filmGrain.intensity.Override(0.25f);
				this.screenSpaceReflections = profile.AddSettings<ScreenSpaceReflections>();
				this.screenSpaceReflections.active = false;
				this.vignette = profile.AddSettings<Vignette>();
				this.vignette.active = false;
				this.vignette.rounded.Override(true);
				if (!viewmodel)
				{
					profile.AddSettings<SkyFog>();
				}
			}

			// Token: 0x0400339D RID: 13213
			public PostProcessProfile profile;

			// Token: 0x0400339E RID: 13214
			public AmbientOcclusion ambientOcclusion;

			// Token: 0x0400339F RID: 13215
			public Bloom bloom;

			// Token: 0x040033A0 RID: 13216
			public ChromaticAberration chromaticAberration;

			// Token: 0x040033A1 RID: 13217
			public ColorGrading colorGrading;

			// Token: 0x040033A2 RID: 13218
			public Grain filmGrain;

			// Token: 0x040033A3 RID: 13219
			public ScreenSpaceReflections screenSpaceReflections;

			// Token: 0x040033A4 RID: 13220
			public Vignette vignette;
		}
	}
}
