using System;

namespace SDG.Unturned
{
	// Token: 0x020002A8 RID: 680
	public class DialogueElement
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x0004CA7F File Offset: 0x0004AC7F
		// (set) Token: 0x06001480 RID: 5248 RVA: 0x0004CA87 File Offset: 0x0004AC87
		public byte index { get; protected set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0004CA90 File Offset: 0x0004AC90
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x0004CA98 File Offset: 0x0004AC98
		public INPCCondition[] conditions { get; protected set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0004CAA1 File Offset: 0x0004ACA1
		public INPCReward[] rewards
		{
			get
			{
				return this.rewardsList.rewards;
			}
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004CAB0 File Offset: 0x0004ACB0
		public bool areConditionsMet(Player player)
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

		// Token: 0x06001485 RID: 5253 RVA: 0x0004CAEC File Offset: 0x0004ACEC
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

		// Token: 0x06001486 RID: 5254 RVA: 0x0004CB22 File Offset: 0x0004AD22
		public void GrantRewards(Player player)
		{
			this.rewardsList.Grant(player);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0004CB30 File Offset: 0x0004AD30
		public DialogueElement(byte newIndex, INPCCondition[] newConditions, NPCRewardsList newRewardsList)
		{
			this.index = newIndex;
			this.conditions = newConditions;
			this.rewardsList = newRewardsList;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0004CB4D File Offset: 0x0004AD4D
		[Obsolete("Removed shouldSend parameter")]
		public void applyConditions(Player player, bool shouldSend)
		{
			this.ApplyConditions(player);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0004CB56 File Offset: 0x0004AD56
		[Obsolete("Removed shouldSend parameter")]
		public void grantRewards(Player player, bool shouldSend)
		{
			this.GrantRewards(player);
		}

		// Token: 0x04000713 RID: 1811
		protected NPCRewardsList rewardsList;
	}
}
