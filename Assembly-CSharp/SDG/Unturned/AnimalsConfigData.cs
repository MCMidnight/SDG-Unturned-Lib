using System;

namespace SDG.Unturned
{
	// Token: 0x020006DE RID: 1758
	public class AnimalsConfigData
	{
		// Token: 0x06003AF7 RID: 15095 RVA: 0x00113A24 File Offset: 0x00111C24
		public AnimalsConfigData(EGameMode mode)
		{
			this.Respawn_Time = 180f;
			if (mode != EGameMode.EASY)
			{
				if (mode != EGameMode.HARD)
				{
					this.Damage_Multiplier = 1f;
					this.Armor_Multiplier = 1f;
				}
				else
				{
					this.Damage_Multiplier = 1.5f;
					this.Armor_Multiplier = 0.75f;
				}
			}
			else
			{
				this.Damage_Multiplier = 0.75f;
				this.Armor_Multiplier = 1.25f;
			}
			this.Max_Instances_Tiny = 4U;
			this.Max_Instances_Small = 8U;
			this.Max_Instances_Medium = 16U;
			this.Max_Instances_Large = 32U;
			this.Max_Instances_Insane = 64U;
			this.Weapons_Use_Player_Damage = (mode == EGameMode.HARD);
		}

		// Token: 0x04002427 RID: 9255
		public float Respawn_Time;

		// Token: 0x04002428 RID: 9256
		public float Damage_Multiplier;

		// Token: 0x04002429 RID: 9257
		public float Armor_Multiplier;

		// Token: 0x0400242A RID: 9258
		public uint Max_Instances_Tiny;

		// Token: 0x0400242B RID: 9259
		public uint Max_Instances_Small;

		// Token: 0x0400242C RID: 9260
		public uint Max_Instances_Medium;

		// Token: 0x0400242D RID: 9261
		public uint Max_Instances_Large;

		// Token: 0x0400242E RID: 9262
		public uint Max_Instances_Insane;

		// Token: 0x0400242F RID: 9263
		public bool Weapons_Use_Player_Damage;
	}
}
