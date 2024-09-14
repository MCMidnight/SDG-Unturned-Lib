using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000332 RID: 818
	public class NPCPlayerKillsCondition : INPCCondition
	{
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x00059ACD File Offset: 0x00057CCD
		// (set) Token: 0x060018A5 RID: 6309 RVA: 0x00059AD5 File Offset: 0x00057CD5
		public ushort id { get; protected set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x00059ADE File Offset: 0x00057CDE
		// (set) Token: 0x060018A7 RID: 6311 RVA: 0x00059AE6 File Offset: 0x00057CE6
		public short value { get; protected set; }

		// Token: 0x060018A8 RID: 6312 RVA: 0x00059AF0 File Offset: 0x00057CF0
		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00059B20 File Offset: 0x00057D20
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.id);
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00059B3C File Offset: 0x00057D3C
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_PlayerKills");
			}
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			return Local.FormatText(this.text, num, this.value);
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00059B9E File Offset: 0x00057D9E
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00059BA9 File Offset: 0x00057DA9
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00059BB8 File Offset: 0x00057DB8
		public NPCPlayerKillsCondition(ushort newID, short newValue, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
		}
	}
}
