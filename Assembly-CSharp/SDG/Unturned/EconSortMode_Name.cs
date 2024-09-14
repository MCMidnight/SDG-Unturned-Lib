using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Sorts name alphabetically to the front of the list.
	/// </summary>
	// Token: 0x02000493 RID: 1171
	public class EconSortMode_Name : Comparer<SteamItemDetails_t>
	{
		// Token: 0x06002489 RID: 9353 RVA: 0x00091CF4 File Offset: 0x0008FEF4
		public override int Compare(SteamItemDetails_t x, SteamItemDetails_t y)
		{
			string inventoryName = Provider.provider.economyService.getInventoryName(x.m_iDefinition.m_SteamItemDef);
			string inventoryName2 = Provider.provider.economyService.getInventoryName(y.m_iDefinition.m_SteamItemDef);
			return inventoryName.CompareTo(inventoryName2);
		}
	}
}
