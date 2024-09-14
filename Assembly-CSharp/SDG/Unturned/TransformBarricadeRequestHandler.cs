using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200053D RID: 1341
	// (Invoke) Token: 0x060029F5 RID: 10741
	public delegate void TransformBarricadeRequestHandler(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow);
}
