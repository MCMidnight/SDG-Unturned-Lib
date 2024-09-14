using System;

namespace SDG.Unturned
{
	// Token: 0x020006E6 RID: 1766
	public class GameplayConfigData
	{
		// Token: 0x06003B02 RID: 15106 RVA: 0x00114008 File Offset: 0x00112208
		public GameplayConfigData(EGameMode mode)
		{
			this.Repair_Level_Max = 3U;
			if (mode == EGameMode.HARD)
			{
				this.Hitmarkers = false;
				this.Crosshair = false;
			}
			else
			{
				this.Hitmarkers = true;
				this.Crosshair = true;
			}
			if (mode == EGameMode.EASY)
			{
				this.Ballistics = false;
			}
			else
			{
				this.Ballistics = true;
			}
			this.Chart = (mode == EGameMode.EASY);
			this.Satellite = false;
			this.Compass = false;
			this.Group_Map = (mode != EGameMode.HARD);
			this.Group_HUD = true;
			this.Group_Player_List = true;
			this.Allow_Static_Groups = true;
			this.Allow_Dynamic_Groups = true;
			this.Allow_Lobby_Groups = true;
			this.Allow_Shoulder_Camera = true;
			this.Can_Suicide = true;
			this.Friendly_Fire = false;
			this.Bypass_Buildable_Mobility = false;
			this.Timer_Exit = 10U;
			this.Timer_Respawn = 10U;
			this.Timer_Home = 30U;
			this.Timer_Leave_Group = 30U;
			this.Max_Group_Members = 0U;
			this.Allow_Freeform_Buildables = true;
			this.Allow_Freeform_Buildables_On_Vehicles = true;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x0011412E File Offset: 0x0011232E
		public void InitSingleplayerDefaults()
		{
			this.Bypass_Buildable_Mobility = true;
		}

		// Token: 0x04002499 RID: 9369
		public uint Repair_Level_Max;

		// Token: 0x0400249A RID: 9370
		public bool Hitmarkers;

		// Token: 0x0400249B RID: 9371
		public bool Crosshair;

		// Token: 0x0400249C RID: 9372
		public bool Ballistics;

		// Token: 0x0400249D RID: 9373
		public bool Chart;

		// Token: 0x0400249E RID: 9374
		public bool Satellite;

		// Token: 0x0400249F RID: 9375
		public bool Compass;

		// Token: 0x040024A0 RID: 9376
		public bool Group_Map;

		// Token: 0x040024A1 RID: 9377
		public bool Group_HUD;

		/// <summary>
		/// Should group connections be shown on player list?
		/// </summary>
		// Token: 0x040024A2 RID: 9378
		public bool Group_Player_List;

		// Token: 0x040024A3 RID: 9379
		public bool Allow_Static_Groups;

		// Token: 0x040024A4 RID: 9380
		public bool Allow_Dynamic_Groups;

		/// <summary>
		/// If true, allow automatically creating an in-game group for members of your Steam lobby.
		/// Requires Allow_Dynamic_Groups to be enabled as well.
		/// </summary>
		// Token: 0x040024A5 RID: 9381
		public bool Allow_Lobby_Groups;

		// Token: 0x040024A6 RID: 9382
		public bool Allow_Shoulder_Camera;

		// Token: 0x040024A7 RID: 9383
		public bool Can_Suicide;

		/// <summary>
		/// Is friendly-fire allowed?
		/// </summary>
		// Token: 0x040024A8 RID: 9384
		public bool Friendly_Fire;

		/// <summary>
		/// Are sentry guns and beds allowed on vehicles?
		/// </summary>
		// Token: 0x040024A9 RID: 9385
		public bool Bypass_Buildable_Mobility;

		/// <summary>
		/// Should holiday (Halloween and Christmas) content like NPC outfits and decorations be loaded?
		/// </summary>
		// Token: 0x040024AA RID: 9386
		public bool Allow_Holidays = true;

		/// <summary>
		/// Can "freeform" barricades be placed in the world?
		/// Defaults to true.
		/// </summary>
		// Token: 0x040024AB RID: 9387
		public bool Allow_Freeform_Buildables;

		/// <summary>
		/// Can "freeform" barricades be placed on vehicles?
		/// Defaults to true.
		/// </summary>
		// Token: 0x040024AC RID: 9388
		public bool Allow_Freeform_Buildables_On_Vehicles;

		// Token: 0x040024AD RID: 9389
		internal const uint MAX_TIMER_EXIT = 60U;

		// Token: 0x040024AE RID: 9390
		public uint Timer_Exit;

		// Token: 0x040024AF RID: 9391
		public uint Timer_Respawn;

		// Token: 0x040024B0 RID: 9392
		public uint Timer_Home;

		// Token: 0x040024B1 RID: 9393
		public uint Timer_Leave_Group;

		// Token: 0x040024B2 RID: 9394
		public uint Max_Group_Members;

		/// <summary>
		/// Scales velocity added to players by explosion knock-back.
		/// </summary>
		// Token: 0x040024B3 RID: 9395
		public float Explosion_Launch_Speed_Multiplier = 1f;

		/// <summary>
		/// Scales midair input change in player direction.
		/// </summary>
		// Token: 0x040024B4 RID: 9396
		public float AirStrafing_Acceleration_Multiplier = 1f;

		/// <summary>
		/// Scales midair decrease in speed while faster than max walk speed.
		/// </summary>
		// Token: 0x040024B5 RID: 9397
		public float AirStrafing_Deceleration_Multiplier = 1f;

		/// <summary>
		/// Scales magnitude of recoil while using third-person perspective.
		/// </summary>
		// Token: 0x040024B6 RID: 9398
		public float ThirdPerson_RecoilMultiplier = 2f;

		/// <summary>
		/// Scales magnitude of bullet inaccuracy while using third-person perspective.
		/// </summary>
		// Token: 0x040024B7 RID: 9399
		public float ThirdPerson_SpreadMultiplier = 2f;

		// Token: 0x040024B8 RID: 9400
		internal static CommandLineFlag _forceTrustClient = new CommandLineFlag(false, "-ForceTrustClient");
	}
}
