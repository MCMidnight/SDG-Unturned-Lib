using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200074E RID: 1870
	// (Invoke) Token: 0x06003D20 RID: 15648
	public delegate void DamageToolPlayerDamagedHandler(Player player, ref EDeathCause cause, ref ELimb limb, ref CSteamID killer, ref Vector3 direction, ref float damage, ref float times, ref bool canDamage);
}
