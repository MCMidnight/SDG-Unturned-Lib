using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000675 RID: 1653
	public static class Lobbies
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060036CF RID: 14031 RVA: 0x00100FA0 File Offset: 0x000FF1A0
		public static bool canOpenInvitations
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x060036D0 RID: 14032 RVA: 0x00100FA8 File Offset: 0x000FF1A8
		// (remove) Token: 0x060036D1 RID: 14033 RVA: 0x00100FDC File Offset: 0x000FF1DC
		public static event LobbiesRefreshedHandler lobbiesRefreshed;

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060036D2 RID: 14034 RVA: 0x00101010 File Offset: 0x000FF210
		// (remove) Token: 0x060036D3 RID: 14035 RVA: 0x00101044 File Offset: 0x000FF244
		public static event LobbiesEnteredHandler lobbiesEntered;

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060036D4 RID: 14036 RVA: 0x00101077 File Offset: 0x000FF277
		// (set) Token: 0x060036D5 RID: 14037 RVA: 0x0010107E File Offset: 0x000FF27E
		public static bool isHost { get; private set; }

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060036D6 RID: 14038 RVA: 0x00101086 File Offset: 0x000FF286
		// (set) Token: 0x060036D7 RID: 14039 RVA: 0x0010108D File Offset: 0x000FF28D
		public static bool inLobby { get; private set; }

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060036D8 RID: 14040 RVA: 0x00101095 File Offset: 0x000FF295
		// (set) Token: 0x060036D9 RID: 14041 RVA: 0x0010109C File Offset: 0x000FF29C
		public static CSteamID currentLobby { get; private set; }

		// Token: 0x060036DA RID: 14042 RVA: 0x001010A4 File Offset: 0x000FF2A4
		private static void onLobbyCreated(LobbyCreated_t callback, bool io)
		{
			UnturnedLog.info("Lobby created: {0} {1} {2}", new object[]
			{
				callback.m_eResult,
				callback.m_ulSteamIDLobby,
				io
			});
			Lobbies.isHost = true;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x001010E4 File Offset: 0x000FF2E4
		private static void onLobbyEnter(LobbyEnter_t callback)
		{
			UnturnedLog.info("Lobby entered: {0} {1} {2} {3}", new object[]
			{
				callback.m_bLocked,
				callback.m_ulSteamIDLobby,
				callback.m_EChatRoomEnterResponse,
				callback.m_rgfChatPermissions
			});
			Lobbies.inLobby = true;
			Lobbies.currentLobby = new CSteamID(callback.m_ulSteamIDLobby);
			Lobbies.triggerLobbiesRefreshed();
			Lobbies.triggerLobbiesEntered();
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x0010115C File Offset: 0x000FF35C
		private static void onGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
		{
			UnturnedLog.info("Lobby join requested: {0} {1}", new object[]
			{
				callback.m_steamIDLobby,
				callback.m_steamIDFriend
			});
			if (Provider.isConnected)
			{
				return;
			}
			Lobbies.joinLobby(callback.m_steamIDLobby);
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x001011A8 File Offset: 0x000FF3A8
		private static void onPersonaStateChange(PersonaStateChange_t callback)
		{
			if (Lobbies.currentLobby == CSteamID.Nil)
			{
				return;
			}
			Lobbies.triggerLobbiesRefreshed();
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x001011C4 File Offset: 0x000FF3C4
		private static void onLobbyGameCreated(LobbyGameCreated_t callback)
		{
			UnturnedLog.info("Lobby game created: {0} {1} {2} {3}", new object[]
			{
				callback.m_ulSteamIDLobby,
				callback.m_unIP,
				callback.m_usPort,
				callback.m_ulSteamIDGameServer
			});
			Provider.provider.matchmakingService.connect(new SteamConnectionInfo(callback.m_unIP, callback.m_usPort, ""));
			Provider.provider.matchmakingService.autoJoinServerQuery = true;
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x00101250 File Offset: 0x000FF450
		private static void onLobbyChatUpdate(LobbyChatUpdate_t callback)
		{
			UnturnedLog.info("Lobby chat update: {0} {1} {2} {3}", new object[]
			{
				callback.m_ulSteamIDLobby,
				callback.m_ulSteamIDMakingChange,
				callback.m_ulSteamIDUserChanged,
				callback.m_rgfChatMemberStateChange
			});
			Lobbies.triggerLobbiesRefreshed();
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x001012AA File Offset: 0x000FF4AA
		private static void triggerLobbiesRefreshed()
		{
			Provider.updateRichPresence();
			LobbiesRefreshedHandler lobbiesRefreshedHandler = Lobbies.lobbiesRefreshed;
			if (lobbiesRefreshedHandler == null)
			{
				return;
			}
			lobbiesRefreshedHandler();
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x001012C0 File Offset: 0x000FF4C0
		private static void triggerLobbiesEntered()
		{
			LobbiesEnteredHandler lobbiesEnteredHandler = Lobbies.lobbiesEntered;
			if (lobbiesEnteredHandler == null)
			{
				return;
			}
			lobbiesEnteredHandler();
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x001012D4 File Offset: 0x000FF4D4
		public static void createLobby()
		{
			UnturnedLog.info("Create lobby");
			SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, 24);
			Lobbies.lobbyCreated.Set(hAPICall, null);
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x00101300 File Offset: 0x000FF500
		public static void joinLobby(CSteamID newLobby)
		{
			if (Lobbies.inLobby)
			{
				Lobbies.leaveLobby();
			}
			UnturnedLog.info("Join lobby: {0}", new object[]
			{
				newLobby
			});
			SteamMatchmaking.JoinLobby(newLobby);
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0010132E File Offset: 0x000FF52E
		public static void LinkLobby(uint ip, ushort queryPort)
		{
			if (!Lobbies.isHost)
			{
				return;
			}
			UnturnedLog.info("Link lobby: {0} {1}", new object[]
			{
				ip,
				queryPort
			});
			SteamMatchmaking.SetLobbyGameServer(Lobbies.currentLobby, ip, queryPort, CSteamID.Nil);
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x0010136B File Offset: 0x000FF56B
		public static void leaveLobby()
		{
			if (!Lobbies.inLobby)
			{
				return;
			}
			UnturnedLog.info("Leave lobby");
			Lobbies.isHost = false;
			Lobbies.inLobby = false;
			SteamMatchmaking.LeaveLobby(Lobbies.currentLobby);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x00101395 File Offset: 0x000FF595
		public static int getLobbyMemberCount()
		{
			return SteamMatchmaking.GetNumLobbyMembers(Lobbies.currentLobby);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x001013A1 File Offset: 0x000FF5A1
		public static CSteamID getLobbyMemberByIndex(int index)
		{
			return SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.currentLobby, index);
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x001013AE File Offset: 0x000FF5AE
		public static void openInvitations()
		{
			SteamFriends.ActivateGameOverlayInviteDialog(Lobbies.currentLobby);
		}

		// Token: 0x04002077 RID: 8311
		private static CallResult<LobbyCreated_t> lobbyCreated = CallResult<LobbyCreated_t>.Create(new CallResult<LobbyCreated_t>.APIDispatchDelegate(Lobbies.onLobbyCreated));

		// Token: 0x04002078 RID: 8312
		private static Callback<LobbyEnter_t> lobbyEnter = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(Lobbies.onLobbyEnter));

		// Token: 0x04002079 RID: 8313
		private static Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(Lobbies.onGameLobbyJoinRequested));

		// Token: 0x0400207A RID: 8314
		private static Callback<PersonaStateChange_t> personaStateChange = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(Lobbies.onPersonaStateChange));

		// Token: 0x0400207B RID: 8315
		private static Callback<LobbyGameCreated_t> lobbyGameCreated = Callback<LobbyGameCreated_t>.Create(new Callback<LobbyGameCreated_t>.DispatchDelegate(Lobbies.onLobbyGameCreated));

		// Token: 0x0400207C RID: 8316
		private static Callback<LobbyChatUpdate_t> lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(Lobbies.onLobbyChatUpdate));
	}
}
