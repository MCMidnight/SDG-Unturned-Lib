using System;
using System.Collections.Generic;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x02000523 RID: 1315
	public static class WeatherEventListenerManager
	{
		// Token: 0x0600291B RID: 10523 RVA: 0x000AF154 File Offset: 0x000AD354
		public static void AddBlendAlphaListener(Guid assetGuid, WeatherBlendAlphaChangedListener listener)
		{
			WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup = WeatherEventListenerManager.FindOrAddGroupByAssetGuid(assetGuid);
			if (weatherListenerGroup != null)
			{
				weatherListenerGroup.blendAlphaListeners.Add(listener);
			}
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000AF178 File Offset: 0x000AD378
		public static void RemoveBlendAlphaListener(WeatherBlendAlphaChangedListener listener)
		{
			foreach (WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup in WeatherEventListenerManager.customWeatherListeners)
			{
				weatherListenerGroup.blendAlphaListeners.RemoveFast(listener);
			}
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000AF1D0 File Offset: 0x000AD3D0
		public static void AddStatusListener(Guid assetGuid, WeatherStatusChangedListener listener)
		{
			WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup = WeatherEventListenerManager.FindOrAddGroupByAssetGuid(assetGuid);
			if (weatherListenerGroup != null)
			{
				weatherListenerGroup.statusListeners.Add(listener);
			}
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x000AF1F4 File Offset: 0x000AD3F4
		public static void RemoveStatusListener(WeatherStatusChangedListener listener)
		{
			foreach (WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup in WeatherEventListenerManager.customWeatherListeners)
			{
				weatherListenerGroup.statusListeners.RemoveFast(listener);
			}
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000AF24C File Offset: 0x000AD44C
		internal static void AddComponentListener(Guid assetGuid, CustomWeatherEventHook listener)
		{
			WeatherEventListenerManager.FindOrAddComponentListenersByAssetGuid(assetGuid).Add(listener);
			bool flag;
			bool flag2;
			if (LevelLighting.GetWeatherStateForListeners(assetGuid, out flag, out flag2) && flag)
			{
				if (flag2)
				{
					listener.OnWeatherEndTransitionIn.TryInvoke(listener);
					return;
				}
				listener.OnWeatherBeginTransitionIn.TryInvoke(listener);
			}
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000AF290 File Offset: 0x000AD490
		internal static void RemoveComponentListener(Guid assetGuid, CustomWeatherEventHook listener)
		{
			List<CustomWeatherEventHook> list = WeatherEventListenerManager.FindComponentListenersByAssetGuid(assetGuid);
			if (list != null)
			{
				list.RemoveFast(listener);
			}
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000AF2B0 File Offset: 0x000AD4B0
		internal static void InvokeBeginTransitionIn(Guid assetGuid)
		{
			foreach (CustomWeatherEventHook customWeatherEventHook in WeatherEventListenerManager.EnumerateComponentListeners(assetGuid))
			{
				customWeatherEventHook.OnWeatherBeginTransitionIn.TryInvoke(customWeatherEventHook);
			}
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000AF304 File Offset: 0x000AD504
		internal static void InvokeEndTransitionIn(Guid assetGuid)
		{
			foreach (CustomWeatherEventHook customWeatherEventHook in WeatherEventListenerManager.EnumerateComponentListeners(assetGuid))
			{
				customWeatherEventHook.OnWeatherEndTransitionIn.TryInvoke(customWeatherEventHook);
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000AF358 File Offset: 0x000AD558
		internal static void InvokeBeginTransitionOut(Guid assetGuid)
		{
			foreach (CustomWeatherEventHook customWeatherEventHook in WeatherEventListenerManager.EnumerateComponentListeners(assetGuid))
			{
				customWeatherEventHook.OnWeatherBeginTransitionOut.TryInvoke(customWeatherEventHook);
			}
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000AF3AC File Offset: 0x000AD5AC
		internal static void InvokeEndTransitionOut(Guid assetGuid)
		{
			foreach (CustomWeatherEventHook customWeatherEventHook in WeatherEventListenerManager.EnumerateComponentListeners(assetGuid))
			{
				customWeatherEventHook.OnWeatherEndTransitionOut.TryInvoke(customWeatherEventHook);
			}
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000AF400 File Offset: 0x000AD600
		internal static void InvokeStatusChange(WeatherAssetBase asset, EWeatherStatusChange statusChange)
		{
			WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup = WeatherEventListenerManager.FindGroupByAssetGuid(asset.GUID);
			if (weatherListenerGroup != null)
			{
				for (int i = weatherListenerGroup.statusListeners.Count - 1; i >= 0; i--)
				{
					WeatherStatusChangedListener weatherStatusChangedListener = weatherListenerGroup.statusListeners[i];
					if (weatherStatusChangedListener != null)
					{
						weatherStatusChangedListener(asset, statusChange);
					}
					else
					{
						weatherListenerGroup.blendAlphaListeners.RemoveAtFast(i);
					}
				}
			}
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000AF45C File Offset: 0x000AD65C
		internal static void InvokeBlendAlphaChanged(WeatherAssetBase asset, float blendAlpha)
		{
			WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup = WeatherEventListenerManager.FindGroupByAssetGuid(asset.GUID);
			if (weatherListenerGroup != null)
			{
				for (int i = weatherListenerGroup.blendAlphaListeners.Count - 1; i >= 0; i--)
				{
					WeatherBlendAlphaChangedListener weatherBlendAlphaChangedListener = weatherListenerGroup.blendAlphaListeners[i];
					if (weatherBlendAlphaChangedListener != null)
					{
						weatherBlendAlphaChangedListener(asset, blendAlpha);
					}
					else
					{
						weatherListenerGroup.blendAlphaListeners.RemoveAtFast(i);
					}
				}
			}
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000AF4B8 File Offset: 0x000AD6B8
		private static WeatherEventListenerManager.WeatherListenerGroup FindGroupByAssetGuid(Guid assetGuid)
		{
			foreach (WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup in WeatherEventListenerManager.customWeatherListeners)
			{
				if (weatherListenerGroup.assetGuid == assetGuid)
				{
					return weatherListenerGroup;
				}
			}
			return null;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000AF518 File Offset: 0x000AD718
		private static WeatherEventListenerManager.WeatherListenerGroup FindOrAddGroupByAssetGuid(Guid assetGuid)
		{
			WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup = WeatherEventListenerManager.FindGroupByAssetGuid(assetGuid);
			if (weatherListenerGroup == null)
			{
				weatherListenerGroup = new WeatherEventListenerManager.WeatherListenerGroup(assetGuid);
				WeatherEventListenerManager.customWeatherListeners.Add(weatherListenerGroup);
			}
			return weatherListenerGroup;
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000AF544 File Offset: 0x000AD744
		private static List<CustomWeatherEventHook> FindComponentListenersByAssetGuid(Guid assetGuid)
		{
			WeatherEventListenerManager.WeatherListenerGroup weatherListenerGroup = WeatherEventListenerManager.FindGroupByAssetGuid(assetGuid);
			if (weatherListenerGroup == null)
			{
				return null;
			}
			return weatherListenerGroup.componentListeners;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000AF563 File Offset: 0x000AD763
		private static IEnumerable<CustomWeatherEventHook> EnumerateComponentListeners(Guid assetGuid)
		{
			WeatherEventListenerManager.WeatherListenerGroup group = WeatherEventListenerManager.FindGroupByAssetGuid(assetGuid);
			if (group == null)
			{
				yield break;
			}
			int num;
			for (int index = group.componentListeners.Count - 1; index >= 0; index = num)
			{
				CustomWeatherEventHook customWeatherEventHook = group.componentListeners[index];
				if (customWeatherEventHook != null)
				{
					yield return customWeatherEventHook;
				}
				else
				{
					group.componentListeners.RemoveAtFast(index);
				}
				num = index - 1;
			}
			yield break;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000AF573 File Offset: 0x000AD773
		private static List<CustomWeatherEventHook> FindOrAddComponentListenersByAssetGuid(Guid assetGuid)
		{
			return WeatherEventListenerManager.FindOrAddGroupByAssetGuid(assetGuid).componentListeners;
		}

		// Token: 0x040015EA RID: 5610
		private static List<WeatherEventListenerManager.WeatherListenerGroup> customWeatherListeners = new List<WeatherEventListenerManager.WeatherListenerGroup>();

		// Token: 0x02000964 RID: 2404
		private class WeatherListenerGroup
		{
			// Token: 0x06004B38 RID: 19256 RVA: 0x001B3CE7 File Offset: 0x001B1EE7
			public WeatherListenerGroup(Guid assetGuid)
			{
				this.assetGuid = assetGuid;
				this.componentListeners = new List<CustomWeatherEventHook>();
				this.blendAlphaListeners = new List<WeatherBlendAlphaChangedListener>();
				this.statusListeners = new List<WeatherStatusChangedListener>();
			}

			// Token: 0x04003349 RID: 13129
			public Guid assetGuid;

			// Token: 0x0400334A RID: 13130
			public List<CustomWeatherEventHook> componentListeners;

			// Token: 0x0400334B RID: 13131
			public List<WeatherBlendAlphaChangedListener> blendAlphaListeners;

			// Token: 0x0400334C RID: 13132
			public List<WeatherStatusChangedListener> statusListeners;
		}
	}
}
