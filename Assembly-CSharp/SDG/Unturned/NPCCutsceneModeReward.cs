using System;

namespace SDG.Unturned
{
	// Token: 0x02000321 RID: 801
	public class NPCCutsceneModeReward : INPCReward
	{
		// Token: 0x06001830 RID: 6192 RVA: 0x0005899D File Offset: 0x00056B9D
		public override void GrantReward(Player player)
		{
			player.quests.ServerSetCutsceneModeActive(this.value);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x000589B0 File Offset: 0x00056BB0
		public NPCCutsceneModeReward(bool newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}

		// Token: 0x04000AE9 RID: 2793
		private bool value;
	}
}
