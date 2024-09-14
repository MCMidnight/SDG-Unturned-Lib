using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows map makers to create custom weather events.
	/// </summary>
	// Token: 0x02000387 RID: 903
	public class WeatherAsset : WeatherAssetBase
	{
		/// <summary>
		/// Does this weather affect fog color and density?
		/// </summary>
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x000642BA File Offset: 0x000624BA
		// (set) Token: 0x06001C0C RID: 7180 RVA: 0x000642C2 File Offset: 0x000624C2
		public bool overrideFog { get; protected set; }

		/// <summary>
		/// Does this weather affect sky fog color?
		/// </summary>
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000642CB File Offset: 0x000624CB
		// (set) Token: 0x06001C0E RID: 7182 RVA: 0x000642D3 File Offset: 0x000624D3
		public bool overrideAtmosphericFog { get; protected set; }

		/// <summary>
		/// Does this weather affect cloud colors?
		/// </summary>
		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x000642DC File Offset: 0x000624DC
		// (set) Token: 0x06001C10 RID: 7184 RVA: 0x000642E4 File Offset: 0x000624E4
		public bool overrideCloudColors { get; protected set; }

		// Token: 0x06001C11 RID: 7185 RVA: 0x000642ED File Offset: 0x000624ED
		public void getTimeValues(int blendKey, int currentKey, out WeatherAsset.TimeValues blendFrom, out WeatherAsset.TimeValues blendTo)
		{
			blendTo = this.timeValues[currentKey];
			blendFrom = ((blendKey == -1) ? blendTo : this.timeValues[blendKey]);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x00064310 File Offset: 0x00062510
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (base.componentType == typeof(WeatherComponentBase))
			{
				base.componentType = typeof(CustomWeatherComponent);
			}
			this.overrideFog = data.ParseBool("Override_Fog", false);
			this.overrideAtmosphericFog = data.ParseBool("Override_Atmospheric_Fog", false);
			this.overrideCloudColors = data.ParseBool("Override_Cloud_Colors", false);
			if (data.ContainsKey("Shadow_Strength_Multiplier"))
			{
				this.shadowStrengthMultiplier = data.ParseFloat("Shadow_Strength_Multiplier", 0f);
			}
			else
			{
				this.shadowStrengthMultiplier = 1f;
			}
			if (data.ContainsKey("Fog_Blend_Exponent"))
			{
				this.fogBlendExponent = data.ParseFloat("Fog_Blend_Exponent", 0f);
			}
			else
			{
				this.fogBlendExponent = 1f;
			}
			if (data.ContainsKey("Cloud_Blend_Exponent"))
			{
				this.cloudBlendExponent = data.ParseFloat("Cloud_Blend_Exponent", 0f);
			}
			else
			{
				this.cloudBlendExponent = 1f;
			}
			this.windMain = data.ParseFloat("Wind_Main", 0f);
			this.staminaPerSecond = data.ParseFloat("Stamina_Per_Second", 0f);
			this.healthPerSecond = data.ParseFloat("Health_Per_Second", 0f);
			this.foodPerSecond = data.ParseFloat("Food_Per_Second", 0f);
			this.waterPerSecond = data.ParseFloat("Water_Per_Second", 0f);
			this.virusPerSecond = data.ParseFloat("Virus_Per_Second", 0f);
			this.timeValues = new WeatherAsset.TimeValues[4];
			this.timeValues[0] = new WeatherAsset.TimeValues(data.GetDictionary("Dawn"));
			this.timeValues[1] = new WeatherAsset.TimeValues(data.GetDictionary("Midday"));
			this.timeValues[2] = new WeatherAsset.TimeValues(data.GetDictionary("Dusk"));
			this.timeValues[3] = new WeatherAsset.TimeValues(data.GetDictionary("Midnight"));
			DatList datList;
			if (data.TryGetList("Effects", out datList))
			{
				this.effects = new WeatherAsset.Effect[datList.Count];
				for (int i = 0; i < datList.Count; i++)
				{
					this.effects[i] = datList[i].ParseStruct(default(WeatherAsset.Effect));
				}
			}
		}

		/// <summary>
		/// Directional light shadow strength multiplier.
		/// </summary>
		// Token: 0x04000D2B RID: 3371
		public float shadowStrengthMultiplier;

		/// <summary>
		/// Exponent applied to effect blend alpha.
		/// </summary>
		// Token: 0x04000D2C RID: 3372
		public float fogBlendExponent;

		/// <summary>
		/// Exponent applied to effect blend alpha.
		/// </summary>
		// Token: 0x04000D2D RID: 3373
		public float cloudBlendExponent;

		/// <summary>
		/// SpeedTree wind strength for blizzard. Should be removed?
		/// </summary>
		// Token: 0x04000D2E RID: 3374
		public float windMain;

		// Token: 0x04000D2F RID: 3375
		public float staminaPerSecond;

		// Token: 0x04000D30 RID: 3376
		public float healthPerSecond;

		// Token: 0x04000D31 RID: 3377
		public float foodPerSecond;

		// Token: 0x04000D32 RID: 3378
		public float waterPerSecond;

		// Token: 0x04000D33 RID: 3379
		public float virusPerSecond;

		// Token: 0x04000D34 RID: 3380
		public WeatherAsset.Effect[] effects;

		// Token: 0x04000D35 RID: 3381
		protected WeatherAsset.TimeValues[] timeValues;

		// Token: 0x0200092B RID: 2347
		public struct WeatherColor
		{
			// Token: 0x06004A89 RID: 19081 RVA: 0x001B1A80 File Offset: 0x001AFC80
			public WeatherColor(DatDictionary data)
			{
				if (data == null)
				{
					this.customColor = Color.black;
					this.levelEnum = ELightingColor.CUSTOM_OVERRIDE;
					return;
				}
				byte r = data.ContainsKey("R") ? data.ParseUInt8("R", 0) : byte.MaxValue;
				byte g = data.ContainsKey("G") ? data.ParseUInt8("G", 0) : byte.MaxValue;
				byte b = data.ContainsKey("B") ? data.ParseUInt8("B", 0) : byte.MaxValue;
				this.customColor = new Color32(r, g, b, byte.MaxValue);
				if (data.ContainsKey("Level_Enum"))
				{
					this.levelEnum = data.ParseEnum<ELightingColor>("Level_Enum", ELightingColor.SUN);
					return;
				}
				this.levelEnum = ELightingColor.CUSTOM_OVERRIDE;
			}

			// Token: 0x06004A8A RID: 19082 RVA: 0x001B1B47 File Offset: 0x001AFD47
			public Color Evaluate(LightingInfo levelValues)
			{
				if (this.levelEnum != ELightingColor.CUSTOM_OVERRIDE)
				{
					return levelValues.colors[(int)this.levelEnum] * this.customColor;
				}
				return this.customColor;
			}

			// Token: 0x04003291 RID: 12945
			public Color customColor;

			/// <summary>
			/// If specified level editor color can be used rather than a per-asset color.
			/// </summary>
			// Token: 0x04003292 RID: 12946
			public ELightingColor levelEnum;
		}

		// Token: 0x0200092C RID: 2348
		public class TimeValues
		{
			// Token: 0x06004A8B RID: 19083 RVA: 0x001B1B78 File Offset: 0x001AFD78
			public TimeValues(DatDictionary data)
			{
				if (data == null)
				{
					this.brightnessMultiplier = 1f;
					return;
				}
				this.fogColor = new WeatherAsset.WeatherColor(data.GetDictionary("Fog_Color"));
				this.fogDensity = data.ParseFloat("Fog_Density", 0f);
				this.cloudColor = new WeatherAsset.WeatherColor(data.GetDictionary("Cloud_Color"));
				this.cloudRimColor = new WeatherAsset.WeatherColor(data.GetDictionary("Cloud_Rim_Color"));
				if (data.ContainsKey("Brightness_Multiplier"))
				{
					this.brightnessMultiplier = data.ParseFloat("Brightness_Multiplier", 0f);
					return;
				}
				this.brightnessMultiplier = 1f;
			}

			// Token: 0x04003293 RID: 12947
			public WeatherAsset.WeatherColor fogColor;

			// Token: 0x04003294 RID: 12948
			public float fogDensity;

			// Token: 0x04003295 RID: 12949
			public WeatherAsset.WeatherColor cloudColor;

			// Token: 0x04003296 RID: 12950
			public WeatherAsset.WeatherColor cloudRimColor;

			// Token: 0x04003297 RID: 12951
			public float brightnessMultiplier;
		}

		// Token: 0x0200092D RID: 2349
		public struct Effect : IDatParseable
		{
			// Token: 0x06004A8C RID: 19084 RVA: 0x001B1C24 File Offset: 0x001AFE24
			public bool TryParse(IDatNode node)
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary == null)
				{
					return false;
				}
				this.prefab = datDictionary.ParseStruct<MasterBundleReference<GameObject>>("Prefab", default(MasterBundleReference<GameObject>));
				this.emissionExponent = datDictionary.ParseFloat("Emission_Exponent", 0f);
				this.pitch = datDictionary.ParseFloat("Pitch", 0f);
				this.translateWithView = datDictionary.ParseBool("Translate_With_View", false);
				this.rotateYawWithWind = datDictionary.ParseBool("Rotate_Yaw_With_Wind", false);
				return true;
			}

			// Token: 0x04003298 RID: 12952
			public MasterBundleReference<GameObject> prefab;

			// Token: 0x04003299 RID: 12953
			public float emissionExponent;

			// Token: 0x0400329A RID: 12954
			public float pitch;

			// Token: 0x0400329B RID: 12955
			public bool translateWithView;

			// Token: 0x0400329C RID: 12956
			public bool rotateYawWithWind;
		}
	}
}
