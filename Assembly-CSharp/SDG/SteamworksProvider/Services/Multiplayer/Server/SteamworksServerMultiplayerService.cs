using System;
using System.IO;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider.Services.Community;
using SDG.Unturned;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Multiplayer.Server
{
	// Token: 0x0200001F RID: 31
	public class SteamworksServerMultiplayerService : Service, IServerMultiplayerService, IService
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003BCB File Offset: 0x00001DCB
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003BD3 File Offset: 0x00001DD3
		public IServerInfo serverInfo { get; protected set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003BDC File Offset: 0x00001DDC
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public bool isHosting { get; protected set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003BED File Offset: 0x00001DED
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003BF5 File Offset: 0x00001DF5
		public MemoryStream stream { get; protected set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003BFE File Offset: 0x00001DFE
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003C06 File Offset: 0x00001E06
		public BinaryReader reader { get; protected set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003C0F File Offset: 0x00001E0F
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003C17 File Offset: 0x00001E17
		public BinaryWriter writer { get; protected set; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000B1 RID: 177 RVA: 0x00003C20 File Offset: 0x00001E20
		// (remove) Token: 0x060000B2 RID: 178 RVA: 0x00003C58 File Offset: 0x00001E58
		public event ServerMultiplayerServiceReadyHandler ready;

		// Token: 0x060000B3 RID: 179 RVA: 0x00003C90 File Offset: 0x00001E90
		public void open(uint ip, ushort port, ESecurityMode security)
		{
			if (this.isHosting)
			{
				return;
			}
			EServerMode eServerMode = EServerMode.eServerModeInvalid;
			switch (security)
			{
			case ESecurityMode.LAN:
				eServerMode = EServerMode.eServerModeNoAuthentication;
				break;
			case ESecurityMode.SECURE:
				eServerMode = EServerMode.eServerModeAuthenticationAndSecure;
				break;
			case ESecurityMode.INSECURE:
				eServerMode = EServerMode.eServerModeAuthentication;
				break;
			}
			if (!GameServer.Init(ip, port, port, eServerMode, "1.0.0.0"))
			{
				throw new Exception("GameServer API initialization failed!");
			}
			SteamGameServer.SetDedicatedServer(this.appInfo.isDedicated);
			SteamGameServer.SetProduct(this.appInfo.name);
			SteamGameServer.SetModDir(this.appInfo.name);
			if (Provider.configData.Server.Use_FakeIP)
			{
				if (!SteamGameServerNetworkingSockets.BeginAsyncRequestFakeIP(1))
				{
					CommandWindow.LogError("Fatal: BeginAsyncRequestFakeIP returned false");
					throw new NotSupportedException("BeginAsyncRequestFakeIP returned false");
				}
				CommandWindow.Log("Requesting \"FakeIP\" from Steam");
			}
			if (SteamworksServerMultiplayerService.clShouldLogin)
			{
				string loginToken = CommandGSLT.loginToken;
				string text = (loginToken != null) ? loginToken.Trim() : null;
				if (string.IsNullOrEmpty(text))
				{
					string login_Token = Provider.configData.Browser.Login_Token;
					text = ((login_Token != null) ? login_Token.Trim() : null);
				}
				if (string.IsNullOrEmpty(text))
				{
					UnturnedLog.info("Not using login token");
					if (security != ESecurityMode.LAN)
					{
						Level.onPostLevelLoaded = (PostLevelLoaded)Delegate.Combine(Level.onPostLevelLoaded, new PostLevelLoaded(this.OnPostLevelLoaded));
					}
					SteamGameServer.LogOnAnonymous();
				}
				else
				{
					if (text.Length == 32)
					{
						UnturnedLog.info("Using login token");
					}
					else
					{
						UnturnedLog.warn("Using login token, but it does not seem to be correctly formatted");
					}
					SteamGameServer.LogOn(text);
				}
			}
			else
			{
				UnturnedLog.info("Skipping Steam game server login");
			}
			if (SteamworksServerMultiplayerService.clShouldEnableAdvertisement)
			{
				SteamGameServer.SetAdvertiseServerActive(true);
			}
			else
			{
				UnturnedLog.info("Not enabling Steam game server advertisement");
			}
			this.isHosting = true;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003E27 File Offset: 0x00002027
		public void close()
		{
			if (!this.isHosting)
			{
				return;
			}
			SteamGameServer.SetAdvertiseServerActive(false);
			SteamGameServer.LogOff();
			GameServer.Shutdown();
			this.isHosting = false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003E49 File Offset: 0x00002049
		public bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel)
		{
			entity = SteamworksCommunityEntity.INVALID;
			length = 0UL;
			return false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003E5C File Offset: 0x0000205C
		public void write(ICommunityEntity entity, byte[] data, ulong length)
		{
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003E5E File Offset: 0x0000205E
		public void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel)
		{
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003E60 File Offset: 0x00002060
		public SteamworksServerMultiplayerService(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			SteamworksServerMultiplayerService.steamServerConnectFailure = Callback<SteamServerConnectFailure_t>.CreateGameServer(new Callback<SteamServerConnectFailure_t>.DispatchDelegate(this.onSteamServerConnectFailure));
			SteamworksServerMultiplayerService.steamServersConnected = Callback<SteamServersConnected_t>.CreateGameServer(new Callback<SteamServersConnected_t>.DispatchDelegate(this.onSteamServersConnected));
			SteamworksServerMultiplayerService.steamServersDisconnected = Callback<SteamServersDisconnected_t>.CreateGameServer(new Callback<SteamServersDisconnected_t>.DispatchDelegate(this.onSteamServersDisconnected));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003EBC File Offset: 0x000020BC
		private void onSteamServerConnectFailure(SteamServerConnectFailure_t callback)
		{
			if (Dedicator.offlineOnly)
			{
				return;
			}
			if (callback.m_bStillRetrying)
			{
				CommandWindow.LogFormat("Failed to connect to Steam servers because {0}, still retrying", new object[]
				{
					callback.m_eResult
				});
			}
			else
			{
				CommandWindow.LogFormat("Failed to connect to Steam servers because {0}, no longer retrying", new object[]
				{
					callback.m_eResult
				});
			}
			if (callback.m_eResult == EResult.k_EResultInvalidParam || callback.m_eResult == EResult.k_EResultAccountNotFound)
			{
				CommandWindow.LogWarning(string.Format("{0} probably means Game Server Login Token (GSLT) is invalid", callback.m_eResult));
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003F49 File Offset: 0x00002149
		private void onSteamServersConnected(SteamServersConnected_t callback)
		{
			ServerMultiplayerServiceReadyHandler serverMultiplayerServiceReadyHandler = this.ready;
			if (serverMultiplayerServiceReadyHandler == null)
			{
				return;
			}
			serverMultiplayerServiceReadyHandler();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003F5B File Offset: 0x0000215B
		private void onSteamServersDisconnected(SteamServersDisconnected_t callback)
		{
			if (Dedicator.offlineOnly)
			{
				return;
			}
			CommandWindow.LogFormat("Lost connection to Steam servers because {0}", new object[]
			{
				callback.m_eResult
			});
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003F88 File Offset: 0x00002188
		private void OnPostLevelLoaded(int id)
		{
			CommandWindow.LogWarning("Steam Game Server Login Token (GSLT) not set");
			CommandWindow.LogWarning("Without a login token the server:");
			CommandWindow.LogWarning("- Is not visible in Internet server list");
			CommandWindow.LogWarning("- Cannot be joined over the Internet");
			CommandWindow.LogWarning("See this link for guide and more information:");
			CommandWindow.LogWarning("https://docs.smartlydressedgames.com/en/stable/servers/game-server-login-tokens.html");
		}

		// Token: 0x04000052 RID: 82
		private SteamworksAppInfo appInfo;

		// Token: 0x04000053 RID: 83
		private static Callback<SteamServerConnectFailure_t> steamServerConnectFailure;

		// Token: 0x04000054 RID: 84
		private static Callback<SteamServersConnected_t> steamServersConnected;

		// Token: 0x04000055 RID: 85
		private static Callback<SteamServersDisconnected_t> steamServersDisconnected;

		// Token: 0x04000056 RID: 86
		private static CommandLineFlag clShouldLogin = new CommandLineFlag(true, "-NoSteamGameServerLogin");

		// Token: 0x04000057 RID: 87
		private static CommandLineFlag clShouldEnableAdvertisement = new CommandLineFlag(true, "-NoSteamGameServerAdvertisement");
	}
}
