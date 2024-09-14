using System;

namespace SDG.Unturned
{
	// Token: 0x020006E1 RID: 1761
	public class PlayersConfigData
	{
		// Token: 0x06003AFC RID: 15100 RVA: 0x00113BD4 File Offset: 0x00111DD4
		public PlayersConfigData(EGameMode mode)
		{
			this.Health_Default = 100U;
			this.Health_Regen_Min_Food = 90U;
			this.Health_Regen_Min_Water = 90U;
			this.Health_Regen_Ticks = 60U;
			this.Food_Damage_Ticks = 15U;
			this.Water_Damage_Ticks = 20U;
			this.Virus_Default = 100U;
			this.Virus_Infect = 50U;
			this.Virus_Use_Ticks = 125U;
			this.Virus_Damage_Ticks = 25U;
			this.Leg_Regen_Ticks = 750U;
			this.Bleed_Damage_Ticks = 10U;
			this.Bleed_Regen_Ticks = 750U;
			if (mode == EGameMode.HARD)
			{
				this.Food_Default = 85U;
				this.Water_Default = 85U;
			}
			else
			{
				this.Food_Default = 100U;
				this.Water_Default = 100U;
			}
			if (mode != EGameMode.EASY)
			{
				if (mode != EGameMode.HARD)
				{
					this.Food_Use_Ticks = 300U;
					this.Water_Use_Ticks = 270U;
				}
				else
				{
					this.Food_Use_Ticks = 250U;
					this.Water_Use_Ticks = 220U;
				}
			}
			else
			{
				this.Food_Use_Ticks = 350U;
				this.Water_Use_Ticks = 320U;
			}
			switch (mode)
			{
			case EGameMode.EASY:
				this.Experience_Multiplier = 1.5f;
				break;
			case EGameMode.NORMAL:
				this.Experience_Multiplier = 1f;
				break;
			case EGameMode.HARD:
				this.Experience_Multiplier = 1.5f;
				break;
			default:
				this.Experience_Multiplier = 10f;
				break;
			}
			if (mode != EGameMode.EASY)
			{
				if (mode != EGameMode.HARD)
				{
					this.Detect_Radius_Multiplier = 1f;
				}
				else
				{
					this.Detect_Radius_Multiplier = 1.25f;
				}
			}
			else
			{
				this.Detect_Radius_Multiplier = 0.5f;
			}
			this.Ray_Aggressor_Distance = 8f;
			this.Armor_Multiplier = 1f;
			this.Lose_Skills_PvP = 1f;
			this.Lose_Skills_PvE = 1f;
			this.Lose_Skill_Levels_PvP = 1U;
			this.Lose_Skill_Levels_PvE = 1U;
			this.Lose_Experience_PvP = 0.5f;
			this.Lose_Experience_PvE = 0.5f;
			this.Lose_Items_PvP = 1f;
			this.Lose_Items_PvE = 1f;
			this.Lose_Clothes_PvP = true;
			this.Lose_Clothes_PvE = true;
			this.Lose_Weapons_PvP = true;
			this.Lose_Weapons_PvE = true;
			this.Can_Hurt_Legs = true;
			if (mode == EGameMode.EASY)
			{
				this.Can_Break_Legs = false;
				this.Can_Start_Bleeding = false;
				this.Lose_Skill_Levels_PvP = 0U;
				this.Lose_Skill_Levels_PvE = 0U;
			}
			else
			{
				this.Can_Break_Legs = true;
				this.Can_Start_Bleeding = true;
			}
			if (mode == EGameMode.HARD)
			{
				this.Can_Fix_Legs = false;
				this.Can_Stop_Bleeding = false;
				this.Lose_Skill_Levels_PvP = 2U;
				this.Lose_Skill_Levels_PvE = 2U;
			}
			else
			{
				this.Can_Fix_Legs = true;
				this.Can_Stop_Bleeding = true;
			}
			this.Spawn_With_Max_Skills = false;
			this.Spawn_With_Stamina_Skills = false;
			this.Allow_Instakill_Headshots = (mode == EGameMode.HARD);
			this.Allow_Per_Character_Saves = false;
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x00113E4C File Offset: 0x0011204C
		public void InitSingleplayerDefaults()
		{
			this.Allow_Per_Character_Saves = true;
		}

		// Token: 0x04002442 RID: 9282
		public uint Health_Default;

		// Token: 0x04002443 RID: 9283
		public uint Health_Regen_Min_Food;

		// Token: 0x04002444 RID: 9284
		public uint Health_Regen_Min_Water;

		// Token: 0x04002445 RID: 9285
		public uint Health_Regen_Ticks;

		// Token: 0x04002446 RID: 9286
		public uint Food_Default;

		// Token: 0x04002447 RID: 9287
		public uint Food_Use_Ticks;

		// Token: 0x04002448 RID: 9288
		public uint Food_Damage_Ticks;

		// Token: 0x04002449 RID: 9289
		public uint Water_Default;

		// Token: 0x0400244A RID: 9290
		public uint Water_Use_Ticks;

		// Token: 0x0400244B RID: 9291
		public uint Water_Damage_Ticks;

		// Token: 0x0400244C RID: 9292
		public uint Virus_Default;

		// Token: 0x0400244D RID: 9293
		public uint Virus_Infect;

		// Token: 0x0400244E RID: 9294
		public uint Virus_Use_Ticks;

		// Token: 0x0400244F RID: 9295
		public uint Virus_Damage_Ticks;

		// Token: 0x04002450 RID: 9296
		public uint Leg_Regen_Ticks;

		// Token: 0x04002451 RID: 9297
		public uint Bleed_Damage_Ticks;

		// Token: 0x04002452 RID: 9298
		public uint Bleed_Regen_Ticks;

		// Token: 0x04002453 RID: 9299
		public float Armor_Multiplier;

		// Token: 0x04002454 RID: 9300
		public float Experience_Multiplier;

		// Token: 0x04002455 RID: 9301
		public float Detect_Radius_Multiplier;

		// Token: 0x04002456 RID: 9302
		public float Ray_Aggressor_Distance;

		/// <summary>
		/// [0, 1] percentage of skill levels to retain after death.
		/// </summary>
		// Token: 0x04002457 RID: 9303
		public float Lose_Skills_PvP;

		/// <summary>
		/// [0, 1] percentage of skill levels to retain after death.
		/// </summary>
		// Token: 0x04002458 RID: 9304
		public float Lose_Skills_PvE;

		/// <summary>
		/// Number of skill levels to remove after death.
		/// </summary>
		// Token: 0x04002459 RID: 9305
		public uint Lose_Skill_Levels_PvP;

		/// <summary>
		/// Number of skill levels to remove after death.
		/// </summary>
		// Token: 0x0400245A RID: 9306
		public uint Lose_Skill_Levels_PvE;

		/// <summary>
		/// [0, 1] percentage of experience points to retain after death.
		/// </summary>
		// Token: 0x0400245B RID: 9307
		public float Lose_Experience_PvP;

		/// <summary>
		/// [0, 1] percentage of experience points to retain after death.
		/// </summary>
		// Token: 0x0400245C RID: 9308
		public float Lose_Experience_PvE;

		// Token: 0x0400245D RID: 9309
		public float Lose_Items_PvP;

		// Token: 0x0400245E RID: 9310
		public float Lose_Items_PvE;

		// Token: 0x0400245F RID: 9311
		public bool Lose_Clothes_PvP;

		// Token: 0x04002460 RID: 9312
		public bool Lose_Clothes_PvE;

		// Token: 0x04002461 RID: 9313
		public bool Lose_Weapons_PvP;

		// Token: 0x04002462 RID: 9314
		public bool Lose_Weapons_PvE;

		// Token: 0x04002463 RID: 9315
		public bool Can_Hurt_Legs;

		// Token: 0x04002464 RID: 9316
		public bool Can_Break_Legs;

		// Token: 0x04002465 RID: 9317
		public bool Can_Fix_Legs;

		// Token: 0x04002466 RID: 9318
		public bool Can_Start_Bleeding;

		// Token: 0x04002467 RID: 9319
		public bool Can_Stop_Bleeding;

		// Token: 0x04002468 RID: 9320
		public bool Spawn_With_Max_Skills;

		// Token: 0x04002469 RID: 9321
		public bool Spawn_With_Stamina_Skills;

		// Token: 0x0400246A RID: 9322
		public bool Allow_Instakill_Headshots;

		/// <summary>
		/// Should each character slot have separate savedata?
		/// </summary>
		// Token: 0x0400246B RID: 9323
		public bool Allow_Per_Character_Saves;

		/// <summary>
		/// If true, players will be kicked if their skin color is too similar to one of the level's terrain colors.
		/// </summary>
		// Token: 0x0400246C RID: 9324
		public bool Enable_Terrain_Color_Kick = true;
	}
}
