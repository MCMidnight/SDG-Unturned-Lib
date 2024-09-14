using System;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

namespace SDG.Unturned
{
	// Token: 0x02000672 RID: 1650
	public static class GrantPackagePromo
	{
		/// <summary>
		/// Perform rate limiting and update timestamp.
		/// </summary>
		/// <returns>True if we can proceed with request.</returns>
		// Token: 0x060036C3 RID: 14019 RVA: 0x00100DE0 File Offset: 0x000FEFE0
		private static bool CheckRateLimit()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup - GrantPackagePromo.RequestRealtime < 1f)
			{
				return false;
			}
			GrantPackagePromo.RequestRealtime = realtimeSinceStartup;
			return true;
		}

		/// <summary>
		/// Do we think the local player is eligible to send request?
		/// </summary>
		// Token: 0x060036C4 RID: 14020 RVA: 0x00100E0C File Offset: 0x000FF00C
		public static bool IsEligible()
		{
			if (Provider.statusData == null || Provider.statusData.Game == null)
			{
				return false;
			}
			if (Provider.statusData.Game.GrantPackageIDs.Length < 1 || string.IsNullOrEmpty(Provider.statusData.Game.GrantPackageURL))
			{
				return false;
			}
			if (SteamApps.BIsSubscribedApp(new AppId_t(427840U)))
			{
				return false;
			}
			foreach (int item in Provider.statusData.Game.GrantPackageIDs)
			{
				if (Provider.provider.economyService.getInventoryPackage(item) > 0UL)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x00100EA8 File Offset: 0x000FF0A8
		public static void SendRequest()
		{
			if (!GrantPackagePromo.CheckRateLimit())
			{
				return;
			}
			if (!GrantPackagePromo.IsEligible())
			{
				return;
			}
			if (!Provider.allowWebRequests)
			{
				UnturnedLog.warn("Not granting package promo because web requests are disabled");
				return;
			}
			string text = Provider.statusData.Game.GrantPackageURL;
			text = string.Format(text, SteamUser.GetSteamID().m_SteamID);
			UnturnedLog.info("Grant package promo requested: '{0}'", new object[]
			{
				text
			});
			using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(text))
			{
				unityWebRequest.timeout = 15;
				UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
				while (!unityWebRequestAsyncOperation.isDone)
				{
				}
				if (unityWebRequest.result != 1)
				{
					UnturnedLog.warn("Grand package promo error: {0}", new object[]
					{
						unityWebRequest.error
					});
				}
				else
				{
					UnturnedLog.info("Response: '{0}'", new object[]
					{
						unityWebRequest.downloadHandler.text
					});
				}
			}
		}

		/// <summary>
		/// Last realtime a request was sent.
		/// Used to rate-limit clientside.
		/// </summary>
		// Token: 0x04002071 RID: 8305
		private static float RequestRealtime = -999f;
	}
}
