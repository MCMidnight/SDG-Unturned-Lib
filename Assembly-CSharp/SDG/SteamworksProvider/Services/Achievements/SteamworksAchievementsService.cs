using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Achievements;
using SDG.Unturned;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Achievements
{
	// Token: 0x0200002D RID: 45
	public class SteamworksAchievementsService : Service, IAchievementsService, IService
	{
		// Token: 0x06000113 RID: 275 RVA: 0x000048F7 File Offset: 0x00002AF7
		public bool getAchievement(string name, out bool has)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool achievement = SteamUserStats.GetAchievement(name, out has);
			if (!achievement)
			{
				UnturnedLog.error("Failed to get Steam achievement \"" + name + "\" status");
			}
			return achievement;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004928 File Offset: 0x00002B28
		public bool setAchievement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool flag = SteamUserStats.SetAchievement(name);
			if (flag)
			{
				UnturnedLog.info("Unlocked Steam achievement \"" + name + "\"");
			}
			else
			{
				UnturnedLog.error("Failed to unlock Steam achievement \"" + name + "\"");
			}
			SteamUserStats.StoreStats();
			return flag;
		}
	}
}
