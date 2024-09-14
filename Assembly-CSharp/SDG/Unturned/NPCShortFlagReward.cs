using System;

namespace SDG.Unturned
{
	// Token: 0x02000347 RID: 839
	public class NPCShortFlagReward : INPCReward
	{
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x0005A86B File Offset: 0x00058A6B
		// (set) Token: 0x0600192A RID: 6442 RVA: 0x0005A873 File Offset: 0x00058A73
		public ushort id { get; protected set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x0005A87C File Offset: 0x00058A7C
		// (set) Token: 0x0600192C RID: 6444 RVA: 0x0005A884 File Offset: 0x00058A84
		public virtual short value { get; protected set; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x0005A88D File Offset: 0x00058A8D
		// (set) Token: 0x0600192E RID: 6446 RVA: 0x0005A895 File Offset: 0x00058A95
		public ENPCModificationType modificationType { get; protected set; }

		// Token: 0x0600192F RID: 6447 RVA: 0x0005A8A0 File Offset: 0x00058AA0
		public override void GrantReward(Player player)
		{
			if (this.modificationType == ENPCModificationType.ASSIGN)
			{
				player.quests.sendSetFlag(this.id, this.value);
				return;
			}
			short num;
			player.quests.getFlag(this.id, out num);
			if (this.modificationType == ENPCModificationType.INCREMENT)
			{
				num += this.value;
			}
			else if (this.modificationType == ENPCModificationType.DECREMENT)
			{
				num -= this.value;
			}
			player.quests.sendSetFlag(this.id, num);
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0005A91C File Offset: 0x00058B1C
		public NPCShortFlagReward(ushort newID, short newValue, ENPCModificationType newModificationType, string newText) : base(newText)
		{
			this.id = newID;
			this.value = newValue;
			this.modificationType = newModificationType;
		}
	}
}
