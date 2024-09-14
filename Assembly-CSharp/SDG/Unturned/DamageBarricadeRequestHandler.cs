using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000537 RID: 1335
	// (Invoke) Token: 0x060029DD RID: 10717
	public delegate void DamageBarricadeRequestHandler(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin);
}
