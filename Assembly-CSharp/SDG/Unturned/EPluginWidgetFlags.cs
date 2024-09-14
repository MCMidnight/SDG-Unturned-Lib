using System;

namespace SDG.Unturned
{
	/// <summary>
	/// 32-bit mask granting server plugins additional control over custom UIs.
	/// Only replicated to owner.
	/// </summary>
	// Token: 0x02000601 RID: 1537
	[Flags]
	public enum EPluginWidgetFlags
	{
		// Token: 0x04001B72 RID: 7026
		None = 0,
		/// <summary>
		/// Enables cursor movement while not in a vanilla menu.
		/// </summary>
		// Token: 0x04001B73 RID: 7027
		Modal = 1,
		/// <summary>
		/// Disable background blur regardless of other UI state.
		/// </summary>
		// Token: 0x04001B74 RID: 7028
		NoBlur = 2,
		/// <summary>
		/// Enable background blur regardless of other UI state.
		/// Takes precedence over NoBlur.
		/// </summary>
		// Token: 0x04001B75 RID: 7029
		ForceBlur = 4,
		/// <summary>
		/// Enable title card while focusing a nearby player.
		/// </summary>
		// Token: 0x04001B76 RID: 7030
		ShowInteractWithEnemy = 8,
		/// <summary>
		/// Enable explanation and respawn buttons while dead.
		/// </summary>
		// Token: 0x04001B77 RID: 7031
		ShowDeathMenu = 16,
		/// <summary>
		/// Enable health meter in the HUD.
		/// </summary>
		// Token: 0x04001B78 RID: 7032
		ShowHealth = 32,
		/// <summary>
		/// Enable food meter in the HUD.
		/// </summary>
		// Token: 0x04001B79 RID: 7033
		ShowFood = 64,
		/// <summary>
		/// Enable water meter in the HUD.
		/// </summary>
		// Token: 0x04001B7A RID: 7034
		ShowWater = 128,
		/// <summary>
		/// Enable virus/radiation/infection meter in the HUD.
		/// </summary>
		// Token: 0x04001B7B RID: 7035
		ShowVirus = 256,
		/// <summary>
		/// Enable stamina meter in the HUD.
		/// </summary>
		// Token: 0x04001B7C RID: 7036
		ShowStamina = 512,
		/// <summary>
		/// Enable oxygen meter in the HUD.
		/// </summary>
		// Token: 0x04001B7D RID: 7037
		ShowOxygen = 1024,
		/// <summary>
		/// Enable icons for bleeding, broken bones, temperature, starving, dehydrating, infected, drowning, full moon,
		/// safezone, and arrested status.
		/// </summary>
		// Token: 0x04001B7E RID: 7038
		ShowStatusIcons = 2048,
		/// <summary>
		/// Enable UseableGun ammo and firemode in the HUD.
		/// </summary>
		// Token: 0x04001B7F RID: 7039
		ShowUseableGunStatus = 4096,
		/// <summary>
		/// Enable vehicle fuel, speed, health, battery charge, and locked status in the HUD.
		/// </summary>
		// Token: 0x04001B80 RID: 7040
		ShowVehicleStatus = 8192,
		/// <summary>
		/// Enable center dot when guns are not equipped.
		/// </summary>
		// Token: 0x04001B81 RID: 7041
		ShowCenterDot = 16384,
		/// <summary>
		/// Enable popup when in-game rep is increased/decreased.
		/// </summary>
		// Token: 0x04001B82 RID: 7042
		ShowReputationChangeNotification = 32768,
		// Token: 0x04001B83 RID: 7043
		ShowLifeMeters = 2016,
		/// <summary>
		/// Default flags set when player spawns.
		/// </summary>
		// Token: 0x04001B84 RID: 7044
		Default = 65528
	}
}
