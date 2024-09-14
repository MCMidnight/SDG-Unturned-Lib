using System;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Utilities for testing whether a particular server is allowed to download a workshop item.
	/// Available from client and server side so that clients can help enforce restrictions.
	/// </summary>
	// Token: 0x0200038A RID: 906
	public class WorkshopDownloadRestrictions
	{
		/// <summary>
		/// Get ip restrictions value if set, otherwise null.
		/// Can be called from client or server.
		/// </summary>
		// Token: 0x06001C23 RID: 7203 RVA: 0x00064734 File Offset: 0x00062934
		public static string getAllowedIpsTagValue(UGCQueryHandle_t queryHandle, uint resultIndex)
		{
			string result;
			WorkshopUtils.findQueryUGCKeyValue(queryHandle, resultIndex, WorkshopDownloadRestrictions.IP_RESTRICTIONS_KVTAG, out result);
			return result;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00064754 File Offset: 0x00062954
		public static EWorkshopDownloadRestrictionResult getRestrictionResult(UGCQueryHandle_t queryHandle, uint resultIndex, uint ip)
		{
			SteamUGCDetails_t steamUGCDetails_t;
			if (WorkshopUtils.getQueryUGCResult(queryHandle, resultIndex, out steamUGCDetails_t))
			{
				if (steamUGCDetails_t.m_bBanned)
				{
					return EWorkshopDownloadRestrictionResult.Banned;
				}
				if (steamUGCDetails_t.m_eVisibility == ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate || steamUGCDetails_t.m_eResult == EResult.k_EResultAccessDenied)
				{
					return EWorkshopDownloadRestrictionResult.PrivateVisibility;
				}
			}
			string allowedIpsTagValue = WorkshopDownloadRestrictions.getAllowedIpsTagValue(queryHandle, resultIndex);
			if (string.IsNullOrEmpty(allowedIpsTagValue))
			{
				return EWorkshopDownloadRestrictionResult.NoRestrictions;
			}
			return WorkshopDownloadRestrictions.getRestrictionResult(allowedIpsTagValue, ip);
		}

		/// <summary>
		/// Test whether IP is whitelisted or blacklisted in filter.
		/// </summary>
		// Token: 0x06001C25 RID: 7205 RVA: 0x000647A4 File Offset: 0x000629A4
		public static EWorkshopDownloadRestrictionResult getRestrictionResult(string filter, uint ip)
		{
			uint[] array;
			uint[] array2;
			WorkshopDownloadRestrictions.parseAllowedIPs(filter, out array, out array2);
			if (array != null && array.Length != 0)
			{
				if (WorkshopDownloadRestrictions.isAddressInList(ip, array))
				{
					return EWorkshopDownloadRestrictionResult.Allowed;
				}
				return EWorkshopDownloadRestrictionResult.NotWhitelisted;
			}
			else
			{
				if (array2 == null || array2.Length == 0)
				{
					return EWorkshopDownloadRestrictionResult.NoRestrictions;
				}
				if (WorkshopDownloadRestrictions.isAddressInList(ip, array2))
				{
					return EWorkshopDownloadRestrictionResult.Blacklisted;
				}
				return EWorkshopDownloadRestrictionResult.Allowed;
			}
		}

		/// <summary>
		/// Split x,y-z format into whitelist [x, y] and blacklist [z].
		/// </summary>
		// Token: 0x06001C26 RID: 7206 RVA: 0x000647E4 File Offset: 0x000629E4
		public static void splitAllowedIPs(string allowedIPs, out string[] whitelistIps, out string[] blacklistIps)
		{
			whitelistIps = null;
			blacklistIps = null;
			int num = allowedIPs.IndexOf('-');
			if (num >= 0 && num < allowedIPs.Length - 1)
			{
				string text = allowedIPs.Substring(0, num);
				string text2 = allowedIPs.Substring(num + 1);
				whitelistIps = text.Split(WorkshopDownloadRestrictions.IP_SEPARATOR, 1);
				blacklistIps = text2.Split(WorkshopDownloadRestrictions.IP_SEPARATOR, 1);
				return;
			}
			whitelistIps = allowedIPs.Split(',', 0);
		}

		/// <summary>
		/// Split whitelist-blacklist format and parse string IPs into integer IPs.
		/// </summary>
		// Token: 0x06001C27 RID: 7207 RVA: 0x0006484C File Offset: 0x00062A4C
		public static void parseAllowedIPs(string allowedIPs, out uint[] whitelist, out uint[] blacklist)
		{
			string[] array;
			string[] array2;
			WorkshopDownloadRestrictions.splitAllowedIPs(allowedIPs, out array, out array2);
			if (array == null || array.Length < 1)
			{
				whitelist = null;
			}
			else
			{
				WorkshopDownloadRestrictions.parseStringIps(array, out whitelist);
			}
			if (array2 == null || array2.Length < 1)
			{
				blacklist = null;
				return;
			}
			WorkshopDownloadRestrictions.parseStringIps(array2, out blacklist);
		}

		/// <summary>
		/// Parse CIDR string IPs into integer IPs.
		/// </summary>
		// Token: 0x06001C28 RID: 7208 RVA: 0x0006488C File Offset: 0x00062A8C
		public static void parseStringIps(string[] strings, out uint[] integers)
		{
			int num = strings.Length;
			integers = new uint[num];
			for (int i = 0; i < num; i++)
			{
				integers[i] = Parser.getUInt32FromIP(strings[i]);
			}
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000648C0 File Offset: 0x00062AC0
		public static bool isAddressInList(uint ip, uint[] list)
		{
			foreach (uint num in list)
			{
				if (ip == num)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Workshop item key-value tag storing IP whitelist and blacklist.
		/// </summary>
		// Token: 0x04000D48 RID: 3400
		public static readonly string IP_RESTRICTIONS_KVTAG = "allowed_ips";

		// Token: 0x04000D49 RID: 3401
		private static readonly char[] IP_SEPARATOR = new char[]
		{
			','
		};
	}
}
