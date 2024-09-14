using System;

namespace SDG.Unturned
{
	// Token: 0x020006DD RID: 1757
	public class ZombiesConfigData
	{
		// Token: 0x06003AF6 RID: 15094 RVA: 0x00113734 File Offset: 0x00111934
		public ZombiesConfigData(EGameMode mode)
		{
			this.Respawn_Day_Time = 360f;
			this.Respawn_Night_Time = 30f;
			this.Respawn_Beacon_Time = 0f;
			this.Quest_Boss_Respawn_Interval = 600f;
			switch (mode)
			{
			case EGameMode.EASY:
				this.Spawn_Chance = 0.2f;
				this.Loot_Chance = 0.55f;
				this.Crawler_Chance = 0f;
				this.Sprinter_Chance = 0f;
				this.Flanker_Chance = 0f;
				this.Burner_Chance = 0f;
				this.Acid_Chance = 0f;
				break;
			case EGameMode.NORMAL:
				this.Spawn_Chance = 0.25f;
				this.Loot_Chance = 0.5f;
				this.Crawler_Chance = 0.15f;
				this.Sprinter_Chance = 0.15f;
				this.Flanker_Chance = 0.025f;
				this.Burner_Chance = 0.025f;
				this.Acid_Chance = 0.025f;
				break;
			case EGameMode.HARD:
				this.Spawn_Chance = 0.3f;
				this.Loot_Chance = 0.3f;
				this.Crawler_Chance = 0.125f;
				this.Sprinter_Chance = 0.175f;
				this.Flanker_Chance = 0.05f;
				this.Burner_Chance = 0.05f;
				this.Acid_Chance = 0.05f;
				break;
			default:
				this.Spawn_Chance = 1f;
				this.Loot_Chance = 0f;
				this.Crawler_Chance = 0f;
				this.Sprinter_Chance = 0f;
				this.Flanker_Chance = 0f;
				this.Burner_Chance = 0f;
				this.Acid_Chance = 0f;
				break;
			}
			this.Boss_Electric_Chance = 0f;
			this.Boss_Wind_Chance = 0f;
			this.Boss_Fire_Chance = 0f;
			this.Spirit_Chance = 0f;
			this.DL_Red_Volatile_Chance = 0f;
			this.DL_Blue_Volatile_Chance = 0f;
			this.Boss_Elver_Stomper_Chance = 0f;
			this.Boss_Kuwait_Chance = 0f;
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
			this.Backstab_Multiplier = 1.25f;
			this.NonHeadshot_Armor_Multiplier = 1f;
			this.Beacon_Experience_Multiplier = 1f;
			this.Full_Moon_Experience_Multiplier = 2f;
			this.Min_Drops = 1U;
			this.Max_Drops = 1U;
			this.Min_Mega_Drops = 5U;
			this.Max_Mega_Drops = 5U;
			this.Min_Boss_Drops = 8U;
			this.Max_Boss_Drops = 10U;
			this.Slow_Movement = (mode == EGameMode.EASY);
			this.Can_Stun = (mode != EGameMode.HARD);
			this.Only_Critical_Stuns = (mode == EGameMode.HARD);
			this.Weapons_Use_Player_Damage = (mode == EGameMode.HARD);
			this.Can_Target_Barricades = true;
			this.Can_Target_Structures = true;
			this.Can_Target_Vehicles = true;
			this.Beacon_Max_Rewards = 0U;
			this.Beacon_Max_Participants = 0U;
			this.Beacon_Rewards_Multiplier = 1f;
		}

		// Token: 0x040023FE RID: 9214
		public float Spawn_Chance;

		// Token: 0x040023FF RID: 9215
		public float Loot_Chance;

		// Token: 0x04002400 RID: 9216
		public float Crawler_Chance;

		// Token: 0x04002401 RID: 9217
		public float Sprinter_Chance;

		// Token: 0x04002402 RID: 9218
		public float Flanker_Chance;

		// Token: 0x04002403 RID: 9219
		public float Burner_Chance;

		// Token: 0x04002404 RID: 9220
		public float Acid_Chance;

		// Token: 0x04002405 RID: 9221
		public float Boss_Electric_Chance;

		// Token: 0x04002406 RID: 9222
		public float Boss_Wind_Chance;

		// Token: 0x04002407 RID: 9223
		public float Boss_Fire_Chance;

		// Token: 0x04002408 RID: 9224
		public float Spirit_Chance;

		// Token: 0x04002409 RID: 9225
		public float DL_Red_Volatile_Chance;

		// Token: 0x0400240A RID: 9226
		public float DL_Blue_Volatile_Chance;

		// Token: 0x0400240B RID: 9227
		public float Boss_Elver_Stomper_Chance;

		// Token: 0x0400240C RID: 9228
		public float Boss_Kuwait_Chance;

		// Token: 0x0400240D RID: 9229
		public float Respawn_Day_Time;

		// Token: 0x0400240E RID: 9230
		public float Respawn_Night_Time;

		// Token: 0x0400240F RID: 9231
		public float Respawn_Beacon_Time;

		/// <summary>
		/// Minimum seconds between boss zombie spawns for players doing quests.
		/// Players were abusing the spawns to farm boss tier loot.
		/// </summary>
		// Token: 0x04002410 RID: 9232
		public float Quest_Boss_Respawn_Interval;

		// Token: 0x04002411 RID: 9233
		public float Damage_Multiplier;

		// Token: 0x04002412 RID: 9234
		public float Armor_Multiplier;

		// Token: 0x04002413 RID: 9235
		public float Backstab_Multiplier;

		/// <summary>
		/// Weapon damage multiplier against body, arms, legs. Useful for headshot-only mode.
		/// </summary>
		// Token: 0x04002414 RID: 9236
		public float NonHeadshot_Armor_Multiplier;

		// Token: 0x04002415 RID: 9237
		public float Beacon_Experience_Multiplier;

		// Token: 0x04002416 RID: 9238
		public float Full_Moon_Experience_Multiplier;

		// Token: 0x04002417 RID: 9239
		public uint Min_Drops;

		// Token: 0x04002418 RID: 9240
		public uint Max_Drops;

		// Token: 0x04002419 RID: 9241
		public uint Min_Mega_Drops;

		// Token: 0x0400241A RID: 9242
		public uint Max_Mega_Drops;

		// Token: 0x0400241B RID: 9243
		public uint Min_Boss_Drops;

		// Token: 0x0400241C RID: 9244
		public uint Max_Boss_Drops;

		// Token: 0x0400241D RID: 9245
		public bool Slow_Movement;

		// Token: 0x0400241E RID: 9246
		public bool Can_Stun;

		// Token: 0x0400241F RID: 9247
		public bool Only_Critical_Stuns;

		// Token: 0x04002420 RID: 9248
		public bool Weapons_Use_Player_Damage;

		// Token: 0x04002421 RID: 9249
		public bool Can_Target_Barricades;

		// Token: 0x04002422 RID: 9250
		public bool Can_Target_Structures;

		// Token: 0x04002423 RID: 9251
		public bool Can_Target_Vehicles;

		// Token: 0x04002424 RID: 9252
		public uint Beacon_Max_Rewards;

		// Token: 0x04002425 RID: 9253
		public uint Beacon_Max_Participants;

		// Token: 0x04002426 RID: 9254
		public float Beacon_Rewards_Multiplier;
	}
}
