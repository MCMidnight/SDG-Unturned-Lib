using System;
using System.Collections.Generic;
using System.Net;
using SDG.NetPak;
using SDG.NetTransport;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000694 RID: 1684
	public class SteamPlayer : SteamConnectedClientBase
	{
		// Token: 0x060038C6 RID: 14534 RVA: 0x0010BE19 File Offset: 0x0010A019
		public NetId GetNetId()
		{
			return this._netId;
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060038C7 RID: 14535 RVA: 0x0010BE21 File Offset: 0x0010A021
		public SteamPlayerID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060038C8 RID: 14536 RVA: 0x0010BE29 File Offset: 0x0010A029
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060038C9 RID: 14537 RVA: 0x0010BE31 File Offset: 0x0010A031
		public Player player
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x0010BE39 File Offset: 0x0010A039
		public bool isPro
		{
			get
			{
				return (!OptionsSettings.streamer || !(this.playerID.steamID != Provider.user)) && this._isPro;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060038CB RID: 14539 RVA: 0x0010BE61 File Offset: 0x0010A061
		public int channel
		{
			get
			{
				return this._channel;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060038CC RID: 14540 RVA: 0x0010BE69 File Offset: 0x0010A069
		// (set) Token: 0x060038CD RID: 14541 RVA: 0x0010BE91 File Offset: 0x0010A091
		public bool isAdmin
		{
			get
			{
				return (!OptionsSettings.streamer || !(this.playerID.steamID != Provider.user)) && this._isAdmin;
			}
			set
			{
				this._isAdmin = value;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060038CE RID: 14542 RVA: 0x0010BE9A File Offset: 0x0010A09A
		public float ping
		{
			get
			{
				return this._ping;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060038CF RID: 14543 RVA: 0x0010BEA2 File Offset: 0x0010A0A2
		public float joined
		{
			get
			{
				return this._joined;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x0010BEAA File Offset: 0x0010A0AA
		public byte hair
		{
			get
			{
				return this._hair;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x0010BEB2 File Offset: 0x0010A0B2
		public byte beard
		{
			get
			{
				return this._beard;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x0010BEBA File Offset: 0x0010A0BA
		public Color skin
		{
			get
			{
				return this._skin;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x060038D3 RID: 14547 RVA: 0x0010BEC2 File Offset: 0x0010A0C2
		public Color color
		{
			get
			{
				return this._color;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060038D4 RID: 14548 RVA: 0x0010BECA File Offset: 0x0010A0CA
		public Color markerColor
		{
			get
			{
				return this._markerColor;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060038D5 RID: 14549 RVA: 0x0010BED2 File Offset: 0x0010A0D2
		public bool IsLeftHanded
		{
			get
			{
				return this._hand;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060038D6 RID: 14550 RVA: 0x0010BEDA File Offset: 0x0010A0DA
		[Obsolete("Renamed to IsLeftHanded")]
		public bool hand
		{
			get
			{
				return this._hand;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060038D7 RID: 14551 RVA: 0x0010BEE2 File Offset: 0x0010A0E2
		public EPlayerSkillset skillset
		{
			get
			{
				return this._skillset;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060038D8 RID: 14552 RVA: 0x0010BEEA File Offset: 0x0010A0EA
		public string language
		{
			get
			{
				return this._language;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x0010BEF2 File Offset: 0x0010A0F2
		// (set) Token: 0x060038DA RID: 14554 RVA: 0x0010BEFA File Offset: 0x0010A0FA
		public CSteamID lobbyID { get; private set; }

		// Token: 0x060038DB RID: 14555 RVA: 0x0010BF03 File Offset: 0x0010A103
		public bool getItemSkinItemDefID(ushort itemID, out int itemdefid)
		{
			itemdefid = 0;
			return this.itemSkins != null && this.itemSkins.TryGetValue(itemID, ref itemdefid);
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x0010BF1F File Offset: 0x0010A11F
		[Obsolete("This will be removed in a future version!")]
		public bool getVehicleSkinItemDefID(ushort vehicleID, out int itemdefid)
		{
			itemdefid = 0;
			return this.vehicleSkins != null && this.vehicleSkins.TryGetValue(vehicleID, ref itemdefid);
		}

		/// <summary>
		/// Get Steam item definition ID equipped for given vehicle.
		/// </summary>
		/// <returns>True if a skin was available.</returns>
		// Token: 0x060038DD RID: 14557 RVA: 0x0010BF3C File Offset: 0x0010A13C
		public bool GetVehicleSkinItemDefId(InteractableVehicle vehicle, out int itemdefid)
		{
			itemdefid = 0;
			if (vehicle == null || this.vehicleGuidToSkinItemDefId == null)
			{
				return false;
			}
			VehicleAsset vehicleAsset = vehicle.asset.FindSharedSkinVehicleAsset();
			return vehicleAsset != null && this.vehicleGuidToSkinItemDefId.TryGetValue(vehicleAsset.GUID, ref itemdefid);
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x0010BF84 File Offset: 0x0010A184
		public bool getTagsAndDynamicPropsForItem(int item, out string tags, out string dynamic_props)
		{
			tags = string.Empty;
			dynamic_props = string.Empty;
			int i = 0;
			while (i < this.skinItems.Length)
			{
				if (this.skinItems[i] == item)
				{
					if (i < this.skinTags.Length && i < this.skinDynamicProps.Length)
					{
						tags = this.skinTags[i];
						dynamic_props = this.skinDynamicProps[i];
						return true;
					}
					return false;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		/// <summary>
		/// Build econ details struct from tags and dynamic_props.
		/// Note that details cannot be modified because it's a struct and has copies of the data.
		/// </summary>
		// Token: 0x060038DF RID: 14559 RVA: 0x0010BFEC File Offset: 0x0010A1EC
		public bool getDynamicEconDetails(ushort itemID, out DynamicEconDetails details)
		{
			int itemdefid;
			if (!this.getItemSkinItemDefID(itemID, out itemdefid))
			{
				details = default(DynamicEconDetails);
				return false;
			}
			return this.getDynamicEconDetailsForItemDef(itemdefid, out details);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0010C018 File Offset: 0x0010A218
		public bool getDynamicEconDetailsForItemDef(int itemdefid, out DynamicEconDetails details)
		{
			string tags;
			string dynamic_props;
			if (!this.getTagsAndDynamicPropsForItem(itemdefid, out tags, out dynamic_props))
			{
				details = default(DynamicEconDetails);
				return false;
			}
			details = new DynamicEconDetails(tags, dynamic_props);
			return true;
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x0010C04C File Offset: 0x0010A24C
		public bool getStatTrackerValue(ushort itemID, out EStatTrackerType type, out int kills)
		{
			DynamicEconDetails dynamicEconDetails;
			if (!this.getDynamicEconDetails(itemID, out dynamicEconDetails))
			{
				type = EStatTrackerType.NONE;
				kills = -1;
				return false;
			}
			return dynamicEconDetails.getStatTrackerValue(out type, out kills);
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x0010C078 File Offset: 0x0010A278
		public bool getRagdollEffect(ushort itemID, out ERagdollEffect effect)
		{
			DynamicEconDetails dynamicEconDetails;
			if (!this.getDynamicEconDetails(itemID, out dynamicEconDetails))
			{
				effect = ERagdollEffect.NONE;
				return false;
			}
			return dynamicEconDetails.getRagdollEffect(out effect);
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x0010C0A0 File Offset: 0x0010A2A0
		public ushort getParticleEffectForItemDef(int itemdefid)
		{
			DynamicEconDetails dynamicEconDetails;
			if (this.getDynamicEconDetailsForItemDef(itemdefid, out dynamicEconDetails))
			{
				return dynamicEconDetails.getParticleEffect();
			}
			return 0;
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0010C0C4 File Offset: 0x0010A2C4
		public void incrementStatTrackerValue(ushort itemID, EPlayerStat stat)
		{
			int num;
			if (!this.getItemSkinItemDefID(itemID, out num))
			{
				return;
			}
			string tags;
			string dynamic_props;
			if (!this.getTagsAndDynamicPropsForItem(num, out tags, out dynamic_props))
			{
				return;
			}
			DynamicEconDetails dynamicEconDetails = new DynamicEconDetails(tags, dynamic_props);
			EStatTrackerType estatTrackerType;
			int num2;
			if (!dynamicEconDetails.getStatTrackerValue(out estatTrackerType, out num2))
			{
				return;
			}
			if (estatTrackerType != EStatTrackerType.TOTAL)
			{
				if (estatTrackerType != EStatTrackerType.PLAYER)
				{
					return;
				}
				if (stat != EPlayerStat.KILLS_PLAYERS)
				{
					return;
				}
			}
			else if (stat != EPlayerStat.KILLS_ANIMALS && stat != EPlayerStat.KILLS_PLAYERS && stat != EPlayerStat.KILLS_ZOMBIES_MEGA && stat != EPlayerStat.KILLS_ZOMBIES_NORMAL)
			{
				return;
			}
			if (!this.modifiedItems.Contains(itemID))
			{
				this.modifiedItems.Add(itemID);
			}
			num2++;
			int i = 0;
			while (i < this.skinItems.Length)
			{
				if (this.skinItems[i] == num)
				{
					if (i < this.skinDynamicProps.Length)
					{
						this.skinDynamicProps[i] = dynamicEconDetails.getPredictedDynamicPropsJsonForStatTracker(estatTrackerType, num2);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x0010C188 File Offset: 0x0010A388
		public void commitModifiedDynamicProps()
		{
			if (this.modifiedItems.Count < 1 || this.submittedModifiedItems)
			{
				return;
			}
			this.submittedModifiedItems = true;
			SteamInventoryUpdateHandle_t handle = SteamInventory.StartUpdateProperties();
			int num = 0;
			foreach (ushort itemID in this.modifiedItems)
			{
				ulong value;
				EStatTrackerType type;
				int num2;
				if (Characters.getPackageForItemID(itemID, out value) && this.getStatTrackerValue(itemID, out type, out num2))
				{
					string statTrackerPropertyName = Provider.provider.economyService.getStatTrackerPropertyName(type);
					if (!string.IsNullOrEmpty(statTrackerPropertyName))
					{
						SteamInventory.SetProperty(handle, new SteamItemInstanceID_t(value), statTrackerPropertyName, (long)num2);
						num++;
					}
				}
			}
			SteamInventory.SubmitUpdateProperties(handle, out Provider.provider.economyService.commitResult);
			UnturnedLog.info(string.Format("Submitted {0} item property update(s)", num));
		}

		/// <summary>
		/// Add a recent ping sample to the average ping window.
		/// Updates ping based on the average of several recent ping samples.
		/// </summary>
		/// <param name="value">Most recent ping value.</param>
		// Token: 0x060038E6 RID: 14566 RVA: 0x0010C270 File Offset: 0x0010A470
		public void lag(float value)
		{
			value = Mathf.Clamp01(value);
			this._ping = value;
			for (int i = this.pings.Length - 1; i > 0; i--)
			{
				this.pings[i] = this.pings[i - 1];
				if (this.pings[i] > 0.001f)
				{
					this._ping += this.pings[i];
				}
			}
			this._ping /= (float)this.pings.Length;
			this.pings[0] = value;
		}

		/// <returns>True if both players exist, are both members of groups, and are both members of the same group.</returns>
		// Token: 0x060038E7 RID: 14567 RVA: 0x0010C2F6 File Offset: 0x0010A4F6
		public bool isMemberOfSameGroupAs(Player other)
		{
			return this.player != null && other != null && this.player.quests.isMemberOfSameGroupAs(other);
		}

		/// <returns>True if both players exist, are both members of groups, and are both members of the same group.</returns>
		// Token: 0x060038E8 RID: 14568 RVA: 0x0010C322 File Offset: 0x0010A522
		public bool isMemberOfSameGroupAs(SteamPlayer other)
		{
			return other != null && this.isMemberOfSameGroupAs(other.player);
		}

		/// <summary>
		/// Get real IPv4 address of remote player NOT the relay server.
		/// </summary>
		/// <returns>True if address was available, and not flagged as a relay server.</returns>
		// Token: 0x060038E9 RID: 14569 RVA: 0x0010C335 File Offset: 0x0010A535
		public bool getIPv4Address(out uint address)
		{
			if (base.transportConnection != null)
			{
				return base.transportConnection.TryGetIPv4Address(out address);
			}
			address = 0U;
			return false;
		}

		/// <summary>
		/// See above, returns zero if failed.
		/// </summary>
		// Token: 0x060038EA RID: 14570 RVA: 0x0010C350 File Offset: 0x0010A550
		public uint getIPv4AddressOrZero()
		{
			uint result;
			this.getIPv4Address(out result);
			return result;
		}

		/// <summary>
		/// Get real address of remote player NOT a relay server.
		/// </summary>
		/// <returns>Null if address was unavailable.</returns>
		// Token: 0x060038EB RID: 14571 RVA: 0x0010C367 File Offset: 0x0010A567
		public IPAddress getAddress()
		{
			if (base.transportConnection != null)
			{
				return base.transportConnection.GetAddress();
			}
			return null;
		}

		/// <summary>
		/// Get string representation of remote end point.
		/// </summary>
		/// <returns>Null if address was unavailable.</returns>
		// Token: 0x060038EC RID: 14572 RVA: 0x0010C37E File Offset: 0x0010A57E
		public string getAddressString(bool withPort)
		{
			if (base.transportConnection != null)
			{
				return base.transportConnection.GetAddressString(withPort);
			}
			return null;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x0010C396 File Offset: 0x0010A596
		public bool Equals(SteamPlayer otherClient)
		{
			return otherClient != null && this.playerID.Equals(otherClient.playerID);
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x0010C3AE File Offset: 0x0010A5AE
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SteamPlayer);
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x0010C3BC File Offset: 0x0010A5BC
		public override int GetHashCode()
		{
			return this.playerID.GetHashCode();
		}

		/// <summary>
		/// Players can set a "nickname" which is only shown to the members in their group.
		/// </summary>
		// Token: 0x060038F0 RID: 14576 RVA: 0x0010C3CC File Offset: 0x0010A5CC
		internal string GetLocalDisplayName()
		{
			if (!string.IsNullOrEmpty(this.playerID.nickName) && this.playerID.steamID != Provider.client && this.player != null && this.player.quests != null && Player.player != null && this.player.quests.isMemberOfSameGroupAs(Player.player))
			{
				return this.playerID.nickName;
			}
			return this.playerID.characterName;
		}

		/// <summary>
		/// Can be used by plugins to verify player is on a particular server.
		///
		/// OnSteamAuthTicketForWebApiReceived will be invoked when the response is received.
		/// Note that the client doesn't send anything if the request to Steam fails, so plugins may wish to kick
		/// players if a certain amount of time passes. (e.g., if a cheat is canceling the request)
		/// </summary>
		// Token: 0x060038F1 RID: 14577 RVA: 0x0010C460 File Offset: 0x0010A660
		public void RequestSteamAuthTicketForWebApi(string identity)
		{
			if (string.IsNullOrWhiteSpace(identity))
			{
				throw new ArgumentException("cannot be null or empty", "identity");
			}
			if (!this.requestedSteamAuthTicketIdentities.Add(identity))
			{
				return;
			}
			UnturnedLog.info(string.Format("Sending request to {0} for Steam auth ticket for web API identity \"{1}\"", base.transportConnection, identity));
			SteamPlayer.SendGetSteamAuthTicketForWebApiRequest.Invoke(ENetReliability.Reliable, base.transportConnection, identity);
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0010C4BC File Offset: 0x0010A6BC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveGetSteamAuthTicketForWebApiRequest(string identity)
		{
			UnturnedLog.info("Received request to get Steam auth ticket for web API identity \"" + identity + "\"");
			Provider.RequestSteamAuthTicketForWebApi(identity);
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x0010C4DC File Offset: 0x0010A6DC
		[SteamCall(ESteamCallValidation.SERVERSIDE)]
		public static void ReceiveGetSteamAuthTicketForWebApiResponse(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			SteamPlayer callingPlayer = context.GetCallingPlayer();
			string text;
			if (!SystemNetPakReaderEx.ReadString(reader, ref text, 5))
			{
				context.Kick("Unable to read Steam auth ticket web API identity");
				return;
			}
			if (!callingPlayer.requestedSteamAuthTicketIdentities.Contains(text))
			{
				context.Kick("Server did not request Steam auth ticket for provided web API identity");
				return;
			}
			if (!callingPlayer.receivedSteamAuthTicketIdentities.Add(text))
			{
				context.Kick("Client sent duplicate Steam auth ticket for web API response");
				return;
			}
			ushort num;
			if (!SystemNetPakReaderEx.ReadUInt16(reader, ref num))
			{
				context.Kick("Unable to read Steam web API auth ticket length");
				return;
			}
			if (num > 2560)
			{
				context.Kick("Steam web API auth ticket longer than maximum");
				return;
			}
			byte[] array = new byte[(int)num];
			if (!reader.ReadBytes(array))
			{
				context.Kick("Unable to read Steam web API auth ticket contents");
				return;
			}
			UnturnedLog.info(string.Format("Received response from {0} for Steam auth ticket for web API identity \"{1}\" length: {2}", callingPlayer.transportConnection, text, num));
			Action<SteamPlayer, string, byte[]> onSteamAuthTicketForWebApiReceived = SteamPlayer.OnSteamAuthTicketForWebApiReceived;
			if (onSteamAuthTicketForWebApiReceived == null)
			{
				return;
			}
			onSteamAuthTicketForWebApiReceived.TryInvoke("OnSteamAuthTicketForWebApiReceived", callingPlayer, text, array);
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0010C5C0 File Offset: 0x0010A7C0
		public SteamPlayer(ITransportConnection transportConnection, NetId netId, SteamPlayerID newPlayerID, Transform newModel, bool newPro, bool newAdmin, int newChannel, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, Color newMarkerColor, bool newHand, int newShirtItem, int newPantsItem, int newHatItem, int newBackpackItem, int newVestItem, int newMaskItem, int newGlassesItem, int[] newSkinItems, string[] newSkinTags, string[] newSkinDynamicProps, EPlayerSkillset newSkillset, string newLanguage, CSteamID newLobbyID, EClientPlatform clientPlatform)
		{
			base.transportConnection = transportConnection;
			this._netId = netId;
			NetIdRegistry.Assign(this._netId, this);
			this._playerID = newPlayerID;
			this._model = newModel;
			this.model.name = this.playerID.characterName + " [" + this.playerID.playerName + "]";
			this.model.GetComponent<SteamChannel>().id = newChannel;
			this.model.GetComponent<SteamChannel>().owner = this;
			this.model.GetComponent<SteamChannel>().setup();
			this._player = this.model.GetComponent<Player>();
			this._player.AssignNetIdBlock(this._netId);
			this._isPro = newPro;
			this._channel = newChannel;
			this.isAdmin = newAdmin;
			this.face = newFace;
			this._hair = newHair;
			this._beard = newBeard;
			this._skin = newSkin;
			this._color = newColor;
			this._markerColor = newMarkerColor;
			this._hand = newHand;
			this._skillset = newSkillset;
			this._language = newLanguage;
			this.shirtItem = newShirtItem;
			this.pantsItem = newPantsItem;
			this.hatItem = newHatItem;
			this.backpackItem = newBackpackItem;
			this.vestItem = newVestItem;
			this.maskItem = newMaskItem;
			this.glassesItem = newGlassesItem;
			this.skinItems = newSkinItems;
			this.skinTags = newSkinTags;
			this.skinDynamicProps = newSkinDynamicProps;
			this.itemSkins = new Dictionary<ushort, int>();
			this.vehicleSkins = new Dictionary<ushort, int>();
			this.vehicleGuidToSkinItemDefId = new Dictionary<Guid, int>();
			this.modifiedItems = new HashSet<ushort>();
			for (int i = 0; i < this.skinItems.Length; i++)
			{
				int num = this.skinItems[i];
				if (num != 0)
				{
					Guid guid;
					Guid guid2;
					Provider.provider.economyService.getInventoryTargetID(num, out guid, out guid2);
					if (guid != default(Guid))
					{
						ItemAsset itemAsset = Assets.find<ItemAsset>(guid);
						if (itemAsset != null && !this.itemSkins.ContainsKey(itemAsset.id))
						{
							this.itemSkins.Add(itemAsset.id, num);
						}
					}
					else if (guid2 != default(Guid))
					{
						VehicleAsset vehicleAsset = VehicleTool.FindVehicleByGuidAndHandleRedirects(guid2);
						if (vehicleAsset != null)
						{
							if (!this.vehicleSkins.ContainsKey(vehicleAsset.id))
							{
								this.vehicleSkins.Add(vehicleAsset.id, num);
							}
							this.vehicleGuidToSkinItemDefId[vehicleAsset.GUID] = num;
						}
					}
				}
			}
			this.pings = new float[4];
			this.timeLastPacketWasReceivedFromClient = Time.realtimeSinceStartup;
			this.lastChat = Time.realtimeSinceStartup;
			this.nextVote = Time.realtimeSinceStartup;
			this.lastReceivedPingRequestRealtime = Time.realtimeSinceStartup;
			this._joined = Time.realtimeSinceStartup;
			this.lobbyID = newLobbyID;
			this.clientPlatform = clientPlatform;
		}

		// Token: 0x040021B9 RID: 8633
		public static Action<SteamPlayer, string, byte[]> OnSteamAuthTicketForWebApiReceived;

		// Token: 0x040021BA RID: 8634
		private NetId _netId;

		// Token: 0x040021BB RID: 8635
		private SteamPlayerID _playerID;

		// Token: 0x040021BC RID: 8636
		private Transform _model;

		// Token: 0x040021BD RID: 8637
		private Player _player;

		// Token: 0x040021BE RID: 8638
		private bool _isPro;

		// Token: 0x040021BF RID: 8639
		private int _channel;

		/// <summary>
		/// Not an actual Steam ID or BattlEye ID, instead this is used to map player references to and from BE.
		/// </summary>
		// Token: 0x040021C0 RID: 8640
		internal int battlEyeId;

		// Token: 0x040021C1 RID: 8641
		private bool _isAdmin;

		// Token: 0x040021C2 RID: 8642
		private float[] pings;

		// Token: 0x040021C3 RID: 8643
		private float _ping;

		// Token: 0x040021C4 RID: 8644
		private float _joined;

		// Token: 0x040021C5 RID: 8645
		public byte face;

		// Token: 0x040021C6 RID: 8646
		private byte _hair;

		// Token: 0x040021C7 RID: 8647
		private byte _beard;

		// Token: 0x040021C8 RID: 8648
		private Color _skin;

		// Token: 0x040021C9 RID: 8649
		private Color _color;

		// Token: 0x040021CA RID: 8650
		private Color _markerColor;

		// Token: 0x040021CB RID: 8651
		private bool _hand;

		// Token: 0x040021CC RID: 8652
		public int shirtItem;

		// Token: 0x040021CD RID: 8653
		public int pantsItem;

		// Token: 0x040021CE RID: 8654
		public int hatItem;

		// Token: 0x040021CF RID: 8655
		public int backpackItem;

		// Token: 0x040021D0 RID: 8656
		public int vestItem;

		// Token: 0x040021D1 RID: 8657
		public int maskItem;

		// Token: 0x040021D2 RID: 8658
		public int glassesItem;

		// Token: 0x040021D3 RID: 8659
		public int[] skinItems;

		// Token: 0x040021D4 RID: 8660
		public string[] skinTags;

		// Token: 0x040021D5 RID: 8661
		public string[] skinDynamicProps;

		// Token: 0x040021D6 RID: 8662
		public Dictionary<ushort, int> itemSkins;

		// Token: 0x040021D7 RID: 8663
		[Obsolete("This will be removed in a future version!")]
		public Dictionary<ushort, int> vehicleSkins;

		// Token: 0x040021D8 RID: 8664
		private Dictionary<Guid, int> vehicleGuidToSkinItemDefId;

		// Token: 0x040021D9 RID: 8665
		public HashSet<ushort> modifiedItems;

		// Token: 0x040021DA RID: 8666
		private bool submittedModifiedItems;

		// Token: 0x040021DB RID: 8667
		private EPlayerSkillset _skillset;

		// Token: 0x040021DC RID: 8668
		private string _language;

		// Token: 0x040021DE RID: 8670
		public float timeLastPacketWasReceivedFromClient;

		// Token: 0x040021DF RID: 8671
		public float timeLastPingRequestWasSentToClient;

		// Token: 0x040021E0 RID: 8672
		public float lastChat;

		// Token: 0x040021E1 RID: 8673
		public float nextVote;

		// Token: 0x040021E2 RID: 8674
		public bool isVoiceChatLocallyMuted;

		// Token: 0x040021E3 RID: 8675
		public bool isTextChatLocallyMuted;

		// Token: 0x040021E4 RID: 8676
		[Obsolete("This field should not have been externally used and will be removed in a future version.")]
		public float rpcCredits;

		// Token: 0x040021E5 RID: 8677
		public float lastReceivedPingRequestRealtime;

		/// <summary>
		/// Next time method is allowed to be called.
		/// </summary>
		// Token: 0x040021E6 RID: 8678
		public float[] rpcAllowedTimes = new float[NetReflection.rateLimitedMethodsCount];

		/// <summary>
		/// Number of times client has tried to invoke this method while rate-limited.
		/// </summary>
		// Token: 0x040021E7 RID: 8679
		internal int[] rpcHitCount = new int[NetReflection.rateLimitedMethodsCount];

		// Token: 0x040021E8 RID: 8680
		internal EClientPlatform clientPlatform;

		// Token: 0x040021E9 RID: 8681
		private static readonly ClientStaticMethod<string> SendGetSteamAuthTicketForWebApiRequest = ClientStaticMethod<string>.Get(new ClientStaticMethod<string>.ReceiveDelegate(SteamPlayer.ReceiveGetSteamAuthTicketForWebApiRequest));

		// Token: 0x040021EA RID: 8682
		internal static readonly ServerStaticMethod SendGetSteamAuthTicketForWebApiResponse = ServerStaticMethod.Get(new ServerStaticMethod.ReceiveDelegateWithContext(SteamPlayer.ReceiveGetSteamAuthTicketForWebApiResponse));

		// Token: 0x040021EB RID: 8683
		internal HashSet<Guid> validatedGuids = new HashSet<Guid>();

		// Token: 0x040021EC RID: 8684
		private HashSet<string> requestedSteamAuthTicketIdentities = new HashSet<string>();

		// Token: 0x040021ED RID: 8685
		private HashSet<string> receivedSteamAuthTicketIdentities = new HashSet<string>();
	}
}
