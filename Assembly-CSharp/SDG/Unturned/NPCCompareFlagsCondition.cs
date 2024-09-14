using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200031E RID: 798
	public class NPCCompareFlagsCondition : NPCLogicCondition
	{
		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x00058663 File Offset: 0x00056863
		// (set) Token: 0x06001816 RID: 6166 RVA: 0x0005866B File Offset: 0x0005686B
		public ushort flag_A_ID { get; protected set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x00058674 File Offset: 0x00056874
		// (set) Token: 0x06001818 RID: 6168 RVA: 0x0005867C File Offset: 0x0005687C
		public bool allowFlag_A_Unset { get; protected set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x00058685 File Offset: 0x00056885
		// (set) Token: 0x0600181A RID: 6170 RVA: 0x0005868D File Offset: 0x0005688D
		public bool allowFlag_B_Unset { get; protected set; }

		// Token: 0x0600181B RID: 6171 RVA: 0x00058698 File Offset: 0x00056898
		public override bool isConditionMet(Player player)
		{
			short a;
			short b;
			return (player.quests.getFlag(this.flag_A_ID, out a) || this.allowFlag_A_Unset) && (player.quests.getFlag(this.flag_B_ID, out b) || this.allowFlag_B_Unset) && base.doesLogicPass<short>(a, b);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000586EB File Offset: 0x000568EB
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.flag_A_ID);
			player.quests.sendRemoveFlag(this.flag_B_ID);
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00058718 File Offset: 0x00056918
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return this.text;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0005872F File Offset: 0x0005692F
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.flag_A_ID || flagID == this.flag_B_ID;
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00058745 File Offset: 0x00056945
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.flag_A_ID);
			associatedFlags.Add(this.flag_B_ID);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00058761 File Offset: 0x00056961
		public NPCCompareFlagsCondition(ushort newFlag_A_ID, ushort newFlag_B_ID, bool newAllowFlag_A_Unset, bool newAllowFlag_B_Unset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.flag_A_ID = newFlag_A_ID;
			this.allowFlag_A_Unset = newAllowFlag_A_Unset;
			this.flag_B_ID = newFlag_B_ID;
			this.allowFlag_B_Unset = newAllowFlag_B_Unset;
		}

		// Token: 0x04000AE3 RID: 2787
		public ushort flag_B_ID;
	}
}
