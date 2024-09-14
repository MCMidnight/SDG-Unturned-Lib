using System;

namespace SDG.Unturned
{
	// Token: 0x02000344 RID: 836
	public class NPCRewardsAsset : Asset
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x0005A50F File Offset: 0x0005870F
		// (set) Token: 0x06001917 RID: 6423 RVA: 0x0005A517 File Offset: 0x00058717
		public INPCCondition[] conditions { get; private set; }

		// Token: 0x06001918 RID: 6424 RVA: 0x0005A520 File Offset: 0x00058720
		public bool AreConditionsMet(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					if (!this.conditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0005A55C File Offset: 0x0005875C
		public void ApplyConditions(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					this.conditions[i].ApplyCondition(player);
				}
			}
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0005A592 File Offset: 0x00058792
		public void GrantRewards(Player player)
		{
			this.rewardsList.Grant(player);
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0005A5A0 File Offset: 0x000587A0
		public override string GetTypeFriendlyName()
		{
			return "NPC Rewards List";
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0005A5A8 File Offset: 0x000587A8
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.conditions = new INPCCondition[(int)data.ParseUInt8("Conditions", 0)];
			NPCTool.readConditions(data, localization, "Condition_", this.conditions, this);
			this.rewardsList.Parse(data, localization, this, "Rewards", "Reward_");
		}

		// Token: 0x04000B39 RID: 2873
		private NPCRewardsList rewardsList;
	}
}
