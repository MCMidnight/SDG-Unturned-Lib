using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007EF RID: 2031
	// (Invoke) Token: 0x060045E7 RID: 17895
	public delegate void PaintVehicleRequestHandler(InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref Color32 desiredColor);
}
