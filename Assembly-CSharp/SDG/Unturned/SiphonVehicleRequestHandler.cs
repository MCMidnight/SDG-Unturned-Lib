using System;

namespace SDG.Unturned
{
	// Token: 0x020005A7 RID: 1447
	// (Invoke) Token: 0x06002E48 RID: 11848
	public delegate void SiphonVehicleRequestHandler(InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref ushort desiredAmount);
}
