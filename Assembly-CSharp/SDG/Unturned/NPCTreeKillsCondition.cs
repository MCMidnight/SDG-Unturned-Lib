using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200034B RID: 843
	public class NPCTreeKillsCondition : INPCCondition
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x0005AB80 File Offset: 0x00058D80
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x0005AB88 File Offset: 0x00058D88
		public ushort id { get; protected set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x0005AB91 File Offset: 0x00058D91
		// (set) Token: 0x06001942 RID: 6466 RVA: 0x0005AB99 File Offset: 0x00058D99
		public short value { get; protected set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x0005ABA2 File Offset: 0x00058DA2
		// (set) Token: 0x06001944 RID: 6468 RVA: 0x0005ABAA File Offset: 0x00058DAA
		public Guid treeGuid { get; protected set; }

		// Token: 0x06001945 RID: 6469 RVA: 0x0005ABB4 File Offset: 0x00058DB4
		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0005ABE4 File Offset: 0x00058DE4
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.id);
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0005AC00 File Offset: 0x00058E00
		public override string formatCondition(Player player)
		{
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			ResourceAsset resourceAsset = Assets.find(this.treeGuid) as ResourceAsset;
			string arg = (resourceAsset == null) ? "?" : resourceAsset.resourceName;
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_TreeKills");
			}
			return Local.FormatText(this.text, num, this.value, arg);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0005AC85 File Offset: 0x00058E85
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0005AC90 File Offset: 0x00058E90
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0005AC9F File Offset: 0x00058E9F
		public NPCTreeKillsCondition(ushort newID, short newValue, Guid newTreeGuid, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
			this.treeGuid = newTreeGuid;
		}
	}
}
