using System;

namespace SDG.Unturned
{
	// Token: 0x02000320 RID: 800
	public class NPCCurrencyReward : INPCReward
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x000588C1 File Offset: 0x00056AC1
		// (set) Token: 0x0600182A RID: 6186 RVA: 0x000588C9 File Offset: 0x00056AC9
		public AssetReference<ItemCurrencyAsset> currency { get; protected set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600182B RID: 6187 RVA: 0x000588D2 File Offset: 0x00056AD2
		// (set) Token: 0x0600182C RID: 6188 RVA: 0x000588DA File Offset: 0x00056ADA
		public uint value { get; protected set; }

		// Token: 0x0600182D RID: 6189 RVA: 0x000588E4 File Offset: 0x00056AE4
		public override void GrantReward(Player player)
		{
			ItemCurrencyAsset itemCurrencyAsset = this.currency.Find();
			if (itemCurrencyAsset == null)
			{
				return;
			}
			itemCurrencyAsset.grantValue(player, this.value);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00058914 File Offset: 0x00056B14
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				ItemCurrencyAsset itemCurrencyAsset = this.currency.Find();
				if (itemCurrencyAsset != null && !string.IsNullOrEmpty(itemCurrencyAsset.valueFormat))
				{
					this.text = itemCurrencyAsset.valueFormat;
				}
				else
				{
					this.text = PlayerNPCQuestUI.localization.read("Reward_Currency");
				}
			}
			return Local.FormatText(this.text, this.value);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x00058986 File Offset: 0x00056B86
		public NPCCurrencyReward(AssetReference<ItemCurrencyAsset> newCurrency, uint newValue, string newText) : base(newText)
		{
			this.currency = newCurrency;
			this.value = newValue;
		}
	}
}
