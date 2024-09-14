using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000538 RID: 1336
	// (Invoke) Token: 0x060029E1 RID: 10721
	public delegate void RepairBarricadeRequestHandler(CSteamID instigatorSteamID, Transform barricadeTransform, ref float pendingTotalHealing, ref bool shouldAllow);
}
