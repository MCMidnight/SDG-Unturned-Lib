using System;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Information about a game server retrieved through Steam's "A2S" query system.
	/// Available when joining using the Steam server list API (in-game server browser)
	/// or querying the Server's A2S port directly (connect by IP menu), but not when
	/// joining by Steam ID.
	/// </summary>
	// Token: 0x020006B6 RID: 1718
	public class SteamServerAdvertisement
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x0010D164 File Offset: 0x0010B364
		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x0010D16C File Offset: 0x0010B36C
		public uint ip
		{
			get
			{
				return this._ip;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x0010D174 File Offset: 0x0010B374
		public string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x0010D17C File Offset: 0x0010B37C
		public string map
		{
			get
			{
				return this._map;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x0010D184 File Offset: 0x0010B384
		public bool isPvP
		{
			get
			{
				return this._isPvP;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x0010D18C File Offset: 0x0010B38C
		public bool hasCheats
		{
			get
			{
				return this._hasCheats;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x0010D194 File Offset: 0x0010B394
		public bool isWorkshop
		{
			get
			{
				return this._isWorkshop;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x0010D19C File Offset: 0x0010B39C
		public EGameMode mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x0010D1A4 File Offset: 0x0010B3A4
		public ECameraMode cameraMode
		{
			get
			{
				return this._cameraMode;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x0010D1AC File Offset: 0x0010B3AC
		// (set) Token: 0x06003955 RID: 14677 RVA: 0x0010D1B4 File Offset: 0x0010B3B4
		public EServerMonetizationTag monetization { get; private set; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06003956 RID: 14678 RVA: 0x0010D1BD File Offset: 0x0010B3BD
		public int ping
		{
			get
			{
				return this._ping;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x0010D1C5 File Offset: 0x0010B3C5
		public int players
		{
			get
			{
				return this._players;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003958 RID: 14680 RVA: 0x0010D1CD File Offset: 0x0010B3CD
		public int maxPlayers
		{
			get
			{
				return this._maxPlayers;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x0010D1D5 File Offset: 0x0010B3D5
		public bool isPassworded
		{
			get
			{
				return this._isPassworded;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x0010D1DD File Offset: 0x0010B3DD
		// (set) Token: 0x0600395B RID: 14683 RVA: 0x0010D1E5 File Offset: 0x0010B3E5
		public bool IsVACSecure { get; private set; }

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x0600395C RID: 14684 RVA: 0x0010D1EE File Offset: 0x0010B3EE
		// (set) Token: 0x0600395D RID: 14685 RVA: 0x0010D1F6 File Offset: 0x0010B3F6
		public bool IsBattlEyeSecure { get; private set; }

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x0600395E RID: 14686 RVA: 0x0010D1FF File Offset: 0x0010B3FF
		public bool isPro
		{
			get
			{
				return this._isPro;
			}
		}

		/// <summary>
		/// ID of network transport implementation to use.
		/// </summary>
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x0010D207 File Offset: 0x0010B407
		// (set) Token: 0x06003960 RID: 14688 RVA: 0x0010D20F File Offset: 0x0010B40F
		public string networkTransport { get; protected set; }

		/// <summary>
		/// Known plugin systems.
		/// </summary>
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x0010D218 File Offset: 0x0010B418
		// (set) Token: 0x06003962 RID: 14690 RVA: 0x0010D220 File Offset: 0x0010B420
		public SteamServerAdvertisement.EPluginFramework pluginFramework { get; protected set; }

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x0010D229 File Offset: 0x0010B429
		// (set) Token: 0x06003964 RID: 14692 RVA: 0x0010D231 File Offset: 0x0010B431
		public string thumbnailURL { get; protected set; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x0010D23A File Offset: 0x0010B43A
		// (set) Token: 0x06003966 RID: 14694 RVA: 0x0010D242 File Offset: 0x0010B442
		public string descText { get; protected set; }

		/// <summary>
		/// Probably just checks whether IP is link-local, but may as well use Steam's utility function.
		/// </summary>
		// Token: 0x06003967 RID: 14695 RVA: 0x0010D24B File Offset: 0x0010B44B
		public bool IsAddressUsingSteamFakeIP()
		{
			return SteamNetworkingUtils.IsFakeIPv4(this.ip);
		}

		/// <summary>
		/// Active player count divided by max player count.
		/// </summary>
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x0010D258 File Offset: 0x0010B458
		internal float NormalizedPlayerCount
		{
			get
			{
				if (this._maxPlayers <= 0)
				{
					return 0f;
				}
				return Mathf.Clamp01((float)this._players / (float)this._maxPlayers);
			}
		}

		/// <summary>
		/// Nelson 2024-08-20: This score is intended to prioritize low ping without making it the be-all end-all. The
		/// old default of sorting by ping could put near-empty servers at the top of the list, and encouraged using
		/// anycast caching to make the server appear as low-ping as possible.
		/// </summary>
		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x0010D27D File Offset: 0x0010B47D
		private float PingUtilityScore
		{
			get
			{
				return SteamServerAdvertisement.pingCurve.Evaluate((float)this.sortingPing);
			}
		}

		/// <summary>
		/// Nelson 2024-08-20: This score is intended to prioritize servers around 75% capacity. My thought process is
		/// that near-empty and near-full servers are already easy to find, but typically if you want to play online you
		/// want a server with space for you and your friends. Unfortunately, servers with plenty of players but an even
		/// higher max players make a 50% score plenty good.
		/// </summary>
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x0010D290 File Offset: 0x0010B490
		private float FullnessUtilityScore
		{
			get
			{
				int num = Mathf.Clamp(this._maxPlayers, 1, 100);
				float time = Mathf.Clamp01((float)this._players / (float)num);
				return SteamServerAdvertisement.fullnessCurve.Evaluate(time);
			}
		}

		/// <summary>
		/// Nelson 2024-08-20: This score is intended to balance out the downside of the fullness score decreasing for
		/// servers with very high max player counts, and over-scoring servers with low max players.
		/// </summary>
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		private float PlayerCountUtilityScore
		{
			get
			{
				return SteamServerAdvertisement.playerCountCurve.Evaluate((float)this._players);
			}
		}

		/// <summary>
		/// Called before inserting to server list.
		/// </summary>
		// Token: 0x0600396C RID: 14700 RVA: 0x0010D2DA File Offset: 0x0010B4DA
		internal void CalculateUtilityScore()
		{
			this.utilityScore = this.PingUtilityScore * this.FullnessUtilityScore * this.PlayerCountUtilityScore;
		}

		/// <summary>
		/// Parses value between two keys <stuff>thing</stuff> would parse thing
		/// </summary>
		// Token: 0x0600396D RID: 14701 RVA: 0x0010D2F8 File Offset: 0x0010B4F8
		protected string parseTagValue(string tags, string startKey, string endKey)
		{
			int num = tags.IndexOf(startKey);
			if (num == -1)
			{
				return null;
			}
			num += startKey.Length;
			int num2 = tags.IndexOf(endKey, num);
			if (num2 == -1)
			{
				return null;
			}
			if (num2 == num)
			{
				return null;
			}
			return tags.Substring(num, num2 - num);
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x0010D33C File Offset: 0x0010B53C
		protected bool hasTagKey(string tags, string key, int thumbnailIndex)
		{
			int num = tags.IndexOf(key);
			return num != -1 && (thumbnailIndex == -1 || num < thumbnailIndex);
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x0010D364 File Offset: 0x0010B564
		public SteamServerAdvertisement(gameserveritem_t data, SteamServerAdvertisement.EInfoSource infoSource)
		{
			this._steamID = data.m_steamID;
			this._ip = data.m_NetAdr.GetIP();
			this.queryPort = data.m_NetAdr.GetQueryPort();
			this.connectionPort = this.queryPort + 1;
			this._name = data.GetServerName();
			ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref this._name);
			this._map = data.GetMap();
			string gameTags = data.GetGameTags();
			if (gameTags.Length > 0)
			{
				int thumbnailIndex = gameTags.IndexOf("<tn>");
				this._isPvP = this.hasTagKey(gameTags, "PVP", thumbnailIndex);
				this._hasCheats = this.hasTagKey(gameTags, "CHy", thumbnailIndex);
				this._isWorkshop = this.hasTagKey(gameTags, "WSy", thumbnailIndex);
				if (this.hasTagKey(gameTags, Provider.getModeTagAbbreviation(EGameMode.EASY), thumbnailIndex))
				{
					this._mode = EGameMode.EASY;
				}
				else if (this.hasTagKey(gameTags, Provider.getModeTagAbbreviation(EGameMode.HARD), thumbnailIndex))
				{
					this._mode = EGameMode.HARD;
				}
				else
				{
					this._mode = EGameMode.NORMAL;
				}
				if (this.hasTagKey(gameTags, Provider.getCameraModeTagAbbreviation(ECameraMode.FIRST), thumbnailIndex))
				{
					this._cameraMode = ECameraMode.FIRST;
				}
				else if (this.hasTagKey(gameTags, Provider.getCameraModeTagAbbreviation(ECameraMode.THIRD), thumbnailIndex))
				{
					this._cameraMode = ECameraMode.THIRD;
				}
				else if (this.hasTagKey(gameTags, Provider.getCameraModeTagAbbreviation(ECameraMode.BOTH), thumbnailIndex))
				{
					this._cameraMode = ECameraMode.BOTH;
				}
				else
				{
					this._cameraMode = ECameraMode.VEHICLE;
				}
				if (this.hasTagKey(gameTags, Provider.GetMonetizationTagAbbreviation(EServerMonetizationTag.None), thumbnailIndex))
				{
					this.monetization = EServerMonetizationTag.None;
				}
				else if (this.hasTagKey(gameTags, Provider.GetMonetizationTagAbbreviation(EServerMonetizationTag.NonGameplay), thumbnailIndex))
				{
					this.monetization = EServerMonetizationTag.NonGameplay;
				}
				else if (this.hasTagKey(gameTags, Provider.GetMonetizationTagAbbreviation(EServerMonetizationTag.Monetized), thumbnailIndex))
				{
					this.monetization = EServerMonetizationTag.Monetized;
				}
				else
				{
					this.monetization = EServerMonetizationTag.Unspecified;
				}
				this._isPro = this.hasTagKey(gameTags, "GLD", thumbnailIndex);
				this.IsBattlEyeSecure = this.hasTagKey(gameTags, "BEy", thumbnailIndex);
				this.networkTransport = this.parseTagValue(gameTags, "<net>", "</net>");
				if (string.IsNullOrEmpty(this.networkTransport))
				{
					UnturnedLog.warn("Unable to parse net transport tag for server \"{0}\" from \"{1}\"", new object[]
					{
						this.name,
						gameTags
					});
				}
				string text = this.parseTagValue(gameTags, "<pf>", "</pf>");
				if (string.IsNullOrEmpty(text))
				{
					if (data.m_nBotPlayers == 1)
					{
						this.pluginFramework = SteamServerAdvertisement.EPluginFramework.Rocket;
					}
					else
					{
						this.pluginFramework = SteamServerAdvertisement.EPluginFramework.None;
					}
				}
				else if (text.Equals("rm"))
				{
					this.pluginFramework = SteamServerAdvertisement.EPluginFramework.Rocket;
				}
				else if (text.Equals("om"))
				{
					this.pluginFramework = SteamServerAdvertisement.EPluginFramework.OpenMod;
				}
				else
				{
					this.pluginFramework = SteamServerAdvertisement.EPluginFramework.Unknown;
				}
				this.thumbnailURL = this.parseTagValue(gameTags, "<tn>", "</tn>");
				string text2 = data.GetGameDescription();
				if (!RichTextUtil.IsTextValidForServerListShortDescription(text2))
				{
					text2 = null;
				}
				else
				{
					ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref text2);
				}
				if (StringExtension.ContainsNewLine(text2) || StringExtension.ContainsChar(text2, '\t'))
				{
					text2 = null;
					UnturnedLog.warn("Control characters not allowed in server \"" + this.name + "\" description");
				}
				this.descText = text2;
			}
			else
			{
				this._isPvP = true;
				this._hasCheats = false;
				this._mode = EGameMode.NORMAL;
				this._cameraMode = ECameraMode.FIRST;
				this.monetization = EServerMonetizationTag.Unspecified;
				this._isPro = true;
				this.IsBattlEyeSecure = false;
				this.networkTransport = null;
				this.pluginFramework = SteamServerAdvertisement.EPluginFramework.None;
				this.thumbnailURL = null;
				this.descText = null;
			}
			this._ping = data.m_nPing;
			this.sortingPing = this._ping;
			this._maxPlayers = data.m_nMaxPlayers;
			if (data.m_nPlayers < 0 || data.m_nBotPlayers < 0 || data.m_nPlayers > 255 || data.m_nBotPlayers > 255)
			{
				this._players = 0;
			}
			else
			{
				this._players = Mathf.Max(0, data.m_nPlayers - data.m_nBotPlayers);
			}
			this._isPassworded = data.m_bPassword;
			this.IsVACSecure = data.m_bSecure;
			this.infoSource = infoSource;
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x0010D72B File Offset: 0x0010B92B
		public SteamServerAdvertisement(string newName, EGameMode newMode, bool newVACSecure, bool newBattlEyeEnabled, bool newPro)
		{
			this._name = newName;
			ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref this._name);
			this._mode = newMode;
			this.IsVACSecure = newVACSecure;
			this.IsBattlEyeSecure = newBattlEyeEnabled;
			this._isPro = newPro;
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x0010D768 File Offset: 0x0010B968
		public SteamServerAdvertisement(CSteamID steamId)
		{
			this._steamID = steamId;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x0010D778 File Offset: 0x0010B978
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Name: ",
				this.name,
				" Map: ",
				this.map,
				" PvP: ",
				this.isPvP.ToString(),
				" Mode: ",
				this.mode.ToString(),
				" Ping: ",
				this.ping.ToString(),
				" Players: ",
				this.players.ToString(),
				"/",
				this.maxPlayers.ToString(),
				" Passworded: ",
				this.isPassworded.ToString()
			});
		}

		// Token: 0x040021F9 RID: 8697
		private CSteamID _steamID;

		// Token: 0x040021FA RID: 8698
		private uint _ip;

		// Token: 0x040021FB RID: 8699
		public ushort queryPort;

		// Token: 0x040021FC RID: 8700
		public ushort connectionPort;

		// Token: 0x040021FD RID: 8701
		private string _name;

		// Token: 0x040021FE RID: 8702
		private string _map;

		// Token: 0x040021FF RID: 8703
		private bool _isPvP;

		// Token: 0x04002200 RID: 8704
		private bool _hasCheats;

		// Token: 0x04002201 RID: 8705
		private bool _isWorkshop;

		// Token: 0x04002202 RID: 8706
		private EGameMode _mode;

		// Token: 0x04002203 RID: 8707
		private ECameraMode _cameraMode;

		// Token: 0x04002205 RID: 8709
		private int _ping;

		// Token: 0x04002206 RID: 8710
		internal int sortingPing;

		// Token: 0x04002207 RID: 8711
		private int _players;

		// Token: 0x04002208 RID: 8712
		private int _maxPlayers;

		// Token: 0x04002209 RID: 8713
		private bool _isPassworded;

		// Token: 0x0400220C RID: 8716
		private bool _isPro;

		// Token: 0x04002211 RID: 8721
		internal float utilityScore;

		// Token: 0x04002212 RID: 8722
		internal SteamServerAdvertisement.EInfoSource infoSource;

		// Token: 0x04002213 RID: 8723
		private static AnimationCurve pingCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(50f, 1f),
			new Keyframe(100f, 0.6f),
			new Keyframe(300f, 0.3f),
			new Keyframe(900f, 0.1f)
		});

		// Token: 0x04002214 RID: 8724
		private static AnimationCurve fullnessCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.1f),
			new Keyframe(0.5f, 0.8f),
			new Keyframe(0.75f, 1f),
			new Keyframe(1f, 0.8f)
		});

		// Token: 0x04002215 RID: 8725
		private static AnimationCurve playerCountCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(2f, 0.1f),
			new Keyframe(18f, 0.8f),
			new Keyframe(64f, 1f)
		});

		// Token: 0x020009E6 RID: 2534
		public enum EPluginFramework
		{
			// Token: 0x04003468 RID: 13416
			None,
			// Token: 0x04003469 RID: 13417
			Rocket,
			// Token: 0x0400346A RID: 13418
			OpenMod,
			// Token: 0x0400346B RID: 13419
			Unknown
		}

		// Token: 0x020009E7 RID: 2535
		public enum EInfoSource
		{
			/// <summary>
			/// Join server by IP.
			/// </summary>
			// Token: 0x0400346D RID: 13421
			DirectConnect,
			// Token: 0x0400346E RID: 13422
			InternetServerList,
			// Token: 0x0400346F RID: 13423
			FavoriteServerList,
			// Token: 0x04003470 RID: 13424
			FriendServerList,
			// Token: 0x04003471 RID: 13425
			HistoryServerList,
			// Token: 0x04003472 RID: 13426
			LanServerList
		}
	}
}
