using System;

namespace SDG.Unturned
{
	// Token: 0x0200035F RID: 863
	public class QuestAsset : Asset
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x0005DB3C File Offset: 0x0005BD3C
		// (set) Token: 0x06001A0C RID: 6668 RVA: 0x0005DB44 File Offset: 0x0005BD44
		public string questName { get; protected set; }

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x0005DB4D File Offset: 0x0005BD4D
		// (set) Token: 0x06001A0E RID: 6670 RVA: 0x0005DB55 File Offset: 0x0005BD55
		public string questDescription { get; protected set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0005DB5E File Offset: 0x0005BD5E
		// (set) Token: 0x06001A10 RID: 6672 RVA: 0x0005DB66 File Offset: 0x0005BD66
		public INPCCondition[] conditions { get; protected set; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x0005DB6F File Offset: 0x0005BD6F
		public INPCReward[] rewards
		{
			get
			{
				return this.rewardsList.rewards;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0005DB7C File Offset: 0x0005BD7C
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NPC;
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0005DB80 File Offset: 0x0005BD80
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

		// Token: 0x06001A14 RID: 6676 RVA: 0x0005DBBC File Offset: 0x0005BDBC
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

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005DBF2 File Offset: 0x0005BDF2
		public void GrantRewards(Player player)
		{
			this.rewardsList.Grant(player);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0005DC00 File Offset: 0x0005BE00
		public void GrantAbandonmentRewards(Player player)
		{
			this.abandonmentRewardsList.Grant(player);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0005DC10 File Offset: 0x0005BE10
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 2000 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this.questName = localization.format("Name");
			this.questName = ItemTool.filterRarityRichText(this.questName);
			string text = localization.format("Description");
			text = ItemTool.filterRarityRichText(text);
			RichTextUtil.replaceNewlineMarkup(ref text);
			this.questDescription = text;
			this.conditions = new INPCCondition[(int)data.ParseUInt8("Conditions", 0)];
			NPCTool.readConditions(data, localization, "Condition_", this.conditions, this);
			this.rewardsList.Parse(data, localization, this, "Rewards", "Reward_");
			this.abandonmentRewardsList.Parse(data, localization, this, "AbandonmentRewards", "AbandonmentReward_");
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0005DCF0 File Offset: 0x0005BEF0
		[Obsolete("Removed shouldSend parameter")]
		public void applyConditions(Player player, bool shouldSend)
		{
			this.ApplyConditions(player);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x0005DCF9 File Offset: 0x0005BEF9
		[Obsolete("Removed shouldSend parameter")]
		public void grantRewards(Player player, bool shouldSend)
		{
			this.GrantRewards(player);
		}

		// Token: 0x04000BEB RID: 3051
		protected NPCRewardsList rewardsList;

		/// <summary>
		/// Rewards to grant when quest is removed without completing.
		/// Not granted when player finishes quest.
		/// </summary>
		// Token: 0x04000BEC RID: 3052
		protected NPCRewardsList abandonmentRewardsList;
	}
}
