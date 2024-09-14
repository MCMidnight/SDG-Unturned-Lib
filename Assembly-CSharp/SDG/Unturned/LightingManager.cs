using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000579 RID: 1401
	public class LightingManager : SteamCaller
	{
		// Token: 0x06002CB5 RID: 11445 RVA: 0x000C1C37 File Offset: 0x000BFE37
		private static void broadcastDayNightUpdated(bool isDaytime)
		{
			DayNightUpdated dayNightUpdated = LightingManager.onDayNightUpdated;
			if (dayNightUpdated != null)
			{
				dayNightUpdated(isDaytime);
			}
			DayNightUpdated dayNightUpdated2 = LightingManager.onDayNightUpdated_ModHook;
			if (dayNightUpdated2 == null)
			{
				return;
			}
			dayNightUpdated2(isDaytime);
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000C1C5A File Offset: 0x000BFE5A
		private static void broadcastMoonUpdated(bool isFullMoon)
		{
			MoonUpdated moonUpdated = LightingManager.onMoonUpdated;
			if (moonUpdated != null)
			{
				moonUpdated(isFullMoon);
			}
			MoonUpdated moonUpdated2 = LightingManager.onMoonUpdated_ModHook;
			if (moonUpdated2 == null)
			{
				return;
			}
			moonUpdated2(isFullMoon);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000C1C7D File Offset: 0x000BFE7D
		internal static void broadcastRainUpdated(ELightingRain rain)
		{
			RainUpdated rainUpdated = LightingManager.onRainUpdated;
			if (rainUpdated != null)
			{
				rainUpdated(rain);
			}
			RainUpdated rainUpdated2 = LightingManager.onRainUpdated_ModHook;
			if (rainUpdated2 == null)
			{
				return;
			}
			rainUpdated2(rain);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000C1CA0 File Offset: 0x000BFEA0
		internal static void broadcastSnowUpdated(ELightingSnow snow)
		{
			SnowUpdated snowUpdated = LightingManager.onSnowUpdated;
			if (snowUpdated != null)
			{
				snowUpdated(snow);
			}
			SnowUpdated snowUpdated2 = LightingManager.onSnowUpdated_ModHook;
			if (snowUpdated2 == null)
			{
				return;
			}
			snowUpdated2(snow);
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002CB9 RID: 11449 RVA: 0x000C1CC3 File Offset: 0x000BFEC3
		public static float day
		{
			get
			{
				return LightingManager.time / LightingManager.cycle;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002CBA RID: 11450 RVA: 0x000C1CD4 File Offset: 0x000BFED4
		// (set) Token: 0x06002CBB RID: 11451 RVA: 0x000C1CDC File Offset: 0x000BFEDC
		public static uint cycle
		{
			get
			{
				return LightingManager._cycle;
			}
			set
			{
				LightingManager._offset = Provider.time - (uint)(LightingManager.day * value);
				LightingManager._cycle = ((value > 0U) ? value : 3600U);
				if (Provider.isServer)
				{
					LightingManager.manager.updateLighting();
					LightingManager.SendLightingCycle.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), LightingManager.cycle);
				}
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x000C1D35 File Offset: 0x000BFF35
		// (set) Token: 0x06002CBD RID: 11453 RVA: 0x000C1D3C File Offset: 0x000BFF3C
		public static uint time
		{
			get
			{
				return LightingManager._time;
			}
			set
			{
				if (LightingManager.cycle > 0U)
				{
					value %= LightingManager.cycle;
				}
				LightingManager._offset = Provider.time - value;
				LightingManager._time = value;
				LightingManager.manager.updateLighting();
				LightingManager.SendLightingOffset.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), LightingManager.offset);
			}
		}

		/// <summary>
		/// Number of in-game days this world has run.
		/// Incremented each time night ends.
		/// Saved between sessions.
		/// </summary>
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x000C1D8B File Offset: 0x000BFF8B
		public static long DateCounter
		{
			get
			{
				return LightingManager.dateCounter;
			}
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000C1D92 File Offset: 0x000BFF92
		[Obsolete("Replaced by LevelLighting.GetActiveWeatherAsset")]
		public static WeatherAssetBase getCustomWeather()
		{
			return LevelLighting.GetActiveWeatherAsset();
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000C1D9C File Offset: 0x000BFF9C
		private static void SetAndReplicateActiveWeatherAsset(WeatherAssetBase asset, float blendAlpha)
		{
			NetId netId = (asset != null) ? NetIdRegistry.ClaimBlock(2U) : default(NetId);
			LevelLighting.SetActiveWeatherAsset(asset, blendAlpha, netId);
			Guid arg = (asset != null) ? asset.GUID : Guid.Empty;
			LightingManager.SendLightingActiveWeather.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), arg, blendAlpha, netId);
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002CC1 RID: 11457 RVA: 0x000C1DEA File Offset: 0x000BFFEA
		public static uint offset
		{
			get
			{
				return LightingManager._offset;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x000C1DF1 File Offset: 0x000BFFF1
		[Obsolete]
		public static bool hasRain
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000C1DF4 File Offset: 0x000BFFF4
		[Obsolete]
		public static bool hasSnow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000C1DF7 File Offset: 0x000BFFF7
		public static bool IsWeatherActive(WeatherAssetBase weatherAsset)
		{
			if (weatherAsset == null)
			{
				throw new ArgumentNullException("weatherAsset");
			}
			return LevelLighting.IsWeatherActive(weatherAsset);
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000C1E10 File Offset: 0x000C0010
		private static void SetPerpetualWeather(WeatherAssetBase asset, float blendAlpha)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.PerpetuallyActive;
			LightingManager.scheduledWeatherForecastTimer = 0f;
			LightingManager.scheduledWeatherActiveTimer = 0f;
			LightingManager.scheduledWeatherRef = asset.getReferenceTo<WeatherAssetBase>();
			LightingManager.SetAndReplicateActiveWeatherAsset(asset, blendAlpha);
			LightingManager.shouldTickScheduledWeather = false;
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000C1E5D File Offset: 0x000C005D
		public static void SetScheduledWeather(WeatherAssetBase weatherAsset, float forecastDuration, float activeDuration)
		{
			if (weatherAsset == null)
			{
				throw new ArgumentNullException("weatherAsset");
			}
			LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.Forecast;
			LightingManager.scheduledWeatherForecastTimer = forecastDuration;
			LightingManager.scheduledWeatherActiveTimer = activeDuration;
			LightingManager.scheduledWeatherRef = weatherAsset.getReferenceTo<WeatherAssetBase>();
			LightingManager.shouldTickScheduledWeather = true;
		}

		/// <summary>
		/// Set weather active and disable scheduling.
		/// </summary>
		// Token: 0x06002CC7 RID: 11463 RVA: 0x000C1E90 File Offset: 0x000C0090
		public static void ActivatePerpetualWeather(WeatherAssetBase asset)
		{
			LightingManager.SetPerpetualWeather(asset, 0f);
		}

		/// <returns>True if given weather has config.</returns>
		// Token: 0x06002CC8 RID: 11464 RVA: 0x000C1EA0 File Offset: 0x000C00A0
		public static bool ForecastWeatherImmediately(WeatherAssetBase weatherAsset)
		{
			if (LightingManager.schedulableWeathers != null)
			{
				foreach (LevelAsset.SchedulableWeather schedulableWeather in LightingManager.schedulableWeathers)
				{
					AssetReference<WeatherAssetBase> assetRef = schedulableWeather.assetRef;
					if (assetRef.isReferenceTo(weatherAsset))
					{
						float activeDuration = Random.Range(schedulableWeather.minDuration, schedulableWeather.maxDuration) * Provider.modeConfigData.Events.Weather_Duration_Multiplier * LightingManager.cycle;
						LightingManager.SetScheduledWeather(weatherAsset, 0f, activeDuration);
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Cancel scheduled weather and re-evaluate on next update.
		/// </summary>
		// Token: 0x06002CC9 RID: 11465 RVA: 0x000C1F1C File Offset: 0x000C011C
		public static void ResetScheduledWeather()
		{
			if (LevelLighting.GetActiveWeatherAsset() != null)
			{
				LightingManager.SetAndReplicateActiveWeatherAsset(null, 0f);
			}
			LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
		}

		/// <summary>
		/// Cancel active weather and prevent next weather from being scheduled.
		/// </summary>
		// Token: 0x06002CCA RID: 11466 RVA: 0x000C1F36 File Offset: 0x000C0136
		public static void DisableWeather()
		{
			if (LevelLighting.GetActiveWeatherAsset() != null)
			{
				LightingManager.SetAndReplicateActiveWeatherAsset(null, 0f);
			}
			LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
			LightingManager.shouldTickScheduledWeather = false;
		}

		/// <summary>
		/// Get weather override for the currently loaded level.
		/// Warning: this is kept for backwards compatibility, whereas newer maps will set LevelAsset.perpetualWeather.
		/// </summary>
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002CCB RID: 11467 RVA: 0x000C1F56 File Offset: 0x000C0156
		public static ELevelWeatherOverride levelWeatherOverride
		{
			get
			{
				if (Level.info != null && Level.info.configData != null)
				{
					return Level.info.configData.Weather_Override;
				}
				return ELevelWeatherOverride.NONE;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002CCD RID: 11469 RVA: 0x000C1F96 File Offset: 0x000C0196
		// (set) Token: 0x06002CCC RID: 11468 RVA: 0x000C1F7C File Offset: 0x000C017C
		public static bool isFullMoon
		{
			get
			{
				return LightingManager._isFullMoon;
			}
			set
			{
				if (value != LightingManager.isFullMoon)
				{
					LightingManager._isFullMoon = value;
					LightingManager.broadcastMoonUpdated(LightingManager.isFullMoon);
				}
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000C1F9D File Offset: 0x000C019D
		public static bool isDaytime
		{
			get
			{
				return LightingManager.day < LevelLighting.bias;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000C1FAB File Offset: 0x000C01AB
		public static bool isNighttime
		{
			get
			{
				return !LightingManager.isDaytime;
			}
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000C1FB8 File Offset: 0x000C01B8
		[Obsolete]
		public void tellLighting(CSteamID steamID, uint serverTime, uint newCycle, uint newOffset, byte moon, byte wind, byte rain, byte snow, Guid activeWeatherGuid)
		{
			this.tellLighting(steamID, serverTime, newCycle, newOffset, moon, wind, activeWeatherGuid, 0f);
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000C1FDC File Offset: 0x000C01DC
		[Obsolete]
		public void tellLighting(CSteamID steamID, uint serverTime, uint newCycle, uint newOffset, byte moon, byte wind, Guid activeWeatherGuid, float activeWeatherBlendAlpha)
		{
			LightingManager.ReceiveInitialLightingState(serverTime, newCycle, newOffset, moon, wind, activeWeatherGuid, activeWeatherBlendAlpha, default(NetId), 0);
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000C2004 File Offset: 0x000C0204
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLighting")]
		public static void ReceiveInitialLightingState(uint serverTime, uint newCycle, uint newOffset, byte moon, byte wind, Guid activeWeatherGuid, float activeWeatherBlendAlpha, NetId activeWeatherNetId, int newDateCounter)
		{
			Provider.time = serverTime;
			LightingManager._cycle = newCycle;
			LightingManager._offset = newOffset;
			LightingManager.dateCounter = (long)newDateCounter;
			UnturnedLog.info(string.Format("Received initial date counter: {0}", LightingManager.dateCounter));
			AssetReference<WeatherAssetBase> assetReference = new AssetReference<WeatherAssetBase>(activeWeatherGuid);
			WeatherAssetBase asset = assetReference.Find();
			LevelLighting.SetActiveWeatherAsset(asset, activeWeatherBlendAlpha, activeWeatherNetId);
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(activeWeatherGuid, asset, "ReceiveInitialLightingState");
			}
			LightingManager.manager.updateLighting();
			LevelLighting.moon = moon;
			LightingManager.isCycled = (LightingManager.day > LevelLighting.bias);
			LightingManager.isFullMoon = (LightingManager.isCycled && LevelLighting.moon == 2);
			LightingManager.broadcastDayNightUpdated(LightingManager.isDaytime);
			TimeOfDayChanged timeOfDayChanged = LightingManager.onTimeOfDayChanged;
			if (timeOfDayChanged != null)
			{
				timeOfDayChanged();
			}
			LevelLighting.wind = (float)wind * 2f;
			Level.isLoadingLighting = false;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000C20D9 File Offset: 0x000C02D9
		[Obsolete]
		public void askLighting(CSteamID steamID)
		{
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000C20DC File Offset: 0x000C02DC
		internal static void SendInitialGlobalState(SteamPlayer client)
		{
			if (Level.info.type != ELevelType.SURVIVAL)
			{
				return;
			}
			WeatherAssetBase weatherAssetBase;
			float arg;
			NetId arg2;
			LevelLighting.GetActiveWeatherNetState(out weatherAssetBase, out arg, out arg2);
			Guid arg3 = (weatherAssetBase != null) ? weatherAssetBase.GUID : Guid.Empty;
			LightingManager.SendInitialLightingState.Invoke(ENetReliability.Reliable, client.transportConnection, Provider.time, LightingManager.cycle, LightingManager.offset, LevelLighting.moon, MeasurementTool.angleToByte(LevelLighting.wind), arg3, arg, arg2, (int)LightingManager.dateCounter);
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000C214B File Offset: 0x000C034B
		[Obsolete]
		public void tellLightingCycle(CSteamID steamID, uint newScale)
		{
			LightingManager.ReceiveLightingCycle(newScale);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000C2153 File Offset: 0x000C0353
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLightingCycle")]
		public static void ReceiveLightingCycle(uint newScale)
		{
			LightingManager._offset = Provider.time - (uint)(LightingManager.day * newScale);
			LightingManager._cycle = newScale;
			LightingManager.manager.updateLighting();
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000C217A File Offset: 0x000C037A
		[Obsolete]
		public void tellLightingOffset(CSteamID steamID, uint newOffset)
		{
			LightingManager.ReceiveLightingOffset(newOffset);
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000C2182 File Offset: 0x000C0382
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLightingOffset")]
		public static void ReceiveLightingOffset(uint newOffset)
		{
			LightingManager._offset = newOffset;
			LightingManager.manager.updateLighting();
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000C2194 File Offset: 0x000C0394
		[Obsolete]
		public void tellLightingWind(CSteamID steamID, byte newWind)
		{
			LightingManager.ReceiveLightingWind(newWind);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000C219C File Offset: 0x000C039C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLightingWind")]
		public static void ReceiveLightingWind(byte newWind)
		{
			LevelLighting.wind = MeasurementTool.byteToAngle(newWind);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000C21A9 File Offset: 0x000C03A9
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveDateCounter(long newValue)
		{
			LightingManager.dateCounter = newValue;
			UnturnedLog.info(string.Format("Received date counter update: {0}", LightingManager.dateCounter));
			TimeOfDayChanged timeOfDayChanged = LightingManager.onTimeOfDayChanged;
			if (timeOfDayChanged == null)
			{
				return;
			}
			timeOfDayChanged();
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000C21D9 File Offset: 0x000C03D9
		[Obsolete]
		public void tellLightingRain(CSteamID steamID, byte newRain)
		{
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000C21DB File Offset: 0x000C03DB
		[Obsolete]
		public void tellLightingSnow(CSteamID steamID, byte newSnow)
		{
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000C21E0 File Offset: 0x000C03E0
		[Obsolete]
		public void tellLightingActiveWeather(CSteamID steamID, Guid assetGuid, float blendAlpha)
		{
			LightingManager.ReceiveLightingActiveWeather(assetGuid, blendAlpha, default(NetId));
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000C2200 File Offset: 0x000C0400
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLightingActiveWeather")]
		public static void ReceiveLightingActiveWeather(Guid assetGuid, float blendAlpha, NetId netId)
		{
			AssetReference<WeatherAssetBase> assetReference = new AssetReference<WeatherAssetBase>(assetGuid);
			WeatherAssetBase asset = assetReference.Find();
			LevelLighting.SetActiveWeatherAsset(asset, blendAlpha, netId);
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetGuid, asset, "ReceiveLightingActiveWeather");
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000C2238 File Offset: 0x000C0438
		private void updateLighting()
		{
			LightingManager._time = (Provider.time - LightingManager.offset) % LightingManager.cycle;
			if (Provider.isServer && Time.time - LightingManager.lastWind > LightingManager.windDelay)
			{
				LightingManager.windDelay = (float)Random.Range(45, 75);
				LightingManager.lastWind = Time.time;
				LevelLighting.wind = (float)Random.Range(0, 360);
				LightingManager.SendLightingWind.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), MeasurementTool.angleToByte(LevelLighting.wind));
			}
			if (LightingManager.day > LevelLighting.bias)
			{
				if (!LightingManager.isCycled)
				{
					LightingManager.isCycled = true;
					if (LevelLighting.moon < LevelLighting.MOON_CYCLES - 1)
					{
						LevelLighting.moon += 1;
						LightingManager.isFullMoon = (LevelLighting.moon == 2);
					}
					else
					{
						LevelLighting.moon = 0;
						LightingManager.isFullMoon = false;
					}
					LightingManager.broadcastDayNightUpdated(false);
				}
			}
			else if (LightingManager.isCycled)
			{
				LightingManager.isCycled = false;
				LightingManager.isFullMoon = false;
				if (Provider.isServer)
				{
					LightingManager.dateCounter += 1L;
					UnturnedLog.info(string.Format("Incremented date counter: {0}", LightingManager.dateCounter));
					LightingManager.SendDateCounter.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), LightingManager.dateCounter);
				}
				LightingManager.broadcastDayNightUpdated(true);
			}
			TimeOfDayChanged timeOfDayChanged = LightingManager.onTimeOfDayChanged;
			if (timeOfDayChanged == null)
			{
				return;
			}
			timeOfDayChanged();
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000C237C File Offset: 0x000C057C
		private void TickScheduledWeather()
		{
			if (LightingManager.scheduledWeatherStage != LightingManager.EScheduledWeatherStage.None)
			{
				if (LightingManager.scheduledWeatherStage == LightingManager.EScheduledWeatherStage.Forecast)
				{
					LightingManager.scheduledWeatherForecastTimer -= Time.deltaTime;
					if (LightingManager.scheduledWeatherForecastTimer <= 0f)
					{
						WeatherAssetBase weatherAssetBase = LightingManager.scheduledWeatherRef.Find();
						if (weatherAssetBase != null)
						{
							LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.Active;
							LightingManager.SetAndReplicateActiveWeatherAsset(LightingManager.scheduledWeatherRef.Find(), 0f);
							UnturnedLog.info("Weather {0} starting for {1} seconds", new object[]
							{
								weatherAssetBase.name,
								LightingManager.scheduledWeatherActiveTimer
							});
							return;
						}
						LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
						UnturnedLog.warn("Missing forecast weather asset {0}", new object[]
						{
							LightingManager.scheduledWeatherRef
						});
						return;
					}
				}
				else if (LightingManager.scheduledWeatherStage == LightingManager.EScheduledWeatherStage.Active)
				{
					LightingManager.scheduledWeatherActiveTimer -= Time.deltaTime;
					if (LightingManager.scheduledWeatherActiveTimer <= 0f)
					{
						WeatherAssetBase weatherAssetBase2 = LightingManager.scheduledWeatherRef.Find();
						if (weatherAssetBase2 != null)
						{
							LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
							LightingManager.SetAndReplicateActiveWeatherAsset(null, 0f);
							UnturnedLog.info("Weather {0} ending", new object[]
							{
								weatherAssetBase2.name
							});
							return;
						}
						LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
						UnturnedLog.warn("Missing active weather asset {0}", new object[]
						{
							LightingManager.scheduledWeatherRef
						});
					}
				}
				return;
			}
			if (LightingManager.schedulableWeathers == null || LightingManager.schedulableWeathers.Length == 0)
			{
				UnturnedLog.warn("Disabling scheduled weather because none are available");
				LightingManager.shouldTickScheduledWeather = false;
				return;
			}
			int num = Random.Range(0, LightingManager.schedulableWeathers.Length);
			LevelAsset.SchedulableWeather schedulableWeather = LightingManager.schedulableWeathers[num];
			WeatherAssetBase weatherAssetBase3 = schedulableWeather.assetRef.Find();
			if (weatherAssetBase3 != null)
			{
				float forecastDuration = Random.Range(schedulableWeather.minFrequency, schedulableWeather.maxFrequency) * Provider.modeConfigData.Events.Weather_Frequency_Multiplier * LightingManager.cycle;
				float activeDuration = Random.Range(schedulableWeather.minDuration, schedulableWeather.maxDuration) * Provider.modeConfigData.Events.Weather_Duration_Multiplier * LightingManager.cycle;
				LightingManager.SetScheduledWeather(weatherAssetBase3, forecastDuration, activeDuration);
				UnturnedLog.info("Weather {0} forecast in {1} seconds", new object[]
				{
					weatherAssetBase3.name,
					LightingManager.scheduledWeatherForecastTimer
				});
				return;
			}
			UnturnedLog.warn("Missing level weather asset {0}", new object[]
			{
				schedulableWeather.assetRef
			});
			LightingManager.shouldTickScheduledWeather = false;
		}

		/// <summary>
		/// Assign schedulableWeathers array according to level asset or legacy lighting settings.
		/// </summary>
		// Token: 0x06002CE2 RID: 11490 RVA: 0x000C25B4 File Offset: 0x000C07B4
		private void InitSchedulableWeathers()
		{
			if (Provider.modeConfigData.Events.Weather_Duration_Multiplier < 0.001f)
			{
				UnturnedLog.info("Disabling scheduled weather because duration multiplier is zero");
				LightingManager.schedulableWeathers = null;
				return;
			}
			LevelAsset asset = Level.getAsset();
			if (asset != null && asset.schedulableWeathers != null)
			{
				LightingManager.schedulableWeathers = asset.schedulableWeathers;
				return;
			}
			if (!LevelLighting.canRain && !LevelLighting.canSnow)
			{
				LightingManager.schedulableWeathers = null;
				return;
			}
			List<LevelAsset.SchedulableWeather> list = new List<LevelAsset.SchedulableWeather>(2);
			if (LevelLighting.canRain)
			{
				float num = Provider.modeConfigData.Events.Rain_Duration_Min * LevelLighting.rainDur;
				float b = Provider.modeConfigData.Events.Rain_Duration_Max * LevelLighting.rainDur;
				if (Mathf.Max(num, b) > 0.001f)
				{
					list.Add(new LevelAsset.SchedulableWeather
					{
						assetRef = WeatherAssetBase.DEFAULT_RAIN,
						minFrequency = Mathf.Max(0f, Provider.modeConfigData.Events.Rain_Frequency_Min * LevelLighting.rainFreq),
						maxFrequency = Mathf.Max(0f, Provider.modeConfigData.Events.Rain_Frequency_Max * LevelLighting.rainFreq),
						minDuration = Mathf.Max(0f, num),
						maxDuration = Mathf.Max(0f, b)
					});
				}
				else
				{
					UnturnedLog.info("Disabling legacy rain because max duration is zero");
				}
			}
			if (LevelLighting.canSnow)
			{
				float num2 = Provider.modeConfigData.Events.Snow_Duration_Min * LevelLighting.snowDur;
				float b2 = Provider.modeConfigData.Events.Snow_Duration_Max * LevelLighting.snowDur;
				if (Mathf.Max(num2, b2) > 0.001f)
				{
					list.Add(new LevelAsset.SchedulableWeather
					{
						assetRef = WeatherAssetBase.DEFAULT_SNOW,
						minFrequency = Mathf.Max(0f, Provider.modeConfigData.Events.Snow_Frequency_Min * LevelLighting.snowFreq),
						maxFrequency = Mathf.Max(0f, Provider.modeConfigData.Events.Snow_Frequency_Max * LevelLighting.snowFreq),
						minDuration = Mathf.Max(0f, num2),
						maxDuration = Mathf.Max(0f, b2)
					});
				}
				else
				{
					UnturnedLog.info("Disabling legacy snow because max duration is zero");
				}
			}
			LightingManager.schedulableWeathers = list.ToArray();
		}

		/// <returns>True if perpetual weather was enabled, false otherwise.</returns>
		// Token: 0x06002CE3 RID: 11491 RVA: 0x000C27F0 File Offset: 0x000C09F0
		private bool InitPerpetualWeather()
		{
			LevelAsset asset = Level.getAsset();
			AssetReference<WeatherAssetBase> assetReference;
			if (asset != null && asset.perpetualWeatherRef.isValid)
			{
				assetReference = asset.perpetualWeatherRef;
			}
			else
			{
				ELevelWeatherOverride levelWeatherOverride = LightingManager.levelWeatherOverride;
				if (levelWeatherOverride != ELevelWeatherOverride.RAIN)
				{
					if (levelWeatherOverride != ELevelWeatherOverride.SNOW)
					{
						return false;
					}
					assetReference = WeatherAssetBase.DEFAULT_SNOW;
				}
				else
				{
					assetReference = WeatherAssetBase.DEFAULT_RAIN;
				}
			}
			WeatherAssetBase weatherAssetBase = assetReference.Find();
			if (weatherAssetBase != null)
			{
				UnturnedLog.info("Level perpetual weather override {0}", new object[]
				{
					weatherAssetBase.name
				});
				LightingManager.SetPerpetualWeather(weatherAssetBase, 1f);
				return true;
			}
			UnturnedLog.warn("Missing level perpetual weather asset {0}", new object[]
			{
				assetReference
			});
			return false;
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000C288C File Offset: 0x000C0A8C
		private void InitLoadedWeather()
		{
			if (LightingManager.scheduledWeatherStage == LightingManager.EScheduledWeatherStage.Forecast)
			{
				WeatherAssetBase weatherAssetBase = LightingManager.scheduledWeatherRef.Find();
				if (weatherAssetBase != null)
				{
					UnturnedLog.info("Loaded weather {0} forecast in {1} seconds", new object[]
					{
						weatherAssetBase.name,
						LightingManager.scheduledWeatherForecastTimer
					});
					return;
				}
				LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
				UnturnedLog.warn("Missing loaded forecast weather asset {0}", new object[]
				{
					LightingManager.scheduledWeatherRef
				});
				return;
			}
			else
			{
				if (LightingManager.scheduledWeatherStage != LightingManager.EScheduledWeatherStage.Active)
				{
					if (LightingManager.scheduledWeatherStage == LightingManager.EScheduledWeatherStage.PerpetuallyActive)
					{
						WeatherAssetBase weatherAssetBase2 = LightingManager.scheduledWeatherRef.Find();
						if (weatherAssetBase2 != null)
						{
							LightingManager.SetAndReplicateActiveWeatherAsset(LightingManager.scheduledWeatherRef.Find(), LightingManager.loadedWeatherBlendAlpha);
							UnturnedLog.info("Loaded perpetual weather {0} with global alpha {1}", new object[]
							{
								weatherAssetBase2.name,
								LightingManager.loadedWeatherBlendAlpha
							});
							LightingManager.shouldTickScheduledWeather = false;
							return;
						}
						LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
						UnturnedLog.warn("Missing loaded perpetual weather asset {0}", new object[]
						{
							LightingManager.scheduledWeatherRef
						});
					}
					return;
				}
				WeatherAssetBase weatherAssetBase3 = LightingManager.scheduledWeatherRef.Find();
				if (weatherAssetBase3 != null)
				{
					LightingManager.SetAndReplicateActiveWeatherAsset(LightingManager.scheduledWeatherRef.Find(), LightingManager.loadedWeatherBlendAlpha);
					UnturnedLog.info("Loaded weather {0} with global alpha {1} ending in {2} seconds", new object[]
					{
						weatherAssetBase3.name,
						LightingManager.loadedWeatherBlendAlpha,
						LightingManager.scheduledWeatherActiveTimer
					});
					return;
				}
				LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
				UnturnedLog.warn("Missing loaded active weather asset {0}", new object[]
				{
					LightingManager.scheduledWeatherRef
				});
				return;
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000C29F6 File Offset: 0x000C0BF6
		private void onPrePreLevelLoaded(int level)
		{
			LightingManager.onDayNightUpdated = null;
			LightingManager.onTimeOfDayChanged = null;
			LightingManager.onMoonUpdated = null;
			LightingManager.onRainUpdated = null;
			LightingManager.onSnowUpdated = null;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000C2A18 File Offset: 0x000C0C18
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				LightingManager.scheduledWeatherStage = LightingManager.EScheduledWeatherStage.None;
				LightingManager.scheduledWeatherForecastTimer = -1f;
				LightingManager.scheduledWeatherActiveTimer = -1f;
				LightingManager.scheduledWeatherRef = AssetReference<WeatherAssetBase>.invalid;
				this.InitSchedulableWeathers();
				LightingManager.shouldTickScheduledWeather = (LightingManager.schedulableWeathers != null && LightingManager.schedulableWeathers.Length != 0);
				LevelLighting.rainyness = ELightingRain.NONE;
				LevelLighting.snowyness = ELightingSnow.NONE;
				if (Level.info != null && Level.info.type != ELevelType.SURVIVAL)
				{
					LightingManager._cycle = 3600U;
					LightingManager._offset = 0U;
					LightingManager.dateCounter = 0L;
					if (Level.info.type == ELevelType.HORDE)
					{
						LightingManager._time = (uint)((LevelLighting.bias + (1f - LevelLighting.bias) / 2f) * LightingManager.cycle);
						LightingManager._isFullMoon = true;
					}
					else if (Level.info.type == ELevelType.ARENA)
					{
						LightingManager._time = (uint)(LevelLighting.transition * LightingManager.cycle);
						LightingManager._isFullMoon = false;
					}
					LightingManager.windDelay = (float)Random.Range(45, 75);
					LevelLighting.wind = (float)Random.Range(0, 360);
					this.InitPerpetualWeather();
					Level.isLoadingLighting = false;
					return;
				}
				LightingManager._cycle = 3600U;
				LightingManager._time = 0U;
				LightingManager._offset = 0U;
				LightingManager.dateCounter = 0L;
				LightingManager._isFullMoon = false;
				LightingManager.isCycled = false;
				LightingManager.broadcastDayNightUpdated(true);
				TimeOfDayChanged timeOfDayChanged = LightingManager.onTimeOfDayChanged;
				if (timeOfDayChanged != null)
				{
					timeOfDayChanged();
				}
				LightingManager.windDelay = (float)Random.Range(45, 75);
				LevelLighting.wind = (float)Random.Range(0, 360);
				if (Provider.isServer)
				{
					LightingManager.load();
					if (!this.InitPerpetualWeather())
					{
						this.InitLoadedWeather();
					}
					this.updateLighting();
					Level.isLoadingLighting = false;
				}
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000C2BC4 File Offset: 0x000C0DC4
		private void Update()
		{
			if (!Level.isLoaded || Level.info == null)
			{
				return;
			}
			if (Level.isEditor)
			{
				LevelLighting.updateLighting();
			}
			else if (Level.info.type == ELevelType.SURVIVAL)
			{
				this.updateLighting();
			}
			LevelLighting.tickCustomWeatherBlending(uint.MaxValue);
			if (Provider.isServer && LightingManager.shouldTickScheduledWeather)
			{
				this.TickScheduledWeather();
			}
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000C2C1C File Offset: 0x000C0E1C
		private void Start()
		{
			LightingManager.manager = this;
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onPrePreLevelLoaded));
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000C2C70 File Offset: 0x000C0E70
		public static void load()
		{
			bool flag = true;
			if (LevelSavedata.fileExists("/Lighting.dat"))
			{
				River river = LevelSavedata.openRiver("/Lighting.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					LightingManager._cycle = river.readUInt32();
					if (LightingManager._cycle < 1U)
					{
						LightingManager._cycle = 3600U;
					}
					LightingManager._time = river.readUInt32();
					if (b > 1 && b < 5)
					{
						river.readUInt32();
						river.readUInt32();
						river.readBoolean();
						river.readByte();
					}
					if (b > 2 && b < 5)
					{
						river.readUInt32();
						river.readUInt32();
						river.readBoolean();
						river.readByte();
					}
					if (b > 3)
					{
						LightingManager.scheduledWeatherStage = (LightingManager.EScheduledWeatherStage)river.readByte();
						LightingManager.scheduledWeatherForecastTimer = river.readSingle();
						LightingManager.scheduledWeatherActiveTimer = river.readSingle();
						LightingManager.scheduledWeatherRef = new AssetReference<WeatherAssetBase>(river.readGUID());
						if (b > 5)
						{
							LightingManager.loadedWeatherBlendAlpha = river.readSingle();
						}
						else
						{
							LightingManager.loadedWeatherBlendAlpha = 0f;
						}
					}
					LightingManager._offset = Provider.time - LightingManager.time;
					if (b >= 7)
					{
						LightingManager.dateCounter = river.readInt64();
						if (LightingManager.dateCounter < 0L)
						{
							LightingManager.dateCounter = 0L;
						}
						UnturnedLog.info(string.Format("Loaded date counter: {0}", LightingManager.dateCounter));
					}
					else
					{
						LightingManager.dateCounter = 0L;
					}
					flag = false;
				}
				river.closeRiver();
			}
			if (flag)
			{
				LightingManager._time = (uint)(LightingManager.cycle * LevelLighting.transition);
				LightingManager._offset = Provider.time - LightingManager.time;
				LightingManager.dateCounter = 0L;
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000C2DF0 File Offset: 0x000C0FF0
		public static void save()
		{
			River river = LevelSavedata.openRiver("/Lighting.dat", false);
			river.writeByte(7);
			river.writeUInt32(LightingManager.cycle);
			river.writeUInt32(LightingManager.time);
			river.writeByte((byte)LightingManager.scheduledWeatherStage);
			river.writeSingle(LightingManager.scheduledWeatherForecastTimer);
			river.writeSingle(LightingManager.scheduledWeatherActiveTimer);
			river.writeGUID(LightingManager.scheduledWeatherRef.GUID);
			river.writeSingle(LevelLighting.GetActiveWeatherGlobalBlendAlpha());
			river.writeInt64(LightingManager.dateCounter);
			river.closeRiver();
		}

		/// <summary>
		/// Version before named version constants were introduced. (2023-11-07)
		/// </summary>
		// Token: 0x0400181D RID: 6173
		public const byte SAVEDATA_VERSION_INITIAL = 6;

		// Token: 0x0400181E RID: 6174
		public const byte SAVEDATA_VERSION_ADDED_DATE_COUNTER = 7;

		// Token: 0x0400181F RID: 6175
		private const byte SAVEDATA_VERSION_NEWEST = 7;

		// Token: 0x04001820 RID: 6176
		public static readonly byte SAVEDATA_VERSION = 7;

		// Token: 0x04001821 RID: 6177
		public static DayNightUpdated onDayNightUpdated;

		/// <summary>
		/// Delegate not reset when level reset.
		/// </summary>
		// Token: 0x04001822 RID: 6178
		public static DayNightUpdated onDayNightUpdated_ModHook;

		// Token: 0x04001823 RID: 6179
		public static TimeOfDayChanged onTimeOfDayChanged;

		// Token: 0x04001824 RID: 6180
		public static MoonUpdated onMoonUpdated;

		/// <summary>
		/// Delegate not reset when level reset.
		/// </summary>
		// Token: 0x04001825 RID: 6181
		public static MoonUpdated onMoonUpdated_ModHook;

		// Token: 0x04001826 RID: 6182
		public static RainUpdated onRainUpdated;

		/// <summary>
		/// Delegate not reset when level reset.
		/// </summary>
		// Token: 0x04001827 RID: 6183
		public static RainUpdated onRainUpdated_ModHook;

		// Token: 0x04001828 RID: 6184
		public static SnowUpdated onSnowUpdated;

		/// <summary>
		/// Delegate not reset when level reset.
		/// </summary>
		// Token: 0x04001829 RID: 6185
		public static SnowUpdated onSnowUpdated_ModHook;

		// Token: 0x0400182A RID: 6186
		private static LightingManager manager;

		// Token: 0x0400182B RID: 6187
		private static uint _cycle;

		// Token: 0x0400182C RID: 6188
		private static uint _time;

		// Token: 0x0400182D RID: 6189
		private static long dateCounter;

		// Token: 0x0400182E RID: 6190
		private static uint _offset;

		// Token: 0x0400182F RID: 6191
		[Obsolete]
		public static uint rainFrequency;

		// Token: 0x04001830 RID: 6192
		[Obsolete]
		public static uint rainDuration;

		// Token: 0x04001831 RID: 6193
		[Obsolete]
		public static uint snowFrequency;

		// Token: 0x04001832 RID: 6194
		[Obsolete]
		public static uint snowDuration;

		/// <summary>
		/// Determines which weather can naturally be scheduled in this level.
		/// Includes default rain and snow for older levels.
		/// </summary>
		// Token: 0x04001833 RID: 6195
		private static LevelAsset.SchedulableWeather[] schedulableWeathers;

		// Token: 0x04001834 RID: 6196
		private static LightingManager.EScheduledWeatherStage scheduledWeatherStage;

		/// <summary>
		/// Seconds until weather activates.
		/// </summary>
		// Token: 0x04001835 RID: 6197
		private static float scheduledWeatherForecastTimer;

		/// <summary>
		/// Seconds until weather deactivates.
		/// </summary>
		// Token: 0x04001836 RID: 6198
		private static float scheduledWeatherActiveTimer;

		/// <summary>
		/// Forecast or active weather.
		/// </summary>
		// Token: 0x04001837 RID: 6199
		private static AssetReference<WeatherAssetBase> scheduledWeatherRef;

		// Token: 0x04001838 RID: 6200
		private static bool shouldTickScheduledWeather;

		// Token: 0x04001839 RID: 6201
		private static float loadedWeatherBlendAlpha;

		// Token: 0x0400183A RID: 6202
		private static bool isCycled;

		// Token: 0x0400183B RID: 6203
		private static bool _isFullMoon;

		// Token: 0x0400183C RID: 6204
		private static float lastWind;

		// Token: 0x0400183D RID: 6205
		private static float windDelay;

		// Token: 0x0400183E RID: 6206
		private static readonly ClientStaticMethod<uint, uint, uint, byte, byte, Guid, float, NetId, int> SendInitialLightingState = ClientStaticMethod<uint, uint, uint, byte, byte, Guid, float, NetId, int>.Get(new ClientStaticMethod<uint, uint, uint, byte, byte, Guid, float, NetId, int>.ReceiveDelegate(LightingManager.ReceiveInitialLightingState));

		// Token: 0x0400183F RID: 6207
		private static readonly ClientStaticMethod<uint> SendLightingCycle = ClientStaticMethod<uint>.Get(new ClientStaticMethod<uint>.ReceiveDelegate(LightingManager.ReceiveLightingCycle));

		// Token: 0x04001840 RID: 6208
		private static readonly ClientStaticMethod<uint> SendLightingOffset = ClientStaticMethod<uint>.Get(new ClientStaticMethod<uint>.ReceiveDelegate(LightingManager.ReceiveLightingOffset));

		// Token: 0x04001841 RID: 6209
		private static readonly ClientStaticMethod<byte> SendLightingWind = ClientStaticMethod<byte>.Get(new ClientStaticMethod<byte>.ReceiveDelegate(LightingManager.ReceiveLightingWind));

		// Token: 0x04001842 RID: 6210
		private static readonly ClientStaticMethod<long> SendDateCounter = ClientStaticMethod<long>.Get(new ClientStaticMethod<long>.ReceiveDelegate(LightingManager.ReceiveDateCounter));

		// Token: 0x04001843 RID: 6211
		private static readonly ClientStaticMethod<Guid, float, NetId> SendLightingActiveWeather = ClientStaticMethod<Guid, float, NetId>.Get(new ClientStaticMethod<Guid, float, NetId>.ReceiveDelegate(LightingManager.ReceiveLightingActiveWeather));

		// Token: 0x0200097C RID: 2428
		private enum EScheduledWeatherStage
		{
			/// <summary>
			/// Weather has not been decided yet. Level might not have any enabled.
			/// </summary>
			// Token: 0x04003383 RID: 13187
			None,
			/// <summary>
			/// Weather has been forecast. Timer counts down until activation.
			/// </summary>
			// Token: 0x04003384 RID: 13188
			Forecast,
			/// <summary>
			/// Weather is now active. Timer counts down until deactivation.
			/// </summary>
			// Token: 0x04003385 RID: 13189
			Active,
			/// <summary>
			/// Weather is active. Will not deactivate naturally.
			/// Prevents loaded perpetual weather from deactivating.
			/// </summary>
			// Token: 0x04003386 RID: 13190
			PerpetuallyActive
		}
	}
}
