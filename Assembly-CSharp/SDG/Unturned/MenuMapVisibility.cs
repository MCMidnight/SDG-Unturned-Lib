using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDG.Unturned
{
	/// <summary>
	/// Component in the root Menu scene.
	/// Additively loads decoration levels without modifying main scene.
	/// </summary>
	// Token: 0x020005B9 RID: 1465
	public class MenuMapVisibility : MonoBehaviour
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x000D24C8 File Offset: 0x000D06C8
		public void Awake()
		{
			if (MenuMapVisibility.HelperClass.clNoAdditiveMenu)
			{
				UnturnedLog.info("Skipping loading of additive menu scenes");
				return;
			}
			bool flag = true;
			string text = null;
			if (MenuMapVisibility.HelperClass.clAdditiveMenuOverride.hasValue)
			{
				flag = false;
				text = MenuMapVisibility.HelperClass.clAdditiveMenuOverride.value;
			}
			else if (Provider.statusData != null && Provider.statusData.Menu != null && !string.IsNullOrEmpty(Provider.statusData.Menu.PromoLevel))
			{
				DateTime promoStart = Provider.statusData.Menu.PromoStart;
				DateTime promoEnd = Provider.statusData.Menu.PromoEnd;
				if (new DateTimeRange(promoStart, promoEnd).isNowWithinRange())
				{
					flag = false;
					text = Provider.statusData.Menu.PromoLevel;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				UnturnedLog.info("Loading additive promo scene {0}", new object[]
				{
					text
				});
				SceneManager.LoadSceneAsync(text, LoadSceneMode.Additive);
			}
			if (flag)
			{
				if (Provider.isBackendRealtimeAvailable)
				{
					this.handleHolidayScenes();
					return;
				}
				Provider.onBackendRealtimeAvailable = (Provider.BackendRealtimeAvailableHandler)Delegate.Combine(Provider.onBackendRealtimeAvailable, new Provider.BackendRealtimeAvailableHandler(this.onBackendRealtimeAvailable));
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000D25C7 File Offset: 0x000D07C7
		protected void onBackendRealtimeAvailable()
		{
			Provider.onBackendRealtimeAvailable = (Provider.BackendRealtimeAvailableHandler)Delegate.Remove(Provider.onBackendRealtimeAvailable, new Provider.BackendRealtimeAvailableHandler(this.onBackendRealtimeAvailable));
			this.handleHolidayScenes();
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000D25F0 File Offset: 0x000D07F0
		protected void handleHolidayScenes()
		{
			ENPCHoliday activeHoliday = HolidayUtil.getActiveHoliday();
			if (activeHoliday == ENPCHoliday.CHRISTMAS)
			{
				UnturnedLog.info("Loading additive Christmas scene");
				SceneManager.LoadSceneAsync("ChristmasMenu", LoadSceneMode.Additive);
				return;
			}
			if (activeHoliday == ENPCHoliday.HALLOWEEN)
			{
				UnturnedLog.info("Loading additive Halloween scene");
				SceneManager.LoadSceneAsync("HalloweenMenu", LoadSceneMode.Additive);
				return;
			}
			if (activeHoliday == ENPCHoliday.PRIDE_MONTH)
			{
				UnturnedLog.info("Loading additive Pride Month scene");
				SceneManager.LoadSceneAsync("PrideMonthMenu", LoadSceneMode.Additive);
				return;
			}
			UnturnedLog.info("Loading additive default menu");
			SceneManager.LoadSceneAsync("DefaultMenu", LoadSceneMode.Additive);
		}

		/// <summary>
		/// Prevents static member from being initialized during MonoBehaviour construction. (Unity warning)
		/// </summary>
		// Token: 0x02000993 RID: 2451
		private static class HelperClass
		{
			// Token: 0x040033B5 RID: 13237
			public static CommandLineString clAdditiveMenuOverride = new CommandLineString("-AdditiveMenuOverride");

			// Token: 0x040033B6 RID: 13238
			public static CommandLineFlag clNoAdditiveMenu = new CommandLineFlag(false, "-NoAdditiveMenu");
		}
	}
}
