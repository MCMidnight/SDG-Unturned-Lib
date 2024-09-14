using System;

namespace SDG.Unturned
{
	// Token: 0x02000346 RID: 838
	public class NPCShortFlagCondition : NPCFlagCondition
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x0005A78D File Offset: 0x0005898D
		// (set) Token: 0x06001924 RID: 6436 RVA: 0x0005A795 File Offset: 0x00058995
		public short value { get; protected set; }

		// Token: 0x06001925 RID: 6437 RVA: 0x0005A7A0 File Offset: 0x000589A0
		public override bool isConditionMet(Player player)
		{
			short a;
			if (player.quests.getFlag(base.id, out a))
			{
				return base.doesLogicPass<short>(a, this.value);
			}
			return base.allowUnset;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0005A7D6 File Offset: 0x000589D6
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(base.id);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0005A7F4 File Offset: 0x000589F4
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			short num;
			if (!player.quests.getFlag(base.id, out num))
			{
				if (base.allowUnset)
				{
					num = this.value;
				}
				else
				{
					num = 0;
				}
			}
			return Local.FormatText(this.text, num, this.value);
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0005A854 File Offset: 0x00058A54
		public NPCShortFlagCondition(ushort newID, short newValue, bool newAllowUnset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newID, newAllowUnset, newLogicType, newText, newShouldReset)
		{
			this.value = newValue;
		}
	}
}
