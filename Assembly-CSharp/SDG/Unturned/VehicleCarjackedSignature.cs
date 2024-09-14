using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005A6 RID: 1446
	// (Invoke) Token: 0x06002E44 RID: 11844
	public delegate void VehicleCarjackedSignature(InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow, ref Vector3 force, ref Vector3 torque);
}
