using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000695 RID: 1685
	public class SteamPlayerID
	{
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x0010C8FD File Offset: 0x0010AAFD
		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x0010C905 File Offset: 0x0010AB05
		private string streamerName
		{
			get
			{
				if (Provider.streamerNames != null)
				{
					return Provider.streamerNames[(int)(this.steamID.m_SteamID % (ulong)((long)Provider.streamerNames.Count))];
				}
				return string.Empty;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x060038F8 RID: 14584 RVA: 0x0010C936 File Offset: 0x0010AB36
		public string playerName
		{
			get
			{
				if (OptionsSettings.streamer)
				{
					return this.streamerName;
				}
				return this._playerName;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x0010C94C File Offset: 0x0010AB4C
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x0010C962 File Offset: 0x0010AB62
		public string characterName
		{
			get
			{
				if (OptionsSettings.streamer)
				{
					return this.streamerName;
				}
				return this._characterName;
			}
			set
			{
				this._characterName = value;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x060038FB RID: 14587 RVA: 0x0010C96B File Offset: 0x0010AB6B
		// (set) Token: 0x060038FC RID: 14588 RVA: 0x0010C993 File Offset: 0x0010AB93
		public string nickName
		{
			get
			{
				if (OptionsSettings.streamer && this.steamID != Provider.user)
				{
					return this.streamerName;
				}
				return this._nickName;
			}
			set
			{
				this._nickName = value;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x0010C99C File Offset: 0x0010AB9C
		[Obsolete("Each client has multiple HWIDs, call GetHwids instead, this property returns the first HWID")]
		public byte[] hwid
		{
			get
			{
				return this.hwids[0];
			}
		}

		/// <summary>
		/// 20-byte SHA1 salted hashes of client's hardware ID(s).
		/// Providing multiple HWIDs makes it more difficult to bypass HWID bans because spoofing a single component
		/// only changes one of the bans. For example spoofing the MAC address will not spoof the Windows GUID.
		///
		/// Randomized if system did not support hwid, or perhaps player is cheating.
		/// Should not be called on the client side, but just in case there is a default zeroed array.
		/// </summary>
		// Token: 0x060038FE RID: 14590 RVA: 0x0010C9A6 File Offset: 0x0010ABA6
		public IEnumerable<byte[]> GetHwids()
		{
			return this.hwids;
		}

		/// <summary>
		/// Ignore requests to kick me in debug mode. :)
		/// Steam ID may not have been authenticated yet here which may seem like a security risk, but fortunately that
		/// would get caught when Steam auth ticket response is received.
		/// </summary>
		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x060038FF RID: 14591 RVA: 0x0010C9AE File Offset: 0x0010ABAE
		internal bool BypassIntegrityChecks
		{
			get
			{
				return this.steamID.m_SteamID == 76561198036822957UL && this.characterName.Equals("Debug");
			}
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x0010C9D8 File Offset: 0x0010ABD8
		public SteamPlayerID(CSteamID newSteamID, byte newCharacterID, string newPlayerName, string newCharacterName, string newNickName, CSteamID newGroup) : this(newSteamID, newCharacterID, newPlayerName, newCharacterName, newNickName, newGroup, new byte[20])
		{
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0010C9F0 File Offset: 0x0010ABF0
		public SteamPlayerID(CSteamID newSteamID, byte newCharacterID, string newPlayerName, string newCharacterName, string newNickName, CSteamID newGroup, byte[] newHwid) : this(newSteamID, newCharacterID, newPlayerName, newCharacterName, newNickName, newGroup, new byte[][]
		{
			newHwid
		})
		{
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x0010CA17 File Offset: 0x0010AC17
		public SteamPlayerID(CSteamID newSteamID, byte newCharacterID, string newPlayerName, string newCharacterName, string newNickName, CSteamID newGroup, byte[][] newHwids)
		{
			this._steamID = newSteamID;
			this.characterID = newCharacterID;
			this._playerName = newPlayerName;
			this._characterName = newCharacterName;
			this._nickName = newNickName;
			this.group = newGroup;
			this.hwids = newHwids;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x0010CA54 File Offset: 0x0010AC54
		public override string ToString()
		{
			return string.Format("{0}[{1}] \"{2}\"", this.steamID, this.characterID, this.playerName);
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x0010CA7C File Offset: 0x0010AC7C
		public static bool operator ==(SteamPlayerID playerID_0, SteamPlayerID playerID_1)
		{
			return playerID_0.steamID == playerID_1.steamID;
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0010CA8F File Offset: 0x0010AC8F
		public static bool operator !=(SteamPlayerID playerID_0, SteamPlayerID playerID_1)
		{
			return !(playerID_0 == playerID_1);
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x0010CA9C File Offset: 0x0010AC9C
		public static string operator +(SteamPlayerID playerID, string text)
		{
			return playerID.steamID.ToString() + text;
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x0010CAC4 File Offset: 0x0010ACC4
		public bool Equals(SteamPlayerID otherPlayerID)
		{
			return otherPlayerID != null && this.steamID.Equals(otherPlayerID.steamID);
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x0010CAEC File Offset: 0x0010ACEC
		public override int GetHashCode()
		{
			return this.steamID.GetHashCode();
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0010CB0D File Offset: 0x0010AD0D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SteamPlayerID);
		}

		// Token: 0x040021EE RID: 8686
		private CSteamID _steamID;

		/// <summary>
		/// In vanilla this field is ONLY used for the per-character saves on servers.
		/// If that changes check that it does not affect the savedata options.
		/// </summary>
		// Token: 0x040021EF RID: 8687
		public byte characterID;

		// Token: 0x040021F0 RID: 8688
		private string _playerName;

		// Token: 0x040021F1 RID: 8689
		private string _characterName;

		// Token: 0x040021F2 RID: 8690
		private string _nickName;

		// Token: 0x040021F3 RID: 8691
		public CSteamID group;

		/// <summary>
		/// Array of 20-byte SHA1 hashes.
		/// </summary>
		// Token: 0x040021F4 RID: 8692
		private byte[][] hwids;
	}
}
