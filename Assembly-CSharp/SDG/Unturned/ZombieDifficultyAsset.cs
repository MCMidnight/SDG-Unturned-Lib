using System;

namespace SDG.Unturned
{
	// Token: 0x0200038C RID: 908
	public class ZombieDifficultyAsset : Asset
	{
		// Token: 0x06001C32 RID: 7218 RVA: 0x000649A0 File Offset: 0x00062BA0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (data.ContainsKey("Overrides_Spawn_Chance"))
			{
				this.Overrides_Spawn_Chance = data.ParseBool("Overrides_Spawn_Chance", false);
			}
			else
			{
				this.Overrides_Spawn_Chance = true;
			}
			this.Crawler_Chance = data.ParseFloat("Crawler_Chance", 0f);
			this.Sprinter_Chance = data.ParseFloat("Sprinter_Chance", 0f);
			this.Flanker_Chance = data.ParseFloat("Flanker_Chance", 0f);
			this.Burner_Chance = data.ParseFloat("Burner_Chance", 0f);
			this.Acid_Chance = data.ParseFloat("Acid_Chance", 0f);
			this.Boss_Electric_Chance = data.ParseFloat("Boss_Electric_Chance", 0f);
			this.Boss_Wind_Chance = data.ParseFloat("Boss_Wind_Chance", 0f);
			this.Boss_Fire_Chance = data.ParseFloat("Boss_Fire_Chance", 0f);
			this.Spirit_Chance = data.ParseFloat("Spirit_Chance", 0f);
			this.DL_Red_Volatile_Chance = data.ParseFloat("DL_Red_Volatile_Chance", 0f);
			this.DL_Blue_Volatile_Chance = data.ParseFloat("DL_Blue_Volatile_Chance", 0f);
			this.Boss_Elver_Stomper_Chance = data.ParseFloat("Boss_Elver_Stomper_Chance", 0f);
			this.Boss_Kuwait_Chance = data.ParseFloat("Boss_Kuwait_Chance", 0f);
			this.Mega_Stun_Threshold = data.ParseInt32("Mega_Stun_Threshold", 0);
			if (this.Mega_Stun_Threshold < 1)
			{
				this.Mega_Stun_Threshold = -1;
			}
			this.Normal_Stun_Threshold = data.ParseInt32("Normal_Stun_Threshold", 0);
			if (this.Normal_Stun_Threshold < 1)
			{
				this.Normal_Stun_Threshold = -1;
			}
			if (data.ContainsKey("Allow_Horde_Beacon"))
			{
				this.Allow_Horde_Beacon = data.ParseBool("Allow_Horde_Beacon", false);
				return;
			}
			this.Allow_Horde_Beacon = true;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x00064B67 File Offset: 0x00062D67
		protected virtual void construct()
		{
			this.Allow_Horde_Beacon = true;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00064B70 File Offset: 0x00062D70
		public ZombieDifficultyAsset()
		{
			this.construct();
		}

		// Token: 0x04000D4A RID: 3402
		public bool Overrides_Spawn_Chance;

		// Token: 0x04000D4B RID: 3403
		public float Crawler_Chance;

		// Token: 0x04000D4C RID: 3404
		public float Sprinter_Chance;

		// Token: 0x04000D4D RID: 3405
		public float Flanker_Chance;

		// Token: 0x04000D4E RID: 3406
		public float Burner_Chance;

		// Token: 0x04000D4F RID: 3407
		public float Acid_Chance;

		// Token: 0x04000D50 RID: 3408
		public float Boss_Electric_Chance;

		// Token: 0x04000D51 RID: 3409
		public float Boss_Wind_Chance;

		// Token: 0x04000D52 RID: 3410
		public float Boss_Fire_Chance;

		// Token: 0x04000D53 RID: 3411
		public float Spirit_Chance;

		// Token: 0x04000D54 RID: 3412
		public float DL_Red_Volatile_Chance;

		// Token: 0x04000D55 RID: 3413
		public float DL_Blue_Volatile_Chance;

		// Token: 0x04000D56 RID: 3414
		public float Boss_Elver_Stomper_Chance;

		// Token: 0x04000D57 RID: 3415
		public float Boss_Kuwait_Chance;

		// Token: 0x04000D58 RID: 3416
		public int Mega_Stun_Threshold;

		// Token: 0x04000D59 RID: 3417
		public int Normal_Stun_Threshold;

		/// <summary>
		/// Can horde beacons be placed in the associated bounds?
		/// </summary>
		// Token: 0x04000D5A RID: 3418
		public bool Allow_Horde_Beacon;
	}
}
