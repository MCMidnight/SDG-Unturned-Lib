using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004B3 RID: 1203
	public class CustomWeatherComponent : WeatherComponentBase
	{
		// Token: 0x0600251E RID: 9502 RVA: 0x00093D64 File Offset: 0x00091F64
		public override void InitializeWeather()
		{
			base.InitializeWeather();
			this.customAsset = (this.asset as WeatherAsset);
			this.overrideFog = this.customAsset.overrideFog;
			this.overrideAtmosphericFog = this.customAsset.overrideAtmosphericFog;
			this.overrideCloudColors = this.customAsset.overrideCloudColors;
			this.shadowStrengthMultiplier = this.customAsset.shadowStrengthMultiplier;
			this.fogBlendExponent = this.customAsset.fogBlendExponent;
			this.cloudBlendExponent = this.customAsset.cloudBlendExponent;
			this.windMain = this.customAsset.windMain;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x00093DFF File Offset: 0x00091FFF
		public override void UpdateWeather()
		{
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x00093E04 File Offset: 0x00092004
		public override void UpdateLightingTime(int blendKey, int currentKey, float timeAlpha)
		{
			LightingInfo lightingInfo = LevelLighting.times[currentKey];
			LightingInfo levelValues = (blendKey == -1) ? lightingInfo : LevelLighting.times[blendKey];
			WeatherAsset.TimeValues timeValues;
			WeatherAsset.TimeValues timeValues2;
			this.customAsset.getTimeValues(blendKey, currentKey, out timeValues, out timeValues2);
			Color a = timeValues.fogColor.Evaluate(levelValues);
			Color b = timeValues2.fogColor.Evaluate(lightingInfo);
			this.fogColor = Color.Lerp(a, b, timeAlpha);
			this.fogDensity = Mathf.Lerp(timeValues.fogDensity, timeValues2.fogDensity, timeAlpha);
			Color a2 = timeValues.cloudColor.Evaluate(levelValues);
			Color b2 = timeValues2.cloudColor.Evaluate(lightingInfo);
			this.cloudColor = Color.Lerp(a2, b2, timeAlpha);
			Color a3 = timeValues.cloudRimColor.Evaluate(levelValues);
			Color b3 = timeValues2.cloudRimColor.Evaluate(lightingInfo);
			this.cloudRimColor = Color.Lerp(a3, b3, timeAlpha);
			this.brightnessMultiplier = Mathf.Lerp(timeValues.brightnessMultiplier, timeValues2.brightnessMultiplier, timeAlpha);
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x00093EED File Offset: 0x000920ED
		public override void PreDestroyWeather()
		{
			base.PreDestroyWeather();
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x00093EF8 File Offset: 0x000920F8
		private void Update()
		{
			if (this.customAsset == null || !Provider.isServer)
			{
				return;
			}
			float num = Time.deltaTime * this.globalBlendAlpha;
			this.staminaBuffer += this.customAsset.staminaPerSecond * num;
			this.healthBuffer += this.customAsset.healthPerSecond * num;
			this.foodBuffer += this.customAsset.foodPerSecond * num;
			this.waterBuffer += this.customAsset.waterPerSecond * num;
			this.virusBuffer += this.customAsset.virusPerSecond * num;
			int num2 = MathfEx.TruncateToInt(this.staminaBuffer);
			if (num2 != 0)
			{
				this.staminaBuffer -= (float)num2;
				foreach (Player player in base.EnumerateMaskedPlayers())
				{
					player.life.serverModifyStamina((float)num2);
				}
			}
			int num3 = MathfEx.TruncateToInt(this.healthBuffer);
			if (num3 != 0)
			{
				this.healthBuffer -= (float)num3;
				foreach (Player player2 in base.EnumerateMaskedPlayers())
				{
					player2.life.serverModifyHealth((float)num3);
				}
			}
			int num4 = MathfEx.TruncateToInt(this.foodBuffer);
			if (num4 != 0)
			{
				this.foodBuffer -= (float)num4;
				foreach (Player player3 in base.EnumerateMaskedPlayers())
				{
					player3.life.serverModifyFood((float)num4);
				}
			}
			int num5 = MathfEx.TruncateToInt(this.waterBuffer);
			if (num5 != 0)
			{
				this.waterBuffer -= (float)num5;
				foreach (Player player4 in base.EnumerateMaskedPlayers())
				{
					player4.life.serverModifyWater((float)num5);
				}
			}
			int num6 = MathfEx.TruncateToInt(this.virusBuffer);
			if (num6 != 0)
			{
				this.virusBuffer -= (float)num6;
				foreach (Player player5 in base.EnumerateMaskedPlayers())
				{
					player5.life.serverModifyVirus((float)num6);
				}
			}
		}

		// Token: 0x040012D4 RID: 4820
		public WeatherAsset customAsset;

		// Token: 0x040012D5 RID: 4821
		private float staminaBuffer;

		// Token: 0x040012D6 RID: 4822
		private float healthBuffer;

		// Token: 0x040012D7 RID: 4823
		private float foodBuffer;

		// Token: 0x040012D8 RID: 4824
		private float waterBuffer;

		// Token: 0x040012D9 RID: 4825
		private float virusBuffer;
	}
}
