using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200032C RID: 812
	public class NPCHordeKillsCondition : INPCCondition
	{
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600185E RID: 6238 RVA: 0x00058E15 File Offset: 0x00057015
		// (set) Token: 0x0600185F RID: 6239 RVA: 0x00058E1D File Offset: 0x0005701D
		public ushort id { get; protected set; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x00058E26 File Offset: 0x00057026
		// (set) Token: 0x06001861 RID: 6241 RVA: 0x00058E2E File Offset: 0x0005702E
		public short value { get; protected set; }

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x00058E37 File Offset: 0x00057037
		// (set) Token: 0x06001863 RID: 6243 RVA: 0x00058E3F File Offset: 0x0005703F
		public byte nav { get; protected set; }

		// Token: 0x06001864 RID: 6244 RVA: 0x00058E48 File Offset: 0x00057048
		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00058E78 File Offset: 0x00057078
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.id);
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00058E94 File Offset: 0x00057094
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_HordeKills");
			}
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			return Local.FormatText(this.text, num, this.value);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00058EF6 File Offset: 0x000570F6
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00058F01 File Offset: 0x00057101
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00058F10 File Offset: 0x00057110
		public NPCHordeKillsCondition(ushort newID, short newValue, byte newNav, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
			this.nav = newNav;
		}
	}
}
