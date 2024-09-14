using System;
using SDG.NetTransport;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows NPCs to trigger plugin or script events.
	/// </summary>
	// Token: 0x02000760 RID: 1888
	public class NPCEventManager
	{
		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x06003DBE RID: 15806 RVA: 0x00128C44 File Offset: 0x00126E44
		// (remove) Token: 0x06003DBF RID: 15807 RVA: 0x00128C78 File Offset: 0x00126E78
		[Obsolete("onEvent provides the instigating player.")]
		public static event NPCEventTriggeredHandler eventTriggered;

		/// <summary>
		/// instigatingPlayer can be null. For example, if instigated by NpcGlobalEventMessenger.
		/// </summary>
		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x06003DC0 RID: 15808 RVA: 0x00128CAC File Offset: 0x00126EAC
		// (remove) Token: 0x06003DC1 RID: 15809 RVA: 0x00128CE0 File Offset: 0x00126EE0
		public static event NPCEventHandler onEvent;

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00128D14 File Offset: 0x00126F14
		[Obsolete("broadcastEvent provides the instigating player.")]
		public static void triggerEventTriggered(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return;
			}
			NPCEventTriggeredHandler npceventTriggeredHandler = NPCEventManager.eventTriggered;
			if (npceventTriggeredHandler != null)
			{
				npceventTriggeredHandler(id);
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00128D3A File Offset: 0x00126F3A
		public static void broadcastEvent(Player instigatingPlayer, string eventId)
		{
			NPCEventManager.broadcastEvent(instigatingPlayer, eventId, false);
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x00128D44 File Offset: 0x00126F44
		public static void broadcastEvent(Player instigatingPlayer, string eventId, bool shouldReplicate = false)
		{
			if (string.IsNullOrEmpty(eventId))
			{
				return;
			}
			try
			{
				NPCEventHandler npceventHandler = NPCEventManager.onEvent;
				if (npceventHandler != null)
				{
					npceventHandler(instigatingPlayer, eventId);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception raised during server NPC event \"{0}\"", new object[]
				{
					eventId
				});
			}
			if (shouldReplicate)
			{
				byte arg;
				if (instigatingPlayer != null && instigatingPlayer.channel != null && instigatingPlayer.channel.owner != null)
				{
					arg = (byte)instigatingPlayer.channel.owner.channel;
				}
				else
				{
					arg = 0;
				}
				NPCEventManager.SendBroadcast.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), arg, eventId);
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00128DE4 File Offset: 0x00126FE4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveBroadcast(byte channelId, string eventId)
		{
			SteamPlayer steamPlayer = PlayerTool.findSteamPlayerByChannel((int)channelId);
			Player instigatingPlayer = (steamPlayer != null) ? steamPlayer.player : null;
			try
			{
				NPCEventHandler npceventHandler = NPCEventManager.onEvent;
				if (npceventHandler != null)
				{
					npceventHandler(instigatingPlayer, eventId);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception raised during client NPC event \"{0}\"", new object[]
				{
					eventId
				});
			}
		}

		// Token: 0x040026DE RID: 9950
		private static readonly ClientStaticMethod<byte, string> SendBroadcast = ClientStaticMethod<byte, string>.Get(new ClientStaticMethod<byte, string>.ReceiveDelegate(NPCEventManager.ReceiveBroadcast));
	}
}
