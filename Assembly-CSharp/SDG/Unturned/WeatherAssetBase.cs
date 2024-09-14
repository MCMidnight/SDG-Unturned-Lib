using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000388 RID: 904
	public class WeatherAssetBase : Asset
	{
		/// <summary>
		/// Seconds between weather event starting and reaching full intensity.
		/// </summary>
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x0006455D File Offset: 0x0006275D
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x00064565 File Offset: 0x00062765
		public float fadeInDuration { get; protected set; }

		/// <summary>
		/// Seconds between weather event ending and reaching zero intensity.
		/// </summary>
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0006456E File Offset: 0x0006276E
		// (set) Token: 0x06001C17 RID: 7191 RVA: 0x00064576 File Offset: 0x00062776
		public float fadeOutDuration { get; protected set; }

		/// <summary>
		/// Sound clip to play. Volume matches the intensity.
		/// </summary>
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0006457F File Offset: 0x0006277F
		// (set) Token: 0x06001C19 RID: 7193 RVA: 0x00064587 File Offset: 0x00062787
		public MasterBundleReference<AudioClip> ambientAudio { get; protected set; }

		/// <summary>
		/// Component to spawn for additional weather logic.
		/// </summary>
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x00064590 File Offset: 0x00062790
		// (set) Token: 0x06001C1B RID: 7195 RVA: 0x00064598 File Offset: 0x00062798
		public Type componentType { get; protected set; }

		/// <summary>
		/// If per-volume mask AND is non zero the weather will blend in.
		/// </summary>
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x000645A1 File Offset: 0x000627A1
		// (set) Token: 0x06001C1D RID: 7197 RVA: 0x000645A9 File Offset: 0x000627A9
		public uint volumeMask { get; protected set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x000645B2 File Offset: 0x000627B2
		// (set) Token: 0x06001C1F RID: 7199 RVA: 0x000645BA File Offset: 0x000627BA
		public bool hasLightning { get; protected set; }

		// Token: 0x06001C20 RID: 7200 RVA: 0x000645C4 File Offset: 0x000627C4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.fadeInDuration = data.ParseFloat("Fade_In_Duration", 0f);
			this.fadeOutDuration = data.ParseFloat("Fade_Out_Duration", 0f);
			this.ambientAudio = data.ParseStruct<MasterBundleReference<AudioClip>>("Ambient_Audio_Clip", default(MasterBundleReference<AudioClip>));
			this.componentType = data.ParseType("Component_Type", null);
			if (this.componentType == null)
			{
				this.componentType = typeof(WeatherComponentBase);
			}
			if (data.ContainsKey("Volume_Mask"))
			{
				this.volumeMask = data.ParseUInt32("Volume_Mask", 0U);
			}
			else
			{
				this.volumeMask = uint.MaxValue;
			}
			this.hasLightning = data.ParseBool("Has_Lightning", false);
			if (this.hasLightning)
			{
				this.minLightningInterval = Mathf.Max(5f, data.ParseFloat("Min_Lightning_Interval", 0f));
				this.maxLightningInterval = Mathf.Max(5f, data.ParseFloat("Max_Lightning_Interval", 0f));
				if (data.ContainsKey("Lightning_Target_Radius"))
				{
					this.lightningTargetRadius = Mathf.Max(0f, data.ParseFloat("Lightning_Target_Radius", 0f));
					return;
				}
				this.lightningTargetRadius = 500f;
			}
		}

		// Token: 0x04000D36 RID: 3382
		public static readonly AssetReference<WeatherAssetBase> DEFAULT_SNOW = new AssetReference<WeatherAssetBase>("903577da2ecd4f5784b2f7aed8c300c1");

		// Token: 0x04000D37 RID: 3383
		public static readonly AssetReference<WeatherAssetBase> DEFAULT_RAIN = new AssetReference<WeatherAssetBase>("d73923f4416c43dfa5bc8b6234cf0257");

		// Token: 0x04000D3E RID: 3390
		public float minLightningInterval;

		// Token: 0x04000D3F RID: 3391
		public float maxLightningInterval;

		// Token: 0x04000D40 RID: 3392
		public float lightningTargetRadius;
	}
}
