using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Tags how client expects server to use a raycast input.
	/// For example, client may think they fired a gun while server thinks they dequipped the gun,
	/// so tagging the input prevents the server from handling it as a punch instead.
	/// </summary>
	// Token: 0x02000622 RID: 1570
	public enum ERaycastInfoUsage
	{
		// Token: 0x04001D0B RID: 7435
		Punch,
		// Token: 0x04001D0C RID: 7436
		ConsumeableAid,
		// Token: 0x04001D0D RID: 7437
		Melee,
		// Token: 0x04001D0E RID: 7438
		Gun,
		// Token: 0x04001D0F RID: 7439
		Bayonet,
		// Token: 0x04001D10 RID: 7440
		Refill,
		// Token: 0x04001D11 RID: 7441
		Tire,
		// Token: 0x04001D12 RID: 7442
		Battery,
		// Token: 0x04001D13 RID: 7443
		Detonator,
		// Token: 0x04001D14 RID: 7444
		Carlockpick,
		// Token: 0x04001D15 RID: 7445
		Fuel,
		// Token: 0x04001D16 RID: 7446
		Carjack,
		// Token: 0x04001D17 RID: 7447
		Grower,
		// Token: 0x04001D18 RID: 7448
		ArrestStart,
		// Token: 0x04001D19 RID: 7449
		ArrestEnd,
		// Token: 0x04001D1A RID: 7450
		Paint
	}
}
