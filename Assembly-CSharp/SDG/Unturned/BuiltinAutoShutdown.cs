using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SDG.Unturned
{
	/// <summary>
	/// Manages scheduled restart for dedicated server.
	/// </summary>
	// Token: 0x02000666 RID: 1638
	internal class BuiltinAutoShutdown : MonoBehaviour
	{
		// Token: 0x060036AE RID: 13998 RVA: 0x001006CC File Offset: 0x000FE8CC
		private void InitScheduledShutdown()
		{
			if (!Provider.configData.Server.Enable_Scheduled_Shutdown)
			{
				return;
			}
			DateTime dateTime;
			if (!DateTime.TryParse(Provider.configData.Server.Scheduled_Shutdown_Time, ref dateTime))
			{
				CommandWindow.LogWarning("Unable to parse scheduled shutdown time \"" + Provider.configData.Server.Scheduled_Shutdown_Time + "\"");
				return;
			}
			this.isScheduledShutdownEnabled = true;
			DateTime utcNow = DateTime.UtcNow;
			this.scheduledShutdownTime = utcNow.Date + dateTime.ToUniversalTime().TimeOfDay;
			if (this.scheduledShutdownTime < utcNow)
			{
				this.scheduledShutdownTime = this.scheduledShutdownTime.AddDays(1.0);
			}
			TimeSpan timeSpan = this.scheduledShutdownTime - DateTime.UtcNow;
			CommandWindow.LogFormat(string.Format("Shutdown is scheduled for {0} ({1:g} from now)", this.scheduledShutdownTime.ToLocalTime(), timeSpan), Array.Empty<object>());
			this.scheduledShutdownRealtime = Time.realtimeSinceStartupAsDouble + timeSpan.TotalSeconds;
			this.scheduledShutdownWarnings = new List<double>(Provider.configData.Server.Scheduled_Shutdown_Warnings.Length);
			foreach (string text in Provider.configData.Server.Scheduled_Shutdown_Warnings)
			{
				TimeSpan timeSpan2;
				if (TimeSpan.TryParse(text, ref timeSpan2))
				{
					this.scheduledShutdownWarnings.Add(timeSpan2.TotalSeconds);
				}
				else
				{
					CommandWindow.LogWarning("Unable to parse scheduled shutdown warning time \"" + text + "\"");
				}
			}
			this.scheduledShutdownWarnings.Sort();
			if (this.scheduledShutdownWarnings.Count > 0)
			{
				double num = this.scheduledShutdownRealtime - Time.realtimeSinceStartupAsDouble;
				this.scheduledShutdownWarningIndex = this.scheduledShutdownWarnings.Count - 1;
				while (this.scheduledShutdownWarningIndex >= 0)
				{
					if (num > this.scheduledShutdownWarnings[this.scheduledShutdownWarningIndex])
					{
						return;
					}
					this.scheduledShutdownWarningIndex--;
				}
				return;
			}
			this.scheduledShutdownWarningIndex = -1;
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x001008B8 File Offset: 0x000FEAB8
		private void InitUpdateShutdown()
		{
			if (!Provider.configData.Server.Enable_Update_Shutdown)
			{
				return;
			}
			if (Dedicator.offlineOnly)
			{
				CommandWindow.LogWarning("Disabling check for game updates because Offline-Only mode is enabled");
				return;
			}
			string update_Steam_Beta_Name = Provider.configData.Server.Update_Steam_Beta_Name;
			if (string.IsNullOrEmpty(update_Steam_Beta_Name))
			{
				CommandWindow.LogWarning("Unable to check for game updates with empty Steam beta name (default is \"public\")");
				return;
			}
			this.updateShutdownWarnings = new List<double>(Provider.configData.Server.Update_Shutdown_Warnings.Length);
			foreach (string text in Provider.configData.Server.Update_Shutdown_Warnings)
			{
				TimeSpan timeSpan;
				if (TimeSpan.TryParse(text, ref timeSpan))
				{
					this.updateShutdownWarnings.Add(timeSpan.TotalSeconds);
				}
				else
				{
					CommandWindow.LogWarning("Unable to parse update shutdown warning time \"" + text + "\"");
				}
			}
			this.updateShutdownWarnings.Sort();
			CommandWindow.LogFormat("Monitoring for game updates on Steam beta branch \"" + update_Steam_Beta_Name + "\"", Array.Empty<object>());
			string url = "https://smartlydressedgames.com/unturned-steam-versions/" + update_Steam_Beta_Name + ".txt";
			base.StartCoroutine(this.CheckVersion(url));
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x001009C9 File Offset: 0x000FEBC9
		private void OnEnable()
		{
			this.InitScheduledShutdown();
			this.InitUpdateShutdown();
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x001009D8 File Offset: 0x000FEBD8
		private void Update()
		{
			if (this.isShuttingDownForUpdate)
			{
				double num = this.updateShutdownRealtime - Time.realtimeSinceStartupAsDouble;
				if (num < 0.0)
				{
					this.isShuttingDownForUpdate = false;
					string key = this.isUpdateRollback ? "RollbackShutdown_KickExplanation" : "UpdateShutdown_KickExplanation";
					Provider.shutdown(0, Provider.localization.format(key, this.updateVersionString));
					return;
				}
				if (this.updateShutdownWarnings.Count > 0 && this.updateShutdownWarningIndex >= 0 && num < this.updateShutdownWarnings[this.updateShutdownWarningIndex])
				{
					TimeSpan timeSpan;
					timeSpan..ctor(0, 0, (int)this.updateShutdownWarnings[this.updateShutdownWarningIndex]);
					string key2 = this.isUpdateRollback ? "RollbackShutdown_Timer" : "UpdateShutdown_Timer";
					string text = Provider.localization.format(key2, this.updateVersionString, timeSpan.ToString("g"));
					CommandWindow.Log(text);
					ChatManager.say(text, ChatManager.welcomeColor, false);
					this.updateShutdownWarningIndex--;
					return;
				}
			}
			else if (this.isScheduledShutdownEnabled)
			{
				double num2 = this.scheduledShutdownRealtime - Time.realtimeSinceStartupAsDouble;
				if (num2 < 0.0)
				{
					this.isScheduledShutdownEnabled = false;
					Provider.shutdown(0, Provider.localization.format("ScheduledMaintenance_KickExplanation"));
					return;
				}
				if (this.scheduledShutdownWarnings.Count > 0 && this.scheduledShutdownWarningIndex >= 0 && num2 < this.scheduledShutdownWarnings[this.scheduledShutdownWarningIndex])
				{
					TimeSpan timeSpan2;
					timeSpan2..ctor(0, 0, (int)this.scheduledShutdownWarnings[this.scheduledShutdownWarningIndex]);
					string text2 = Provider.localization.format("ScheduledMaintenance_Timer", timeSpan2.ToString("g"));
					CommandWindow.Log(text2);
					ChatManager.say(text2, ChatManager.welcomeColor, false);
					this.scheduledShutdownWarningIndex--;
				}
			}
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x00100BA2 File Offset: 0x000FEDA2
		private IEnumerator CheckVersion(string url)
		{
			yield return new WaitForSecondsRealtime(300f);
			for (;;)
			{
				UnturnedLog.info("Checking for game updates...");
				using (UnityWebRequest request = UnityWebRequest.Get(url))
				{
					request.timeout = 30;
					yield return request.SendWebRequest();
					if (request.result == 1)
					{
						string text = request.downloadHandler.text;
						uint num;
						if (Parser.TryGetUInt32FromIP(text, out num))
						{
							if (num != Provider.APP_VERSION_PACKED)
							{
								if (num > Provider.APP_VERSION_PACKED)
								{
									CommandWindow.Log("Detected newer game version: " + text);
								}
								else
								{
									CommandWindow.Log("Detected rollback to older game version: " + text);
								}
								bool flag = true;
								GameUpdateMonitor.NotifyGameUpdateDetected(text, ref flag);
								if (flag)
								{
									this.isShuttingDownForUpdate = true;
									this.isUpdateRollback = (num < Provider.APP_VERSION_PACKED);
									this.updateVersionString = text;
									this.updateShutdownWarningIndex = this.updateShutdownWarnings.Count - 1;
									this.updateShutdownRealtime = Time.realtimeSinceStartupAsDouble + ((this.updateShutdownWarningIndex >= 0) ? this.updateShutdownWarnings[this.updateShutdownWarningIndex] : 0.0);
								}
								yield break;
							}
							UnturnedLog.info("Game version is up-to-date");
						}
						else
						{
							UnturnedLog.info("Unable to parse newest game version \"" + text + "\"");
						}
					}
					else
					{
						UnturnedLog.info("Network error checking for game updates: \"" + request.error + "\"");
					}
					yield return new WaitForSecondsRealtime(600f);
				}
				UnityWebRequest request = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04001FA7 RID: 8103
		public bool isScheduledShutdownEnabled;

		// Token: 0x04001FA8 RID: 8104
		public DateTime scheduledShutdownTime;

		// Token: 0x04001FA9 RID: 8105
		private double scheduledShutdownRealtime;

		/// <summary>
		/// Sorted from low to high.
		/// </summary>
		// Token: 0x04001FAA RID: 8106
		private List<double> scheduledShutdownWarnings;

		// Token: 0x04001FAB RID: 8107
		private int scheduledShutdownWarningIndex = -1;

		// Token: 0x04001FAC RID: 8108
		private bool isShuttingDownForUpdate;

		// Token: 0x04001FAD RID: 8109
		private bool isUpdateRollback;

		// Token: 0x04001FAE RID: 8110
		private double updateShutdownRealtime;

		// Token: 0x04001FAF RID: 8111
		private string updateVersionString;

		/// <summary>
		/// Sorted from low to high.
		/// </summary>
		// Token: 0x04001FB0 RID: 8112
		private List<double> updateShutdownWarnings;

		// Token: 0x04001FB1 RID: 8113
		private int updateShutdownWarningIndex = -1;
	}
}
