using System;

namespace SDG.Unturned
{
	// Token: 0x02000329 RID: 809
	public class NPCHintReward : INPCReward
	{
		// Token: 0x06001858 RID: 6232 RVA: 0x00058DBB File Offset: 0x00056FBB
		public override void GrantReward(Player player)
		{
			player.ServerShowHint(this.text, this.duration);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00058DCF File Offset: 0x00056FCF
		public NPCHintReward(float newDuration, string newText) : base(newText)
		{
			this.duration = newDuration;
		}

		/// <summary>
		/// How many seconds message should popup.
		/// </summary>
		// Token: 0x04000AF9 RID: 2809
		private float duration;
	}
}
