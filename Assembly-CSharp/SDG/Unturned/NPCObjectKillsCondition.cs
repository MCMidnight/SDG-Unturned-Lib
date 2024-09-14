using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000331 RID: 817
	public class NPCObjectKillsCondition : INPCCondition
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001896 RID: 6294 RVA: 0x00059997 File Offset: 0x00057B97
		// (set) Token: 0x06001897 RID: 6295 RVA: 0x0005999F File Offset: 0x00057B9F
		public ushort id { get; protected set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x000599A8 File Offset: 0x00057BA8
		// (set) Token: 0x06001899 RID: 6297 RVA: 0x000599B0 File Offset: 0x00057BB0
		public short value { get; protected set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600189A RID: 6298 RVA: 0x000599B9 File Offset: 0x00057BB9
		// (set) Token: 0x0600189B RID: 6299 RVA: 0x000599C1 File Offset: 0x00057BC1
		public Guid objectGuid { get; protected set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x000599CA File Offset: 0x00057BCA
		// (set) Token: 0x0600189D RID: 6301 RVA: 0x000599D2 File Offset: 0x00057BD2
		public byte nav { get; protected set; }

		// Token: 0x0600189E RID: 6302 RVA: 0x000599DC File Offset: 0x00057BDC
		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00059A0C File Offset: 0x00057C0C
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.id);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00059A28 File Offset: 0x00057C28
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_ObjectKills");
			}
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			return Local.FormatText(this.text, num, this.value);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00059A8A File Offset: 0x00057C8A
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00059A95 File Offset: 0x00057C95
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00059AA4 File Offset: 0x00057CA4
		public NPCObjectKillsCondition(ushort newID, short newValue, Guid newObjectGuid, byte newNav, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
			this.objectGuid = newObjectGuid;
			this.nav = newNav;
		}
	}
}
