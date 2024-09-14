using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200063B RID: 1595
	// (Invoke) Token: 0x06003394 RID: 13204
	public delegate void Hurt(Player player, byte damage, Vector3 force, EDeathCause cause, ELimb limb, CSteamID killer);
}
