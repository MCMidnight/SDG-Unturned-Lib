using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000535 RID: 1333
	// (Invoke) Token: 0x060029D5 RID: 10709
	public delegate void DeployBarricadeRequestHandler(Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow);
}
