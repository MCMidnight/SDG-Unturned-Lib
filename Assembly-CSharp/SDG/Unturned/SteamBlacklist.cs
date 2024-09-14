using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000684 RID: 1668
	public class SteamBlacklist
	{
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x00109819 File Offset: 0x00107A19
		public static List<SteamBlacklistID> list
		{
			get
			{
				return SteamBlacklist._list;
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x00109820 File Offset: 0x00107A20
		[Obsolete]
		public static void ban(CSteamID playerID, CSteamID judgeID, string reason, uint duration)
		{
			SteamBlacklist.ban(playerID, 0U, judgeID, reason, duration);
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x0010982C File Offset: 0x00107A2C
		[Obsolete("Now accepts list of HWIDs")]
		public static void ban(CSteamID playerID, uint ip, CSteamID judgeID, string reason, uint duration)
		{
			SteamBlacklist.ban(playerID, ip, null, judgeID, reason, duration);
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x0010983C File Offset: 0x00107A3C
		public static void ban(CSteamID playerID, uint ip, IEnumerable<byte[]> hwids, CSteamID judgeID, string reason, uint duration)
		{
			Provider.ban(playerID, reason, duration);
			for (int i = 0; i < SteamBlacklist.list.Count; i++)
			{
				if (SteamBlacklist.list[i].playerID == playerID)
				{
					SteamBlacklist.list[i].judgeID = judgeID;
					SteamBlacklist.list[i].reason = reason;
					SteamBlacklist.list[i].duration = duration;
					SteamBlacklist.list[i].banned = Provider.time;
					return;
				}
			}
			byte[][] newHwids;
			if (hwids != null)
			{
				List<byte[]> list = new List<byte[]>(8);
				foreach (byte[] array in hwids)
				{
					list.Add(array);
				}
				newHwids = list.ToArray();
			}
			else
			{
				newHwids = null;
			}
			SteamBlacklist.list.Add(new SteamBlacklistID(playerID, ip, judgeID, reason, duration, Provider.time, newHwids));
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x0010993C File Offset: 0x00107B3C
		public static bool unban(CSteamID playerID)
		{
			for (int i = 0; i < SteamBlacklist.list.Count; i++)
			{
				if (SteamBlacklist.list[i].playerID == playerID)
				{
					SteamBlacklist.list.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x00109984 File Offset: 0x00107B84
		[Obsolete]
		public static bool checkBanned(CSteamID playerID, out SteamBlacklistID blacklistID)
		{
			return SteamBlacklist.checkBanned(playerID, 0U, out blacklistID);
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x0010998E File Offset: 0x00107B8E
		[Obsolete("Now checks HWID")]
		public static bool checkBanned(CSteamID playerID, uint ip, out SteamBlacklistID blacklistID)
		{
			return SteamBlacklist.checkBanned(playerID, ip, null, out blacklistID);
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x0010999C File Offset: 0x00107B9C
		public static bool checkBanned(CSteamID playerID, uint ip, IEnumerable<byte[]> hwids, out SteamBlacklistID blacklistID)
		{
			blacklistID = null;
			int i = SteamBlacklist.list.Count - 1;
			while (i >= 0)
			{
				if (SteamBlacklist.list[i].playerID == playerID || (SteamBlacklist.list[i].ip == ip && ip != 0U) || SteamBlacklist.list[i].DoesAnyHwidMatch(hwids))
				{
					if (SteamBlacklist.list[i].isExpired)
					{
						SteamBlacklist.list.RemoveAt(i);
						return false;
					}
					blacklistID = SteamBlacklist.list[i];
					return true;
				}
				else
				{
					i--;
				}
			}
			return false;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x00109A34 File Offset: 0x00107C34
		public static void load()
		{
			SteamBlacklist._list = new List<SteamBlacklistID>();
			if (ServerSavedata.fileExists("/Server/Blacklist.dat"))
			{
				River river = ServerSavedata.openRiver("/Server/Blacklist.dat", true);
				byte b = river.readByte();
				if (b > 1)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						CSteamID newPlayerID = river.readSteamID();
						uint newIP;
						if (b > 2)
						{
							newIP = river.readUInt32();
						}
						else
						{
							newIP = 0U;
						}
						CSteamID newJudgeID = river.readSteamID();
						string newReason = river.readString();
						uint newDuration = river.readUInt32();
						uint newBanned = river.readUInt32();
						byte[][] array;
						if (b >= 4)
						{
							int num3 = river.readInt32();
							if (num3 > 0)
							{
								array = new byte[num3][];
								for (int i = 0; i < num3; i++)
								{
									array[i] = river.readBytes();
								}
							}
							else
							{
								array = null;
							}
						}
						else
						{
							array = null;
						}
						SteamBlacklistID steamBlacklistID = new SteamBlacklistID(newPlayerID, newIP, newJudgeID, newReason, newDuration, newBanned, array);
						if (!steamBlacklistID.isExpired)
						{
							SteamBlacklist.list.Add(steamBlacklistID);
						}
					}
				}
				river.closeRiver();
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x00109B38 File Offset: 0x00107D38
		public static void save()
		{
			River river = ServerSavedata.openRiver("/Server/Blacklist.dat", false);
			river.writeByte(SteamBlacklist.SAVEDATA_VERSION);
			river.writeUInt16((ushort)SteamBlacklist.list.Count);
			ushort num = 0;
			while ((int)num < SteamBlacklist.list.Count)
			{
				SteamBlacklistID steamBlacklistID = SteamBlacklist.list[(int)num];
				river.writeSteamID(steamBlacklistID.playerID);
				river.writeUInt32(steamBlacklistID.ip);
				river.writeSteamID(steamBlacklistID.judgeID);
				river.writeString(steamBlacklistID.reason);
				river.writeUInt32(steamBlacklistID.duration);
				river.writeUInt32(steamBlacklistID.banned);
				if (steamBlacklistID.hwids == null)
				{
					river.writeInt32(0);
				}
				else
				{
					river.writeInt32(steamBlacklistID.hwids.Length);
					foreach (byte[] values in steamBlacklistID.hwids)
					{
						river.writeBytes(values);
					}
				}
				num += 1;
			}
			river.closeRiver();
		}

		// Token: 0x0400214C RID: 8524
		public const byte SAVEDATA_VERSION_ADDED_HWID = 4;

		// Token: 0x0400214D RID: 8525
		private const byte SAVEDATA_VERSION_NEWEST = 4;

		// Token: 0x0400214E RID: 8526
		public static readonly byte SAVEDATA_VERSION = 4;

		// Token: 0x0400214F RID: 8527
		public static readonly uint PERMANENT = 31536000U;

		// Token: 0x04002150 RID: 8528
		public static readonly uint TEMPORARY = 180U;

		// Token: 0x04002151 RID: 8529
		private static List<SteamBlacklistID> _list;
	}
}
