using System;
using System.Collections.Generic;
using System.Globalization;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000764 RID: 1892
	public class PlayerTool
	{
		// Token: 0x06003DDB RID: 15835 RVA: 0x0012BA88 File Offset: 0x00129C88
		private static string getRepKey(int rep)
		{
			string result = "";
			if (rep <= -200)
			{
				result = "Villain";
			}
			else if (rep <= -100)
			{
				result = "Bandit";
			}
			else if (rep <= -33)
			{
				result = "Gangster";
			}
			else if (rep <= -8)
			{
				result = "Outlaw";
			}
			else if (rep < 0)
			{
				result = "Thug";
			}
			else if (rep >= 200)
			{
				result = "Paragon";
			}
			else if (rep >= 100)
			{
				result = "Sheriff";
			}
			else if (rep >= 33)
			{
				result = "Deputy";
			}
			else if (rep >= 8)
			{
				result = "Constable";
			}
			else if (rep > 0)
			{
				result = "Vigilante";
			}
			else if (rep == 0)
			{
				result = "Neutral";
			}
			return result;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0012BB2A File Offset: 0x00129D2A
		public static Texture2D getRepTexture(int rep)
		{
			return (Texture2D)Resources.Load("Reputation/" + PlayerTool.getRepKey(rep));
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0012BB46 File Offset: 0x00129D46
		public static string getRepTitle(int rep)
		{
			return PlayerDashboardInformationUI.localization.format("Rep", PlayerDashboardInformationUI.localization.format("Rep_" + PlayerTool.getRepKey(rep)), rep);
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0012BB78 File Offset: 0x00129D78
		public static Color getRepColor(int rep)
		{
			if (rep == 0)
			{
				return Color.white;
			}
			if (rep < 0)
			{
				float num = (float)Mathf.Min(Mathf.Abs(rep), 200) / 200f;
				if (num < 0.5f)
				{
					return Color.Lerp(Color.white, Palette.COLOR_Y, num * 2f);
				}
				return Color.Lerp(Palette.COLOR_Y, Palette.COLOR_R, (num - 0.5f) * 2f);
			}
			else
			{
				if (rep > 0)
				{
					float t = (float)Mathf.Min(Mathf.Abs(rep), 200) / 200f;
					return Color.Lerp(Color.white, Palette.COLOR_G, t);
				}
				return Color.white;
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0012BC18 File Offset: 0x00129E18
		public static void getPlayersInRadius(Vector3 center, float sqrRadius, List<Player> result)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				Player player = Provider.clients[i].player;
				if (!(player == null) && (player.transform.position - center).sqrMagnitude < sqrRadius)
				{
					result.Add(player);
				}
			}
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0012BC77 File Offset: 0x00129E77
		public static SteamPlayer[] getSteamPlayers()
		{
			return Provider.clients.ToArray();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0012BC84 File Offset: 0x00129E84
		public static SteamPlayer getSteamPlayer(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (NameTool.checkNames(name, Provider.clients[i].playerID.playerName) || NameTool.checkNames(name, Provider.clients[i].playerID.characterName))
				{
					return Provider.clients[i];
				}
			}
			return null;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x0012BCF8 File Offset: 0x00129EF8
		public static SteamPlayer getSteamPlayer(ulong steamID)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID.m_SteamID == steamID)
				{
					return Provider.clients[i];
				}
			}
			return null;
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0012BD44 File Offset: 0x00129F44
		public static SteamPlayer getSteamPlayer(CSteamID steamID)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID == steamID)
				{
					return Provider.clients[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Find client with given RPC channel ID.
		/// </summary>
		// Token: 0x06003DE4 RID: 15844 RVA: 0x0012BD90 File Offset: 0x00129F90
		public static SteamPlayer findSteamPlayerByChannel(int channel)
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer != null && steamPlayer.channel == channel)
				{
					return steamPlayer;
				}
			}
			return null;
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0012BDF0 File Offset: 0x00129FF0
		public static Transform getPlayerModel(CSteamID steamID)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
			if (steamPlayer != null && steamPlayer.model != null)
			{
				return steamPlayer.model;
			}
			return null;
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0012BE20 File Offset: 0x0012A020
		public static Player getPlayer(CSteamID steamID)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
			if (steamPlayer != null && steamPlayer.player != null)
			{
				return steamPlayer.player;
			}
			return null;
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x0012BE50 File Offset: 0x0012A050
		public static Transform getPlayerModel(string name)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(name);
			if (steamPlayer != null && steamPlayer.model != null)
			{
				return steamPlayer.model;
			}
			return null;
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0012BE80 File Offset: 0x0012A080
		public static Player getPlayer(string name)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(name);
			if (steamPlayer != null && steamPlayer.player != null)
			{
				return steamPlayer.player;
			}
			return null;
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0012BEB0 File Offset: 0x0012A0B0
		public static bool tryGetSteamPlayer(string input, out SteamPlayer player)
		{
			player = null;
			if (string.IsNullOrEmpty(input))
			{
				return false;
			}
			ulong steamID;
			if (ulong.TryParse(input, 511, CultureInfo.InvariantCulture, ref steamID))
			{
				player = PlayerTool.getSteamPlayer(steamID);
				return player != null;
			}
			player = PlayerTool.getSteamPlayer(input);
			return player != null;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0012BEFC File Offset: 0x0012A0FC
		public static bool tryGetSteamID(string input, out CSteamID steamID)
		{
			steamID = CSteamID.Nil;
			if (string.IsNullOrEmpty(input))
			{
				return false;
			}
			ulong ulSteamID;
			if (ulong.TryParse(input, 511, CultureInfo.InvariantCulture, ref ulSteamID))
			{
				steamID = new CSteamID(ulSteamID);
				return true;
			}
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(input);
			if (steamPlayer != null)
			{
				steamID = steamPlayer.playerID.steamID;
				return true;
			}
			return false;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0012BF5E File Offset: 0x0012A15E
		public static IEnumerable<Player> EnumeratePlayers()
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer.player != null)
				{
					yield return steamPlayer.player;
				}
			}
			List<SteamPlayer>.Enumerator enumerator = default(List<SteamPlayer>.Enumerator);
			yield break;
			yield break;
		}
	}
}
