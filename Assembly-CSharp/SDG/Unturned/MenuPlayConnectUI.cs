using System;
using System.Net;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200079A RID: 1946
	public class MenuPlayConnectUI
	{
		/// <param name="shouldAutoJoin">If true the server is immediately joined, otherwise show server details beforehand.</param>
		// Token: 0x060040A5 RID: 16549 RVA: 0x0014F509 File Offset: 0x0014D709
		public static void connect(SteamConnectionInfo info, bool shouldAutoJoin, MenuPlayServerInfoUI.EServerInfoOpenContext openContext)
		{
			Provider.provider.matchmakingService.connect(info);
			Provider.provider.matchmakingService.autoJoinServerQuery = shouldAutoJoin;
			Provider.provider.matchmakingService.serverQueryContext = openContext;
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x0014F53B File Offset: 0x0014D73B
		public static void open()
		{
			if (MenuPlayConnectUI.active)
			{
				return;
			}
			MenuPlayConnectUI.active = true;
			MenuPlayConnectUI.container.AnimateIntoView();
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0014F555 File Offset: 0x0014D755
		public static void close()
		{
			if (!MenuPlayConnectUI.active)
			{
				return;
			}
			MenuPlayConnectUI.active = false;
			MenuSettings.save();
			MenuPlayConnectUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x0014F580 File Offset: 0x0014D780
		internal static bool TryParseHostString(string input, out IPv4Address address, out CSteamID steamId, out ushort queryPortOverride)
		{
			address = default(IPv4Address);
			steamId = default(CSteamID);
			queryPortOverride = 0;
			if (string.IsNullOrEmpty(input))
			{
				UnturnedLog.info("Unable to parse empty host string");
				return false;
			}
			input = input.Trim();
			if (string.IsNullOrEmpty(input))
			{
				UnturnedLog.info("Unable to parse host string empty after trimming");
				return false;
			}
			bool autoPrefix = input.Contains('/');
			string text;
			if (WebUtils.ParseThirdPartyUrl(input, out text, autoPrefix, false))
			{
				if (!Provider.allowWebRequests)
				{
					UnturnedLog.warn("Unable to request host details because web requests are disabled");
					return false;
				}
				UnturnedLog.info("Requesting host details from " + text + "...");
				using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(text))
				{
					unityWebRequest.timeout = 2;
					unityWebRequest.SendWebRequest();
					while (!unityWebRequest.isDone)
					{
					}
					if (unityWebRequest.result != 1)
					{
						UnturnedLog.warn("Network error requesting host details: \"" + unityWebRequest.error + "\"");
						return false;
					}
					string text2 = unityWebRequest.downloadHandler.text.Trim();
					if (string.IsNullOrEmpty(text2))
					{
						UnturnedLog.info("Unable to parse empty host details response");
						return false;
					}
					int num = text2.IndexOf(':');
					if (num < 0)
					{
						input = text2;
					}
					else
					{
						input = text2.Substring(0, num);
						ushort.TryParse(text2.Substring(num + 1), ref queryPortOverride);
					}
					UnturnedLog.info(string.Format("Received host details ({0}:{1}) from {2}", input, queryPortOverride, text));
				}
			}
			ulong ulSteamID;
			if (input.Length >= 6 && ulong.TryParse(input, ref ulSteamID))
			{
				steamId = new CSteamID(ulSteamID);
				if (steamId.BGameServerAccount())
				{
					return true;
				}
				steamId = CSteamID.Nil;
			}
			if (string.Equals(input, "localhost", 5))
			{
				address = new IPv4Address("127.0.0.1");
				return true;
			}
			if (IPv4Address.TryParse(input, ref address))
			{
				return true;
			}
			string text3;
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(input);
				if (hostAddresses.Length != 0 && hostAddresses[0] != null)
				{
					text3 = hostAddresses[0].ToString();
				}
				else
				{
					text3 = null;
				}
			}
			catch (Exception e)
			{
				text3 = input;
				UnturnedLog.exception(e, "Caught exception while resolving host string \"" + input + "\":");
			}
			if (string.IsNullOrEmpty(text3))
			{
				UnturnedLog.info("Resolved address was empty");
				return false;
			}
			if (!IPv4Address.TryParse(text3, ref address))
			{
				UnturnedLog.info("Unable to parse resolved address \"" + text3 + "\"");
				return false;
			}
			return true;
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x0014F7DC File Offset: 0x0014D9DC
		private static void onClickedConnectButton(ISleekElement button)
		{
			IPv4Address pv4Address;
			CSteamID steamId;
			ushort num;
			if (!MenuPlayConnectUI.TryParseHostString(MenuPlayConnectUI.hostField.Text, out pv4Address, out steamId, out num))
			{
				UnturnedLog.info("Cannot connect because unable to parse host string");
				return;
			}
			if (steamId.BGameServerAccount())
			{
				MenuSettings.save();
				Provider.connect(new ServerConnectParameters(steamId, MenuPlayConnectUI.passwordField.Text), null, null);
				return;
			}
			ushort num2 = (num > 0) ? num : MenuPlayConnectUI.portField.Value;
			if (num2 == 0)
			{
				UnturnedLog.info("Cannot connect because port field is empty");
				return;
			}
			SteamConnectionInfo info = new SteamConnectionInfo(pv4Address.value, num2, MenuPlayConnectUI.passwordField.Text);
			MenuSettings.save();
			MenuPlayConnectUI.connect(info, false, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0014F873 File Offset: 0x0014DA73
		private static void onTypedHostField(ISleekField field, string text)
		{
			PlaySettings.connectHost = text;
			MenuPlayConnectUI.addressInfoBox.IsVisible = false;
			MenuPlayConnectUI.RefreshServerCodeInfo();
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0014F88C File Offset: 0x0014DA8C
		private static void SplitHostIntoAddressAndPort()
		{
			string text = MenuPlayConnectUI.hostField.Text;
			int num = text.LastIndexOf(':');
			if (num < 0)
			{
				return;
			}
			ushort connectPort;
			if (!ushort.TryParse(text.Substring(num + 1), ref connectPort))
			{
				return;
			}
			PlaySettings.connectHost = text.Substring(0, num);
			PlaySettings.connectPort = connectPort;
			MenuPlayConnectUI.hostField.Text = PlaySettings.connectHost;
			MenuPlayConnectUI.portField.Value = PlaySettings.connectPort;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0014F8F6 File Offset: 0x0014DAF6
		private static void OnIpFieldCommitted(ISleekField field)
		{
			MenuPlayConnectUI.SplitHostIntoAddressAndPort();
			MenuPlayConnectUI.RefreshAddressInfo();
			MenuPlayConnectUI.RefreshServerCodeInfo();
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0014F907 File Offset: 0x0014DB07
		private static void onTypedPortField(ISleekUInt16Field field, ushort state)
		{
			PlaySettings.connectPort = state;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0014F90F File Offset: 0x0014DB0F
		private static void onTypedPasswordField(ISleekField field, string text)
		{
			PlaySettings.connectPassword = text;
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x0014F917 File Offset: 0x0014DB17
		private static void onAttemptUpdated(int attempt)
		{
			MenuUI.openAlert(MenuPlayConnectUI.localization.format("Connecting", attempt), true);
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0014F934 File Offset: 0x0014DB34
		private static void onTimedOut()
		{
			if (Provider.connectionFailureInfo != ESteamConnectionFailureInfo.NONE)
			{
				ESteamConnectionFailureInfo connectionFailureInfo = Provider.connectionFailureInfo;
				Provider.resetConnectionFailure();
				if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO_SERVER)
				{
					MenuUI.alert(MenuPlayConnectUI.localization.format("Pro_Server"));
					return;
				}
				if (connectionFailureInfo == ESteamConnectionFailureInfo.PASSWORD)
				{
					MenuUI.alert(MenuPlayConnectUI.localization.format("Password"));
					return;
				}
				if (connectionFailureInfo == ESteamConnectionFailureInfo.FULL)
				{
					MenuUI.alert(MenuPlayConnectUI.localization.format("Full"));
					return;
				}
				if (connectionFailureInfo == ESteamConnectionFailureInfo.TIMED_OUT)
				{
					MenuUI.alert(MenuPlayConnectUI.localization.format("Timed_Out"));
				}
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0014F9B8 File Offset: 0x0014DBB8
		private static void RefreshAddressInfo()
		{
			MenuPlayConnectUI.addressInfoBox.IsVisible = false;
			string text = MenuPlayConnectUI.hostField.Text.ToLower();
			text = text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			ulong num;
			if (text.Length >= 6 && ulong.TryParse(text, ref num))
			{
				return;
			}
			string text2 = null;
			if (text == "localhost")
			{
				text2 = "127.0.0.1";
			}
			else
			{
				try
				{
					IPAddress[] hostAddresses = Dns.GetHostAddresses(text);
					if (hostAddresses.Length != 0 && hostAddresses[0] != null)
					{
						text2 = hostAddresses[0].ToString();
					}
					else
					{
						text2 = null;
					}
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Caught exception while resolving \"" + text + "\" for address info box:");
					text2 = text;
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				return;
			}
			IPv4Address pv4Address;
			if (!IPv4Address.TryParse(text2, ref pv4Address))
			{
				return;
			}
			if (pv4Address.IsLoopback)
			{
				MenuPlayConnectUI.addressInfoBox.Text = MenuPlayConnectUI.localization.format("Address_Loopback_Label");
				MenuPlayConnectUI.addressInfoBox.TooltipText = MenuPlayConnectUI.localization.format("Address_Loopback_Tooltip");
				MenuPlayConnectUI.addressInfoBox.IsVisible = true;
				return;
			}
			if (pv4Address.IsLocalPrivate)
			{
				MenuPlayConnectUI.addressInfoBox.Text = MenuPlayConnectUI.localization.format("Address_LocalPrivate_Label");
				MenuPlayConnectUI.addressInfoBox.TooltipText = MenuPlayConnectUI.localization.format("Address_LocalPrivate_Tooltip");
				MenuPlayConnectUI.addressInfoBox.IsVisible = true;
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0014FB0C File Offset: 0x0014DD0C
		private static void RefreshServerCodeInfo()
		{
			MenuPlayConnectUI.serverCodeInfoBox.IsVisible = false;
			MenuPlayConnectUI.portField.IsVisible = true;
			string text = MenuPlayConnectUI.hostField.Text.ToLower();
			text = text.Trim();
			if (string.IsNullOrEmpty(text) || text.Length < 6)
			{
				return;
			}
			ulong ulSteamID;
			if (!ulong.TryParse(text, ref ulSteamID))
			{
				return;
			}
			CSteamID csteamID = new CSteamID(ulSteamID);
			if (csteamID.BGameServerAccount())
			{
				MenuPlayConnectUI.serverCodeInfoBox.Text = MenuPlayConnectUI.localization.format("ServerCode_Valid_Label");
				MenuPlayConnectUI.serverCodeInfoBox.TooltipText = MenuPlayConnectUI.localization.format("ServerCode_Valid_Tooltip");
				MenuPlayConnectUI.serverCodeIcon.Texture = MenuPlayConnectUI.icons.load<Texture2D>("ValidServerCode");
				MenuPlayConnectUI.serverCodeIcon.TintColor = 2;
			}
			else if (csteamID.BIndividualAccount())
			{
				MenuPlayConnectUI.serverCodeInfoBox.Text = MenuPlayConnectUI.localization.format("ServerCode_Invalid_Label");
				MenuPlayConnectUI.serverCodeInfoBox.TooltipText = MenuPlayConnectUI.localization.format("ServerCode_Friend_Tooltip");
				MenuPlayConnectUI.serverCodeIcon.Texture = MenuPlayConnectUI.icons.load<Texture2D>("InvalidServerCode");
				MenuPlayConnectUI.serverCodeIcon.TintColor = 6;
			}
			else
			{
				MenuPlayConnectUI.serverCodeInfoBox.Text = MenuPlayConnectUI.localization.format("ServerCode_Invalid_Label");
				MenuPlayConnectUI.serverCodeInfoBox.TooltipText = MenuPlayConnectUI.localization.format("ServerCode_Invalid_Tooltip");
				MenuPlayConnectUI.serverCodeIcon.Texture = MenuPlayConnectUI.icons.load<Texture2D>("InvalidServerCode");
				MenuPlayConnectUI.serverCodeIcon.TintColor = 6;
			}
			MenuPlayConnectUI.serverCodeInfoBox.IsVisible = true;
			MenuPlayConnectUI.portField.IsVisible = false;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0014FCAB File Offset: 0x0014DEAB
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuPlayUI.open();
			MenuPlayConnectUI.close();
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0014FCB7 File Offset: 0x0014DEB7
		public void OnDestroy()
		{
			Provider.provider.matchmakingService.onAttemptUpdated -= MenuPlayConnectUI.onAttemptUpdated;
			Provider.provider.matchmakingService.onTimedOut -= MenuPlayConnectUI.onTimedOut;
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0014FCF0 File Offset: 0x0014DEF0
		public MenuPlayConnectUI()
		{
			if (MenuPlayConnectUI.icons != null)
			{
				MenuPlayConnectUI.icons.unload();
			}
			MenuPlayConnectUI.localization = Localization.read("/Menu/Play/MenuPlayConnect.dat");
			MenuPlayConnectUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayConnect/MenuPlayConnect.unity3d");
			MenuPlayConnectUI.container = new SleekFullscreenBox();
			MenuPlayConnectUI.container.PositionOffset_X = 10f;
			MenuPlayConnectUI.container.PositionOffset_Y = 10f;
			MenuPlayConnectUI.container.PositionScale_Y = 1f;
			MenuPlayConnectUI.container.SizeOffset_X = -20f;
			MenuPlayConnectUI.container.SizeOffset_Y = -20f;
			MenuPlayConnectUI.container.SizeScale_X = 1f;
			MenuPlayConnectUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayConnectUI.container);
			MenuPlayConnectUI.active = false;
			MenuPlayConnectUI.hostField = Glazier.Get().CreateStringField();
			MenuPlayConnectUI.hostField.PositionOffset_X = -300f;
			MenuPlayConnectUI.hostField.PositionOffset_Y = -75f;
			MenuPlayConnectUI.hostField.PositionScale_X = 0.5f;
			MenuPlayConnectUI.hostField.PositionScale_Y = 0.5f;
			MenuPlayConnectUI.hostField.SizeOffset_X = 600f;
			MenuPlayConnectUI.hostField.SizeOffset_Y = 30f;
			MenuPlayConnectUI.hostField.MaxLength = 64;
			MenuPlayConnectUI.hostField.AddLabel(MenuPlayConnectUI.localization.format("Host_Field_Label"), 1);
			MenuPlayConnectUI.hostField.TooltipText = MenuPlayConnectUI.localization.format("Host_Field_Tooltip");
			MenuPlayConnectUI.hostField.Text = PlaySettings.connectHost;
			MenuPlayConnectUI.hostField.OnTextChanged += new Typed(MenuPlayConnectUI.onTypedHostField);
			MenuPlayConnectUI.hostField.OnTextSubmitted += new Entered(MenuPlayConnectUI.OnIpFieldCommitted);
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.hostField);
			MenuPlayConnectUI.addressInfoBox = Glazier.Get().CreateBox();
			MenuPlayConnectUI.addressInfoBox.PositionOffset_X = -410f;
			MenuPlayConnectUI.addressInfoBox.PositionOffset_Y = -75f;
			MenuPlayConnectUI.addressInfoBox.PositionScale_X = 0.5f;
			MenuPlayConnectUI.addressInfoBox.PositionScale_Y = 0.5f;
			MenuPlayConnectUI.addressInfoBox.SizeOffset_X = 100f;
			MenuPlayConnectUI.addressInfoBox.SizeOffset_Y = 30f;
			MenuPlayConnectUI.addressInfoBox.IsVisible = false;
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.addressInfoBox);
			MenuPlayConnectUI.serverCodeInfoBox = Glazier.Get().CreateBox();
			MenuPlayConnectUI.serverCodeInfoBox.PositionOffset_X = -300f;
			MenuPlayConnectUI.serverCodeInfoBox.PositionOffset_Y = -35f;
			MenuPlayConnectUI.serverCodeInfoBox.PositionScale_X = 0.5f;
			MenuPlayConnectUI.serverCodeInfoBox.PositionScale_Y = 0.5f;
			MenuPlayConnectUI.serverCodeInfoBox.SizeOffset_X = 600f;
			MenuPlayConnectUI.serverCodeInfoBox.SizeOffset_Y = 30f;
			MenuPlayConnectUI.serverCodeInfoBox.IsVisible = false;
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.serverCodeInfoBox);
			MenuPlayConnectUI.serverCodeIcon = Glazier.Get().CreateImage();
			MenuPlayConnectUI.serverCodeIcon.PositionOffset_X = 5f;
			MenuPlayConnectUI.serverCodeIcon.PositionOffset_Y = 5f;
			MenuPlayConnectUI.serverCodeIcon.SizeOffset_X = 20f;
			MenuPlayConnectUI.serverCodeIcon.SizeOffset_Y = 20f;
			MenuPlayConnectUI.serverCodeInfoBox.AddChild(MenuPlayConnectUI.serverCodeIcon);
			MenuPlayConnectUI.portField = Glazier.Get().CreateUInt16Field();
			MenuPlayConnectUI.portField.PositionOffset_X = -300f;
			MenuPlayConnectUI.portField.PositionOffset_Y = -35f;
			MenuPlayConnectUI.portField.PositionScale_X = 0.5f;
			MenuPlayConnectUI.portField.PositionScale_Y = 0.5f;
			MenuPlayConnectUI.portField.SizeOffset_X = 600f;
			MenuPlayConnectUI.portField.SizeOffset_Y = 30f;
			MenuPlayConnectUI.portField.AddLabel(MenuPlayConnectUI.localization.format("Port_Field_Label"), 1);
			MenuPlayConnectUI.portField.TooltipText = MenuPlayConnectUI.localization.format("Port_Field_Tooltip");
			MenuPlayConnectUI.portField.Value = PlaySettings.connectPort;
			MenuPlayConnectUI.portField.OnValueChanged += new TypedUInt16(MenuPlayConnectUI.onTypedPortField);
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.portField);
			MenuPlayConnectUI.passwordField = Glazier.Get().CreateStringField();
			MenuPlayConnectUI.passwordField.PositionOffset_X = -300f;
			MenuPlayConnectUI.passwordField.PositionOffset_Y = 5f;
			MenuPlayConnectUI.passwordField.PositionScale_X = 0.5f;
			MenuPlayConnectUI.passwordField.PositionScale_Y = 0.5f;
			MenuPlayConnectUI.passwordField.SizeOffset_X = 600f;
			MenuPlayConnectUI.passwordField.SizeOffset_Y = 30f;
			MenuPlayConnectUI.passwordField.AddLabel(MenuPlayConnectUI.localization.format("Password_Field_Label"), 1);
			MenuPlayConnectUI.passwordField.IsPasswordField = true;
			MenuPlayConnectUI.passwordField.MaxLength = 0;
			MenuPlayConnectUI.passwordField.Text = PlaySettings.connectPassword;
			MenuPlayConnectUI.passwordField.OnTextChanged += new Typed(MenuPlayConnectUI.onTypedPasswordField);
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.passwordField);
			MenuPlayConnectUI.connectButton = new SleekButtonIcon(MenuPlayConnectUI.icons.load<Texture2D>("Connect"));
			MenuPlayConnectUI.connectButton.PositionOffset_X = -300f;
			MenuPlayConnectUI.connectButton.PositionOffset_Y = 45f;
			MenuPlayConnectUI.connectButton.PositionScale_X = 0.5f;
			MenuPlayConnectUI.connectButton.PositionScale_Y = 0.5f;
			MenuPlayConnectUI.connectButton.SizeOffset_X = 600f;
			MenuPlayConnectUI.connectButton.SizeOffset_Y = 30f;
			MenuPlayConnectUI.connectButton.text = MenuPlayConnectUI.localization.format("Connect_Button");
			MenuPlayConnectUI.connectButton.tooltip = MenuPlayConnectUI.localization.format("Connect_Button_Tooltip");
			MenuPlayConnectUI.connectButton.iconColor = 2;
			MenuPlayConnectUI.connectButton.onClickedButton += new ClickedButton(MenuPlayConnectUI.onClickedConnectButton);
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.connectButton);
			MenuPlayConnectUI.RefreshAddressInfo();
			MenuPlayConnectUI.RefreshServerCodeInfo();
			Provider.provider.matchmakingService.onAttemptUpdated += MenuPlayConnectUI.onAttemptUpdated;
			Provider.provider.matchmakingService.onTimedOut += MenuPlayConnectUI.onTimedOut;
			if (!MenuPlayConnectUI.isLaunched)
			{
				MenuPlayConnectUI.isLaunched = true;
				uint newIP;
				ushort newPort;
				string newPassword;
				ulong ulSteamID;
				if (CommandLine.TryGetSteamConnect(Environment.CommandLine, out newIP, out newPort, out newPassword))
				{
					SteamConnectionInfo steamConnectionInfo = new SteamConnectionInfo(newIP, newPort, newPassword);
					UnturnedLog.info("Command-line connect IP: {0} Port: {1} Password: '{2}'", new object[]
					{
						Parser.getIPFromUInt32(steamConnectionInfo.ip),
						steamConnectionInfo.port,
						steamConnectionInfo.password
					});
					MenuPlayConnectUI.connect(steamConnectionInfo, false, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
				}
				else if (CommandLine.tryGetLobby(Environment.CommandLine, out ulSteamID))
				{
					UnturnedLog.info("Lobby: " + ulSteamID.ToString());
					Lobbies.joinLobby(new CSteamID(ulSteamID));
				}
			}
			else if (MenuPlayConnectUI.hasPendingServerRelay)
			{
				MenuPlayConnectUI.hasPendingServerRelay = false;
				UnturnedLog.info("Relay connect IP: {0} Port: {1} Code: {2} Password: \"{3}\"", new object[]
				{
					Parser.getIPFromUInt32(MenuPlayConnectUI.serverRelayIP),
					MenuPlayConnectUI.serverRelayPort,
					MenuPlayConnectUI.serverRelayServerCode,
					MenuPlayConnectUI.serverRelayPassword
				});
				bool shouldAutoJoin = !MenuPlayConnectUI.serverRelayWaitOnMenu;
				if (MenuPlayConnectUI.serverRelayServerCode != CSteamID.Nil)
				{
					if (MenuPlayConnectUI.serverRelayServerCode.BGameServerAccount())
					{
						Provider.connect(new ServerConnectParameters(MenuPlayConnectUI.serverRelayServerCode, MenuPlayConnectUI.serverRelayPassword), null, null);
					}
					else
					{
						UnturnedLog.warn(string.Format("Unable to join non-gameserver code ({0})", MenuPlayConnectUI.serverRelayServerCode.GetEAccountType()));
					}
				}
				else
				{
					MenuPlayConnectUI.connect(new SteamConnectionInfo(MenuPlayConnectUI.serverRelayIP, MenuPlayConnectUI.serverRelayPort, MenuPlayConnectUI.serverRelayPassword), shouldAutoJoin, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
				}
			}
			MenuPlayConnectUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuPlayConnectUI.backButton.PositionOffset_Y = -50f;
			MenuPlayConnectUI.backButton.PositionScale_Y = 1f;
			MenuPlayConnectUI.backButton.SizeOffset_X = 200f;
			MenuPlayConnectUI.backButton.SizeOffset_Y = 50f;
			MenuPlayConnectUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayConnectUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuPlayConnectUI.backButton.onClickedButton += new ClickedButton(MenuPlayConnectUI.onClickedBackButton);
			MenuPlayConnectUI.backButton.fontSize = 3;
			MenuPlayConnectUI.backButton.iconColor = 2;
			MenuPlayConnectUI.container.AddChild(MenuPlayConnectUI.backButton);
		}

		// Token: 0x04002983 RID: 10627
		public static Bundle icons;

		// Token: 0x04002984 RID: 10628
		public static Local localization;

		// Token: 0x04002985 RID: 10629
		private static SleekFullscreenBox container;

		// Token: 0x04002986 RID: 10630
		public static bool active;

		/// <summary>
		/// These server relay variables redirect the client to another server when the menu opens
		/// similar to how Steam sets the +connect string on game startup. Allows plugin to redirect
		/// player to another server on the same network.
		/// </summary>
		// Token: 0x04002987 RID: 10631
		public static bool hasPendingServerRelay;

		// Token: 0x04002988 RID: 10632
		public static uint serverRelayIP;

		// Token: 0x04002989 RID: 10633
		public static ushort serverRelayPort;

		// Token: 0x0400298A RID: 10634
		public static CSteamID serverRelayServerCode;

		// Token: 0x0400298B RID: 10635
		public static string serverRelayPassword;

		// Token: 0x0400298C RID: 10636
		public static bool serverRelayWaitOnMenu;

		// Token: 0x0400298D RID: 10637
		private static SleekButtonIcon backButton;

		// Token: 0x0400298E RID: 10638
		private static ISleekField hostField;

		// Token: 0x0400298F RID: 10639
		private static ISleekUInt16Field portField;

		// Token: 0x04002990 RID: 10640
		private static ISleekField passwordField;

		// Token: 0x04002991 RID: 10641
		private static SleekButtonIcon connectButton;

		// Token: 0x04002992 RID: 10642
		private static ISleekBox addressInfoBox;

		// Token: 0x04002993 RID: 10643
		private static ISleekBox serverCodeInfoBox;

		// Token: 0x04002994 RID: 10644
		private static ISleekImage serverCodeIcon;

		// Token: 0x04002995 RID: 10645
		private static bool isLaunched;
	}
}
