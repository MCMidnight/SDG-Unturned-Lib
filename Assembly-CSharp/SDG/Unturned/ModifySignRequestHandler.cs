using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200053B RID: 1339
	// (Invoke) Token: 0x060029ED RID: 10733
	public delegate void ModifySignRequestHandler(CSteamID instigator, InteractableSign sign, ref string text, ref bool shouldAllow);
}
