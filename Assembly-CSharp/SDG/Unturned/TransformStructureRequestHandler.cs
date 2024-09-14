using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200059A RID: 1434
	// (Invoke) Token: 0x06002DBF RID: 11711
	public delegate void TransformStructureRequestHandler(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow);
}
