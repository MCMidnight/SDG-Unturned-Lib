using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Tracks whether we should show the "NEW" label on listings and item store button.
	/// </summary>
	// Token: 0x020005B6 RID: 1462
	public static class ItemStoreSavedata
	{
		// Token: 0x06002F90 RID: 12176 RVA: 0x000D22B2 File Offset: 0x000D04B2
		public static bool WasNewCraftingPageSeen()
		{
			return true;
		}

		/// <summary>
		/// Track that player has seen the new crafting blueprints.
		/// </summary>
		// Token: 0x06002F91 RID: 12177 RVA: 0x000D22B5 File Offset: 0x000D04B5
		public static void MarkNewCraftingPageSeen()
		{
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000D22B7 File Offset: 0x000D04B7
		public static bool WasNewListingsPageSeen()
		{
			return true;
		}

		/// <summary>
		/// Track that player has seen the page with all new listings.
		/// </summary>
		// Token: 0x06002F93 RID: 12179 RVA: 0x000D22BA File Offset: 0x000D04BA
		public static void MarkNewListingsPageSeen()
		{
		}

		/// <summary>
		/// Has player seen the given listing?
		/// </summary>
		// Token: 0x06002F94 RID: 12180 RVA: 0x000D22BC File Offset: 0x000D04BC
		public static bool WasNewListingSeen(int itemdefid)
		{
			string flag = ItemStoreSavedata.FormatNewListingSeenFlag(itemdefid);
			return ConvenientSavedata.get().hasFlag(flag);
		}

		/// <summary>
		/// Track that the player has seen the given listing.
		/// </summary>
		// Token: 0x06002F95 RID: 12181 RVA: 0x000D22DC File Offset: 0x000D04DC
		public static void MarkNewListingSeen(int itemdefid)
		{
			string flag = ItemStoreSavedata.FormatNewListingSeenFlag(itemdefid);
			ConvenientSavedata.get().setFlag(flag);
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000D22FB File Offset: 0x000D04FB
		private static string FormatNewListingSeenFlag(int itemdefid)
		{
			return "New_Listing_Seen_" + itemdefid.ToString();
		}
	}
}
