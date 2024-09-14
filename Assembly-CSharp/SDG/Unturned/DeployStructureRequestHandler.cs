using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000594 RID: 1428
	// (Invoke) Token: 0x06002DA7 RID: 11687
	public delegate void DeployStructureRequestHandler(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow);
}
