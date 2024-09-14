using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020005A5 RID: 1445
	// (Invoke) Token: 0x06002E40 RID: 11840
	public delegate void DamageTireRequestHandler(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin);
}
