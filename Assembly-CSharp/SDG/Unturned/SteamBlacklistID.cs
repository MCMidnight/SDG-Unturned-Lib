using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000685 RID: 1669
	public class SteamBlacklistID
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x00109C4D File Offset: 0x00107E4D
		public CSteamID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x0600382B RID: 14379 RVA: 0x00109C55 File Offset: 0x00107E55
		public uint ip
		{
			get
			{
				return this._ip;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x00109C5D File Offset: 0x00107E5D
		public bool isExpired
		{
			get
			{
				return Provider.time > this.banned + this.duration;
			}
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x00109C73 File Offset: 0x00107E73
		public uint getTime()
		{
			return this.duration - (Provider.time - this.banned);
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x00109C88 File Offset: 0x00107E88
		public bool DoesAnyHwidMatch(IEnumerable<byte[]> clientHwids)
		{
			if (this.hwids == null || clientHwids == null)
			{
				return false;
			}
			foreach (byte[] hash_ in this.hwids)
			{
				foreach (byte[] hash_2 in clientHwids)
				{
					if (Hash.verifyHash(hash_, hash_2))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x00109D04 File Offset: 0x00107F04
		public SteamBlacklistID(CSteamID newPlayerID, uint newIP, CSteamID newJudgeID, string newReason, uint newDuration, uint newBanned, byte[][] newHwids)
		{
			this._playerID = newPlayerID;
			this._ip = newIP;
			this.judgeID = newJudgeID;
			this.reason = newReason;
			this.duration = newDuration;
			this.banned = newBanned;
			this.hwids = newHwids;
		}

		// Token: 0x04002152 RID: 8530
		private CSteamID _playerID;

		// Token: 0x04002153 RID: 8531
		private uint _ip;

		// Token: 0x04002154 RID: 8532
		internal byte[][] hwids;

		// Token: 0x04002155 RID: 8533
		public CSteamID judgeID;

		// Token: 0x04002156 RID: 8534
		public string reason;

		// Token: 0x04002157 RID: 8535
		public uint duration;

		// Token: 0x04002158 RID: 8536
		public uint banned;
	}
}
