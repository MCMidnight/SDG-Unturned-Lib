using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200031B RID: 795
	public class NPCAnimalKillsCondition : INPCCondition
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x0005843F File Offset: 0x0005663F
		// (set) Token: 0x060017FE RID: 6142 RVA: 0x00058447 File Offset: 0x00056647
		public ushort id { get; protected set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x00058450 File Offset: 0x00056650
		// (set) Token: 0x06001800 RID: 6144 RVA: 0x00058458 File Offset: 0x00056658
		public short value { get; protected set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x00058461 File Offset: 0x00056661
		// (set) Token: 0x06001802 RID: 6146 RVA: 0x00058469 File Offset: 0x00056669
		public ushort animal { get; protected set; }

		// Token: 0x06001803 RID: 6147 RVA: 0x00058474 File Offset: 0x00056674
		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000584A4 File Offset: 0x000566A4
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.id);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000584C0 File Offset: 0x000566C0
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_AnimalKills");
			}
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			return Local.FormatText(this.text, num, this.value);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00058522 File Offset: 0x00056722
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0005852D File Offset: 0x0005672D
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0005853C File Offset: 0x0005673C
		public NPCAnimalKillsCondition(ushort newID, short newValue, ushort newAnimal, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
			this.animal = newAnimal;
		}
	}
}
