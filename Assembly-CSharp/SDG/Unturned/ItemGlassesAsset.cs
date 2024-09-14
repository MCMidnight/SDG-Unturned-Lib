using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E6 RID: 742
	public class ItemGlassesAsset : ItemGearAsset
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x00051938 File Offset: 0x0004FB38
		public GameObject glasses
		{
			get
			{
				return this._glasses;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x00051940 File Offset: 0x0004FB40
		public ELightingVision vision
		{
			get
			{
				return this._vision;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x00051948 File Offset: 0x0004FB48
		// (set) Token: 0x0600160F RID: 5647 RVA: 0x00051950 File Offset: 0x0004FB50
		public PlayerSpotLightConfig lightConfig { get; protected set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x00051959 File Offset: 0x0004FB59
		// (set) Token: 0x06001611 RID: 5649 RVA: 0x00051961 File Offset: 0x0004FB61
		public bool isBlindfold { get; protected set; }

		/// <summary>
		/// If true, NVGs work in third-person, not just first-person.
		/// Defaults to false.
		/// </summary>
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x0005196A File Offset: 0x0004FB6A
		// (set) Token: 0x06001613 RID: 5651 RVA: 0x00051972 File Offset: 0x0004FB72
		public bool isNightvisionAllowedInThirdPerson { get; protected set; }

		// Token: 0x06001614 RID: 5652 RVA: 0x0005197B File Offset: 0x0004FB7B
		public override byte[] getState(EItemOrigin origin)
		{
			if (this.vision != ELightingVision.NONE)
			{
				return new byte[]
				{
					1
				};
			}
			return new byte[0];
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00051998 File Offset: 0x0004FB98
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (data.ContainsKey("Vision"))
			{
				this._vision = (ELightingVision)Enum.Parse(typeof(ELightingVision), data.GetString("Vision", null), true);
				if (this.vision == ELightingVision.HEADLAMP)
				{
					this.lightConfig = new PlayerSpotLightConfig(data);
				}
				else if (this.vision == ELightingVision.CIVILIAN)
				{
					this.nightvisionColor = data.LegacyParseColor32RGB("Nightvision_Color", LevelLighting.NIGHTVISION_CIVILIAN);
					this.nightvisionFogIntensity = data.ParseFloat("Nightvision_Fog_Intensity", 0.5f);
					this.nightvisionColor.g = this.nightvisionColor.r;
					this.nightvisionColor.b = this.nightvisionColor.r;
				}
				else if (this.vision == ELightingVision.MILITARY)
				{
					this.nightvisionColor = data.LegacyParseColor32RGB("Nightvision_Color", LevelLighting.NIGHTVISION_MILITARY);
					this.nightvisionFogIntensity = data.ParseFloat("Nightvision_Fog_Intensity", 0.25f);
				}
				this.isNightvisionAllowedInThirdPerson = data.ParseBool("Nightvision_Allowed_In_ThirdPerson", false);
			}
			else
			{
				this._vision = ELightingVision.NONE;
			}
			this.isBlindfold = data.ContainsKey("Blindfold");
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00051AD7 File Offset: 0x0004FCD7
		protected override bool GetDefaultTakesPriorityOverCosmetic()
		{
			return this.vision != ELightingVision.NONE || this.isBlindfold;
		}

		// Token: 0x04000946 RID: 2374
		protected GameObject _glasses;

		// Token: 0x04000947 RID: 2375
		private ELightingVision _vision;

		// Token: 0x04000948 RID: 2376
		public Color nightvisionColor;

		// Token: 0x04000949 RID: 2377
		public float nightvisionFogIntensity;
	}
}
