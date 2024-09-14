using System;

namespace SDG.Unturned
{
	// Token: 0x020006DC RID: 1756
	public class VehiclesConfigData
	{
		// Token: 0x06003AF5 RID: 15093 RVA: 0x0011359C File Offset: 0x0011179C
		public VehiclesConfigData(EGameMode mode)
		{
			this.Decay_Time = 604800f;
			this.Decay_Damage_Per_Second = 0.1f;
			this.Has_Battery_Chance = 0.8f;
			this.Min_Battery_Charge = 0.5f;
			this.Max_Battery_Charge = 0.75f;
			switch (mode)
			{
			case EGameMode.EASY:
				this.Has_Battery_Chance = 1f;
				this.Min_Battery_Charge = 0.8f;
				this.Max_Battery_Charge = 1f;
				this.Has_Tire_Chance = 1f;
				break;
			case EGameMode.NORMAL:
				this.Has_Battery_Chance = 0.8f;
				this.Min_Battery_Charge = 0.5f;
				this.Max_Battery_Charge = 0.75f;
				this.Has_Tire_Chance = 0.85f;
				break;
			case EGameMode.HARD:
				this.Has_Battery_Chance = 0.25f;
				this.Min_Battery_Charge = 0.1f;
				this.Max_Battery_Charge = 0.3f;
				this.Has_Tire_Chance = 0.7f;
				break;
			default:
				this.Has_Battery_Chance = 1f;
				this.Min_Battery_Charge = 1f;
				this.Max_Battery_Charge = 1f;
				this.Has_Tire_Chance = 1f;
				break;
			}
			this.Respawn_Time = 300f;
			this.Unlocked_After_Seconds_In_Safezone = 3600f;
			this.Armor_Multiplier = 1f;
			this.Child_Explosion_Armor_Multiplier = 1f;
			this.Gun_Lowcal_Damage_Multiplier = 1f;
			this.Gun_Highcal_Damage_Multiplier = 1f;
			this.Melee_Damage_Multiplier = 1f;
			this.Melee_Repair_Multiplier = 1f;
			this.Max_Instances_Tiny = 4U;
			this.Max_Instances_Small = 8U;
			this.Max_Instances_Medium = 16U;
			this.Max_Instances_Large = 32U;
			this.Max_Instances_Insane = 64U;
		}

		/// <summary>
		/// Seconds vehicle can be neglected before it begins taking damage.
		/// </summary>
		// Token: 0x040023EB RID: 9195
		public float Decay_Time;

		/// <summary>
		/// After vehicle has been neglected for more than Decay_Time seconds it will begin taking this much damage per second.
		/// </summary>
		// Token: 0x040023EC RID: 9196
		public float Decay_Damage_Per_Second;

		// Token: 0x040023ED RID: 9197
		public float Has_Battery_Chance;

		// Token: 0x040023EE RID: 9198
		public float Min_Battery_Charge;

		// Token: 0x040023EF RID: 9199
		public float Max_Battery_Charge;

		// Token: 0x040023F0 RID: 9200
		public float Has_Tire_Chance;

		// Token: 0x040023F1 RID: 9201
		public float Respawn_Time;

		// Token: 0x040023F2 RID: 9202
		public float Unlocked_After_Seconds_In_Safezone;

		// Token: 0x040023F3 RID: 9203
		public float Armor_Multiplier;

		// Token: 0x040023F4 RID: 9204
		public float Child_Explosion_Armor_Multiplier;

		// Token: 0x040023F5 RID: 9205
		public float Gun_Lowcal_Damage_Multiplier;

		// Token: 0x040023F6 RID: 9206
		public float Gun_Highcal_Damage_Multiplier;

		// Token: 0x040023F7 RID: 9207
		public float Melee_Damage_Multiplier;

		// Token: 0x040023F8 RID: 9208
		public float Melee_Repair_Multiplier;

		// Token: 0x040023F9 RID: 9209
		public uint Max_Instances_Tiny;

		// Token: 0x040023FA RID: 9210
		public uint Max_Instances_Small;

		// Token: 0x040023FB RID: 9211
		public uint Max_Instances_Medium;

		// Token: 0x040023FC RID: 9212
		public uint Max_Instances_Large;

		// Token: 0x040023FD RID: 9213
		public uint Max_Instances_Insane;
	}
}
