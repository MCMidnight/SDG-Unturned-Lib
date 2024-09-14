using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.SteamworksProvider.Services.Community
{
	// Token: 0x0200002A RID: 42
	public class SteamworksCommunityService : Service, ICommunityService, IService
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00004624 File Offset: 0x00002824
		public void setStatus(string status)
		{
			SteamFriends.SetRichPresence("status", status);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004634 File Offset: 0x00002834
		public Texture2D getIcon(int id)
		{
			if (id < 0)
			{
				return null;
			}
			uint num;
			uint num2;
			if (!SteamUtils.GetImageSize(id, out num, out num2))
			{
				return null;
			}
			byte[] array = new byte[num * num2 * 4U];
			if (!SteamUtils.GetImageRGBA(id, array, array.Length))
			{
				return null;
			}
			int num3 = (int)num;
			int num4 = (int)num2;
			int num5 = num4 / 2;
			for (int i = 0; i < num5; i++)
			{
				int num6 = num4 - 1 - i;
				int num7 = i * num3 * 4;
				int num8 = num6 * num3 * 4;
				for (int j = 0; j < num3; j++)
				{
					int num9 = num7 + j * 4;
					int num10 = num8 + j * 4;
					for (int k = 0; k < 4; k++)
					{
						int num11 = num9 + k;
						int num12 = num10 + k;
						byte b = array[num11];
						array[num11] = array[num12];
						array[num12] = b;
					}
				}
			}
			Texture2D texture2D = new Texture2D(num3, num4, TextureFormat.RGBA32, false);
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			texture2D.LoadRawTextureData(array);
			texture2D.Apply(true, true);
			return texture2D;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004714 File Offset: 0x00002914
		public Texture2D getIcon(CSteamID steamID, bool shouldCache = false)
		{
			Texture2D texture2D = null;
			if (!shouldCache || !this.cachedAvatars.TryGetValue(steamID, ref texture2D))
			{
				texture2D = this.getIcon(SteamFriends.GetSmallFriendAvatar(steamID));
				if (shouldCache)
				{
					this.cachedAvatars.Add(steamID, texture2D);
				}
			}
			return texture2D;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004754 File Offset: 0x00002954
		public SteamGroup getCachedGroup(CSteamID steamID)
		{
			SteamGroup result;
			this.cachedGroups.TryGetValue(steamID, ref result);
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004774 File Offset: 0x00002974
		public SteamGroup[] getGroups()
		{
			SteamGroup[] array = new SteamGroup[SteamFriends.GetClanCount()];
			for (int i = 0; i < array.Length; i++)
			{
				CSteamID clanByIndex = SteamFriends.GetClanByIndex(i);
				SteamGroup steamGroup = this.getCachedGroup(clanByIndex);
				if (steamGroup == null)
				{
					string clanName = SteamFriends.GetClanName(clanByIndex);
					Texture2D icon = this.getIcon(clanByIndex, false);
					steamGroup = new SteamGroup(clanByIndex, clanName, icon);
					this.cachedGroups.Add(clanByIndex, steamGroup);
				}
				array[i] = steamGroup;
			}
			return array;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000047DC File Offset: 0x000029DC
		public bool checkGroup(CSteamID steamID)
		{
			for (int i = 0; i < SteamFriends.GetClanCount(); i++)
			{
				if (SteamFriends.GetClanByIndex(i) == steamID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000480A File Offset: 0x00002A0A
		public SteamworksCommunityService()
		{
			this.cachedGroups = new Dictionary<CSteamID, SteamGroup>();
			this.cachedAvatars = new Dictionary<CSteamID, Texture2D>();
		}

		// Token: 0x0400006E RID: 110
		private Dictionary<CSteamID, SteamGroup> cachedGroups;

		// Token: 0x0400006F RID: 111
		private Dictionary<CSteamID, Texture2D> cachedAvatars;
	}
}
