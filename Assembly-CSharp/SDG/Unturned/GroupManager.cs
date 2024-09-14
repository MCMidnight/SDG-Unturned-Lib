using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000559 RID: 1369
	public class GroupManager : SteamCaller
	{
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x000BAA52 File Offset: 0x000B8C52
		public static GroupManager instance
		{
			get
			{
				return GroupManager.manager;
			}
		}

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06002BB8 RID: 11192 RVA: 0x000BAA5C File Offset: 0x000B8C5C
		// (remove) Token: 0x06002BB9 RID: 11193 RVA: 0x000BAA90 File Offset: 0x000B8C90
		public static event GroupInfoReadyHandler groupInfoReady;

		// Token: 0x06002BBA RID: 11194 RVA: 0x000BAAC3 File Offset: 0x000B8CC3
		public static CSteamID generateUniqueGroupID()
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			CSteamID result = GroupManager.availableGroupID;
			GroupManager.availableGroupID.SetAccountID(new AccountID_t(GroupManager.availableGroupID.GetAccountID().m_AccountID + 1U));
			return result;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000BAAF0 File Offset: 0x000B8CF0
		public static GroupInfo addGroup(CSteamID groupID, string name)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			GroupInfo groupInfo = new GroupInfo(groupID, name, 0U);
			GroupManager.knownGroups.Add(groupID, groupInfo);
			return groupInfo;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000BAB18 File Offset: 0x000B8D18
		public static GroupInfo getGroupInfo(CSteamID groupID)
		{
			GroupInfo result = null;
			GroupManager.knownGroups.TryGetValue(groupID, ref result);
			return result;
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000BAB38 File Offset: 0x000B8D38
		public static GroupInfo getOrAddGroup(CSteamID groupID, string name, out bool wasCreated)
		{
			wasCreated = false;
			GroupInfo groupInfo = GroupManager.getGroupInfo(groupID);
			if (groupInfo == null)
			{
				groupInfo = GroupManager.addGroup(groupID, name);
				wasCreated = true;
			}
			return groupInfo;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000BAB60 File Offset: 0x000B8D60
		public static void deleteGroup(CSteamID groupID)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			GroupManager.CancelAllQueuedExitsForGroup(groupID);
			GroupManager.knownGroups.Remove(groupID);
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.player == null) && !(steamPlayer.player.quests == null) && steamPlayer.player.quests.isMemberOfGroup(groupID))
				{
					steamPlayer.player.quests.leaveGroup(true);
				}
			}
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000BAC08 File Offset: 0x000B8E08
		private static void triggerGroupInfoReady(GroupInfo group)
		{
			GroupInfoReadyHandler groupInfoReadyHandler = GroupManager.groupInfoReady;
			if (groupInfoReadyHandler == null)
			{
				return;
			}
			groupInfoReadyHandler(group);
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x000BAC1C File Offset: 0x000B8E1C
		[Obsolete]
		public static void sendGroupInfo(CSteamID steamID, GroupInfo group)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamID);
			if (transportConnection != null)
			{
				GroupManager.sendGroupInfo(transportConnection, group);
			}
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000BAC3A File Offset: 0x000B8E3A
		public static void sendGroupInfo(ITransportConnection transportConnection, GroupInfo group)
		{
			GroupManager.SendGroupInfo.Invoke(ENetReliability.Reliable, transportConnection, group.groupID, group.name, group.members);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000BAC5A File Offset: 0x000B8E5A
		public static void sendGroupInfo(List<ITransportConnection> transportConnections, GroupInfo group)
		{
			GroupManager.SendGroupInfo.Invoke(ENetReliability.Reliable, transportConnections, group.groupID, group.name, group.members);
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000BAC7C File Offset: 0x000B8E7C
		[Obsolete]
		public static void sendGroupInfo(IEnumerable<ITransportConnection> transportConnections, GroupInfo group)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				GroupManager.sendGroupInfo(list, group);
				return;
			}
			throw new ArgumentException("should be a list", "transportConnections");
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000BACAC File Offset: 0x000B8EAC
		public static void sendGroupInfo(GroupInfo group)
		{
			GroupManager.sendGroupInfo(Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer client) => client.player.quests.isMemberOfGroup(group.groupID)), group);
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000BACE2 File Offset: 0x000B8EE2
		[Obsolete]
		public void tellGroupInfo(CSteamID steamID, CSteamID groupID, string name, uint members)
		{
			GroupManager.ReceiveGroupInfo(groupID, name, members);
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000BACF0 File Offset: 0x000B8EF0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellGroupInfo")]
		public static void ReceiveGroupInfo(CSteamID groupID, string name, uint members)
		{
			GroupInfo groupInfo = GroupManager.getGroupInfo(groupID);
			if (groupInfo == null)
			{
				groupInfo = new GroupInfo(groupID, name, members);
				GroupManager.knownGroups.Add(groupInfo.groupID, groupInfo);
			}
			else
			{
				groupInfo.name = name;
				groupInfo.members = members;
			}
			GroupManager.triggerGroupInfoReady(groupInfo);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000BAD38 File Offset: 0x000B8F38
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				GroupManager.availableGroupID = new CSteamID(new AccountID_t(1U), EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeConsoleUser);
				GroupManager.knownGroups = new Dictionary<CSteamID, GroupInfo>();
				GroupManager.queuedExits = new List<QueuedGroupExit>();
				if (Provider.isServer && Level.info != null)
				{
					GroupManager.load();
				}
			}
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000BAD88 File Offset: 0x000B8F88
		private void Start()
		{
			GroupManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000BADDB File Offset: 0x000B8FDB
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Groups: {0}", GroupManager.knownGroups.Count));
			results.Add(string.Format("Queued group exits: {0}", GroupManager.queuedExits.Count));
		}

		/// <summary>
		/// Is player already waiting to exit their group?
		/// </summary>
		// Token: 0x06002BCA RID: 11210 RVA: 0x000BAE1C File Offset: 0x000B901C
		public static bool isPlayerInGroupExitQueue(Player player)
		{
			using (List<QueuedGroupExit>.Enumerator enumerator = GroupManager.queuedExits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.playerID == player.channel.owner.playerID.steamID)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Add player to exit queue if enabled, or immediately remove.
		/// </summary>
		// Token: 0x06002BCB RID: 11211 RVA: 0x000BAE90 File Offset: 0x000B9090
		public static void requestGroupExit(Player player)
		{
			uint timer_Leave_Group = Provider.modeConfigData.Gameplay.Timer_Leave_Group;
			if (timer_Leave_Group <= 0U)
			{
				GroupManager.alertGroupmatesLeft(player);
				player.quests.leaveGroup(false);
				return;
			}
			if (GroupManager.isPlayerInGroupExitQueue(player))
			{
				return;
			}
			GroupManager.alertGroupmatesTimer(player, timer_Leave_Group);
			QueuedGroupExit queuedGroupExit = new QueuedGroupExit();
			queuedGroupExit.playerID = player.channel.owner.playerID.steamID;
			queuedGroupExit.groupId = player.quests.groupID;
			queuedGroupExit.remainingSeconds = timer_Leave_Group;
			GroupManager.queuedExits.Add(queuedGroupExit);
		}

		/// <summary>
		/// Remove player from queue if they're waiting to exit their group.
		/// </summary>
		// Token: 0x06002BCC RID: 11212 RVA: 0x000BAF1C File Offset: 0x000B911C
		public static void cancelGroupExit(Player player)
		{
			for (int i = GroupManager.queuedExits.Count - 1; i >= 0; i--)
			{
				if (GroupManager.queuedExits[i].playerID == player.channel.owner.playerID.steamID)
				{
					GroupManager.queuedExits.RemoveAtFast(i);
					return;
				}
			}
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000BAF78 File Offset: 0x000B9178
		public static void CancelAllQueuedExitsForGroup(CSteamID groupId)
		{
			for (int i = GroupManager.queuedExits.Count - 1; i >= 0; i--)
			{
				if (GroupManager.queuedExits[i].groupId == groupId)
				{
					GroupManager.queuedExits.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000BAFC0 File Offset: 0x000B91C0
		private static void serverSendMessageToGroupmates(Player player, string message)
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.player == null) && steamPlayer.player.quests.isMemberOfSameGroupAs(player))
				{
					ChatManager.serverSendMessage(message, Color.yellow, null, steamPlayer, EChatMode.SAY, null, false);
				}
			}
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000BB03C File Offset: 0x000B923C
		private static void alertGroupmatesTimer(Player player, uint remainingSeconds)
		{
			string playerName = player.channel.owner.playerID.playerName;
			string message = Provider.localization.format("Player_Group_Queue_Leave", playerName, remainingSeconds);
			GroupManager.serverSendMessageToGroupmates(player, message);
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000BB080 File Offset: 0x000B9280
		private static void alertGroupmatesLeft(Player player)
		{
			string playerName = player.channel.owner.playerID.playerName;
			string message = Provider.localization.format("Player_Group_Left", playerName);
			GroupManager.serverSendMessageToGroupmates(player, message);
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000BB0BC File Offset: 0x000B92BC
		private void tickGroupExitQueue(float deltaTime)
		{
			for (int i = GroupManager.queuedExits.Count - 1; i >= 0; i--)
			{
				QueuedGroupExit queuedGroupExit = GroupManager.queuedExits[i];
				queuedGroupExit.remainingSeconds -= deltaTime;
				if (queuedGroupExit.remainingSeconds <= 0f)
				{
					GroupManager.queuedExits.RemoveAtFast(i);
					Player player = PlayerTool.getPlayer(queuedGroupExit.playerID);
					if (!(player == null) && !(player.quests.groupID != queuedGroupExit.groupId))
					{
						GroupManager.alertGroupmatesLeft(player);
						player.quests.leaveGroup(false);
					}
				}
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000BB151 File Offset: 0x000B9351
		private void Update()
		{
			if (Provider.isServer && GroupManager.queuedExits != null && GroupManager.queuedExits.Count > 0)
			{
				this.tickGroupExitQueue(Time.deltaTime);
			}
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000BB17C File Offset: 0x000B937C
		public static void load()
		{
			if (LevelSavedata.fileExists("/Groups.dat"))
			{
				River river = LevelSavedata.openRiver("/Groups.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					GroupManager.availableGroupID = river.readSteamID();
					if (b < 3)
					{
						GroupManager.availableGroupID.SetEUniverse(EUniverse.k_EUniversePublic);
						GroupManager.availableGroupID.SetEAccountType(EAccountType.k_EAccountTypeConsoleUser);
					}
					if (b > 1)
					{
						uint num = GroupManager.availableGroupID.GetAccountID().m_AccountID;
						int num2 = river.readInt32();
						for (int i = 0; i < num2; i++)
						{
							CSteamID csteamID = river.readSteamID();
							string text = river.readString();
							uint num3 = river.readUInt32();
							if (num3 >= 1U && !string.IsNullOrEmpty(text) && !GroupManager.knownGroups.ContainsKey(csteamID))
							{
								num = MathfEx.Max(num, csteamID.GetAccountID().m_AccountID + 1U);
								GroupManager.knownGroups.Add(csteamID, new GroupInfo(csteamID, text, num3));
							}
						}
						GroupManager.availableGroupID.SetAccountID(new AccountID_t(num));
					}
				}
				river.closeRiver();
			}
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000BB27C File Offset: 0x000B947C
		public static void save()
		{
			uint num = GroupManager.availableGroupID.GetAccountID().m_AccountID;
			Dictionary<CSteamID, GroupInfo>.ValueCollection values = GroupManager.knownGroups.Values;
			List<GroupInfo> list = new List<GroupInfo>();
			foreach (GroupInfo groupInfo in values)
			{
				if (groupInfo.members >= 1U && !string.IsNullOrEmpty(groupInfo.name))
				{
					num = MathfEx.Max(num, groupInfo.groupID.GetAccountID().m_AccountID + 1U);
					list.Add(groupInfo);
				}
			}
			GroupManager.availableGroupID.SetAccountID(new AccountID_t(num));
			River river = LevelSavedata.openRiver("/Groups.dat", false);
			river.writeByte(GroupManager.SAVEDATA_VERSION);
			river.writeSteamID(GroupManager.availableGroupID);
			river.writeInt32(list.Count);
			foreach (GroupInfo groupInfo2 in list)
			{
				river.writeSteamID(groupInfo2.groupID);
				river.writeString(groupInfo2.name);
				river.writeUInt32(groupInfo2.members);
			}
			river.closeRiver();
		}

		// Token: 0x04001756 RID: 5974
		public static readonly byte SAVEDATA_VERSION = 3;

		// Token: 0x04001757 RID: 5975
		private static GroupManager manager;

		// Token: 0x04001759 RID: 5977
		private static CSteamID availableGroupID;

		// Token: 0x0400175A RID: 5978
		private static Dictionary<CSteamID, GroupInfo> knownGroups;

		// Token: 0x0400175B RID: 5979
		private static List<QueuedGroupExit> queuedExits;

		// Token: 0x0400175C RID: 5980
		private static readonly ClientStaticMethod<CSteamID, string, uint> SendGroupInfo = ClientStaticMethod<CSteamID, string, uint>.Get(new ClientStaticMethod<CSteamID, string, uint>.ReceiveDelegate(GroupManager.ReceiveGroupInfo));
	}
}
