using System;

namespace SDG.Unturned
{
	// Token: 0x020006E2 RID: 1762
	public class ObjectConfigData
	{
		// Token: 0x06003AFE RID: 15102 RVA: 0x00113E58 File Offset: 0x00112058
		public ObjectConfigData(EGameMode mode)
		{
			this.Binary_State_Reset_Multiplier = 1f;
			this.Fuel_Reset_Multiplier = 1f;
			this.Water_Reset_Multiplier = 1f;
			this.Resource_Reset_Multiplier = 1f;
			this.Resource_Drops_Multiplier = 1f;
			this.Rubble_Reset_Multiplier = 1f;
			this.Allow_Holiday_Drops = true;
			this.Items_Obstruct_Tree_Respawns = true;
		}

		// Token: 0x0400246D RID: 9325
		public float Binary_State_Reset_Multiplier;

		// Token: 0x0400246E RID: 9326
		public float Fuel_Reset_Multiplier;

		// Token: 0x0400246F RID: 9327
		public float Water_Reset_Multiplier;

		// Token: 0x04002470 RID: 9328
		public float Resource_Reset_Multiplier;

		// Token: 0x04002471 RID: 9329
		public float Resource_Drops_Multiplier;

		// Token: 0x04002472 RID: 9330
		public float Rubble_Reset_Multiplier;

		// Token: 0x04002473 RID: 9331
		public bool Allow_Holiday_Drops;

		// Token: 0x04002474 RID: 9332
		public bool Items_Obstruct_Tree_Respawns;
	}
}
