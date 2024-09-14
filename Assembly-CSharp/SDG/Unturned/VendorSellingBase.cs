using System;

namespace SDG.Unturned
{
	// Token: 0x02000384 RID: 900
	public abstract class VendorSellingBase : VendorElement
	{
		// Token: 0x06001BF4 RID: 7156 RVA: 0x00063EF0 File Offset: 0x000620F0
		public bool canBuy(Player player)
		{
			if (!base.outerAsset.currency.isValid)
			{
				return player.skills.experience >= base.cost;
			}
			ItemCurrencyAsset itemCurrencyAsset = base.outerAsset.currency.Find();
			if (itemCurrencyAsset == null)
			{
				Assets.reportError(base.outerAsset, "missing currency asset");
				return false;
			}
			return itemCurrencyAsset.canAfford(player, base.cost);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00063F60 File Offset: 0x00062160
		public virtual void buy(Player player)
		{
			if (base.outerAsset.currency.isValid)
			{
				ItemCurrencyAsset itemCurrencyAsset = base.outerAsset.currency.Find();
				if (itemCurrencyAsset == null)
				{
					Assets.reportError(base.outerAsset, "missing currency asset");
					return;
				}
				if (!itemCurrencyAsset.spendValue(player, base.cost))
				{
					UnturnedLog.error("Spending {0} currency at vendor went wrong (this should never happen)", new object[]
					{
						base.cost
					});
					return;
				}
			}
			else
			{
				player.skills.askSpend(base.cost);
			}
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x00063FE9 File Offset: 0x000621E9
		public virtual void format(Player player, out ushort total)
		{
			total = 0;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x00063FEE File Offset: 0x000621EE
		public VendorSellingBase(VendorAsset newOuterAsset, byte newIndex, Guid newTargetAssetGuid, ushort newLegacyAssetId, uint newCost, INPCCondition[] newConditions, NPCRewardsList newRewardsList) : base(newOuterAsset, newIndex, newTargetAssetGuid, newLegacyAssetId, newCost, newConditions, newRewardsList)
		{
		}
	}
}
