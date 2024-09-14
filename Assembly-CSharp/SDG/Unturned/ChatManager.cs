using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200054F RID: 1359
	public class ChatManager : SteamCaller
	{
		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06002AF9 RID: 11001 RVA: 0x000B730C File Offset: 0x000B550C
		// (remove) Token: 0x06002AFA RID: 11002 RVA: 0x000B7340 File Offset: 0x000B5540
		public static event ChatManager.ClientUnityEventPermissionsHandler onCheckUnityEventPermissions;

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x000B7373 File Offset: 0x000B5573
		public static ChatManager instance
		{
			get
			{
				return ChatManager.manager;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000B737A File Offset: 0x000B557A
		public static List<ReceivedChatMessage> receivedChatHistory
		{
			get
			{
				return ChatManager._receivedChatHistory;
			}
		}

		/// <summary>
		/// Add a newly received chat message to the front of the list,
		/// and remove an old message if necessary.
		/// </summary>
		// Token: 0x06002AFD RID: 11005 RVA: 0x000B7384 File Offset: 0x000B5584
		public static void receiveChatMessage(CSteamID speakerSteamID, string iconURL, EChatMode mode, Color color, bool isRich, string text)
		{
			text = text.Trim();
			ControlsSettings.formatPluginHotkeysIntoText(ref text);
			ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref text);
			if (OptionsSettings.streamer)
			{
				color = Color.white;
			}
			SteamPlayer steamPlayer;
			if (speakerSteamID == CSteamID.Nil)
			{
				steamPlayer = null;
			}
			else
			{
				if (!OptionsSettings.chatText && speakerSteamID != Provider.client)
				{
					return;
				}
				steamPlayer = PlayerTool.getSteamPlayer(speakerSteamID);
				if (steamPlayer.isTextChatLocallyMuted)
				{
					return;
				}
			}
			ReceivedChatMessage receivedChatMessage = new ReceivedChatMessage(steamPlayer, iconURL, mode, color, isRich, text);
			ChatManager.receivedChatHistory.Insert(0, receivedChatMessage);
			if (ChatManager.receivedChatHistory.Count > Provider.preferenceData.Chat.History_Length)
			{
				ChatManager.receivedChatHistory.RemoveAt(ChatManager.receivedChatHistory.Count - 1);
			}
			ChatMessageReceivedHandler chatMessageReceivedHandler = ChatManager.onChatMessageReceived;
			if (chatMessageReceivedHandler == null)
			{
				return;
			}
			chatMessageReceivedHandler();
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000B744C File Offset: 0x000B564C
		public static bool process(SteamPlayer player, string cmd)
		{
			return ChatManager.process(player, cmd, false);
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000B7458 File Offset: 0x000B5658
		public static bool process(SteamPlayer player, string cmd, bool fromUnityEvent)
		{
			bool flag = false;
			bool result = true;
			string text = cmd.Substring(0, 1);
			if (text == "@" || text == "/")
			{
				if (player.isAdmin)
				{
					flag = true;
					result = false;
				}
				if (fromUnityEvent && !Provider.configData.UnityEvents.Allow_Client_Commands)
				{
					flag = false;
					result = false;
				}
			}
			CheckPermissions checkPermissions = ChatManager.onCheckPermissions;
			if (checkPermissions != null)
			{
				checkPermissions(player, cmd, ref flag, ref result);
			}
			if (fromUnityEvent)
			{
				ChatManager.ClientUnityEventPermissionsHandler clientUnityEventPermissionsHandler = ChatManager.onCheckUnityEventPermissions;
				if (clientUnityEventPermissionsHandler != null)
				{
					clientUnityEventPermissionsHandler(player, cmd, ref flag, ref result);
				}
			}
			if (flag)
			{
				Commander.execute(player.playerID.steamID, cmd.Substring(1));
			}
			return result;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000B74FB File Offset: 0x000B56FB
		[Obsolete]
		public void tellVoteStart(CSteamID steamID, CSteamID origin, CSteamID target, byte votesNeeded)
		{
			ChatManager.ReceiveVoteStart(origin, target, votesNeeded);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000B7508 File Offset: 0x000B5708
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVoteStart")]
		public static void ReceiveVoteStart(CSteamID origin, CSteamID target, byte votesNeeded)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(origin);
			if (steamPlayer == null)
			{
				return;
			}
			SteamPlayer steamPlayer2 = PlayerTool.getSteamPlayer(target);
			if (steamPlayer2 == null)
			{
				return;
			}
			ChatManager.needsVote = true;
			ChatManager.hasVote = false;
			VotingStart votingStart = ChatManager.onVotingStart;
			if (votingStart == null)
			{
				return;
			}
			votingStart(steamPlayer, steamPlayer2, votesNeeded);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000B7549 File Offset: 0x000B5749
		[Obsolete]
		public void tellVoteUpdate(CSteamID steamID, byte voteYes, byte voteNo)
		{
			ChatManager.ReceiveVoteUpdate(voteYes, voteNo);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000B7552 File Offset: 0x000B5752
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVoteMessage")]
		public static void ReceiveVoteUpdate(byte voteYes, byte voteNo)
		{
			VotingUpdate votingUpdate = ChatManager.onVotingUpdate;
			if (votingUpdate == null)
			{
				return;
			}
			votingUpdate(voteYes, voteNo);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000B7565 File Offset: 0x000B5765
		[Obsolete]
		public void tellVoteStop(CSteamID steamID, byte message)
		{
			ChatManager.ReceiveVoteStop((EVotingMessage)message);
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000B756D File Offset: 0x000B576D
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVoteStop")]
		public static void ReceiveVoteStop(EVotingMessage message)
		{
			ChatManager.needsVote = false;
			VotingStop votingStop = ChatManager.onVotingStop;
			if (votingStop == null)
			{
				return;
			}
			votingStop(message);
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000B7585 File Offset: 0x000B5785
		[Obsolete]
		public void tellVoteMessage(CSteamID steamID, byte message)
		{
			ChatManager.ReceiveVoteMessage((EVotingMessage)message);
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000B758D File Offset: 0x000B578D
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVoteMessage")]
		public static void ReceiveVoteMessage(EVotingMessage message)
		{
			VotingMessage votingMessage = ChatManager.onVotingMessage;
			if (votingMessage == null)
			{
				return;
			}
			votingMessage(message);
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000B75A0 File Offset: 0x000B57A0
		[Obsolete]
		public void askVote(CSteamID steamID, bool vote)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ChatManager.ReceiveSubmitVoteRequest(serverInvocationContext, vote);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000B75BC File Offset: 0x000B57BC
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askVote")]
		public static void ReceiveSubmitVoteRequest(in ServerInvocationContext context, bool vote)
		{
			SteamPlayer callingPlayer = context.GetCallingPlayer();
			if (callingPlayer == null)
			{
				return;
			}
			if (!ChatManager.isVoting)
			{
				return;
			}
			if (ChatManager.votes.Contains(callingPlayer.playerID.steamID))
			{
				return;
			}
			ChatManager.votes.Add(callingPlayer.playerID.steamID);
			if (vote)
			{
				ChatManager.voteYes += 1;
			}
			else
			{
				ChatManager.voteNo += 1;
			}
			ChatManager.SendVoteUpdate.Invoke(ENetReliability.Reliable, Provider.GatherClientConnections(), ChatManager.voteYes, ChatManager.voteNo);
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000B7644 File Offset: 0x000B5844
		[Obsolete]
		public void askCallVote(CSteamID steamID, CSteamID target)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ChatManager.ReceiveCallVoteRequest(serverInvocationContext, target);
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000B7660 File Offset: 0x000B5860
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askCallVote")]
		public static void ReceiveCallVoteRequest(in ServerInvocationContext context, CSteamID target)
		{
			if (ChatManager.isVoting)
			{
				return;
			}
			SteamPlayer callingPlayer = context.GetCallingPlayer();
			if (callingPlayer == null || Time.realtimeSinceStartup < callingPlayer.nextVote)
			{
				ChatManager.SendVoteMessage.Invoke(ENetReliability.Reliable, context.GetTransportConnection(), EVotingMessage.DELAY);
				return;
			}
			if (!ChatManager.voteAllowed)
			{
				ChatManager.SendVoteMessage.Invoke(ENetReliability.Reliable, context.GetTransportConnection(), EVotingMessage.OFF);
				return;
			}
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(target);
			if (steamPlayer == null || steamPlayer.isAdmin)
			{
				return;
			}
			if (Provider.clients.Count < (int)ChatManager.votePlayers)
			{
				ChatManager.SendVoteMessage.Invoke(ENetReliability.Reliable, context.GetTransportConnection(), EVotingMessage.PLAYERS);
				return;
			}
			CommandWindow.Log(Provider.localization.format("Vote_Kick", new object[]
			{
				callingPlayer.playerID.characterName,
				callingPlayer.playerID.playerName,
				steamPlayer.playerID.characterName,
				steamPlayer.playerID.playerName
			}));
			ChatManager.lastVote = Time.realtimeSinceStartup;
			ChatManager.isVoting = true;
			ChatManager.voteYes = 0;
			ChatManager.voteNo = 0;
			ChatManager.votesPossible = (byte)Provider.clients.Count;
			ChatManager.votesNeeded = (byte)Mathf.Ceil((float)ChatManager.votesPossible * ChatManager.votePercentage);
			ChatManager.voteOrigin = callingPlayer;
			ChatManager.voteTarget = target;
			ChatManager.votes = new List<CSteamID>();
			ChatManager.voteIP = steamPlayer.getIPv4AddressOrZero();
			ChatManager.SendVoteStart.Invoke(ENetReliability.Reliable, Provider.GatherClientConnections(), callingPlayer.playerID.steamID, target, ChatManager.votesNeeded);
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000B77C7 File Offset: 0x000B59C7
		public static void sendVote(bool vote)
		{
			ChatManager.SendSubmitVoteRequest.Invoke(ENetReliability.Reliable, vote);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000B77D5 File Offset: 0x000B59D5
		public static void sendCallVote(CSteamID target)
		{
			ChatManager.SendCallVoteRequest.Invoke(ENetReliability.Unreliable, target);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000B77E3 File Offset: 0x000B59E3
		[Obsolete]
		public void tellChat(CSteamID steamID, CSteamID owner, string iconURL, byte mode, Color color, bool rich, string text)
		{
			ChatManager.ReceiveChatEntry(owner, iconURL, (EChatMode)mode, color, rich, text);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000B77F4 File Offset: 0x000B59F4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellChat")]
		public static void ReceiveChatEntry(CSteamID owner, string iconURL, EChatMode mode, Color color, bool rich, string text)
		{
			ChatManager.receiveChatMessage(owner, iconURL, mode, color, rich, text);
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000B7804 File Offset: 0x000B5A04
		[Obsolete]
		public void askChat(CSteamID steamID, byte flags, string text)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ChatManager.ReceiveChatRequest(serverInvocationContext, flags, text);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000B7824 File Offset: 0x000B5A24
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 15, legacyName = "askChat")]
		public static void ReceiveChatRequest(in ServerInvocationContext context, byte flags, string text)
		{
			SteamPlayer callingPlayer = context.GetCallingPlayer();
			if (callingPlayer == null || callingPlayer.player == null)
			{
				return;
			}
			if (Time.realtimeSinceStartup - callingPlayer.lastChat < ChatManager.chatrate)
			{
				return;
			}
			callingPlayer.lastChat = Time.realtimeSinceStartup;
			EChatMode echatMode = (EChatMode)(flags & 127);
			bool flag = (flags & 128) > 0;
			if (text.Length < 2)
			{
				return;
			}
			if (flag && !Provider.configData.UnityEvents.Allow_Client_Messages)
			{
				return;
			}
			text = text.Trim();
			if (text.Length < 2)
			{
				return;
			}
			if (StringExtension.ContainsNewLine(text))
			{
				return;
			}
			if (text.Length > ChatManager.MAX_MESSAGE_LENGTH)
			{
				text = text.Substring(0, ChatManager.MAX_MESSAGE_LENGTH);
			}
			if (echatMode == EChatMode.GLOBAL)
			{
				if (CommandWindow.shouldLogChat)
				{
					CommandWindow.Log(Provider.localization.format("Global", callingPlayer.playerID.characterName, callingPlayer.playerID.playerName, text));
				}
			}
			else if (echatMode == EChatMode.LOCAL)
			{
				if (CommandWindow.shouldLogChat)
				{
					CommandWindow.Log(Provider.localization.format("Local", callingPlayer.playerID.characterName, callingPlayer.playerID.playerName, text));
				}
			}
			else
			{
				if (echatMode != EChatMode.GROUP)
				{
					return;
				}
				if (CommandWindow.shouldLogChat)
				{
					CommandWindow.Log(Provider.localization.format("Group", callingPlayer.playerID.characterName, callingPlayer.playerID.playerName, text));
				}
			}
			if (flag)
			{
				UnturnedLog.info("UnityEventMsg {0}: '{1}'", new object[]
				{
					callingPlayer.playerID.steamID,
					text
				});
			}
			Color color = Color.white;
			if (callingPlayer.isAdmin && !Provider.hideAdmins)
			{
				color = Palette.ADMIN;
			}
			else if (callingPlayer.isPro)
			{
				color = Palette.PRO;
			}
			bool useRichTextFormatting = false;
			bool flag2 = true;
			Chatted chatted = ChatManager.onChatted;
			if (chatted != null)
			{
				chatted(callingPlayer, echatMode, ref color, ref useRichTextFormatting, text, ref flag2);
			}
			if (ProfanityFilter.NaiveContainsHardcodedBannedWord(text))
			{
				return;
			}
			if (ChatManager.process(callingPlayer, text, flag) && flag2)
			{
				if (ChatManager.onServerFormattingMessage != null)
				{
					ChatManager.onServerFormattingMessage(callingPlayer, echatMode, ref text);
				}
				else
				{
					text = "%SPEAKER%: " + text;
					if (echatMode != EChatMode.LOCAL)
					{
						if (echatMode == EChatMode.GROUP)
						{
							text = "[G] " + text;
						}
					}
					else
					{
						text = "[A] " + text;
					}
				}
				if (echatMode == EChatMode.GLOBAL)
				{
					ChatManager.serverSendMessage(text, color, callingPlayer, null, EChatMode.GLOBAL, null, useRichTextFormatting);
					return;
				}
				if (echatMode == EChatMode.LOCAL)
				{
					float num = 16384f;
					using (List<SteamPlayer>.Enumerator enumerator = Provider.clients.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SteamPlayer steamPlayer = enumerator.Current;
							if (!(steamPlayer.player == null) && (steamPlayer.player.transform.position - callingPlayer.player.transform.position).sqrMagnitude < num)
							{
								ChatManager.serverSendMessage(text, color, callingPlayer, steamPlayer, EChatMode.LOCAL, null, useRichTextFormatting);
							}
						}
						return;
					}
				}
				if (echatMode == EChatMode.GROUP && callingPlayer.player.quests.groupID != CSteamID.Nil)
				{
					foreach (SteamPlayer steamPlayer2 in Provider.clients)
					{
						if (!(steamPlayer2.player == null) && steamPlayer2.player.quests.isMemberOfSameGroupAs(callingPlayer.player))
						{
							ChatManager.serverSendMessage(text, color, callingPlayer, steamPlayer2, EChatMode.GROUP, null, useRichTextFormatting);
						}
					}
				}
			}
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000B7B98 File Offset: 0x000B5D98
		public static string getRecentlySentMessage(int index)
		{
			if (index >= 0 && index < ChatManager.recentlySentMessages.Length)
			{
				return ChatManager.recentlySentMessages[index];
			}
			return string.Empty;
		}

		/// <summary>
		/// Send a request to chat from the client to the server.
		/// </summary>
		// Token: 0x06002B13 RID: 11027 RVA: 0x000B7BB8 File Offset: 0x000B5DB8
		public static void sendChat(EChatMode mode, string text)
		{
			for (int i = ChatManager.recentlySentMessages.Length - 1; i > 0; i--)
			{
				ChatManager.recentlySentMessages[i] = ChatManager.recentlySentMessages[i - 1];
			}
			ChatManager.recentlySentMessages[0] = text;
			ChatManager.SendChatRequest.Invoke(ENetReliability.Reliable, (byte)mode, text);
		}

		/// <summary>
		/// Allows Unity events to send text chat messages from the client, for example to execute commands.
		/// Messenger context is logged to help track down abusive assets.
		/// </summary>
		// Token: 0x06002B14 RID: 11028 RVA: 0x000B7C00 File Offset: 0x000B5E00
		public static void clientSendMessage_UnityEvent(EChatMode mode, string text, ClientTextChatMessenger messenger)
		{
			if (messenger == null)
			{
				throw new ArgumentNullException("messenger");
			}
			UnturnedLog.info("UnityEventMsg {0}: '{1}'", new object[]
			{
				messenger.gameObject.GetSceneHierarchyPath(),
				text
			});
			ChatManager.SendChatRequest.Invoke(ENetReliability.Reliable, (byte)(mode | (EChatMode)128), text);
		}

		/// <summary>
		/// Allows Unity events to broadcast text chat messages from the server.
		/// Messenger context is logged to help track down abusive assets.
		/// </summary>
		// Token: 0x06002B15 RID: 11029 RVA: 0x000B7C58 File Offset: 0x000B5E58
		public static void serverSendMessage_UnityEvent(string text, Color color, string iconURL, bool useRichTextFormatting, ServerTextChatMessenger messenger)
		{
			if (messenger == null)
			{
				throw new ArgumentNullException("messenger");
			}
			if (!Provider.configData.UnityEvents.Allow_Server_Messages)
			{
				return;
			}
			UnturnedLog.info("UnityEventMsg {0}: '{1}'", new object[]
			{
				messenger.gameObject.GetSceneHierarchyPath(),
				text
			});
			ChatManager.serverSendMessage(text, color, null, null, EChatMode.SAY, iconURL, useRichTextFormatting);
		}

		/// <summary>
		/// Server send message to specific player.
		/// Used in vanilla for the welcome message.
		/// Should not be removed because plugins may depend on it.
		/// </summary>
		// Token: 0x06002B16 RID: 11030 RVA: 0x000B7CBB File Offset: 0x000B5EBB
		public static void say(CSteamID target, string text, Color color, bool isRich = false)
		{
			ChatManager.say(target, text, color, EChatMode.WELCOME, isRich);
		}

		/// <summary>
		/// Server send message to specific player.
		/// Used in vanilla by help command to tell player about command options.
		/// Should not be removed because plugins may depend on it.
		/// </summary>
		// Token: 0x06002B17 RID: 11031 RVA: 0x000B7CC8 File Offset: 0x000B5EC8
		public static void say(CSteamID target, string text, Color color, EChatMode mode, bool isRich = false)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(target);
			if (steamPlayer == null)
			{
				return;
			}
			ChatManager.serverSendMessage(text, color, null, steamPlayer, EChatMode.SAY, null, isRich);
		}

		/// <summary>
		/// Server send message to all players.
		/// Used in vanilla by some alerts and broadcast command.
		/// Should not be removed because plugins may depend on it.
		/// </summary>
		// Token: 0x06002B18 RID: 11032 RVA: 0x000B7CED File Offset: 0x000B5EED
		public static void say(string text, Color color, bool isRich = false)
		{
			ChatManager.serverSendMessage(text, color, null, null, EChatMode.SAY, null, isRich);
		}

		/// <summary>
		/// Serverside send a chat message to all players, or a specific player.
		/// </summary>
		/// <param name="text">Contents to display.</param>
		/// <param name="color">Default text color unless rich formatting overrides it.</param>
		/// <param name="fromPlayer">Player who sent the message (used for avatar), or null if send by a plugin.</param>
		/// <param name="toPlayer">Send message to only this player, or all players if null.</param>
		/// <param name="mode">Mostly deprecated, but global/local/group may be displayed.</param>
		/// <param name="iconURL">URL to a 32x32 .png to show rather than a player avatar, or null/empty.</param>
		/// <param name="useRichTextFormatting">Enable rich tags e.g., bold, italics in the message contents.</param>
		// Token: 0x06002B19 RID: 11033 RVA: 0x000B7CFC File Offset: 0x000B5EFC
		public static void serverSendMessage(string text, Color color, SteamPlayer fromPlayer = null, SteamPlayer toPlayer = null, EChatMode mode = EChatMode.SAY, string iconURL = null, bool useRichTextFormatting = false)
		{
			if (!Provider.isServer)
			{
				throw new Exception("Tried to send server message, but currently a client! Text: " + text);
			}
			ThreadUtil.assertIsGameThread();
			ServerSendingChatMessageHandler serverSendingChatMessageHandler = ChatManager.onServerSendingMessage;
			if (serverSendingChatMessageHandler != null)
			{
				serverSendingChatMessageHandler(ref text, ref color, fromPlayer, toPlayer, mode, ref iconURL, ref useRichTextFormatting);
			}
			if (fromPlayer != null && toPlayer != null)
			{
				string text2;
				if (!string.IsNullOrEmpty(fromPlayer.playerID.nickName) && fromPlayer != toPlayer && toPlayer.player != null && fromPlayer.player != null && fromPlayer.player.quests.isMemberOfSameGroupAs(toPlayer.player))
				{
					text2 = fromPlayer.playerID.nickName;
				}
				else
				{
					text2 = fromPlayer.playerID.characterName;
				}
				text = text.Replace("%SPEAKER%", text2);
			}
			if (iconURL == null)
			{
				iconURL = string.Empty;
			}
			CSteamID arg = (fromPlayer == null) ? CSteamID.Nil : fromPlayer.playerID.steamID;
			if (toPlayer == null)
			{
				using (List<SteamPlayer>.Enumerator enumerator = Provider.clients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SteamPlayer steamPlayer = enumerator.Current;
						if (steamPlayer != null)
						{
							ChatManager.serverSendMessage(text, color, fromPlayer, steamPlayer, mode, iconURL, useRichTextFormatting);
						}
					}
					return;
				}
			}
			ChatManager.SendChatEntry.Invoke(ENetReliability.Reliable, toPlayer.transportConnection, arg, iconURL, mode, color, useRichTextFormatting, text);
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000B7E48 File Offset: 0x000B6048
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				ChatManager.receivedChatHistory.Clear();
			}
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000B7E5C File Offset: 0x000B605C
		private void onServerConnected(CSteamID steamID)
		{
			if (Provider.isServer && ChatManager.welcomeText != "")
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
				ChatManager.say(steamPlayer.playerID.steamID, string.Format(ChatManager.welcomeText, steamPlayer.playerID.characterName), ChatManager.welcomeColor, false);
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000B7EB4 File Offset: 0x000B60B4
		private void Update()
		{
			if (ChatManager.isVoting && (Time.realtimeSinceStartup - ChatManager.lastVote > ChatManager.voteDuration || ChatManager.voteYes >= ChatManager.votesNeeded || ChatManager.voteNo > ChatManager.votesPossible - ChatManager.votesNeeded))
			{
				ChatManager.isVoting = false;
				if (ChatManager.voteYes >= ChatManager.votesNeeded)
				{
					if (ChatManager.voteOrigin != null)
					{
						ChatManager.voteOrigin.nextVote = Time.realtimeSinceStartup + ChatManager.votePassCooldown;
					}
					CommandWindow.Log(Provider.localization.format("Vote_Pass"));
					ChatManager.SendVoteStop.Invoke(ENetReliability.Reliable, Provider.GatherClientConnections(), EVotingMessage.PASS);
					SteamBlacklist.ban(ChatManager.voteTarget, ChatManager.voteIP, null, CSteamID.Nil, "you were vote kicked", SteamBlacklist.TEMPORARY);
				}
				else
				{
					if (ChatManager.voteOrigin != null)
					{
						ChatManager.voteOrigin.nextVote = Time.realtimeSinceStartup + ChatManager.voteFailCooldown;
					}
					CommandWindow.Log(Provider.localization.format("Vote_Fail"));
					ChatManager.SendVoteStop.Invoke(ENetReliability.Reliable, Provider.GatherClientConnections(), EVotingMessage.FAIL);
				}
			}
			if (ChatManager.needsVote && !ChatManager.hasVote)
			{
				if (InputEx.GetKeyDown(KeyCode.F1))
				{
					ChatManager.needsVote = false;
					ChatManager.hasVote = true;
					ChatManager.sendVote(true);
					return;
				}
				if (InputEx.GetKeyDown(KeyCode.F2))
				{
					ChatManager.needsVote = false;
					ChatManager.hasVote = true;
					ChatManager.sendVote(false);
				}
			}
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000B8000 File Offset: 0x000B6200
		private void Start()
		{
			ChatManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Provider.onServerConnected = (Provider.ServerConnected)Delegate.Combine(Provider.onServerConnected, new Provider.ServerConnected(this.onServerConnected));
		}

		// Token: 0x040016D3 RID: 5843
		public static readonly int MAX_MESSAGE_LENGTH = 512;

		/// <summary>
		/// Called on the client after a new message is inserted to the front of the list.
		/// </summary>
		// Token: 0x040016D4 RID: 5844
		public static ChatMessageReceivedHandler onChatMessageReceived;

		/// <summary>
		/// Called on the server when preparing a message to be sent to a player.
		/// Allows controlling how %SPEAKER% is formatted for the receiving player.
		/// </summary>
		// Token: 0x040016D5 RID: 5845
		public static ServerSendingChatMessageHandler onServerSendingMessage;

		/// <summary>
		/// Called on the server when formatting a player's message before sending to anyone.
		/// Allows structuring the message and where the player's name is, for example: '[CustomPluginRoleThing] %SPEAKER% - OriginalMessageText'
		/// </summary>
		// Token: 0x040016D6 RID: 5846
		public static ServerFormattingChatMessageHandler onServerFormattingMessage;

		// Token: 0x040016D7 RID: 5847
		public static Chatted onChatted;

		// Token: 0x040016D8 RID: 5848
		public static CheckPermissions onCheckPermissions;

		// Token: 0x040016D9 RID: 5849
		public static VotingStart onVotingStart;

		// Token: 0x040016DA RID: 5850
		public static VotingUpdate onVotingUpdate;

		// Token: 0x040016DB RID: 5851
		public static VotingStop onVotingStop;

		// Token: 0x040016DC RID: 5852
		public static VotingMessage onVotingMessage;

		// Token: 0x040016DE RID: 5854
		public static string welcomeText = "";

		// Token: 0x040016DF RID: 5855
		public static Color welcomeColor = Palette.SERVER;

		// Token: 0x040016E0 RID: 5856
		public static float chatrate = 0.25f;

		// Token: 0x040016E1 RID: 5857
		public static bool voteAllowed = false;

		// Token: 0x040016E2 RID: 5858
		public static float votePassCooldown = 5f;

		// Token: 0x040016E3 RID: 5859
		public static float voteFailCooldown = 60f;

		// Token: 0x040016E4 RID: 5860
		public static float voteDuration = 15f;

		// Token: 0x040016E5 RID: 5861
		public static float votePercentage = 0.75f;

		// Token: 0x040016E6 RID: 5862
		public static byte votePlayers = 3;

		// Token: 0x040016E7 RID: 5863
		private static float lastVote;

		// Token: 0x040016E8 RID: 5864
		private static bool isVoting;

		// Token: 0x040016E9 RID: 5865
		private static bool needsVote;

		// Token: 0x040016EA RID: 5866
		private static bool hasVote;

		// Token: 0x040016EB RID: 5867
		private static byte voteYes;

		// Token: 0x040016EC RID: 5868
		private static byte voteNo;

		// Token: 0x040016ED RID: 5869
		private static byte votesPossible;

		// Token: 0x040016EE RID: 5870
		private static byte votesNeeded;

		// Token: 0x040016EF RID: 5871
		private static SteamPlayer voteOrigin;

		// Token: 0x040016F0 RID: 5872
		private static CSteamID voteTarget;

		// Token: 0x040016F1 RID: 5873
		private static uint voteIP;

		// Token: 0x040016F2 RID: 5874
		private static List<CSteamID> votes;

		// Token: 0x040016F3 RID: 5875
		private static ChatManager manager;

		// Token: 0x040016F4 RID: 5876
		private static List<ReceivedChatMessage> _receivedChatHistory = new List<ReceivedChatMessage>();

		// Token: 0x040016F5 RID: 5877
		private static readonly ClientStaticMethod<CSteamID, CSteamID, byte> SendVoteStart = ClientStaticMethod<CSteamID, CSteamID, byte>.Get(new ClientStaticMethod<CSteamID, CSteamID, byte>.ReceiveDelegate(ChatManager.ReceiveVoteStart));

		// Token: 0x040016F6 RID: 5878
		private static readonly ClientStaticMethod<byte, byte> SendVoteUpdate = ClientStaticMethod<byte, byte>.Get(new ClientStaticMethod<byte, byte>.ReceiveDelegate(ChatManager.ReceiveVoteUpdate));

		// Token: 0x040016F7 RID: 5879
		private static readonly ClientStaticMethod<EVotingMessage> SendVoteStop = ClientStaticMethod<EVotingMessage>.Get(new ClientStaticMethod<EVotingMessage>.ReceiveDelegate(ChatManager.ReceiveVoteStop));

		// Token: 0x040016F8 RID: 5880
		private static readonly ClientStaticMethod<EVotingMessage> SendVoteMessage = ClientStaticMethod<EVotingMessage>.Get(new ClientStaticMethod<EVotingMessage>.ReceiveDelegate(ChatManager.ReceiveVoteMessage));

		// Token: 0x040016F9 RID: 5881
		private static readonly ServerStaticMethod<bool> SendSubmitVoteRequest = ServerStaticMethod<bool>.Get(new ServerStaticMethod<bool>.ReceiveDelegateWithContext(ChatManager.ReceiveSubmitVoteRequest));

		// Token: 0x040016FA RID: 5882
		private static readonly ServerStaticMethod<CSteamID> SendCallVoteRequest = ServerStaticMethod<CSteamID>.Get(new ServerStaticMethod<CSteamID>.ReceiveDelegateWithContext(ChatManager.ReceiveCallVoteRequest));

		// Token: 0x040016FB RID: 5883
		private static readonly ClientStaticMethod<CSteamID, string, EChatMode, Color, bool, string> SendChatEntry = ClientStaticMethod<CSteamID, string, EChatMode, Color, bool, string>.Get(new ClientStaticMethod<CSteamID, string, EChatMode, Color, bool, string>.ReceiveDelegate(ChatManager.ReceiveChatEntry));

		// Token: 0x040016FC RID: 5884
		private static readonly ServerStaticMethod<byte, string> SendChatRequest = ServerStaticMethod<byte, string>.Get(new ServerStaticMethod<byte, string>.ReceiveDelegateWithContext(ChatManager.ReceiveChatRequest));

		/// <summary>
		/// Previous messages sent to server from this client.
		/// Newest at the front, oldest at the back. Used to repeat chat commands.
		/// </summary>
		// Token: 0x040016FD RID: 5885
		private static string[] recentlySentMessages = new string[10];

		// Token: 0x02000971 RID: 2417
		// (Invoke) Token: 0x06004B61 RID: 19297
		public delegate void ClientUnityEventPermissionsHandler(SteamPlayer player, string command, ref bool shouldExecuteCommand, ref bool shouldList);
	}
}
