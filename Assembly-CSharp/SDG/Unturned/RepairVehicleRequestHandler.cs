using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020005A4 RID: 1444
	// (Invoke) Token: 0x06002E3C RID: 11836
	public delegate void RepairVehicleRequestHandler(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalHealing, ref bool shouldAllow);
}
