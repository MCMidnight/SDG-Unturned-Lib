using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200058A RID: 1418
	// (Invoke) Token: 0x06002D57 RID: 11607
	public delegate void DamageResourceRequestHandler(CSteamID instigatorSteamID, Transform objectTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin);
}
