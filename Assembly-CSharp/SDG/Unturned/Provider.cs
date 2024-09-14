using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using BattlEye;
using SDG.Framework.Modules;
using SDG.HostBans;
using SDG.NetPak;
using SDG.NetTransport;
using SDG.Provider;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using Unturned.SystemEx;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x02000680 RID: 1664
	public class Provider : MonoBehaviour
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003717 RID: 14103 RVA: 0x00101D9F File Offset: 0x000FFF9F
		// (set) Token: 0x06003718 RID: 14104 RVA: 0x00101DA6 File Offset: 0x000FFFA6
		public static string APP_VERSION { get; protected set; }

		/// <summary>
		/// App version string packed into a 32-bit number for replication.
		/// </summary>
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003719 RID: 14105 RVA: 0x00101DAE File Offset: 0x000FFFAE
		// (set) Token: 0x0600371A RID: 14106 RVA: 0x00101DB5 File Offset: 0x000FFFB5
		public static uint APP_VERSION_PACKED { get; protected set; }

		// Token: 0x0600371B RID: 14107 RVA: 0x00101DBD File Offset: 0x000FFFBD
		private IEnumerator CaptureScreenshot()
		{
			bool enableScreenshotSupersampling = OptionsSettings.enableScreenshotSupersampling;
			int max = enableScreenshotSupersampling ? 4 : 16;
			int sizeMultiplier = Mathf.Clamp(OptionsSettings.screenshotSizeMultiplier, 1, max);
			int finalWidth = Screen.width * sizeMultiplier;
			int finalHeight = Screen.height * sizeMultiplier;
			int maxTextureSize = SystemInfo.maxTextureSize;
			if (finalWidth > maxTextureSize || finalHeight > maxTextureSize)
			{
				UnturnedLog.warn(string.Format("Unable to capture {0}x{1} screenshot because it exceeds max supported texture size ({2})", finalWidth, finalHeight, maxTextureSize));
				Provider.isCapturingScreenshot = false;
				yield break;
			}
			if (sizeMultiplier > 1 || enableScreenshotSupersampling)
			{
				UnturnedPostProcess.instance.DisableAntiAliasingForScreenshot = true;
			}
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Screenshots");
			Directory.CreateDirectory(text);
			bool flag;
			if (Level.isEditor && EditorUI.window != null)
			{
				flag = EditorUI.window.isEnabled;
			}
			else
			{
				flag = (!(Player.player != null) || PlayerUI.window == null || PlayerUI.window.isEnabled);
			}
			string text2 = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			if (!flag)
			{
				text2 += "_NoUI";
			}
			string filePath = Path.Combine(text, text2 + ".png");
			UnturnedLog.info(string.Format("Capturing {0}x{1} screenshot (Size Multiplier: {2} Use Supersampling: {3} HUD Visible: {4})", new object[]
			{
				finalWidth,
				finalHeight,
				sizeMultiplier,
				enableScreenshotSupersampling,
				flag
			}));
			if (!enableScreenshotSupersampling)
			{
				ScreenCapture.CaptureScreenshot(filePath, sizeMultiplier);
				yield return null;
				UnturnedPostProcess.instance.DisableAntiAliasingForScreenshot = false;
				float timePassed = 0f;
				for (;;)
				{
					timePassed += Time.deltaTime;
					if (File.Exists(filePath))
					{
						goto IL_45D;
					}
					if (timePassed >= 10f)
					{
						break;
					}
					yield return null;
				}
				UnturnedLog.error(string.Format("Screenshot file is not available after {0}s ({1})", timePassed, filePath));
				Provider.isCapturingScreenshot = false;
				yield break;
			}
			yield return new WaitForEndOfFrame();
			int num = sizeMultiplier * 2;
			Texture2D supersampledTexture = ScreenCapture.CaptureScreenshotAsTexture(num);
			UnturnedPostProcess.instance.DisableAntiAliasingForScreenshot = false;
			if (supersampledTexture == null)
			{
				UnturnedLog.error("CaptureScreenshotAsTexture returned null");
				Provider.isCapturingScreenshot = false;
				yield break;
			}
			yield return null;
			supersampledTexture.filterMode = FilterMode.Bilinear;
			RenderTexture downsampleRenderTexture = RenderTexture.GetTemporary(finalWidth, finalHeight, 0, supersampledTexture.graphicsFormat);
			Graphics.Blit(supersampledTexture, downsampleRenderTexture, Provider.screenshotBlitMaterial);
			yield return null;
			Texture2D downsampledTexture = new Texture2D(finalWidth, finalHeight, supersampledTexture.format, false, false);
			RenderTexture.active = downsampleRenderTexture;
			downsampledTexture.ReadPixels(new Rect(0f, 0f, (float)finalWidth, (float)finalHeight), 0, 0, false);
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(downsampleRenderTexture);
			Object.Destroy(supersampledTexture);
			yield return null;
			byte[] downsampledBytes = ImageConversion.EncodeToPNG(downsampledTexture);
			Object.Destroy(downsampledTexture);
			yield return null;
			File.WriteAllBytes(filePath, downsampledBytes);
			yield return null;
			supersampledTexture = null;
			downsampleRenderTexture = null;
			downsampledTexture = null;
			downsampledBytes = null;
			IL_45D:
			UnturnedLog.info("Captured screenshot: " + filePath);
			ScreenshotHandle hScreenshot = SteamScreenshots.AddScreenshotToLibrary(filePath, null, finalWidth, finalHeight);
			if (Level.info != null)
			{
				string localizedName = Level.info.getLocalizedName();
				SteamScreenshots.SetLocation(hScreenshot, localizedName);
				UnturnedLog.info("Tagged location \"" + localizedName + "\" in screenshot");
			}
			Camera instance = MainCamera.instance;
			if (instance != null)
			{
				Vector3 position = instance.transform.position;
				foreach (SteamPlayer steamPlayer in Provider.clients)
				{
					if (!(steamPlayer.player == null) && !steamPlayer.player.channel.IsLocalPlayer)
					{
						Vector3 vector = steamPlayer.player.transform.position + Vector3.up;
						if ((vector - position).sqrMagnitude <= 4096f)
						{
							Vector3 vector2 = instance.WorldToViewportPoint(vector);
							if (vector2.x >= 0f && vector2.x <= 1f && vector2.y >= 0f && vector2.y <= 1f && vector2.z >= 0f)
							{
								SteamScreenshots.TagUser(hScreenshot, steamPlayer.playerID.steamID);
								UnturnedLog.info("Tagged player \"" + steamPlayer.GetLocalDisplayName() + "\" in screenshot");
							}
						}
					}
				}
			}
			Provider.isCapturingScreenshot = false;
			yield break;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x00101DC5 File Offset: 0x000FFFC5
		public static void RequestScreenshot()
		{
			if (Provider.isCapturingScreenshot)
			{
				return;
			}
			Provider.isCapturingScreenshot = true;
			Provider.steam.StartCoroutine(Provider.steam.CaptureScreenshot());
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x00101DEA File Offset: 0x000FFFEA
		private static void OnSteamScreenshotRequested(ScreenshotRequested_t callback)
		{
			UnturnedLog.info("Steam overlay screenshot requested");
			Provider.RequestScreenshot();
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600371E RID: 14110 RVA: 0x00101DFB File Offset: 0x000FFFFB
		// (set) Token: 0x0600371F RID: 14111 RVA: 0x00101E02 File Offset: 0x00100002
		public static string language
		{
			get
			{
				return Provider.privateLanguage;
			}
			private set
			{
				Provider.privateLanguage = value;
				Provider.languageIsEnglish = (value == "English");
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003720 RID: 14112 RVA: 0x00101E1A File Offset: 0x0010001A
		public static string path
		{
			get
			{
				return Provider._path;
			}
		}

		/// <summary>
		/// Path to directory containing "Editor", "Menu", "Player", "Curse_Words.txt", etc files.
		/// </summary>
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003721 RID: 14113 RVA: 0x00101E21 File Offset: 0x00100021
		// (set) Token: 0x06003722 RID: 14114 RVA: 0x00101E28 File Offset: 0x00100028
		public static string localizationRoot { get; private set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003723 RID: 14115 RVA: 0x00101E30 File Offset: 0x00100030
		// (set) Token: 0x06003724 RID: 14116 RVA: 0x00101E37 File Offset: 0x00100037
		public static List<string> streamerNames { get; private set; }

		// Token: 0x06003725 RID: 14117 RVA: 0x00101E3F File Offset: 0x0010003F
		internal static void battlEyeClientPrintMessage(string message)
		{
			UnturnedLog.info("BattlEye client message: {0}", new object[]
			{
				message
			});
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x00101E55 File Offset: 0x00100055
		internal static void battlEyeClientRequestRestart(int reason)
		{
			if (reason == 0)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BATTLEYE_BROKEN;
			}
			else if (reason == 1)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BATTLEYE_UPDATE;
			}
			else
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BATTLEYE_UNKNOWN;
			}
			Provider.battlEyeHasRequiredRestart = true;
			UnturnedLog.info("BattlEye client requested restart with reason: " + reason.ToString());
		}

		/// <summary>
		/// Called clientside by BattlEye when it needs us to send a packet to the server.
		/// </summary>
		// Token: 0x06003727 RID: 14119 RVA: 0x00101E94 File Offset: 0x00100094
		internal static void battlEyeClientSendPacket(IntPtr packetHandle, int length)
		{
			NetMessages.SendMessageToServer(EServerMessage.BattlEye, ENetReliability.Unreliable, delegate(NetPakWriter writer)
			{
				writer.WriteBits((uint)length, Provider.battlEyeBufferSize.bitCount);
				if (!writer.WriteBytes(packetHandle, length))
				{
					UnturnedLog.error("Unable to write BattlEye packet ({0} bytes)", new object[]
					{
						length
					});
				}
			});
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x00101EC8 File Offset: 0x001000C8
		private static void battlEyeServerPrintMessage(string message)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (steamPlayer != null && !(steamPlayer.player == null) && steamPlayer.player.wantsBattlEyeLogs)
				{
					steamPlayer.player.sendTerminalRelay(message);
				}
			}
			if (CommandWindow.shouldLogAnticheat)
			{
				CommandWindow.Log("BattlEye Server: " + message);
				return;
			}
			UnturnedLog.info("BattlEye Print: {0}", new object[]
			{
				message
			});
		}

		/// <summary>
		/// Event for plugins when BattlEye wants to kick a player.
		/// </summary>
		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06003729 RID: 14121 RVA: 0x00101F4C File Offset: 0x0010014C
		// (remove) Token: 0x0600372A RID: 14122 RVA: 0x00101F80 File Offset: 0x00100180
		public static event Provider.BattlEyeKickCallback onBattlEyeKick;

		// Token: 0x0600372B RID: 14123 RVA: 0x00101FB4 File Offset: 0x001001B4
		private static void broadcastBattlEyeKick(SteamPlayer client, string reason)
		{
			try
			{
				Provider.BattlEyeKickCallback battlEyeKickCallback = Provider.onBattlEyeKick;
				if (battlEyeKickCallback != null)
				{
					battlEyeKickCallback(client, reason);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onBattlEyeKick:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x00101FF8 File Offset: 0x001001F8
		private static void battlEyeServerKickPlayer(int playerID, string reason)
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer.battlEyeId == playerID)
				{
					if (steamPlayer.playerID.BypassIntegrityChecks)
					{
						break;
					}
					Provider.broadcastBattlEyeKick(steamPlayer, reason);
					UnturnedLog.info("BattlEye Kick {0} Reason: {1}", new object[]
					{
						steamPlayer.playerID.steamID,
						reason
					});
					if (reason.Length == 18 && reason.StartsWith("Global Ban #"))
					{
						ChatManager.say(steamPlayer.playerID.playerName + " got banned by BattlEye", Color.yellow, false);
					}
					Provider.kick(steamPlayer.playerID.steamID, "BattlEye: " + reason);
					SteamBlacklist.ban(steamPlayer.playerID.steamID, steamPlayer.getIPv4AddressOrZero(), steamPlayer.playerID.GetHwids(), CSteamID.Nil, "(Temporary) BattlEye: " + reason, 60U);
					break;
				}
			}
		}

		/// <summary>
		/// Called serverside by BattlEye when it needs us to send a packet to a player.
		/// </summary>
		// Token: 0x0600372D RID: 14125 RVA: 0x0010211C File Offset: 0x0010031C
		private static void battlEyeServerSendPacket(int playerID, IntPtr packetHandle, int length)
		{
			NetMessages.ClientWriteHandler <>9__0;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].battlEyeId == playerID)
				{
					EClientMessage index = EClientMessage.BattlEye;
					ENetReliability reliability = ENetReliability.Unreliable;
					ITransportConnection transportConnection = Provider.clients[i].transportConnection;
					NetMessages.ClientWriteHandler callback;
					if ((callback = <>9__0) == null)
					{
						callback = (<>9__0 = delegate(NetPakWriter writer)
						{
							writer.WriteBits((uint)length, Provider.battlEyeBufferSize.bitCount);
							writer.WriteBytes(packetHandle, length);
						});
					}
					NetMessages.SendMessageToClient(index, reliability, transportConnection, callback);
					return;
				}
			}
		}

		/// <summary>
		/// Call whenever something impacting rich presence changes for example loading a server or changing lobbies.
		/// </summary>
		// Token: 0x0600372E RID: 14126 RVA: 0x0010219D File Offset: 0x0010039D
		public static void updateRichPresence()
		{
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x001021A0 File Offset: 0x001003A0
		private static void updateSteamRichPresence()
		{
			if (Level.info != null)
			{
				if (Level.isEditor)
				{
					Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Editing", Level.info.getLocalizedName()));
					SteamFriends.SetRichPresence("steam_display", "#Status_EditingLevel");
					SteamFriends.SetRichPresence("steam_player_group", string.Empty);
				}
				else
				{
					Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Playing", Level.info.getLocalizedName()));
					if (Provider.isConnected && !Provider.isServer && Provider.server.m_SteamID > 0UL)
					{
						SteamFriends.SetRichPresence("steam_display", "#Status_PlayingMultiplayer");
						SteamFriends.SetRichPresence("steam_player_group", Provider.server.ToString());
					}
					else
					{
						SteamFriends.SetRichPresence("steam_display", "#Status_PlayingSingleplayer");
						SteamFriends.SetRichPresence("steam_player_group", string.Empty);
					}
				}
				SteamFriends.SetRichPresence("level_name", Level.info.getLocalizedName());
				return;
			}
			if (Lobbies.inLobby)
			{
				Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Lobby"));
				SteamFriends.SetRichPresence("steam_display", "#Status_WaitingInLobby");
				SteamFriends.SetRichPresence("steam_player_group", Lobbies.currentLobby.ToString());
				return;
			}
			Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Menu"));
			SteamFriends.SetRichPresence("steam_display", "#Status_AtMainMenu");
			SteamFriends.SetRichPresence("steam_player_group", string.Empty);
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06003730 RID: 14128 RVA: 0x00102347 File Offset: 0x00100547
		public static uint bytesSent
		{
			get
			{
				return Provider._bytesSent;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x0010234E File Offset: 0x0010054E
		public static uint bytesReceived
		{
			get
			{
				return Provider._bytesReceived;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003732 RID: 14130 RVA: 0x00102355 File Offset: 0x00100555
		public static uint packetsSent
		{
			get
			{
				return Provider._packetsSent;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x0010235C File Offset: 0x0010055C
		public static uint packetsReceived
		{
			get
			{
				return Provider._packetsReceived;
			}
		}

		/// <summary>
		/// Only used on client.
		/// Information about current game server retrieved through Steam's "A2S" query system.
		/// Available when joining using the Steam server list API (in-game server browser)
		/// or querying the Server's A2S port directly (connect by IP menu), but not when
		/// joining by Steam ID.
		/// </summary>
		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x00102363 File Offset: 0x00100563
		public static SteamServerAdvertisement CurrentServerAdvertisement
		{
			get
			{
				return Provider._currentServerAdvertisement;
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x0010236A File Offset: 0x0010056A
		public static ServerConnectParameters CurrentServerConnectParameters
		{
			get
			{
				return Provider._currentServerConnectParameters;
			}
		}

		/// <summary>
		/// On client, is current server protected by VAC?
		/// Set after initial response is received.
		/// </summary>
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x00102371 File Offset: 0x00100571
		internal static bool IsVacActiveOnCurrentServer
		{
			get
			{
				return Provider.isVacActive;
			}
		}

		/// <summary>
		/// On client, is current server protected by BattlEye?
		/// Set after initial response is received.
		/// </summary>
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x00102378 File Offset: 0x00100578
		internal static bool IsBattlEyeActiveOnCurrentServer
		{
			get
			{
				return Provider.isBattlEyeActive;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x0010237F File Offset: 0x0010057F
		public static CSteamID server
		{
			get
			{
				return Provider._server;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x00102386 File Offset: 0x00100586
		public static CSteamID client
		{
			get
			{
				return Provider._client;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x0010238D File Offset: 0x0010058D
		public static CSteamID user
		{
			get
			{
				return Provider._user;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x00102394 File Offset: 0x00100594
		public static byte[] clientHash
		{
			get
			{
				return Provider._clientHash;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x0010239B File Offset: 0x0010059B
		public static string clientName
		{
			get
			{
				return Provider._clientName;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x001023A2 File Offset: 0x001005A2
		public static List<SteamPlayer> clients
		{
			get
			{
				return Provider._clients;
			}
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x001023AC File Offset: 0x001005AC
		public static PooledTransportConnectionList GatherClientConnections()
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				pooledTransportConnectionList.Add(steamPlayer.transportConnection);
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x0010240C File Offset: 0x0010060C
		[Obsolete("Replaced by GatherClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients()
		{
			return Provider.GatherClientConnections();
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x00102414 File Offset: 0x00100614
		public static PooledTransportConnectionList GatherClientConnectionsMatchingPredicate(Predicate<SteamPlayer> predicate)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				if (predicate.Invoke(steamPlayer))
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x0010247C File Offset: 0x0010067C
		[Obsolete("Replaced by GatherClientConnectionsMatchingPredicate")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Predicate(Predicate<SteamPlayer> predicate)
		{
			return Provider.GatherClientConnectionsMatchingPredicate(predicate);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x00102484 File Offset: 0x00100684
		public static PooledTransportConnectionList GatherClientConnectionsWithinSphere(Vector3 position, float radius)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			float num = radius * radius;
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				if (steamPlayer.player != null && (steamPlayer.player.transform.position - position).sqrMagnitude < num)
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x00102518 File Offset: 0x00100718
		[Obsolete("Replaced by GatherClientConnectionsWithinSphere")]
		public static IEnumerable<ITransportConnection> EnumerateClients_WithinSphere(Vector3 position, float radius)
		{
			return Provider.GatherClientConnectionsWithinSphere(position, radius);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x00102524 File Offset: 0x00100724
		public static PooledTransportConnectionList GatherRemoteClientConnectionsWithinSphere(Vector3 position, float radius)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			float num = radius * radius;
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				if (steamPlayer.player != null && (steamPlayer.player.transform.position - position).sqrMagnitude < num)
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x001025B8 File Offset: 0x001007B8
		[Obsolete("Replaced by GatherRemoteClientConnectionsWithinSphere")]
		public static IEnumerable<ITransportConnection> EnumerateClients_RemoteWithinSphere(Vector3 position, float radius)
		{
			return Provider.GatherRemoteClientConnectionsWithinSphere(position, radius);
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x001025C4 File Offset: 0x001007C4
		public static PooledTransportConnectionList GatherRemoteClientConnections()
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				pooledTransportConnectionList.Add(steamPlayer.transportConnection);
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x00102624 File Offset: 0x00100824
		[Obsolete("Replaced by GatherRemoteClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Remote()
		{
			return Provider.GatherRemoteClientConnections();
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x0010262C File Offset: 0x0010082C
		public static PooledTransportConnectionList GatherRemoteClientConnectionsMatchingPredicate(Predicate<SteamPlayer> predicate)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				if (predicate.Invoke(steamPlayer))
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x00102694 File Offset: 0x00100894
		[Obsolete("Replaced by GatherRemoteClientsMatchingPredicate")]
		public static IEnumerable<ITransportConnection> EnumerateClients_RemotePredicate(Predicate<SteamPlayer> predicate)
		{
			return Provider.GatherRemoteClientConnectionsMatchingPredicate(predicate);
		}

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x0010269C File Offset: 0x0010089C
		[Obsolete]
		public static List<SteamPlayer> players
		{
			get
			{
				return Provider.clients;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x001026A3 File Offset: 0x001008A3
		public static bool isServer
		{
			get
			{
				return Provider._isServer;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x001026AA File Offset: 0x001008AA
		public static bool isClient
		{
			get
			{
				return Provider._isClient;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x001026B1 File Offset: 0x001008B1
		public static bool isPro
		{
			get
			{
				return Provider._isPro;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x001026B8 File Offset: 0x001008B8
		public static bool isConnected
		{
			get
			{
				return Provider._isConnected;
			}
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x001026C0 File Offset: 0x001008C0
		private static bool doServerItemsMatchAdvertisement(List<PublishedFileId_t> pendingWorkshopItems)
		{
			if (Provider.waitingForExpectedWorkshopItems == null)
			{
				return true;
			}
			if (Provider.waitingForExpectedWorkshopItems.Count < pendingWorkshopItems.Count)
			{
				return false;
			}
			foreach (PublishedFileId_t publishedFileId_t in pendingWorkshopItems)
			{
				if (!Provider.waitingForExpectedWorkshopItems.Contains(publishedFileId_t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x00102738 File Offset: 0x00100938
		internal static void receiveWorkshopResponse(Provider.CachedWorkshopResponse response)
		{
			Provider.authorityHoliday = response.holiday;
			Provider.currentServerWorkshopResponse = response;
			Provider.isWaitingForWorkshopResponse = false;
			Provider.serverName = response.serverName;
			Provider.map = response.levelName;
			Provider.isPvP = response.isPvP;
			Provider.mode = response.gameMode;
			Provider.cameraMode = response.cameraMode;
			Provider.maxPlayers = response.maxPlayers;
			Provider.isVacActive = response.isVACSecure;
			Provider.isBattlEyeActive = response.isBattlEyeSecure;
			List<PublishedFileId_t> list = new List<PublishedFileId_t>(response.requiredFiles.Count);
			foreach (Provider.ServerRequiredWorkshopFile serverRequiredWorkshopFile in response.requiredFiles)
			{
				if (serverRequiredWorkshopFile.fileId != 0UL)
				{
					list.Add(new PublishedFileId_t(serverRequiredWorkshopFile.fileId));
				}
			}
			Provider.provider.workshopService.resetServerInvalidItems();
			if (Provider.CurrentServerAdvertisement != null)
			{
				if (!string.IsNullOrEmpty(Provider.CurrentServerAdvertisement.map) && !string.Equals(Provider.CurrentServerAdvertisement.map, response.levelName, 5))
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_MAP_ADVERTISEMENT_MISMATCH;
					Provider.RequestDisconnect(string.Concat(new string[]
					{
						"server map advertisement mismatch (Advertisement: \"",
						Provider.CurrentServerAdvertisement.map,
						"\" Response: \"",
						response.levelName,
						"\")"
					}));
					return;
				}
				if (Provider.CurrentServerAdvertisement.IsBattlEyeSecure != response.isBattlEyeSecure)
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_BATTLEYE_ADVERTISEMENT_MISMATCH;
					Provider.RequestDisconnect(string.Format("server BE advertisement mismatch (Advertisement: {0} Response: {1})", Provider.CurrentServerAdvertisement.IsBattlEyeSecure, response.isBattlEyeSecure));
					return;
				}
				if (Provider.CurrentServerAdvertisement.maxPlayers != (int)response.maxPlayers)
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_MAXPLAYERS_ADVERTISEMENT_MISMATCH;
					Provider.RequestDisconnect(string.Format("server max players advertisement mismatch (Advertisement: {0} Response: {1})", Provider.CurrentServerAdvertisement.maxPlayers, response.maxPlayers));
					return;
				}
				if (Provider.CurrentServerAdvertisement.cameraMode != response.cameraMode)
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_CAMERAMODE_ADVERTISEMENT_MISMATCH;
					Provider.RequestDisconnect(string.Format("server camera mode advertisement mismatch (Advertisement: {0} Response: {1})", Provider.CurrentServerAdvertisement.cameraMode, response.cameraMode));
					return;
				}
				if (Provider.CurrentServerAdvertisement.isPvP != response.isPvP)
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_PVP_ADVERTISEMENT_MISMATCH;
					Provider.RequestDisconnect(string.Format("server PvP advertisement mismatch (Advertisement: {0} Response: {1})", Provider.CurrentServerAdvertisement.isPvP, response.isPvP));
					return;
				}
			}
			if (list.Count < 1)
			{
				UnturnedLog.info("Server specified no workshop items, launching");
				Provider.launch();
				return;
			}
			SteamServerAdvertisement currentServerAdvertisement = Provider.CurrentServerAdvertisement;
			if ((currentServerAdvertisement == null || currentServerAdvertisement.isWorkshop) && Provider.doServerItemsMatchAdvertisement(list))
			{
				Provider.canCurrentlyHandleClientTransportFailure = false;
				UnturnedLog.info("Server specified {0} workshop item(s), querying details", new object[]
				{
					list.Count
				});
				Provider.provider.workshopService.queryServerWorkshopItems(list, response.ip);
				return;
			}
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.WORKSHOP_ADVERTISEMENT_MISMATCH;
			Provider.RequestDisconnect("workshop advertisement mismatch");
		}

		/// <summary>
		/// Only safe to use serverside.
		/// Get the list of workshop ids that a client needs to download when joining.
		/// </summary>
		// Token: 0x06003751 RID: 14161 RVA: 0x00102A34 File Offset: 0x00100C34
		public static List<ulong> getServerWorkshopFileIDs()
		{
			return Provider._serverWorkshopFileIDs;
		}

		/// <summary>
		/// Only safe to use serverside.
		/// Lets clients know that this workshop id is being used on the server, and that they need to download it when joining.
		/// </summary>
		// Token: 0x06003752 RID: 14162 RVA: 0x00102A3B File Offset: 0x00100C3B
		public static void registerServerUsingWorkshopFileId(ulong id)
		{
			Provider.registerServerUsingWorkshopFileId(id, 0U);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x00102A44 File Offset: 0x00100C44
		internal static void registerServerUsingWorkshopFileId(ulong id, uint timestamp)
		{
			if (Provider._serverWorkshopFileIDs.Contains(id))
			{
				return;
			}
			Provider._serverWorkshopFileIDs.Add(id);
			Provider.ServerRequiredWorkshopFile serverRequiredWorkshopFile = new Provider.ServerRequiredWorkshopFile
			{
				fileId = id,
				timestamp = DateTimeEx.FromUtcUnixTimeSeconds(timestamp)
			};
			UnturnedLog.info(string.Format("Workshop file {0} requiring timestamp {1}", id, serverRequiredWorkshopFile.timestamp.ToLocalTime()));
			Provider.serverRequiredWorkshopFiles.Add(serverRequiredWorkshopFile);
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x00102ABA File Offset: 0x00100CBA
		public static bool isLoading
		{
			get
			{
				return Provider.isLoadingUGC;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x00102AC1 File Offset: 0x00100CC1
		[Obsolete]
		public static int channels
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Channel id was 32-bits, but now that it is in the RPC header it can be 8-bits since there never that many
		/// players online. The "manager" components are on channel 1, and each player has a channel.
		/// </summary>
		// Token: 0x06003756 RID: 14166 RVA: 0x00102AC4 File Offset: 0x00100CC4
		private static int allocPlayerChannelId()
		{
			for (int i = 0; i < 255; i++)
			{
				int num = Provider.nextPlayerChannelId;
				Provider.nextPlayerChannelId++;
				if (Provider.nextPlayerChannelId > 255)
				{
					Provider.nextPlayerChannelId = 2;
				}
				if (Provider.findChannelComponent(num) == null)
				{
					return num;
				}
			}
			CommandWindow.LogErrorFormat("Fatal error! Ran out of player RPC channel IDs", Array.Empty<object>());
			Provider.shutdown(1, "Fatal error! Ran out of player RPC channel IDs");
			return 2;
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x00102B30 File Offset: 0x00100D30
		// (set) Token: 0x06003758 RID: 14168 RVA: 0x00102B37 File Offset: 0x00100D37
		public static ESteamConnectionFailureInfo connectionFailureInfo
		{
			get
			{
				return Provider._connectionFailureInfo;
			}
			set
			{
				Provider._connectionFailureInfo = value;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x00102B3F File Offset: 0x00100D3F
		// (set) Token: 0x0600375A RID: 14170 RVA: 0x00102B46 File Offset: 0x00100D46
		public static string connectionFailureReason
		{
			get
			{
				return Provider._connectionFailureReason;
			}
			set
			{
				Provider._connectionFailureReason = value;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x00102B4E File Offset: 0x00100D4E
		public static uint connectionFailureDuration
		{
			get
			{
				return Provider._connectionFailureDuration;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x00102B55 File Offset: 0x00100D55
		public static List<SteamChannel> receivers
		{
			get
			{
				return Provider._receivers;
			}
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x00102B5C File Offset: 0x00100D5C
		private static int allocBattlEyePlayerId()
		{
			int result = Provider.nextBattlEyePlayerId;
			Provider.nextBattlEyePlayerId++;
			return result;
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x00102B6F File Offset: 0x00100D6F
		public static void resetConnectionFailure()
		{
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NONE;
			Provider._connectionFailureReason = "";
			Provider._connectionFailureDuration = 0U;
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x00102B87 File Offset: 0x00100D87
		[Conditional("LOG_NETCHANNEL")]
		private static void LogNetChannel(string format, params object[] args)
		{
			UnturnedLog.info(format, args);
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x00102B90 File Offset: 0x00100D90
		public static void openChannel(SteamChannel receiver)
		{
			Provider.receivers.Add(receiver);
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x00102B9D File Offset: 0x00100D9D
		public static void closeChannel(SteamChannel receiver)
		{
			Provider.receivers.RemoveFast(receiver);
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x00102BAC File Offset: 0x00100DAC
		internal static SteamChannel findChannelComponent(int id)
		{
			for (int i = Provider.receivers.Count - 1; i >= 0; i--)
			{
				SteamChannel steamChannel = Provider.receivers[i];
				if (steamChannel == null)
				{
					Provider.receivers.RemoveAtFast(i);
				}
				else if (steamChannel.id == id)
				{
					return steamChannel;
				}
			}
			return null;
		}

		/// <summary>
		/// Should the network transport layer accept incoming connections?
		/// If both the queue and connected slots are full then incoming connections are ignored.
		/// </summary>
		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x00102BFE File Offset: 0x00100DFE
		public static bool hasRoomForNewConnection
		{
			get
			{
				return Provider.clients.Count < (int)Provider.maxPlayers || Provider.pending.Count < (int)Provider.queueSize;
			}
		}

		/// <summary>
		/// Find player in the queue associated with a client connection.
		/// </summary>
		// Token: 0x06003764 RID: 14180 RVA: 0x00102C24 File Offset: 0x00100E24
		public static SteamPending findPendingPlayer(ITransportConnection transportConnection)
		{
			if (transportConnection == null)
			{
				return null;
			}
			foreach (SteamPending steamPending in Provider.pending)
			{
				if (transportConnection.Equals(steamPending.transportConnection))
				{
					return steamPending;
				}
			}
			return null;
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x00102C8C File Offset: 0x00100E8C
		internal static SteamPending findPendingPlayerBySteamId(CSteamID steamId)
		{
			foreach (SteamPending steamPending in Provider.pending)
			{
				if (steamPending.playerID.steamID == steamId)
				{
					return steamPending;
				}
			}
			return null;
		}

		/// <summary>
		/// Find player associated with a client connection.
		/// </summary>
		// Token: 0x06003766 RID: 14182 RVA: 0x00102CF4 File Offset: 0x00100EF4
		public static SteamPlayer findPlayer(ITransportConnection transportConnection)
		{
			if (transportConnection == null)
			{
				return null;
			}
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (transportConnection.Equals(steamPlayer.transportConnection))
				{
					return steamPlayer;
				}
			}
			return null;
		}

		/// <summary>
		/// Find net transport layer connection associated with a client steam id. This could be a pending player in the
		/// queue, or a fully connected player.
		/// </summary>
		// Token: 0x06003767 RID: 14183 RVA: 0x00102D5C File Offset: 0x00100F5C
		public static ITransportConnection findTransportConnection(CSteamID steamId)
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer.playerID.steamID == steamId)
				{
					return steamPlayer.transportConnection;
				}
			}
			foreach (SteamPending steamPending in Provider.pending)
			{
				if (steamPending.playerID.steamID == steamId)
				{
					return steamPending.transportConnection;
				}
			}
			return null;
		}

		/// <summary>
		/// Find player steam id associated with connection, otherwise nil if not found.
		/// </summary>
		// Token: 0x06003768 RID: 14184 RVA: 0x00102E20 File Offset: 0x00101020
		public static CSteamID findTransportConnectionSteamId(ITransportConnection transportConnection)
		{
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer != null)
			{
				return steamPlayer.playerID.steamID;
			}
			SteamPending steamPending = Provider.findPendingPlayer(transportConnection);
			if (steamPending != null)
			{
				return steamPending.playerID.steamID;
			}
			return CSteamID.Nil;
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x00102E5E File Offset: 0x0010105E
		internal static NetId ClaimNetIdBlockForNewPlayer()
		{
			return NetIdRegistry.ClaimBlock(16U);
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x00102E68 File Offset: 0x00101068
		internal static SteamPlayer addPlayer(ITransportConnection transportConnection, NetId netId, SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, Color markerColor, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, string[] skinTags, string[] skinDynamicProps, EPlayerSkillset skillset, string language, CSteamID lobbyID, EClientPlatform clientPlatform)
		{
			if (playerID.steamID == Provider.client && Level.placeholderAudioListener != null)
			{
				Object.Destroy(Level.placeholderAudioListener);
				Level.placeholderAudioListener = null;
			}
			Transform transform = null;
			try
			{
				transform = Provider.gameMode.getPlayerGameObject(playerID).transform;
				transform.position = point;
				transform.rotation = Quaternion.Euler(0f, (float)(angle * 2), 0f);
			}
			catch (Exception e)
			{
				UnturnedLog.error("Exception thrown when getting player from game mode:");
				UnturnedLog.exception(e);
			}
			SteamPlayer steamPlayer = null;
			try
			{
				steamPlayer = new SteamPlayer(transportConnection, netId, playerID, transform, isPro, isAdmin, channel, face, hair, beard, skin, color, markerColor, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skinTags, skinDynamicProps, skillset, language, lobbyID, clientPlatform);
				Provider.clients.Add(steamPlayer);
			}
			catch (Exception e2)
			{
				UnturnedLog.error("Exception thrown when adding player:");
				UnturnedLog.exception(e2);
			}
			Provider.updateRichPresence();
			Provider.broadcastEnemyConnected(steamPlayer);
			return steamPlayer;
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x00102F70 File Offset: 0x00101170
		internal static void removePlayer(byte index)
		{
			if (index < 0 || (int)index >= Provider.clients.Count)
			{
				UnturnedLog.error("Failed to find player: " + index.ToString());
				return;
			}
			SteamPlayer steamPlayer = Provider.clients[(int)index];
			if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnChangePlayerStatus != null)
			{
				Provider.battlEyeServerRunData.pfnChangePlayerStatus.Invoke(steamPlayer.battlEyeId, -1);
			}
			steamPlayer.transportConnection.CloseConnection();
			Provider.broadcastEnemyDisconnected(steamPlayer);
			steamPlayer.player.ReleaseNetIdBlock();
			if (steamPlayer.model != null)
			{
				Object.Destroy(steamPlayer.model.gameObject);
			}
			NetIdRegistry.Release(steamPlayer.GetNetId());
			Provider.clients.RemoveAt((int)index);
			Provider.verifyNextPlayerInQueue();
			Provider.updateRichPresence();
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x00103048 File Offset: 0x00101248
		private static void replicateRemovePlayer(CSteamID skipSteamID, byte removalIndex)
		{
			NetMessages.SendMessageToClients(EClientMessage.PlayerDisconnected, ENetReliability.Reliable, Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer potentialRecipient) => potentialRecipient.playerID.steamID != skipSteamID), delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, removalIndex);
			});
		}

		/// <summary>
		/// If there's space on the server, asks player at front of queue for their verification to begin playing.
		/// </summary>
		// Token: 0x0600376D RID: 14189 RVA: 0x00103090 File Offset: 0x00101290
		internal static void verifyNextPlayerInQueue()
		{
			if (Provider.pending.Count < 1)
			{
				return;
			}
			if (Provider.clients.Count >= (int)Provider.maxPlayers)
			{
				return;
			}
			SteamPending steamPending = Provider.pending[0];
			if (steamPending.hasSentVerifyPacket)
			{
				return;
			}
			steamPending.sendVerifyPacket();
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x001030D8 File Offset: 0x001012D8
		[Obsolete]
		private static bool isUnreliable(ESteamPacket type)
		{
			return type == ESteamPacket.UPDATE_UNRELIABLE_BUFFER || type - ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER <= 1;
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x001030E7 File Offset: 0x001012E7
		[Obsolete]
		public static bool isChunk(ESteamPacket packet)
		{
			return packet - ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER <= 1;
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x001030F2 File Offset: 0x001012F2
		[Obsolete]
		private static bool isUpdate(ESteamPacket packet)
		{
			return packet <= ESteamPacket.UPDATE_VOICE;
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x001030FC File Offset: 0x001012FC
		internal static void resetChannels()
		{
			Provider._bytesSent = 0U;
			Provider._bytesReceived = 0U;
			Provider._packetsSent = 0U;
			Provider._packetsReceived = 0U;
			Provider._clients.Clear();
			Provider.pending.Clear();
			NetIdRegistry.Clear();
			NetInvocationDeferralRegistry.Clear();
			ClientAssetIntegrity.Clear();
			PhysicsMaterialNetTable.Clear();
			ItemManager.ClearNetworkStuff();
			BarricadeManager.ClearNetworkStuff();
			StructureManager.ClearNetworkStuff();
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x00103158 File Offset: 0x00101358
		private static void loadPlayerSpawn(SteamPlayerID playerID, out Vector3 point, out byte angle, out EPlayerStance initialStance)
		{
			point = Vector3.zero;
			angle = 0;
			initialStance = EPlayerStance.STAND;
			bool flag = false;
			if (PlayerSavedata.fileExists(playerID, "/Player/Player.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(playerID, "/Player/Player.dat", 1);
				point = block.readSingleVector3() + new Vector3(0f, 0.01f, 0f);
				angle = block.readByte();
				if (!point.IsFinite())
				{
					flag = true;
					UnturnedLog.info("Reset {0} spawn position ({1}) because it was NaN or infinity", new object[]
					{
						playerID,
						point
					});
				}
				else if (point.y > Level.HEIGHT)
				{
					UnturnedLog.info("Clamped {0} spawn position ({1}) because it was above the world height limit ({2})", new object[]
					{
						playerID,
						point,
						Level.HEIGHT
					});
					point.y = Level.HEIGHT - 10f;
				}
				else if (!PlayerStance.getStanceForPosition(point, ref initialStance))
				{
					UnturnedLog.info("Reset {0} spawn position ({1}) because it was obstructed", new object[]
					{
						playerID,
						point
					});
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			try
			{
				if (Provider.onLoginSpawning != null)
				{
					float num = (float)(angle * 2);
					Provider.onLoginSpawning(playerID, ref point, ref num, ref initialStance, ref flag);
					angle = (byte)(num / 2f);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onLoginSpawning:");
				UnturnedLog.exception(e);
			}
			if (flag)
			{
				PlayerSpawnpoint spawn = LevelPlayers.getSpawn(false);
				point = spawn.point + new Vector3(0f, 0.5f, 0f);
				angle = (byte)(spawn.angle / 2f);
			}
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x00103318 File Offset: 0x00101518
		private static void ResetClientTransportFailure()
		{
			Provider.canCurrentlyHandleClientTransportFailure = true;
			Provider.hasPendingClientTransportFailure = false;
			Provider.pendingClientTransportFailureMessage = null;
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x0010332C File Offset: 0x0010152C
		private static void TriggerDisconnectFromClientTransportFailure()
		{
			Provider.hasPendingClientTransportFailure = false;
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.CUSTOM;
			Provider._connectionFailureReason = Provider.pendingClientTransportFailureMessage;
			Provider.RequestDisconnect("Client transport failure: \"" + Provider.pendingClientTransportFailureMessage + "\"");
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x00103360 File Offset: 0x00101560
		private static void onLevelLoaded(int level)
		{
			if (level == 2)
			{
				Provider.isLoadingUGC = false;
				if (Provider.isConnected)
				{
					if (Provider.isServer)
					{
						if (Provider.isClient)
						{
							SteamPlayerID steamPlayerID = new SteamPlayerID(Provider.client, Characters.selected, Provider.clientName, Characters.active.name, Characters.active.nick, Characters.active.group);
							Vector3 point;
							byte angle;
							EPlayerStance initialStance;
							Provider.loadPlayerSpawn(steamPlayerID, out point, out angle, out initialStance);
							int inventoryItem = Provider.provider.economyService.getInventoryItem(Characters.active.packageShirt);
							int inventoryItem2 = Provider.provider.economyService.getInventoryItem(Characters.active.packagePants);
							int inventoryItem3 = Provider.provider.economyService.getInventoryItem(Characters.active.packageHat);
							int inventoryItem4 = Provider.provider.economyService.getInventoryItem(Characters.active.packageBackpack);
							int inventoryItem5 = Provider.provider.economyService.getInventoryItem(Characters.active.packageVest);
							int inventoryItem6 = Provider.provider.economyService.getInventoryItem(Characters.active.packageMask);
							int inventoryItem7 = Provider.provider.economyService.getInventoryItem(Characters.active.packageGlasses);
							int[] array = new int[Characters.packageSkins.Count];
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = Provider.provider.economyService.getInventoryItem(Characters.packageSkins[i]);
							}
							string[] array2 = new string[Characters.packageSkins.Count];
							for (int j = 0; j < array2.Length; j++)
							{
								array2[j] = Provider.provider.economyService.getInventoryTags(Characters.packageSkins[j]);
							}
							string[] array3 = new string[Characters.packageSkins.Count];
							for (int k = 0; k < array3.Length; k++)
							{
								array3[k] = Provider.provider.economyService.getInventoryDynamicProps(Characters.packageSkins[k]);
							}
							ITransportConnection transportConnection = null;
							NetId netId = Provider.ClaimNetIdBlockForNewPlayer();
							SteamPlayer steamPlayer = Provider.addPlayer(transportConnection, netId, steamPlayerID, point, angle, Provider.isPro, true, Provider.allocPlayerChannelId(), Characters.active.face, Characters.active.hair, Characters.active.beard, Characters.active.skin, Characters.active.color, Characters.active.markerColor, Characters.active.hand, inventoryItem, inventoryItem2, inventoryItem3, inventoryItem4, inventoryItem5, inventoryItem6, inventoryItem7, array, array2, array3, Characters.active.skillset, Provider.language, Lobbies.currentLobby, EClientPlatform.Windows);
							steamPlayer.player.stance.initialStance = initialStance;
							steamPlayer.player.InitializePlayer();
							steamPlayer.player.SendInitialPlayerState(steamPlayer);
							Lobbies.leaveLobby();
							Provider.updateRichPresence();
							try
							{
								Provider.ServerConnected serverConnected = Provider.onServerConnected;
								if (serverConnected != null)
								{
									serverConnected(steamPlayerID.steamID);
								}
								return;
							}
							catch (Exception e)
							{
								UnturnedLog.warn("Plugin raised an exception from onServerConnected:");
								UnturnedLog.exception(e);
								return;
							}
						}
						CommandWindow.Log("//////////////////////////////////////////////////////");
						CommandWindow.Log(Provider.localization.format("ServerCode", SteamGameServer.GetSteamID()));
						CommandWindow.Log(Provider.localization.format("ServerCodeDetails"));
						CommandWindow.Log(Provider.localization.format("ServerCodeCopy", "CopyServerCode"));
						CommandWindow.Log("//////////////////////////////////////////////////////");
						return;
					}
					if (Provider.hasPendingClientTransportFailure)
					{
						UnturnedLog.info("Now able to handle client transport failure that occurred during level load");
						Provider.TriggerDisconnectFromClientTransportFailure();
						return;
					}
					Provider.canCurrentlyHandleClientTransportFailure = true;
					EClientPlatform clientPlatform = EClientPlatform.Windows;
					Provider.critMods.Clear();
					Provider.modBuilder.Length = 0;
					ModuleHook.getRequiredModules(Provider.critMods);
					for (int l = 0; l < Provider.critMods.Count; l++)
					{
						Provider.modBuilder.Append(Provider.critMods[l].config.Name);
						Provider.modBuilder.Append(",");
						Provider.modBuilder.Append(Provider.critMods[l].config.Version_Internal);
						if (l < Provider.critMods.Count - 1)
						{
							Provider.modBuilder.Append(";");
						}
					}
					UnturnedLog.info("Ready to connect");
					Provider.isWaitingForConnectResponse = true;
					Provider.sentConnectRequestTime = Time.realtimeSinceStartup;
					NetMessages.SendMessageToServer(EServerMessage.ReadyToConnect, ENetReliability.Reliable, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, Characters.selected);
						SystemNetPakWriterEx.WriteString(writer, Provider.clientName, 11);
						SystemNetPakWriterEx.WriteString(writer, Characters.active.name, 11);
						writer.WriteBytes(Provider._serverPasswordHash, 20);
						writer.WriteBytes(Level.hash, 20);
						writer.WriteBytes(ReadWrite.readData(), 20);
						writer.WriteEnum(clientPlatform);
						SystemNetPakWriterEx.WriteUInt32(writer, Provider.APP_VERSION_PACKED);
						writer.WriteBit(Provider.isPro);
						SteamServerAdvertisement currentServerAdvertisement = Provider.CurrentServerAdvertisement;
						SystemNetPakWriterEx.WriteUInt16(writer, MathfEx.ClampToUShort((currentServerAdvertisement != null) ? currentServerAdvertisement.ping : 1));
						SystemNetPakWriterEx.WriteString(writer, Characters.active.nick, 11);
						SteamworksNetPakWriterEx.WriteSteamID(writer, Characters.active.group);
						SystemNetPakWriterEx.WriteUInt8(writer, Characters.active.face);
						SystemNetPakWriterEx.WriteUInt8(writer, Characters.active.hair);
						SystemNetPakWriterEx.WriteUInt8(writer, Characters.active.beard);
						UnityNetPakWriterEx.WriteColor32RGB(writer, Characters.active.skin);
						UnityNetPakWriterEx.WriteColor32RGB(writer, Characters.active.color);
						UnityNetPakWriterEx.WriteColor32RGB(writer, Characters.active.markerColor);
						writer.WriteBit(Characters.active.hand);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packageShirt);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packagePants);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packageHat);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packageBackpack);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packageVest);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packageMask);
						SystemNetPakWriterEx.WriteUInt64(writer, Characters.active.packageGlasses);
						SystemNetPakWriterEx.WriteList<ulong>(writer, Characters.packageSkins, new SystemNetPakWriterEx.WriteListItem<ulong>(writer.WriteUInt64), Provider.MAX_SKINS_LENGTH);
						writer.WriteEnum(Characters.active.skillset);
						SystemNetPakWriterEx.WriteString(writer, Provider.modBuilder.ToString(), 11);
						SystemNetPakWriterEx.WriteString(writer, Provider.language, 11);
						SteamworksNetPakWriterEx.WriteSteamID(writer, Lobbies.currentLobby);
						SystemNetPakWriterEx.WriteUInt32(writer, Level.packedVersion);
						byte[][] hwids = LocalHwid.GetHwids();
						SystemNetPakWriterEx.WriteUInt8(writer, (byte)hwids.Length);
						foreach (byte[] array5 in hwids)
						{
							writer.WriteBytes(array5, 20);
						}
						writer.WriteBytes(TempSteamworksEconomy.econInfoHash, 20);
						SteamworksNetPakWriterEx.WriteSteamID(writer, Provider.user);
					});
				}
			}
		}

		/// <summary>
		/// Connect to server entry point on client.
		/// Requests workshop details for download prior to loading level.
		/// Once workshop is ready launch() is called.
		/// </summary>
		// Token: 0x06003776 RID: 14198 RVA: 0x001037CC File Offset: 0x001019CC
		public static void connect(ServerConnectParameters parameters, SteamServerAdvertisement advertisement, List<PublishedFileId_t> expectedWorkshopItems)
		{
			if (Provider.isConnected)
			{
				return;
			}
			Provider._currentServerConnectParameters = parameters;
			Provider._currentServerAdvertisement = advertisement;
			Provider.isWhitelisted = false;
			Provider.isVacActive = false;
			Provider.isBattlEyeActive = false;
			Provider._isConnected = true;
			Provider._queuePosition = 0;
			Provider.resetChannels();
			if (Provider._currentServerAdvertisement != null)
			{
				Lobbies.LinkLobby(Provider._currentServerAdvertisement.ip, Provider._currentServerAdvertisement.queryPort);
				Provider._server = Provider._currentServerAdvertisement.steamID;
			}
			else
			{
				Lobbies.LinkLobby(parameters.address.value, parameters.queryPort);
				Provider._server = parameters.steamId;
			}
			Provider._serverPassword = parameters.password;
			Provider._serverPasswordHash = Hash.SHA1(parameters.password);
			Provider._isClient = true;
			Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
			Provider.pings = new float[4];
			Provider.lag((Provider._currentServerAdvertisement != null) ? ((float)Provider._currentServerAdvertisement.ping / 1000f) : 0f);
			Provider.isLoadingUGC = true;
			LoadingUI.updateScene();
			Provider.isWaitingForConnectResponse = false;
			Provider.isWaitingForWorkshopResponse = true;
			Provider.waitingForExpectedWorkshopItems = expectedWorkshopItems;
			Provider.catPouncingMechanism = -22f;
			List<SteamItemInstanceID_t> list = new List<SteamItemInstanceID_t>();
			if (Characters.active.packageShirt != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageShirt);
			}
			if (Characters.active.packagePants != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packagePants);
			}
			if (Characters.active.packageHat != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageHat);
			}
			if (Characters.active.packageBackpack != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageBackpack);
			}
			if (Characters.active.packageVest != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageVest);
			}
			if (Characters.active.packageMask != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageMask);
			}
			if (Characters.active.packageGlasses != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageGlasses);
			}
			for (int i = 0; i < Characters.packageSkins.Count; i++)
			{
				ulong num = Characters.packageSkins[i];
				if (num != 0UL)
				{
					list.Add((SteamItemInstanceID_t)num);
				}
			}
			if (list.Count > 0)
			{
				SteamInventory.GetItemsByID(out Provider.provider.economyService.wearingResult, list.ToArray(), (uint)list.Count);
			}
			Level.loading();
			Provider.ResetClientTransportFailure();
			SteamServerAdvertisement currentServerAdvertisement = Provider._currentServerAdvertisement;
			Provider.clientTransport = NetTransportFactory.CreateClientTransport((currentServerAdvertisement != null) ? currentServerAdvertisement.networkTransport : null);
			UnturnedLog.info("Initializing {0}", new object[]
			{
				Provider.clientTransport.GetType().Name
			});
			Provider.clientTransport.Initialize(new ClientTransportReady(Provider.onClientTransportReady), new ClientTransportFailure(Provider.onClientTransportFailure));
		}

		/// <summary>
		/// Callback once client transport is ready to send messages.
		/// </summary>
		// Token: 0x06003777 RID: 14199 RVA: 0x00103A94 File Offset: 0x00101C94
		private static void onClientTransportReady()
		{
			Provider.CachedWorkshopResponse cachedWorkshopResponse = null;
			foreach (Provider.CachedWorkshopResponse cachedWorkshopResponse2 in Provider.cachedWorkshopResponses)
			{
				if (cachedWorkshopResponse2.server == Provider.server && Time.realtimeSinceStartup - cachedWorkshopResponse2.realTime < 60f)
				{
					cachedWorkshopResponse = cachedWorkshopResponse2;
					break;
				}
			}
			if (cachedWorkshopResponse != null)
			{
				Provider.receiveWorkshopResponse(cachedWorkshopResponse);
				return;
			}
			NetMessages.SendMessageToServer(EServerMessage.GetWorkshopFiles, ENetReliability.Reliable, delegate(NetPakWriter writer)
			{
				writer.AlignToByte();
				for (int i = 0; i < 240; i++)
				{
					writer.WriteBits(0U, 32);
				}
				SystemNetPakWriterEx.WriteString(writer, "Hello!", 11);
			});
		}

		/// <summary>
		/// Callback when something goes wrong and client must disconnect.
		/// </summary>
		// Token: 0x06003778 RID: 14200 RVA: 0x00103B3C File Offset: 0x00101D3C
		private static void onClientTransportFailure(string message)
		{
			Provider.hasPendingClientTransportFailure = true;
			Provider.pendingClientTransportFailureMessage = message;
			if (Provider.canCurrentlyHandleClientTransportFailure)
			{
				Provider.TriggerDisconnectFromClientTransportFailure();
				return;
			}
			UnturnedLog.info("Deferring client transport failure because we can't currently handle it");
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x00103B64 File Offset: 0x00101D64
		private static bool CompareClientAndServerWorkshopFileTimestamps()
		{
			if (Provider.provider.workshopService.serverPendingIDs == null)
			{
				return true;
			}
			foreach (PublishedFileId_t publishedFileId_t in Provider.provider.workshopService.serverPendingIDs)
			{
				Provider.ServerRequiredWorkshopFile serverRequiredWorkshopFile;
				ulong num;
				string text;
				uint num2;
				if (!Provider.currentServerWorkshopResponse.FindRequiredFile(publishedFileId_t.m_PublishedFileId, out serverRequiredWorkshopFile))
				{
					UnturnedLog.error(string.Format("Server workshop files response missing details for file: {0}", publishedFileId_t));
				}
				else if (serverRequiredWorkshopFile.timestamp.Year < 2000)
				{
					UnturnedLog.info(string.Format("Skipping timestamp comparison for server workshop file {0} because timestamp is invalid ({1})", publishedFileId_t, serverRequiredWorkshopFile.timestamp.ToLocalTime()));
				}
				else if (!SteamUGC.GetItemInstallInfo(publishedFileId_t, out num, out text, 1024U, out num2))
				{
					UnturnedLog.info(string.Format("Skipping timestamp comparison for server workshop file {0} because item install info is missing", publishedFileId_t));
				}
				else
				{
					DateTime dateTime = DateTimeEx.FromUtcUnixTimeSeconds(num2);
					if (!(dateTime == serverRequiredWorkshopFile.timestamp))
					{
						CachedUGCDetails cachedUGCDetails;
						bool cachedDetails = TempSteamworksWorkshop.getCachedDetails(publishedFileId_t, out cachedUGCDetails);
						string text2;
						if (cachedDetails)
						{
							text2 = cachedUGCDetails.GetTitle();
						}
						else
						{
							text2 = string.Format("Unknown File ID {0}", publishedFileId_t);
						}
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.CUSTOM;
						string text3;
						if (serverRequiredWorkshopFile.timestamp > dateTime)
						{
							text3 = "Server is running a newer version of the \"" + text2 + "\" workshop file.";
						}
						else
						{
							text3 = "Server is running an older version of the \"" + text2 + "\" workshop file.";
						}
						if (cachedDetails)
						{
							DateTime dateTime2 = DateTimeEx.FromUtcUnixTimeSeconds(cachedUGCDetails.updateTimestamp);
							if (dateTime == dateTime2)
							{
								text3 += "\nYour installed copy of the file matches the most recent version on Steam.";
								text3 += string.Format("\nLocal and Steam timestamp: {0} Server timestamp: {1}", dateTime.ToLocalTime(), serverRequiredWorkshopFile.timestamp.ToLocalTime());
							}
							else if (serverRequiredWorkshopFile.timestamp == dateTime2)
							{
								text3 += "\nThe server's installed copy of the file matches the most recent version on Steam.";
								text3 += string.Format("\nLocal timestamp: {0} Server and Steam timestamp: {1}", dateTime.ToLocalTime(), serverRequiredWorkshopFile.timestamp.ToLocalTime());
							}
							else
							{
								text3 += string.Format("\nLocal timestamp: {0} Server timestamp: {1} Steam timestamp: {2}", dateTime.ToLocalTime(), serverRequiredWorkshopFile.timestamp.ToLocalTime(), dateTime2);
							}
						}
						else
						{
							text3 += string.Format("\nLocal timestamp: {0} Server timestamp: {1}", dateTime.ToLocalTime(), serverRequiredWorkshopFile.timestamp.ToLocalTime());
						}
						Provider._connectionFailureReason = text3;
						Provider.RequestDisconnect(string.Format("Loaded workshop file timestamp mismatch (File ID: {0} Local timestamp: {1} Server timestamp: {2})", publishedFileId_t, dateTime.ToLocalTime(), serverRequiredWorkshopFile.timestamp.ToLocalTime()));
						return false;
					}
					UnturnedLog.info(string.Format("Workshop file {0} timestamp matches between client and server ({1})", publishedFileId_t, dateTime));
				}
			}
			return true;
		}

		/// <summary>
		/// Multiplayer load level entry point on client.
		/// Called once workshop downloads are finished, or we know the server is not using workshop.
		/// Once level is loaded the connect packet is sent to the server.
		/// </summary>
		// Token: 0x0600377A RID: 14202 RVA: 0x00103E74 File Offset: 0x00102074
		public static void launch()
		{
			LevelInfo level = Level.getLevel(Provider.map);
			if (level == null)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.MAP;
				Provider.RequestDisconnect("could not find level \"" + Provider.map + "\"");
				return;
			}
			if (!Provider.CompareClientAndServerWorkshopFileTimestamps())
			{
				return;
			}
			if (Provider.hasPendingClientTransportFailure)
			{
				UnturnedLog.info("Now able to handle client transport failure that occurred during workshop file download/install/load");
				Provider.TriggerDisconnectFromClientTransportFailure();
				return;
			}
			Assets.ApplyServerAssetMapping(level, Provider.provider.workshopService.serverPendingIDs);
			Provider.canCurrentlyHandleClientTransportFailure = false;
			UnturnedLog.info("Loading server level ({0})", new object[]
			{
				Provider.map
			});
			Level.load(level, false);
			Provider.loadGameMode();
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x00103F10 File Offset: 0x00102110
		private static void loadGameMode()
		{
			LevelAsset asset = Level.getAsset();
			if (asset == null)
			{
				Provider.gameMode = new SurvivalGameMode();
				return;
			}
			Type type = asset.defaultGameMode.type;
			if (type == null)
			{
				Provider.gameMode = new SurvivalGameMode();
				return;
			}
			Provider.gameMode = (Activator.CreateInstance(type) as GameMode);
			if (Provider.gameMode == null)
			{
				Provider.gameMode = new SurvivalGameMode();
			}
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x00103F72 File Offset: 0x00102172
		private static void unloadGameMode()
		{
			Provider.gameMode = null;
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x00103F7C File Offset: 0x0010217C
		public static void singleplayer(EGameMode singleplayerMode, bool singleplayerCheats)
		{
			Provider._isConnected = true;
			Provider.resetChannels();
			Dedicator.serverVisibility = ESteamServerVisibility.LAN;
			Dedicator.serverID = "Singleplayer_" + Characters.selected.ToString();
			Commander.init();
			Provider.maxPlayers = 1;
			Provider.queueSize = 8;
			Provider.serverName = "Singleplayer #" + ((int)(Characters.selected + 1)).ToString();
			Provider.serverPassword = "Singleplayer";
			Provider.isVacActive = false;
			Provider.isBattlEyeActive = false;
			Provider.ip = 0U;
			Provider.port = 25000;
			Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
			Provider.pings = new float[4];
			Provider.isPvP = true;
			Provider.isWhitelisted = false;
			Provider.hideAdmins = false;
			Provider.hasCheats = singleplayerCheats;
			Provider.filterName = false;
			Provider.mode = singleplayerMode;
			Provider.isGold = false;
			Provider.gameMode = null;
			Provider.cameraMode = ECameraMode.BOTH;
			if (singleplayerMode != EGameMode.TUTORIAL)
			{
				PlayerInventory.skillsets = PlayerInventory.SKILLSETS_CLIENT;
			}
			Provider.lag(0f);
			SteamWhitelist.load();
			SteamBlacklist.load();
			SteamAdminlist.load();
			Provider._currentServerAdvertisement = null;
			Provider._configData = ConfigData.CreateDefault(true);
			if (ServerSavedata.fileExists("/Config.json"))
			{
				try
				{
					ServerSavedata.populateJSON("/Config.json", Provider._configData);
				}
				catch (Exception e)
				{
					UnturnedLog.error("Exception while parsing singleplayer config:");
					UnturnedLog.exception(e);
				}
			}
			Provider._modeConfigData = Provider._configData.getModeConfig(Provider.mode);
			if (Provider._modeConfigData == null)
			{
				Provider._modeConfigData = new ModeConfigData(Provider.mode);
				Provider._modeConfigData.InitSingleplayerDefaults();
			}
			Provider.authorityHoliday = (Provider._modeConfigData.Gameplay.Allow_Holidays ? HolidayUtil.BackendGetActiveHoliday() : ENPCHoliday.NONE);
			Provider._isServer = true;
			Provider._isClient = true;
			PhysicsMaterialNetTable.ServerPopulateTable();
			Provider.time = SteamUtils.GetServerRealTime();
			Level.load(Level.getLevel(Provider.map), true);
			Provider.loadGameMode();
			Provider.applyLevelModeConfigOverrides();
			Provider._server = Provider.user;
			Provider._client = Provider._server;
			Provider._clientHash = Hash.SHA1(Provider.client);
			Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
			Provider.broadcastServerHosted();
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x00104188 File Offset: 0x00102388
		public static void host()
		{
			Provider._isConnected = true;
			Provider.resetChannels();
			Provider.openGameServer();
			Provider._isServer = true;
			Provider.broadcastServerHosted();
		}

		/// <summary>
		/// Event for plugins prior to kicking players during shutdown.
		/// </summary>
		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x0600377F RID: 14207 RVA: 0x001041A8 File Offset: 0x001023A8
		// (remove) Token: 0x06003780 RID: 14208 RVA: 0x001041DC File Offset: 0x001023DC
		public static event Provider.CommenceShutdownHandler onCommenceShutdown;

		// Token: 0x06003781 RID: 14209 RVA: 0x00104210 File Offset: 0x00102410
		private static void broadcastCommenceShutdown()
		{
			try
			{
				Provider.CommenceShutdownHandler commenceShutdownHandler = Provider.onCommenceShutdown;
				if (commenceShutdownHandler != null)
				{
					commenceShutdownHandler();
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onCommenceShutdown:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x00104250 File Offset: 0x00102450
		public static void shutdown()
		{
			Provider.shutdown(0);
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x00104258 File Offset: 0x00102458
		public static void shutdown(int timer)
		{
			Provider.shutdown(timer, string.Empty);
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x00104265 File Offset: 0x00102465
		public static void shutdown(int timer, string explanation)
		{
			Provider.countShutdownTimer = timer;
			Provider.lastTimerMessage = Time.realtimeSinceStartup;
			Provider.shutdownMessage = explanation;
			UnturnedLog.info(string.Format("Set server shutdown timer to {0}s (client message: {1})", timer, explanation));
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x00104293 File Offset: 0x00102493
		internal static bool IsBattlEyeEnabled
		{
			get
			{
				return Provider.configData != null && Provider.configData.Server.BattlEye_Secure && !Dedicator.offlineOnly;
			}
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x001042BC File Offset: 0x001024BC
		public static void RequestDisconnect(string reason)
		{
			UnturnedLog.info("Disconnecting: " + reason);
			Provider.disconnect();
		}

		/// <summary>
		/// Client should call RequestDisconnect instead to ensure all disconnects have a logged reason.
		/// </summary>
		// Token: 0x06003787 RID: 14215 RVA: 0x001042D4 File Offset: 0x001024D4
		public static void disconnect()
		{
			if (Provider.isServer)
			{
				if (Provider.isBattlEyeActive && Provider.battlEyeServerHandle != IntPtr.Zero)
				{
					if (Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnExit != null)
					{
						UnturnedLog.info("Shutting down BattlEye server");
						bool flag = Provider.battlEyeServerRunData.pfnExit.Invoke();
						UnturnedLog.info("BattlEye server shutdown result: {0}", new object[]
						{
							flag
						});
					}
					BEServer.FreeLibrary(Provider.battlEyeServerHandle);
					Provider.battlEyeServerHandle = IntPtr.Zero;
				}
				if (Provider.serverTransport != null)
				{
					Provider.serverTransport.TearDown();
				}
				Provider.closeGameServer();
				if (Provider.isClient)
				{
					Provider._client = Provider.user;
					Provider._clientHash = Hash.SHA1(Provider.client);
				}
				Provider._isServer = false;
				Provider._isClient = false;
			}
			else if (Provider.isClient)
			{
				if (Provider.battlEyeClientHandle != IntPtr.Zero)
				{
					if (Provider.battlEyeClientRunData != null && Provider.battlEyeClientRunData.pfnExit != null)
					{
						UnturnedLog.info("Shutting down BattlEye client");
						bool flag2 = Provider.battlEyeClientRunData.pfnExit.Invoke();
						UnturnedLog.info("BattlEye client shutdown result: {0}", new object[]
						{
							flag2
						});
					}
					BEClient.FreeLibrary(Provider.battlEyeClientHandle);
					Provider.battlEyeClientHandle = IntPtr.Zero;
				}
				NetMessages.SendMessageToServer(EServerMessage.GracefullyDisconnect, ENetReliability.Reliable, delegate(NetPakWriter writer)
				{
				});
				Provider.clientTransport.TearDown();
				SteamFriends.SetRichPresence("connect", "");
				Lobbies.leaveLobby();
				Provider.CancelAllSteamAuthTickets();
				SteamUser.AdvertiseGame(CSteamID.Nil, 0U, 0);
				Provider._server = default(CSteamID);
				Provider._isServer = false;
				Provider._isClient = false;
			}
			Provider.ClientDisconnected clientDisconnected = Provider.onClientDisconnected;
			if (clientDisconnected != null)
			{
				clientDisconnected();
			}
			if (!Provider.isApplicationQuitting)
			{
				Provider.authorityHoliday = HolidayUtil.BackendGetActiveHoliday();
				Level.exit();
			}
			Assets.ClearServerAssetMapping();
			Provider.unloadGameMode();
			Provider._isConnected = false;
			Provider.isWaitingForConnectResponse = false;
			Provider.isWaitingForWorkshopResponse = false;
			Provider.isLoadingUGC = false;
			Provider.isLoadingInventory = true;
			UnturnedLog.info("Disconnected");
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x001044E0 File Offset: 0x001026E0
		[Obsolete]
		public static void sendGUIDTable(SteamPending player)
		{
			Provider.accept(player);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x001044E8 File Offset: 0x001026E8
		private static bool initializeBattlEyeServer()
		{
			string text = ReadWrite.PATH + "/BattlEye/BEServer_x64.dll";
			if (!File.Exists(text))
			{
				text = ReadWrite.PATH + "/BattlEye/BEServer.dll";
			}
			if (!File.Exists(text))
			{
				UnturnedLog.error("Missing BattlEye server library! (" + text + ")");
				return false;
			}
			UnturnedLog.info("Loading BattlEye server library from: " + text);
			bool result;
			try
			{
				Provider.battlEyeServerHandle = BEServer.LoadLibraryW(text);
				if (Provider.battlEyeServerHandle != IntPtr.Zero)
				{
					BEServer.BEServerInitFn beserverInitFn = Marshal.GetDelegateForFunctionPointer(BEServer.GetProcAddress(Provider.battlEyeServerHandle, "Init"), typeof(BEServer.BEServerInitFn)) as BEServer.BEServerInitFn;
					if (beserverInitFn != null)
					{
						Provider.battlEyeServerInitData = new BEServer.BESV_GAME_DATA();
						Provider.battlEyeServerInitData.pstrGameVersion = Provider.APP_NAME + " " + Provider.APP_VERSION;
						Provider.battlEyeServerInitData.pfnPrintMessage = new BEServer.BESV_GAME_DATA.PrintMessageFn(Provider.battlEyeServerPrintMessage);
						Provider.battlEyeServerInitData.pfnKickPlayer = new BEServer.BESV_GAME_DATA.KickPlayerFn(Provider.battlEyeServerKickPlayer);
						Provider.battlEyeServerInitData.pfnSendPacket = new BEServer.BESV_GAME_DATA.SendPacketFn(Provider.battlEyeServerSendPacket);
						Provider.battlEyeServerRunData = new BEServer.BESV_BE_DATA();
						if (beserverInitFn.Invoke(0, Provider.battlEyeServerInitData, Provider.battlEyeServerRunData))
						{
							result = true;
						}
						else
						{
							BEServer.FreeLibrary(Provider.battlEyeServerHandle);
							Provider.battlEyeServerHandle = IntPtr.Zero;
							UnturnedLog.error("Failed to call BattlEye server init!");
							result = false;
						}
					}
					else
					{
						BEServer.FreeLibrary(Provider.battlEyeServerHandle);
						Provider.battlEyeServerHandle = IntPtr.Zero;
						UnturnedLog.error("Failed to get BattlEye server init delegate!");
						result = false;
					}
				}
				else
				{
					UnturnedLog.error("Failed to load BattlEye server library!");
					result = false;
				}
			}
			catch (Exception e)
			{
				UnturnedLog.error("Unhandled exception when loading BattlEye server library!");
				UnturnedLog.exception(e);
				result = false;
			}
			return result;
		}

		/// <summary>
		/// Internet server callback when backend is ready.
		/// </summary>
		// Token: 0x0600378A RID: 14218 RVA: 0x001046A4 File Offset: 0x001028A4
		private static void handleServerReady()
		{
			if (Provider.isServerConnectedToSteam)
			{
				return;
			}
			Provider.isServerConnectedToSteam = true;
			CommandWindow.Log("Steam servers ready!");
			Provider.initializeDedicatedUGC();
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x001046C4 File Offset: 0x001028C4
		private static void initializeDedicatedUGC()
		{
			WorkshopDownloadConfig orLoad = WorkshopDownloadConfig.getOrLoad();
			DedicatedUGC.initialize();
			if (Assets.shouldLoadAnyAssets)
			{
				foreach (ulong id in orLoad.File_IDs)
				{
					DedicatedUGC.registerItemInstallation(id);
				}
			}
			DedicatedUGC.installed += Provider.onDedicatedUGCInstalled;
			DedicatedUGC.beginInstallingItems(Dedicator.offlineOnly);
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x0010474C File Offset: 0x0010294C
		public static string getModeTagAbbreviation(EGameMode gm)
		{
			switch (gm)
			{
			case EGameMode.EASY:
				return "EZY";
			case EGameMode.NORMAL:
				return "NRM";
			case EGameMode.HARD:
				return "HRD";
			default:
				return null;
			}
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00104775 File Offset: 0x00102975
		public static string getCameraModeTagAbbreviation(ECameraMode cm)
		{
			switch (cm)
			{
			case ECameraMode.FIRST:
				return "1Pp";
			case ECameraMode.THIRD:
				return "3Pp";
			case ECameraMode.BOTH:
				return "2Pp";
			case ECameraMode.VEHICLE:
				return "4Pp";
			default:
				return null;
			}
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x001047A8 File Offset: 0x001029A8
		public static string GetMonetizationTagAbbreviation(EServerMonetizationTag monetization)
		{
			switch (monetization)
			{
			case EServerMonetizationTag.None:
				return "MTXn";
			case EServerMonetizationTag.NonGameplay:
				return "MTXy";
			case EServerMonetizationTag.Monetized:
				return "MTXg";
			default:
				return null;
			}
		}

		/// <summary>
		/// If missing map is a curated map then log information about how to install it.
		/// </summary>
		// Token: 0x0600378F RID: 14223 RVA: 0x001047D4 File Offset: 0x001029D4
		private static void maybeLogCuratedMapFallback(string attemptedMap)
		{
			if (Provider.statusData == null || Provider.statusData.Maps == null || Provider.statusData.Maps.Curated_Map_Links == null)
			{
				return;
			}
			foreach (CuratedMapLink curatedMapLink in Provider.statusData.Maps.Curated_Map_Links)
			{
				if (curatedMapLink.Name.Equals(attemptedMap, 3))
				{
					CommandWindow.LogWarningFormat("Attempting to load curated map '{0}'? Include its workshop file ID ({1}) in the WorkshopDownloadConfig.json File_IDs array.", new object[]
					{
						curatedMapLink.Name,
						curatedMapLink.Workshop_File_Id
					});
					break;
				}
			}
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x00104888 File Offset: 0x00102A88
		private static void onDedicatedUGCInstalled()
		{
			if (Provider.isDedicatedUGCInstalled)
			{
				return;
			}
			Provider.isDedicatedUGCInstalled = true;
			Provider.apiWarningMessageHook = new SteamAPIWarningMessageHook_t(Provider.onAPIWarningMessage);
			SteamGameServerUtils.SetWarningMessageHook(Provider.apiWarningMessageHook);
			Provider.time = SteamGameServerUtils.GetServerRealTime();
			LevelInfo level = Level.getLevel(Provider.map);
			if (level == null)
			{
				string text = Provider.map;
				Provider.maybeLogCuratedMapFallback(text);
				Provider.map = "PEI";
				CommandWindow.LogError(Provider.localization.format("Map_Missing", text, Provider.map));
				level = Level.getLevel(Provider.map);
				if (level == null)
				{
					CommandWindow.LogError("Fatal error: unable to load fallback map");
				}
			}
			if (level != null)
			{
				Provider.map = level.name;
			}
			List<PublishedFileId_t> list = null;
			if (Provider._serverWorkshopFileIDs != null)
			{
				list = new List<PublishedFileId_t>(Provider._serverWorkshopFileIDs.Count);
				foreach (ulong value in Provider._serverWorkshopFileIDs)
				{
					list.Add(new PublishedFileId_t(value));
				}
			}
			Assets.ApplyServerAssetMapping(level, list);
			PhysicsMaterialNetTable.ServerPopulateTable();
			Level.load(level, true);
			Provider.loadGameMode();
			Provider.applyLevelModeConfigOverrides();
			SteamGameServer.SetMaxPlayerCount((int)Provider.maxPlayers);
			SteamGameServer.SetServerName(Provider.serverName);
			SteamGameServer.SetPasswordProtected(Provider.serverPassword != "");
			SteamGameServer.SetMapName(Provider.map);
			if (!ReadWrite.folderExists("/Bundles/Workshop/Content", true))
			{
				ReadWrite.createFolder("/Bundles/Workshop/Content", true);
			}
			string text2 = "/Bundles/Workshop/Content";
			string[] folders = ReadWrite.getFolders(text2);
			for (int i = 0; i < folders.Length; i++)
			{
				string text3 = ReadWrite.folderName(folders[i]);
				ulong id;
				if (ulong.TryParse(text3, 511, CultureInfo.InvariantCulture, ref id))
				{
					Provider.registerServerUsingWorkshopFileId(id);
					CommandWindow.Log("Recommended to add workshop item " + id.ToString() + " to WorkshopDownloadConfig.json and remove it from " + text2);
				}
				else
				{
					CommandWindow.LogWarning("Invalid workshop item '" + text3 + "' in " + text2);
				}
			}
			string text4 = ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Content";
			if (!ReadWrite.folderExists(text4, true))
			{
				ReadWrite.createFolder(text4, true);
			}
			string[] folders2 = ReadWrite.getFolders(text4);
			for (int j = 0; j < folders2.Length; j++)
			{
				string text5 = ReadWrite.folderName(folders2[j]);
				ulong id2;
				if (ulong.TryParse(text5, 511, CultureInfo.InvariantCulture, ref id2))
				{
					Provider.registerServerUsingWorkshopFileId(id2);
					CommandWindow.Log("Recommended to add workshop item " + id2.ToString() + " to WorkshopDownloadConfig.json and remove it from " + text4);
				}
				else
				{
					CommandWindow.LogWarning("Invalid workshop item '" + text5 + "' in " + text4);
				}
			}
			ulong id3;
			if (ulong.TryParse(new DirectoryInfo(Level.info.path).Parent.Name, 511, CultureInfo.InvariantCulture, ref id3))
			{
				Provider.registerServerUsingWorkshopFileId(id3);
			}
			SteamGameServer.SetGameData(((Provider.serverPassword != "") ? "PASS" : "SSAP") + "," + (Provider.configData.Server.VAC_Secure ? "VAC_ON" : "VAC_OFF") + ",GAME_VERSION_" + VersionUtils.binaryToHexadecimal(Provider.APP_VERSION_PACKED) + ",MAP_VERSION_" + VersionUtils.binaryToHexadecimal(Level.packedVersion));
			SteamGameServer.SetKeyValue("GameVersion", Provider.APP_VERSION);
			int num = 128;
			string text6 = string.Concat(new string[]
			{
				Provider.isPvP ? "PVP" : "PVE",
				",",
				Provider.hasCheats ? "CHy" : "CHn",
				",",
				Provider.getModeTagAbbreviation(Provider.mode),
				",",
				Provider.getCameraModeTagAbbreviation(Provider.cameraMode),
				",",
				(Provider.getServerWorkshopFileIDs().Count > 0) ? "WSy" : "WSn",
				",",
				Provider.isGold ? "GLD" : "F2P"
			});
			text6 = text6 + "," + (Provider.isBattlEyeActive ? "BEy" : "BEn");
			if (!Provider.hasSetIsBattlEyeActive)
			{
				CommandWindow.LogError("Order of things is messed up! isBattlEyeActive should have been set before advertising game server.");
			}
			string monetizationTagAbbreviation = Provider.GetMonetizationTagAbbreviation(Provider.configData.Browser.Monetization);
			if (!string.IsNullOrEmpty(monetizationTagAbbreviation))
			{
				text6 = text6 + "," + monetizationTagAbbreviation;
			}
			if (!string.IsNullOrEmpty(Provider.configData.Browser.Thumbnail))
			{
				text6 = text6 + ",<tn>" + Provider.configData.Browser.Thumbnail + "</tn>";
			}
			text6 += string.Format(",<net>{0}</net>", NetTransportFactory.GetTag(Provider.serverTransport));
			string pluginFrameworkTag = SteamPluginAdvertising.Get().PluginFrameworkTag;
			if (!string.IsNullOrEmpty(pluginFrameworkTag))
			{
				text6 += string.Format(",<pf>{0}</pf>", pluginFrameworkTag);
			}
			if (text6.Length > num)
			{
				CommandWindow.LogWarning("Server browser thumbnail URL is " + (text6.Length - num).ToString() + " characters over budget!");
				CommandWindow.LogWarning("Server will not list properly until this URL is adjusted!");
			}
			SteamGameServer.SetGameTags(text6);
			int num2 = 64;
			if (Provider.configData.Browser.Desc_Server_List.Length > num2)
			{
				CommandWindow.LogWarning("Server browser description is " + (Provider.configData.Browser.Desc_Server_List.Length - num2).ToString() + " characters over budget!");
			}
			SteamGameServer.SetGameDescription(Provider.configData.Browser.Desc_Server_List);
			SteamGameServer.SetKeyValue("Browser_Icon", Provider.configData.Browser.Icon);
			SteamGameServer.SetKeyValue("Browser_Desc_Hint", Provider.configData.Browser.Desc_Hint);
			SteamGameServer.SetKeyValue("BookmarkHost", Provider.configData.Browser.BookmarkHost);
			Provider.AdvertiseFullDescription(Provider.configData.Browser.Desc_Full);
			if (Provider.getServerWorkshopFileIDs().Count > 0)
			{
				string text7 = string.Empty;
				for (int k = 0; k < Provider.getServerWorkshopFileIDs().Count; k++)
				{
					if (text7.Length > 0)
					{
						text7 += ",";
					}
					text7 += Provider.getServerWorkshopFileIDs()[k].ToString();
				}
				int num3 = (text7.Length - 1) / 127 + 1;
				int num4 = 0;
				SteamGameServer.SetKeyValue("Mod_Count", num3.ToString());
				for (int l = 0; l < text7.Length; l += 127)
				{
					int num5 = 127;
					if (l + num5 > text7.Length)
					{
						num5 = text7.Length - l;
					}
					string pValue = text7.Substring(l, num5);
					SteamGameServer.SetKeyValue("Mod_" + num4.ToString(), pValue);
					num4++;
				}
			}
			if (Provider.configData.Browser.Links != null && Provider.configData.Browser.Links.Length != 0)
			{
				SteamGameServer.SetKeyValue("Custom_Links_Count", Provider.configData.Browser.Links.Length.ToString());
				for (int m = 0; m < Provider.configData.Browser.Links.Length; m++)
				{
					BrowserConfigData.Link link = Provider.configData.Browser.Links[m];
					string pValue2;
					string pValue3;
					if (!ConvertEx.TryEncodeUtf8StringAsBase64(link.Message, ref pValue2))
					{
						UnturnedLog.error("Unable to encode lobby link message as Base64: \"" + link.Message + "\"");
					}
					else if (!ConvertEx.TryEncodeUtf8StringAsBase64(link.Url, ref pValue3))
					{
						UnturnedLog.error("Unable to encode lobby link url as Base64: \"" + link.Url + "\"");
					}
					else
					{
						SteamGameServer.SetKeyValue("Custom_Link_Message_" + m.ToString(), pValue2);
						SteamGameServer.SetKeyValue("Custom_Link_Url_" + m.ToString(), pValue3);
					}
				}
			}
			Provider.AdvertiseConfig();
			SteamPluginAdvertising.Get().NotifyGameServerReady();
			Provider.dswUpdateMonitor = DedicatedWorkshopUpdateMonitorFactory.createForLevel(level);
			Provider._server = SteamGameServer.GetSteamID();
			Provider._client = Provider._server;
			Provider._clientHash = Hash.SHA1(Provider.client);
			Provider._clientName = Provider.localization.format("Console");
			Provider.autoShutdownManager = Provider.steam.gameObject.AddComponent<BuiltinAutoShutdown>();
			uint num6;
			SteamGameServer.GetPublicIP().TryGetIPv4Address(out num6);
			EHostBanFlags ehostBanFlags = HostBansManager.Get().MatchBasicDetails(new IPv4Address(num6), Provider.port, Provider.serverName, Provider._server.m_SteamID);
			ehostBanFlags |= HostBansManager.Get().MatchExtendedDetails(Provider.configData.Browser.Desc_Server_List, Provider.configData.Browser.Thumbnail);
			if ((ehostBanFlags & 138) != null)
			{
				CommandWindow.LogWarning("It appears this server has received a warning.");
				CommandWindow.LogWarning("Checking the Unturned Server Standing page is recommended:");
				CommandWindow.LogWarning("https://smartlydressedgames.com/UnturnedHostBans/index.html");
			}
			if (ehostBanFlags.HasFlag(4))
			{
				Provider.shutdown();
			}
			Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Set key/value tags on Steam server advertisement so that client can display text in browser.
		/// </summary>
		// Token: 0x06003791 RID: 14225 RVA: 0x00105194 File Offset: 0x00103394
		private static void AdvertiseFullDescription(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			string text;
			if (!ConvertEx.TryEncodeUtf8StringAsBase64(message, ref text))
			{
				UnturnedLog.error("Unable to encode server browser description to Base64");
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				UnturnedLog.error("Base64 server browser description was empty");
				return;
			}
			int num = (text.Length - 1) / 127 + 1;
			int num2 = 0;
			SteamGameServer.SetKeyValue("Browser_Desc_Full_Count", num.ToString());
			for (int i = 0; i < text.Length; i += 127)
			{
				int num3 = 127;
				if (i + num3 > text.Length)
				{
					num3 = text.Length - i;
				}
				string pValue = text.Substring(i, num3);
				SteamGameServer.SetKeyValue("Browser_Desc_Full_Line_" + num2.ToString(), pValue);
				num2++;
			}
		}

		/// <summary>
		/// Set key/value tags on Steam server advertisement so that client can display server config in browser.
		/// </summary>
		// Token: 0x06003792 RID: 14226 RVA: 0x00105248 File Offset: 0x00103448
		private static void AdvertiseConfig()
		{
			ModeConfigData modeConfig = ConfigData.CreateDefault(false).getModeConfig(Provider.mode);
			if (modeConfig == null)
			{
				CommandWindow.LogError("Unable to compare default for advertise config");
				return;
			}
			int num = 0;
			foreach (FieldInfo fieldInfo in Provider.modeConfigData.GetType().GetFields())
			{
				object value = fieldInfo.GetValue(Provider.modeConfigData);
				object value2 = fieldInfo.GetValue(modeConfig);
				foreach (FieldInfo fieldInfo2 in value.GetType().GetFields())
				{
					object value3 = fieldInfo2.GetValue(value);
					object value4 = fieldInfo2.GetValue(value2);
					string text = null;
					Type fieldType = fieldInfo2.FieldType;
					if (fieldType == typeof(bool))
					{
						bool flag = (bool)value3;
						bool flag2 = (bool)value4;
						if (flag != flag2)
						{
							text = string.Concat(new string[]
							{
								fieldInfo.Name,
								".",
								fieldInfo2.Name,
								"=",
								flag ? "T" : "F"
							});
						}
					}
					else if (fieldType == typeof(float))
					{
						float a = (float)value3;
						float b = (float)value4;
						if (!MathfEx.IsNearlyEqual(a, b, 0.0001f))
						{
							text = string.Concat(new string[]
							{
								fieldInfo.Name,
								".",
								fieldInfo2.Name,
								"=",
								a.ToString(CultureInfo.InvariantCulture)
							});
						}
					}
					else if (fieldType == typeof(uint))
					{
						uint num2 = (uint)value3;
						uint num3 = (uint)value4;
						if (num2 != num3)
						{
							text = string.Concat(new string[]
							{
								fieldInfo.Name,
								".",
								fieldInfo2.Name,
								"=",
								num2.ToString(CultureInfo.InvariantCulture)
							});
						}
					}
					else
					{
						CommandWindow.LogErrorFormat("Unable to advertise config type: {0}", new object[]
						{
							fieldType
						});
					}
					if (!string.IsNullOrEmpty(text))
					{
						string pKey = "Cfg_" + num.ToString(CultureInfo.InvariantCulture);
						num++;
						SteamGameServer.SetKeyValue(pKey, text);
					}
				}
			}
			SteamGameServer.SetKeyValue("Cfg_Count", num.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Primarily kept for backwards compatibility with plugins. Some RPCs that reply to sender also use this but
		/// should be tidied up.
		/// </summary>
		// Token: 0x06003793 RID: 14227 RVA: 0x001054C0 File Offset: 0x001036C0
		[Obsolete]
		public static void send(CSteamID steamID, ESteamPacket type, byte[] packet, int size, int channel)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				Provider.sendToClient(transportConnection, type, packet, size);
			}
		}

		/// <summary>
		/// Hack to deal with the oversight of reordering the ESteamPacket enum during net messaging rewrite causing
		/// older plugins to send wrong packet type.
		/// </summary>
		// Token: 0x06003794 RID: 14228 RVA: 0x001054E0 File Offset: 0x001036E0
		[Obsolete]
		private static bool remapSteamPacketType(ref ESteamPacket type)
		{
			ESteamPacket esteamPacket = type;
			if (esteamPacket == ESteamPacket.KICKED)
			{
				type = ESteamPacket.UPDATE_RELIABLE_BUFFER;
				return true;
			}
			if (esteamPacket != ESteamPacket.CONNECTED)
			{
				return false;
			}
			type = ESteamPacket.UPDATE_UNRELIABLE_BUFFER;
			return true;
		}

		/// <summary>
		/// Send to a connected client.
		/// </summary>
		// Token: 0x06003795 RID: 14229 RVA: 0x00105508 File Offset: 0x00103708
		[Obsolete]
		public static void sendToClient(ITransportConnection transportConnection, ESteamPacket type, byte[] packet, int size)
		{
			if (size < 1)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (transportConnection == null)
			{
				throw new ArgumentNullException("transportConnection");
			}
			ThreadUtil.assertIsGameThread();
			if (!Provider.isConnected)
			{
				return;
			}
			if (!Provider.isServer)
			{
				throw new NotSupportedException("Sending to client while not running as server");
			}
			if (Provider.remapSteamPacketType(ref type))
			{
				packet[0] = (byte)type;
			}
			Provider._bytesSent += (uint)size;
			Provider._packetsSent += 1U;
			ENetReliability reliability;
			if (Provider.isUnreliable(type))
			{
				reliability = ENetReliability.Unreliable;
			}
			else
			{
				reliability = ENetReliability.Reliable;
			}
			transportConnection.Send(packet, (long)size, reliability);
		}

		/// <summary>
		/// Hacked-together initial implementation to refuse network messages from specific players.
		/// On PC some cheats send garbage packets in which case those clients should be blocked.
		/// </summary>
		// Token: 0x06003796 RID: 14230 RVA: 0x0010558F File Offset: 0x0010378F
		public static bool shouldNetIgnoreSteamId(CSteamID id)
		{
			return Provider.netIgnoredSteamIDs.Contains(id);
		}

		/// <summary>
		/// Close connection, and refuse all future connection attempts from a remote player.
		/// Used when garbage messages are received from hacked clients to avoid wasting time on them.
		/// </summary>
		// Token: 0x06003797 RID: 14231 RVA: 0x0010559C File Offset: 0x0010379C
		public static void refuseGarbageConnection(CSteamID remoteId, string reason)
		{
			string[] array = new string[5];
			array[0] = "Refusing connections from ";
			int num = 1;
			CSteamID csteamID = remoteId;
			array[num] = csteamID.ToString();
			array[2] = " (";
			array[3] = reason;
			array[4] = ")";
			UnturnedLog.info(string.Concat(array));
			Provider.netIgnoredSteamIDs.Add(remoteId);
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x001055F4 File Offset: 0x001037F4
		public static void refuseGarbageConnection(ITransportConnection transportConnection, string reason)
		{
			if (transportConnection == null)
			{
				throw new ArgumentNullException("transportConnection");
			}
			transportConnection.CloseConnection();
			CSteamID csteamID = Provider.findTransportConnectionSteamId(transportConnection);
			if (csteamID != CSteamID.Nil)
			{
				Provider.refuseGarbageConnection(csteamID, reason);
			}
		}

		/// <summary>
		/// Should buffers used by plugin network events be read-only copies?
		/// </summary>
		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003799 RID: 14233 RVA: 0x00105630 File Offset: 0x00103830
		public static bool useConstNetEvents
		{
			get
			{
				return Provider._constNetEvents;
			}
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x0010563C File Offset: 0x0010383C
		public static bool hasNetBufferChanged(byte[] original, byte[] copy, int offset, int size)
		{
			for (int i = offset + size - 1; i >= offset; i--)
			{
				if (copy[i] != original[i])
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// First four bytes of RPC messages are the channel id.
		/// </summary>
		// Token: 0x0600379B RID: 14235 RVA: 0x00105664 File Offset: 0x00103864
		internal static bool getChannelHeader(byte[] packet, int size, int offset, out int channel)
		{
			int num = offset + 2;
			if (num + 1 > size)
			{
				channel = -1;
				return false;
			}
			channel = (int)packet[num];
			return true;
		}

		/// <summary>
		/// Is version number supplied by client compatible with us?
		/// </summary>
		// Token: 0x0600379C RID: 14236 RVA: 0x00105686 File Offset: 0x00103886
		internal static bool canClientVersionJoinServer(uint version)
		{
			return version == Provider.APP_VERSION_PACKED;
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00105690 File Offset: 0x00103890
		internal static void legacyReceiveClient(byte[] packet, int offset, int size)
		{
			CSteamID server = Provider.server;
			Provider._bytesReceived += (uint)size;
			Provider._packetsReceived += 1U;
			int id;
			if (!Provider.getChannelHeader(packet, size, offset, out id))
			{
				return;
			}
			SteamChannel steamChannel = Provider.findChannelComponent(id);
			if (steamChannel != null)
			{
				steamChannel.receive(server, packet, offset, size);
			}
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x001056E4 File Offset: 0x001038E4
		private static void listenServer()
		{
			long num;
			ITransportConnection transportConnection;
			while (Provider.serverTransport.Receive(Provider.buffer, out num, out transportConnection))
			{
				NetMessages.ReceiveMessageFromClient(transportConnection, Provider.buffer, 0, (int)num);
			}
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x00105718 File Offset: 0x00103918
		private static void listenClient()
		{
			long num;
			while (Provider.clientTransport.Receive(Provider.buffer, out num))
			{
				NetMessages.ReceiveMessageFromServer(Provider.buffer, 0, (int)num);
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x00105748 File Offset: 0x00103948
		private void SendPingRequestToAllClients()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (realtimeSinceStartup - steamPlayer.timeLastPingRequestWasSentToClient > 1f || steamPlayer.timeLastPingRequestWasSentToClient < 0f)
				{
					steamPlayer.timeLastPingRequestWasSentToClient = realtimeSinceStartup;
					NetMessages.SendMessageToClient(EClientMessage.PingRequest, ENetReliability.Unreliable, steamPlayer.transportConnection, delegate(NetPakWriter writer)
					{
					});
				}
			}
		}

		/// <summary>
		/// Notify players waiting to join server if their position in the queue has changed.
		/// </summary>
		// Token: 0x060037A1 RID: 14241 RVA: 0x001057E8 File Offset: 0x001039E8
		private void NotifyClientsInQueueOfPosition()
		{
			int queuePosition;
			int queuePosition2;
			for (queuePosition = 0; queuePosition < Provider.pending.Count; queuePosition = queuePosition2)
			{
				if (Provider.pending[queuePosition].lastNotifiedQueuePosition != queuePosition)
				{
					Provider.pending[queuePosition].lastNotifiedQueuePosition = queuePosition;
					NetMessages.SendMessageToClient(EClientMessage.QueuePositionChanged, ENetReliability.Reliable, Provider.pending[queuePosition].transportConnection, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, MathfEx.ClampToByte(queuePosition));
					});
				}
				queuePosition2 = queuePosition + 1;
			}
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x0010588C File Offset: 0x00103A8C
		private void KickClientsWithBadConnection()
		{
			this.clientsWithBadConnecion.Clear();
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				float num5 = realtimeSinceStartup - steamPlayer.timeLastPacketWasReceivedFromClient;
				if (num5 > Provider.configData.Server.Timeout_Game_Seconds)
				{
					if (CommandWindow.shouldLogJoinLeave)
					{
						SteamPlayerID playerID = steamPlayer.playerID;
						CommandWindow.Log(Provider.localization.format("Dismiss_Timeout", playerID.steamID, playerID.playerName, playerID.characterName));
					}
					UnturnedLog.info(string.Format("Kicking {0} after {1} s without message", steamPlayer.transportConnection, num5));
					this.clientsWithBadConnecion.Add(steamPlayer);
					num += num5;
					num2++;
				}
				else if (realtimeSinceStartup - steamPlayer.joined > Provider.configData.Server.Timeout_Game_Seconds)
				{
					int num6 = Mathf.FloorToInt(steamPlayer.ping * 1000f);
					if ((long)num6 > (long)((ulong)Provider.configData.Server.Max_Ping_Milliseconds))
					{
						if (CommandWindow.shouldLogJoinLeave)
						{
							SteamPlayerID playerID2 = steamPlayer.playerID;
							CommandWindow.Log(Provider.localization.format("Dismiss_Ping", new object[]
							{
								num6,
								Provider.configData.Server.Max_Ping_Milliseconds,
								playerID2.steamID,
								playerID2.playerName,
								playerID2.characterName
							}));
						}
						UnturnedLog.info(string.Format("Kicking {0} because their ping ({1} ms) exceeds limit ({2} ms)", steamPlayer.transportConnection, num6, Provider.configData.Server.Max_Ping_Milliseconds));
						this.clientsWithBadConnecion.Add(steamPlayer);
						num3 += num6;
						num4++;
					}
				}
			}
			if (this.clientsWithBadConnecion.Count > 1)
			{
				UnturnedLog.info(string.Format("Kicking {0} clients with bad connection this frame. Maybe something blocked the main thread on the server? ({1} clients kicked of {2} total clients)", this.clientsWithBadConnecion.Count, this.clientsWithBadConnecion.Count, Provider.clients.Count));
				float num7 = (num2 > 0) ? (num / (float)this.clientsWithBadConnecion.Count) : 0f;
				UnturnedLog.info(string.Format("Kicking {0} for exceeding timeout limit ({1} s) with average of {2} s without message", num2, Provider.configData.Server.Timeout_Game_Seconds, num7));
				int num8 = (num4 > 0) ? (num3 / num4) : 0;
				UnturnedLog.info(string.Format("Kicking {0} for exceeding ping limit ({1} ms) with average of {2} ms ping", num4, Provider.configData.Server.Max_Ping_Milliseconds, num8));
			}
			foreach (SteamPlayer steamPlayer2 in this.clientsWithBadConnecion)
			{
				try
				{
					Provider.dismiss(steamPlayer2.playerID.steamID);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Caught exception while kicking client for bad connection:");
				}
			}
		}

		/// <summary>
		/// Prevent any particular client from delaying the server connection queue process.
		/// </summary>
		// Token: 0x060037A3 RID: 14243 RVA: 0x00105BF8 File Offset: 0x00103DF8
		private void KickClientsBlockingUpQueue()
		{
			if (Provider.pending.Count < 1)
			{
				return;
			}
			float clampedTimeoutQueueSeconds = Provider.configData.Server.GetClampedTimeoutQueueSeconds();
			SteamPending steamPending = Provider.pending[0];
			if (steamPending.hasSentVerifyPacket && steamPending.realtimeSinceSentVerifyPacket > clampedTimeoutQueueSeconds)
			{
				UnturnedLog.info("Front of queue player timed out: {0} ({1})", new object[]
				{
					steamPending.playerID.steamID,
					steamPending.GetQueueStateDebugString()
				});
				ESteamRejection rejection;
				if (!steamPending.hasAuthentication && steamPending.hasProof && steamPending.hasGroup)
				{
					rejection = ESteamRejection.LATE_PENDING_STEAM_AUTH;
					UnturnedLog.info(string.Format("Server was only waiting for Steam authentication response for front of queue player, but {0}s passed so we will give the next player a chance instead.", steamPending.realtimeSinceSentVerifyPacket));
				}
				else if (steamPending.hasAuthentication && !steamPending.hasProof && steamPending.hasGroup)
				{
					rejection = ESteamRejection.LATE_PENDING_STEAM_ECON;
					UnturnedLog.info(string.Format("Server was only waiting for Steam economy/inventory details response for front of queue player, but {0}s passed so we will give the next player a chance instead.", steamPending.realtimeSinceSentVerifyPacket));
				}
				else if (steamPending.hasAuthentication && steamPending.hasProof && !steamPending.hasGroup)
				{
					rejection = ESteamRejection.LATE_PENDING_STEAM_GROUPS;
					UnturnedLog.info(string.Format("Server was only waiting for Steam group/clan details response for front of queue player, but {0}s passed so we will give the next player a chance instead.", steamPending.realtimeSinceSentVerifyPacket));
				}
				else
				{
					rejection = ESteamRejection.LATE_PENDING;
					UnturnedLog.info(string.Format("Server was waiting for multiple responses about front of queue player, but {0}s passed so we will give the next player a chance instead.", steamPending.realtimeSinceSentVerifyPacket));
				}
				Provider.reject(steamPending.playerID.steamID, rejection);
				return;
			}
			if (Provider.pending.Count > 1)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				for (int i = Provider.pending.Count - 1; i > 0; i--)
				{
					float num = realtimeSinceStartup - Provider.pending[i].lastReceivedPingRequestRealtime;
					if (num > Provider.configData.Server.Timeout_Queue_Seconds)
					{
						SteamPending steamPending2 = Provider.pending[i];
						UnturnedLog.info(string.Format("Kicking queued player {0} after {1}s without message", steamPending2.transportConnection, num));
						Provider.reject(steamPending2.playerID.steamID, ESteamRejection.LATE_PENDING);
						return;
					}
				}
			}
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x00105DE4 File Offset: 0x00103FE4
		private static void listen()
		{
			if (!Provider.isConnected)
			{
				return;
			}
			if (Provider.isServer)
			{
				if (!Level.isLoaded)
				{
					return;
				}
				TransportConnectionListPool.ReleaseAll();
				Provider.listenServer();
				if (Time.realtimeSinceStartup - Provider.lastPingRequestTime > Provider.PING_REQUEST_INTERVAL)
				{
					Provider.lastPingRequestTime = Time.realtimeSinceStartup;
					Provider.steam.SendPingRequestToAllClients();
				}
				if (Time.realtimeSinceStartup - Provider.lastQueueNotificationTime > 6f)
				{
					Provider.lastQueueNotificationTime = Time.realtimeSinceStartup;
					Provider.steam.NotifyClientsInQueueOfPosition();
				}
				Provider.steam.KickClientsWithBadConnection();
				Provider.steam.KickClientsBlockingUpQueue();
				if (Provider.steam.clientsKickedForTransportConnectionFailureCount > 1)
				{
					UnturnedLog.info(string.Format("Removed {0} clients due to transport failure this frame", Provider.steam.clientsKickedForTransportConnectionFailureCount));
				}
				Provider.steam.clientsKickedForTransportConnectionFailureCount = 0;
				if (Provider.dswUpdateMonitor != null)
				{
					Provider.dswUpdateMonitor.tick(Time.deltaTime);
					return;
				}
			}
			else
			{
				Provider.listenClient();
				if (!Provider.isConnected)
				{
					return;
				}
				if (Time.realtimeSinceStartup - Provider.lastPingRequestTime > Provider.PING_REQUEST_INTERVAL && (Time.realtimeSinceStartup - Provider.timeLastPingRequestWasSentToServer > 1f || Provider.timeLastPingRequestWasSentToServer < 0f))
				{
					Provider.lastPingRequestTime = Time.realtimeSinceStartup;
					Provider.timeLastPingRequestWasSentToServer = Time.realtimeSinceStartup;
					NetMessages.SendMessageToServer(EServerMessage.PingRequest, ENetReliability.Unreliable, delegate(NetPakWriter writer)
					{
					});
				}
				if (Provider.isLoadingUGC)
				{
					if (Provider.isWaitingForWorkshopResponse)
					{
						float num = Time.realtimeSinceStartup - Provider.timeLastPacketWasReceivedFromServer;
						if (num > (float)Provider.CLIENT_TIMEOUT)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
							Provider.RequestDisconnect(string.Format("Server did not reply to workshop details request ({0}s elapsed)", num));
						}
						return;
					}
					Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
					return;
				}
				else if (Level.isLoading)
				{
					float num2 = Time.realtimeSinceStartup - Provider.timeLastPacketWasReceivedFromServer;
					if (Provider.isWaitingForConnectResponse && num2 > 10f)
					{
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
						Provider.RequestDisconnect(string.Format("Server did not reply to connection request ({0}s elapsed)", num2));
						return;
					}
					Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
					return;
				}
				else
				{
					float num3 = Time.realtimeSinceStartup - Provider.timeLastPacketWasReceivedFromServer;
					if (num3 > (float)Provider.CLIENT_TIMEOUT)
					{
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
						Provider.RequestDisconnect(string.Format("it has been {0}s without a message from the server", num3));
						return;
					}
					if (Provider.battlEyeHasRequiredRestart)
					{
						Provider.battlEyeHasRequiredRestart = false;
						Provider.RequestDisconnect("BattlEye required restart");
						return;
					}
					if (Provider.catPouncingMechanism > -0.5f)
					{
						Provider.catPouncingMechanism -= Time.deltaTime;
						if (Provider.catPouncingMechanism < 0.01f)
						{
							Provider.catPouncingMechanism = -66f;
							Provider.RequestDisconnect(Random.Range(1, 256).ToString());
							return;
						}
					}
					ClientAssetIntegrity.SendRequests();
				}
			}
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x0010606C File Offset: 0x0010426C
		private static void broadcastServerDisconnected(CSteamID steamID)
		{
			try
			{
				Provider.ServerDisconnected serverDisconnected = Provider.onServerDisconnected;
				if (serverDisconnected != null)
				{
					serverDisconnected(steamID);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onServerDisconnected:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x001060B0 File Offset: 0x001042B0
		private static void broadcastServerHosted()
		{
			try
			{
				Provider.ServerHosted serverHosted = Provider.onServerHosted;
				if (serverHosted != null)
				{
					serverHosted();
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onServerHosted:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x001060F0 File Offset: 0x001042F0
		private static void broadcastServerShutdown()
		{
			try
			{
				Provider.ServerShutdown serverShutdown = Provider.onServerShutdown;
				if (serverShutdown != null)
				{
					serverShutdown();
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onServerShutdown:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x00106130 File Offset: 0x00104330
		private static void onP2PSessionConnectFail(P2PSessionConnectFail_t callback)
		{
			UnturnedLog.info(string.Format("Removing player {0} due to P2P connect failure (Error: {1})", callback.m_steamIDRemote, callback.m_eP2PSessionError));
			Provider.dismiss(callback.m_steamIDRemote);
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x00106164 File Offset: 0x00104364
		internal static void checkBanStatus(SteamPlayerID playerID, uint remoteIP, out bool isBanned, out string banReason, out uint banRemainingDuration)
		{
			isBanned = false;
			banReason = string.Empty;
			banRemainingDuration = 0U;
			SteamBlacklistID steamBlacklistID;
			if (SteamBlacklist.checkBanned(playerID.steamID, remoteIP, playerID.GetHwids(), out steamBlacklistID))
			{
				isBanned = true;
				banReason = steamBlacklistID.reason;
				banRemainingDuration = steamBlacklistID.getTime();
			}
			try
			{
				if (Provider.onCheckBanStatusWithHWID != null)
				{
					Provider.onCheckBanStatusWithHWID(playerID, remoteIP, ref isBanned, ref banReason, ref banRemainingDuration);
				}
				else if (Provider.onCheckBanStatus != null)
				{
					Provider.onCheckBanStatus(playerID.steamID, remoteIP, ref isBanned, ref banReason, ref banRemainingDuration);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onCheckBanStatus:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x00106204 File Offset: 0x00104404
		[Obsolete("Now accepts list of HWIDs to ban")]
		public static bool requestBanPlayer(CSteamID instigator, CSteamID playerToBan, uint ipToBan, string reason, uint duration)
		{
			return Provider.requestBanPlayer(instigator, playerToBan, ipToBan, null, reason, duration);
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x00106214 File Offset: 0x00104414
		public static bool requestBanPlayer(CSteamID instigator, CSteamID playerToBan, uint ipToBan, IEnumerable<byte[]> hwidsToBan, string reason, uint duration)
		{
			bool flag = true;
			try
			{
				Provider.RequestBanPlayerHandler requestBanPlayerHandler = Provider.onBanPlayerRequested;
				if (requestBanPlayerHandler != null)
				{
					requestBanPlayerHandler(instigator, playerToBan, ipToBan, ref reason, ref duration, ref flag);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Plugin raised an exception from onBanPlayerRequested:");
			}
			try
			{
				Provider.RequestBanPlayerHandlerV2 requestBanPlayerHandlerV = Provider.onBanPlayerRequestedV2;
				if (requestBanPlayerHandlerV != null)
				{
					requestBanPlayerHandlerV(instigator, playerToBan, ipToBan, hwidsToBan, ref reason, ref duration, ref flag);
				}
			}
			catch (Exception e2)
			{
				UnturnedLog.exception(e2, "Plugin raised an exception from onBanPlayerRequestedV2:");
			}
			if (flag)
			{
				SteamBlacklist.ban(playerToBan, ipToBan, hwidsToBan, instigator, reason, duration);
			}
			return true;
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x001062A0 File Offset: 0x001044A0
		public static bool requestUnbanPlayer(CSteamID instigator, CSteamID playerToUnban)
		{
			bool flag = true;
			try
			{
				Provider.RequestUnbanPlayerHandler requestUnbanPlayerHandler = Provider.onUnbanPlayerRequested;
				if (requestUnbanPlayerHandler != null)
				{
					requestUnbanPlayerHandler(instigator, playerToUnban, ref flag);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onUnbanPlayerRequested:");
				UnturnedLog.exception(e);
			}
			return !flag || SteamBlacklist.unban(playerToUnban);
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x001062F4 File Offset: 0x001044F4
		private static void handleValidateAuthTicketResponse(ValidateAuthTicketResponse_t callback)
		{
			if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseOK)
			{
				SteamPending steamPending = null;
				for (int i = 0; i < Provider.pending.Count; i++)
				{
					if (Provider.pending[i].playerID.steamID == callback.m_SteamID)
					{
						steamPending = Provider.pending[i];
						break;
					}
				}
				if (steamPending == null)
				{
					for (int j = 0; j < Provider.clients.Count; j++)
					{
						if (Provider.clients[j].playerID.steamID == callback.m_SteamID)
						{
							return;
						}
					}
					Provider.reject(callback.m_SteamID, ESteamRejection.NOT_PENDING);
					return;
				}
				bool flag = true;
				string empty = string.Empty;
				try
				{
					if (Provider.onCheckValidWithExplanation != null)
					{
						Provider.onCheckValidWithExplanation(callback, ref flag, ref empty);
					}
					else if (Provider.onCheckValid != null)
					{
						Provider.onCheckValid(callback, ref flag);
					}
				}
				catch (Exception e)
				{
					UnturnedLog.warn("Plugin raised an exception from onCheckValidWithExplanation or onCheckValid:");
					UnturnedLog.exception(e);
				}
				if (!flag)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.PLUGIN, empty);
					return;
				}
				bool flag2 = SteamGameServer.UserHasLicenseForApp(steamPending.playerID.steamID, Provider.PRO_ID) != EUserHasLicenseForAppResult.k_EUserHasLicenseResultDoesNotHaveLicense;
				if (Provider.isGold && !flag2)
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_SERVER);
					return;
				}
				if ((steamPending.playerID.characterID >= Customization.FREE_CHARACTERS && !flag2) || steamPending.playerID.characterID >= Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS)
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_CHARACTER);
					return;
				}
				if (!flag2 && steamPending.isPro)
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_DESYNC);
					return;
				}
				if (steamPending.face >= Customization.FACES_FREE + Customization.FACES_PRO || (!flag2 && steamPending.face >= Customization.FACES_FREE))
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
					return;
				}
				if (steamPending.hair >= Customization.HAIRS_FREE + Customization.HAIRS_PRO || (!flag2 && steamPending.hair >= Customization.HAIRS_FREE))
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
					return;
				}
				if (steamPending.beard >= Customization.BEARDS_FREE + Customization.BEARDS_PRO || (!flag2 && steamPending.beard >= Customization.BEARDS_FREE))
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
					return;
				}
				if (!flag2)
				{
					if (!Customization.checkSkin(steamPending.skin))
					{
						Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
						return;
					}
					if (!Customization.checkColor(steamPending.color))
					{
						Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
						return;
					}
				}
				steamPending.assignedPro = flag2;
				steamPending.assignedAdmin = SteamAdminlist.checkAdmin(steamPending.playerID.steamID);
				steamPending.hasAuthentication = true;
				if (steamPending.canAcceptYet)
				{
					Provider.accept(steamPending);
					return;
				}
			}
			else
			{
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseUserNotConnectedToSteam)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NO_STEAM);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseNoLicenseOrExpired)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_LICENSE_EXPIRED);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseVACBanned)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_VAC_BAN);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseLoggedInElseWhere)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_ELSEWHERE);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseVACCheckTimedOut)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_TIMED_OUT);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketCanceled)
				{
					if (CommandWindow.shouldLogJoinLeave)
					{
						string text = "Player finished session: ";
						CSteamID steamID = callback.m_SteamID;
						CommandWindow.Log(text + steamID.ToString());
					}
					else
					{
						string text2 = "Player finished session: ";
						CSteamID steamID = callback.m_SteamID;
						UnturnedLog.info(text2 + steamID.ToString());
					}
					Provider.dismiss(callback.m_SteamID);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_USED);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalid)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NO_USER);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponsePublisherIssuedBan)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_PUB_BAN);
					return;
				}
				if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketNetworkIdentityFailure)
				{
					Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NETWORK_IDENTITY_FAILURE);
					return;
				}
				if (CommandWindow.shouldLogJoinLeave)
				{
					string text3 = "Kicking player ";
					CSteamID steamID = callback.m_SteamID;
					CommandWindow.Log(text3 + steamID.ToString() + " for unknown session response " + callback.m_eAuthSessionResponse.ToString());
				}
				else
				{
					string text4 = "Kicking player ";
					CSteamID steamID = callback.m_SteamID;
					UnturnedLog.info(text4 + steamID.ToString() + " for unknown session response " + callback.m_eAuthSessionResponse.ToString());
				}
				Provider.dismiss(callback.m_SteamID);
			}
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x00106784 File Offset: 0x00104984
		private static void onValidateAuthTicketResponse(ValidateAuthTicketResponse_t callback)
		{
			Provider.handleValidateAuthTicketResponse(callback);
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0010678C File Offset: 0x0010498C
		private static void handleClientGroupStatus(GSClientGroupStatus_t callback)
		{
			SteamPending steamPending = null;
			for (int i = 0; i < Provider.pending.Count; i++)
			{
				if (Provider.pending[i].playerID.steamID == callback.m_SteamIDUser)
				{
					steamPending = Provider.pending[i];
					break;
				}
			}
			if (steamPending == null)
			{
				Provider.reject(callback.m_SteamIDUser, ESteamRejection.NOT_PENDING);
				return;
			}
			if (!callback.m_bMember && !callback.m_bOfficer)
			{
				steamPending.playerID.group = CSteamID.Nil;
			}
			steamPending.hasGroup = true;
			if (steamPending.canAcceptYet)
			{
				Provider.accept(steamPending);
			}
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x00106826 File Offset: 0x00104A26
		private static void onClientGroupStatus(GSClientGroupStatus_t callback)
		{
			Provider.handleClientGroupStatus(callback);
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060037B1 RID: 14257 RVA: 0x0010682E File Offset: 0x00104A2E
		// (set) Token: 0x060037B2 RID: 14258 RVA: 0x00106838 File Offset: 0x00104A38
		public static byte maxPlayers
		{
			get
			{
				return Provider._maxPlayers;
			}
			set
			{
				Provider._maxPlayers = value;
				if (Provider.clMaxPlayersLimit.hasValue && (int)Provider.maxPlayers > Provider.clMaxPlayersLimit.value)
				{
					Provider._maxPlayers = (byte)Provider.clMaxPlayersLimit.value;
					UnturnedLog.info("Clamped max players down from {0} to {1}", new object[]
					{
						value,
						Provider.clMaxPlayersLimit.value
					});
				}
				if (Provider.isServer)
				{
					SteamGameServer.SetMaxPlayerCount((int)Provider.maxPlayers);
				}
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060037B3 RID: 14259 RVA: 0x001068B4 File Offset: 0x00104AB4
		public static byte queuePosition
		{
			get
			{
				return Provider._queuePosition;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x001068BB File Offset: 0x00104ABB
		// (set) Token: 0x060037B5 RID: 14261 RVA: 0x001068C2 File Offset: 0x00104AC2
		public static string serverName
		{
			get
			{
				return Provider._serverName;
			}
			set
			{
				Provider._serverName = value;
				if (Dedicator.commandWindow != null)
				{
					Dedicator.commandWindow.title = Provider.serverName;
				}
				if (Provider.isServer)
				{
					SteamGameServer.SetServerName(Provider.serverName);
				}
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060037B6 RID: 14262 RVA: 0x001068F1 File Offset: 0x00104AF1
		// (set) Token: 0x060037B7 RID: 14263 RVA: 0x001068F8 File Offset: 0x00104AF8
		public static string serverID
		{
			get
			{
				return Dedicator.serverID;
			}
			set
			{
				Dedicator.serverID = value;
			}
		}

		/// <summary>
		/// If hosting a server, get the game traffic port.
		/// </summary>
		// Token: 0x060037B8 RID: 14264 RVA: 0x00106900 File Offset: 0x00104B00
		public static ushort GetServerConnectionPort()
		{
			return Provider.port + 1;
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060037B9 RID: 14265 RVA: 0x0010690A File Offset: 0x00104B0A
		public static byte[] serverPasswordHash
		{
			get
			{
				return Provider._serverPasswordHash;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x00106911 File Offset: 0x00104B11
		// (set) Token: 0x060037BB RID: 14267 RVA: 0x00106918 File Offset: 0x00104B18
		public static string serverPassword
		{
			get
			{
				return Provider._serverPassword;
			}
			set
			{
				Provider._serverPassword = value;
				Provider._serverPasswordHash = Hash.SHA1(Provider.serverPassword);
				if (Provider.isServer)
				{
					SteamGameServer.SetPasswordProtected(Provider.serverPassword != "");
				}
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x0010694A File Offset: 0x00104B4A
		public static StatusData statusData
		{
			get
			{
				return Provider._statusData;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060037BD RID: 14269 RVA: 0x00106951 File Offset: 0x00104B51
		public static PreferenceData preferenceData
		{
			get
			{
				return Provider._preferenceData;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x00106958 File Offset: 0x00104B58
		public static ConfigData configData
		{
			get
			{
				return Provider._configData;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060037BF RID: 14271 RVA: 0x0010695F File Offset: 0x00104B5F
		public static ModeConfigData modeConfigData
		{
			get
			{
				return Provider._modeConfigData;
			}
		}

		/// <summary>
		/// Called while running
		/// </summary>
		// Token: 0x060037C0 RID: 14272 RVA: 0x00106968 File Offset: 0x00104B68
		public static void resetConfig()
		{
			Provider._modeConfigData = new ModeConfigData(Provider.mode);
			switch (Provider.mode)
			{
			case EGameMode.EASY:
				Provider.configData.Easy = Provider.modeConfigData;
				break;
			case EGameMode.NORMAL:
				Provider.configData.Normal = Provider.modeConfigData;
				break;
			case EGameMode.HARD:
				Provider.configData.Hard = Provider.modeConfigData;
				break;
			}
			ServerSavedata.serializeJSON<ConfigData>("/Config.json", Provider.configData);
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x001069E0 File Offset: 0x00104BE0
		private static void applyLevelConfigOverride(FieldInfo field, object targetObject, object defaultTargetObject, KeyValuePair<string, object> levelOverride)
		{
			object value = field.GetValue(targetObject);
			object value2 = field.GetValue(defaultTargetObject);
			Type fieldType = field.FieldType;
			bool flag3;
			if (fieldType == typeof(bool))
			{
				bool flag = (bool)value;
				bool flag2 = (bool)value2;
				flag3 = (flag == flag2);
				if (flag3)
				{
					field.SetValue(targetObject, Convert.ToBoolean(levelOverride.Value));
				}
			}
			else if (fieldType == typeof(float))
			{
				float a = (float)value;
				float b = (float)value2;
				flag3 = MathfEx.IsNearlyEqual(a, b, 0.0001f);
				if (flag3)
				{
					field.SetValue(targetObject, Convert.ToSingle(levelOverride.Value));
				}
			}
			else
			{
				if (!(fieldType == typeof(uint)))
				{
					CommandWindow.LogErrorFormat("Unable to handle level mode config override type: {0} ({1})", new object[]
					{
						fieldType,
						levelOverride.Key
					});
					return;
				}
				uint num = (uint)value;
				uint num2 = (uint)value2;
				flag3 = (num == num2);
				if (flag3)
				{
					field.SetValue(targetObject, Convert.ToUInt32(levelOverride.Value));
				}
			}
			if (!flag3)
			{
				CommandWindow.LogFormat("Skipping level config override {0} because server value ({1}) is not the default ({2})", new object[]
				{
					levelOverride.Key,
					value,
					value2
				});
				return;
			}
			CommandWindow.LogFormat("Level overrides config {0}: {1} (Default: {2})", new object[]
			{
				levelOverride.Key,
				levelOverride.Value,
				value2
			});
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x00106B48 File Offset: 0x00104D48
		public static void applyLevelModeConfigOverrides()
		{
			if (Level.info == null || Level.info.configData == null)
			{
				return;
			}
			ModeConfigData modeConfig = ConfigData.CreateDefault(false).getModeConfig(Provider.mode);
			if (modeConfig == null)
			{
				CommandWindow.LogError("Unable to compare default for level mode config overrides");
				return;
			}
			foreach (KeyValuePair<string, object> levelOverride in Level.info.configData.Mode_Config_Overrides)
			{
				if (string.IsNullOrEmpty(levelOverride.Key))
				{
					CommandWindow.LogError("Level mode config overrides contains an empty key");
					break;
				}
				if (levelOverride.Value == null)
				{
					CommandWindow.LogError("Level mode config overrides contains a null value");
					break;
				}
				Type type = typeof(ModeConfigData);
				object obj = Provider.modeConfigData;
				object obj2 = modeConfig;
				string[] array = levelOverride.Key.Split('.', 0);
				int i = 0;
				while (i < array.Length)
				{
					string text = array[i];
					FieldInfo field = type.GetField(text);
					if (field == null)
					{
						CommandWindow.LogError("Failed to find mode config for level override: " + text);
						break;
					}
					if (i == array.Length - 1)
					{
						try
						{
							Provider.applyLevelConfigOverride(field, obj, obj2, levelOverride);
							goto IL_139;
						}
						catch (Exception e)
						{
							CommandWindow.LogError("Exception when applying level config override: " + levelOverride.Key);
							UnturnedLog.exception(e);
							break;
						}
						goto IL_11B;
					}
					goto IL_11B;
					IL_139:
					i++;
					continue;
					IL_11B:
					type = field.FieldType;
					obj = field.GetValue(obj);
					obj2 = field.GetValue(obj2);
					goto IL_139;
				}
			}
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x00106CF0 File Offset: 0x00104EF0
		public static void accept(SteamPending player)
		{
			Provider.accept(player.playerID, player.assignedPro, player.assignedAdmin, player.face, player.hair, player.beard, player.skin, player.color, player.markerColor, player.hand, player.shirtItem, player.pantsItem, player.hatItem, player.backpackItem, player.vestItem, player.maskItem, player.glassesItem, player.skinItems, player.skinTags, player.skinDynamicProps, player.skillset, player.language, player.lobbyID, player.clientPlatform);
		}

		/// <summary>
		/// Used to build packet about each existing player for new player, and then once to build a packet
		/// for existing players about the new player. Note that in this second case forPlayer is null
		/// because the packet is re-used.
		/// </summary>
		// Token: 0x060037C4 RID: 14276 RVA: 0x00106D94 File Offset: 0x00104F94
		private static void WriteConnectedMessage(NetPakWriter writer, SteamPlayer aboutPlayer, SteamPlayer forPlayer)
		{
			writer.WriteNetId(aboutPlayer.GetNetId());
			SteamworksNetPakWriterEx.WriteSteamID(writer, aboutPlayer.playerID.steamID);
			SystemNetPakWriterEx.WriteUInt8(writer, aboutPlayer.playerID.characterID);
			SystemNetPakWriterEx.WriteString(writer, aboutPlayer.playerID.playerName, 11);
			SystemNetPakWriterEx.WriteString(writer, aboutPlayer.playerID.characterName, 11);
			Vector3 vector;
			byte b;
			if (aboutPlayer.player.movement.canAddSimulationResultsToUpdates || !aboutPlayer.player.movement.hasMostRecentlyAddedUpdate)
			{
				vector = aboutPlayer.model.transform.position;
				b = MeasurementTool.angleToByte(aboutPlayer.model.transform.rotation.eulerAngles.y);
			}
			else
			{
				vector = aboutPlayer.player.movement.mostRecentlyAddedUpdate.pos;
				b = aboutPlayer.player.movement.mostRecentlyAddedUpdate.rot;
			}
			UnityNetPakWriterEx.WriteClampedVector3(writer, vector, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBit(aboutPlayer.isPro);
			bool flag = aboutPlayer.isAdmin;
			if (forPlayer != aboutPlayer && Provider.hideAdmins)
			{
				flag = false;
			}
			writer.WriteBit(flag);
			SystemNetPakWriterEx.WriteUInt8(writer, (byte)aboutPlayer.channel);
			SteamworksNetPakWriterEx.WriteSteamID(writer, aboutPlayer.playerID.group);
			SystemNetPakWriterEx.WriteString(writer, aboutPlayer.playerID.nickName, 11);
			SystemNetPakWriterEx.WriteUInt8(writer, aboutPlayer.face);
			SystemNetPakWriterEx.WriteUInt8(writer, aboutPlayer.hair);
			SystemNetPakWriterEx.WriteUInt8(writer, aboutPlayer.beard);
			UnityNetPakWriterEx.WriteColor32RGB(writer, aboutPlayer.skin);
			UnityNetPakWriterEx.WriteColor32RGB(writer, aboutPlayer.color);
			UnityNetPakWriterEx.WriteColor32RGB(writer, aboutPlayer.markerColor);
			writer.WriteBit(aboutPlayer.IsLeftHanded);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.shirtItem);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.pantsItem);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.hatItem);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.backpackItem);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.vestItem);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.maskItem);
			SystemNetPakWriterEx.WriteInt32(writer, aboutPlayer.glassesItem);
			int[] skinItems = aboutPlayer.skinItems;
			SystemNetPakWriterEx.WriteUInt8(writer, (byte)skinItems.Length);
			foreach (int num in skinItems)
			{
				SystemNetPakWriterEx.WriteInt32(writer, num);
			}
			string[] skinTags = aboutPlayer.skinTags;
			SystemNetPakWriterEx.WriteUInt8(writer, (byte)skinTags.Length);
			foreach (string text in skinTags)
			{
				SystemNetPakWriterEx.WriteString(writer, text, 11);
			}
			string[] skinDynamicProps = aboutPlayer.skinDynamicProps;
			SystemNetPakWriterEx.WriteUInt8(writer, (byte)skinDynamicProps.Length);
			foreach (string text2 in skinDynamicProps)
			{
				SystemNetPakWriterEx.WriteString(writer, text2, 11);
			}
			writer.WriteEnum(aboutPlayer.skillset);
			SystemNetPakWriterEx.WriteString(writer, aboutPlayer.language, 11);
		}

		/// <summary>
		/// Not exactly ideal, but this a few old "once per player" client-&gt;server RPCs.
		/// </summary>
		// Token: 0x060037C5 RID: 14277 RVA: 0x00107084 File Offset: 0x00105284
		private static void SendInitialGlobalState(SteamPlayer client)
		{
			PhysicsMaterialNetTable.Send(client.transportConnection);
			LightingManager.SendInitialGlobalState(client);
			VehicleManager.SendInitialGlobalState(client);
			AnimalManager.SendInitialGlobalState(client.transportConnection);
			LevelManager.SendInitialGlobalState(client);
			ZombieManager.SendInitialGlobalState(client);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x001070B4 File Offset: 0x001052B4
		[Obsolete("This should not have been public in the first place")]
		public static void accept(SteamPlayerID playerID, bool isPro, bool isAdmin, byte face, byte hair, byte beard, Color skin, Color color, Color markerColor, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, string[] skinTags, string[] skinDynamicProps, EPlayerSkillset skillset, string language, CSteamID lobbyID)
		{
			Provider.accept(playerID, isPro, isAdmin, face, hair, beard, skin, color, markerColor, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skinTags, skinDynamicProps, skillset, language, lobbyID, EClientPlatform.Windows);
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x001070F4 File Offset: 0x001052F4
		internal static void accept(SteamPlayerID playerID, bool isPro, bool isAdmin, byte face, byte hair, byte beard, Color skin, Color color, Color markerColor, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, string[] skinTags, string[] skinDynamicProps, EPlayerSkillset skillset, string language, CSteamID lobbyID, EClientPlatform clientPlatform)
		{
			ITransportConnection transportConnection = null;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < Provider.pending.Count; i++)
			{
				if (Provider.pending[i].playerID == playerID)
				{
					if (Provider.pending[i].inventoryResult != SteamInventoryResult_t.Invalid)
					{
						SteamGameServerInventory.DestroyResult(Provider.pending[i].inventoryResult);
						Provider.pending[i].inventoryResult = SteamInventoryResult_t.Invalid;
					}
					transportConnection = Provider.pending[i].transportConnection;
					flag = true;
					num = i;
					Provider.pending.RemoveAt(i);
					break;
				}
			}
			if (!flag)
			{
				UnturnedLog.info(string.Format("Ignoring call to accept {0} because they are not in the queue", playerID));
				return;
			}
			UnturnedLog.info(string.Format("Accepting queued player {0}", playerID));
			string characterName = playerID.characterName;
			uint uScore = isPro ? 1U : 0U;
			SteamGameServer.BUpdateUserData(playerID.steamID, characterName, uScore);
			Vector3 point;
			byte angle;
			EPlayerStance initialStance;
			Provider.loadPlayerSpawn(playerID, out point, out angle, out initialStance);
			int channel = Provider.allocPlayerChannelId();
			NetId netId = Provider.ClaimNetIdBlockForNewPlayer();
			SteamPlayer newClient = Provider.addPlayer(transportConnection, netId, playerID, point, angle, isPro, isAdmin, channel, face, hair, beard, skin, color, markerColor, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skinTags, skinDynamicProps, skillset, language, lobbyID, clientPlatform);
			newClient.battlEyeId = Provider.allocBattlEyePlayerId();
			PlayerStance component = newClient.player.GetComponent<PlayerStance>();
			if (component != null)
			{
				component.initialStance = initialStance;
			}
			else
			{
				UnturnedLog.warn("Was unable to get PlayerStance for new connection!");
			}
			using (List<SteamPlayer>.Enumerator enumerator = Provider._clients.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SteamPlayer aboutClient = enumerator.Current;
					NetMessages.SendMessageToClient(EClientMessage.PlayerConnected, ENetReliability.Reliable, newClient.transportConnection, delegate(NetPakWriter writer)
					{
						Provider.WriteConnectedMessage(writer, aboutClient, newClient);
					});
				}
			}
			uint ipForClient;
			ushort queryPortForClient;
			Provider.GetAddressAndPortForClientAdvertisement(out ipForClient, out queryPortForClient);
			NetMessages.SendMessageToClient(EClientMessage.Accepted, ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt32(writer, ipForClient);
				SystemNetPakWriterEx.WriteUInt16(writer, queryPortForClient);
				SystemNetPakWriterEx.WriteUInt8(writer, (byte)Provider.modeConfigData.Gameplay.Repair_Level_Max);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Hitmarkers);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Crosshair);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Ballistics);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Chart);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Satellite);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Compass);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Group_Map);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Group_HUD);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Group_Player_List);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Allow_Static_Groups);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Allow_Shoulder_Camera);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Can_Suicide);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Friendly_Fire);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Bypass_Buildable_Mobility);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables);
				writer.WriteBit(Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables_On_Vehicles);
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)Provider.modeConfigData.Gameplay.Timer_Exit);
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)Provider.modeConfigData.Gameplay.Timer_Respawn);
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)Provider.modeConfigData.Gameplay.Timer_Home);
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)Provider.modeConfigData.Gameplay.Max_Group_Members);
				writer.WriteBit(Provider.modeConfigData.Barricades.Allow_Item_Placement_On_Vehicle);
				writer.WriteBit(Provider.modeConfigData.Barricades.Allow_Trap_Placement_On_Vehicle);
				SystemNetPakWriterEx.WriteFloat(writer, Provider.modeConfigData.Barricades.Max_Item_Distance_From_Hull);
				SystemNetPakWriterEx.WriteFloat(writer, Provider.modeConfigData.Barricades.Max_Trap_Distance_From_Hull);
				SystemNetPakWriterEx.WriteFloat(writer, Provider.modeConfigData.Gameplay.AirStrafing_Acceleration_Multiplier);
				SystemNetPakWriterEx.WriteFloat(writer, Provider.modeConfigData.Gameplay.AirStrafing_Deceleration_Multiplier);
				SystemNetPakWriterEx.WriteFloat(writer, Provider.modeConfigData.Gameplay.ThirdPerson_RecoilMultiplier);
				SystemNetPakWriterEx.WriteFloat(writer, Provider.modeConfigData.Gameplay.ThirdPerson_SpreadMultiplier);
			});
			if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnAddPlayer != null && Provider.battlEyeServerRunData.pfnReceivedPlayerGUID != null)
			{
				uint ipv4AddressOrZero = newClient.getIPv4AddressOrZero();
				ushort num2;
				transportConnection.TryGetPort(out num2);
				uint num3 = (ipv4AddressOrZero & 255U) << 24 | (ipv4AddressOrZero & 65280U) << 8 | (ipv4AddressOrZero & 16711680U) >> 8 | (ipv4AddressOrZero & 4278190080U) >> 24;
				ushort num4 = (ushort)((int)(num2 & 255) << 8 | (int)((uint)(num2 & 65280) >> 8));
				Provider.battlEyeServerRunData.pfnAddPlayer.Invoke(newClient.battlEyeId, num3, num4, playerID.playerName);
				GCHandle gchandle = GCHandle.Alloc(playerID.steamID.m_SteamID, 3);
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				Provider.battlEyeServerRunData.pfnReceivedPlayerGUID.Invoke(newClient.battlEyeId, intPtr, 8);
				gchandle.Free();
			}
			NetMessages.SendMessageToClients(EClientMessage.PlayerConnected, ENetReliability.Reliable, Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer potentialRecipient) => potentialRecipient != newClient), delegate(NetPakWriter writer)
			{
				Provider.WriteConnectedMessage(writer, newClient, null);
			});
			Provider.SendInitialGlobalState(newClient);
			newClient.player.InitializePlayer();
			foreach (SteamPlayer steamPlayer in Provider._clients)
			{
				steamPlayer.player.SendInitialPlayerState(newClient);
			}
			newClient.player.SendInitialPlayerState(Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer potentialRecipient) => potentialRecipient != newClient));
			try
			{
				Provider.ServerConnected serverConnected = Provider.onServerConnected;
				if (serverConnected != null)
				{
					serverConnected(playerID.steamID);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onServerConnected:");
				UnturnedLog.exception(e);
			}
			if (CommandWindow.shouldLogJoinLeave)
			{
				CommandWindow.Log(Provider.localization.format("PlayerConnectedText", playerID.steamID, playerID.playerName, playerID.characterName));
			}
			else
			{
				UnturnedLog.info(Provider.localization.format("PlayerConnectedText", playerID.steamID, playerID.playerName, playerID.characterName));
			}
			if (num == 0)
			{
				Provider.verifyNextPlayerInQueue();
			}
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x00107580 File Offset: 0x00105780
		private static void GetAddressAndPortForClientAdvertisement(out uint ip, out ushort queryPort)
		{
			if (!Provider.configData.Server.Use_FakeIP)
			{
				SteamGameServer.GetPublicIP().TryGetIPv4Address(out ip);
				queryPort = Provider.port;
				return;
			}
			SteamNetworkingFakeIPResult_t steamNetworkingFakeIPResult_t;
			SteamGameServerNetworkingSockets.GetFakeIP(0, out steamNetworkingFakeIPResult_t);
			if (steamNetworkingFakeIPResult_t.m_eResult == EResult.k_EResultOK && steamNetworkingFakeIPResult_t.m_unPorts != null && steamNetworkingFakeIPResult_t.m_unPorts.Length != 0)
			{
				ip = steamNetworkingFakeIPResult_t.m_unIP;
				queryPort = steamNetworkingFakeIPResult_t.m_unPorts[0];
				return;
			}
			ip = 0U;
			queryPort = 0;
		}

		/// <summary>
		/// Event for plugins when rejecting a player.
		/// </summary>
		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x060037C9 RID: 14281 RVA: 0x001075F0 File Offset: 0x001057F0
		// (remove) Token: 0x060037CA RID: 14282 RVA: 0x00107624 File Offset: 0x00105824
		public static event Provider.RejectingPlayerCallback onRejectingPlayer;

		// Token: 0x060037CB RID: 14283 RVA: 0x00107658 File Offset: 0x00105858
		private static void broadcastRejectingPlayer(CSteamID steamID, ESteamRejection rejection, string explanation)
		{
			try
			{
				Provider.RejectingPlayerCallback rejectingPlayerCallback = Provider.onRejectingPlayer;
				if (rejectingPlayerCallback != null)
				{
					rejectingPlayerCallback(steamID, rejection, explanation);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onRejectingPlayer:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x0010769C File Offset: 0x0010589C
		public static void reject(CSteamID steamID, ESteamRejection rejection)
		{
			Provider.reject(steamID, rejection, string.Empty);
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x001076AC File Offset: 0x001058AC
		public static void reject(CSteamID steamID, ESteamRejection rejection, string explanation)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				Provider.reject(transportConnection, rejection, explanation);
			}
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x001076CB File Offset: 0x001058CB
		public static void reject(ITransportConnection transportConnection, ESteamRejection rejection)
		{
			Provider.reject(transportConnection, rejection, string.Empty);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x001076DC File Offset: 0x001058DC
		public static void reject(ITransportConnection transportConnection, ESteamRejection rejection, string explanation)
		{
			if (transportConnection == null)
			{
				throw new ArgumentNullException("transportConnection");
			}
			ThreadUtil.assertIsGameThread();
			CSteamID csteamID = Provider.findTransportConnectionSteamId(transportConnection);
			if (csteamID != CSteamID.Nil)
			{
				Provider.broadcastRejectingPlayer(csteamID, rejection, explanation);
			}
			int i = 0;
			while (i < Provider.pending.Count)
			{
				if (transportConnection.Equals(Provider.pending[i].transportConnection))
				{
					if (rejection == ESteamRejection.AUTH_VAC_BAN)
					{
						ChatManager.say(Provider.pending[i].playerID.playerName + " was banned by VAC", Color.yellow, false);
					}
					else if (rejection == ESteamRejection.AUTH_PUB_BAN)
					{
						ChatManager.say(Provider.pending[i].playerID.playerName + " was banned by BattlEye", Color.yellow, false);
					}
					if (Provider.pending[i].inventoryResult != SteamInventoryResult_t.Invalid)
					{
						SteamGameServerInventory.DestroyResult(Provider.pending[i].inventoryResult);
						Provider.pending[i].inventoryResult = SteamInventoryResult_t.Invalid;
					}
					Provider.pending.RemoveAt(i);
					if (i == 0)
					{
						Provider.verifyNextPlayerInQueue();
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			SteamGameServer.EndAuthSession(csteamID);
			NetMessages.SendMessageToClient(EClientMessage.Rejected, ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				writer.WriteEnum(rejection);
				SystemNetPakWriterEx.WriteString(writer, explanation, 11);
			});
			transportConnection.CloseConnection();
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x00107855 File Offset: 0x00105A55
		[Obsolete]
		internal static void notifyClientPending(ITransportConnection transportConnection)
		{
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x00107858 File Offset: 0x00105A58
		private static bool findClientForKickBanDismiss(CSteamID steamID, out SteamPlayer foundClient, out byte foundIndex)
		{
			byte b = 0;
			while ((int)b < Provider.clients.Count)
			{
				SteamPlayer steamPlayer = Provider.clients[(int)b];
				if (steamPlayer.playerID.steamID == steamID)
				{
					foundClient = steamPlayer;
					foundIndex = b;
					return true;
				}
				b += 1;
			}
			foundClient = null;
			foundIndex = 0;
			return false;
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x001078A9 File Offset: 0x00105AA9
		private static void validateDisconnectedMaintainedIndex(CSteamID steamID, byte index)
		{
			if ((int)index >= Provider.clients.Count || Provider.clients[(int)index].playerID.steamID != steamID)
			{
				UnturnedLog.error("Clients array was modified during onServerDisconnected!");
			}
		}

		/// <summary>
		/// Notify client that they were kicked.
		/// </summary>
		// Token: 0x060037D3 RID: 14291 RVA: 0x001078E0 File Offset: 0x00105AE0
		private static void notifyKickedInternal(ITransportConnection transportConnection, string reason)
		{
			NetMessages.SendMessageToClient(EClientMessage.Kicked, ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteString(writer, reason, 11);
			});
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x00107910 File Offset: 0x00105B10
		public static void kick(CSteamID steamID, string reason)
		{
			ThreadUtil.assertIsGameThread();
			SteamPlayer steamPlayer;
			byte b;
			if (!Provider.findClientForKickBanDismiss(steamID, out steamPlayer, out b))
			{
				return;
			}
			UnturnedLog.info(string.Format("Kicking player {0} because \"{1}\"", steamID, reason));
			Provider.notifyKickedInternal(steamPlayer.transportConnection, reason);
			Provider.broadcastServerDisconnected(steamID);
			Provider.validateDisconnectedMaintainedIndex(steamID, b);
			SteamGameServer.EndAuthSession(steamID);
			Provider.removePlayer(b);
			Provider.replicateRemovePlayer(steamID, b);
		}

		/// <summary>
		/// Notify client that they were banned.
		/// </summary>
		// Token: 0x060037D5 RID: 14293 RVA: 0x00107974 File Offset: 0x00105B74
		internal static void notifyBannedInternal(ITransportConnection transportConnection, string reason, uint duration)
		{
			NetMessages.SendMessageToClient(EClientMessage.Banned, ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteString(writer, reason, 11);
				SystemNetPakWriterEx.WriteUInt32(writer, duration);
			});
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x001079AC File Offset: 0x00105BAC
		public static void ban(CSteamID steamID, string reason, uint duration)
		{
			ThreadUtil.assertIsGameThread();
			SteamPlayer steamPlayer;
			byte b;
			if (!Provider.findClientForKickBanDismiss(steamID, out steamPlayer, out b))
			{
				return;
			}
			UnturnedLog.info(string.Format("Banning player {0} for {1} because \"{2}\"", steamID, TimeSpan.FromSeconds(duration), reason));
			Provider.notifyBannedInternal(steamPlayer.transportConnection, reason, duration);
			Provider.broadcastServerDisconnected(steamID);
			Provider.validateDisconnectedMaintainedIndex(steamID, b);
			SteamGameServer.EndAuthSession(steamID);
			Provider.removePlayer(b);
			Provider.replicateRemovePlayer(steamID, b);
		}

		/// <summary>
		/// Player left server by canceling their ticket, or we are disconnecting them without telling them.
		/// Does not send any packets to the disconnecting player.
		/// </summary>
		// Token: 0x060037D7 RID: 14295 RVA: 0x00107A1C File Offset: 0x00105C1C
		public static void dismiss(CSteamID steamID)
		{
			ThreadUtil.assertIsGameThread();
			SteamPlayer steamPlayer;
			byte b;
			if (!Provider.findClientForKickBanDismiss(steamID, out steamPlayer, out b))
			{
				return;
			}
			Provider.broadcastServerDisconnected(steamID);
			Provider.validateDisconnectedMaintainedIndex(steamID, b);
			SteamGameServer.EndAuthSession(steamID);
			if (CommandWindow.shouldLogJoinLeave)
			{
				CommandWindow.Log(Provider.localization.format("PlayerDisconnectedText", steamID, steamPlayer.playerID.playerName, steamPlayer.playerID.characterName));
			}
			else
			{
				UnturnedLog.info(Provider.localization.format("PlayerDisconnectedText", steamID, steamPlayer.playerID.playerName, steamPlayer.playerID.characterName));
			}
			Provider.removePlayer(b);
			Provider.replicateRemovePlayer(steamID, b);
		}

		/// <summary>
		/// Callback when a pending player or existing player unexpectedly loses connection at the transport level.
		/// </summary>
		// Token: 0x060037D8 RID: 14296 RVA: 0x00107AC4 File Offset: 0x00105CC4
		private static void OnServerTransportConnectionFailure(ITransportConnection transportConnection, string debugString, bool isError)
		{
			SteamPending steamPending = Provider.findPendingPlayer(transportConnection);
			if (steamPending != null)
			{
				if (isError)
				{
					Provider.steam.clientsKickedForTransportConnectionFailureCount++;
					UnturnedLog.info(string.Format("Removing player in queue {0} due to transport failure ({1}) queue state: \"{2}\"", transportConnection, debugString, steamPending.GetQueueStateDebugString()));
				}
				else
				{
					UnturnedLog.info(string.Format("Removing player in queue {0} because they disconnected ({1}) queue state: \"{2}\"", transportConnection, debugString, steamPending.GetQueueStateDebugString()));
				}
				Provider.reject(transportConnection, ESteamRejection.LATE_PENDING);
				return;
			}
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer != null)
			{
				if (isError)
				{
					Provider.steam.clientsKickedForTransportConnectionFailureCount++;
					UnturnedLog.info(string.Format("Removing player {0} due to transport failure ({1})", transportConnection, debugString));
				}
				else
				{
					UnturnedLog.info(string.Format("Removing player {0} because they disconnected ({1})", transportConnection, debugString));
				}
				Provider.dismiss(steamPlayer.playerID.steamID);
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x00107B7C File Offset: 0x00105D7C
		internal static bool verifyTicket(CSteamID steamID, byte[] ticket)
		{
			return SteamGameServer.BeginAuthSession(ticket, ticket.Length, steamID) == EBeginAuthSessionResult.k_EBeginAuthSessionResultOK;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x00107B8C File Offset: 0x00105D8C
		private static void openGameServer()
		{
			if (Provider.isServer || Provider.isClient)
			{
				UnturnedLog.error("Failed to open game server: session already in progress.");
				return;
			}
			ESecurityMode esecurityMode = ESecurityMode.LAN;
			ESteamServerVisibility serverVisibility = Dedicator.serverVisibility;
			if (serverVisibility != ESteamServerVisibility.Internet)
			{
				if (serverVisibility == ESteamServerVisibility.LAN)
				{
					esecurityMode = ESecurityMode.LAN;
				}
			}
			else if (Provider.configData.Server.VAC_Secure)
			{
				esecurityMode = ESecurityMode.SECURE;
			}
			else
			{
				esecurityMode = ESecurityMode.INSECURE;
			}
			if (esecurityMode == ESecurityMode.INSECURE)
			{
				CommandWindow.LogWarning(Provider.localization.format("InsecureWarningText"));
			}
			Provider.isVacActive = (esecurityMode == ESecurityMode.SECURE);
			if (Provider.IsBattlEyeEnabled && esecurityMode == ESecurityMode.SECURE)
			{
				if (!Provider.initializeBattlEyeServer())
				{
					Provider.QuitGame("BattlEye server init failed");
					return;
				}
				Provider.isBattlEyeActive = true;
			}
			else
			{
				Provider.isBattlEyeActive = false;
			}
			Provider.hasSetIsBattlEyeActive = true;
			bool flag = !Dedicator.offlineOnly;
			if (flag)
			{
				Provider.provider.multiplayerService.serverMultiplayerService.ready += Provider.handleServerReady;
			}
			try
			{
				Provider.provider.multiplayerService.serverMultiplayerService.open(Provider.ip, Provider.port, esecurityMode);
			}
			catch (Exception ex)
			{
				Provider.QuitGame("server init failed (" + ex.Message + ")");
				return;
			}
			Provider.serverTransport = NetTransportFactory.CreateServerTransport();
			UnturnedLog.info("Initializing {0}", new object[]
			{
				Provider.serverTransport.GetType().Name
			});
			Provider.serverTransport.Initialize(new ServerTransportConnectionFailureCallback(Provider.OnServerTransportConnectionFailure));
			Provider.backendRealtimeSeconds = SteamGameServerUtils.GetServerRealTime();
			Provider.authorityHoliday = (Provider._modeConfigData.Gameplay.Allow_Holidays ? HolidayUtil.BackendGetActiveHoliday() : ENPCHoliday.NONE);
			if (flag)
			{
				CommandWindow.Log("Waiting for Steam servers...");
				return;
			}
			Provider.initializeDedicatedUGC();
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x00107D30 File Offset: 0x00105F30
		private static void closeGameServer()
		{
			if (!Provider.isServer)
			{
				UnturnedLog.error("Failed to close game server: no session in progress.");
				return;
			}
			Provider.broadcastServerShutdown();
			Provider._isServer = false;
			Provider.provider.multiplayerService.serverMultiplayerService.close();
		}

		/// <summary>
		/// Check whether a server is one of our favorites or not.
		/// </summary>
		// Token: 0x060037DC RID: 14300 RVA: 0x00107D64 File Offset: 0x00105F64
		public static bool GetServerIsFavorited(uint ip, ushort queryPort)
		{
			foreach (Provider.CachedFavorite cachedFavorite in Provider.cachedFavorites)
			{
				if (cachedFavorite.matchesServer(ip, queryPort))
				{
					return cachedFavorite.isFavorited;
				}
			}
			for (int i = 0; i < SteamMatchmaking.GetFavoriteGameCount(); i++)
			{
				AppId_t appId_t;
				uint num;
				ushort num2;
				ushort num3;
				uint num4;
				uint num5;
				SteamMatchmaking.GetFavoriteGame(i, out appId_t, out num, out num2, out num3, out num4, out num5);
				if ((num4 | Provider.STEAM_FAVORITE_FLAG_FAVORITE) == num4 && num == ip && num3 == queryPort)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Set whether a server is one of our favorites or not.
		/// </summary>
		// Token: 0x060037DD RID: 14301 RVA: 0x00107E04 File Offset: 0x00106004
		public static void SetServerIsFavorited(uint ip, ushort connectionPort, ushort queryPort, bool newFavorited)
		{
			bool flag = false;
			foreach (Provider.CachedFavorite cachedFavorite in Provider.cachedFavorites)
			{
				if (cachedFavorite.matchesServer(ip, queryPort))
				{
					cachedFavorite.isFavorited = newFavorited;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Provider.CachedFavorite cachedFavorite2 = new Provider.CachedFavorite();
				cachedFavorite2.ip = ip;
				cachedFavorite2.queryPort = Provider.port;
				cachedFavorite2.isFavorited = newFavorited;
				Provider.cachedFavorites.Add(cachedFavorite2);
			}
			if (newFavorited)
			{
				SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, ip, connectionPort, queryPort, Provider.STEAM_FAVORITE_FLAG_FAVORITE, SteamUtils.GetServerRealTime());
				UnturnedLog.info(string.Format("Added favorite server IP: {0} Connection Port: {1} Query Port: {2}", new IPv4Address(ip), connectionPort, queryPort));
				return;
			}
			SteamMatchmaking.RemoveFavoriteGame(Provider.APP_ID, ip, connectionPort, queryPort, Provider.STEAM_FAVORITE_FLAG_FAVORITE);
			UnturnedLog.info(string.Format("Removed favorite server IP: {0} Connection Port: {1} Query Port: {2}", new IPv4Address(ip), connectionPort, queryPort));
		}

		/// <summary>
		/// Open URL in the steam overlay, or if disabled use the default browser instead.
		/// Warning: any third party url should be checked by WebUtils.ParseThirdPartyUrl.
		/// </summary>
		// Token: 0x060037DE RID: 14302 RVA: 0x00107F10 File Offset: 0x00106110
		public static void openURL(string url)
		{
			if (SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Process.Start(url);
		}

		/// <summary>
		/// Steam's favorites list requires that we know the server's IPv4 address and port,
		/// so we can't favorite when joining by Steam ID.
		/// </summary>
		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x00107F28 File Offset: 0x00106128
		public static bool CanFavoriteCurrentServer
		{
			get
			{
				IPv4Address pv4Address;
				ushort num;
				ushort num2;
				return !Provider.isServer && Provider.clientTransport != null && (Provider.clientTransport.TryGetIPv4Address(out pv4Address) & Provider.clientTransport.TryGetConnectionPort(out num) & Provider.clientTransport.TryGetQueryPort(out num2) & !SteamNetworkingUtils.IsFakeIPv4(pv4Address.value));
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x00107F7A File Offset: 0x0010617A
		public static bool CanBookmarkCurrentServer
		{
			get
			{
				return !Provider.isServer && Provider.currentServerWorkshopResponse != null && Provider.currentServerWorkshopResponse.server.BPersistentGameServerAccount() && !string.IsNullOrEmpty(Provider.currentServerWorkshopResponse.bookmarkHost);
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x060037E1 RID: 14305 RVA: 0x00107FB4 File Offset: 0x001061B4
		public static bool isCurrentServerFavorited
		{
			get
			{
				if (Provider.isServer || Provider.clientTransport == null)
				{
					return false;
				}
				IPv4Address pv4Address;
				Provider.clientTransport.TryGetIPv4Address(out pv4Address);
				ushort queryPort;
				Provider.clientTransport.TryGetQueryPort(out queryPort);
				return Provider.GetServerIsFavorited(pv4Address.value, queryPort);
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060037E2 RID: 14306 RVA: 0x00107FF7 File Offset: 0x001061F7
		public static bool IsCurrentServerBookmarked
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Toggle whether we've favorited the server we're currently playing on.
		/// </summary>
		// Token: 0x060037E3 RID: 14307 RVA: 0x00107FFC File Offset: 0x001061FC
		public static void toggleCurrentServerFavorited()
		{
			if (Provider.isServer || Provider.clientTransport == null)
			{
				return;
			}
			IPv4Address pv4Address;
			ushort connectionPort;
			ushort queryPort;
			if (Provider.clientTransport.TryGetIPv4Address(out pv4Address) & Provider.clientTransport.TryGetConnectionPort(out connectionPort) & Provider.clientTransport.TryGetQueryPort(out queryPort))
			{
				bool newFavorited = !Provider.GetServerIsFavorited(pv4Address.value, queryPort);
				Provider.SetServerIsFavorited(pv4Address.value, connectionPort, queryPort, newFavorited);
				return;
			}
			UnturnedLog.info("Unable to toggle server favorite because connection details are unavailable");
		}

		/// <summary>
		/// Toggle whether we've bookmarked the server we're currently playing on.
		/// </summary>
		// Token: 0x060037E4 RID: 14308 RVA: 0x00108069 File Offset: 0x00106269
		public static void ToggleCurrentServerBookmarked()
		{
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x0010806C File Offset: 0x0010626C
		private static void broadcastEnemyConnected(SteamPlayer player)
		{
			try
			{
				Provider.EnemyConnected enemyConnected = Provider.onEnemyConnected;
				if (enemyConnected != null)
				{
					enemyConnected(player);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Exception during onEnemyConnected:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x001080B0 File Offset: 0x001062B0
		private static void broadcastEnemyDisconnected(SteamPlayer player)
		{
			try
			{
				Provider.EnemyDisconnected enemyDisconnected = Provider.onEnemyDisconnected;
				if (enemyDisconnected != null)
				{
					enemyDisconnected(player);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Exception during onEnemyDisconnected:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x001080F4 File Offset: 0x001062F4
		private static void onPersonaStateChange(PersonaStateChange_t callback)
		{
			if (callback.m_nChangeFlags == EPersonaChange.k_EPersonaChangeName && callback.m_ulSteamID == Provider.client.m_SteamID)
			{
				Provider._clientName = SteamFriends.GetPersonaName();
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0010811C File Offset: 0x0010631C
		private static void OnGetTicketForWebApiResponse(GetTicketForWebApiResponse_t callback)
		{
			string identity;
			if (!Provider.pluginTicketHandles.TryGetValue(callback.m_hAuthTicket, ref identity))
			{
				UnturnedLog.info(string.Format("Received Steam auth ticket for web API for handle {0}, but no linked identity (Result: {1})", callback.m_hAuthTicket, callback.m_eResult));
				SteamUser.CancelAuthTicket(callback.m_hAuthTicket);
				return;
			}
			if (callback.m_eResult != EResult.k_EResultOK)
			{
				UnturnedLog.warn(string.Format("Error getting Steam auth ticket for web API identity \"{0}\": {1}", identity, callback.m_eResult));
				Provider.pluginTicketHandles.Remove(callback.m_hAuthTicket);
				SteamUser.CancelAuthTicket(callback.m_hAuthTicket);
				return;
			}
			UnturnedLog.info(string.Format("Received Steam web API ticket response for identity \"{0}\" (length: {1})", identity, callback.m_cubTicket));
			SteamPlayer.SendGetSteamAuthTicketForWebApiResponse.Invoke(ENetReliability.Reliable, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteString(writer, identity, 5);
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)callback.m_cubTicket);
				writer.WriteBytes(callback.m_rgubTicket, callback.m_cubTicket);
			});
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x0010822C File Offset: 0x0010642C
		private static void onGameServerChangeRequested(GameServerChangeRequested_t callback)
		{
			if (Provider.isConnected)
			{
				return;
			}
			UnturnedLog.info("onGameServerChangeRequested {0} {1}", new object[]
			{
				callback.m_rgchServer,
				callback.m_rgchPassword
			});
			SteamConnectionInfo steamConnectionInfo = new SteamConnectionInfo(callback.m_rgchServer, callback.m_rgchPassword);
			UnturnedLog.info("External connect IP: {0} Port: {1} Password: '{2}'", new object[]
			{
				Parser.getIPFromUInt32(steamConnectionInfo.ip),
				steamConnectionInfo.port,
				steamConnectionInfo.password
			});
			MenuPlayConnectUI.connect(steamConnectionInfo, false, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x001082B8 File Offset: 0x001064B8
		private static void onGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t callback)
		{
			if (Provider.isConnected)
			{
				return;
			}
			UnturnedLog.info("onGameRichPresenceJoinRequested {0}", new object[]
			{
				callback.m_rgchConnect
			});
			uint newIP;
			ushort newPort;
			string newPassword;
			if (CommandLine.TryGetSteamConnect(callback.m_rgchConnect, out newIP, out newPort, out newPassword))
			{
				SteamConnectionInfo steamConnectionInfo = new SteamConnectionInfo(newIP, newPort, newPassword);
				UnturnedLog.info("Rich presence connect IP: {0} Port: {1} Password: '{2}'", new object[]
				{
					Parser.getIPFromUInt32(steamConnectionInfo.ip),
					steamConnectionInfo.port,
					steamConnectionInfo.password
				});
				MenuPlayConnectUI.connect(steamConnectionInfo, false, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060037EB RID: 14315 RVA: 0x00108341 File Offset: 0x00106541
		// (set) Token: 0x060037EC RID: 14316 RVA: 0x00108348 File Offset: 0x00106548
		public static float timeLastPacketWasReceivedFromServer { get; internal set; }

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060037ED RID: 14317 RVA: 0x00108350 File Offset: 0x00106550
		public static float ping
		{
			get
			{
				return Provider._ping;
			}
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x00108358 File Offset: 0x00106558
		internal static void lag(float value)
		{
			value = Mathf.Clamp01(value);
			Provider._ping = value;
			for (int i = Provider.pings.Length - 1; i > 0; i--)
			{
				Provider.pings[i] = Provider.pings[i - 1];
				if (Provider.pings[i] > 0.001f)
				{
					Provider._ping += Provider.pings[i];
				}
			}
			Provider._ping /= (float)Provider.pings.Length;
			Provider.pings[0] = value;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x001083D4 File Offset: 0x001065D4
		internal static byte[] openTicket(SteamNetworkingIdentity serverIdentity)
		{
			if (Provider.ticketHandle != HAuthTicket.Invalid)
			{
				return null;
			}
			byte[] array = new byte[1024];
			string text;
			serverIdentity.ToString(out text);
			UnturnedLog.info("Calling GetAuthSessionTicket with identity " + text);
			uint num;
			Provider.ticketHandle = SteamUser.GetAuthSessionTicket(array, array.Length, out num, ref serverIdentity);
			if (num == 0U)
			{
				UnturnedLog.info("GetAuthSessionTicket returned size zero");
				return null;
			}
			UnturnedLog.info(string.Format("GetAuthSessionTicket ticket handle is valid: {0} (size: {1})", Provider.ticketHandle != HAuthTicket.Invalid, num));
			byte[] array2 = new byte[num];
			Buffer.BlockCopy(array, 0, array2, 0, (int)num);
			return array2;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x00108474 File Offset: 0x00106674
		internal static void RequestSteamAuthTicketForWebApi(string identity)
		{
			foreach (KeyValuePair<HAuthTicket, string> keyValuePair in Provider.pluginTicketHandles)
			{
				if (string.Equals(keyValuePair.Value, identity))
				{
					UnturnedLog.error("Ignoring duplicate Steam web API ticket request for identity \"" + identity + "\"");
					return;
				}
			}
			HAuthTicket authTicketForWebApi = SteamUser.GetAuthTicketForWebApi(identity);
			if (authTicketForWebApi != HAuthTicket.Invalid)
			{
				Provider.pluginTicketHandles.Add(authTicketForWebApi, identity);
				UnturnedLog.info(string.Format("Added handle {0} for Steam web API ticket request for identity \"{1}\"", authTicketForWebApi, identity));
				return;
			}
			UnturnedLog.error("GetAuthTicketForWebApi for identity \"" + identity + "\" returned invalid handle");
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x00108534 File Offset: 0x00106734
		private static void CancelAllSteamAuthTickets()
		{
			if (Provider.ticketHandle != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(Provider.ticketHandle);
				Provider.ticketHandle = HAuthTicket.Invalid;
				UnturnedLog.info("Cancelled main Steam auth ticket");
			}
			foreach (KeyValuePair<HAuthTicket, string> keyValuePair in Provider.pluginTicketHandles)
			{
				SteamUser.CancelAuthTicket(keyValuePair.Key);
				UnturnedLog.info("Cancelled Steam web API auth ticket for identity \"" + keyValuePair.Value + "\"");
			}
			Provider.pluginTicketHandles.Clear();
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060037F2 RID: 14322 RVA: 0x001085E0 File Offset: 0x001067E0
		// (set) Token: 0x060037F3 RID: 14323 RVA: 0x001085E7 File Offset: 0x001067E7
		public static IProvider provider { get; protected set; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x001085EF File Offset: 0x001067EF
		public static bool isInitialized
		{
			get
			{
				return Provider._isInitialized;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x001085F6 File Offset: 0x001067F6
		// (set) Token: 0x060037F6 RID: 14326 RVA: 0x0010860C File Offset: 0x0010680C
		public static uint time
		{
			get
			{
				return Provider._time + (uint)(Time.realtimeSinceStartup - Provider.timeOffset);
			}
			set
			{
				Provider._time = value;
				Provider.timeOffset = (uint)Time.realtimeSinceStartup;
			}
		}

		/// <summary>
		/// Number of seconds since January 1st, 1970 GMT as reported by backend servers.
		/// Used by holiday events to keep timing somewhat synced between players.
		/// </summary>
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060037F7 RID: 14327 RVA: 0x0010861F File Offset: 0x0010681F
		// (set) Token: 0x060037F8 RID: 14328 RVA: 0x00108633 File Offset: 0x00106833
		public static uint backendRealtimeSeconds
		{
			get
			{
				return Provider.initialBackendRealtimeSeconds + (uint)(Time.realtimeSinceStartup - Provider.initialLocalRealtime);
			}
			private set
			{
				Provider.initialBackendRealtimeSeconds = value;
				Provider.initialLocalRealtime = Time.realtimeSinceStartup;
				Provider.BackendRealtimeAvailableHandler backendRealtimeAvailableHandler = Provider.onBackendRealtimeAvailable;
				if (backendRealtimeAvailableHandler == null)
				{
					return;
				}
				backendRealtimeAvailableHandler();
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x00108654 File Offset: 0x00106854
		public static DateTime backendRealtimeDate
		{
			get
			{
				return Provider.unixEpochDateTime.AddSeconds(Provider.backendRealtimeSeconds);
			}
		}

		/// <summary>
		/// Has the initial backend realtime been queried yet?
		/// Not available immediately on servers because SteamGameServerUtils cannot be used until the actual Steam instance is available.
		/// </summary>
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060037FA RID: 14330 RVA: 0x00108667 File Offset: 0x00106867
		public static bool isBackendRealtimeAvailable
		{
			get
			{
				return Provider.initialBackendRealtimeSeconds > 0U;
			}
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x00108671 File Offset: 0x00106871
		private IEnumerator QuitAfterDelay(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			Application.quitting -= new Action(this.onApplicationQuitting);
			this.onApplicationQuitting();
			Provider.QuitGame("server shutdown");
			yield break;
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x00108687 File Offset: 0x00106887
		private static void onAPIWarningMessage(int severity, StringBuilder warning)
		{
			CommandWindow.LogWarning("Steam API Warning Message:");
			CommandWindow.LogWarning("Severity: " + severity.ToString());
			CommandWindow.LogWarning("Warning: " + ((warning != null) ? warning.ToString() : null));
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x001086C8 File Offset: 0x001068C8
		private void updateDebug()
		{
			Provider.debugUpdates++;
			if (Time.realtimeSinceStartup - Provider.debugLastUpdate > 1f)
			{
				Provider.debugUPS = (int)((float)Provider.debugUpdates / (Time.realtimeSinceStartup - Provider.debugLastUpdate));
				Provider.debugLastUpdate = Time.realtimeSinceStartup;
				Provider.debugUpdates = 0;
			}
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x0010871C File Offset: 0x0010691C
		private void tickDebug()
		{
			Provider.debugTicks++;
			if (Time.realtimeSinceStartup - Provider.debugLastTick > 1f)
			{
				Provider.debugTPS = (int)((float)Provider.debugTicks / (Time.realtimeSinceStartup - Provider.debugLastTick));
				Provider.debugLastTick = Time.realtimeSinceStartup;
				Provider.debugTicks = 0;
			}
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x0010876F File Offset: 0x0010696F
		private IEnumerator downloadIcon(Provider.PendingIconRequest iconQueryParams)
		{
			using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(iconQueryParams.url, true))
			{
				request.timeout = 15;
				yield return request.SendWebRequest();
				Texture2D texture2D = null;
				bool flag = false;
				if (request.result != 1)
				{
					UnturnedLog.warn(string.Format("{0} downloading \"{1}\" for icon query: \"{2}\"", request.result, iconQueryParams.url, request.error));
				}
				else
				{
					Texture2D content = DownloadHandlerTexture.GetContent(request);
					content.hideFlags = HideFlags.HideAndDontSave;
					content.filterMode = FilterMode.Trilinear;
					if (iconQueryParams.shouldCache)
					{
						if (Provider.downloadedIconCache.TryGetValue(iconQueryParams.url, ref texture2D))
						{
							Object.Destroy(content);
						}
						else
						{
							Provider.downloadedIconCache.Add(iconQueryParams.url, content);
							texture2D = content;
						}
						flag = false;
					}
					else
					{
						texture2D = content;
						flag = true;
					}
				}
				if (iconQueryParams.callback == null)
				{
					if (flag && texture2D != null)
					{
						Object.Destroy(texture2D);
					}
				}
				else
				{
					try
					{
						iconQueryParams.callback(texture2D, flag);
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "Caught exception during texture downloaded callback:");
					}
				}
				if (iconQueryParams.shouldCache)
				{
					Provider.pendingCachableIconRequests.Remove(iconQueryParams.url);
				}
			}
			UnityWebRequest request = null;
			yield break;
			yield break;
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x00108780 File Offset: 0x00106980
		public static void destroyCachedIcon(string url)
		{
			Texture2D obj;
			if (Provider.downloadedIconCache.TryGetValue(url, ref obj))
			{
				Object.Destroy(obj);
				Provider.downloadedIconCache.Remove(url);
			}
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x001087B0 File Offset: 0x001069B0
		public static void refreshIcon(Provider.IconQueryParams iconQueryParams)
		{
			if (iconQueryParams.callback == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(iconQueryParams.url) || !Provider.allowWebRequests)
			{
				iconQueryParams.callback(null, false);
				return;
			}
			iconQueryParams.url = iconQueryParams.url.Trim();
			if (string.IsNullOrEmpty(iconQueryParams.url))
			{
				iconQueryParams.callback(null, false);
				return;
			}
			if (iconQueryParams.shouldCache)
			{
				Texture2D icon;
				if (Provider.downloadedIconCache.TryGetValue(iconQueryParams.url, ref icon))
				{
					iconQueryParams.callback(icon, false);
					return;
				}
				Provider.PendingIconRequest pendingIconRequest;
				if (Provider.pendingCachableIconRequests.TryGetValue(iconQueryParams.url, ref pendingIconRequest))
				{
					Provider.PendingIconRequest pendingIconRequest2 = pendingIconRequest;
					pendingIconRequest2.callback = (Provider.IconQueryCallback)Delegate.Combine(pendingIconRequest2.callback, iconQueryParams.callback);
					return;
				}
			}
			Provider.PendingIconRequest pendingIconRequest3 = new Provider.PendingIconRequest();
			pendingIconRequest3.url = iconQueryParams.url;
			pendingIconRequest3.callback = iconQueryParams.callback;
			pendingIconRequest3.shouldCache = iconQueryParams.shouldCache;
			if (iconQueryParams.shouldCache)
			{
				Provider.pendingCachableIconRequests.Add(iconQueryParams.url, pendingIconRequest3);
			}
			Provider.steam.StartCoroutine(Provider.steam.downloadIcon(pendingIconRequest3));
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x001088D0 File Offset: 0x00106AD0
		private void Update()
		{
			if (!Provider.isInitialized)
			{
				return;
			}
			if (Time.unscaledDeltaTime > 1.5f)
			{
				UnturnedLog.info("Long delay between Updates: {0}s", new object[]
				{
					Time.unscaledDeltaTime
				});
			}
			if (Provider.battlEyeClientHandle != IntPtr.Zero && Provider.battlEyeClientRunData != null && Provider.battlEyeClientRunData.pfnRun != null)
			{
				Provider.battlEyeClientRunData.pfnRun.Invoke();
			}
			if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnRun != null)
			{
				Provider.battlEyeServerRunData.pfnRun.Invoke();
			}
			if (Provider.isConnected)
			{
				Provider.listen();
			}
			this.updateDebug();
			Provider.provider.update();
			if (Provider.countShutdownTimer > 0)
			{
				if (Time.realtimeSinceStartup - Provider.lastTimerMessage > 1f)
				{
					Provider.lastTimerMessage = Time.realtimeSinceStartup;
					Provider.countShutdownTimer--;
					if (Provider.countShutdownTimer == 300 || Provider.countShutdownTimer == 60 || Provider.countShutdownTimer == 30 || Provider.countShutdownTimer == 15 || Provider.countShutdownTimer == 3 || Provider.countShutdownTimer == 2 || Provider.countShutdownTimer == 1)
					{
						ChatManager.say(Provider.localization.format("Shutdown", Provider.countShutdownTimer), ChatManager.welcomeColor, false);
						return;
					}
				}
			}
			else if (Provider.countShutdownTimer == 0)
			{
				UnturnedLog.info("Server shutdown timer reached zero");
				Provider.didServerShutdownTimerReachZero = true;
				Provider.countShutdownTimer = -1;
				Provider.broadcastCommenceShutdown();
				bool flag = Provider._clients.Count > 0;
				if (Provider._clients.Count > 0)
				{
					NetMessages.SendMessageToClients(EClientMessage.Shutdown, ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteString(writer, Provider.shutdownMessage, 11);
					});
				}
				foreach (SteamPlayer steamPlayer in Provider._clients)
				{
					SteamGameServer.EndAuthSession(steamPlayer.playerID.steamID);
				}
				float num = flag ? 1f : 0f;
				if (flag)
				{
					UnturnedLog.info(string.Format("Delaying server quit by {0}s to ensure shutdown message reaches clients", num));
				}
				base.StartCoroutine(this.QuitAfterDelay(num));
			}
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x00108B1C File Offset: 0x00106D1C
		private void FixedUpdate()
		{
			if (!Provider.isInitialized)
			{
				return;
			}
			this.tickDebug();
		}

		/// <summary>
		/// In here because we want to call this very early in startup after initializing provider,
		/// but with plenty of time to hopefully install maps prior to reaching the main menu.
		/// </summary>
		// Token: 0x06003804 RID: 14340 RVA: 0x00108B2C File Offset: 0x00106D2C
		public static void initAutoSubscribeMaps()
		{
			if (Provider.statusData == null || Provider.statusData.Maps == null)
			{
				return;
			}
			foreach (AutoSubscribeMap autoSubscribeMap in Provider.statusData.Maps.Auto_Subscribe)
			{
				if (!LocalNews.hasAutoSubscribedToWorkshopItem(autoSubscribeMap.Workshop_File_Id) && new DateTimeRange(autoSubscribeMap.Start, autoSubscribeMap.End).isNowWithinRange())
				{
					LocalNews.markAutoSubscribedToWorkshopItem(autoSubscribeMap.Workshop_File_Id);
					Provider.provider.workshopService.setSubscribed(autoSubscribeMap.Workshop_File_Id, true);
				}
			}
			ConvenientSavedata.SaveIfDirty();
		}

		/// <summary>
		/// This file is of particular importance to the dedicated server because otherwise Steam networking sockets
		/// will say the certificate is for the wrong app. When launching the game outside Steam this sets the app.
		/// </summary>
		// Token: 0x06003805 RID: 14341 RVA: 0x00108BE0 File Offset: 0x00106DE0
		private void WriteSteamAppIdFileAndEnvironmentVariables()
		{
			uint appId = Provider.APP_ID.m_AppId;
			string text = appId.ToString(CultureInfo.InvariantCulture);
			UnturnedLog.info("Unturned overriding Steam AppId with \"" + text + "\"");
			try
			{
				Environment.SetEnvironmentVariable("SteamOverlayGameId", text, 0);
				Environment.SetEnvironmentVariable("SteamGameId", text, 0);
				Environment.SetEnvironmentVariable("SteamAppId", text, 0);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception writing Steam environment variables:");
			}
			string text2 = PathEx.Join(UnityPaths.GameDirectory, "steam_appid.txt");
			try
			{
				using (FileStream fileStream = new FileStream(text2, 4, 2, 3))
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.ASCII))
					{
						streamWriter.Write(text);
					}
				}
			}
			catch (Exception e2)
			{
				UnturnedLog.exception(e2, "Caught exception writing steam_appid.txt file:");
			}
		}

		/// <summary>
		/// Hackily exposed as an easy way for editor code to check the verison number.
		/// </summary>
		// Token: 0x06003806 RID: 14342 RVA: 0x00108CD8 File Offset: 0x00106ED8
		public static StatusData LoadStatusData()
		{
			if (ReadWrite.fileExists("/Status.json", false, true))
			{
				try
				{
					return ReadWrite.deserializeJSON<StatusData>("/Status.json", false, true);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Unable to parse Status.json! consider validating with a JSON linter");
				}
			}
			return null;
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x00108D24 File Offset: 0x00106F24
		public void awake()
		{
			Provider._statusData = Provider.LoadStatusData();
			if (Provider.statusData == null)
			{
				Provider._statusData = new StatusData();
			}
			HolidayUtil.scheduleHolidays(Provider.statusData.Holidays);
			Provider.APP_VERSION = Provider.statusData.Game.FormatApplicationVersion();
			Provider.APP_VERSION_PACKED = Parser.getUInt32FromIP(Provider.APP_VERSION);
			if (Provider.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Provider._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			Provider.steam = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(Provider.onLevelLoaded));
			Application.quitting += new Action(this.onApplicationQuitting);
			Application.wantsToQuit += new Func<bool>(this.onApplicationWantsToQuit);
			try
			{
				this.WriteSteamAppIdFileAndEnvironmentVariables();
				Provider.provider = new SteamworksProvider(new SteamworksAppInfo(Provider.APP_ID.m_AppId, Provider.APP_NAME, Provider.APP_VERSION, true));
				Provider.provider.intialize();
			}
			catch (Exception ex)
			{
				Provider.QuitGame("Steam init exception (" + ex.Message + ")");
				return;
			}
			string language;
			if (!CommandLine.tryGetLanguage(out language, out Provider._path))
			{
				Provider._path = ReadWrite.PATH + "/Localization/";
				language = "English";
			}
			Provider.language = language;
			Provider.localizationRoot = Provider.path + Provider.language;
			Provider.localization = Localization.read("/Server/ServerConsole.dat");
			Provider.p2pSessionConnectFail = Callback<P2PSessionConnectFail_t>.CreateGameServer(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(Provider.onP2PSessionConnectFail));
			Provider.validateAuthTicketResponse = Callback<ValidateAuthTicketResponse_t>.CreateGameServer(new Callback<ValidateAuthTicketResponse_t>.DispatchDelegate(Provider.onValidateAuthTicketResponse));
			Provider.clientGroupStatus = Callback<GSClientGroupStatus_t>.CreateGameServer(new Callback<GSClientGroupStatus_t>.DispatchDelegate(Provider.onClientGroupStatus));
			Provider._isPro = true;
			CommandWindow.Log("Game version: " + Provider.APP_VERSION + " Engine version: " + Application.unityVersion);
			Provider.maxPlayers = 8;
			Provider.queueSize = 8;
			Provider.serverName = "Unturned";
			Provider.serverPassword = "";
			Provider.ip = 0U;
			Provider.port = 27015;
			Provider.map = "PEI";
			Provider.isPvP = true;
			Provider.isWhitelisted = false;
			Provider.hideAdmins = false;
			Provider.hasCheats = false;
			Provider.filterName = false;
			Provider.mode = EGameMode.NORMAL;
			Provider.isGold = false;
			Provider.gameMode = null;
			Provider.cameraMode = ECameraMode.FIRST;
			Commander.init();
			SteamWhitelist.load();
			SteamBlacklist.load();
			SteamAdminlist.load();
			string[] commands = CommandLine.getCommands();
			UnturnedLog.info(string.Format("Executing {0} potential game command(s) from the command-line:", commands.Length));
			for (int i = 0; i < commands.Length; i++)
			{
				if (!Commander.execute(CSteamID.Nil, commands[i]))
				{
					UnturnedLog.info("Did not match \"" + commands[i] + "\" with any commands");
				}
			}
			if (ServerSavedata.fileExists("/Server/Commands.dat"))
			{
				FileStream fileStream = null;
				StreamReader streamReader = null;
				try
				{
					fileStream = new FileStream(ReadWrite.PATH + "/Servers/" + Provider.serverID + "/Server/Commands.dat", 3, 1, 1);
					streamReader = new StreamReader(fileStream);
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (!string.IsNullOrWhiteSpace(text) && !Commander.execute(CSteamID.Nil, text))
						{
							UnturnedLog.error("Unknown entry in Commands.dat: '{0}'", new object[]
							{
								text
							});
						}
					}
					goto IL_348;
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
			}
			Data data = new Data();
			ServerSavedata.writeData("/Server/Commands.dat", data);
			IL_348:
			if (!ServerSavedata.folderExists("/Bundles"))
			{
				ServerSavedata.createFolder("/Bundles");
			}
			if (!ServerSavedata.folderExists("/Maps"))
			{
				ServerSavedata.createFolder("/Maps");
			}
			if (!ServerSavedata.folderExists("/Workshop/Content"))
			{
				ServerSavedata.createFolder("/Workshop/Content");
			}
			if (!ServerSavedata.folderExists("/Workshop/Maps"))
			{
				ServerSavedata.createFolder("/Workshop/Maps");
			}
			Provider._configData = ConfigData.CreateDefault(false);
			if (ServerSavedata.fileExists("/Config.json"))
			{
				try
				{
					ServerSavedata.populateJSON("/Config.json", Provider._configData);
				}
				catch (Exception e)
				{
					UnturnedLog.error("Exception while parsing server config:");
					UnturnedLog.exception(e);
				}
			}
			ServerSavedata.serializeJSON<ConfigData>("/Config.json", Provider.configData);
			Provider._modeConfigData = Provider._configData.getModeConfig(Provider.mode);
			if (Provider._modeConfigData == null)
			{
				Provider._modeConfigData = new ModeConfigData(Provider.mode);
			}
			if (!Dedicator.offlineOnly)
			{
				HostBansManager.Get().Refresh();
			}
			this.LogSystemInfo();
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x00109188 File Offset: 0x00107388
		public void start()
		{
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x0010918C File Offset: 0x0010738C
		private void LogSystemInfo()
		{
			try
			{
				UnturnedLog.info("Platform: {0}", new object[]
				{
					Application.platform
				});
				UnturnedLog.info("Operating System: " + SystemInfo.operatingSystem);
				UnturnedLog.info("System Memory: " + SystemInfo.systemMemorySize.ToString() + "MB");
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception while logging system info:");
			}
		}

		/// <summary>
		/// Has the onApplicationQuitting callback been invoked?
		/// </summary>
		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600380A RID: 14346 RVA: 0x0010920C File Offset: 0x0010740C
		// (set) Token: 0x0600380B RID: 14347 RVA: 0x00109213 File Offset: 0x00107413
		public static bool isApplicationQuitting { get; private set; }

		/// <summary>
		/// Moved from OnApplicationQuit when that was deprecated.
		/// </summary>
		// Token: 0x0600380C RID: 14348 RVA: 0x0010921B File Offset: 0x0010741B
		private void onApplicationQuitting()
		{
			UnturnedLog.info("Application quitting");
			Provider.isApplicationQuitting = true;
			if (!Provider.isInitialized)
			{
				return;
			}
			Provider.RequestDisconnect("application quitting");
			Provider.provider.shutdown();
			UnturnedLog.info("Finished quitting");
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x00109253 File Offset: 0x00107453
		public static void QuitGame(string reason)
		{
			UnturnedLog.info("Quit game: " + reason);
			Provider.wasQuitGameCalled = true;
			Application.Quit();
		}

		/// <summary>
		/// Moved from OnApplicationQuit when Application.CancelQuit was deprecated.
		/// </summary>
		// Token: 0x0600380E RID: 14350 RVA: 0x00109270 File Offset: 0x00107470
		private bool onApplicationWantsToQuit()
		{
			bool flag = Provider.wasQuitGameCalled;
			return true;
		}

		// Token: 0x0400208D RID: 8333
		public static readonly string STEAM_IC = "Steam";

		// Token: 0x0400208E RID: 8334
		public static readonly string STEAM_DC = "<color=#2784c6>Steam</color>";

		// Token: 0x0400208F RID: 8335
		public static readonly AppId_t APP_ID = new AppId_t(304930U);

		// Token: 0x04002090 RID: 8336
		public static readonly AppId_t PRO_ID = new AppId_t(306460U);

		// Token: 0x04002093 RID: 8339
		public static readonly string APP_NAME = "Unturned";

		// Token: 0x04002094 RID: 8340
		public static readonly string APP_AUTHOR = "Nelson Sexton";

		// Token: 0x04002095 RID: 8341
		public static readonly int CLIENT_TIMEOUT = 30;

		// Token: 0x04002096 RID: 8342
		internal static readonly float PING_REQUEST_INTERVAL = 1f;

		// Token: 0x04002097 RID: 8343
		private static bool isCapturingScreenshot;

		// Token: 0x04002098 RID: 8344
		private static StaticResourceRef<Material> screenshotBlitMaterial = new StaticResourceRef<Material>("Materials/ScreenshotBlit");

		// Token: 0x04002099 RID: 8345
		private static Callback<ScreenshotRequested_t> screenshotRequestedCallback;

		// Token: 0x0400209A RID: 8346
		private static string privateLanguage;

		// Token: 0x0400209B RID: 8347
		internal static bool languageIsEnglish;

		// Token: 0x0400209C RID: 8348
		private static string _path;

		// Token: 0x0400209E RID: 8350
		public static Local localization;

		// Token: 0x040020A0 RID: 8352
		internal static IntPtr battlEyeClientHandle = IntPtr.Zero;

		// Token: 0x040020A1 RID: 8353
		internal static BEClient.BECL_GAME_DATA battlEyeClientInitData = null;

		// Token: 0x040020A2 RID: 8354
		internal static BEClient.BECL_BE_DATA battlEyeClientRunData = null;

		// Token: 0x040020A3 RID: 8355
		private static bool battlEyeHasRequiredRestart = false;

		// Token: 0x040020A4 RID: 8356
		internal static readonly NetLength battlEyeBufferSize = new NetLength(4095U);

		// Token: 0x040020A5 RID: 8357
		internal static IntPtr battlEyeServerHandle = IntPtr.Zero;

		// Token: 0x040020A6 RID: 8358
		internal static BEServer.BESV_GAME_DATA battlEyeServerInitData = null;

		// Token: 0x040020A7 RID: 8359
		internal static BEServer.BESV_BE_DATA battlEyeServerRunData = null;

		// Token: 0x040020A9 RID: 8361
		private static uint _bytesSent;

		// Token: 0x040020AA RID: 8362
		private static uint _bytesReceived;

		// Token: 0x040020AB RID: 8363
		private static uint _packetsSent;

		// Token: 0x040020AC RID: 8364
		private static uint _packetsReceived;

		// Token: 0x040020AD RID: 8365
		private static SteamServerAdvertisement _currentServerAdvertisement;

		// Token: 0x040020AE RID: 8366
		private static ServerConnectParameters _currentServerConnectParameters;

		// Token: 0x040020AF RID: 8367
		private static CSteamID _server;

		// Token: 0x040020B0 RID: 8368
		private static CSteamID _client;

		// Token: 0x040020B1 RID: 8369
		private static CSteamID _user;

		// Token: 0x040020B2 RID: 8370
		private static byte[] _clientHash;

		// Token: 0x040020B3 RID: 8371
		private static string _clientName;

		// Token: 0x040020B4 RID: 8372
		internal static List<SteamPlayer> _clients = new List<SteamPlayer>();

		// Token: 0x040020B5 RID: 8373
		public static List<SteamPending> pending = new List<SteamPending>();

		// Token: 0x040020B6 RID: 8374
		private static bool _isServer;

		// Token: 0x040020B7 RID: 8375
		private static bool _isClient;

		// Token: 0x040020B8 RID: 8376
		private static bool _isPro;

		// Token: 0x040020B9 RID: 8377
		private static bool _isConnected;

		// Token: 0x040020BA RID: 8378
		internal static bool isWaitingForWorkshopResponse;

		/// <summary>
		/// File IDs the client thinks the server advertised it was using, or null if UGC response was pending.
		/// Prevents the server from advertising a smaller or fake list of items.
		/// </summary>
		// Token: 0x040020BB RID: 8379
		private static List<PublishedFileId_t> waitingForExpectedWorkshopItems;

		/// <summary>
		/// Needed before loading level.
		/// </summary>
		// Token: 0x040020BC RID: 8380
		internal static ENPCHoliday authorityHoliday;

		// Token: 0x040020BD RID: 8381
		private static Provider.CachedWorkshopResponse currentServerWorkshopResponse;

		// Token: 0x040020BE RID: 8382
		private static List<ulong> _serverWorkshopFileIDs = new List<ulong>();

		// Token: 0x040020BF RID: 8383
		internal static List<Provider.ServerRequiredWorkshopFile> serverRequiredWorkshopFiles = new List<Provider.ServerRequiredWorkshopFile>();

		// Token: 0x040020C0 RID: 8384
		public static bool isLoadingUGC;

		// Token: 0x040020C1 RID: 8385
		public static bool isLoadingInventory;

		// Token: 0x040020C2 RID: 8386
		private static int nextPlayerChannelId = 2;

		// Token: 0x040020C3 RID: 8387
		public static ESteamConnectionFailureInfo _connectionFailureInfo;

		// Token: 0x040020C4 RID: 8388
		internal static string _connectionFailureReason;

		// Token: 0x040020C5 RID: 8389
		internal static uint _connectionFailureDuration;

		// Token: 0x040020C6 RID: 8390
		private static List<SteamChannel> _receivers = new List<SteamChannel>();

		// Token: 0x040020C7 RID: 8391
		internal static byte[] buffer = new byte[Block.BUFFER_SIZE];

		// Token: 0x040020C8 RID: 8392
		internal static List<Module> critMods = new List<Module>();

		// Token: 0x040020C9 RID: 8393
		private static StringBuilder modBuilder = new StringBuilder();

		// Token: 0x040020CA RID: 8394
		private static int nextBattlEyePlayerId = 1;

		/// <summary>
		/// Called when determining spawnpoint during player login.
		/// </summary>
		// Token: 0x040020CB RID: 8395
		public static Provider.LoginSpawningHandler onLoginSpawning;

		/// <summary>
		/// Is client waiting for response to ESteamPacket.CONNECT request?
		/// </summary>
		// Token: 0x040020CC RID: 8396
		internal static bool isWaitingForConnectResponse;

		/// <summary>
		/// Realtime that client sent ESteamPacket.CONNECT request.
		/// </summary>
		// Token: 0x040020CD RID: 8397
		private static float sentConnectRequestTime;

		// Token: 0x040020CE RID: 8398
		internal static float catPouncingMechanism = -33f;

		/// <summary>
		/// Nelson 2023-08-09: adding because in some cases, namely workshop download and level loading,
		/// we can't properly handle client transport failures because these loading systems don't
		/// currently support cancelling partway through. (public issue #4036)
		/// </summary>
		// Token: 0x040020CF RID: 8399
		private static bool canCurrentlyHandleClientTransportFailure;

		// Token: 0x040020D0 RID: 8400
		private static bool hasPendingClientTransportFailure;

		// Token: 0x040020D1 RID: 8401
		private static string pendingClientTransportFailureMessage;

		// Token: 0x040020D2 RID: 8402
		internal static readonly NetLength MAX_SKINS_LENGTH = new NetLength(127U);

		/// <summary>
		/// Manages client to server communication.
		/// </summary>
		// Token: 0x040020D3 RID: 8403
		internal static IClientTransport clientTransport;

		/// <summary>
		/// Manages server to client communication.
		/// </summary>
		// Token: 0x040020D4 RID: 8404
		private static IServerTransport serverTransport;

		// Token: 0x040020D6 RID: 8406
		private static int countShutdownTimer = -1;

		// Token: 0x040020D7 RID: 8407
		private static string shutdownMessage = string.Empty;

		// Token: 0x040020D8 RID: 8408
		private static float lastTimerMessage;

		// Token: 0x040020D9 RID: 8409
		internal static bool didServerShutdownTimerReachZero;

		/// <summary>
		/// Set on the server when initializing Steam API.
		/// Used to notify pending clients whether VAC is active.
		/// Set on clients after initial response is received.
		/// </summary>
		// Token: 0x040020DA RID: 8410
		internal static bool isVacActive;

		/// <summary>
		/// Set on the server when initializing BattlEye API.
		/// Used to notify pending clients whether BE is active.
		/// Set on clients after initial response is received.
		/// </summary>
		// Token: 0x040020DB RID: 8411
		internal static bool isBattlEyeActive;

		// Token: 0x040020DC RID: 8412
		private static bool hasSetIsBattlEyeActive;

		// Token: 0x040020DD RID: 8413
		private static bool isServerConnectedToSteam;

		// Token: 0x040020DE RID: 8414
		internal static BuiltinAutoShutdown autoShutdownManager = null;

		// Token: 0x040020DF RID: 8415
		private static IDedicatedWorkshopUpdateMonitor dswUpdateMonitor = null;

		// Token: 0x040020E0 RID: 8416
		private static bool isDedicatedUGCInstalled;

		/// <summary>
		/// Was not able to find documentation for this unfortunately,
		/// but it seems the max length is 127 characters as of 2022-09-12.
		/// </summary>
		// Token: 0x040020E1 RID: 8417
		private const int STEAM_KEYVALUE_MAX_VALUE_LENGTH = 127;

		// Token: 0x040020E2 RID: 8418
		[Obsolete]
		public static Provider.ServerWritingPacketHandler onServerWritingPacket;

		// Token: 0x040020E3 RID: 8419
		internal static List<Provider.WorkshopRequestLog> workshopRequests = new List<Provider.WorkshopRequestLog>();

		// Token: 0x040020E4 RID: 8420
		internal static List<Provider.CachedWorkshopResponse> cachedWorkshopResponses = new List<Provider.CachedWorkshopResponse>();

		// Token: 0x040020E5 RID: 8421
		private static List<CSteamID> netIgnoredSteamIDs = new List<CSteamID>();

		/// <summary>
		/// Private to prevent plugins from changing the value.
		/// </summary>
		// Token: 0x040020E6 RID: 8422
		private static CommandLineFlag _constNetEvents = new CommandLineFlag(false, "-ConstNetEvents");

		// Token: 0x040020E7 RID: 8423
		[Obsolete]
		public static Provider.ServerReadingPacketHandler onServerReadingPacket;

		// Token: 0x040020E8 RID: 8424
		private List<SteamPlayer> clientsWithBadConnecion = new List<SteamPlayer>();

		// Token: 0x040020E9 RID: 8425
		public static Provider.ServerConnected onServerConnected;

		// Token: 0x040020EA RID: 8426
		public static Provider.ServerDisconnected onServerDisconnected;

		// Token: 0x040020EB RID: 8427
		public static Provider.ServerHosted onServerHosted;

		// Token: 0x040020EC RID: 8428
		public static Provider.ServerShutdown onServerShutdown;

		// Token: 0x040020ED RID: 8429
		private static Callback<P2PSessionConnectFail_t> p2pSessionConnectFail;

		// Token: 0x040020EE RID: 8430
		[Obsolete("onCheckValidWithExplanation takes priority if bound")]
		public static Provider.CheckValid onCheckValid;

		// Token: 0x040020EF RID: 8431
		public static Provider.CheckValidWithExplanation onCheckValidWithExplanation;

		// Token: 0x040020F0 RID: 8432
		[Obsolete]
		public static Provider.CheckBanStatusHandler onCheckBanStatus;

		// Token: 0x040020F1 RID: 8433
		public static Provider.CheckBanStatusWithHWIDHandler onCheckBanStatusWithHWID;

		// Token: 0x040020F2 RID: 8434
		[Obsolete("V2 provides list of HWIDs to ban")]
		public static Provider.RequestBanPlayerHandler onBanPlayerRequested;

		// Token: 0x040020F3 RID: 8435
		public static Provider.RequestBanPlayerHandlerV2 onBanPlayerRequestedV2;

		// Token: 0x040020F4 RID: 8436
		public static Provider.RequestUnbanPlayerHandler onUnbanPlayerRequested;

		// Token: 0x040020F5 RID: 8437
		private static Callback<ValidateAuthTicketResponse_t> validateAuthTicketResponse;

		// Token: 0x040020F6 RID: 8438
		private static Callback<GSClientGroupStatus_t> clientGroupStatus;

		/// <summary>
		/// Allows hosting providers to limit the configurable max players value from the command-line.
		/// </summary>
		// Token: 0x040020F7 RID: 8439
		private static CommandLineInt clMaxPlayersLimit = new CommandLineInt("-MaxPlayersLimit");

		// Token: 0x040020F8 RID: 8440
		private static byte _maxPlayers;

		// Token: 0x040020F9 RID: 8441
		public static byte queueSize;

		// Token: 0x040020FA RID: 8442
		internal static byte _queuePosition;

		// Token: 0x040020FB RID: 8443
		public static Provider.QueuePositionUpdated onQueuePositionUpdated;

		// Token: 0x040020FC RID: 8444
		private static string _serverName;

		/// <summary>
		/// Deprecated-ish IPv4 to bind listen socket to. Set by bind command.
		/// </summary>
		// Token: 0x040020FD RID: 8445
		public static uint ip;

		/// <summary>
		/// Local address to bind listen socket to. Set by bind command.
		/// </summary>
		// Token: 0x040020FE RID: 8446
		public static string bindAddress;

		/// <summary>
		/// Steam query port.
		/// </summary>
		// Token: 0x040020FF RID: 8447
		public static ushort port;

		// Token: 0x04002100 RID: 8448
		internal static byte[] _serverPasswordHash;

		// Token: 0x04002101 RID: 8449
		private static string _serverPassword;

		// Token: 0x04002102 RID: 8450
		public static string map;

		// Token: 0x04002103 RID: 8451
		public static bool isPvP;

		// Token: 0x04002104 RID: 8452
		public static bool isWhitelisted;

		// Token: 0x04002105 RID: 8453
		public static bool hideAdmins;

		// Token: 0x04002106 RID: 8454
		public static bool hasCheats;

		// Token: 0x04002107 RID: 8455
		public static bool filterName;

		// Token: 0x04002108 RID: 8456
		public static EGameMode mode;

		// Token: 0x04002109 RID: 8457
		public static bool isGold;

		// Token: 0x0400210A RID: 8458
		public static GameMode gameMode;

		// Token: 0x0400210B RID: 8459
		public static ECameraMode cameraMode;

		// Token: 0x0400210C RID: 8460
		private static StatusData _statusData;

		// Token: 0x0400210D RID: 8461
		private static PreferenceData _preferenceData;

		// Token: 0x0400210E RID: 8462
		private static ConfigData _configData;

		// Token: 0x0400210F RID: 8463
		internal static ModeConfigData _modeConfigData;

		/// <summary>
		/// Number of transport connection failures on this frame.
		/// </summary>
		// Token: 0x04002111 RID: 8465
		private int clientsKickedForTransportConnectionFailureCount;

		// Token: 0x04002112 RID: 8466
		private static uint STEAM_FAVORITE_FLAG_FAVORITE = 1U;

		// Token: 0x04002113 RID: 8467
		internal static uint STEAM_FAVORITE_FLAG_HISTORY = 2U;

		// Token: 0x04002114 RID: 8468
		private static List<Provider.CachedFavorite> cachedFavorites = new List<Provider.CachedFavorite>();

		// Token: 0x04002115 RID: 8469
		public static Provider.ClientConnected onClientConnected;

		// Token: 0x04002116 RID: 8470
		public static Provider.ClientDisconnected onClientDisconnected;

		// Token: 0x04002117 RID: 8471
		public static Provider.EnemyConnected onEnemyConnected;

		// Token: 0x04002118 RID: 8472
		public static Provider.EnemyDisconnected onEnemyDisconnected;

		// Token: 0x04002119 RID: 8473
		private static Callback<PersonaStateChange_t> personaStateChange;

		// Token: 0x0400211A RID: 8474
		private static Callback<GetTicketForWebApiResponse_t> getTicketForWebApiResponseCallback;

		// Token: 0x0400211B RID: 8475
		private static Callback<GameServerChangeRequested_t> gameServerChangeRequested;

		// Token: 0x0400211C RID: 8476
		private static Callback<GameRichPresenceJoinRequested_t> gameRichPresenceJoinRequested;

		// Token: 0x0400211D RID: 8477
		private static HAuthTicket ticketHandle = HAuthTicket.Invalid;

		// Token: 0x0400211E RID: 8478
		private static Dictionary<HAuthTicket, string> pluginTicketHandles = new Dictionary<HAuthTicket, string>();

		// Token: 0x0400211F RID: 8479
		private static float lastPingRequestTime;

		// Token: 0x04002120 RID: 8480
		private static float lastQueueNotificationTime;

		// Token: 0x04002122 RID: 8482
		internal static float timeLastPingRequestWasSentToServer;

		// Token: 0x04002123 RID: 8483
		public static readonly float EPSILON = 0.01f;

		// Token: 0x04002124 RID: 8484
		public static readonly float UPDATE_TIME = 0.08f;

		// Token: 0x04002125 RID: 8485
		public static readonly float UPDATE_DELAY = 0.1f;

		// Token: 0x04002126 RID: 8486
		public static readonly float UPDATE_DISTANCE = 0.01f;

		// Token: 0x04002127 RID: 8487
		public static readonly uint UPDATES = 1U;

		// Token: 0x04002128 RID: 8488
		public static readonly float LERP = 3f;

		// Token: 0x04002129 RID: 8489
		internal const float INTERP_SPEED = 10f;

		// Token: 0x0400212A RID: 8490
		private static float[] pings;

		// Token: 0x0400212B RID: 8491
		private static float _ping;

		// Token: 0x0400212C RID: 8492
		private static Provider steam;

		// Token: 0x0400212E RID: 8494
		private static bool _isInitialized;

		// Token: 0x0400212F RID: 8495
		private static uint timeOffset;

		// Token: 0x04002130 RID: 8496
		private static uint _time;

		// Token: 0x04002131 RID: 8497
		private static uint initialBackendRealtimeSeconds;

		// Token: 0x04002132 RID: 8498
		private static float initialLocalRealtime;

		/// <summary>
		/// Current UTC as reported by backend servers.
		/// Used by holiday events to keep timing somewhat synced between players. 
		/// </summary>
		// Token: 0x04002133 RID: 8499
		private static DateTime unixEpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 1);

		/// <summary>
		/// Invoked after backend realtime becomes available.
		/// </summary>
		// Token: 0x04002134 RID: 8500
		public static Provider.BackendRealtimeAvailableHandler onBackendRealtimeAvailable;

		// Token: 0x04002135 RID: 8501
		private static SteamAPIWarningMessageHook_t apiWarningMessageHook;

		// Token: 0x04002136 RID: 8502
		private static int debugUpdates;

		// Token: 0x04002137 RID: 8503
		public static int debugUPS;

		// Token: 0x04002138 RID: 8504
		private static float debugLastUpdate;

		// Token: 0x04002139 RID: 8505
		private static int debugTicks;

		// Token: 0x0400213A RID: 8506
		public static int debugTPS;

		// Token: 0x0400213B RID: 8507
		private static float debugLastTick;

		// Token: 0x0400213C RID: 8508
		private static Dictionary<string, Texture2D> downloadedIconCache = new Dictionary<string, Texture2D>();

		// Token: 0x0400213D RID: 8509
		private static Dictionary<string, Provider.PendingIconRequest> pendingCachableIconRequests = new Dictionary<string, Provider.PendingIconRequest>();

		// Token: 0x0400213E RID: 8510
		internal static CommandLineFlag allowWebRequests = new CommandLineFlag(true, "-NoWebRequests");

		// Token: 0x04002140 RID: 8512
		private static bool wasQuitGameCalled;

		/// <summary>
		/// A couple of players have reported the PRO_DESYNC kick because their client thinks they own the gold upgrade,
		/// but the Steam backend thinks otherwise. This option is a bit of a hack to work around the problem for them.
		/// </summary>
		// Token: 0x04002141 RID: 8513
		private static CommandLineFlag shouldCheckForGoldUpgrade = new CommandLineFlag(true, "-NoGoldUpgrade");

		// Token: 0x020009B5 RID: 2485
		// (Invoke) Token: 0x06004C27 RID: 19495
		public delegate void BattlEyeKickCallback(SteamPlayer client, string reason);

		// Token: 0x020009B6 RID: 2486
		internal struct ServerRequiredWorkshopFile
		{
			// Token: 0x04003412 RID: 13330
			public ulong fileId;

			// Token: 0x04003413 RID: 13331
			public DateTime timestamp;
		}

		// Token: 0x020009B7 RID: 2487
		// (Invoke) Token: 0x06004C2B RID: 19499
		public delegate void LoginSpawningHandler(SteamPlayerID playerID, ref Vector3 point, ref float yaw, ref EPlayerStance initialStance, ref bool needsNewSpawnpoint);

		// Token: 0x020009B8 RID: 2488
		// (Invoke) Token: 0x06004C2F RID: 19503
		public delegate void CommenceShutdownHandler();

		// Token: 0x020009B9 RID: 2489
		// (Invoke) Token: 0x06004C33 RID: 19507
		[Obsolete]
		public delegate void ServerWritingPacketHandler(CSteamID remoteSteamId, ESteamPacket type, byte[] payload, int size, int channel);

		/// <summary>
		/// Workshop info is requested prior to authenticating so that it can be downloaded before joining,
		/// but cheat devs are abusing this to spam the server with workshop requests. This class keeps
		/// track of who and when requested that information.
		/// </summary>
		// Token: 0x020009BA RID: 2490
		internal struct WorkshopRequestLog
		{
			/// <summary>
			/// Hash code of remote connection.
			/// </summary>
			// Token: 0x04003414 RID: 13332
			public int sender;

			// Token: 0x04003415 RID: 13333
			public float realTime;
		}

		/// <summary>
		/// The server ignores workshop info requests if it's been less than 30 seconds,
		/// so we cache that info for 1 minute in-case we try to connect again right away.
		/// </summary>
		// Token: 0x020009BB RID: 2491
		internal class CachedWorkshopResponse
		{
			// Token: 0x06004C36 RID: 19510 RVA: 0x001B66FC File Offset: 0x001B48FC
			internal bool FindRequiredFile(ulong fileId, out Provider.ServerRequiredWorkshopFile details)
			{
				foreach (Provider.ServerRequiredWorkshopFile serverRequiredWorkshopFile in this.requiredFiles)
				{
					if (serverRequiredWorkshopFile.fileId == fileId)
					{
						details = serverRequiredWorkshopFile;
						return true;
					}
				}
				details = default(Provider.ServerRequiredWorkshopFile);
				return false;
			}

			/// <summary>
			/// This information is needed before the level is loaded.
			/// </summary>
			// Token: 0x04003416 RID: 13334
			public ENPCHoliday holiday;

			/// <summary>
			/// Advertised server name. e.g., "Nelson's Unturned Server"
			/// </summary>
			// Token: 0x04003417 RID: 13335
			public string serverName;

			/// <summary>
			/// Name of map to load.
			/// </summary>
			// Token: 0x04003418 RID: 13336
			public string levelName;

			// Token: 0x04003419 RID: 13337
			public bool isPvP;

			// Token: 0x0400341A RID: 13338
			public bool allowAdminCheatCodes;

			// Token: 0x0400341B RID: 13339
			public bool isVACSecure;

			// Token: 0x0400341C RID: 13340
			public bool isBattlEyeSecure;

			// Token: 0x0400341D RID: 13341
			public bool isGold;

			/// <summary>
			/// Legacy difficulty mode that should be removed eventually.
			/// </summary>
			// Token: 0x0400341E RID: 13342
			public EGameMode gameMode;

			/// <summary>
			/// Perspective settings.
			/// </summary>
			// Token: 0x0400341F RID: 13343
			public ECameraMode cameraMode;

			// Token: 0x04003420 RID: 13344
			public byte maxPlayers;

			// Token: 0x04003421 RID: 13345
			public string bookmarkHost;

			// Token: 0x04003422 RID: 13346
			public string thumbnailUrl;

			// Token: 0x04003423 RID: 13347
			public string serverDescription;

			// Token: 0x04003424 RID: 13348
			public CSteamID server;

			/// <summary>
			/// Server's IP from when we originally received response.
			/// Used to test download restrictions.
			/// </summary>
			// Token: 0x04003425 RID: 13349
			public uint ip;

			// Token: 0x04003426 RID: 13350
			public List<Provider.ServerRequiredWorkshopFile> requiredFiles = new List<Provider.ServerRequiredWorkshopFile>();

			/// <summary>
			/// Last realtime this cache was updated.
			/// </summary>
			// Token: 0x04003427 RID: 13351
			public float realTime;
		}

		// Token: 0x020009BC RID: 2492
		// (Invoke) Token: 0x06004C39 RID: 19513
		public delegate void ServerReadingPacketHandler(CSteamID remoteSteamId, byte[] payload, int offset, int size, int channel);

		// Token: 0x020009BD RID: 2493
		// (Invoke) Token: 0x06004C3D RID: 19517
		public delegate void ServerConnected(CSteamID steamID);

		// Token: 0x020009BE RID: 2494
		// (Invoke) Token: 0x06004C41 RID: 19521
		public delegate void ServerDisconnected(CSteamID steamID);

		// Token: 0x020009BF RID: 2495
		// (Invoke) Token: 0x06004C45 RID: 19525
		public delegate void ServerHosted();

		// Token: 0x020009C0 RID: 2496
		// (Invoke) Token: 0x06004C49 RID: 19529
		public delegate void ServerShutdown();

		// Token: 0x020009C1 RID: 2497
		// (Invoke) Token: 0x06004C4D RID: 19533
		[Obsolete]
		public delegate void CheckValid(ValidateAuthTicketResponse_t callback, ref bool isValid);

		// Token: 0x020009C2 RID: 2498
		// (Invoke) Token: 0x06004C51 RID: 19537
		public delegate void CheckValidWithExplanation(ValidateAuthTicketResponse_t callback, ref bool isValid, ref string explanation);

		// Token: 0x020009C3 RID: 2499
		// (Invoke) Token: 0x06004C55 RID: 19541
		public delegate void CheckBanStatusHandler(CSteamID steamID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration);

		// Token: 0x020009C4 RID: 2500
		// (Invoke) Token: 0x06004C59 RID: 19545
		public delegate void CheckBanStatusWithHWIDHandler(SteamPlayerID playerID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration);

		// Token: 0x020009C5 RID: 2501
		// (Invoke) Token: 0x06004C5D RID: 19549
		public delegate void RequestBanPlayerHandler(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan);

		// Token: 0x020009C6 RID: 2502
		// (Invoke) Token: 0x06004C61 RID: 19553
		public delegate void RequestBanPlayerHandlerV2(CSteamID instigator, CSteamID playerToBan, uint ipToBan, IEnumerable<byte[]> hwidsToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan);

		// Token: 0x020009C7 RID: 2503
		// (Invoke) Token: 0x06004C65 RID: 19557
		public delegate void RequestUnbanPlayerHandler(CSteamID instigator, CSteamID playerToUnban, ref bool shouldVanillaUnban);

		// Token: 0x020009C8 RID: 2504
		// (Invoke) Token: 0x06004C69 RID: 19561
		public delegate void QueuePositionUpdated();

		// Token: 0x020009C9 RID: 2505
		// (Invoke) Token: 0x06004C6D RID: 19565
		public delegate void RejectingPlayerCallback(CSteamID steamID, ESteamRejection rejection, string explanation);

		// Token: 0x020009CA RID: 2506
		private class CachedFavorite
		{
			// Token: 0x06004C70 RID: 19568 RVA: 0x001B677B File Offset: 0x001B497B
			public bool matchesServer(uint ip, ushort queryPort)
			{
				return this.ip == ip && this.queryPort == queryPort;
			}

			// Token: 0x04003428 RID: 13352
			public uint ip;

			// Token: 0x04003429 RID: 13353
			public ushort queryPort;

			// Token: 0x0400342A RID: 13354
			public bool isFavorited;
		}

		// Token: 0x020009CB RID: 2507
		// (Invoke) Token: 0x06004C73 RID: 19571
		public delegate void ClientConnected();

		// Token: 0x020009CC RID: 2508
		// (Invoke) Token: 0x06004C77 RID: 19575
		public delegate void ClientDisconnected();

		// Token: 0x020009CD RID: 2509
		// (Invoke) Token: 0x06004C7B RID: 19579
		public delegate void EnemyConnected(SteamPlayer player);

		// Token: 0x020009CE RID: 2510
		// (Invoke) Token: 0x06004C7F RID: 19583
		public delegate void EnemyDisconnected(SteamPlayer player);

		// Token: 0x020009CF RID: 2511
		// (Invoke) Token: 0x06004C83 RID: 19587
		public delegate void BackendRealtimeAvailableHandler();

		// Token: 0x020009D0 RID: 2512
		// (Invoke) Token: 0x06004C87 RID: 19591
		public delegate void IconQueryCallback(Texture2D icon, bool responsibleForDestroy);

		// Token: 0x020009D1 RID: 2513
		public struct IconQueryParams
		{
			// Token: 0x06004C8A RID: 19594 RVA: 0x001B6799 File Offset: 0x001B4999
			public IconQueryParams(string url, Provider.IconQueryCallback callback, bool shouldCache = true)
			{
				this.url = url;
				this.callback = callback;
				this.shouldCache = shouldCache;
			}

			// Token: 0x0400342B RID: 13355
			public string url;

			// Token: 0x0400342C RID: 13356
			public Provider.IconQueryCallback callback;

			// Token: 0x0400342D RID: 13357
			public bool shouldCache;
		}

		// Token: 0x020009D2 RID: 2514
		private class PendingIconRequest
		{
			// Token: 0x0400342E RID: 13358
			public string url;

			// Token: 0x0400342F RID: 13359
			public Provider.IconQueryCallback callback;

			// Token: 0x04003430 RID: 13360
			public bool shouldCache;
		}
	}
}
