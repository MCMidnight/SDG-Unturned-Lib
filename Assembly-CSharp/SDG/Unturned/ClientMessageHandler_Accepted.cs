using System;
using System.IO;
using System.Runtime.InteropServices;
using BattlEye;
using SDG.NetPak;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000266 RID: 614
	internal static class ClientMessageHandler_Accepted
	{
		// Token: 0x06001262 RID: 4706 RVA: 0x0003EF58 File Offset: 0x0003D158
		internal static void ReadMessage(NetPakReader reader)
		{
			uint num;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num);
			ushort num2;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num2);
			UnturnedLog.info("Accepted by server");
			if (Provider.IsBattlEyeActiveOnCurrentServer)
			{
				string text = ReadWrite.PATH + "/BattlEye/BEClient_x64.dll";
				if (!File.Exists(text))
				{
					text = ReadWrite.PATH + "/BattlEye/BEClient.dll";
				}
				if (!File.Exists(text))
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
					Provider._connectionFailureReason = "Missing BattlEye client library! (" + text + ")";
					UnturnedLog.error(Provider.connectionFailureReason);
					Provider.RequestDisconnect("BattlEye missing");
					return;
				}
				UnturnedLog.info("Loading BattlEye client library from: " + text);
				try
				{
					Provider.battlEyeClientHandle = BEClient.LoadLibraryW(text);
					if (!(Provider.battlEyeClientHandle != IntPtr.Zero))
					{
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
						Provider._connectionFailureReason = "Failed to load BattlEye client library!";
						UnturnedLog.error(Provider.connectionFailureReason);
						Provider.RequestDisconnect("BattlEye load error");
						return;
					}
					BEClient.BEClientInitFn beclientInitFn = Marshal.GetDelegateForFunctionPointer(BEClient.GetProcAddress(Provider.battlEyeClientHandle, "Init"), typeof(BEClient.BEClientInitFn)) as BEClient.BEClientInitFn;
					if (beclientInitFn == null)
					{
						BEClient.FreeLibrary(Provider.battlEyeClientHandle);
						Provider.battlEyeClientHandle = IntPtr.Zero;
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
						Provider._connectionFailureReason = "Failed to get BattlEye client init delegate!";
						UnturnedLog.error(Provider.connectionFailureReason);
						Provider.RequestDisconnect("BattlEye get init error");
						return;
					}
					uint ulAddress;
					ushort usPort;
					if (SteamNetworkingUtils.IsFakeIPv4(num))
					{
						ulAddress = 0U;
						usPort = 0;
					}
					else
					{
						ulAddress = ((num & 255U) << 24 | (num & 65280U) << 8 | (num & 16711680U) >> 8 | (num & 4278190080U) >> 24);
						usPort = (ushort)((int)(num2 & 255) << 8 | (int)((uint)(num2 & 65280) >> 8));
					}
					Provider.battlEyeClientInitData = new BEClient.BECL_GAME_DATA();
					Provider.battlEyeClientInitData.pstrGameVersion = Provider.APP_NAME + " " + Provider.APP_VERSION;
					Provider.battlEyeClientInitData.ulAddress = ulAddress;
					Provider.battlEyeClientInitData.usPort = usPort;
					Provider.battlEyeClientInitData.pfnPrintMessage = new BEClient.BECL_GAME_DATA.PrintMessageFn(Provider.battlEyeClientPrintMessage);
					Provider.battlEyeClientInitData.pfnRequestRestart = new BEClient.BECL_GAME_DATA.RequestRestartFn(Provider.battlEyeClientRequestRestart);
					Provider.battlEyeClientInitData.pfnSendPacket = new BEClient.BECL_GAME_DATA.SendPacketFn(Provider.battlEyeClientSendPacket);
					Provider.battlEyeClientRunData = new BEClient.BECL_BE_DATA();
					if (!beclientInitFn.Invoke(2, Provider.battlEyeClientInitData, Provider.battlEyeClientRunData))
					{
						BEClient.FreeLibrary(Provider.battlEyeClientHandle);
						Provider.battlEyeClientHandle = IntPtr.Zero;
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
						Provider._connectionFailureReason = "Failed to call BattlEye client init!";
						UnturnedLog.error(Provider.connectionFailureReason);
						Provider.RequestDisconnect("BattlEye init error");
						return;
					}
				}
				catch (Exception e)
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
					Provider._connectionFailureReason = "Unhandled exception when loading BattlEye client library!";
					UnturnedLog.error(Provider.connectionFailureReason);
					UnturnedLog.exception(e);
					Provider.RequestDisconnect("BattlEye load exception");
					return;
				}
			}
			Provider._modeConfigData = new ModeConfigData(Provider.mode);
			byte repair_Level_Max;
			SystemNetPakReaderEx.ReadUInt8(reader, ref repair_Level_Max);
			Provider._modeConfigData.Gameplay.Repair_Level_Max = (uint)repair_Level_Max;
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Hitmarkers);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Crosshair);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Ballistics);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Chart);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Satellite);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Compass);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Group_Map);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Group_HUD);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Group_Player_List);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Allow_Static_Groups);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Allow_Dynamic_Groups);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Allow_Shoulder_Camera);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Can_Suicide);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Friendly_Fire);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Bypass_Buildable_Mobility);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Allow_Freeform_Buildables);
			reader.ReadBit(ref Provider._modeConfigData.Gameplay.Allow_Freeform_Buildables_On_Vehicles);
			ushort a;
			SystemNetPakReaderEx.ReadUInt16(reader, ref a);
			Provider._modeConfigData.Gameplay.Timer_Exit = MathfEx.Min((uint)a, 60U);
			ushort timer_Respawn;
			SystemNetPakReaderEx.ReadUInt16(reader, ref timer_Respawn);
			Provider._modeConfigData.Gameplay.Timer_Respawn = (uint)timer_Respawn;
			ushort timer_Home;
			SystemNetPakReaderEx.ReadUInt16(reader, ref timer_Home);
			Provider._modeConfigData.Gameplay.Timer_Home = (uint)timer_Home;
			ushort max_Group_Members;
			SystemNetPakReaderEx.ReadUInt16(reader, ref max_Group_Members);
			Provider._modeConfigData.Gameplay.Max_Group_Members = (uint)max_Group_Members;
			reader.ReadBit(ref Provider._modeConfigData.Barricades.Allow_Item_Placement_On_Vehicle);
			reader.ReadBit(ref Provider._modeConfigData.Barricades.Allow_Trap_Placement_On_Vehicle);
			SystemNetPakReaderEx.ReadFloat(reader, ref Provider._modeConfigData.Barricades.Max_Item_Distance_From_Hull);
			SystemNetPakReaderEx.ReadFloat(reader, ref Provider._modeConfigData.Barricades.Max_Trap_Distance_From_Hull);
			SystemNetPakReaderEx.ReadFloat(reader, ref Provider._modeConfigData.Gameplay.AirStrafing_Acceleration_Multiplier);
			SystemNetPakReaderEx.ReadFloat(reader, ref Provider._modeConfigData.Gameplay.AirStrafing_Deceleration_Multiplier);
			SystemNetPakReaderEx.ReadFloat(reader, ref Provider._modeConfigData.Gameplay.ThirdPerson_RecoilMultiplier);
			SystemNetPakReaderEx.ReadFloat(reader, ref Provider._modeConfigData.Gameplay.ThirdPerson_SpreadMultiplier);
			if (OptionsSettings.streamer)
			{
				SteamFriends.SetRichPresence("connect", "");
			}
			else
			{
				SteamUser.AdvertiseGame(Provider.server, 0U, 0);
				SteamFriends.SetRichPresence("connect", "+connect " + num.ToString() + ":" + num2.ToString());
			}
			Lobbies.leaveLobby();
			SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, num, num2 + 1, num2, Provider.STEAM_FAVORITE_FLAG_HISTORY, SteamUtils.GetServerRealTime());
			Provider.updateRichPresence();
			Provider.ClientConnected onClientConnected = Provider.onClientConnected;
			if (onClientConnected != null)
			{
				onClientConnected();
			}
			Action onGameplayConfigReceived = ClientMessageHandler_Accepted.OnGameplayConfigReceived;
			if (onGameplayConfigReceived == null)
			{
				return;
			}
			onGameplayConfigReceived.Invoke();
		}

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06001263 RID: 4707 RVA: 0x0003F580 File Offset: 0x0003D780
		// (remove) Token: 0x06001264 RID: 4708 RVA: 0x0003F5B4 File Offset: 0x0003D7B4
		internal static event Action OnGameplayConfigReceived;
	}
}
