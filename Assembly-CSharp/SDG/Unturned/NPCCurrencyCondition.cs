using System;

namespace SDG.Unturned
{
	// Token: 0x0200031F RID: 799
	public class NPCCurrencyCondition : NPCLogicCondition
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x0005878C File Offset: 0x0005698C
		// (set) Token: 0x06001822 RID: 6178 RVA: 0x00058794 File Offset: 0x00056994
		public AssetReference<ItemCurrencyAsset> currency { get; protected set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x0005879D File Offset: 0x0005699D
		// (set) Token: 0x06001824 RID: 6180 RVA: 0x000587A5 File Offset: 0x000569A5
		public uint value { get; protected set; }

		// Token: 0x06001825 RID: 6181 RVA: 0x000587B0 File Offset: 0x000569B0
		public override bool isConditionMet(Player player)
		{
			ItemCurrencyAsset itemCurrencyAsset = this.currency.Find();
			if (itemCurrencyAsset == null)
			{
				return false;
			}
			uint inventoryValue = itemCurrencyAsset.getInventoryValue(player);
			return base.doesLogicPass<uint>(inventoryValue, this.value);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x000587E8 File Offset: 0x000569E8
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			ItemCurrencyAsset itemCurrencyAsset = this.currency.Find();
			if (itemCurrencyAsset == null)
			{
				return;
			}
			itemCurrencyAsset.spendValue(player, this.value);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00058820 File Offset: 0x00056A20
		public override string formatCondition(Player player)
		{
			ItemCurrencyAsset itemCurrencyAsset = this.currency.Find();
			if (itemCurrencyAsset == null)
			{
				return "?";
			}
			if (string.IsNullOrEmpty(this.text))
			{
				if (!string.IsNullOrEmpty(itemCurrencyAsset.defaultConditionFormat))
				{
					this.text = itemCurrencyAsset.defaultConditionFormat;
				}
				else
				{
					this.text = PlayerNPCQuestUI.localization.format("Condition_Currency");
				}
			}
			uint inventoryValue = itemCurrencyAsset.getInventoryValue(player);
			return Local.FormatText(this.text, inventoryValue, this.value);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x000588A6 File Offset: 0x00056AA6
		public NPCCurrencyCondition(AssetReference<ItemCurrencyAsset> newCurrency, uint newValue, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.currency = newCurrency;
			this.value = newValue;
		}
	}
}
