using System;

namespace SDG.Unturned
{
	// Token: 0x020006DB RID: 1755
	public class ItemsConfigData
	{
		// Token: 0x06003AF2 RID: 15090 RVA: 0x00113300 File Offset: 0x00111500
		public ItemsConfigData(EGameMode mode)
		{
			this.Despawn_Dropped_Time = 600f;
			this.Despawn_Natural_Time = 900f;
			switch (mode)
			{
			case EGameMode.EASY:
				this.Spawn_Chance = 0.35f;
				this.Respawn_Time = 50f;
				this.Quality_Full_Chance = 0.1f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 0.1f;
				this.Gun_Bullets_Multiplier = 1f;
				this.Magazine_Bullets_Full_Chance = 0.1f;
				this.Magazine_Bullets_Multiplier = 1f;
				this.Crate_Bullets_Full_Chance = 0.1f;
				this.Crate_Bullets_Multiplier = 1f;
				break;
			case EGameMode.NORMAL:
				this.Spawn_Chance = 0.35f;
				this.Respawn_Time = 100f;
				this.Quality_Full_Chance = 0.1f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 0.05f;
				this.Gun_Bullets_Multiplier = 0.25f;
				this.Magazine_Bullets_Full_Chance = 0.05f;
				this.Magazine_Bullets_Multiplier = 0.5f;
				this.Crate_Bullets_Full_Chance = 0.05f;
				this.Crate_Bullets_Multiplier = 1f;
				break;
			case EGameMode.HARD:
				this.Spawn_Chance = 0.15f;
				this.Respawn_Time = 150f;
				this.Quality_Full_Chance = 0.01f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 0.025f;
				this.Gun_Bullets_Multiplier = 0.1f;
				this.Magazine_Bullets_Full_Chance = 0.025f;
				this.Magazine_Bullets_Multiplier = 0.25f;
				this.Crate_Bullets_Full_Chance = 0.025f;
				this.Crate_Bullets_Multiplier = 0.75f;
				break;
			default:
				this.Spawn_Chance = 1f;
				this.Respawn_Time = 1000000f;
				this.Quality_Full_Chance = 1f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 1f;
				this.Gun_Bullets_Multiplier = 1f;
				this.Magazine_Bullets_Full_Chance = 1f;
				this.Magazine_Bullets_Multiplier = 1f;
				this.Crate_Bullets_Full_Chance = 1f;
				this.Crate_Bullets_Multiplier = 1f;
				break;
			}
			if (mode == EGameMode.EASY)
			{
				this.Has_Durability = false;
				this.Food_Spawns_At_Full_Quality = true;
				this.Water_Spawns_At_Full_Quality = true;
				this.Clothing_Spawns_At_Full_Quality = true;
				this.Weapons_Spawn_At_Full_Quality = true;
				this.Default_Spawns_At_Full_Quality = true;
				this.Clothing_Has_Durability = false;
				this.Weapons_Have_Durability = false;
				return;
			}
			this.Has_Durability = true;
			this.Food_Spawns_At_Full_Quality = false;
			this.Water_Spawns_At_Full_Quality = false;
			this.Clothing_Spawns_At_Full_Quality = false;
			this.Weapons_Spawn_At_Full_Quality = false;
			this.Default_Spawns_At_Full_Quality = false;
			this.Clothing_Has_Durability = true;
			this.Weapons_Have_Durability = true;
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x00113578 File Offset: 0x00111778
		internal bool ShouldClothingTakeDamage
		{
			get
			{
				return this.Has_Durability && this.Clothing_Has_Durability;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x0011358A File Offset: 0x0011178A
		internal bool ShouldWeaponTakeDamage
		{
			get
			{
				return this.Has_Durability && this.Weapons_Have_Durability;
			}
		}

		// Token: 0x040023D7 RID: 9175
		public float Spawn_Chance;

		// Token: 0x040023D8 RID: 9176
		public float Despawn_Dropped_Time;

		// Token: 0x040023D9 RID: 9177
		public float Despawn_Natural_Time;

		// Token: 0x040023DA RID: 9178
		public float Respawn_Time;

		// Token: 0x040023DB RID: 9179
		public float Quality_Full_Chance;

		// Token: 0x040023DC RID: 9180
		public float Quality_Multiplier;

		// Token: 0x040023DD RID: 9181
		public float Gun_Bullets_Full_Chance;

		// Token: 0x040023DE RID: 9182
		public float Gun_Bullets_Multiplier;

		// Token: 0x040023DF RID: 9183
		public float Magazine_Bullets_Full_Chance;

		// Token: 0x040023E0 RID: 9184
		public float Magazine_Bullets_Multiplier;

		// Token: 0x040023E1 RID: 9185
		public float Crate_Bullets_Full_Chance;

		// Token: 0x040023E2 RID: 9186
		public float Crate_Bullets_Multiplier;

		/// <summary>
		/// Original option for disabling item quality. Defaults to true. If false, items spawn at 100% quality and
		/// their quality doesn't decrease. For backwards compatibility, the newer per-item-type durability options
		/// are ignored if this is off.
		/// </summary>
		// Token: 0x040023E3 RID: 9187
		public bool Has_Durability;

		/// <summary>
		/// Food-specific replacement for <see cref="F:SDG.Unturned.ItemsConfigData.Has_Durability" />. Defaults to false. If true, food spawns at 100% quality.
		/// </summary>
		// Token: 0x040023E4 RID: 9188
		public bool Food_Spawns_At_Full_Quality;

		/// <summary>
		/// Water-specific replacement for <see cref="F:SDG.Unturned.ItemsConfigData.Has_Durability" />. Defaults to false. If true, water spawns at 100% quality.
		/// </summary>
		// Token: 0x040023E5 RID: 9189
		public bool Water_Spawns_At_Full_Quality;

		/// <summary>
		/// Clothing-specific replacement for <see cref="F:SDG.Unturned.ItemsConfigData.Has_Durability" />. Defaults to false. If true, clothing spawns at 100% quality.
		/// </summary>
		// Token: 0x040023E6 RID: 9190
		public bool Clothing_Spawns_At_Full_Quality;

		/// <summary>
		/// Weapon-specific replacement for <see cref="F:SDG.Unturned.ItemsConfigData.Has_Durability" />. Defaults to false. If true, weapons spawns at 100% quality.
		/// </summary>
		// Token: 0x040023E7 RID: 9191
		public bool Weapons_Spawn_At_Full_Quality;

		/// <summary>
		/// Fallback used when spawning an item that doesn't fit into one of the other quality/durability settings.
		/// Defaults to false. If true, items spawn at 100% quality.
		/// </summary>
		// Token: 0x040023E8 RID: 9192
		public bool Default_Spawns_At_Full_Quality;

		/// <summary>
		/// Clothing-specific replacement for <see cref="F:SDG.Unturned.ItemsConfigData.Has_Durability" />. Defaults to true. If false, clothing quality
		/// doesn't decrease when damaged.
		/// </summary>
		// Token: 0x040023E9 RID: 9193
		public bool Clothing_Has_Durability;

		/// <summary>
		/// Melee and gun replacement for <see cref="F:SDG.Unturned.ItemsConfigData.Has_Durability" />. Defaults to true. If false, weapons quality
		/// doesn't decrease when used.
		/// </summary>
		// Token: 0x040023EA RID: 9194
		public bool Weapons_Have_Durability;
	}
}
