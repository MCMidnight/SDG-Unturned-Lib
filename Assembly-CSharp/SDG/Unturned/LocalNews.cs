using System;

namespace SDG.Unturned
{
	// Token: 0x020005B5 RID: 1461
	public static class LocalNews
	{
		/// <summary>
		/// Has player dismissed the given workshop item?
		/// </summary>
		// Token: 0x06002F8A RID: 12170 RVA: 0x000D220C File Offset: 0x000D040C
		public static bool wasWorkshopItemDismissed(ulong id)
		{
			string flag = LocalNews.formatDismissedWorkshopItemFlag(id);
			return ConvenientSavedata.get().hasFlag(flag);
		}

		/// <summary>
		/// Track that the player has dismissed the given workshop item.
		/// </summary>
		// Token: 0x06002F8B RID: 12171 RVA: 0x000D222C File Offset: 0x000D042C
		public static void dismissWorkshopItem(ulong id)
		{
			string flag = LocalNews.formatDismissedWorkshopItemFlag(id);
			ConvenientSavedata.get().setFlag(flag);
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000D224B File Offset: 0x000D044B
		private static string formatDismissedWorkshopItemFlag(ulong id)
		{
			return "Dismissed_Workshop_Item_" + id.ToString();
		}

		/// <summary>
		/// Has player already auto-subscribed to the given workshop item?
		/// </summary>
		// Token: 0x06002F8D RID: 12173 RVA: 0x000D2260 File Offset: 0x000D0460
		public static bool hasAutoSubscribedToWorkshopItem(ulong id)
		{
			string flag = LocalNews.formatAutoSubscribedWorkshopItemFlag(id);
			return ConvenientSavedata.get().hasFlag(flag);
		}

		/// <summary>
		/// Track that the player has auto-subscribed to the given workshop item.
		/// </summary>
		// Token: 0x06002F8E RID: 12174 RVA: 0x000D2280 File Offset: 0x000D0480
		public static void markAutoSubscribedToWorkshopItem(ulong id)
		{
			string flag = LocalNews.formatAutoSubscribedWorkshopItemFlag(id);
			ConvenientSavedata.get().setFlag(flag);
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000D229F File Offset: 0x000D049F
		private static string formatAutoSubscribedWorkshopItemFlag(ulong id)
		{
			return "Auto_Subscribed_Workshop_Item_" + id.ToString();
		}
	}
}
