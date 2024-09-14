using System;
using UnityEngine;
using UnityEngine.Events;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject to receive weather events:
	/// - Day/Night
	/// - Full Moon
	/// - Rain
	/// - Snow
	/// </summary>
	// Token: 0x020005DB RID: 1499
	[AddComponentMenu("Unturned/Weather Event Hook")]
	public class WeatherEventHook : MonoBehaviour
	{
		// Token: 0x06003023 RID: 12323 RVA: 0x000D4644 File Offset: 0x000D2844
		protected void onDayNightUpdated(bool isDaytime)
		{
			if (isDaytime)
			{
				this.OnDay.TryInvoke(this);
				return;
			}
			this.OnNight.TryInvoke(this);
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000D4662 File Offset: 0x000D2862
		protected void onMoonUpdated(bool isFullMoon)
		{
			if (isFullMoon)
			{
				this.OnFullMoonBegin.TryInvoke(this);
				return;
			}
			this.OnFullMoonEnd.TryInvoke(this);
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x000D4680 File Offset: 0x000D2880
		protected void onRainUpdated(ELightingRain rain)
		{
			if (rain == ELightingRain.DRIZZLE)
			{
				this.OnRainBegin.TryInvoke(this);
				return;
			}
			this.OnRainEnd.TryInvoke(this);
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x000D469F File Offset: 0x000D289F
		protected void onSnowUpdated(ELightingSnow snow)
		{
			if (snow == ELightingSnow.BLIZZARD)
			{
				this.OnSnowBegin.TryInvoke(this);
				return;
			}
			this.OnSnowEnd.TryInvoke(this);
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x000D46C0 File Offset: 0x000D28C0
		protected void OnEnable()
		{
			LightingManager.onDayNightUpdated_ModHook = (DayNightUpdated)Delegate.Combine(LightingManager.onDayNightUpdated_ModHook, new DayNightUpdated(this.onDayNightUpdated));
			LightingManager.onMoonUpdated_ModHook = (MoonUpdated)Delegate.Combine(LightingManager.onMoonUpdated_ModHook, new MoonUpdated(this.onMoonUpdated));
			LightingManager.onRainUpdated_ModHook = (RainUpdated)Delegate.Combine(LightingManager.onRainUpdated_ModHook, new RainUpdated(this.onRainUpdated));
			LightingManager.onSnowUpdated_ModHook = (SnowUpdated)Delegate.Combine(LightingManager.onSnowUpdated_ModHook, new SnowUpdated(this.onSnowUpdated));
			this.onDayNightUpdated(LightingManager.isDaytime);
			this.onMoonUpdated(LightingManager.isFullMoon);
			this.onRainUpdated(LevelLighting.rainyness);
			this.onSnowUpdated(LevelLighting.snowyness);
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000D477C File Offset: 0x000D297C
		protected void OnDisable()
		{
			LightingManager.onDayNightUpdated_ModHook = (DayNightUpdated)Delegate.Remove(LightingManager.onDayNightUpdated_ModHook, new DayNightUpdated(this.onDayNightUpdated));
			LightingManager.onMoonUpdated_ModHook = (MoonUpdated)Delegate.Remove(LightingManager.onMoonUpdated_ModHook, new MoonUpdated(this.onMoonUpdated));
			LightingManager.onRainUpdated_ModHook = (RainUpdated)Delegate.Remove(LightingManager.onRainUpdated_ModHook, new RainUpdated(this.onRainUpdated));
			LightingManager.onSnowUpdated_ModHook = (SnowUpdated)Delegate.Remove(LightingManager.onSnowUpdated_ModHook, new SnowUpdated(this.onSnowUpdated));
		}

		/// <summary>
		/// Invoked when night changes to day.
		/// </summary>
		// Token: 0x04001A35 RID: 6709
		public UnityEvent OnDay;

		/// <summary>
		/// Invoked when day changes to night.
		/// </summary>
		// Token: 0x04001A36 RID: 6710
		public UnityEvent OnNight;

		/// <summary>
		/// Invoked when a zombie full-moon event starts.
		/// </summary>
		// Token: 0x04001A37 RID: 6711
		public UnityEvent OnFullMoonBegin;

		/// <summary>
		/// Invoked when a zombie full-moon event finishes.
		/// </summary>
		// Token: 0x04001A38 RID: 6712
		public UnityEvent OnFullMoonEnd;

		/// <summary>
		/// Invoked when rain starts to fall.
		/// </summary>
		// Token: 0x04001A39 RID: 6713
		public UnityEvent OnRainBegin;

		/// <summary>
		/// Invoked when rain finishes falling.
		/// </summary>
		// Token: 0x04001A3A RID: 6714
		public UnityEvent OnRainEnd;

		/// <summary>
		/// Invoked when snow starts to fall.
		/// </summary>
		// Token: 0x04001A3B RID: 6715
		public UnityEvent OnSnowBegin;

		/// <summary>
		/// Invoked when snow finishes falling.
		/// </summary>
		// Token: 0x04001A3C RID: 6716
		public UnityEvent OnSnowEnd;
	}
}
