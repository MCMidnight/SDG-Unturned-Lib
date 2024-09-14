using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000590 RID: 1424
	public class SaveManager : SteamCaller
	{
		// Token: 0x06002D85 RID: 11653 RVA: 0x000C65CC File Offset: 0x000C47CC
		private static void broadcastPreSave()
		{
			try
			{
				SaveHandler saveHandler = SaveManager.onPreSave;
				if (saveHandler != null)
				{
					saveHandler();
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised exception during onPreSave:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000C660C File Offset: 0x000C480C
		private static void broadcastPostSave()
		{
			try
			{
				SaveHandler saveHandler = SaveManager.onPostSave;
				if (saveHandler != null)
				{
					saveHandler();
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised exception during onPostSave:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000C664C File Offset: 0x000C484C
		public static void save()
		{
			ThreadUtil.assertIsGameThread();
			if (!Level.isLoaded)
			{
				UnturnedLog.warn("Ignoring request to save before level finished loading");
				return;
			}
			SaveManager.broadcastPreSave();
			if (Level.info != null && Level.info.type == ELevelType.SURVIVAL)
			{
				foreach (SteamPlayer steamPlayer in Provider.clients)
				{
					if (steamPlayer != null && !(steamPlayer.player == null))
					{
						steamPlayer.player.save();
					}
				}
				VehicleManager.save();
				BarricadeManager.save();
				StructureManager.save();
				ObjectManager.save();
				LightingManager.save();
				GroupManager.save();
			}
			SteamWhitelist.save();
			SteamBlacklist.save();
			SteamAdminlist.save();
			SaveManager.broadcastPostSave();
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000C6714 File Offset: 0x000C4914
		private static void onServerShutdown()
		{
			if (Provider.isServer && Level.isLoaded)
			{
				UnturnedLog.info("Saving during server shutdown");
				SaveManager.save();
			}
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000C6734 File Offset: 0x000C4934
		private static void onServerDisconnected(CSteamID steamID)
		{
			if (Provider.isServer && Level.isLoaded)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player != null)
				{
					player.save();
				}
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000C6768 File Offset: 0x000C4968
		private void Start()
		{
			Provider.onServerShutdown = (Provider.ServerShutdown)Delegate.Combine(Provider.onServerShutdown, new Provider.ServerShutdown(SaveManager.onServerShutdown));
			Provider.onServerDisconnected = (Provider.ServerDisconnected)Delegate.Combine(Provider.onServerDisconnected, new Provider.ServerDisconnected(SaveManager.onServerDisconnected));
		}

		// Token: 0x0400188B RID: 6283
		public static SaveHandler onPreSave;

		// Token: 0x0400188C RID: 6284
		public static SaveHandler onPostSave;
	}
}
