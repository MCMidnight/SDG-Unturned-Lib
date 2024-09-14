using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020005A3 RID: 1443
	// (Invoke) Token: 0x06002E38 RID: 11832
	public delegate void DamageVehicleRequestHandler(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin);
}
