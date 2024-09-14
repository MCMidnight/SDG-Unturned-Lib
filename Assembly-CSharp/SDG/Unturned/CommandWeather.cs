using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D8 RID: 984
	public class CommandWeather : Command
	{
		// Token: 0x06001D5A RID: 7514 RVA: 0x0006B5D0 File Offset: 0x000697D0
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (string.Equals(parameter, "0"))
			{
				LightingManager.ResetScheduledWeather();
				CommandWindow.Log(this.localization.format("WeatherText", "null"));
				return;
			}
			AssetReference<WeatherAssetBase> assetReference;
			if (AssetReference<WeatherAssetBase>.TryParse(parameter, out assetReference))
			{
				WeatherAssetBase weatherAssetBase = assetReference.Find();
				if (weatherAssetBase != null)
				{
					if (!LightingManager.ForecastWeatherImmediately(weatherAssetBase))
					{
						LightingManager.ActivatePerpetualWeather(weatherAssetBase);
					}
					CommandWindow.Log(this.localization.format("WeatherText", weatherAssetBase.name));
					return;
				}
			}
			string text = parameter.ToLower();
			if (text == this.localization.format("WeatherNone").ToLower())
			{
				LightingManager.ResetScheduledWeather();
			}
			else if (text == this.localization.format("WeatherDisable").ToLower())
			{
				LightingManager.DisableWeather();
			}
			else if (text == this.localization.format("WeatherStorm").ToLower())
			{
				WeatherAssetBase weatherAssetBase2 = WeatherAssetBase.DEFAULT_RAIN.Find();
				if (weatherAssetBase2 != null)
				{
					if (LightingManager.IsWeatherActive(weatherAssetBase2))
					{
						LightingManager.ResetScheduledWeather();
					}
					else
					{
						LightingManager.ForecastWeatherImmediately(weatherAssetBase2);
					}
				}
			}
			else
			{
				if (!(text == this.localization.format("WeatherBlizzard").ToLower()))
				{
					CommandWindow.LogError(this.localization.format("NoWeatherErrorText", text));
					return;
				}
				WeatherAssetBase weatherAssetBase3 = WeatherAssetBase.DEFAULT_SNOW.Find();
				if (weatherAssetBase3 != null)
				{
					if (LightingManager.IsWeatherActive(weatherAssetBase3))
					{
						LightingManager.ResetScheduledWeather();
					}
					else
					{
						LightingManager.ForecastWeatherImmediately(weatherAssetBase3);
					}
				}
			}
			CommandWindow.Log(this.localization.format("WeatherText", text));
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0006B760 File Offset: 0x00069960
		public CommandWeather(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("WeatherCommandText");
			this._info = this.localization.format("WeatherInfoText");
			this._help = this.localization.format("WeatherHelpText");
		}
	}
}
