using System;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Utilities for calling workshop functions without worrying about client/server.
	/// This could be nicely refactored into a client and server interface, but not enough time for that right now.
	/// </summary>
	// Token: 0x0200038B RID: 907
	public class WorkshopUtils
	{
		/// <summary>
		/// Client/server safe version of GetQueryUGCNumKeyValueTags.
		/// </summary>
		// Token: 0x06001C2C RID: 7212 RVA: 0x0006490C File Offset: 0x00062B0C
		public static uint getQueryUGCNumKeyValueTags(UGCQueryHandle_t queryHandle, uint resultIndex)
		{
			return SteamGameServerUGC.GetQueryUGCNumKeyValueTags(queryHandle, resultIndex);
		}

		/// <summary>
		/// Client/server safe version of GetQueryUGCKeyValueTag.
		/// </summary>
		// Token: 0x06001C2D RID: 7213 RVA: 0x00064915 File Offset: 0x00062B15
		public static bool getQueryUGCKeyValueTag(UGCQueryHandle_t queryHandle, uint resultIndex, uint tagIndex, out string key, out string value)
		{
			return SteamGameServerUGC.GetQueryUGCKeyValueTag(queryHandle, resultIndex, tagIndex, out key, 255U, out value, 255U);
		}

		/// <summary>
		/// Search for the value associated with a given key.
		/// </summary>
		// Token: 0x06001C2E RID: 7214 RVA: 0x0006492C File Offset: 0x00062B2C
		public static bool findQueryUGCKeyValue(UGCQueryHandle_t queryHandle, uint resultIndex, string key, out string value)
		{
			uint queryUGCNumKeyValueTags = WorkshopUtils.getQueryUGCNumKeyValueTags(queryHandle, resultIndex);
			for (uint num = 0U; num < queryUGCNumKeyValueTags; num += 1U)
			{
				string text;
				string text2;
				if (WorkshopUtils.getQueryUGCKeyValueTag(queryHandle, resultIndex, num, out text, out text2) && text.Equals(key, 3))
				{
					value = text2;
					return true;
				}
			}
			value = null;
			return false;
		}

		/// <summary>
		///             Client/server safe version of GetQueryUGCResult.
		/// </summary>
		// Token: 0x06001C2F RID: 7215 RVA: 0x0006496E File Offset: 0x00062B6E
		public static bool getQueryUGCResult(UGCQueryHandle_t queryHandle, uint resultIndex, out SteamUGCDetails_t details)
		{
			return SteamGameServerUGC.GetQueryUGCResult(queryHandle, resultIndex, out details);
		}

		/// <summary>
		/// Is file banned?
		/// </summary>
		// Token: 0x06001C30 RID: 7216 RVA: 0x00064978 File Offset: 0x00062B78
		public static bool getQueryUGCBanned(UGCQueryHandle_t queryHandle, uint resultIndex)
		{
			SteamUGCDetails_t steamUGCDetails_t;
			return WorkshopUtils.getQueryUGCResult(queryHandle, resultIndex, out steamUGCDetails_t) && steamUGCDetails_t.m_bBanned;
		}
	}
}
