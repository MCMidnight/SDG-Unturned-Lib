using System;

namespace SDG.Unturned
{
	// Token: 0x020006E4 RID: 1764
	public class EventsConfigData
	{
		// Token: 0x06003B00 RID: 15104 RVA: 0x00113EC4 File Offset: 0x001120C4
		public EventsConfigData(EGameMode mode)
		{
			this.Rain_Frequency_Min = 2.3f;
			this.Rain_Frequency_Max = 5.6f;
			this.Rain_Duration_Min = 0.05f;
			this.Rain_Duration_Max = 0.15f;
			this.Snow_Frequency_Min = 1.3f;
			this.Snow_Frequency_Max = 4.6f;
			this.Snow_Duration_Min = 0.2f;
			this.Snow_Duration_Max = 0.5f;
			this.Weather_Frequency_Multiplier = 1f;
			this.Weather_Duration_Multiplier = 1f;
			this.Airdrop_Frequency_Min = 0.8f;
			this.Airdrop_Frequency_Max = 6.5f;
			this.Airdrop_Speed = 128f;
			this.Airdrop_Force = 9.5f;
			this.Arena_Clear_Timer = 5U;
			this.Arena_Finale_Timer = 10U;
			this.Arena_Restart_Timer = 15U;
			this.Arena_Compactor_Delay_Timer = 1U;
			this.Arena_Compactor_Pause_Timer = 5U;
			this.Arena_Min_Players = 2U;
			this.Arena_Compactor_Damage = 9U;
			this.Arena_Compactor_Extra_Damage_Per_Second = 1f;
			this.Use_Airdrops = true;
			this.Arena_Use_Compactor_Pause = true;
			this.Arena_Compactor_Speed_Tiny = 0.5f;
			this.Arena_Compactor_Speed_Small = 1.5f;
			this.Arena_Compactor_Speed_Medium = 3f;
			this.Arena_Compactor_Speed_Large = 4.5f;
			this.Arena_Compactor_Speed_Insane = 6f;
			this.Arena_Compactor_Shrink_Factor = 0.5f;
		}

		// Token: 0x04002477 RID: 9335
		public float Rain_Frequency_Min;

		// Token: 0x04002478 RID: 9336
		public float Rain_Frequency_Max;

		// Token: 0x04002479 RID: 9337
		public float Rain_Duration_Min;

		// Token: 0x0400247A RID: 9338
		public float Rain_Duration_Max;

		// Token: 0x0400247B RID: 9339
		public float Snow_Frequency_Min;

		// Token: 0x0400247C RID: 9340
		public float Snow_Frequency_Max;

		// Token: 0x0400247D RID: 9341
		public float Snow_Duration_Min;

		// Token: 0x0400247E RID: 9342
		public float Snow_Duration_Max;

		/// <summary>
		/// Each per-level custom weather frequency is multiplied by this value.
		/// </summary>
		// Token: 0x0400247F RID: 9343
		public float Weather_Frequency_Multiplier;

		/// <summary>
		/// Each per-level custom weather duration is multiplied by this value.
		/// </summary>
		// Token: 0x04002480 RID: 9344
		public float Weather_Duration_Multiplier;

		// Token: 0x04002481 RID: 9345
		public float Airdrop_Frequency_Min;

		// Token: 0x04002482 RID: 9346
		public float Airdrop_Frequency_Max;

		// Token: 0x04002483 RID: 9347
		public float Airdrop_Speed;

		// Token: 0x04002484 RID: 9348
		public float Airdrop_Force;

		// Token: 0x04002485 RID: 9349
		public uint Arena_Min_Players;

		// Token: 0x04002486 RID: 9350
		public uint Arena_Compactor_Damage;

		// Token: 0x04002487 RID: 9351
		public float Arena_Compactor_Extra_Damage_Per_Second;

		// Token: 0x04002488 RID: 9352
		public uint Arena_Clear_Timer;

		// Token: 0x04002489 RID: 9353
		public uint Arena_Finale_Timer;

		// Token: 0x0400248A RID: 9354
		public uint Arena_Restart_Timer;

		// Token: 0x0400248B RID: 9355
		public uint Arena_Compactor_Delay_Timer;

		// Token: 0x0400248C RID: 9356
		public uint Arena_Compactor_Pause_Timer;

		// Token: 0x0400248D RID: 9357
		public bool Use_Airdrops;

		// Token: 0x0400248E RID: 9358
		public bool Arena_Use_Compactor_Pause;

		// Token: 0x0400248F RID: 9359
		public float Arena_Compactor_Speed_Tiny;

		// Token: 0x04002490 RID: 9360
		public float Arena_Compactor_Speed_Small;

		// Token: 0x04002491 RID: 9361
		public float Arena_Compactor_Speed_Medium;

		// Token: 0x04002492 RID: 9362
		public float Arena_Compactor_Speed_Large;

		// Token: 0x04002493 RID: 9363
		public float Arena_Compactor_Speed_Insane;

		// Token: 0x04002494 RID: 9364
		public float Arena_Compactor_Shrink_Factor;
	}
}
