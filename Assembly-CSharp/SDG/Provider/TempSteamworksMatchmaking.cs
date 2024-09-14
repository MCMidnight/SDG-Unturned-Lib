using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Provider
{
	// Token: 0x02000036 RID: 54
	public class TempSteamworksMatchmaking
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600016A RID: 362 RVA: 0x00006870 File Offset: 0x00004A70
		// (remove) Token: 0x0600016B RID: 363 RVA: 0x000068A8 File Offset: 0x00004AA8
		public event TempSteamworksMatchmaking.AttemptUpdated onAttemptUpdated;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600016C RID: 364 RVA: 0x000068E0 File Offset: 0x00004AE0
		// (remove) Token: 0x0600016D RID: 365 RVA: 0x00006918 File Offset: 0x00004B18
		public event TempSteamworksMatchmaking.TimedOut onTimedOut;

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000694D File Offset: 0x00004B4D
		public ESteamServerList currentList
		{
			get
			{
				return this._currentList;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00006955 File Offset: 0x00004B55
		public List<SteamServerAdvertisement> serverList
		{
			get
			{
				return this._serverList;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000695D File Offset: 0x00004B5D
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00006965 File Offset: 0x00004B65
		public bool isAttemptingServerQuery { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000696E File Offset: 0x00004B6E
		public IComparer<SteamServerAdvertisement> serverInfoComparer
		{
			get
			{
				return this._serverInfoComparer;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006976 File Offset: 0x00004B76
		public void sortMasterServer(IComparer<SteamServerAdvertisement> newServerInfoComparer)
		{
			this._serverInfoComparer = newServerInfoComparer;
			this.serverList.Sort(this.serverInfoComparer);
			TempSteamworksMatchmaking.MasterServerResorted masterServerResorted = this.onMasterServerResorted;
			if (masterServerResorted == null)
			{
				return;
			}
			masterServerResorted();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000069A0 File Offset: 0x00004BA0
		private void cleanupServerQuery()
		{
			if (this.serverQuery == HServerQuery.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.CancelServerQuery(this.serverQuery);
			this.serverQuery = HServerQuery.Invalid;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000069CB File Offset: 0x00004BCB
		private void cleanupPlayersQuery()
		{
			if (this.playersQuery == HServerQuery.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.CancelServerQuery(this.playersQuery);
			this.playersQuery = HServerQuery.Invalid;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000069F6 File Offset: 0x00004BF6
		private void cleanupRulesQuery()
		{
			if (this.rulesQuery == HServerQuery.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.CancelServerQuery(this.rulesQuery);
			this.rulesQuery = HServerQuery.Invalid;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006A21 File Offset: 0x00004C21
		private void cleanupServerListRequest()
		{
			if (this.serverListRequest == HServerListRequest.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.ReleaseRequest(this.serverListRequest);
			this.serverListRequest = HServerListRequest.Invalid;
			this.serverListRefreshIndex = -1;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006A53 File Offset: 0x00004C53
		public void connect(SteamConnectionInfo info)
		{
			if (Provider.isConnected)
			{
				return;
			}
			this.connectionInfo = info;
			this.serverQueryAttempts = 0;
			this.isAttemptingServerQuery = true;
			this.autoJoinServerQuery = false;
			this.serverQueryContext = MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT;
			this.attemptServerQuery();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006A86 File Offset: 0x00004C86
		public void cancel()
		{
			if (!this.isAttemptingServerQuery)
			{
				return;
			}
			this.serverQueryAttempts = 99;
			this.onPingFailedToRespond();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006AA0 File Offset: 0x00004CA0
		private void attemptServerQuery()
		{
			this.cleanupServerQuery();
			this.serverQuery = SteamMatchmakingServers.PingServer(this.connectionInfo.ip, this.connectionInfo.port, this.serverPingResponse);
			this.serverQueryAttempts++;
			TempSteamworksMatchmaking.AttemptUpdated attemptUpdated = this.onAttemptUpdated;
			if (attemptUpdated == null)
			{
				return;
			}
			attemptUpdated(this.serverQueryAttempts);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006AFE File Offset: 0x00004CFE
		public void cancelRequest()
		{
			if (this.serverListRequest != HServerListRequest.Invalid)
			{
				SteamMatchmakingServers.CancelQuery(this.serverListRequest);
				this.cleanupServerListRequest();
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006B24 File Offset: 0x00004D24
		public void refreshMasterServer(ServerListFilters inputFilters)
		{
			this._currentList = inputFilters.listSource;
			this.currentPluginsFilter = inputFilters.plugins;
			this.currentNameFilter = inputFilters.serverName;
			this.isCurrentNameFilterSet = !string.IsNullOrEmpty(this.currentNameFilter);
			this.currentNameRegex = null;
			string text = "regex:";
			if (this.isCurrentNameFilterSet && this.currentNameFilter.StartsWith(text))
			{
				string text2 = this.currentNameFilter.Substring(text.Length);
				try
				{
					this.currentNameRegex = new Regex(text2);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Caught exception parsing server name regex \"" + text2 + "\":");
					this.isCurrentNameFilterSet = false;
				}
			}
			this.currentMaxPingFilter = inputFilters.maxPing;
			this._serverList.Clear();
			TempSteamworksMatchmaking.MasterServerRemoved masterServerRemoved = this.onMasterServerRemoved;
			if (masterServerRemoved != null)
			{
				masterServerRemoved();
			}
			this.cleanupServerListRequest();
			if (inputFilters.listSource == ESteamServerList.LAN)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestLANServerList(Provider.APP_ID, this.serverListResponse);
				return;
			}
			this.filters = new List<MatchMakingKeyValuePair_t>();
			MatchMakingKeyValuePair_t matchMakingKeyValuePair_t = default(MatchMakingKeyValuePair_t);
			matchMakingKeyValuePair_t.m_szKey = "gamedir";
			matchMakingKeyValuePair_t.m_szValue = "unturned";
			this.filters.Add(matchMakingKeyValuePair_t);
			if (inputFilters.mapNames != null && inputFilters.mapNames.Count > 0)
			{
				List<LevelInfo> list = new List<LevelInfo>();
				inputFilters.GetLevels(list);
				if (list.Count > 0)
				{
					if (list.Count > 1)
					{
						int num = list.Count * 3;
						this.filters.Add(new MatchMakingKeyValuePair_t
						{
							m_szKey = "or",
							m_szValue = num.ToString()
						});
					}
					foreach (LevelInfo levelInfo in list)
					{
						this.filters.Add(new MatchMakingKeyValuePair_t
						{
							m_szKey = "and",
							m_szValue = "2"
						});
						MatchMakingKeyValuePair_t matchMakingKeyValuePair_t2 = default(MatchMakingKeyValuePair_t);
						matchMakingKeyValuePair_t2.m_szKey = "map";
						matchMakingKeyValuePair_t2.m_szValue = levelInfo.name.ToLower();
						this.filters.Add(matchMakingKeyValuePair_t2);
						MatchMakingKeyValuePair_t matchMakingKeyValuePair_t3 = default(MatchMakingKeyValuePair_t);
						matchMakingKeyValuePair_t3.m_szKey = "gamedataand";
						matchMakingKeyValuePair_t3.m_szValue = "MAP_VERSION_" + VersionUtils.binaryToHexadecimal(levelInfo.configData.PackedVersion);
						this.filters.Add(matchMakingKeyValuePair_t3);
					}
				}
			}
			if (inputFilters.attendance == EAttendance.Empty)
			{
				MatchMakingKeyValuePair_t matchMakingKeyValuePair_t4 = default(MatchMakingKeyValuePair_t);
				matchMakingKeyValuePair_t4.m_szKey = "noplayers";
				matchMakingKeyValuePair_t4.m_szValue = "1";
				this.filters.Add(matchMakingKeyValuePair_t4);
			}
			else if (inputFilters.attendance == EAttendance.HasPlayers)
			{
				MatchMakingKeyValuePair_t matchMakingKeyValuePair_t5 = default(MatchMakingKeyValuePair_t);
				matchMakingKeyValuePair_t5.m_szKey = "hasplayers";
				matchMakingKeyValuePair_t5.m_szValue = "1";
				this.filters.Add(matchMakingKeyValuePair_t5);
			}
			if (inputFilters.notFull)
			{
				MatchMakingKeyValuePair_t matchMakingKeyValuePair_t6 = default(MatchMakingKeyValuePair_t);
				matchMakingKeyValuePair_t6.m_szKey = "notfull";
				matchMakingKeyValuePair_t6.m_szValue = "1";
				this.filters.Add(matchMakingKeyValuePair_t6);
			}
			MatchMakingKeyValuePair_t matchMakingKeyValuePair_t7 = default(MatchMakingKeyValuePair_t);
			matchMakingKeyValuePair_t7.m_szKey = "gamedataand";
			if (inputFilters.password == EPassword.YES)
			{
				matchMakingKeyValuePair_t7.m_szValue = "PASS";
			}
			else if (inputFilters.password == EPassword.NO)
			{
				matchMakingKeyValuePair_t7.m_szValue = "SSAP";
			}
			if (inputFilters.vacProtection == EVACProtectionFilter.Secure)
			{
				matchMakingKeyValuePair_t7.m_szValue += ",";
				matchMakingKeyValuePair_t7.m_szValue += "VAC_ON";
				MatchMakingKeyValuePair_t matchMakingKeyValuePair_t8 = default(MatchMakingKeyValuePair_t);
				matchMakingKeyValuePair_t8.m_szKey = "secure";
				matchMakingKeyValuePair_t8.m_szValue = "1";
				this.filters.Add(matchMakingKeyValuePair_t8);
			}
			else if (inputFilters.vacProtection == EVACProtectionFilter.Insecure)
			{
				matchMakingKeyValuePair_t7.m_szValue += ",";
				matchMakingKeyValuePair_t7.m_szValue += "VAC_OFF";
			}
			matchMakingKeyValuePair_t7.m_szValue += ",GAME_VERSION_";
			matchMakingKeyValuePair_t7.m_szValue += VersionUtils.binaryToHexadecimal(Provider.APP_VERSION_PACKED);
			if (!string.IsNullOrEmpty(matchMakingKeyValuePair_t7.m_szValue))
			{
				this.filters.Add(matchMakingKeyValuePair_t7);
			}
			MatchMakingKeyValuePair_t matchMakingKeyValuePair_t9 = default(MatchMakingKeyValuePair_t);
			matchMakingKeyValuePair_t9.m_szKey = "gametagsand";
			if (inputFilters.workshop == EWorkshop.YES)
			{
				matchMakingKeyValuePair_t9.m_szValue = "WSy";
			}
			else if (inputFilters.workshop == EWorkshop.NO)
			{
				matchMakingKeyValuePair_t9.m_szValue = "WSn";
			}
			if (inputFilters.combat == ECombat.PVP)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",PVP";
			}
			else if (inputFilters.combat == ECombat.PVE)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",PVE";
			}
			if (inputFilters.cheats == ECheats.YES)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",CHy";
			}
			else if (inputFilters.cheats == ECheats.NO)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",CHn";
			}
			if (inputFilters.camera != ECameraMode.ANY)
			{
				matchMakingKeyValuePair_t9.m_szValue = matchMakingKeyValuePair_t9.m_szValue + "," + Provider.getCameraModeTagAbbreviation(inputFilters.camera);
			}
			if (inputFilters.monetization == EServerMonetizationTag.None)
			{
				matchMakingKeyValuePair_t9.m_szValue = matchMakingKeyValuePair_t9.m_szValue + "," + Provider.GetMonetizationTagAbbreviation(inputFilters.monetization);
			}
			else if (inputFilters.monetization == EServerMonetizationTag.NonGameplay)
			{
				this.filters.Add(new MatchMakingKeyValuePair_t
				{
					m_szKey = "or",
					m_szValue = "2"
				});
				this.filters.Add(new MatchMakingKeyValuePair_t
				{
					m_szKey = "gametagsand",
					m_szValue = Provider.GetMonetizationTagAbbreviation(EServerMonetizationTag.None)
				});
				this.filters.Add(new MatchMakingKeyValuePair_t
				{
					m_szKey = "gametagsand",
					m_szValue = Provider.GetMonetizationTagAbbreviation(EServerMonetizationTag.NonGameplay)
				});
			}
			if (inputFilters.gold == EServerListGoldFilter.RequiresGold)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",GLD";
			}
			else if (inputFilters.gold == EServerListGoldFilter.DoesNotRequireGold)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",F2P";
			}
			if (inputFilters.battlEyeProtection == EBattlEyeProtectionFilter.Secure)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",BEy";
			}
			else if (inputFilters.battlEyeProtection == EBattlEyeProtectionFilter.Insecure)
			{
				matchMakingKeyValuePair_t9.m_szValue += ",BEn";
			}
			if (!string.IsNullOrEmpty(matchMakingKeyValuePair_t9.m_szValue))
			{
				this.filters.Add(matchMakingKeyValuePair_t9);
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			stringBuilder.Append("Refreshing server list with filters:");
			foreach (MatchMakingKeyValuePair_t matchMakingKeyValuePair_t10 in this.filters)
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(matchMakingKeyValuePair_t10.m_szKey);
				stringBuilder.Append('=');
				stringBuilder.Append(matchMakingKeyValuePair_t10.m_szValue);
			}
			UnturnedLog.info(stringBuilder.ToString());
			if (inputFilters.listSource == ESteamServerList.HISTORY)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestHistoryServerList(Provider.APP_ID, this.filters.ToArray(), (uint)this.filters.Count, this.serverListResponse);
				return;
			}
			if (inputFilters.listSource == ESteamServerList.FAVORITES)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestFavoritesServerList(Provider.APP_ID, this.filters.ToArray(), (uint)this.filters.Count, this.serverListResponse);
				return;
			}
			if (inputFilters.listSource == ESteamServerList.INTERNET)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestInternetServerList(Provider.APP_ID, this.filters.ToArray(), (uint)this.filters.Count, this.serverListResponse);
				return;
			}
			if (inputFilters.listSource == ESteamServerList.FRIENDS)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestFriendsServerList(Provider.APP_ID, this.filters.ToArray(), (uint)this.filters.Count, this.serverListResponse);
				return;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007314 File Offset: 0x00005514
		public void refreshPlayers(uint ip, ushort port)
		{
			this.cleanupPlayersQuery();
			this.playersQuery = SteamMatchmakingServers.PlayerDetails(ip, port, this.playersResponse);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000732F File Offset: 0x0000552F
		public void refreshRules(uint ip, ushort port)
		{
			this.cleanupRulesQuery();
			this.rulesMap = new Dictionary<string, string>();
			this.rulesQuery = SteamMatchmakingServers.ServerRules(ip, port, this.rulesResponse);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007358 File Offset: 0x00005558
		private void onPingResponded(gameserveritem_t data)
		{
			this.isAttemptingServerQuery = false;
			this.cleanupServerQuery();
			if (data.m_nAppID == Provider.APP_ID.m_AppId)
			{
				SteamServerAdvertisement steamServerAdvertisement = new SteamServerAdvertisement(data, SteamServerAdvertisement.EInfoSource.DirectConnect);
				if (!steamServerAdvertisement.isPro || Provider.isPro)
				{
					bool flag = false;
					if (this.autoJoinServerQuery && (!steamServerAdvertisement.isPassworded || !string.IsNullOrEmpty(this.connectionInfo.password)))
					{
						flag = true;
					}
					if (flag)
					{
						Provider.connect(new ServerConnectParameters(new IPv4Address(steamServerAdvertisement.ip), steamServerAdvertisement.queryPort, steamServerAdvertisement.connectionPort, this.connectionInfo.password), steamServerAdvertisement, null);
					}
					else
					{
						MenuUI.closeAll();
						MenuUI.closeAlert();
						MenuPlayServerInfoUI.open(steamServerAdvertisement, this.connectionInfo.password, this.serverQueryContext);
					}
				}
				else
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_SERVER;
				}
			}
			else
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
			}
			TempSteamworksMatchmaking.TimedOut timedOut = this.onTimedOut;
			if (timedOut == null)
			{
				return;
			}
			timedOut();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000743A File Offset: 0x0000563A
		private void onPingFailedToRespond()
		{
			if (this.serverQueryAttempts < 10)
			{
				this.attemptServerQuery();
				return;
			}
			this.isAttemptingServerQuery = false;
			this.cleanupServerQuery();
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
			TempSteamworksMatchmaking.TimedOut timedOut = this.onTimedOut;
			if (timedOut == null)
			{
				return;
			}
			timedOut();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007474 File Offset: 0x00005674
		private void onServerListResponded(HServerListRequest request, int index)
		{
			if (request != this.serverListRequest)
			{
				return;
			}
			gameserveritem_t serverDetails = SteamMatchmakingServers.GetServerDetails(request, index);
			if (this._currentList == ESteamServerList.INTERNET && !serverDetails.m_steamID.BPersistentGameServerAccount())
			{
				UnturnedLog.info("Ignoring server \"" + serverDetails.GetServerName() + "\" because it is anonymous on the internet list");
				return;
			}
			if (serverDetails.m_nAppID != Provider.APP_ID.m_AppId)
			{
				UnturnedLog.info(string.Format("Ignoring server \"{0}\" because it has a different AppID ({1})", serverDetails.GetServerName(), serverDetails.m_nAppID));
				return;
			}
			new IPv4Address(serverDetails.m_NetAdr.GetIP());
			SteamServerAdvertisement.EInfoSource infoSource;
			switch (this._currentList)
			{
			default:
				infoSource = SteamServerAdvertisement.EInfoSource.InternetServerList;
				break;
			case ESteamServerList.LAN:
				infoSource = SteamServerAdvertisement.EInfoSource.LanServerList;
				break;
			case ESteamServerList.HISTORY:
				infoSource = SteamServerAdvertisement.EInfoSource.HistoryServerList;
				break;
			case ESteamServerList.FAVORITES:
				infoSource = SteamServerAdvertisement.EInfoSource.FavoriteServerList;
				break;
			case ESteamServerList.FRIENDS:
				infoSource = SteamServerAdvertisement.EInfoSource.FriendServerList;
				break;
			}
			SteamServerAdvertisement steamServerAdvertisement = new SteamServerAdvertisement(serverDetails, infoSource);
			if (index == this.serverListRefreshIndex)
			{
				TempSteamworksMatchmaking.MasterServerQueryRefreshed masterServerQueryRefreshed = this.onMasterServerQueryRefreshed;
				if (masterServerQueryRefreshed == null)
				{
					return;
				}
				masterServerQueryRefreshed(steamServerAdvertisement);
				return;
			}
			else
			{
				if (this.currentPluginsFilter == EPlugins.NO)
				{
					if (steamServerAdvertisement.pluginFramework != SteamServerAdvertisement.EPluginFramework.None)
					{
						return;
					}
				}
				else if (this.currentPluginsFilter == EPlugins.YES && steamServerAdvertisement.pluginFramework == SteamServerAdvertisement.EPluginFramework.None)
				{
					return;
				}
				if (steamServerAdvertisement.maxPlayers < (int)CommandMaxPlayers.MIN_NUMBER)
				{
					return;
				}
				if (this.isCurrentNameFilterSet)
				{
					if (this.currentNameRegex != null)
					{
						if (!this.currentNameRegex.IsMatch(steamServerAdvertisement.name))
						{
							return;
						}
					}
					else if (steamServerAdvertisement.name.IndexOf(this.currentNameFilter, 5) == -1)
					{
						return;
					}
				}
				else if (Mathf.Max(steamServerAdvertisement.players, steamServerAdvertisement.maxPlayers) > (int)CommandMaxPlayers.MAX_NUMBER)
				{
					return;
				}
				if (this.currentMaxPingFilter > 0 && steamServerAdvertisement.ping > this.currentMaxPingFilter)
				{
					return;
				}
				steamServerAdvertisement.CalculateUtilityScore();
				int num = this.serverList.BinarySearch(steamServerAdvertisement, this.serverInfoComparer);
				if (num < 0)
				{
					num = ~num;
				}
				this.serverList.Insert(num, steamServerAdvertisement);
				TempSteamworksMatchmaking.MasterServerAdded masterServerAdded = this.onMasterServerAdded;
				if (masterServerAdded == null)
				{
					return;
				}
				masterServerAdded(num, steamServerAdvertisement);
				return;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000763F File Offset: 0x0000583F
		private void onServerListFailedToRespond(HServerListRequest request, int index)
		{
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007644 File Offset: 0x00005844
		private void onRefreshComplete(HServerListRequest request, EMatchMakingServerResponse response)
		{
			if (request == this.serverListRequest)
			{
				TempSteamworksMatchmaking.MasterServerRefreshed masterServerRefreshed = this.onMasterServerRefreshed;
				if (masterServerRefreshed != null)
				{
					masterServerRefreshed(response);
				}
				if (response == EMatchMakingServerResponse.eNoServersListedOnMasterServer || this.serverList.Count == 0)
				{
					UnturnedLog.info("No servers found on the master server.");
					return;
				}
				if (response == EMatchMakingServerResponse.eServerFailedToRespond)
				{
					UnturnedLog.error("Failed to connect to the master server.");
					return;
				}
				if (response == EMatchMakingServerResponse.eServerResponded)
				{
					UnturnedLog.info("Successfully refreshed the master server.");
					return;
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000076AA File Offset: 0x000058AA
		private void onAddPlayerToList(string name, int score, float time)
		{
			TempSteamworksMatchmaking.PlayersQueryRefreshed playersQueryRefreshed = this.onPlayersQueryRefreshed;
			if (playersQueryRefreshed == null)
			{
				return;
			}
			playersQueryRefreshed(name, score, time);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000076BF File Offset: 0x000058BF
		private void onPlayersFailedToRespond()
		{
			UnturnedLog.warn("Server players query failed to respond");
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000076CB File Offset: 0x000058CB
		private void onPlayersRefreshComplete()
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000076CD File Offset: 0x000058CD
		private void onRulesResponded(string key, string value)
		{
			if (this.rulesMap == null)
			{
				return;
			}
			this.rulesMap.Add(key, value);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000076E5 File Offset: 0x000058E5
		private void onRulesFailedToRespond()
		{
			UnturnedLog.warn("Server rules query failed to respond");
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000076F1 File Offset: 0x000058F1
		private void onRulesRefreshComplete()
		{
			TempSteamworksMatchmaking.RulesQueryRefreshed rulesQueryRefreshed = this.onRulesQueryRefreshed;
			if (rulesQueryRefreshed == null)
			{
				return;
			}
			rulesQueryRefreshed(this.rulesMap);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000770C File Offset: 0x0000590C
		public TempSteamworksMatchmaking()
		{
			this.serverPingResponse = new ISteamMatchmakingPingResponse(new ISteamMatchmakingPingResponse.ServerResponded(this.onPingResponded), new ISteamMatchmakingPingResponse.ServerFailedToRespond(this.onPingFailedToRespond));
			this.serverListResponse = new ISteamMatchmakingServerListResponse(new ISteamMatchmakingServerListResponse.ServerResponded(this.onServerListResponded), new ISteamMatchmakingServerListResponse.ServerFailedToRespond(this.onServerListFailedToRespond), new ISteamMatchmakingServerListResponse.RefreshComplete(this.onRefreshComplete));
			this.playersResponse = new ISteamMatchmakingPlayersResponse(new ISteamMatchmakingPlayersResponse.AddPlayerToList(this.onAddPlayerToList), new ISteamMatchmakingPlayersResponse.PlayersFailedToRespond(this.onPlayersFailedToRespond), new ISteamMatchmakingPlayersResponse.PlayersRefreshComplete(this.onPlayersRefreshComplete));
			this.rulesResponse = new ISteamMatchmakingRulesResponse(new ISteamMatchmakingRulesResponse.RulesResponded(this.onRulesResponded), new ISteamMatchmakingRulesResponse.RulesFailedToRespond(this.onRulesFailedToRespond), new ISteamMatchmakingRulesResponse.RulesRefreshComplete(this.onRulesRefreshComplete));
		}

		// Token: 0x04000091 RID: 145
		public TempSteamworksMatchmaking.MasterServerAdded onMasterServerAdded;

		// Token: 0x04000092 RID: 146
		public TempSteamworksMatchmaking.MasterServerRemoved onMasterServerRemoved;

		// Token: 0x04000093 RID: 147
		public TempSteamworksMatchmaking.MasterServerResorted onMasterServerResorted;

		// Token: 0x04000094 RID: 148
		public TempSteamworksMatchmaking.MasterServerRefreshed onMasterServerRefreshed;

		// Token: 0x04000095 RID: 149
		public TempSteamworksMatchmaking.MasterServerQueryRefreshed onMasterServerQueryRefreshed;

		// Token: 0x04000098 RID: 152
		public TempSteamworksMatchmaking.PlayersQueryRefreshed onPlayersQueryRefreshed;

		// Token: 0x04000099 RID: 153
		public TempSteamworksMatchmaking.RulesQueryRefreshed onRulesQueryRefreshed;

		// Token: 0x0400009A RID: 154
		private SteamConnectionInfo connectionInfo;

		// Token: 0x0400009B RID: 155
		private ESteamServerList _currentList;

		// Token: 0x0400009C RID: 156
		private string currentNameFilter;

		// Token: 0x0400009D RID: 157
		private Regex currentNameRegex;

		// Token: 0x0400009E RID: 158
		private bool isCurrentNameFilterSet;

		// Token: 0x0400009F RID: 159
		private int currentMaxPingFilter;

		// Token: 0x040000A0 RID: 160
		private EPlugins currentPluginsFilter;

		// Token: 0x040000A1 RID: 161
		private List<SteamServerAdvertisement> _serverList = new List<SteamServerAdvertisement>();

		// Token: 0x040000A2 RID: 162
		private List<MatchMakingKeyValuePair_t> filters;

		// Token: 0x040000A3 RID: 163
		private ISteamMatchmakingPingResponse serverPingResponse;

		// Token: 0x040000A4 RID: 164
		private ISteamMatchmakingServerListResponse serverListResponse;

		// Token: 0x040000A5 RID: 165
		private ISteamMatchmakingPlayersResponse playersResponse;

		// Token: 0x040000A6 RID: 166
		private ISteamMatchmakingRulesResponse rulesResponse;

		// Token: 0x040000A7 RID: 167
		private HServerQuery playersQuery = HServerQuery.Invalid;

		// Token: 0x040000A8 RID: 168
		private HServerQuery rulesQuery = HServerQuery.Invalid;

		// Token: 0x040000A9 RID: 169
		private Dictionary<string, string> rulesMap;

		// Token: 0x040000AA RID: 170
		private HServerQuery serverQuery = HServerQuery.Invalid;

		// Token: 0x040000AB RID: 171
		private int serverQueryAttempts;

		/// <summary>
		/// Reset after starting connection attempt, so set to true afterwards to auto join the server.
		/// </summary>
		// Token: 0x040000AD RID: 173
		public bool autoJoinServerQuery;

		// Token: 0x040000AE RID: 174
		public MenuPlayServerInfoUI.EServerInfoOpenContext serverQueryContext;

		// Token: 0x040000AF RID: 175
		private HServerListRequest serverListRequest = HServerListRequest.Invalid;

		// Token: 0x040000B0 RID: 176
		private int serverListRefreshIndex = -1;

		// Token: 0x040000B1 RID: 177
		private IComparer<SteamServerAdvertisement> _serverInfoComparer = new ServerListComparer_UtilityScore();

		// Token: 0x02000843 RID: 2115
		// (Invoke) Token: 0x06004791 RID: 18321
		public delegate void MasterServerAdded(int insert, SteamServerAdvertisement server);

		// Token: 0x02000844 RID: 2116
		// (Invoke) Token: 0x06004795 RID: 18325
		public delegate void MasterServerRemoved();

		// Token: 0x02000845 RID: 2117
		// (Invoke) Token: 0x06004799 RID: 18329
		public delegate void MasterServerResorted();

		// Token: 0x02000846 RID: 2118
		// (Invoke) Token: 0x0600479D RID: 18333
		public delegate void MasterServerRefreshed(EMatchMakingServerResponse response);

		// Token: 0x02000847 RID: 2119
		// (Invoke) Token: 0x060047A1 RID: 18337
		public delegate void MasterServerQueryRefreshed(SteamServerAdvertisement server);

		// Token: 0x02000848 RID: 2120
		// (Invoke) Token: 0x060047A5 RID: 18341
		public delegate void AttemptUpdated(int attempt);

		// Token: 0x02000849 RID: 2121
		// (Invoke) Token: 0x060047A9 RID: 18345
		public delegate void TimedOut();

		// Token: 0x0200084A RID: 2122
		// (Invoke) Token: 0x060047AD RID: 18349
		public delegate void PlayersQueryRefreshed(string name, int score, float time);

		// Token: 0x0200084B RID: 2123
		// (Invoke) Token: 0x060047B1 RID: 18353
		public delegate void RulesQueryRefreshed(Dictionary<string, string> rulesMap);
	}
}
