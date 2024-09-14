using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000293 RID: 659
	public struct AudioReference
	{
		// Token: 0x060013EE RID: 5102 RVA: 0x0004A405 File Offset: 0x00048605
		public AudioReference(string assetBundleName, string path)
		{
			this.assetBundleName = assetBundleName;
			this.path = path;
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x0004A415 File Offset: 0x00048615
		public bool IsNullOrEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.assetBundleName) || string.IsNullOrEmpty(this.path);
			}
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004A434 File Offset: 0x00048634
		public AudioClip LoadAudioClip(out float volumeMultiplier, out float pitchMultiplier)
		{
			volumeMultiplier = 1f;
			pitchMultiplier = 1f;
			if (this.IsNullOrEmpty)
			{
				return null;
			}
			MasterBundleConfig masterBundleConfig = Assets.findMasterBundleByName(this.assetBundleName, true);
			if (masterBundleConfig == null || masterBundleConfig.assetBundle == null)
			{
				UnturnedLog.warn("Unable to find master bundle '{0}' when loading audio reference '{1}'", new object[]
				{
					this.assetBundleName,
					this.path
				});
				return null;
			}
			string text = masterBundleConfig.formatAssetPath(this.path);
			if (!text.EndsWith(".asset"))
			{
				AudioClip audioClip = masterBundleConfig.assetBundle.LoadAsset<AudioClip>(text);
				if (audioClip == null)
				{
					UnturnedLog.warn("Failed to load audio clip '{0}' from master bundle '{1}'", new object[]
					{
						text,
						this.assetBundleName
					});
				}
				return audioClip;
			}
			OneShotAudioDefinition oneShotAudioDefinition = masterBundleConfig.assetBundle.LoadAsset<OneShotAudioDefinition>(text);
			if (oneShotAudioDefinition == null)
			{
				UnturnedLog.warn("Failed to load audio def '{0}' from master bundle '{1}'", new object[]
				{
					text,
					this.assetBundleName
				});
				return null;
			}
			volumeMultiplier = oneShotAudioDefinition.volumeMultiplier;
			pitchMultiplier = Random.Range(oneShotAudioDefinition.minPitch, oneShotAudioDefinition.maxPitch);
			return oneShotAudioDefinition.GetRandomClip();
		}

		// Token: 0x040006DE RID: 1758
		private string assetBundleName;

		// Token: 0x040006DF RID: 1759
		private string path;
	}
}
