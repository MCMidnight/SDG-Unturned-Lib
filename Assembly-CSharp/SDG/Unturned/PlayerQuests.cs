using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000655 RID: 1621
	public class PlayerQuests : PlayerCaller
	{
		/// <summary>
		/// For level objects with QuestCondition called when quests are added or removed.
		/// </summary>
		// Token: 0x140000BF RID: 191
		// (add) Token: 0x060034EA RID: 13546 RVA: 0x000F437C File Offset: 0x000F257C
		// (remove) Token: 0x060034EB RID: 13547 RVA: 0x000F43B4 File Offset: 0x000F25B4
		internal event Action<ushort> OnLocalPlayerQuestsChanged;

		/// <summary>
		/// Event specifically for plugins to listen to global quest progress.
		/// </summary>
		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x060034EC RID: 13548 RVA: 0x000F43EC File Offset: 0x000F25EC
		// (remove) Token: 0x060034ED RID: 13549 RVA: 0x000F4420 File Offset: 0x000F2620
		public static event PlayerQuests.AnyFlagChangedHandler onAnyFlagChanged;

		/// <summary>
		/// Event for plugins when group or rank changes.
		/// </summary>
		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x060034EE RID: 13550 RVA: 0x000F4454 File Offset: 0x000F2654
		// (remove) Token: 0x060034EF RID: 13551 RVA: 0x000F4488 File Offset: 0x000F2688
		public static event PlayerQuests.GroupChangedCallback onGroupChanged;

		// Token: 0x060034F0 RID: 13552 RVA: 0x000F44BC File Offset: 0x000F26BC
		private static void broadcastGroupChanged(PlayerQuests sender, CSteamID oldGroupID, EPlayerGroupRank oldGroupRank, CSteamID newGroupID, EPlayerGroupRank newGroupRank)
		{
			try
			{
				PlayerQuests.GroupChangedCallback groupChangedCallback = PlayerQuests.onGroupChanged;
				if (groupChangedCallback != null)
				{
					groupChangedCallback(sender, oldGroupID, oldGroupRank, newGroupID, newGroupRank);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onGroupChanged:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000F4504 File Offset: 0x000F2704
		private static void triggerGroupUpdated(PlayerQuests sender)
		{
			GroupUpdatedHandler groupUpdatedHandler = PlayerQuests.groupUpdated;
			if (groupUpdatedHandler == null)
			{
				return;
			}
			groupUpdatedHandler(sender);
		}

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x060034F2 RID: 13554 RVA: 0x000F4518 File Offset: 0x000F2718
		// (remove) Token: 0x060034F3 RID: 13555 RVA: 0x000F4550 File Offset: 0x000F2750
		public event TrackedQuestUpdated TrackedQuestUpdated;

		// Token: 0x060034F4 RID: 13556 RVA: 0x000F4588 File Offset: 0x000F2788
		private void TriggerTrackedQuestUpdated()
		{
			if (this.TrackedQuestUpdated == null)
			{
				return;
			}
			try
			{
				this.TrackedQuestUpdated(this);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception during TriggerTrackedQuestUpdated:");
			}
		}

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x060034F5 RID: 13557 RVA: 0x000F45CC File Offset: 0x000F27CC
		// (remove) Token: 0x060034F6 RID: 13558 RVA: 0x000F4604 File Offset: 0x000F2804
		public event GroupIDChangedHandler groupIDChanged;

		// Token: 0x060034F7 RID: 13559 RVA: 0x000F4639 File Offset: 0x000F2839
		private void triggerGroupIDChanged(CSteamID oldGroupID, CSteamID newGroupID)
		{
			GroupIDChangedHandler groupIDChangedHandler = this.groupIDChanged;
			if (groupIDChangedHandler == null)
			{
				return;
			}
			groupIDChangedHandler(this, oldGroupID, newGroupID);
		}

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x060034F8 RID: 13560 RVA: 0x000F4650 File Offset: 0x000F2850
		// (remove) Token: 0x060034F9 RID: 13561 RVA: 0x000F4688 File Offset: 0x000F2888
		public event GroupRankChangedHandler groupRankChanged;

		// Token: 0x060034FA RID: 13562 RVA: 0x000F46BD File Offset: 0x000F28BD
		private void triggerGroupRankChanged(EPlayerGroupRank oldGroupRank, EPlayerGroupRank newGroupRank)
		{
			GroupRankChangedHandler groupRankChangedHandler = this.groupRankChanged;
			if (groupRankChangedHandler == null)
			{
				return;
			}
			groupRankChangedHandler(this, oldGroupRank, newGroupRank);
		}

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x060034FB RID: 13563 RVA: 0x000F46D4 File Offset: 0x000F28D4
		// (remove) Token: 0x060034FC RID: 13564 RVA: 0x000F470C File Offset: 0x000F290C
		public event GroupInvitesChangedHandler groupInvitesChanged;

		// Token: 0x060034FD RID: 13565 RVA: 0x000F4741 File Offset: 0x000F2941
		private void triggerGroupInvitesChanged()
		{
			GroupInvitesChangedHandler groupInvitesChangedHandler = this.groupInvitesChanged;
			if (groupInvitesChangedHandler == null)
			{
				return;
			}
			groupInvitesChangedHandler(this);
		}

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x060034FE RID: 13566 RVA: 0x000F4754 File Offset: 0x000F2954
		// (remove) Token: 0x060034FF RID: 13567 RVA: 0x000F478C File Offset: 0x000F298C
		public event QuestCompletedHandler questCompleted;

		// Token: 0x06003500 RID: 13568 RVA: 0x000F47C1 File Offset: 0x000F29C1
		private void triggerQuestCompleted(QuestAsset asset)
		{
			QuestCompletedHandler questCompletedHandler = this.questCompleted;
			if (questCompletedHandler == null)
			{
				return;
			}
			questCompletedHandler(this, asset);
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06003501 RID: 13569 RVA: 0x000F47D5 File Offset: 0x000F29D5
		// (set) Token: 0x06003502 RID: 13570 RVA: 0x000F47DD File Offset: 0x000F29DD
		public List<PlayerQuestFlag> flagsList { get; private set; }

		// Token: 0x06003503 RID: 13571 RVA: 0x000F47E6 File Offset: 0x000F29E6
		public QuestAsset GetTrackedQuest()
		{
			return this._trackedQuest;
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06003504 RID: 13572 RVA: 0x000F47EE File Offset: 0x000F29EE
		[Obsolete("Replaced by GetTrackedQuest")]
		public ushort TrackedQuestID
		{
			get
			{
				QuestAsset trackedQuest = this._trackedQuest;
				if (trackedQuest == null)
				{
					return 0;
				}
				return trackedQuest.id;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003505 RID: 13573 RVA: 0x000F4801 File Offset: 0x000F2A01
		// (set) Token: 0x06003506 RID: 13574 RVA: 0x000F4809 File Offset: 0x000F2A09
		public List<PlayerQuest> questsList { get; private set; }

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003507 RID: 13575 RVA: 0x000F4812 File Offset: 0x000F2A12
		// (set) Token: 0x06003508 RID: 13576 RVA: 0x000F481A File Offset: 0x000F2A1A
		public bool isMarkerPlaced
		{
			get
			{
				return this._isMarkerPlaced;
			}
			private set
			{
				this._isMarkerPlaced = value;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003509 RID: 13577 RVA: 0x000F4823 File Offset: 0x000F2A23
		// (set) Token: 0x0600350A RID: 13578 RVA: 0x000F482B File Offset: 0x000F2A2B
		public Vector3 markerPosition
		{
			get
			{
				return this._markerPosition;
			}
			private set
			{
				this._markerPosition = value;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x000F4834 File Offset: 0x000F2A34
		// (set) Token: 0x0600350C RID: 13580 RVA: 0x000F483C File Offset: 0x000F2A3C
		public string markerTextOverride
		{
			get
			{
				return this._markerTextOverride;
			}
			private set
			{
				this._markerTextOverride = value;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600350D RID: 13581 RVA: 0x000F4845 File Offset: 0x000F2A45
		// (set) Token: 0x0600350E RID: 13582 RVA: 0x000F484D File Offset: 0x000F2A4D
		public uint radioFrequency
		{
			get
			{
				return this._radioFrequency;
			}
			private set
			{
				this._radioFrequency = value;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x000F4856 File Offset: 0x000F2A56
		// (set) Token: 0x06003510 RID: 13584 RVA: 0x000F485E File Offset: 0x000F2A5E
		public CSteamID groupID
		{
			get
			{
				return this._groupID;
			}
			private set
			{
				this._groupID = value;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000F4867 File Offset: 0x000F2A67
		// (set) Token: 0x06003512 RID: 13586 RVA: 0x000F486F File Offset: 0x000F2A6F
		public EPlayerGroupRank groupRank
		{
			get
			{
				return this._groupRank;
			}
			private set
			{
				this._groupRank = value;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x000F4878 File Offset: 0x000F2A78
		// (set) Token: 0x06003514 RID: 13588 RVA: 0x000F4880 File Offset: 0x000F2A80
		public HashSet<CSteamID> groupInvites { get; private set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000F4889 File Offset: 0x000F2A89
		public bool useMaxGroupMembersLimit
		{
			get
			{
				return Provider.modeConfigData.Gameplay.Max_Group_Members > 0U;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003516 RID: 13590 RVA: 0x000F48A0 File Offset: 0x000F2AA0
		public bool hasSpaceForMoreMembersInGroup
		{
			get
			{
				if (this.useMaxGroupMembersLimit)
				{
					GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
					return groupInfo != null && groupInfo.hasSpaceForMoreMembersInGroup;
				}
				return true;
			}
		}

		/// <summary>
		/// Check before allowing changes to this player's <see cref="P:SDG.Unturned.PlayerQuests.groupID" />
		/// </summary>
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x000F48CE File Offset: 0x000F2ACE
		public bool canChangeGroupMembership
		{
			get
			{
				return !LevelManager.isPlayerInArena(base.player);
			}
		}

		/// <summary>
		/// Can rename the group.
		/// </summary>
		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06003518 RID: 13592 RVA: 0x000F48DE File Offset: 0x000F2ADE
		public bool hasPermissionToChangeName
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		/// <summary>
		/// Can promote and demote members.
		/// </summary>
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000F48E9 File Offset: 0x000F2AE9
		public bool hasPermissionToChangeRank
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x000F48F4 File Offset: 0x000F2AF4
		public bool hasPermissionToInviteMembers
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.ADMIN || this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x000F490A File Offset: 0x000F2B0A
		public bool hasPermissionToKickMembers
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.ADMIN || this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x000F4920 File Offset: 0x000F2B20
		public bool hasPermissionToCreateGroup
		{
			get
			{
				return Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600351D RID: 13597 RVA: 0x000F4934 File Offset: 0x000F2B34
		public bool hasPermissionToLeaveGroup
		{
			get
			{
				if (!Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups)
				{
					return false;
				}
				if (this.groupRank == EPlayerGroupRank.OWNER)
				{
					GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
					if (groupInfo != null && groupInfo.members > 1U)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600351E RID: 13598 RVA: 0x000F4978 File Offset: 0x000F2B78
		public bool hasPermissionToDeleteGroup
		{
			get
			{
				return Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups && !this.inMainGroup && this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x0600351F RID: 13599 RVA: 0x000F49A0 File Offset: 0x000F2BA0
		public bool canBeKickedFromGroup
		{
			get
			{
				return this.groupRank != EPlayerGroupRank.OWNER;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003520 RID: 13600 RVA: 0x000F49AE File Offset: 0x000F2BAE
		public bool isMemberOfAGroup
		{
			get
			{
				return this.groupID != CSteamID.Nil;
			}
		}

		/// <summary>
		/// If true, hide viewmodel and prevent using equipped item. For example, to prevent shooting gun on top of a
		/// first-person scene. This could be expanded in the future with other flags and options.
		/// </summary>
		// Token: 0x06003521 RID: 13601 RVA: 0x000F49C0 File Offset: 0x000F2BC0
		public bool IsCutsceneModeActive()
		{
			return this.npcCutsceneMode;
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000F49C8 File Offset: 0x000F2BC8
		public void ServerSetCutsceneModeActive(bool active)
		{
			if (this.npcCutsceneMode != active)
			{
				this.npcCutsceneMode = active;
				if (base.channel.IsLocalPlayer)
				{
					base.player.animator.NotifyLocalPlayerCutsceneModeActiveChanged(this.npcCutsceneMode);
				}
				PlayerQuests.SendCutsceneMode.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), active);
			}
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000F4A25 File Offset: 0x000F2C25
		public bool isMemberOfGroup(CSteamID groupID)
		{
			return this.isMemberOfAGroup && this.groupID == groupID;
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000F4A3D File Offset: 0x000F2C3D
		public bool isMemberOfSameGroupAs(Player player)
		{
			return player.quests.isMemberOfGroup(this.groupID);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000F4A50 File Offset: 0x000F2C50
		[Obsolete]
		public void tellSetMarker(CSteamID steamID, bool newIsMarkerPlaced, Vector3 newMarkerPosition, string newMarkerTextOverride)
		{
			this.ReceiveMarkerState(newIsMarkerPlaced, newMarkerPosition, newMarkerTextOverride);
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000F4A5C File Offset: 0x000F2C5C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveCutsceneMode(bool newCutsceneMode)
		{
			this.npcCutsceneMode = newCutsceneMode;
			if (base.channel.IsLocalPlayer)
			{
				base.player.animator.NotifyLocalPlayerCutsceneModeActiveChanged(this.npcCutsceneMode);
			}
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000F4A88 File Offset: 0x000F2C88
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSetMarker")]
		public void ReceiveMarkerState(bool newIsMarkerPlaced, Vector3 newMarkerPosition, string newMarkerTextOverride)
		{
			this.isMarkerPlaced = newIsMarkerPlaced;
			this.markerPosition = newMarkerPosition;
			this.markerTextOverride = newMarkerTextOverride;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x000F4A9F File Offset: 0x000F2C9F
		[Obsolete]
		public void askSetMarker(CSteamID steamID, bool newIsMarkerPlaced, Vector3 newMarkerPosition)
		{
			this.ReceiveSetMarkerRequest(newIsMarkerPlaced, newMarkerPosition);
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x000F4AA9 File Offset: 0x000F2CA9
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 5, legacyName = "askSetMarker")]
		public void ReceiveSetMarkerRequest(bool newIsMarkerPlaced, Vector3 newMarkerPosition)
		{
			this.replicateSetMarker(newIsMarkerPlaced, newMarkerPosition, string.Empty);
		}

		/// <summary>
		/// Called serverside to set marker on clients.
		/// </summary>
		// Token: 0x0600352A RID: 13610 RVA: 0x000F4AB8 File Offset: 0x000F2CB8
		public void replicateSetMarker(bool newIsMarkerPlaced, Vector3 newMarkerPosition, string newMarkerTextOverride = "")
		{
			PlayerQuests.SendMarkerState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), newIsMarkerPlaced, newMarkerPosition, newMarkerTextOverride);
		}

		/// <summary>
		/// Ask server to set marker.
		/// </summary>
		// Token: 0x0600352B RID: 13611 RVA: 0x000F4AD3 File Offset: 0x000F2CD3
		public void sendSetMarker(bool newIsMarkerPlaced, Vector3 newMarkerPosition)
		{
			PlayerQuests.SendSetMarkerRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, newIsMarkerPlaced, newMarkerPosition);
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x000F4AE8 File Offset: 0x000F2CE8
		[Obsolete]
		public void tellSetRadioFrequency(CSteamID steamID, uint newRadioFrequency)
		{
			this.ReceiveRadioFrequencyState(newRadioFrequency);
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000F4AF1 File Offset: 0x000F2CF1
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSetRadioFrequency")]
		public void ReceiveRadioFrequencyState(uint newRadioFrequency)
		{
			this.radioFrequency = newRadioFrequency;
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000F4AFA File Offset: 0x000F2CFA
		[Obsolete]
		public void askSetRadioFrequency(CSteamID steamID, uint newRadioFrequency)
		{
			this.ReceiveSetRadioFrequencyRequest(newRadioFrequency);
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000F4B03 File Offset: 0x000F2D03
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 5, legacyName = "askSetRadioFrequency")]
		public void ReceiveSetRadioFrequencyRequest(uint newRadioFrequency)
		{
			PlayerQuests.SendRadioFrequencyState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), newRadioFrequency);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000F4B1C File Offset: 0x000F2D1C
		public void sendSetRadioFrequency(uint newRadioFrequency)
		{
			PlayerQuests.SendSetRadioFrequencyRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, newRadioFrequency);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000F4B30 File Offset: 0x000F2D30
		[Obsolete]
		public void tellSetGroup(CSteamID steamID, CSteamID newGroupID, byte newGroupRank)
		{
			this.ReceiveGroupState(newGroupID, (EPlayerGroupRank)newGroupRank);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000F4B3C File Offset: 0x000F2D3C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSetGroup")]
		public void ReceiveGroupState(CSteamID newGroupID, EPlayerGroupRank newGroupRank)
		{
			CSteamID groupID = this.groupID;
			this.groupID = newGroupID;
			EPlayerGroupRank groupRank = this.groupRank;
			this.groupRank = newGroupRank;
			if (groupID != newGroupID)
			{
				this.triggerGroupIDChanged(groupID, newGroupID);
			}
			if (groupRank != this.groupRank)
			{
				this.triggerGroupRankChanged(groupRank, this.groupRank);
			}
			PlayerQuests.triggerGroupUpdated(this);
			PlayerQuests.broadcastGroupChanged(this, groupID, groupRank, newGroupID, this.groupRank);
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x000F4BA1 File Offset: 0x000F2DA1
		private bool removeGroupInvite(CSteamID newGroupID)
		{
			if (this.groupInvites.Remove(newGroupID))
			{
				this.triggerGroupInvitesChanged();
				PlayerQuests.triggerGroupUpdated(this);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Call serverside to replicate new rank to clients
		/// </summary>
		// Token: 0x06003534 RID: 13620 RVA: 0x000F4BC0 File Offset: 0x000F2DC0
		public void changeRank(EPlayerGroupRank newRank)
		{
			PlayerQuests.SendGroupState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.groupID, newRank);
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000F4BDF File Offset: 0x000F2DDF
		[Obsolete]
		public void askJoinGroupInvite(CSteamID steamID, CSteamID newGroupID)
		{
			this.ReceiveAcceptGroupInvitationRequest(newGroupID);
		}

		/// <summary>
		/// Set player's group to their Steam group (if any) without testing restrictions.
		/// </summary>
		// Token: 0x06003536 RID: 13622 RVA: 0x000F4BE8 File Offset: 0x000F2DE8
		public void ServerAssignToMainGroup()
		{
			CSteamID group = base.channel.owner.playerID.group;
			this.inMainGroup = (group != CSteamID.Nil);
			EPlayerGroupRank arg = EPlayerGroupRank.MEMBER;
			PlayerQuests.SendGroupState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), group, arg);
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000F4C38 File Offset: 0x000F2E38
		public bool ServerAssignToGroup(CSteamID newGroupID, EPlayerGroupRank newRank, bool bypassMemberLimit)
		{
			GroupInfo groupInfo = GroupManager.getGroupInfo(newGroupID);
			if (groupInfo != null && (bypassMemberLimit || groupInfo.hasSpaceForMoreMembersInGroup))
			{
				PlayerQuests.SendGroupState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), newGroupID, newRank);
				this.inMainGroup = false;
				groupInfo.members += 1U;
				GroupManager.sendGroupInfo(groupInfo);
				return true;
			}
			return false;
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000F4C90 File Offset: 0x000F2E90
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askJoinGroupInvite")]
		public void ReceiveAcceptGroupInvitationRequest(CSteamID newGroupID)
		{
			if (!this.canChangeGroupMembership)
			{
				return;
			}
			if (newGroupID == base.channel.owner.playerID.group)
			{
				if (!Provider.modeConfigData.Gameplay.Allow_Static_Groups)
				{
					return;
				}
				this.ServerAssignToMainGroup();
				return;
			}
			else
			{
				if (!this.ServerRemoveGroupInvitation(newGroupID))
				{
					return;
				}
				this.ServerAssignToGroup(newGroupID, EPlayerGroupRank.MEMBER, false);
				return;
			}
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000F4CF0 File Offset: 0x000F2EF0
		public void SendAcceptGroupInvitation(CSteamID newGroupID)
		{
			PlayerQuests.SendAcceptGroupInvitationRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, newGroupID);
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000F4D04 File Offset: 0x000F2F04
		[Obsolete]
		public void askIgnoreGroupInvite(CSteamID steamID, CSteamID newGroupID)
		{
			this.ReceiveDeclineGroupInvitationRequest(newGroupID);
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000F4D0D File Offset: 0x000F2F0D
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askJoinGroupInvite")]
		public void ReceiveDeclineGroupInvitationRequest(CSteamID newGroupID)
		{
			this.ServerRemoveGroupInvitation(newGroupID);
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000F4D17 File Offset: 0x000F2F17
		public void SendDeclineGroupInvitation(CSteamID newGroupID)
		{
			PlayerQuests.SendDeclineGroupInvitationRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, newGroupID);
		}

		/// <param name="force">Ignores group changing rules when true.</param>
		// Token: 0x0600353D RID: 13629 RVA: 0x000F4D2C File Offset: 0x000F2F2C
		public void leaveGroup(bool force = false)
		{
			if (!force)
			{
				if (!this.canChangeGroupMembership)
				{
					return;
				}
				if (!this.hasPermissionToLeaveGroup)
				{
					return;
				}
			}
			GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
			if (groupInfo != null)
			{
				if (groupInfo.members > 0U)
				{
					groupInfo.members -= 1U;
				}
				GroupManager.sendGroupInfo(groupInfo);
			}
			PlayerQuests.SendGroupState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), CSteamID.Nil, EPlayerGroupRank.MEMBER);
			this.inMainGroup = false;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000F4D9D File Offset: 0x000F2F9D
		[Obsolete]
		public void askLeaveGroup(CSteamID steamID)
		{
			this.ReceiveLeaveGroupRequest();
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000F4DA5 File Offset: 0x000F2FA5
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askLeaveGroup")]
		public void ReceiveLeaveGroupRequest()
		{
			if (Time.realtimeSinceStartup - this.lastLeaveGroupRequestRealtime < 5f)
			{
				return;
			}
			this.lastLeaveGroupRequestRealtime = Time.realtimeSinceStartup;
			GroupManager.requestGroupExit(base.player);
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000F4DD1 File Offset: 0x000F2FD1
		public void sendLeaveGroup()
		{
			PlayerQuests.SendLeaveGroupRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000F4DE4 File Offset: 0x000F2FE4
		public void deleteGroup()
		{
			if (!this.canChangeGroupMembership)
			{
				return;
			}
			if (!this.hasPermissionToDeleteGroup)
			{
				return;
			}
			GroupManager.deleteGroup(this.groupID);
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000F4E03 File Offset: 0x000F3003
		[Obsolete]
		public void askDeleteGroup(CSteamID steamID)
		{
			this.ReceiveDeleteGroupRequest();
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000F4E0B File Offset: 0x000F300B
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askDeleteGroup")]
		public void ReceiveDeleteGroupRequest()
		{
			this.deleteGroup();
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000F4E13 File Offset: 0x000F3013
		public void sendDeleteGroup()
		{
			PlayerQuests.SendDeleteGroupRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000F4E26 File Offset: 0x000F3026
		[Obsolete]
		public void askCreateGroup(CSteamID steamID)
		{
			this.ReceiveCreateGroupRequest();
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000F4E30 File Offset: 0x000F3030
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askCreateGroup")]
		public void ReceiveCreateGroupRequest()
		{
			if (!this.canChangeGroupMembership)
			{
				return;
			}
			if (!this.hasPermissionToCreateGroup)
			{
				return;
			}
			CSteamID csteamID = GroupManager.generateUniqueGroupID();
			GroupInfo groupInfo = GroupManager.addGroup(csteamID, base.channel.owner.playerID.playerName + "'s Group");
			groupInfo.members += 1U;
			GroupManager.sendGroupInfo(base.channel.GetOwnerTransportConnection(), groupInfo);
			PlayerQuests.SendGroupState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), csteamID, EPlayerGroupRank.OWNER);
			this.inMainGroup = false;
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000F4EB9 File Offset: 0x000F30B9
		public void sendCreateGroup()
		{
			PlayerQuests.SendCreateGroupRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000F4ECC File Offset: 0x000F30CC
		private void addGroupInvite(CSteamID newGroupID)
		{
			this.groupInvites.Add(newGroupID);
			this.triggerGroupInvitesChanged();
			PlayerQuests.triggerGroupUpdated(this);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000F4EE7 File Offset: 0x000F30E7
		[Obsolete]
		public void tellAddGroupInvite(CSteamID steamID, CSteamID newGroupID)
		{
			this.ReceiveAddGroupInviteClient(newGroupID);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000F4EF0 File Offset: 0x000F30F0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellAddGroupInvite")]
		public void ReceiveAddGroupInviteClient(CSteamID newGroupID)
		{
			this.addGroupInvite(newGroupID);
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x000F4EF9 File Offset: 0x000F30F9
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveRemoveGroupInviteClient(CSteamID newGroupID)
		{
			this.removeGroupInvite(newGroupID);
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000F4F03 File Offset: 0x000F3103
		public bool ServerRemoveGroupInvitation(CSteamID groupId)
		{
			if (!this.removeGroupInvite(groupId))
			{
				return false;
			}
			if (!base.channel.IsLocalPlayer)
			{
				PlayerQuests.SendRemoveGroupInviteClient.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), groupId);
			}
			return true;
		}

		/// <summary>
		/// Serverside send packet telling player about this invite
		/// </summary>
		// Token: 0x0600354D RID: 13645 RVA: 0x000F4F3C File Offset: 0x000F313C
		public void sendAddGroupInvite(CSteamID newGroupID)
		{
			if (this.groupInvites.Contains(newGroupID))
			{
				return;
			}
			this.addGroupInvite(newGroupID);
			GroupInfo groupInfo = GroupManager.getGroupInfo(newGroupID);
			if (groupInfo != null)
			{
				GroupManager.sendGroupInfo(base.channel.GetOwnerTransportConnection(), groupInfo);
			}
			if (!base.channel.IsLocalPlayer)
			{
				PlayerQuests.SendAddGroupInviteClient.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), newGroupID);
			}
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000F4FA4 File Offset: 0x000F31A4
		[Obsolete]
		public void askAddGroupInvite(CSteamID steamID, CSteamID targetID)
		{
			this.ReceiveAddGroupInviteRequest(targetID);
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000F4FB0 File Offset: 0x000F31B0
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askAddGroupInvite")]
		public void ReceiveAddGroupInviteRequest(CSteamID targetID)
		{
			if (!this.isMemberOfAGroup)
			{
				return;
			}
			if (!this.hasPermissionToInviteMembers)
			{
				return;
			}
			if (!this.hasSpaceForMoreMembersInGroup)
			{
				return;
			}
			Player player = PlayerTool.getPlayer(targetID);
			if (player == null)
			{
				return;
			}
			if (player.quests.isMemberOfAGroup)
			{
				return;
			}
			player.quests.sendAddGroupInvite(this.groupID);
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000F5008 File Offset: 0x000F3208
		public void sendAskAddGroupInvite(CSteamID targetID)
		{
			PlayerQuests.SendAddGroupInviteRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, targetID);
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x000F501C File Offset: 0x000F321C
		[Obsolete]
		public void askPromote(CSteamID steamID, CSteamID targetID)
		{
			this.ReceivePromoteRequest(targetID);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000F5028 File Offset: 0x000F3228
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askPromote")]
		public void ReceivePromoteRequest(CSteamID targetID)
		{
			if (!this.isMemberOfAGroup)
			{
				return;
			}
			if (!this.hasPermissionToChangeRank)
			{
				return;
			}
			Player player = PlayerTool.getPlayer(targetID);
			if (player == null)
			{
				return;
			}
			if (!player.quests.isMemberOfSameGroupAs(base.player))
			{
				return;
			}
			if (player.quests.groupRank == EPlayerGroupRank.OWNER)
			{
				CommandWindow.LogWarning("Request to promote owner of group?");
				return;
			}
			player.quests.changeRank(player.quests.groupRank + 1);
			if (player.quests.groupRank == EPlayerGroupRank.OWNER)
			{
				this.changeRank(EPlayerGroupRank.ADMIN);
			}
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x000F50B2 File Offset: 0x000F32B2
		public void sendPromote(CSteamID targetID)
		{
			PlayerQuests.SendPromoteRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, targetID);
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x000F50C6 File Offset: 0x000F32C6
		[Obsolete]
		public void askDemote(CSteamID steamID, CSteamID targetID)
		{
			this.ReceiveDemoteRequest(targetID);
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x000F50D0 File Offset: 0x000F32D0
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askDemote")]
		public void ReceiveDemoteRequest(CSteamID targetID)
		{
			if (!this.isMemberOfAGroup)
			{
				return;
			}
			if (!this.hasPermissionToChangeRank)
			{
				return;
			}
			Player player = PlayerTool.getPlayer(targetID);
			if (player == null)
			{
				return;
			}
			if (!player.quests.isMemberOfSameGroupAs(base.player))
			{
				return;
			}
			if (player.quests.groupRank != EPlayerGroupRank.ADMIN)
			{
				CommandWindow.LogWarning("Request to demote non-admin member of group?");
				return;
			}
			player.quests.changeRank(player.quests.groupRank - 1);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000F5145 File Offset: 0x000F3345
		public void sendDemote(CSteamID targetID)
		{
			PlayerQuests.SendDemoteRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, targetID);
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000F5159 File Offset: 0x000F3359
		[Obsolete]
		public void askKickFromGroup(CSteamID steamID, CSteamID targetID)
		{
			this.ReceiveKickFromGroup(targetID);
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000F5164 File Offset: 0x000F3364
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askKickFromGroup")]
		public void ReceiveKickFromGroup(CSteamID targetID)
		{
			if (!this.isMemberOfAGroup)
			{
				return;
			}
			if (!this.hasPermissionToKickMembers)
			{
				return;
			}
			Player player = PlayerTool.getPlayer(targetID);
			if (player == null)
			{
				return;
			}
			if (!player.quests.isMemberOfSameGroupAs(base.player))
			{
				return;
			}
			if (!player.quests.canBeKickedFromGroup)
			{
				return;
			}
			player.quests.leaveGroup(false);
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000F51C2 File Offset: 0x000F33C2
		public void sendKickFromGroup(CSteamID targetID)
		{
			PlayerQuests.SendKickFromGroup.Invoke(base.GetNetId(), ENetReliability.Unreliable, targetID);
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000F51D6 File Offset: 0x000F33D6
		[Obsolete]
		public void askRenameGroup(CSteamID steamID, string newName)
		{
			this.ReceiveRenameGroupRequest(newName);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000F51DF File Offset: 0x000F33DF
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askRenameGroup")]
		public void ReceiveRenameGroupRequest(string newName)
		{
			if (newName.Length > 32)
			{
				newName = newName.Substring(0, 32);
			}
			if (!this.isMemberOfAGroup)
			{
				return;
			}
			if (!this.hasPermissionToChangeName)
			{
				return;
			}
			GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
			groupInfo.name = newName;
			GroupManager.sendGroupInfo(groupInfo);
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000F521F File Offset: 0x000F341F
		public void sendRenameGroup(string newName)
		{
			PlayerQuests.SendRenameGroupRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, newName);
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000F5234 File Offset: 0x000F3434
		public void setFlag(ushort id, short value)
		{
			PlayerQuestFlag playerQuestFlag;
			if (this.flagsMap.TryGetValue(id, ref playerQuestFlag))
			{
				playerQuestFlag.value = value;
			}
			else
			{
				playerQuestFlag = new PlayerQuestFlag(id, value);
				this.flagsMap.Add(id, playerQuestFlag);
				int num = this.flagsList.BinarySearch(playerQuestFlag, PlayerQuests.flagComparator);
				num = ~num;
				this.flagsList.Insert(num, playerQuestFlag);
			}
			if (base.channel.IsLocalPlayer)
			{
				if (id == 29)
				{
					bool flag;
					if (value >= 1 && Provider.provider.achievementsService.getAchievement("Ensign", out flag) && !flag)
					{
						Provider.provider.achievementsService.setAchievement("Ensign");
					}
					bool flag2;
					if (value >= 2 && Provider.provider.achievementsService.getAchievement("Lieutenant", out flag2) && !flag2)
					{
						Provider.provider.achievementsService.setAchievement("Lieutenant");
					}
					bool flag3;
					if (value >= 3 && Provider.provider.achievementsService.getAchievement("Major", out flag3) && !flag3)
					{
						Provider.provider.achievementsService.setAchievement("Major");
					}
				}
				FlagUpdated flagUpdated = this.onFlagUpdated;
				if (flagUpdated != null)
				{
					flagUpdated(id);
				}
				this.TriggerTrackedQuestUpdated();
			}
			if (Provider.isServer && PlayerQuests.onAnyFlagChanged != null)
			{
				PlayerQuests.onAnyFlagChanged(this, playerQuestFlag);
			}
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000F5378 File Offset: 0x000F3578
		public bool getFlag(ushort id, out short value)
		{
			PlayerQuestFlag playerQuestFlag;
			if (this.flagsMap.TryGetValue(id, ref playerQuestFlag))
			{
				value = playerQuestFlag.value;
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000F53A4 File Offset: 0x000F35A4
		public void removeFlag(ushort id)
		{
			PlayerQuestFlag playerQuestFlag;
			if (this.flagsMap.TryGetValue(id, ref playerQuestFlag))
			{
				int num = this.flagsList.BinarySearch(playerQuestFlag, PlayerQuests.flagComparator);
				if (num >= 0)
				{
					this.flagsMap.Remove(id);
					this.flagsList.RemoveAt(num);
					if (base.channel.IsLocalPlayer)
					{
						FlagUpdated flagUpdated = this.onFlagUpdated;
						if (flagUpdated != null)
						{
							flagUpdated(id);
						}
						this.TriggerTrackedQuestUpdated();
					}
				}
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000F5418 File Offset: 0x000F3618
		public int countValidQuests()
		{
			int num = 0;
			foreach (PlayerQuest playerQuest in this.questsList)
			{
				if (playerQuest != null && playerQuest.asset != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000F5478 File Offset: 0x000F3678
		public void AddQuest(QuestAsset questAsset)
		{
			if (questAsset == null)
			{
				return;
			}
			if (this.FindIndexOfQuest(questAsset) < 0)
			{
				PlayerQuest playerQuest = new PlayerQuest(questAsset);
				this.questsList.Add(playerQuest);
			}
			this.TrackQuest(questAsset);
			if (base.channel.IsLocalPlayer && this.OnLocalPlayerQuestsChanged != null)
			{
				this.OnLocalPlayerQuestsChanged.Invoke(questAsset.id);
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000F54D4 File Offset: 0x000F36D4
		[Obsolete]
		public void addQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				this.AddQuest(questAsset);
			}
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000F54FC File Offset: 0x000F36FC
		[Obsolete]
		public bool getQuest(ushort id, out PlayerQuest quest)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset == null)
			{
				quest = null;
				return false;
			}
			int num = this.FindIndexOfQuest(questAsset);
			if (num >= 0)
			{
				quest = this.questsList[num];
				return true;
			}
			quest = null;
			return false;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000F5540 File Offset: 0x000F3740
		public ENPCQuestStatus GetQuestStatus(QuestAsset questAsset)
		{
			if (questAsset == null)
			{
				return ENPCQuestStatus.NONE;
			}
			if (this.FindIndexOfQuest(questAsset) >= 0)
			{
				if (questAsset.areConditionsMet(base.player))
				{
					return ENPCQuestStatus.READY;
				}
				return ENPCQuestStatus.ACTIVE;
			}
			else
			{
				short num;
				if (this.getFlag(questAsset.id, out num))
				{
					return ENPCQuestStatus.COMPLETED;
				}
				return ENPCQuestStatus.NONE;
			}
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000F5584 File Offset: 0x000F3784
		[Obsolete]
		public ENPCQuestStatus getQuestStatus(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				return this.GetQuestStatus(questAsset);
			}
			return ENPCQuestStatus.NONE;
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000F55AC File Offset: 0x000F37AC
		public void RemoveQuest(QuestAsset questAsset, bool wasCompleted = false)
		{
			int num = this.FindIndexOfQuest(questAsset);
			if (num >= 0)
			{
				this.questsList.RemoveAt(num);
			}
			if (this._trackedQuest != null && this._trackedQuest == questAsset)
			{
				if (this.questsList.Count > 0)
				{
					this.TrackQuest(this.questsList[0].asset);
				}
				else
				{
					this.TrackQuest(null);
				}
			}
			if (base.channel.IsLocalPlayer && questAsset != null)
			{
				bool flag;
				if (wasCompleted && Provider.provider.achievementsService.getAchievement("Quest", out flag) && !flag)
				{
					Provider.provider.achievementsService.setAchievement("Quest");
				}
				Action<ushort> onLocalPlayerQuestsChanged = this.OnLocalPlayerQuestsChanged;
				if (onLocalPlayerQuestsChanged != null)
				{
					onLocalPlayerQuestsChanged.Invoke(questAsset.id);
				}
			}
			if (questAsset != null && wasCompleted)
			{
				this.triggerQuestCompleted(questAsset);
			}
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000F567C File Offset: 0x000F387C
		[Obsolete]
		public void removeQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				this.RemoveQuest(questAsset, false);
			}
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000F56A4 File Offset: 0x000F38A4
		public void trackHordeKill()
		{
			for (int i = 0; i < this.questsList.Count; i++)
			{
				PlayerQuest playerQuest = this.questsList[i];
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					for (int j = 0; j < playerQuest.asset.conditions.Length; j++)
					{
						NPCHordeKillsCondition npchordeKillsCondition = playerQuest.asset.conditions[j] as NPCHordeKillsCondition;
						if (npchordeKillsCondition != null && npchordeKillsCondition.nav == base.player.movement.nav)
						{
							short num;
							this.getFlag(npchordeKillsCondition.id, out num);
							num += 1;
							this.sendSetFlag(npchordeKillsCondition.id, num);
						}
					}
				}
			}
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000F575C File Offset: 0x000F395C
		public void trackZombieKill(Zombie zombie)
		{
			if (zombie == null)
			{
				return;
			}
			float sqrMagnitude = (base.transform.position - zombie.transform.position).sqrMagnitude;
			for (int i = 0; i < this.questsList.Count; i++)
			{
				PlayerQuest playerQuest = this.questsList[i];
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					for (int j = 0; j < playerQuest.asset.conditions.Length; j++)
					{
						NPCZombieKillsCondition npczombieKillsCondition = playerQuest.asset.conditions[j] as NPCZombieKillsCondition;
						if (npczombieKillsCondition != null && (npczombieKillsCondition.zombie == EZombieSpeciality.NONE || npczombieKillsCondition.zombie == zombie.speciality) && (npczombieKillsCondition.nav == 255 || npczombieKillsCondition.nav == base.player.movement.bound) && (npczombieKillsCondition.sqrRadius <= 0.01f || sqrMagnitude <= npczombieKillsCondition.sqrRadius) && (npczombieKillsCondition.sqrMinRadius <= 0.01f || sqrMagnitude >= npczombieKillsCondition.sqrMinRadius))
						{
							short num;
							this.getFlag(npczombieKillsCondition.id, out num);
							num += 1;
							this.sendSetFlag(npczombieKillsCondition.id, num);
						}
					}
				}
			}
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000F58B4 File Offset: 0x000F3AB4
		public void trackObjectKill(Guid objectGuid, byte nav)
		{
			foreach (PlayerQuest playerQuest in this.questsList)
			{
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					INPCCondition[] conditions = playerQuest.asset.conditions;
					for (int i = 0; i < conditions.Length; i++)
					{
						NPCObjectKillsCondition npcobjectKillsCondition = conditions[i] as NPCObjectKillsCondition;
						if (npcobjectKillsCondition != null && (npcobjectKillsCondition.nav == 255 || npcobjectKillsCondition.nav == nav) && npcobjectKillsCondition.objectGuid.Equals(objectGuid))
						{
							short num;
							this.getFlag(npcobjectKillsCondition.id, out num);
							num += 1;
							this.sendSetFlag(npcobjectKillsCondition.id, num);
						}
					}
				}
			}
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000F59A0 File Offset: 0x000F3BA0
		public void trackTreeKill(Guid treeGuid)
		{
			foreach (PlayerQuest playerQuest in this.questsList)
			{
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					INPCCondition[] conditions = playerQuest.asset.conditions;
					for (int i = 0; i < conditions.Length; i++)
					{
						NPCTreeKillsCondition npctreeKillsCondition = conditions[i] as NPCTreeKillsCondition;
						if (npctreeKillsCondition != null && npctreeKillsCondition.treeGuid.Equals(treeGuid))
						{
							short num;
							this.getFlag(npctreeKillsCondition.id, out num);
							num += 1;
							this.sendSetFlag(npctreeKillsCondition.id, num);
						}
					}
				}
			}
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000F5A6C File Offset: 0x000F3C6C
		public void trackAnimalKill(Animal animal)
		{
			if (animal == null)
			{
				return;
			}
			for (int i = 0; i < this.questsList.Count; i++)
			{
				PlayerQuest playerQuest = this.questsList[i];
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					for (int j = 0; j < playerQuest.asset.conditions.Length; j++)
					{
						NPCAnimalKillsCondition npcanimalKillsCondition = playerQuest.asset.conditions[j] as NPCAnimalKillsCondition;
						if (npcanimalKillsCondition != null && npcanimalKillsCondition.animal == animal.id)
						{
							short num;
							this.getFlag(npcanimalKillsCondition.id, out num);
							num += 1;
							this.sendSetFlag(npcanimalKillsCondition.id, num);
						}
					}
				}
			}
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000F5B24 File Offset: 0x000F3D24
		public void trackPlayerKill(Player enemyPlayer)
		{
			if (enemyPlayer == null)
			{
				return;
			}
			for (int i = 0; i < this.questsList.Count; i++)
			{
				PlayerQuest playerQuest = this.questsList[i];
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					for (int j = 0; j < playerQuest.asset.conditions.Length; j++)
					{
						NPCPlayerKillsCondition npcplayerKillsCondition = playerQuest.asset.conditions[j] as NPCPlayerKillsCondition;
						if (npcplayerKillsCondition != null)
						{
							short num;
							this.getFlag(npcplayerKillsCondition.id, out num);
							num += 1;
							this.sendSetFlag(npcplayerKillsCondition.id, num);
						}
					}
				}
			}
		}

		/// <summary>
		/// Called on server to finalize and remove quest.
		/// </summary>
		// Token: 0x0600356E RID: 13678 RVA: 0x000F5BCC File Offset: 0x000F3DCC
		public void CompleteQuest(QuestAsset questAsset, bool ignoreNPC = false)
		{
			if (questAsset == null)
			{
				return;
			}
			if (!ignoreNPC)
			{
				if (this.checkNPC == null)
				{
					return;
				}
				if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
				{
					return;
				}
			}
			if (this.GetQuestStatus(questAsset) != ENPCQuestStatus.READY)
			{
				return;
			}
			this.ServerRemoveQuest(questAsset, true);
			this.sendSetFlag(questAsset.id, 1);
			questAsset.ApplyConditions(base.player);
			questAsset.GrantRewards(base.player);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000F5C58 File Offset: 0x000F3E58
		[Obsolete]
		public void completeQuest(ushort id, bool ignoreNPC = false)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				this.CompleteQuest(questAsset, false);
			}
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000F5C7E File Offset: 0x000F3E7E
		[Obsolete]
		public void askSellToVendor(CSteamID steamID, ushort id, byte index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000F5C88 File Offset: 0x000F3E88
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askSellToVendor")]
		public void ReceiveSellToVendor(in ServerInvocationContext context, Guid assetGuid, byte index, bool asManyAsPossible)
		{
			if (this.checkNPC == null)
			{
				return;
			}
			if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (this.serverCurrentVendorAsset == null)
			{
				return;
			}
			VendorAsset vendorAsset = Assets.find<VendorAsset>(assetGuid);
			if (vendorAsset == null)
			{
				return;
			}
			if (vendorAsset != this.serverCurrentVendorAsset)
			{
				return;
			}
			if (vendorAsset.buying == null)
			{
				return;
			}
			if ((int)index >= vendorAsset.buying.Length)
			{
				return;
			}
			VendorBuying vendorBuying = vendorAsset.buying[(int)index];
			if (vendorBuying == null)
			{
				return;
			}
			int num = 0;
			while (vendorBuying.canSell(base.player) && vendorBuying.areConditionsMet(base.player))
			{
				vendorBuying.ApplyConditions(base.player);
				vendorBuying.GrantRewards(base.player);
				vendorBuying.sell(base.player);
				num++;
				if (!asManyAsPossible || num >= 10)
				{
					return;
				}
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000F5D64 File Offset: 0x000F3F64
		public void sendSellToVendor(Guid assetGuid, byte index, bool asManyAsPossible)
		{
			PlayerQuests.SendSellToVendor.Invoke(base.GetNetId(), ENetReliability.Unreliable, assetGuid, index, asManyAsPossible);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000F5D7A File Offset: 0x000F3F7A
		[Obsolete]
		public void sendSellToVendor(ushort id, byte index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000F5D81 File Offset: 0x000F3F81
		[Obsolete]
		public void askBuyFromVendor(CSteamID steamID, ushort id, byte index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000F5D88 File Offset: 0x000F3F88
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askBuyFromVendor")]
		public void ReceiveBuyFromVendor(in ServerInvocationContext context, Guid assetGuid, byte index, bool asManyAsPossible)
		{
			if (this.checkNPC == null)
			{
				return;
			}
			if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (this.serverCurrentVendorAsset == null)
			{
				return;
			}
			VendorAsset vendorAsset = Assets.find<VendorAsset>(assetGuid);
			if (vendorAsset == null)
			{
				return;
			}
			if (vendorAsset != this.serverCurrentVendorAsset)
			{
				return;
			}
			if (vendorAsset.selling == null)
			{
				return;
			}
			if ((int)index >= vendorAsset.selling.Length)
			{
				return;
			}
			VendorSellingBase vendorSellingBase = vendorAsset.selling[(int)index];
			if (vendorSellingBase == null)
			{
				return;
			}
			if (vendorSellingBase is VendorSellingVehicle)
			{
				asManyAsPossible = false;
				if (Time.realtimeSinceStartup - this.lastVehiclePurchaseRealtime < 5f)
				{
					this.lastVehiclePurchaseRealtime = Time.realtimeSinceStartup;
					return;
				}
			}
			int num = 0;
			while (vendorSellingBase.canBuy(base.player) && vendorSellingBase.areConditionsMet(base.player))
			{
				vendorSellingBase.ApplyConditions(base.player);
				vendorSellingBase.GrantRewards(base.player);
				vendorSellingBase.buy(base.player);
				num++;
				if (!asManyAsPossible || num >= 10)
				{
					return;
				}
			}
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000F5E8E File Offset: 0x000F408E
		public void sendBuyFromVendor(Guid assetGuid, byte index, bool asManyAsPossible)
		{
			PlayerQuests.SendBuyFromVendor.Invoke(base.GetNetId(), ENetReliability.Unreliable, assetGuid, index, asManyAsPossible);
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000F5EA4 File Offset: 0x000F40A4
		[Obsolete]
		public void sendBuyFromVendor(ushort id, byte index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000F5EAB File Offset: 0x000F40AB
		[Obsolete]
		public void tellSetFlag(CSteamID steamID, ushort id, short value)
		{
			this.ReceiveSetFlag(id, value);
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000F5EB5 File Offset: 0x000F40B5
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSetFlag")]
		public void ReceiveSetFlag(ushort id, short value)
		{
			this.setFlag(id, value);
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000F5EBF File Offset: 0x000F40BF
		public void sendSetFlag(ushort id, short value)
		{
			this.setFlag(id, value);
			if (!base.channel.IsLocalPlayer)
			{
				PlayerQuests.SendSetFlag.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), id, value);
			}
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000F5EF4 File Offset: 0x000F40F4
		[Obsolete]
		public void tellRemoveFlag(CSteamID steamID, ushort id)
		{
			this.ReceiveRemoveFlag(id);
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000F5EFD File Offset: 0x000F40FD
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellRemoveFlag")]
		public void ReceiveRemoveFlag(ushort id)
		{
			this.removeFlag(id);
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000F5F06 File Offset: 0x000F4106
		public void sendRemoveFlag(ushort id)
		{
			this.removeFlag(id);
			if (!base.channel.IsLocalPlayer)
			{
				PlayerQuests.SendRemoveFlag.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), id);
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000F5F39 File Offset: 0x000F4139
		[Obsolete]
		public void tellAddQuest(CSteamID steamID, ushort id)
		{
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000F5F3C File Offset: 0x000F413C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveAddQuest(Guid assetGuid)
		{
			QuestAsset questAsset = Assets.find<QuestAsset>(assetGuid);
			if (questAsset != null)
			{
				this.AddQuest(questAsset);
			}
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000F5F5A File Offset: 0x000F415A
		public void ServerAddQuest(QuestAsset questAsset)
		{
			if (questAsset == null)
			{
				return;
			}
			this.AddQuest(questAsset);
			if (!base.channel.IsLocalPlayer)
			{
				PlayerQuests.SendAddQuest.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), questAsset.GUID);
			}
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000F5F98 File Offset: 0x000F4198
		[Obsolete]
		public void sendAddQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				this.ServerAddQuest(questAsset);
			}
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000F5FBD File Offset: 0x000F41BD
		[Obsolete]
		public void tellRemoveQuest(CSteamID steamID, ushort id)
		{
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000F5FC0 File Offset: 0x000F41C0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveRemoveQuest(Guid assetGuid, bool wasCompleted)
		{
			QuestAsset questAsset = Assets.find<QuestAsset>(assetGuid);
			if (questAsset != null)
			{
				this.RemoveQuest(questAsset, wasCompleted);
			}
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000F5FDF File Offset: 0x000F41DF
		public void ServerRemoveQuest(QuestAsset questAsset)
		{
			this.ServerRemoveQuest(questAsset, false);
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000F5FEC File Offset: 0x000F41EC
		public void ServerRemoveQuest(QuestAsset questAsset, bool wasCompleted = false)
		{
			if (questAsset == null)
			{
				return;
			}
			this.RemoveQuest(questAsset, wasCompleted);
			if (!base.channel.IsLocalPlayer)
			{
				PlayerQuests.SendRemoveQuest.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), questAsset.GUID, wasCompleted);
			}
			if (!wasCompleted)
			{
				questAsset.GrantAbandonmentRewards(base.player);
			}
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x000F6044 File Offset: 0x000F4244
		[Obsolete]
		public void sendRemoveQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			this.ServerRemoveQuest(questAsset);
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000F6066 File Offset: 0x000F4266
		public void TrackQuest(QuestAsset questAsset)
		{
			if (this._trackedQuest != null && this._trackedQuest == questAsset)
			{
				this._trackedQuest = null;
			}
			else
			{
				this._trackedQuest = questAsset;
			}
			if (base.channel.IsLocalPlayer)
			{
				this.TriggerTrackedQuestUpdated();
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000F609C File Offset: 0x000F429C
		[Obsolete]
		public void trackQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			this.TrackQuest(questAsset);
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000F60BE File Offset: 0x000F42BE
		[Obsolete]
		public void askTrackQuest(CSteamID steamID, ushort id)
		{
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000F60C0 File Offset: 0x000F42C0
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 5)]
		public void ReceiveTrackQuest(Guid assetGuid)
		{
			QuestAsset questAsset = Assets.find<QuestAsset>(assetGuid);
			this.TrackQuest(questAsset);
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000F60DB File Offset: 0x000F42DB
		public void ClientTrackQuest(QuestAsset questAsset)
		{
			PlayerQuests.SendTrackQuest.Invoke(base.GetNetId(), ENetReliability.Reliable, (questAsset != null) ? questAsset.GUID : Guid.Empty);
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000F6100 File Offset: 0x000F4300
		[Obsolete]
		public void sendTrackQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			this.ClientTrackQuest(questAsset);
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000F6122 File Offset: 0x000F4322
		[Obsolete("Identical to ServerRemoveQuest")]
		public void AbandonQuest(QuestAsset questAsset)
		{
			this.ServerRemoveQuest(questAsset);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000F612C File Offset: 0x000F432C
		[Obsolete]
		public void abandonQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				this.AbandonQuest(questAsset);
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000F6151 File Offset: 0x000F4351
		[Obsolete]
		public void askAbandonQuest(CSteamID steamID, ushort id)
		{
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000F6154 File Offset: 0x000F4354
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 5)]
		public void ReceiveAbandonQuestRequest(Guid assetGuid)
		{
			QuestAsset questAsset = Assets.find<QuestAsset>(assetGuid);
			if (questAsset != null)
			{
				this.ServerRemoveQuest(questAsset);
			}
		}

		/// <summary>
		/// Called by quest details UI to request server to abandon quest.
		/// </summary>
		// Token: 0x06003591 RID: 13713 RVA: 0x000F6172 File Offset: 0x000F4372
		public void ClientAbandonQuest(QuestAsset questAsset)
		{
			if (questAsset != null)
			{
				PlayerQuests.SendAbandonQuestRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, questAsset.GUID);
			}
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000F6190 File Offset: 0x000F4390
		[Obsolete]
		public void sendAbandonQuest(ushort id)
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, id) as QuestAsset;
			if (questAsset != null)
			{
				this.ClientAbandonQuest(questAsset);
			}
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000F61B8 File Offset: 0x000F43B8
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 20)]
		public void ReceiveChooseDialogueResponseRequest(in ServerInvocationContext context, Guid assetGuid, byte index)
		{
			if (this.checkNPC == null)
			{
				return;
			}
			if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (this.serverCurrentDialogueAsset == null)
			{
				return;
			}
			if (this.serverCurrentDialogueMessage == null)
			{
				return;
			}
			DialogueAsset dialogueAsset = Assets.find<DialogueAsset>(assetGuid);
			if (dialogueAsset == null)
			{
				return;
			}
			if (dialogueAsset != this.serverCurrentDialogueAsset)
			{
				return;
			}
			if (dialogueAsset.responses == null || (int)index >= dialogueAsset.responses.Length)
			{
				return;
			}
			if (this.serverCurrentDialogueMessage.responses != null && this.serverCurrentDialogueMessage.responses.Length != 0)
			{
				bool flag = false;
				for (int i = 0; i < this.serverCurrentDialogueMessage.responses.Length; i++)
				{
					if (index == this.serverCurrentDialogueMessage.responses[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			DialogueResponse dialogueResponse = dialogueAsset.responses[(int)index];
			if (dialogueResponse == null || dialogueResponse.conditions == null || !dialogueResponse.areConditionsMet(base.player))
			{
				return;
			}
			if (dialogueResponse.messages != null && dialogueResponse.messages.Length != 0)
			{
				bool flag2 = false;
				for (int j = 0; j < dialogueResponse.messages.Length; j++)
				{
					if (this.serverCurrentDialogueMessage.index == dialogueResponse.messages[j])
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return;
				}
			}
			dialogueResponse.ApplyConditions(base.player);
			dialogueResponse.GrantRewards(base.player);
			VendorAsset vendorAsset = dialogueResponse.FindVendorAsset();
			DialogueAsset dialogueAsset2 = dialogueResponse.FindDialogueAsset();
			DialogueMessage dialogueMessage = (dialogueAsset2 != null) ? dialogueAsset2.GetAvailableMessage(base.player) : null;
			if (vendorAsset != null)
			{
				if (dialogueAsset2 == null || dialogueMessage == null)
				{
					dialogueAsset2 = this.serverCurrentDialogueAsset;
					dialogueMessage = this.serverCurrentDialogueMessage;
				}
				this.serverDefaultNextDialogueAsset = (dialogueMessage.FindPrevDialogueAsset() ?? this.serverCurrentDialogueAsset);
				this.serverCurrentDialogueAsset = dialogueAsset2;
				this.serverCurrentDialogueMessage = dialogueMessage;
				this.serverCurrentVendorAsset = vendorAsset;
				PlayerQuests.SendOpenVendor.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), vendorAsset.GUID, dialogueAsset2.GUID, dialogueMessage.index, this.serverDefaultNextDialogueAsset != null);
			}
			else if (dialogueAsset2 != null && dialogueMessage != null)
			{
				this.serverDefaultNextDialogueAsset = (((dialogueMessage != null) ? dialogueMessage.FindPrevDialogueAsset() : null) ?? this.serverCurrentDialogueAsset);
				this.serverCurrentDialogueAsset = dialogueAsset2;
				this.serverCurrentDialogueMessage = dialogueMessage;
				this.serverCurrentVendorAsset = null;
				PlayerQuests.SendOpenDialogue.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), dialogueAsset2.GUID, dialogueMessage.index, this.serverDefaultNextDialogueAsset != null);
			}
			if (dialogueMessage != null)
			{
				dialogueMessage.ApplyConditions(base.player);
				dialogueMessage.GrantRewards(base.player);
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000F6448 File Offset: 0x000F4648
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 20)]
		public void ReceiveChooseDefaultNextDialogueRequest(in ServerInvocationContext context, Guid assetGuid, byte index)
		{
			if (this.checkNPC == null)
			{
				return;
			}
			if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (this.serverDefaultNextDialogueAsset == null)
			{
				return;
			}
			if (this.serverCurrentDialogueAsset == null)
			{
				return;
			}
			if (this.serverCurrentDialogueMessage == null)
			{
				return;
			}
			DialogueAsset dialogueAsset = Assets.find<DialogueAsset>(assetGuid);
			if (dialogueAsset == null)
			{
				return;
			}
			if (dialogueAsset != this.serverCurrentDialogueAsset || index != this.serverCurrentDialogueMessage.index)
			{
				return;
			}
			DialogueAsset dialogueAsset2 = this.serverDefaultNextDialogueAsset;
			DialogueMessage dialogueMessage = (dialogueAsset2 != null) ? dialogueAsset2.GetAvailableMessage(base.player) : null;
			this.serverDefaultNextDialogueAsset = null;
			if (dialogueMessage != null)
			{
				this.serverDefaultNextDialogueAsset = dialogueMessage.FindPrevDialogueAsset();
				this.serverCurrentDialogueAsset = dialogueAsset2;
				this.serverCurrentDialogueMessage = dialogueMessage;
				this.serverCurrentVendorAsset = null;
				PlayerQuests.SendOpenDialogue.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), dialogueAsset2.GUID, dialogueMessage.index, this.serverDefaultNextDialogueAsset != null);
				dialogueMessage.ApplyConditions(base.player);
				dialogueMessage.GrantRewards(base.player);
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000F6560 File Offset: 0x000F4760
		public void ClientChooseDialogueResponse(Guid assetGuid, byte index)
		{
			PlayerQuests.SendChooseDialogueResponseRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, assetGuid, index);
		}

		/// <summary>
		/// Called when there are no responses to choose, but server has indicated a next dialogue is available.
		/// </summary>
		// Token: 0x06003596 RID: 13718 RVA: 0x000F6575 File Offset: 0x000F4775
		public void ClientChooseNextDialogue(Guid assetGuid, byte index)
		{
			PlayerQuests.SendChooseDefaultNextDialogueRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, assetGuid, index);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000F658A File Offset: 0x000F478A
		[Obsolete]
		public void tellQuests(CSteamID steamID)
		{
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000F658C File Offset: 0x000F478C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveQuests(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			reader.ReadBit(ref this._isMarkerPlaced);
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref this._markerPosition, 13, 7);
			SystemNetPakReaderEx.ReadString(reader, ref this._markerTextOverride, 11);
			SystemNetPakReaderEx.ReadUInt32(reader, ref this._radioFrequency);
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref this._groupID);
			reader.ReadEnum(out this._groupRank);
			if (base.channel.IsLocalPlayer)
			{
				ushort num;
				SystemNetPakReaderEx.ReadUInt16(reader, ref num);
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					ushort num3;
					SystemNetPakReaderEx.ReadUInt16(reader, ref num3);
					short newValue;
					SystemNetPakReaderEx.ReadInt16(reader, ref newValue);
					PlayerQuestFlag playerQuestFlag = new PlayerQuestFlag(num3, newValue);
					this.flagsMap.Add(num3, playerQuestFlag);
					this.flagsList.Add(playerQuestFlag);
				}
				int num4;
				SystemNetPakReaderEx.ReadInt32(reader, ref num4);
				for (int i = 0; i < num4; i++)
				{
					Guid guid;
					SystemNetPakReaderEx.ReadGuid(reader, ref guid);
					QuestAsset questAsset = Assets.find<QuestAsset>(guid);
					if (questAsset != null)
					{
						PlayerQuest playerQuest = new PlayerQuest(questAsset);
						this.questsList.Add(playerQuest);
					}
				}
				Guid guid2;
				SystemNetPakReaderEx.ReadGuid(reader, ref guid2);
				this._trackedQuest = Assets.find<QuestAsset>(guid2);
				reader.ReadBit(ref this.npcCutsceneMode);
				base.player.animator.NotifyLocalPlayerCutsceneModeActiveChanged(this.npcCutsceneMode);
				FlagsUpdated flagsUpdated = this.onFlagsUpdated;
				if (flagsUpdated != null)
				{
					flagsUpdated();
				}
				this.TriggerTrackedQuestUpdated();
			}
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000F66EA File Offset: 0x000F48EA
		[Obsolete]
		public void askQuests(CSteamID steamID)
		{
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000F66EC File Offset: 0x000F48EC
		private void WriteAllState(NetPakWriter writer)
		{
			writer.WriteBit(this.isMarkerPlaced);
			UnityNetPakWriterEx.WriteClampedVector3(writer, this.markerPosition, 13, 7);
			SystemNetPakWriterEx.WriteString(writer, this.markerTextOverride, 11);
			SystemNetPakWriterEx.WriteUInt32(writer, this.radioFrequency);
			SteamworksNetPakWriterEx.WriteSteamID(writer, this.groupID);
			writer.WriteEnum(this.groupRank);
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000F674C File Offset: 0x000F494C
		private void WriteOwnerState(NetPakWriter writer)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, (ushort)this.flagsList.Count);
			ushort num = 0;
			while ((int)num < this.flagsList.Count)
			{
				PlayerQuestFlag playerQuestFlag = this.flagsList[(int)num];
				SystemNetPakWriterEx.WriteUInt16(writer, playerQuestFlag.id);
				SystemNetPakWriterEx.WriteInt16(writer, playerQuestFlag.value);
				num += 1;
			}
			SystemNetPakWriterEx.WriteInt32(writer, this.questsList.Count);
			foreach (PlayerQuest playerQuest in this.questsList)
			{
				Nullable<Guid> guid;
				if (playerQuest == null)
				{
					guid = default(Guid?);
				}
				else
				{
					QuestAsset asset = playerQuest.asset;
					guid = ((asset != null) ? new Guid?(asset.GUID) : default(Guid?));
				}
				SystemNetPakWriterEx.WriteGuid(writer, guid ?? Guid.Empty);
			}
			QuestAsset trackedQuest = this._trackedQuest;
			SystemNetPakWriterEx.WriteGuid(writer, (trackedQuest != null) ? trackedQuest.GUID : Guid.Empty);
			writer.WriteBit(this.npcCutsceneMode);
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000F6874 File Offset: 0x000F4A74
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			bool sendingToOwner = base.channel.owner == client;
			if (base.channel.IsLocalPlayer & sendingToOwner)
			{
				return;
			}
			if (this.isMemberOfAGroup)
			{
				GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
				if (groupInfo != null)
				{
					GroupManager.sendGroupInfo(client.transportConnection, groupInfo);
				}
			}
			PlayerQuests.SendQuests.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, delegate(NetPakWriter writer)
			{
				this.WriteAllState(writer);
				if (sendingToOwner)
				{
					this.WriteOwnerState(writer);
				}
			});
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000F6900 File Offset: 0x000F4B00
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			if (this.isMemberOfAGroup)
			{
				GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
				if (groupInfo != null)
				{
					GroupManager.sendGroupInfo(transportConnections, groupInfo);
				}
			}
			PlayerQuests.SendQuests.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, delegate(NetPakWriter writer)
			{
				this.WriteAllState(writer);
			});
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000F6949 File Offset: 0x000F4B49
		private void OnPlayerNavChanged(PlayerMovement sender, byte oldNav, byte newNav)
		{
			if (newNav == 255)
			{
				return;
			}
			ZombieManager.regions[(int)newNav].UpdateBoss();
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000F6960 File Offset: 0x000F4B60
		private void onExperienceUpdated(uint experience)
		{
			this.TriggerTrackedQuestUpdated();
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000F6968 File Offset: 0x000F4B68
		private void onReputationUpdated(int reputation)
		{
			this.TriggerTrackedQuestUpdated();
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000F6970 File Offset: 0x000F4B70
		private void onInventoryStateUpdated()
		{
			this.TriggerTrackedQuestUpdated();
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000F6978 File Offset: 0x000F4B78
		private void onTimeOfDayChanged()
		{
			ExternalConditionsUpdated externalConditionsUpdated = this.onExternalConditionsUpdated;
			if (externalConditionsUpdated == null)
			{
				return;
			}
			externalConditionsUpdated();
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000F698C File Offset: 0x000F4B8C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveTalkWithNpcResponse(in ClientInvocationContext context, NetId targetNpcNetId, Guid dialogueAssetGuid, byte messageIndex, bool hasNextDialogue)
		{
			InteractableObjectNPC npcFromObjectNetId = InteractableObjectNPC.GetNpcFromObjectNetId(targetNpcNetId);
			if (npcFromObjectNetId == null)
			{
				return;
			}
			DialogueAsset dialogueAsset = Assets.find<DialogueAsset>(dialogueAssetGuid);
			ClientAssetIntegrity.QueueRequest(dialogueAssetGuid, dialogueAsset, "talk with NPC response");
			if (dialogueAsset == null)
			{
				return;
			}
			if ((int)messageIndex >= dialogueAsset.messages.Length)
			{
				return;
			}
			this.checkNPC = npcFromObjectNetId;
			PlayerLifeUI.close();
			PlayerLifeUI.npc = npcFromObjectNetId;
			npcFromObjectNetId.isLookingAtPlayer = true;
			PlayerNPCDialogueUI.open(dialogueAsset, dialogueAsset.messages[(int)messageIndex], hasNextDialogue);
		}

		/// <summary>
		/// Called in singleplayer and on the server after client requests NPC dialogue.
		/// </summary>
		// Token: 0x060035A4 RID: 13732 RVA: 0x000F69F8 File Offset: 0x000F4BF8
		internal void ApproveTalkWithNpcRequest(InteractableObjectNPC targetNpc, DialogueAsset rootDialogueAsset)
		{
			DialogueMessage availableMessage = rootDialogueAsset.GetAvailableMessage(base.player);
			if (availableMessage == null)
			{
				UnturnedLog.warn("Unable to approve talk with NPC (" + targetNpc.npcAsset.FriendlyName + ") request because there is no valid message");
				return;
			}
			this.checkNPC = targetNpc;
			this.serverCurrentDialogueAsset = rootDialogueAsset;
			this.serverCurrentDialogueMessage = availableMessage;
			this.serverCurrentVendorAsset = null;
			this.serverDefaultNextDialogueAsset = availableMessage.FindPrevDialogueAsset();
			PlayerQuests.SendTalkWithNpcResponse.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), targetNpc.GetNpcNetId(), rootDialogueAsset.GUID, this.serverCurrentDialogueMessage.index, this.serverDefaultNextDialogueAsset != null);
			this.serverCurrentDialogueMessage.ApplyConditions(base.player);
			this.serverCurrentDialogueMessage.GrantRewards(base.player);
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000F6ABB File Offset: 0x000F4CBB
		internal void ClearActiveNpc()
		{
			this.checkNPC = null;
			this.serverCurrentDialogueAsset = null;
			this.serverCurrentDialogueMessage = null;
			this.serverCurrentVendorAsset = null;
			this.serverDefaultNextDialogueAsset = null;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000F6AE0 File Offset: 0x000F4CE0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveOpenDialogue(in ClientInvocationContext context, Guid dialogueAssetGuid, byte messageIndex, bool hasNextDialogue)
		{
			DialogueAsset dialogueAsset = Assets.find<DialogueAsset>(dialogueAssetGuid);
			ClientAssetIntegrity.QueueRequest(dialogueAssetGuid, dialogueAsset, "open dialogue");
			if (dialogueAsset == null)
			{
				return;
			}
			if (dialogueAsset.messages == null)
			{
				return;
			}
			int num = dialogueAsset.messages.Length;
			if (PlayerNPCVendorUI.active)
			{
				PlayerNPCVendorUI.close();
			}
			if (PlayerNPCQuestUI.active)
			{
				PlayerNPCQuestUI.close();
			}
			DialogueMessage newMessage = dialogueAsset.messages[(int)messageIndex];
			PlayerNPCDialogueUI.open(dialogueAsset, newMessage, hasNextDialogue);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000F6B44 File Offset: 0x000F4D44
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveOpenVendor(in ClientInvocationContext context, Guid vendorAssetGuid, Guid dialogueAssetGuid, byte messageIndex, bool hasNextDialogue)
		{
			VendorAsset vendorAsset = Assets.find<VendorAsset>(vendorAssetGuid);
			DialogueAsset dialogueAsset = Assets.find<DialogueAsset>(dialogueAssetGuid);
			ClientAssetIntegrity.QueueRequest(vendorAssetGuid, vendorAsset, "open vendor");
			ClientAssetIntegrity.QueueRequest(dialogueAssetGuid, dialogueAsset, "open vendor");
			if (vendorAsset == null)
			{
				return;
			}
			if (dialogueAsset == null)
			{
				return;
			}
			if (dialogueAsset.messages == null)
			{
				return;
			}
			int num = dialogueAsset.messages.Length;
			if (PlayerNPCDialogueUI.active)
			{
				PlayerNPCDialogueUI.close();
			}
			if (PlayerNPCQuestUI.active)
			{
				PlayerNPCQuestUI.close();
			}
			DialogueMessage newNextMessage = dialogueAsset.messages[(int)messageIndex];
			PlayerNPCVendorUI.open(vendorAsset, dialogueAsset, newNextMessage, hasNextDialogue);
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000F6BC0 File Offset: 0x000F4DC0
		internal PlayerDelayedQuestRewardsComponent GetOrCreateDelayedQuestRewards()
		{
			if (!this.hasCreatedDelayedRewards && this.delayedRewardsComponent == null)
			{
				this.hasCreatedDelayedRewards = true;
				this.delayedRewardsGameObject = new GameObject();
				this.delayedRewardsComponent = this.delayedRewardsGameObject.AddComponent<PlayerDelayedQuestRewardsComponent>();
				this.delayedRewardsComponent.player = base.player;
			}
			return this.delayedRewardsComponent;
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000F6C1D File Offset: 0x000F4E1D
		internal void StopDelayedQuestRewards()
		{
			if (this.delayedRewardsComponent != null)
			{
				this.delayedRewardsComponent.StopAllCoroutines();
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000F6C38 File Offset: 0x000F4E38
		private void OnLifeUpdated(bool isDead)
		{
			if (isDead)
			{
				this.StopDelayedQuestRewards();
				this.ServerSetCutsceneModeActive(false);
			}
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000F6C4C File Offset: 0x000F4E4C
		internal void InitializePlayer()
		{
			this.flagsMap = new Dictionary<ushort, PlayerQuestFlag>();
			this.flagsList = new List<PlayerQuestFlag>();
			this.questsList = new List<PlayerQuest>();
			this.groupInvites = new HashSet<CSteamID>();
			if (Provider.isServer || base.channel.IsLocalPlayer)
			{
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.OnLifeUpdated));
			}
			if (Provider.isServer)
			{
				this.load();
				base.player.movement.PlayerNavChanged += this.OnPlayerNavChanged;
				if (base.channel.IsLocalPlayer)
				{
					FlagsUpdated flagsUpdated = this.onFlagsUpdated;
					if (flagsUpdated != null)
					{
						flagsUpdated();
					}
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				PlayerSkills skills = base.player.skills;
				skills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(skills.onExperienceUpdated, new ExperienceUpdated(this.onExperienceUpdated));
				PlayerSkills skills2 = base.player.skills;
				skills2.onReputationUpdated = (ReputationUpdated)Delegate.Combine(skills2.onReputationUpdated, new ReputationUpdated(this.onReputationUpdated));
				PlayerInventory inventory = base.player.inventory;
				inventory.onInventoryStateUpdated = (InventoryStateUpdated)Delegate.Combine(inventory.onInventoryStateUpdated, new InventoryStateUpdated(this.onInventoryStateUpdated));
				LightingManager.onTimeOfDayChanged = (TimeOfDayChanged)Delegate.Combine(LightingManager.onTimeOfDayChanged, new TimeOfDayChanged(this.onTimeOfDayChanged));
			}
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000F6DC0 File Offset: 0x000F4FC0
		private void Start()
		{
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				try
				{
					PlayerCreated onPlayerCreated = Player.onPlayerCreated;
					if (onPlayerCreated != null)
					{
						onPlayerCreated(base.player);
					}
				}
				catch (Exception e)
				{
					UnturnedLog.warn("Exception during onPlayerCreated:");
					UnturnedLog.exception(e);
				}
			}
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000F6E1C File Offset: 0x000F501C
		private void OnDestroy()
		{
			if (base.channel.IsLocalPlayer)
			{
				LightingManager.onTimeOfDayChanged = (TimeOfDayChanged)Delegate.Remove(LightingManager.onTimeOfDayChanged, new TimeOfDayChanged(this.onTimeOfDayChanged));
			}
			this.hasCreatedDelayedRewards = true;
			if (this.delayedRewardsGameObject != null)
			{
				Object.Destroy(this.delayedRewardsGameObject);
				this.delayedRewardsGameObject = null;
			}
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x000F6E80 File Offset: 0x000F5080
		public void load()
		{
			this.wasLoadCalled = true;
			this.isMarkerPlaced = false;
			this.markerPosition = Vector3.zero;
			this.markerTextOverride = string.Empty;
			this.radioFrequency = PlayerQuests.DEFAULT_RADIO_FREQUENCY;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Quests.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = PlayerSavedata.openRiver(base.channel.owner.playerID, "/Player/Quests.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					if (b > 6)
					{
						this.isMarkerPlaced = river.readBoolean();
						this.markerPosition = river.readSingleVector3();
					}
					if (b > 5)
					{
						this.radioFrequency = river.readUInt32();
					}
					if (b > 2)
					{
						this.groupID = river.readSteamID();
					}
					else
					{
						this.groupID = CSteamID.Nil;
					}
					if (b > 3)
					{
						this.groupRank = (EPlayerGroupRank)river.readByte();
					}
					else
					{
						this.groupRank = EPlayerGroupRank.MEMBER;
					}
					if (b > 4)
					{
						this.inMainGroup = river.readBoolean();
					}
					else
					{
						this.inMainGroup = false;
					}
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						ushort num3 = river.readUInt16();
						short newValue = river.readInt16();
						PlayerQuestFlag playerQuestFlag = new PlayerQuestFlag(num3, newValue);
						this.flagsMap.Add(num3, playerQuestFlag);
						this.flagsList.Add(playerQuestFlag);
					}
					if (b >= 10)
					{
						int num4 = river.readInt32();
						for (int i = 0; i < num4; i++)
						{
							QuestAsset questAsset = Assets.find<QuestAsset>(river.readGUID());
							if (questAsset != null)
							{
								PlayerQuest playerQuest = new PlayerQuest(questAsset);
								this.questsList.Add(playerQuest);
							}
						}
					}
					else
					{
						ushort num5 = river.readUInt16();
						for (ushort num6 = 0; num6 < num5; num6 += 1)
						{
							PlayerQuest playerQuest2 = new PlayerQuest(river.readUInt16());
							this.questsList.Add(playerQuest2);
						}
					}
					if (b >= 9)
					{
						this._trackedQuest = Assets.find<QuestAsset>(river.readGUID());
					}
					else if (b > 1)
					{
						this._trackedQuest = (Assets.find(EAssetType.NPC, river.readUInt16()) as QuestAsset);
					}
					else
					{
						this._trackedQuest = null;
					}
					if (b < 8)
					{
						this.npcSpawnId = null;
					}
					else
					{
						this.npcSpawnId = river.readString();
					}
					if (b >= 11)
					{
						this.npcCutsceneMode = river.readBoolean();
					}
					else
					{
						this.npcCutsceneMode = false;
					}
				}
				river.closeRiver();
			}
			if (base.channel.IsLocalPlayer)
			{
				base.player.animator.NotifyLocalPlayerCutsceneModeActiveChanged(this.npcCutsceneMode);
			}
			if (Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups)
			{
				if (this.groupID == CSteamID.Nil)
				{
					if (!(base.channel.owner.lobbyID != CSteamID.Nil) || !Provider.modeConfigData.Gameplay.Allow_Lobby_Groups)
					{
						this.loadMainGroup();
						return;
					}
					bool flag;
					GroupInfo orAddGroup = GroupManager.getOrAddGroup(base.channel.owner.lobbyID, base.channel.owner.playerID.playerName + "'s Group", out flag);
					if (flag || orAddGroup.hasSpaceForMoreMembersInGroup)
					{
						this.groupID = base.channel.owner.lobbyID;
						orAddGroup.members += 1U;
						this.groupRank = (flag ? EPlayerGroupRank.OWNER : EPlayerGroupRank.MEMBER);
						this.inMainGroup = false;
						GroupManager.sendGroupInfo(orAddGroup);
						return;
					}
					this.loadMainGroup();
					return;
				}
				else if (this.inMainGroup)
				{
					if (!Provider.modeConfigData.Gameplay.Allow_Static_Groups)
					{
						this.loadMainGroup();
						return;
					}
					if (this.groupID != base.channel.owner.playerID.group)
					{
						this.loadMainGroup();
						return;
					}
				}
				else if (GroupManager.getGroupInfo(this.groupID) == null)
				{
					this.loadMainGroup();
					return;
				}
			}
			else
			{
				this.loadMainGroup();
			}
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x000F7244 File Offset: 0x000F5444
		private void loadMainGroup()
		{
			if (Provider.modeConfigData.Gameplay.Allow_Static_Groups)
			{
				this.groupID = base.channel.owner.playerID.group;
				this.inMainGroup = (this.groupID != CSteamID.Nil);
			}
			else
			{
				this.groupID = CSteamID.Nil;
				this.inMainGroup = false;
			}
			this.groupRank = EPlayerGroupRank.MEMBER;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000F72B0 File Offset: 0x000F54B0
		private int FindIndexOfQuest(QuestAsset asset)
		{
			if (asset != null)
			{
				for (int i = 0; i < this.questsList.Count; i++)
				{
					if (this.questsList[i].asset == asset)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000F72F0 File Offset: 0x000F54F0
		public void save()
		{
			if (!this.wasLoadCalled)
			{
				return;
			}
			River river = PlayerSavedata.openRiver(base.channel.owner.playerID, "/Player/Quests.dat", false);
			river.writeByte(11);
			river.writeBoolean(this.isMarkerPlaced);
			river.writeSingleVector3(this.markerPosition);
			river.writeUInt32(this.radioFrequency);
			river.writeSteamID(this.groupID);
			river.writeByte((byte)this.groupRank);
			river.writeBoolean(this.inMainGroup);
			river.writeUInt16((ushort)this.flagsList.Count);
			ushort num = 0;
			while ((int)num < this.flagsList.Count)
			{
				PlayerQuestFlag playerQuestFlag = this.flagsList[(int)num];
				river.writeUInt16(playerQuestFlag.id);
				river.writeInt16(playerQuestFlag.value);
				num += 1;
			}
			river.writeInt32(this.questsList.Count);
			foreach (PlayerQuest playerQuest in this.questsList)
			{
				River river2 = river;
				QuestAsset asset = playerQuest.asset;
				river2.writeGUID((asset != null) ? asset.GUID : Guid.Empty);
			}
			River river3 = river;
			QuestAsset trackedQuest = this._trackedQuest;
			river3.writeGUID((trackedQuest != null) ? trackedQuest.GUID : Guid.Empty);
			river.writeString(string.IsNullOrEmpty(this.npcSpawnId) ? string.Empty : this.npcSpawnId);
			river.writeBoolean(this.npcCutsceneMode);
			river.closeRiver();
		}

		// Token: 0x04001EA8 RID: 7848
		private const byte SAVEDATA_VERSION_ADDED_NPC_SPAWN_ID = 8;

		// Token: 0x04001EA9 RID: 7849
		private const byte SAVEDATA_VERSION_ADDED_TRACKED_QUEST_GUID = 9;

		// Token: 0x04001EAA RID: 7850
		private const byte SAVEDATA_VERSION_ADDED_QUEST_LIST_GUIDS = 10;

		// Token: 0x04001EAB RID: 7851
		private const byte SAVEDATA_VERSION_ADDED_NPC_CUTSCENE_MODE = 11;

		// Token: 0x04001EAC RID: 7852
		private const byte SAVEDATA_VERSION_NEWEST = 11;

		// Token: 0x04001EAD RID: 7853
		public static readonly byte SAVEDATA_VERSION = 11;

		// Token: 0x04001EAE RID: 7854
		public static readonly uint DEFAULT_RADIO_FREQUENCY = 460327U;

		// Token: 0x04001EAF RID: 7855
		private static PlayerQuestFlagComparator flagComparator = new PlayerQuestFlagComparator();

		// Token: 0x04001EB0 RID: 7856
		private static PlayerQuestComparator questComparator = new PlayerQuestComparator();

		// Token: 0x04001EB1 RID: 7857
		public InteractableObjectNPC checkNPC;

		// Token: 0x04001EB2 RID: 7858
		private DialogueAsset serverCurrentDialogueAsset;

		// Token: 0x04001EB3 RID: 7859
		private VendorAsset serverCurrentVendorAsset;

		// Token: 0x04001EB4 RID: 7860
		private DialogueMessage serverCurrentDialogueMessage;

		/// <summary>
		/// The dialogue to go to when a message has no available responses.
		/// If this is not specified the previous dialogue is used as a default.
		/// </summary>
		// Token: 0x04001EB5 RID: 7861
		private DialogueAsset serverDefaultNextDialogueAsset;

		// Token: 0x04001EB6 RID: 7862
		private Dictionary<ushort, PlayerQuestFlag> flagsMap;

		// Token: 0x04001EB7 RID: 7863
		public ExternalConditionsUpdated onExternalConditionsUpdated;

		// Token: 0x04001EB8 RID: 7864
		public FlagsUpdated onFlagsUpdated;

		// Token: 0x04001EB9 RID: 7865
		public FlagUpdated onFlagUpdated;

		// Token: 0x04001EBD RID: 7869
		public static GroupUpdatedHandler groupUpdated;

		// Token: 0x04001EC4 RID: 7876
		private QuestAsset _trackedQuest;

		// Token: 0x04001EC6 RID: 7878
		private bool _isMarkerPlaced;

		// Token: 0x04001EC7 RID: 7879
		private Vector3 _markerPosition;

		/// <summary>
		/// Overrides label text next to marker on map.
		/// Used by plugins. Not saved to disk.
		/// </summary>
		// Token: 0x04001EC8 RID: 7880
		private string _markerTextOverride;

		// Token: 0x04001EC9 RID: 7881
		private uint _radioFrequency;

		// Token: 0x04001ECA RID: 7882
		private CSteamID _groupID;

		// Token: 0x04001ECB RID: 7883
		private EPlayerGroupRank _groupRank;

		/// <summary>
		/// Kept serverside. Used to check whether the player is currently in their Steam group or just a normal in-game group.
		/// </summary>
		// Token: 0x04001ECD RID: 7885
		private bool inMainGroup;

		/// <summary>
		/// If set, default spawn logic will check for a location node or spawnpoint node matching name.
		/// Saved and loaded between sessions.
		/// </summary>
		// Token: 0x04001ECE RID: 7886
		public string npcSpawnId;

		// Token: 0x04001ECF RID: 7887
		private bool npcCutsceneMode;

		// Token: 0x04001ED0 RID: 7888
		private static readonly ClientInstanceMethod<bool> SendCutsceneMode = ClientInstanceMethod<bool>.Get(typeof(PlayerQuests), "ReceiveCutsceneMode");

		// Token: 0x04001ED1 RID: 7889
		private static readonly ClientInstanceMethod<bool, Vector3, string> SendMarkerState = ClientInstanceMethod<bool, Vector3, string>.Get(typeof(PlayerQuests), "ReceiveMarkerState");

		// Token: 0x04001ED2 RID: 7890
		private static readonly ServerInstanceMethod<bool, Vector3> SendSetMarkerRequest = ServerInstanceMethod<bool, Vector3>.Get(typeof(PlayerQuests), "ReceiveSetMarkerRequest");

		// Token: 0x04001ED3 RID: 7891
		private static readonly ClientInstanceMethod<uint> SendRadioFrequencyState = ClientInstanceMethod<uint>.Get(typeof(PlayerQuests), "ReceiveRadioFrequencyState");

		// Token: 0x04001ED4 RID: 7892
		private static readonly ServerInstanceMethod<uint> SendSetRadioFrequencyRequest = ServerInstanceMethod<uint>.Get(typeof(PlayerQuests), "ReceiveSetRadioFrequencyRequest");

		// Token: 0x04001ED5 RID: 7893
		private static readonly ClientInstanceMethod<CSteamID, EPlayerGroupRank> SendGroupState = ClientInstanceMethod<CSteamID, EPlayerGroupRank>.Get(typeof(PlayerQuests), "ReceiveGroupState");

		// Token: 0x04001ED6 RID: 7894
		private static readonly ServerInstanceMethod<CSteamID> SendAcceptGroupInvitationRequest = ServerInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveAcceptGroupInvitationRequest");

		// Token: 0x04001ED7 RID: 7895
		private static readonly ServerInstanceMethod<CSteamID> SendDeclineGroupInvitationRequest = ServerInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveDeclineGroupInvitationRequest");

		// Token: 0x04001ED8 RID: 7896
		private float lastLeaveGroupRequestRealtime;

		// Token: 0x04001ED9 RID: 7897
		private static readonly ServerInstanceMethod SendLeaveGroupRequest = ServerInstanceMethod.Get(typeof(PlayerQuests), "ReceiveLeaveGroupRequest");

		// Token: 0x04001EDA RID: 7898
		private static readonly ServerInstanceMethod SendDeleteGroupRequest = ServerInstanceMethod.Get(typeof(PlayerQuests), "ReceiveDeleteGroupRequest");

		// Token: 0x04001EDB RID: 7899
		private static readonly ServerInstanceMethod SendCreateGroupRequest = ServerInstanceMethod.Get(typeof(PlayerQuests), "ReceiveCreateGroupRequest");

		// Token: 0x04001EDC RID: 7900
		private static readonly ClientInstanceMethod<CSteamID> SendAddGroupInviteClient = ClientInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveAddGroupInviteClient");

		// Token: 0x04001EDD RID: 7901
		private static readonly ClientInstanceMethod<CSteamID> SendRemoveGroupInviteClient = ClientInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveRemoveGroupInviteClient");

		// Token: 0x04001EDE RID: 7902
		private static readonly ServerInstanceMethod<CSteamID> SendAddGroupInviteRequest = ServerInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveAddGroupInviteRequest");

		// Token: 0x04001EDF RID: 7903
		private static readonly ServerInstanceMethod<CSteamID> SendPromoteRequest = ServerInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceivePromoteRequest");

		// Token: 0x04001EE0 RID: 7904
		private static readonly ServerInstanceMethod<CSteamID> SendDemoteRequest = ServerInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveDemoteRequest");

		// Token: 0x04001EE1 RID: 7905
		private static readonly ServerInstanceMethod<CSteamID> SendKickFromGroup = ServerInstanceMethod<CSteamID>.Get(typeof(PlayerQuests), "ReceiveKickFromGroup");

		// Token: 0x04001EE2 RID: 7906
		private static readonly ServerInstanceMethod<string> SendRenameGroupRequest = ServerInstanceMethod<string>.Get(typeof(PlayerQuests), "ReceiveRenameGroupRequest");

		// Token: 0x04001EE3 RID: 7907
		private static readonly ServerInstanceMethod<Guid, byte, bool> SendSellToVendor = ServerInstanceMethod<Guid, byte, bool>.Get(typeof(PlayerQuests), "ReceiveSellToVendor");

		// Token: 0x04001EE4 RID: 7908
		private static readonly ServerInstanceMethod<Guid, byte, bool> SendBuyFromVendor = ServerInstanceMethod<Guid, byte, bool>.Get(typeof(PlayerQuests), "ReceiveBuyFromVendor");

		// Token: 0x04001EE5 RID: 7909
		private static readonly ClientInstanceMethod<ushort, short> SendSetFlag = ClientInstanceMethod<ushort, short>.Get(typeof(PlayerQuests), "ReceiveSetFlag");

		// Token: 0x04001EE6 RID: 7910
		private static readonly ClientInstanceMethod<ushort> SendRemoveFlag = ClientInstanceMethod<ushort>.Get(typeof(PlayerQuests), "ReceiveRemoveFlag");

		// Token: 0x04001EE7 RID: 7911
		private static readonly ClientInstanceMethod<Guid> SendAddQuest = ClientInstanceMethod<Guid>.Get(typeof(PlayerQuests), "ReceiveAddQuest");

		// Token: 0x04001EE8 RID: 7912
		private static readonly ClientInstanceMethod<Guid, bool> SendRemoveQuest = ClientInstanceMethod<Guid, bool>.Get(typeof(PlayerQuests), "ReceiveRemoveQuest");

		// Token: 0x04001EE9 RID: 7913
		private static readonly ServerInstanceMethod<Guid> SendTrackQuest = ServerInstanceMethod<Guid>.Get(typeof(PlayerQuests), "ReceiveTrackQuest");

		// Token: 0x04001EEA RID: 7914
		private static readonly ServerInstanceMethod<Guid> SendAbandonQuestRequest = ServerInstanceMethod<Guid>.Get(typeof(PlayerQuests), "ReceiveAbandonQuestRequest");

		// Token: 0x04001EEB RID: 7915
		private static readonly ServerInstanceMethod<Guid, byte> SendChooseDialogueResponseRequest = ServerInstanceMethod<Guid, byte>.Get(typeof(PlayerQuests), "ReceiveChooseDialogueResponseRequest");

		// Token: 0x04001EEC RID: 7916
		private static readonly ServerInstanceMethod<Guid, byte> SendChooseDefaultNextDialogueRequest = ServerInstanceMethod<Guid, byte>.Get(typeof(PlayerQuests), "ReceiveChooseDefaultNextDialogueRequest");

		// Token: 0x04001EED RID: 7917
		private static readonly ClientInstanceMethod SendQuests = ClientInstanceMethod.Get(typeof(PlayerQuests), "ReceiveQuests");

		// Token: 0x04001EEE RID: 7918
		private static readonly ClientInstanceMethod<NetId, Guid, byte, bool> SendTalkWithNpcResponse = ClientInstanceMethod<NetId, Guid, byte, bool>.Get(typeof(PlayerQuests), "ReceiveTalkWithNpcResponse");

		// Token: 0x04001EEF RID: 7919
		private static readonly ClientInstanceMethod<Guid, byte, bool> SendOpenDialogue = ClientInstanceMethod<Guid, byte, bool>.Get(typeof(PlayerQuests), "ReceiveOpenDialogue");

		// Token: 0x04001EF0 RID: 7920
		private static readonly ClientInstanceMethod<Guid, Guid, byte, bool> SendOpenVendor = ClientInstanceMethod<Guid, Guid, byte, bool>.Get(typeof(PlayerQuests), "ReceiveOpenVendor");

		// Token: 0x04001EF1 RID: 7921
		private GameObject delayedRewardsGameObject;

		// Token: 0x04001EF2 RID: 7922
		private PlayerDelayedQuestRewardsComponent delayedRewardsComponent;

		/// <summary>
		/// Prevent re-creating it during destroy (e.g. plugin granting rewards) from leaking gameobject.
		/// </summary>
		// Token: 0x04001EF3 RID: 7923
		private bool hasCreatedDelayedRewards;

		// Token: 0x04001EF4 RID: 7924
		private bool wasLoadCalled;

		// Token: 0x04001EF5 RID: 7925
		private float lastVehiclePurchaseRealtime = -10f;

		// Token: 0x020009AB RID: 2475
		// (Invoke) Token: 0x06004BFC RID: 19452
		public delegate void AnyFlagChangedHandler(PlayerQuests quests, PlayerQuestFlag flag);

		// Token: 0x020009AC RID: 2476
		// (Invoke) Token: 0x06004C00 RID: 19456
		public delegate void GroupChangedCallback(PlayerQuests sender, CSteamID oldGroupID, EPlayerGroupRank oldGroupRank, CSteamID newGroupID, EPlayerGroupRank newGroupRank);
	}
}
