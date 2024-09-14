using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000597 RID: 1431
	// (Invoke) Token: 0x06002DB3 RID: 11699
	public delegate void RepairStructureRequestHandler(CSteamID instigatorSteamID, Transform structureTransform, ref float pendingTotalHealing, ref bool shouldAllow);
}
