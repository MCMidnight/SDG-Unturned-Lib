using System;
using System.Collections;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000604 RID: 1540
	public class Player : MonoBehaviour
	{
		/// <summary>
		/// Per-player event invoked when admin usage flags change.
		/// </summary>
		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x06003070 RID: 12400 RVA: 0x000D50A4 File Offset: 0x000D32A4
		// (remove) Token: 0x06003071 RID: 12401 RVA: 0x000D50DC File Offset: 0x000D32DC
		public event AdminUsageFlagsChanged OnAdminUsageChanged;

		/// <summary>
		/// Event invoked when any player's admin usage flags change.
		/// </summary>
		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06003072 RID: 12402 RVA: 0x000D5114 File Offset: 0x000D3314
		// (remove) Token: 0x06003073 RID: 12403 RVA: 0x000D5148 File Offset: 0x000D3348
		public static event AdminUsageFlagsChanged OnAnyPlayerAdminUsageChanged;

		/// <summary>
		/// Used by plugins.
		/// </summary>
		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06003074 RID: 12404 RVA: 0x000D517C File Offset: 0x000D337C
		// (remove) Token: 0x06003075 RID: 12405 RVA: 0x000D51B0 File Offset: 0x000D33B0
		public static event Player.PlayerStatIncremented onPlayerStatIncremented;

		/// <summary>
		/// Invoked on client when a plugin changes the widget flags. 
		/// </summary>
		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06003076 RID: 12406 RVA: 0x000D51E4 File Offset: 0x000D33E4
		// (remove) Token: 0x06003077 RID: 12407 RVA: 0x000D521C File Offset: 0x000D341C
		public event Player.PluginWidgetFlagsChanged onLocalPluginWidgetFlagsChanged;

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x000D5251 File Offset: 0x000D3451
		public static bool isLoading
		{
			get
			{
				return Player.isLoadingLife || Player.isLoadingInventory || Player.isLoadingClothing;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x000D5268 File Offset: 0x000D3468
		public static Player player
		{
			get
			{
				return Player._player;
			}
		}

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x000D526F File Offset: 0x000D346F
		public static Player instance
		{
			get
			{
				return Player.player;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x0600307B RID: 12411 RVA: 0x000D5276 File Offset: 0x000D3476
		public SteamChannel channel
		{
			get
			{
				return this._channel;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000D527E File Offset: 0x000D347E
		public PlayerAnimator animator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x000D5286 File Offset: 0x000D3486
		public PlayerClothing clothing
		{
			get
			{
				return this._clothing;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x000D528E File Offset: 0x000D348E
		public PlayerInventory inventory
		{
			get
			{
				return this._inventory;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x000D5296 File Offset: 0x000D3496
		public PlayerEquipment equipment
		{
			get
			{
				return this._equipment;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000D529E File Offset: 0x000D349E
		public PlayerLife life
		{
			get
			{
				return this._life;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x000D52A6 File Offset: 0x000D34A6
		public PlayerCrafting crafting
		{
			get
			{
				return this._crafting;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000D52AE File Offset: 0x000D34AE
		public PlayerSkills skills
		{
			get
			{
				return this._skills;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x000D52B6 File Offset: 0x000D34B6
		public PlayerMovement movement
		{
			get
			{
				return this._movement;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000D52BE File Offset: 0x000D34BE
		public PlayerLook look
		{
			get
			{
				return this._look;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003085 RID: 12421 RVA: 0x000D52C6 File Offset: 0x000D34C6
		public PlayerStance stance
		{
			get
			{
				return this._stance;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000D52CE File Offset: 0x000D34CE
		public PlayerInput input
		{
			get
			{
				return this._input;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x000D52D6 File Offset: 0x000D34D6
		public PlayerVoice voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000D52DE File Offset: 0x000D34DE
		public PlayerInteract interact
		{
			get
			{
				return this._interact;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x000D52E6 File Offset: 0x000D34E6
		public PlayerWorkzone workzone
		{
			get
			{
				return this._workzone;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000D52EE File Offset: 0x000D34EE
		public PlayerQuests quests
		{
			get
			{
				return this._quests;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x000D52F6 File Offset: 0x000D34F6
		public Transform first
		{
			get
			{
				return this._first;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000D52FE File Offset: 0x000D34FE
		public Transform third
		{
			get
			{
				return this._third;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x000D5306 File Offset: 0x000D3506
		public Transform character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000D530E File Offset: 0x000D350E
		public bool isSpotOn
		{
			get
			{
				return this.itemOn || this.headlampOn;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x000D5320 File Offset: 0x000D3520
		private PlayerSpotLightConfig lightConfig
		{
			get
			{
				if (this.itemOn && this.headlampOn)
				{
					return new PlayerSpotLightConfig
					{
						angle = Mathf.LerpAngle(this.itemLightConfig.angle, this.headlampLightConfig.angle, 0.5f),
						color = Color.Lerp(this.itemLightConfig.color, this.headlampLightConfig.color, 0.5f),
						intensity = Mathf.Lerp(this.itemLightConfig.intensity, this.headlampLightConfig.intensity, 0.5f),
						range = Mathf.Lerp(this.itemLightConfig.range, this.headlampLightConfig.range, 0.5f)
					};
				}
				if (this.itemOn)
				{
					return this.itemLightConfig;
				}
				if (this.headlampOn)
				{
					return this.headlampLightConfig;
				}
				return default(PlayerSpotLightConfig);
			}
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000D5410 File Offset: 0x000D3610
		public OneShotAudioHandle playSound(AudioClip clip, float volume, float pitch, float deviation)
		{
			return default(OneShotAudioHandle);
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000D5426 File Offset: 0x000D3626
		public OneShotAudioHandle playSound(AudioClip clip, float pitch, float deviation)
		{
			return this.playSound(clip, 1f, pitch, deviation);
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000D5436 File Offset: 0x000D3636
		public OneShotAudioHandle playSound(AudioClip clip, float volume)
		{
			return this.playSound(clip, volume, 1f, 0.1f);
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000D544A File Offset: 0x000D364A
		public OneShotAudioHandle playSound(AudioClip clip)
		{
			return this.playSound(clip, 1f, 1f, 0.1f);
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000D5462 File Offset: 0x000D3662
		[Obsolete]
		public void tellScreenshotDestination(CSteamID steamID)
		{
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000D5464 File Offset: 0x000D3664
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveScreenshotDestination(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			byte[] array = new byte[(int)num];
			reader.ReadBytes(array);
			this.HandleScreenshotData(array);
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000D5498 File Offset: 0x000D3698
		private void HandleScreenshotData(byte[] data)
		{
			ReadWrite.writeBytes(string.Concat(new string[]
			{
				ReadWrite.PATH,
				ServerSavedata.directory,
				"/",
				Provider.serverID,
				"/Spy.jpg"
			}), false, false, data);
			ReadWrite.writeBytes(string.Concat(new string[]
			{
				ReadWrite.PATH,
				ServerSavedata.directory,
				"/",
				Provider.serverID,
				"/Spy/",
				this.channel.owner.playerID.steamID.m_SteamID.ToString(),
				".jpg"
			}), false, false, data);
			PlayerSpyReady playerSpyReady = this.onPlayerSpyReady;
			if (playerSpyReady != null)
			{
				playerSpyReady(this.channel.owner.playerID.steamID, data);
			}
			PlayerSpyReady playerSpyReady2 = this.screenshotsCallbacks.Dequeue();
			if (playerSpyReady2 == null)
			{
				return;
			}
			playerSpyReady2(this.channel.owner.playerID.steamID, data);
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000D559A File Offset: 0x000D379A
		[Obsolete]
		public void tellScreenshotRelay(CSteamID steamID)
		{
		}

		/// <summary>
		/// Not rate limited because server tracks number of expected screenshots.
		/// </summary>
		// Token: 0x06003098 RID: 12440 RVA: 0x000D559C File Offset: 0x000D379C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER)]
		public void ReceiveScreenshotRelay(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			if (this.screenshotsExpected < 1)
			{
				return;
			}
			this.screenshotsExpected--;
			ushort length;
			if (!SystemNetPakReaderEx.ReadUInt16(reader, ref length))
			{
				return;
			}
			byte[] data = new byte[(int)length];
			reader.ReadBytes(data);
			if (this.screenshotsDestination != CSteamID.Nil)
			{
				ITransportConnection transportConnection = Provider.findTransportConnection(this.screenshotsDestination);
				if (transportConnection != null)
				{
					Player.SendScreenshotDestination.Invoke(this.GetNetId(), ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt16(writer, length);
						writer.WriteBytes(data);
					});
				}
			}
			this.HandleScreenshotData(data);
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000D5646 File Offset: 0x000D3846
		private IEnumerator takeScreenshot()
		{
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = ScreenCapture.CaptureScreenshotAsTexture();
			RenderTexture temporary = RenderTexture.GetTemporary(640, 480, 0, texture2D.graphicsFormat);
			Graphics.Blit(texture2D, temporary);
			Object.Destroy(texture2D);
			if (this.screenshotFinal == null)
			{
				this.screenshotFinal = new Texture2D(640, 480, TextureFormat.RGB24, false);
				this.screenshotFinal.name = "Screenshot_Final";
				this.screenshotFinal.hideFlags = HideFlags.HideAndDontSave;
			}
			RenderTexture.active = temporary;
			this.screenshotFinal.ReadPixels(new Rect(0f, 0f, (float)this.screenshotFinal.width, (float)this.screenshotFinal.height), 0, 0, false);
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(temporary);
			byte[] data = ImageConversion.EncodeToJPG(this.screenshotFinal, 33);
			if (data.Length < 40000)
			{
				if (Provider.isServer)
				{
					this.HandleScreenshotData(data);
				}
				else
				{
					Player.SendScreenshotRelay.Invoke(this.GetNetId(), ENetReliability.Reliable, delegate(NetPakWriter writer)
					{
						ushort num = (ushort)data.Length;
						SystemNetPakWriterEx.WriteUInt16(writer, num);
						writer.WriteBytes(data, (int)num);
					});
				}
			}
			else
			{
				UnturnedLog.warn(string.Format("Unable to send screenshot to server because size ({0} bytes) exceeds limit", data.Length));
			}
			yield break;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000D5655 File Offset: 0x000D3855
		[Obsolete]
		public void askScreenshot(CSteamID steamID)
		{
			this.ReceiveTakeScreenshot();
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000D565D File Offset: 0x000D385D
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askScreenshot")]
		public void ReceiveTakeScreenshot()
		{
			base.StartCoroutine(this.takeScreenshot());
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000D566C File Offset: 0x000D386C
		public void sendScreenshot(CSteamID destination, PlayerSpyReady callback = null)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			this.screenshotsExpected++;
			this.screenshotsDestination = destination;
			this.screenshotsCallbacks.Enqueue(callback);
			Player.SendTakeScreenshot.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection());
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000D56BB File Offset: 0x000D38BB
		[Obsolete]
		public void askBrowserRequest(CSteamID steamID, string msg, string url)
		{
			this.ReceiveBrowserRequest(msg, url);
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000D56C8 File Offset: 0x000D38C8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askBrowserRequest")]
		public void ReceiveBrowserRequest(string msg, string url)
		{
			if (!WebUtils.CanParseThirdPartyUrl(url, true, true))
			{
				UnturnedLog.warn("Ignoring potentially unsafe browser request \"{0}\" \"{1}\"", new object[]
				{
					msg,
					url
				});
				return;
			}
			if (PlayerUI.instance != null)
			{
				PlayerUI.instance.browserRequestUI.open(msg, url);
				PlayerLifeUI.close();
			}
		}

		/// <summary>
		/// Request client to open a given URL.
		/// Allows plugins to open web browser, but also gives client the chance to ignore it.
		/// </summary>
		// Token: 0x0600309F RID: 12447 RVA: 0x000D571B File Offset: 0x000D391B
		public void sendBrowserRequest(string msg, string url)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			Player.SendBrowserRequest.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), msg, url);
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000D5740 File Offset: 0x000D3940
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveHintMessage(string message, float durationSeconds)
		{
			if (PlayerUI.instance != null)
			{
				PlayerUI.message(EPlayerMessage.NPC_CUSTOM, message, durationSeconds);
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000D5758 File Offset: 0x000D3958
		public void ServerShowHint(string message, float durationSeconds)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			Player.SendHintMessage.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), message, durationSeconds);
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000D577D File Offset: 0x000D397D
		[Obsolete]
		public void askRelayToServer(CSteamID steamID, uint ip, ushort port, string password, bool shouldShowMenu)
		{
			this.ReceiveRelayToServer(ip, port, CSteamID.Nil, password, shouldShowMenu);
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000D5790 File Offset: 0x000D3990
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveRelayToServer(uint ip, ushort port, CSteamID serverCode, string password, bool shouldShowMenu)
		{
			if (MenuPlayConnectUI.hasPendingServerRelay)
			{
				return;
			}
			if (Provider.isServer)
			{
				throw new NotSupportedException(string.Format("IP: {0} Port: {1} Server Code: {2}", Parser.getIPFromUInt32(ip), port, serverCode));
			}
			MenuPlayConnectUI.hasPendingServerRelay = true;
			MenuPlayConnectUI.serverRelayIP = ip;
			MenuPlayConnectUI.serverRelayPort = port;
			MenuPlayConnectUI.serverRelayServerCode = serverCode;
			MenuPlayConnectUI.serverRelayPassword = password;
			MenuPlayConnectUI.serverRelayWaitOnMenu = shouldShowMenu;
			Provider.RequestDisconnect(string.Format("Relaying to IP: {0} Port: {1} Code: {2}", Parser.getIPFromUInt32(ip), port, serverCode));
		}

		/// <summary>
		/// Tell client to join a specific server.
		/// Disconnects client and sends them to the join server screen.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x060030A4 RID: 12452 RVA: 0x000D5818 File Offset: 0x000D3A18
		public void sendRelayToServer(uint ip, ushort port, string password, bool shouldShowMenu = true)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			Player.SendRelayToServer.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), ip, port, CSteamID.Nil, password, shouldShowMenu);
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000D5850 File Offset: 0x000D3A50
		public void sendRelayToServer(CSteamID serverCode, string password, bool shouldShowMenu = true)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			Player.SendRelayToServer.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), 0U, 0, serverCode, password, shouldShowMenu);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000D5883 File Offset: 0x000D3A83
		public void sendRelayToServer(uint ip, ushort port, string password)
		{
			this.sendRelayToServer(ip, port, password, true);
		}

		/// <summary>
		/// Is this player currently in a plugin's modal dialog?
		/// Enables cursor movement while not in a vanilla menu.
		/// </summary>
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x000D588F File Offset: 0x000D3A8F
		public bool inPluginModal
		{
			get
			{
				return this.isPluginWidgetFlagActive(EPluginWidgetFlags.Modal);
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000D5898 File Offset: 0x000D3A98
		public bool isPluginWidgetFlagActive(EPluginWidgetFlags flag)
		{
			return (this.pluginWidgetFlags & flag) == flag;
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x000D58A5 File Offset: 0x000D3AA5
		// (set) Token: 0x060030AA RID: 12458 RVA: 0x000D58AD File Offset: 0x000D3AAD
		public EPluginWidgetFlags pluginWidgetFlags { get; protected set; } = EPluginWidgetFlags.Default;

		// Token: 0x060030AB RID: 12459 RVA: 0x000D58B6 File Offset: 0x000D3AB6
		[Obsolete]
		public void clientsideSetPluginWidgetFlags(CSteamID steamID, uint newFlags)
		{
			this.ReceiveSetPluginWidgetFlags(newFlags);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000D58C0 File Offset: 0x000D3AC0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "clientsideSetPluginWidgetFlags")]
		public void ReceiveSetPluginWidgetFlags(uint newFlags)
		{
			EPluginWidgetFlags pluginWidgetFlags = this.pluginWidgetFlags;
			this.pluginWidgetFlags = (EPluginWidgetFlags)newFlags;
			Player.PluginWidgetFlagsChanged pluginWidgetFlagsChanged = this.onLocalPluginWidgetFlagsChanged;
			if (pluginWidgetFlagsChanged == null)
			{
				return;
			}
			pluginWidgetFlagsChanged(this, pluginWidgetFlags);
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000D58ED File Offset: 0x000D3AED
		public void setAllPluginWidgetFlags(EPluginWidgetFlags newFlags)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			if (this.pluginWidgetFlags == newFlags)
			{
				return;
			}
			Player.SendSetPluginWidgetFlags.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), (uint)newFlags);
			this.pluginWidgetFlags = newFlags;
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000D5924 File Offset: 0x000D3B24
		public void enablePluginWidgetFlag(EPluginWidgetFlags flag)
		{
			EPluginWidgetFlags allPluginWidgetFlags = this.pluginWidgetFlags | flag;
			this.setAllPluginWidgetFlags(allPluginWidgetFlags);
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x000D5944 File Offset: 0x000D3B44
		public void disablePluginWidgetFlag(EPluginWidgetFlags flag)
		{
			EPluginWidgetFlags allPluginWidgetFlags = this.pluginWidgetFlags & ~flag;
			this.setAllPluginWidgetFlags(allPluginWidgetFlags);
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000D5962 File Offset: 0x000D3B62
		public void setPluginWidgetFlag(EPluginWidgetFlags flag, bool active)
		{
			if (active)
			{
				this.enablePluginWidgetFlag(flag);
				return;
			}
			this.disablePluginWidgetFlag(flag);
		}

		/// <summary>
		/// Tell the client whether to be in plugin modal mode or not.
		/// Kept from prior to introduction of pluginWidgetFlags.
		/// </summary>
		// Token: 0x060030B1 RID: 12465 RVA: 0x000D5976 File Offset: 0x000D3B76
		[Obsolete]
		public void serversideSetPluginModal(bool enableModal)
		{
			this.setPluginWidgetFlag(EPluginWidgetFlags.Modal, enableModal);
		}

		/// <summary>
		/// Which admin powers are currently in use by the client.
		/// Reported to the server by the client.
		/// Does not control which admin powers are available.
		/// Note: Hacks can prevent this notification from being sent.
		/// </summary>
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000D5980 File Offset: 0x000D3B80
		public EPlayerAdminUsageFlags AdminUsageFlags
		{
			get
			{
				return this._adminUsageFlags;
			}
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x000D5988 File Offset: 0x000D3B88
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 8)]
		public void ReceiveAdminUsageFlags(in ServerInvocationContext context, uint newFlagsBitmask)
		{
			try
			{
			}
			catch
			{
				context.Kick("invalid admin usage flags");
				return;
			}
			if (this._adminUsageFlags != (EPlayerAdminUsageFlags)newFlagsBitmask)
			{
				EPlayerAdminUsageFlags adminUsageFlags = this._adminUsageFlags;
				this._adminUsageFlags = (EPlayerAdminUsageFlags)newFlagsBitmask;
				if (adminUsageFlags.HasFlag(EPlayerAdminUsageFlags.Freecam) != ((EPlayerAdminUsageFlags)newFlagsBitmask).HasFlag(EPlayerAdminUsageFlags.Freecam))
				{
					if (((EPlayerAdminUsageFlags)newFlagsBitmask).HasFlag(EPlayerAdminUsageFlags.Freecam))
					{
						UnturnedLog.info(string.Format("{0} entered freecam admin mode", this.channel.owner.playerID));
					}
					else
					{
						UnturnedLog.info(string.Format("{0} exited freecam admin mode", this.channel.owner.playerID));
					}
				}
				if (adminUsageFlags.HasFlag(EPlayerAdminUsageFlags.Workzone) != ((EPlayerAdminUsageFlags)newFlagsBitmask).HasFlag(EPlayerAdminUsageFlags.Workzone))
				{
					if (((EPlayerAdminUsageFlags)newFlagsBitmask).HasFlag(EPlayerAdminUsageFlags.Workzone))
					{
						UnturnedLog.info(string.Format("{0} entered workzone admin mode", this.channel.owner.playerID));
					}
					else
					{
						UnturnedLog.info(string.Format("{0} exited workzone admin mode", this.channel.owner.playerID));
					}
				}
				if (adminUsageFlags.HasFlag(EPlayerAdminUsageFlags.SpectatorStatsOverlay) != ((EPlayerAdminUsageFlags)newFlagsBitmask).HasFlag(EPlayerAdminUsageFlags.SpectatorStatsOverlay))
				{
					if (((EPlayerAdminUsageFlags)newFlagsBitmask).HasFlag(EPlayerAdminUsageFlags.SpectatorStatsOverlay))
					{
						UnturnedLog.info(string.Format("{0} turned on spectator stats overlay admin mode", this.channel.owner.playerID));
					}
					else
					{
						UnturnedLog.info(string.Format("{0} turned off spectator stats overlay admin mode", this.channel.owner.playerID));
					}
				}
				AdminUsageFlagsChanged onAdminUsageChanged = this.OnAdminUsageChanged;
				if (onAdminUsageChanged != null)
				{
					onAdminUsageChanged(this, adminUsageFlags, (EPlayerAdminUsageFlags)newFlagsBitmask);
				}
				AdminUsageFlagsChanged onAnyPlayerAdminUsageChanged = Player.OnAnyPlayerAdminUsageChanged;
				if (onAnyPlayerAdminUsageChanged == null)
				{
					return;
				}
				onAnyPlayerAdminUsageChanged(this, adminUsageFlags, (EPlayerAdminUsageFlags)newFlagsBitmask);
			}
		}

		/// <summary>
		/// Called on the client to notify the server of admin usage changes (if any).
		/// </summary>
		// Token: 0x060030B4 RID: 12468 RVA: 0x000D5B60 File Offset: 0x000D3D60
		private void ClientSetAdminUsageFlags(EPlayerAdminUsageFlags newFlags)
		{
			if (this._adminUsageFlags != newFlags)
			{
				this._adminUsageFlags = newFlags;
				Player.SendAdminUsageFlags.Invoke(this.GetNetId(), ENetReliability.Reliable, (uint)this._adminUsageFlags);
			}
		}

		/// <summary>
		/// Called on the client to notify the server of admin usage changes (if any).
		/// </summary>
		// Token: 0x060030B5 RID: 12469 RVA: 0x000D5B89 File Offset: 0x000D3D89
		internal void ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags flag, bool active)
		{
			if (active)
			{
				this.ClientSetAdminUsageFlags(this._adminUsageFlags | flag);
				return;
			}
			this.ClientSetAdminUsageFlags(this._adminUsageFlags & ~flag);
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000D5BAC File Offset: 0x000D3DAC
		// (set) Token: 0x060030B7 RID: 12471 RVA: 0x000D5BB4 File Offset: 0x000D3DB4
		public bool wantsBattlEyeLogs { get; protected set; }

		// Token: 0x060030B8 RID: 12472 RVA: 0x000D5BBD File Offset: 0x000D3DBD
		[Obsolete]
		public void askRequestBattlEyeLogs(CSteamID steamID)
		{
			this.ReceiveBattlEyeLogsRequest();
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000D5BC5 File Offset: 0x000D3DC5
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 1, legacyName = "askRequestBattlEyeLogs")]
		public void ReceiveBattlEyeLogsRequest()
		{
			if (!this.channel.owner.isAdmin)
			{
				return;
			}
			this.wantsBattlEyeLogs = !this.wantsBattlEyeLogs;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000D5BE9 File Offset: 0x000D3DE9
		[Obsolete]
		public void tellTerminalRelay(CSteamID steamID, string internalMessage)
		{
			this.ReceiveTerminalRelay(internalMessage);
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000D5BF2 File Offset: 0x000D3DF2
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellTerminalRelay")]
		public void ReceiveTerminalRelay(string internalMessage)
		{
			UnturnedLog.info(internalMessage);
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000D5BFA File Offset: 0x000D3DFA
		[Obsolete]
		public void sendTerminalRelay(string internalMessage, string internalCategory, string displayCategory)
		{
			this.sendTerminalRelay(internalMessage);
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000D5C03 File Offset: 0x000D3E03
		public void sendTerminalRelay(string internalMessage)
		{
			Player.SendTerminalRelay.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), internalMessage);
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000D5C22 File Offset: 0x000D3E22
		internal void PostTeleport()
		{
			PlayerTeleported playerTeleported = this.onPlayerTeleported;
			if (playerTeleported == null)
			{
				return;
			}
			playerTeleported(this, base.transform.position);
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000D5C40 File Offset: 0x000D3E40
		[Obsolete]
		public void askTeleport(CSteamID steamID, Vector3 position, byte angle)
		{
			this.ReceiveTeleport(position, angle);
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000D5C4C File Offset: 0x000D3E4C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askTeleport")]
		public void ReceiveTeleport(Vector3 position, byte angle)
		{
			bool flag = false;
			if (this.movement.controller != null)
			{
				this.movement.controller.DisableDetectCollisionsUntilNextFrame();
				flag = this.movement.controller.enabled;
				this.movement.controller.enabled = false;
			}
			base.transform.position = position + new Vector3(0f, 0.5f, 0f);
			base.transform.rotation = Quaternion.Euler(0f, (float)(angle * 2), 0f);
			if (flag)
			{
				this.movement.controller.enabled = true;
			}
			this.look.updateLook();
			this.movement.updateMovement();
			this.PostTeleport();
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000D5D13 File Offset: 0x000D3F13
		public void sendTeleport(Vector3 position, byte angle)
		{
			CommandWindow.LogWarning("Please use teleportToPlayer or teleportToLocation rather than sendTeleport, as they check for error conditions and safe space");
			this.teleportToLocation(position, (float)angle);
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000D5D2C File Offset: 0x000D3F2C
		public bool teleportToPlayer(Player otherPlayer)
		{
			if (otherPlayer == null)
			{
				return false;
			}
			if (otherPlayer.movement.getVehicle() != null)
			{
				return false;
			}
			Vector3 position = otherPlayer.transform.position;
			float y = otherPlayer.transform.rotation.eulerAngles.y;
			return this.teleportToLocation(position, y);
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000D5D86 File Offset: 0x000D3F86
		public bool teleportToLocation(Vector3 position, float yaw)
		{
			if (!this.stance.wouldHaveHeightClearanceAtPosition(position, 0.5f))
			{
				return false;
			}
			this.teleportToLocationUnsafe(position, yaw);
			return true;
		}

		/// <summary>
		/// Teleport to a random player spawn designated in the level.
		/// </summary>
		// Token: 0x060030C4 RID: 12484 RVA: 0x000D5DA8 File Offset: 0x000D3FA8
		public bool teleportToRandomSpawnPoint()
		{
			PlayerSpawnpoint spawn = LevelPlayers.getSpawn(false);
			if (spawn != null)
			{
				this.teleportToLocationUnsafe(spawn.point + new Vector3(0f, 0.5f, 0f), spawn.angle);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Teleport to bed, if player has set one.
		/// </summary>
		// Token: 0x060030C5 RID: 12485 RVA: 0x000D5DF0 File Offset: 0x000D3FF0
		public bool teleportToBed()
		{
			Vector3 position;
			byte angle;
			if (BarricadeManager.tryGetBed(this.channel.owner.playerID.steamID, out position, out angle))
			{
				position.y += 0.5f;
				float yaw = MeasurementTool.byteToAngle(angle);
				return this.teleportToLocation(position, yaw);
			}
			return false;
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000D5E3E File Offset: 0x000D403E
		public bool adjustStanceOrTeleportIfStuck()
		{
			return this.stance.adjustStanceOrTeleportIfStuck();
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000D5E4C File Offset: 0x000D404C
		public void teleportToLocationUnsafe(Vector3 position, float yaw)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			InteractableVehicle vehicle = this.movement.getVehicle();
			if (!(vehicle == null))
			{
				VehicleManager.removePlayerTeleportUnsafe(vehicle, this, position, yaw);
				return;
			}
			byte b = MeasurementTool.angleToByte(yaw);
			if (this.movement.canAddSimulationResultsToUpdates)
			{
				Player.SendTeleport.InvokeAndLoopback(this.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), position, b);
				return;
			}
			Player.SendTeleport.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), position, b);
			this.ReceiveTeleport(position, b);
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000D5ED1 File Offset: 0x000D40D1
		[Obsolete]
		public void tellStat(CSteamID steamID, byte newStat)
		{
			this.ReceiveStat((EPlayerStat)newStat);
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000D5EDC File Offset: 0x000D40DC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellStat")]
		public void ReceiveStat(EPlayerStat stat)
		{
			if (stat == EPlayerStat.NONE)
			{
				return;
			}
			this.trackStat(stat);
			int num11;
			if (stat == EPlayerStat.KILLS_PLAYERS)
			{
				int num;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players", out num))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Players", num + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.KILLS_ZOMBIES_NORMAL)
			{
				int num2;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Normal", out num2))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Zombies_Normal", num2 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.KILLS_ZOMBIES_MEGA)
			{
				int num3;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Mega", out num3))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Zombies_Mega", num3 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.FOUND_ITEMS)
			{
				int num4;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Items", out num4))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Items", num4 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.FOUND_RESOURCES)
			{
				int num5;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Resources", out num5))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Resources", num5 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.KILLS_ANIMALS)
			{
				int num6;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Animals", out num6))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Animals", num6 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.FOUND_CRAFTS)
			{
				int num7;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Crafts", out num7))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Crafts", num7 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.FOUND_FISHES)
			{
				int num8;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Fishes", out num8))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Fishes", num8 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.FOUND_PLANTS)
			{
				int num9;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Plants", out num9))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Plants", num9 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.ARENA_WINS)
			{
				int num10;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Arena_Wins", out num10))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Arena_Wins", num10 + 1);
					return;
				}
			}
			else if (stat == EPlayerStat.FOUND_BUILDABLES && Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Buildables", out num11))
			{
				Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Buildables", num11 + 1);
			}
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000D61CF File Offset: 0x000D43CF
		[Obsolete]
		public void tellAchievementUnlocked(CSteamID steamID, string id)
		{
			this.ReceiveAchievementUnlocked(id);
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000D61D8 File Offset: 0x000D43D8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellAchievementUnlocked")]
		public void ReceiveAchievementUnlocked(string id)
		{
			if (Provider.statusData.Achievements.canBeGrantedByNPC(id))
			{
				bool flag;
				if (Provider.provider.achievementsService.getAchievement(id, out flag) && !flag)
				{
					Provider.provider.achievementsService.setAchievement(id);
					return;
				}
			}
			else
			{
				UnturnedLog.warn("Achievement " + id + " cannot be unlocked by NPCs");
			}
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x000D6238 File Offset: 0x000D4438
		protected void trackStat(EPlayerStat stat)
		{
			if (this.equipment.HasValidUseable && this.equipment.IsEquipAnimationFinished && this.equipment.asset != null)
			{
				this.channel.owner.incrementStatTrackerValue(this.equipment.asset.sharedSkinLookupID, stat);
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000D628D File Offset: 0x000D448D
		public void sendStat(EPlayerKill kill)
		{
			if (kill == EPlayerKill.PLAYER)
			{
				this.sendStat(EPlayerStat.KILLS_PLAYERS);
				return;
			}
			if (kill == EPlayerKill.ZOMBIE)
			{
				this.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
				return;
			}
			if (kill == EPlayerKill.MEGA)
			{
				this.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
				return;
			}
			if (kill == EPlayerKill.ANIMAL)
			{
				this.sendStat(EPlayerStat.KILLS_ANIMALS);
				return;
			}
			if (kill == EPlayerKill.RESOURCE)
			{
				this.sendStat(EPlayerStat.FOUND_RESOURCES);
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000D62CC File Offset: 0x000D44CC
		public void sendStat(EPlayerStat stat)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			if (!this.channel.IsLocalPlayer)
			{
				this.trackStat(stat);
			}
			Player.PlayerStatIncremented playerStatIncremented = Player.onPlayerStatIncremented;
			if (playerStatIncremented != null)
			{
				playerStatIncremented(this, stat);
			}
			Player.SendStat.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), stat);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000D6321 File Offset: 0x000D4521
		public void sendAchievementUnlocked(string id)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			Player.SendAchievementUnlocked.Invoke(this.GetNetId(), ENetReliability.Reliable, this.channel.GetOwnerTransportConnection(), id);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000D6345 File Offset: 0x000D4545
		[Obsolete]
		public void askMessage(CSteamID steamID, byte message)
		{
			this.ReceiveUIMessage((EPlayerMessage)message);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000D634E File Offset: 0x000D454E
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askMessage")]
		public void ReceiveUIMessage(EPlayerMessage message)
		{
			PlayerUI.message(message, string.Empty, 2f);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000D6360 File Offset: 0x000D4560
		public void sendMessage(EPlayerMessage message)
		{
			Player.SendUIMessage.Invoke(this.GetNetId(), ENetReliability.Unreliable, this.channel.GetOwnerTransportConnection(), message);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000D637F File Offset: 0x000D457F
		public void enableItemSpotLight(PlayerSpotLightConfig config)
		{
			this.itemLightConfig = config;
			this.itemOn = config.isEnabled;
			this.updateLights();
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000D639A File Offset: 0x000D459A
		public void disableItemSpotLight()
		{
			this.itemOn = false;
			this.updateLights();
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000D63AC File Offset: 0x000D45AC
		public void updateGlassesLights(bool on)
		{
			if (this.clothing.firstClothes != null && this.clothing.firstClothes.glassesModel != null)
			{
				Transform transform = this.clothing.firstClothes.glassesModel.Find("Model_0");
				if (transform != null)
				{
					Transform transform2 = transform.Find("Light");
					if (transform2 != null)
					{
						transform2.gameObject.SetActive(on);
					}
				}
			}
			if (this.clothing.thirdClothes != null && this.clothing.thirdClothes.glassesModel != null)
			{
				Transform transform3 = this.clothing.thirdClothes.glassesModel.Find("Model_0");
				if (transform3 != null)
				{
					Transform transform4 = transform3.Find("Light");
					if (transform4 != null)
					{
						transform4.gameObject.SetActive(on);
					}
				}
			}
			if (this.clothing.characterClothes != null && this.clothing.characterClothes.glassesModel != null)
			{
				Transform transform5 = this.clothing.characterClothes.glassesModel.Find("Model_0");
				if (transform5 != null)
				{
					Transform transform6 = transform5.Find("Light");
					if (transform6 != null)
					{
						transform6.gameObject.SetActive(on);
					}
				}
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000D650F File Offset: 0x000D470F
		public void enableHeadlamp(PlayerSpotLightConfig config)
		{
			this.headlampLightConfig = config;
			this.headlampOn = config.isEnabled;
			this.updateLights();
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000D652A File Offset: 0x000D472A
		public void disableHeadlamp()
		{
			this.headlampOn = false;
			this.updateLights();
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000D653C File Offset: 0x000D473C
		private void updateLights()
		{
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000D6549 File Offset: 0x000D4749
		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			if (this.isSpotOn)
			{
				this.updateLights();
			}
		}

		/// <summary>
		/// How many rate limited actions have been performed recently.
		/// Increased after performing each rate limited action, and decreased over time.
		/// Cannot perform actions when greater than one.
		/// </summary>
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x000D6559 File Offset: 0x000D4759
		// (set) Token: 0x060030DB RID: 12507 RVA: 0x000D6561 File Offset: 0x000D4761
		public float rateLimitedActionsCredits { get; protected set; }

		/// <summary>
		/// Note: new official code should be using per-method rate limit attribute.
		/// This is kept for backwards compatibility with plugins however.
		///
		/// Call this method before any requests the client can spam to the server.
		/// </summary>
		/// <returns>Should your code proceed with the rate limited action?</returns>
		// Token: 0x060030DC RID: 12508 RVA: 0x000D656A File Offset: 0x000D476A
		public bool tryToPerformRateLimitedAction()
		{
			bool flag = this.rateLimitedActionsCredits < 1f;
			if (flag)
			{
				this.rateLimitedActionsCredits += 1f / this.maxRateLimitedActionsPerSecond;
			}
			return flag;
		}

		/// <summary>
		/// Call every frame to cool down rate limiting.
		/// </summary>
		// Token: 0x060030DD RID: 12509 RVA: 0x000D6597 File Offset: 0x000D4797
		protected void updateRateLimiting()
		{
			this.rateLimitedActionsCredits -= Time.deltaTime;
			if (this.rateLimitedActionsCredits < 0f)
			{
				this.rateLimitedActionsCredits = 0f;
			}
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000D65C3 File Offset: 0x000D47C3
		private void Update()
		{
			if (Provider.isServer)
			{
				this.updateRateLimiting();
			}
		}

		/// <summary>
		/// This code was in the Start message, and should happen before other initialization.
		/// </summary>
		// Token: 0x060030DF RID: 12511 RVA: 0x000D65D4 File Offset: 0x000D47D4
		private void InitializePlayerStart()
		{
			if (this.channel.IsLocalPlayer)
			{
				Player._player = this;
				this._first = base.transform.Find("First");
				this._third = base.transform.Find("Third");
				this.first.gameObject.SetActive(true);
				this.third.gameObject.SetActive(true);
				this._character = ((GameObject)Object.Instantiate(Resources.Load("Characters/Inspect"))).transform;
				this.character.name = "Inspect";
				this.character.transform.position = new Vector3(256f, -256f, 0f);
				this.character.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				this.firstSpot = MainCamera.instance.transform.Find("Spot");
				this.firstSpot.localPosition = Vector3.zero;
				Player.isLoadingInventory = true;
				Player.isLoadingLife = true;
				Player.isLoadingClothing = true;
				PlayerLook look = this.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			else
			{
				this._first = null;
				this._third = base.transform.Find("Third");
				this.third.gameObject.SetActive(true);
			}
			this.thirdSpot = this.third.Find("Skeleton").Find("Spine").Find("Skull").Find("Spot");
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000D6784 File Offset: 0x000D4984
		internal void AssignNetIdBlock(NetId baseId)
		{
			baseId = (this._netId = ++baseId);
			NetIdRegistry.Assign(this._netId, this);
			this._animator.AssignNetId(baseId = ++baseId);
			this._clothing.AssignNetId(baseId = ++baseId);
			this._crafting.AssignNetId(baseId = ++baseId);
			this._equipment.AssignNetId(baseId = ++baseId);
			this._input.AssignNetId(baseId = ++baseId);
			this._interact.AssignNetId(baseId = ++baseId);
			this._inventory.AssignNetId(baseId = ++baseId);
			this._life.AssignNetId(baseId = ++baseId);
			this._look.AssignNetId(baseId = ++baseId);
			this._movement.AssignNetId(baseId = ++baseId);
			this._quests.AssignNetId(baseId = ++baseId);
			this._skills.AssignNetId(baseId = ++baseId);
			this._stance.AssignNetId(baseId = ++baseId);
			this._voice.AssignNetId(baseId = ++baseId);
		}

		/// <summary>
		/// Hacky replacement for Start() that runs after net ids are assigned but before sending player state.
		/// </summary>
		// Token: 0x060030E1 RID: 12513 RVA: 0x000D68C4 File Offset: 0x000D4AC4
		internal void InitializePlayer()
		{
			PlayerUI playerUI = null;
			if (this.channel.IsLocalPlayer)
			{
				Transform transform = base.transform.Find("First");
				PlayerUI playerUI2;
				if (transform == null)
				{
					playerUI2 = null;
				}
				else
				{
					Transform transform2 = transform.Find("Camera");
					playerUI2 = ((transform2 != null) ? transform2.GetComponent<PlayerUI>() : null);
				}
				playerUI = playerUI2;
			}
			this.InitializePlayerStart();
			this.clothing.InitializePlayer();
			this.inventory.InitializePlayer();
			this.life.InitializePlayer();
			this.skills.InitializePlayer();
			this.crafting.InitializePlayer();
			this.stance.InitializePlayer();
			this.movement.InitializePlayer();
			this.look.InitializePlayer();
			this.interact.InitializePlayer();
			this.animator.InitializePlayer();
			this.equipment.InitializePlayer();
			this.input.InitializePlayer();
			this.voice.InitializePlayer();
			if (this.workzone != null)
			{
				this.workzone.InitializePlayer();
			}
			this.quests.InitializePlayer();
			if (playerUI != null)
			{
				playerUI.InitializePlayer();
			}
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x000D69D8 File Offset: 0x000D4BD8
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			this.clothing.SendInitialPlayerState(client);
			this.inventory.SendInitialPlayerState(client);
			this.life.SendInitialPlayerState(client);
			this.skills.SendInitialPlayerState(client);
			this.stance.SendInitialPlayerState(client);
			this.quests.SendInitialPlayerState(client);
			this.equipment.SendInitialPlayerState(client);
			this.animator.SendInitialPlayerState(client);
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000D6A48 File Offset: 0x000D4C48
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			this.clothing.SendInitialPlayerState(transportConnections);
			this.life.SendInitialPlayerState(transportConnections);
			this.skills.SendInitialPlayerState(transportConnections);
			this.stance.SendInitialPlayerState(transportConnections);
			this.quests.SendInitialPlayerState(transportConnections);
			this.equipment.SendInitialPlayerState(transportConnections);
			this.animator.SendInitialPlayerState(transportConnections);
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000D6AAC File Offset: 0x000D4CAC
		internal void ReleaseNetIdBlock()
		{
			NetIdRegistry.Release(this._netId);
			this._netId.Clear();
			this._animator.ReleaseNetId();
			this._clothing.ReleaseNetId();
			this._crafting.ReleaseNetId();
			this._equipment.ReleaseNetId();
			this._input.ReleaseNetId();
			this._interact.ReleaseNetId();
			this._inventory.ReleaseNetId();
			this._life.ReleaseNetId();
			this._look.ReleaseNetId();
			this._movement.ReleaseNetId();
			this._quests.ReleaseNetId();
			this._skills.ReleaseNetId();
			this._stance.ReleaseNetId();
			this._voice.ReleaseNetId();
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000D6B6C File Offset: 0x000D4D6C
		private void Awake()
		{
			this._channel = base.GetComponent<SteamChannel>();
			this.agro = 0;
			this._animator = base.GetComponent<PlayerAnimator>();
			this._clothing = base.GetComponent<PlayerClothing>();
			this._inventory = base.GetComponent<PlayerInventory>();
			this._equipment = base.GetComponent<PlayerEquipment>();
			this._life = base.GetComponent<PlayerLife>();
			this._crafting = base.GetComponent<PlayerCrafting>();
			this._skills = base.GetComponent<PlayerSkills>();
			this._movement = base.GetComponent<PlayerMovement>();
			this._look = base.GetComponent<PlayerLook>();
			this._stance = base.GetComponent<PlayerStance>();
			this._input = base.GetComponent<PlayerInput>();
			this._voice = base.GetComponent<PlayerVoice>();
			this._interact = base.GetComponent<PlayerInteract>();
			this._workzone = base.GetComponent<PlayerWorkzone>();
			this._quests = base.GetComponent<PlayerQuests>();
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000D6C40 File Offset: 0x000D4E40
		private void OnDestroy()
		{
			if (this.screenshotFinal != null)
			{
				Object.DestroyImmediate(this.screenshotFinal);
				this.screenshotFinal = null;
			}
			if (this.channel != null && this.channel.IsLocalPlayer)
			{
				Player.isLoadingInventory = false;
				Player.isLoadingLife = false;
				Player.isLoadingClothing = false;
				this.channel.owner.commitModifiedDynamicProps();
			}
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000D6CAC File Offset: 0x000D4EAC
		public void save()
		{
			this.savePositionAndRotation();
			this.clothing.save();
			this.inventory.save();
			this.life.save();
			this.skills.save();
			this.animator.save();
			this.quests.save();
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000D6D04 File Offset: 0x000D4F04
		protected void savePositionAndRotation()
		{
			bool flag = this.life.IsAlive;
			if (flag)
			{
				Vector3 vector = base.transform.position;
				InteractableVehicle vehicle = this.movement.getVehicle();
				byte seat;
				Vector3 vector2;
				byte b;
				if (vehicle != null && vehicle.findPlayerSeat(this, out seat) && vehicle.tryGetExit(seat, out vector2, out b))
				{
					vector = vector2;
				}
				flag = vector.IsFinite();
			}
			if (flag)
			{
				Block block = new Block();
				block.writeByte(Player.SAVEDATA_VERSION);
				block.writeSingleVector3(base.transform.position);
				block.writeByte((byte)(base.transform.rotation.eulerAngles.y / 2f));
				PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Player.dat", block);
				return;
			}
			if (PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Player.dat"))
			{
				PlayerSavedata.deleteFile(this.channel.owner.playerID, "/Player/Player.dat");
			}
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000D6E05 File Offset: 0x000D5005
		public NetId GetNetId()
		{
			return this._netId;
		}

		// Token: 0x04001B8A RID: 7050
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04001B8B RID: 7051
		public static PlayerCreated onPlayerCreated;

		// Token: 0x04001B8C RID: 7052
		public PlayerTeleported onPlayerTeleported;

		// Token: 0x04001B8D RID: 7053
		public PlayerSpyReady onPlayerSpyReady;

		// Token: 0x04001B8E RID: 7054
		public static PlayerSpyReady onSpyReady;

		// Token: 0x04001B93 RID: 7059
		public static bool isLoadingInventory;

		// Token: 0x04001B94 RID: 7060
		public static bool isLoadingLife;

		// Token: 0x04001B95 RID: 7061
		public static bool isLoadingClothing;

		// Token: 0x04001B96 RID: 7062
		public int agro;

		// Token: 0x04001B97 RID: 7063
		private static Player _player;

		// Token: 0x04001B98 RID: 7064
		protected SteamChannel _channel;

		// Token: 0x04001B99 RID: 7065
		private PlayerAnimator _animator;

		// Token: 0x04001B9A RID: 7066
		private PlayerClothing _clothing;

		// Token: 0x04001B9B RID: 7067
		private PlayerInventory _inventory;

		// Token: 0x04001B9C RID: 7068
		private PlayerEquipment _equipment;

		// Token: 0x04001B9D RID: 7069
		private PlayerLife _life;

		// Token: 0x04001B9E RID: 7070
		private PlayerCrafting _crafting;

		// Token: 0x04001B9F RID: 7071
		private PlayerSkills _skills;

		// Token: 0x04001BA0 RID: 7072
		private PlayerMovement _movement;

		// Token: 0x04001BA1 RID: 7073
		private PlayerLook _look;

		// Token: 0x04001BA2 RID: 7074
		private PlayerStance _stance;

		// Token: 0x04001BA3 RID: 7075
		private PlayerInput _input;

		// Token: 0x04001BA4 RID: 7076
		private PlayerVoice _voice;

		// Token: 0x04001BA5 RID: 7077
		private PlayerInteract _interact;

		// Token: 0x04001BA6 RID: 7078
		private PlayerWorkzone _workzone;

		// Token: 0x04001BA7 RID: 7079
		private PlayerQuests _quests;

		// Token: 0x04001BA8 RID: 7080
		private Transform _first;

		// Token: 0x04001BA9 RID: 7081
		private Transform _third;

		// Token: 0x04001BAA RID: 7082
		private Transform _character;

		// Token: 0x04001BAB RID: 7083
		private Transform firstSpot;

		// Token: 0x04001BAC RID: 7084
		private Transform thirdSpot;

		// Token: 0x04001BAD RID: 7085
		private bool itemOn;

		// Token: 0x04001BAE RID: 7086
		private PlayerSpotLightConfig itemLightConfig;

		// Token: 0x04001BAF RID: 7087
		private bool headlampOn;

		// Token: 0x04001BB0 RID: 7088
		private PlayerSpotLightConfig headlampLightConfig;

		// Token: 0x04001BB1 RID: 7089
		private int screenshotsExpected;

		// Token: 0x04001BB2 RID: 7090
		private CSteamID screenshotsDestination;

		// Token: 0x04001BB3 RID: 7091
		private Queue<PlayerSpyReady> screenshotsCallbacks = new Queue<PlayerSpyReady>();

		// Token: 0x04001BB4 RID: 7092
		private static readonly ClientInstanceMethod SendScreenshotDestination = ClientInstanceMethod.Get(typeof(Player), "ReceiveScreenshotDestination");

		// Token: 0x04001BB5 RID: 7093
		private static readonly ServerInstanceMethod SendScreenshotRelay = ServerInstanceMethod.Get(typeof(Player), "ReceiveScreenshotRelay");

		// Token: 0x04001BB6 RID: 7094
		private Texture2D screenshotFinal;

		// Token: 0x04001BB7 RID: 7095
		private static readonly ClientInstanceMethod SendTakeScreenshot = ClientInstanceMethod.Get(typeof(Player), "ReceiveTakeScreenshot");

		// Token: 0x04001BB8 RID: 7096
		private static readonly ClientInstanceMethod<string, string> SendBrowserRequest = ClientInstanceMethod<string, string>.Get(typeof(Player), "ReceiveBrowserRequest");

		// Token: 0x04001BB9 RID: 7097
		private static readonly ClientInstanceMethod<string, float> SendHintMessage = ClientInstanceMethod<string, float>.Get(typeof(Player), "ReceiveHintMessage");

		// Token: 0x04001BBA RID: 7098
		private static readonly ClientInstanceMethod<uint, ushort, CSteamID, string, bool> SendRelayToServer = ClientInstanceMethod<uint, ushort, CSteamID, string, bool>.Get(typeof(Player), "ReceiveRelayToServer");

		// Token: 0x04001BBC RID: 7100
		private static readonly ClientInstanceMethod<uint> SendSetPluginWidgetFlags = ClientInstanceMethod<uint>.Get(typeof(Player), "ReceiveSetPluginWidgetFlags");

		// Token: 0x04001BBD RID: 7101
		private EPlayerAdminUsageFlags _adminUsageFlags;

		// Token: 0x04001BBE RID: 7102
		private static readonly ServerInstanceMethod<uint> SendAdminUsageFlags = ServerInstanceMethod<uint>.Get(typeof(Player), "ReceiveAdminUsageFlags");

		// Token: 0x04001BC0 RID: 7104
		private static readonly ServerInstanceMethod SendBattlEyeLogsRequest = ServerInstanceMethod.Get(typeof(Player), "ReceiveBattlEyeLogsRequest");

		// Token: 0x04001BC1 RID: 7105
		private static readonly ClientInstanceMethod<string> SendTerminalRelay = ClientInstanceMethod<string>.Get(typeof(Player), "ReceiveTerminalRelay");

		// Token: 0x04001BC2 RID: 7106
		internal const float TELEPORT_VERTICAL_OFFSET = 0.5f;

		// Token: 0x04001BC3 RID: 7107
		private static readonly ClientInstanceMethod<Vector3, byte> SendTeleport = ClientInstanceMethod<Vector3, byte>.Get(typeof(Player), "ReceiveTeleport");

		// Token: 0x04001BC4 RID: 7108
		private static readonly ClientInstanceMethod<EPlayerStat> SendStat = ClientInstanceMethod<EPlayerStat>.Get(typeof(Player), "ReceiveStat");

		// Token: 0x04001BC5 RID: 7109
		private static readonly ClientInstanceMethod<string> SendAchievementUnlocked = ClientInstanceMethod<string>.Get(typeof(Player), "ReceiveAchievementUnlocked");

		// Token: 0x04001BC6 RID: 7110
		private static readonly ClientInstanceMethod<EPlayerMessage> SendUIMessage = ClientInstanceMethod<EPlayerMessage>.Get(typeof(Player), "ReceiveUIMessage");

		/// <summary>
		/// How many calls to <see cref="M:SDG.Unturned.Player.tryToPerformRateLimitedAction" /> will succeed per second.
		/// </summary>
		// Token: 0x04001BC7 RID: 7111
		public uint maxRateLimitedActionsPerSecond = 10U;

		// Token: 0x04001BC9 RID: 7113
		private NetId _netId;

		// Token: 0x0200099D RID: 2461
		// (Invoke) Token: 0x06004BC9 RID: 19401
		public delegate void PlayerStatIncremented(Player player, EPlayerStat stat);

		// Token: 0x0200099E RID: 2462
		// (Invoke) Token: 0x06004BCD RID: 19405
		public delegate void PluginWidgetFlagsChanged(Player player, EPluginWidgetFlags oldFlags);
	}
}
