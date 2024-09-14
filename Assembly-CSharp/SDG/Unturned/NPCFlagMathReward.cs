using System;

namespace SDG.Unturned
{
	// Token: 0x02000328 RID: 808
	public class NPCFlagMathReward : INPCReward
	{
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001850 RID: 6224 RVA: 0x00058CBB File Offset: 0x00056EBB
		// (set) Token: 0x06001851 RID: 6225 RVA: 0x00058CC3 File Offset: 0x00056EC3
		public ushort flag_A_ID { get; protected set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001852 RID: 6226 RVA: 0x00058CCC File Offset: 0x00056ECC
		// (set) Token: 0x06001853 RID: 6227 RVA: 0x00058CD4 File Offset: 0x00056ED4
		public ushort flag_B_ID { get; protected set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001854 RID: 6228 RVA: 0x00058CDD File Offset: 0x00056EDD
		// (set) Token: 0x06001855 RID: 6229 RVA: 0x00058CE5 File Offset: 0x00056EE5
		public ENPCOperationType operationType { get; protected set; }

		// Token: 0x06001856 RID: 6230 RVA: 0x00058CF0 File Offset: 0x00056EF0
		public override void GrantReward(Player player)
		{
			short num;
			player.quests.getFlag(this.flag_A_ID, out num);
			short num2;
			if (this.flag_B_ID == 0 || !player.quests.getFlag(this.flag_B_ID, out num2))
			{
				num2 = this.defaultFlag_B_Value;
			}
			switch (this.operationType)
			{
			case ENPCOperationType.ASSIGN:
				num = num2;
				break;
			case ENPCOperationType.ADDITION:
				num += num2;
				break;
			case ENPCOperationType.SUBTRACTION:
				num -= num2;
				break;
			case ENPCOperationType.MULTIPLICATION:
				num *= num2;
				break;
			case ENPCOperationType.DIVISION:
				num /= num2;
				break;
			case ENPCOperationType.MODULO:
				num %= num2;
				break;
			default:
				return;
			}
			player.quests.sendSetFlag(this.flag_A_ID, num);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x00058D94 File Offset: 0x00056F94
		public NPCFlagMathReward(ushort newFlag_A_ID, ushort newFlag_B_ID, short newFlag_B_Value, ENPCOperationType newOperationType, string newText) : base(newText)
		{
			this.flag_A_ID = newFlag_A_ID;
			this.flag_B_ID = newFlag_B_ID;
			this.defaultFlag_B_Value = newFlag_B_Value;
			this.operationType = newOperationType;
		}

		// Token: 0x04000AF7 RID: 2807
		private short defaultFlag_B_Value;
	}
}
