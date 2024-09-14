using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000596 RID: 1430
	// (Invoke) Token: 0x06002DAF RID: 11695
	public delegate void DamageStructureRequestHandler(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin);
}
