using System;
using System.Collections.Generic;
using SDG.Provider;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Sorts higher rarity items into the front of the list.
	/// </summary>
	// Token: 0x02000492 RID: 1170
	public class EconSortMode_Rarity : Comparer<SteamItemDetails_t>
	{
		// Token: 0x06002487 RID: 9351 RVA: 0x00091C54 File Offset: 0x0008FE54
		public override int Compare(SteamItemDetails_t x, SteamItemDetails_t y)
		{
			UnturnedEconInfo.ERarity inventoryRarity = Provider.provider.economyService.getInventoryRarity(x.m_iDefinition.m_SteamItemDef);
			UnturnedEconInfo.ERarity inventoryRarity2 = Provider.provider.economyService.getInventoryRarity(y.m_iDefinition.m_SteamItemDef);
			int num = inventoryRarity.CompareTo(inventoryRarity2);
			if (num == 0)
			{
				string inventoryName = Provider.provider.economyService.getInventoryName(x.m_iDefinition.m_SteamItemDef);
				string inventoryName2 = Provider.provider.economyService.getInventoryName(y.m_iDefinition.m_SteamItemDef);
				return inventoryName.CompareTo(inventoryName2);
			}
			return -num;
		}
	}
}
