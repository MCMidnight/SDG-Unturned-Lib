using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000580 RID: 1408
	// (Invoke) Token: 0x06002D07 RID: 11527
	public delegate void DamageObjectRequestHandler(CSteamID instigatorSteamID, Transform objectTransform, byte section, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin);
}
