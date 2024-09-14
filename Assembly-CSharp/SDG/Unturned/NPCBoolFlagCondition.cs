using System;

namespace SDG.Unturned
{
	// Token: 0x0200031C RID: 796
	public class NPCBoolFlagCondition : NPCFlagCondition
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x0005855D File Offset: 0x0005675D
		// (set) Token: 0x0600180A RID: 6154 RVA: 0x00058565 File Offset: 0x00056765
		public bool value { get; protected set; }

		// Token: 0x0600180B RID: 6155 RVA: 0x00058570 File Offset: 0x00056770
		public override bool isConditionMet(Player player)
		{
			short num;
			if (player.quests.getFlag(base.id, out num))
			{
				return base.doesLogicPass<bool>(num == 1, this.value);
			}
			return base.allowUnset;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000585A9 File Offset: 0x000567A9
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(base.id);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x000585C5 File Offset: 0x000567C5
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return Local.FormatText(this.text, this.isConditionMet(player) ? 1 : 0);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000585F3 File Offset: 0x000567F3
		public NPCBoolFlagCondition(ushort newID, bool newValue, bool newAllowUnset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newID, newAllowUnset, newLogicType, newText, newShouldReset)
		{
			this.value = newValue;
		}
	}
}
